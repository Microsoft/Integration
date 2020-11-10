//---------------------------------------------------------------------
// File: StandardTransmitBatchHandler.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Batch helper class to to be used by a send adapter, 
// handles batch errors in the standard mannor.
//
//---------------------------------------------------------------------
// This file is part of the Microsoft BizTalk Server 2004 SDK
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft BizTalk
// Server 2004 release and/or on-line documentation. See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// This class handles the failure modes of a transmitters batch, it performes the 
	/// recomended recovery policy for all the possible types of failures for a 
	/// receive batch.
	/// In summary our policy is:
	/// (1) on a submit failure we move the message to the suspend queue
	/// (2) on suspend failure we there is nothing we can do, so we pass the 
	///		error back to the client
	/// (3) register delegates to handle various events
	/// Otherwise:
	/// </summary>
	public class StandardTransmitBatchHandler
	{
		private StandardTransmitBatch	batch				= null;
		private IBTTransportProxy		transportProxy		= null;
		private ArrayList				successfulMsgArray	= new ArrayList();
		private ArrayList				failedMsgArray		= new ArrayList();
		private BYOTTransaction			transaction			= null;
		private SolicitResponseCache	solicitRespCache	= new SolicitResponseCache();
		protected uint					numberIterations	= 3;
		private AutoResetEvent			doneEvent			= new AutoResetEvent(false);
		private bool					batchSucceeded		= false;
		private BatchAsyncResult		asyncResult			= null;
		private AsyncCallback			asyncCb				= null;
		private IBTDTCCommitConfirm		dtcCommitConfirm	= null;
		private bool					autoCommitTx		= false;

		// Adapter client code may register the following types of delegates
		// to take action on these events...
		#region Client Delegates

		private MessageSubmittedSuccessfullyEvent _messageSubmittedSuccessfullyEvent;

		public delegate void AboutToSuspendMessageEvent (IBaseMessage message, object userData, out IBaseMessage msgToSuspend);
		public delegate void MessageSubmittedSuccessfullyEvent (IBaseMessage message, Int32 hrStatus, object userData);
		public delegate void BatchCompleteEvent(ArrayList _successfulMsgArray, ArrayList _failedMsgArray);

		public MessageSubmittedSuccessfullyEvent MessageSubmittedSuccessfullyCallBack
		{
			set { _messageSubmittedSuccessfullyEvent = value; }
		}

		#endregion // Client Delegates

		public StandardTransmitBatchHandler(IBTTransportProxy transportProxy, AboutToSuspendMessageEvent aboutToSuspendMessageCallback)
		{
			this.transportProxy = transportProxy;
			this.batch = new StandardTransmitBatch(this.transportProxy, new StandardTransmitBatch.BatchCompleteEvent(BatchComplete), aboutToSuspendMessageCallback, this.solicitRespCache);
       	}

		public StandardTransmitBatchHandler(IBTTransportProxy transportProxy, bool autoCommitTx)
		{
			this.transportProxy = transportProxy;
			this.autoCommitTx = autoCommitTx;
			this.batch = new StandardTransmitBatch(this.transportProxy, new StandardTransmitBatch.BatchCompleteEvent(BatchComplete), null, this.solicitRespCache);
		}

		public void MoveToSuspendQ(IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.MoveToSuspendQ() called", "Base Adapter: Info" );

			this.batch.Batch.MoveToSuspendQ(message, userData);
		}

		public void DeleteMessage(IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.DeleteMessage() called", "Base Adapter: Info" );

			this.batch.Batch.DeleteMessage (message, userData);
		}

		public void Resubmit(IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.Resubmit() called", "Base Adapter: Info" );

			ResubmitDetails rd = ResubmitDetails.GetResubmitDetails(message);

			if ( rd.RetryCount > 0 ) // If we have more retries available, we will retry
			{
				this.batch.Batch.Resubmit(message, rd.RedeliverAt, userData);
			}
			else // Otherwise we should move to the next transport
			{
				this.batch.Batch.MoveToNextTransport(message, userData);
			}
		}

		public void MoveToNextTransport(IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.MoveToNextTransport() called", "Base Adapter: Info" );

			this.batch.Batch.MoveToNextTransport(message, userData);
		}

		public void SubmitResponseMessage(IBaseMessage solicitMsgSent, IBaseMessage responseMsgToSubmit, object userData)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.SubmitResponseMessage() called", "Base Adapter: Info" );

			this.solicitRespCache.Register(solicitMsgSent, responseMsgToSubmit);
			this.batch.Batch.SubmitResponseMessage(solicitMsgSent, responseMsgToSubmit, userData);
		}

		public BatchResult Done (BYOTTransaction transaction)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.Done() called", "Base Adapter: Info" );

			IAsyncResult ar = BeginDone( transaction, null, null);
			return EndDone(ar);
		}

		public IAsyncResult BeginDone(BYOTTransaction transaction, AsyncCallback cb, Object asyncState)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.BeginDone() called", "Base Adapter: Info" );

			this.asyncResult = new BatchAsyncResult(asyncState);
			this.transaction = transaction;
			this.asyncCb = cb;
			this.dtcCommitConfirm = this.batch.Batch.Done( ((null != transaction) ? transaction.Transaction : null) );
			if ( null != this.transaction )
				this.transaction.DTCCommitConfirm = this.dtcCommitConfirm;

			return this.asyncResult;
		}
		
		public BatchResult EndDone(IAsyncResult ar)
		{
			Trace.WriteLine("StandardTransmitBatchHandler.EndDone() called", "Base Adapter: Info" );

			this.doneEvent.WaitOne();

			return ((BatchAsyncResult)ar).BatchStatus;
		}

		public void BatchComplete(bool batchSucceeded, StandardTransmitBatch filteredBatch, ArrayList successArray, ArrayList failedArray, object cookie)
		{
			Trace.WriteLine(string.Format("StandardTransmitBatchHandler.BatchComplete( batchSucceeded:{0} ) called", batchSucceeded), "Base Adapter: Info" );

			try
			{
				this.numberIterations--;
				this.batchSucceeded = batchSucceeded;

				// If the batch failed we need commit the filtered batch. 
				// Note: if we are using a DTC transaction we should not retry the batch 
				// since the transaction needs to be aborted inorder to undo any work done 
				// against the Message Box
				if ( !batchSucceeded && (null != filteredBatch) && (this.numberIterations > 0) && (null == this.dtcCommitConfirm) )
				{
					filteredBatch.Batch.Done(this.transaction);
					return;
				}
			
				// filteredBatch will be null if only suspend or delete message failed
				// in which case we're done
				if ( !batchSucceeded && (null == filteredBatch) )
				{
					this.batchSucceeded = batchSucceeded;
					this.successfulMsgArray = successArray;
				}
				else if ( batchSucceeded )
				{
					this.successfulMsgArray = successArray;
					this.failedMsgArray = failedArray;

					if ( this.autoCommitTx && null != this.transaction)
					{
						this.transaction.Commit();
					}
				}
			}
			finally
			{
				this.asyncResult.BatchStatus = new BatchResult(this.batchSucceeded, this.dtcCommitConfirm, (((null != this.transaction) && (this.autoCommitTx)) ? this.transaction : null), this.successfulMsgArray, this.failedMsgArray);

				if ( null != this.asyncCb )
					this.asyncCb(this.asyncResult);

				this.doneEvent.Set();
			}
		}

		private void UpdateMessageArrays(ArrayList successMsgArr, ArrayList failedMsgArr)
		{
			if ( null != successMsgArr )
			{
				for ( int c = 0; c < successMsgArr.Count; c++ )
					this.successfulMsgArray.Add(successMsgArr[c]);
			}

			if ( null != failedMsgArr )
			{
				for ( int c = 0; c < failedMsgArr.Count; c++ )
					this.failedMsgArray.Add(failedMsgArr[c]);
			}
		}
	}

	public class StandardTransmitBatch
	{
		private Batch						batch;
		private bool						batchSucceeded		= true;
		private IBTTransportProxy			transportProxy;
		private BatchCompleteEvent			batchComplete;
		private ArrayList					successArray		= new ArrayList();
		private ArrayList					failedArray			= new ArrayList();
		private StandardTransmitBatch		filteredBatch;
		private StandardTransmitBatchHandler.AboutToSuspendMessageEvent aboutToSuspendMessageEvent;
		private SolicitResponseCache		solicitRespCache;

		public delegate void BatchCompleteEvent(bool batchSucceeded, StandardTransmitBatch filteredBatch, ArrayList successArray, ArrayList failedArray, object cookie);

		public Batch Batch
		{
			set { this.batch = value; }
			get { return this.batch; }
		}

		public StandardTransmitBatch(IBTTransportProxy transportProxy, BatchCompleteEvent batchComplete, StandardTransmitBatchHandler.AboutToSuspendMessageEvent aboutToSuspendMessageEvent, SolicitResponseCache solicitRespCache)
		{
			this.transportProxy = transportProxy;
			this.batch = new Batch(this.transportProxy, null, true);
			this.batchComplete = batchComplete;
			this.aboutToSuspendMessageEvent = aboutToSuspendMessageEvent;
			this.solicitRespCache = solicitRespCache;
			SetCallbacks();
		}

		private void SetCallbacks()
		{
			// Set batch start delegate...
			this.batch.StartBatchCompleteCallBack = new Batch.StartBatchCompleteEvent(StartBatchComplete);

			// Set operation delegates...
			this.batch.SubmitSuccessCallBack			= new Batch.SubmitSuccessEvent(SubmitResponseSuccess);
			this.batch.SubmitFailureCallBack			= new Batch.SubmitFailureEvent(SubmitResponseFailure);
			this.batch.DeleteSuccessCallBack			= new Batch.DeleteSuccessEvent(DeleteSuccess);
			this.batch.DeleteFailureCallBack			= new Batch.DeleteFailureEvent(DeleteFailure);
			this.batch.MoveToNextTransportSuccessCallBack = new Batch.MoveToNextTransportSuccessEvent(MoveToNextTransportSuccess);
			this.batch.MoveToNextTransportFailureCallBack = new Batch.MoveToNextTransportFailureEvent(MoveToNextTransportFailure);
			this.batch.ResubmitSuccessCallBack			= new Batch.ResubmitSuccessEvent(ResubmitSuccess);
			this.batch.ResubmitFailureCallBack			= new Batch.ResubmitFailureEvent(ResubmitFailure);
			this.batch.MoveToSuspendQFailureCallBack	= new Batch.MoveToSuspendQFailureEvent(MoveToSuspendQFailure);
			this.batch.MoveToSuspendQSuccessCallBack	= new Batch.MoveToSuspendQSuccessEvent(MoveToSuspendQSuccess);

			// Set batch end delegate...
			this.batch.EndBatchCompleteCallBack			= new Batch.EndBatchCompleteEvent(EndBatchComplete);
		}

		public void StartBatchComplete(Int32 hrStatus)
		{
			if ( hrStatus < 0 )
			{
				this.batchSucceeded = false;
				this.filteredBatch = new StandardTransmitBatch( this.transportProxy, this.batchComplete, this.aboutToSuspendMessageEvent, this.solicitRespCache);
			}
		}

		public void SubmitResponseSuccess(IBaseMessage solicitMessage, Int32 hrStatus, object userData)
		{
			IBaseMessage responseMessage = this.solicitRespCache.GetResponseMessage(solicitMessage);

			this.successArray.Add(new BatchMessage(responseMessage, userData, BatchOperationType.Submit));

			if ( !this.batchSucceeded )
				this.filteredBatch.Batch.SubmitResponseMessage(solicitMessage, responseMessage, userData);
		}

		public void SubmitResponseFailure(IBaseMessage solicitMessage, Int32 hrStatus, object userData)
		{
			IBaseMessage responseMessage = this.solicitRespCache.GetResponseMessage(solicitMessage);
			IBaseMessage msgToSuspend;

			if ( null != this.aboutToSuspendMessageEvent )
				this.aboutToSuspendMessageEvent(responseMessage, userData, out msgToSuspend);
			else
				msgToSuspend = responseMessage;

			this.failedArray.Add(new BatchMessage(msgToSuspend, userData, BatchOperationType.Submit));
			this.filteredBatch.Batch.MoveToSuspendQ(msgToSuspend, userData);
		}

		public void MoveToSuspendQSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToSuspendQ));

			if ( !this.batchSucceeded )
			{
				IBaseMessage msgToSuspend;

				if ( null != this.aboutToSuspendMessageEvent )
					this.aboutToSuspendMessageEvent(message, userData, out msgToSuspend);
				else
					msgToSuspend = message;

				this.successArray.Add(new BatchMessage(msgToSuspend, userData, BatchOperationType.Submit));
				this.filteredBatch.Batch.MoveToSuspendQ(msgToSuspend, userData);
			}
		}

		public void MoveToSuspendQFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			// If delete message fails, there is nothing to do. This could result
			// in the message being sent twice, though for non-transacted adapters
			// there is no guarentee against that anyway
			this.failedArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToSuspendQ));
		}
		
		public void DeleteSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.Delete));

			if ( !this.batchSucceeded )
				this.filteredBatch.Batch.DeleteMessage(message, userData);
		}

		public void DeleteFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			// If delete message fails, there is nothing to do. This could result
			// in the message being sent twice, though for non-transacted adapters
			// there is no guarentee against that anyway
			this.failedArray.Add(new BatchMessage(message, userData, BatchOperationType.Delete));
		}
		
		public void ResubmitSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.Resubmit));

			if ( !this.batchSucceeded )
			{
				ResubmitDetails rd = ResubmitDetails.GetResubmitDetails(message);
				this.filteredBatch.Batch.Resubmit(message, rd.RedeliverAt, userData);
			}
		}

		public void ResubmitFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			// If resubmit fails, give the backup transport a shot
			this.filteredBatch.Batch.MoveToNextTransport(message, userData);
		}
		
		public void MoveToNextTransportSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToNextTransport));

			if ( !this.batchSucceeded )
				this.filteredBatch.Batch.MoveToNextTransport(message, userData);
		}

		public void MoveToNextTransportFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			// If the move the the backup transport fails, suspend the message
			MoveToSuspendQSuccess(message, hrStatus, userData);
		}
		
		public void EndBatchComplete(object cookie)
		{
			if ( null != this.batchComplete )
			{
				this.batchComplete( this.batchSucceeded, this.filteredBatch, this.successArray, this.failedArray, cookie );
			}
		}
	}

	/// <summary>
	/// Helper class the get the messages' resubmit details
	/// </summary>
	public class ResubmitDetails
	{
		private int			retryCount;
		private DateTime	redeliverAt;

		public ResubmitDetails() {}

		public int RetryCount
		{
			set { this.retryCount = value; }
			get { return this.retryCount; }
		}

		public DateTime RedeliverAt
		{
			set { this.redeliverAt = value; }
			get { return this.redeliverAt; }
		}

		public static ResubmitDetails GetResubmitDetails(IBaseMessage msg)
		{
			ResubmitDetails rd = new ResubmitDetails();
			
			SystemMessageContext context = new SystemMessageContext(msg.Context);

			rd.RedeliverAt = DateTime.Now.AddMinutes(context.RetryInterval);
			rd.RetryCount = context.RetryCount;

			return rd;
		}
	}

	public class SolicitResponseCache
	{
		private Hashtable cache = new Hashtable();

		public void Register(IBaseMessage solicitMessage, IBaseMessage responseMessage)
		{
			Guid id = solicitMessage.MessageID;
			this.cache.Add(id, responseMessage);
		}

		public IBaseMessage GetResponseMessage(IBaseMessage solicitMessage)
		{
			Guid id = solicitMessage.MessageID;
			IBaseMessage responseMessage = (IBaseMessage)this.cache[id];
			return responseMessage;
		}
	}
}

