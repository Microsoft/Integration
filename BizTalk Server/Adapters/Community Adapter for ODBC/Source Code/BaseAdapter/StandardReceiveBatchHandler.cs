//---------------------------------------------------------------------
// File: StandardReceiveBatchHandler.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Batch helper class to to be used by a receive adapter,
// handles batch errors in the standard mannor
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
	/// This class handles the failure modes of a receivers batch, it performes the 
	/// recomended recovery policy for all the possible types of failures for a 
	/// receive batch.
	/// In summary our policy is:
	/// (1) on a submit failure we move the message to the suspend queue
	/// (2) on suspend failure we there is nothing we can do, so we pass the 
	///		error back to the client
	/// (3) register delegates to handle various events
	/// Otherwise:
	/// </summary>
	public class StandardReceiveBatchHandler
	{
		protected StandardReceiveBatch	batch				= null;
		protected IBTTransportProxy		transportProxy		= null;
		protected ArrayList				successfulMsgArray	= new ArrayList();
		protected ArrayList				failedMsgArray		= new ArrayList();
		protected BYOTTransaction		transaction			= null;
		protected bool					suspendOnError		= true;
		protected bool					blockOnDone			= false;
		protected AutoResetEvent		doneEvent			= new AutoResetEvent(false);
		protected uint					numberIterations	= 3;
		protected bool					batchSucceeded		= false;
		protected BatchAsyncResult		asyncResult			= null;
		protected AsyncCallback			asyncCb				= null;
		protected IBTDTCCommitConfirm	dtcCommitConfirm	= null;
		protected bool					autoCommitTx		= false;

		// Adapter client code may register the following types of delegates
		// to take action on these events...
		#region Client Delegates

		private AboutToSuspendMessageEvent			aboutToSuspendMsgCallBack;
		private MessageSubmittedSuccessfullyEvent	messageSubmittedSuccessfullyEvent;

		public delegate void AboutToSuspendMessageEvent (IBaseMessage message, object userData, out IBaseMessage msgToSuspend);
		public delegate void MessageSubmittedSuccessfullyEvent (IBaseMessage message, Int32 hrStatus, object userData);
		public delegate void BatchCompleteEvent(ArrayList successfulMsgArray, ArrayList failedMsgArray);

		public MessageSubmittedSuccessfullyEvent MessageSubmittedSuccessfullyCallBack
		{
			set { this.messageSubmittedSuccessfullyEvent = value; }
		}

		public AboutToSuspendMessageEvent AboutToSuspendMessageEventCallBack
		{
			set 
			{ 
				this.aboutToSuspendMsgCallBack = value;
				this.batch.AboutToSuspendMessageEventCallBack = this.aboutToSuspendMsgCallBack;
			}
		}

		#endregion // Client Delegates

		public StandardReceiveBatchHandler(IBTTransportProxy transportProxy, AboutToSuspendMessageEvent aboutToSuspendMessageCallback)
		{
			this.transportProxy = transportProxy;
			this.batch = new StandardReceiveBatch(this.transportProxy, new StandardReceiveBatch.BatchCompleteEvent(BatchComplete), this.suspendOnError);
		}

		public StandardReceiveBatchHandler(IBTTransportProxy transportProxy, AboutToSuspendMessageEvent aboutToSuspendMessageCallback, bool suspendOnError, uint numberIterations)
		{
			this.transportProxy = transportProxy;
			this.suspendOnError = suspendOnError;
			this.numberIterations = numberIterations;
			this.batch = new StandardReceiveBatch(this.transportProxy, new StandardReceiveBatch.BatchCompleteEvent(BatchComplete), this.suspendOnError);
		}

		public void SubmitMessage(IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardReceiveBatchHandler.SubmitMessage() called", "Base Adapter: Info" );

			this.batch.Batch.SubmitMessage(message, userData);
		}

		public void MoveToSuspendQ (IBaseMessage message, object userData)
		{
			Trace.WriteLine("StandardReceiveBatchHandler.MoveToSuspendQ() called", "Base Adapter: Info" );

			this.batch.Batch.MoveToSuspendQ(message, userData);
		}

		public BatchResult Done (BYOTTransaction transaction)
		{
			Trace.WriteLine("StandardReceiveBatchHandler.Done() called", "Base Adapter: Info" );

			IAsyncResult ar = BeginDone( transaction, null, null);
			return EndDone(ar);
		}

		public IAsyncResult BeginDone(BYOTTransaction transaction, AsyncCallback	cb, Object asyncState)
		{
			Trace.WriteLine("StandardReceiveBatchHandler.BeginDone() called", "Base Adapter: Info" );

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
			Trace.WriteLine("StandardReceiveBatchHandler.EndDone() called", "Base Adapter: Info" );

			this.doneEvent.WaitOne();

			return ((BatchAsyncResult)ar).BatchStatus;
		}

		public void BatchComplete(bool batchSucceeded, StandardReceiveBatch filteredBatch, ArrayList successArray, ArrayList failedArray, object cookie)
		{
			Trace.WriteLine(string.Format("StandardReceiveBatchHandler.BatchComplete( batchSucceeded:{0} ) called", batchSucceeded), "Base Adapter: Info" );

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
				else
				{
					// Call back the client to notify it of the outcome so it can clean up the data
					// received from the wire...
					this.successfulMsgArray = successArray;
					this.failedMsgArray = failedArray;

					if ( batchSucceeded && this.autoCommitTx && null != this.transaction )
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
	}

	public class StandardReceiveBatch
	{
		private Batch					batch;
		private bool					batchSucceeded		= true;
		private bool					suspendOnError		= true;
		private IBTTransportProxy		transportProxy;
		private BatchCompleteEvent		batchComplete;
		private ArrayList				successArray		= new ArrayList();
		private ArrayList				failedArray			= new ArrayList();
		private StandardReceiveBatch	filteredBatch;
		private StandardReceiveBatchHandler.AboutToSuspendMessageEvent aboutToSuspendMsgEvent;

		public delegate void BatchCompleteEvent(bool batchSucceeded, StandardReceiveBatch filteredBatch, ArrayList _successArray, ArrayList _failedArray, object cookie);

		public Batch Batch
		{
			set { this.batch = value; }
			get { return this.batch; }
		}

		public StandardReceiveBatchHandler.AboutToSuspendMessageEvent AboutToSuspendMessageEventCallBack
		{
			set { this.aboutToSuspendMsgEvent = value; }
		}

		public StandardReceiveBatch(IBTTransportProxy transportProxy, BatchCompleteEvent batchComplete, bool suspendOnError)
		{
			this.transportProxy = transportProxy;
			this.batch = new Batch(this.transportProxy, null, true);
			this.batchComplete = batchComplete;
			this.suspendOnError = suspendOnError;
			SetCallbacks();
		}

		private void SetCallbacks()
		{
			this.batch.StartBatchCompleteCallBack		= new Batch.StartBatchCompleteEvent(StartBatchComplete);
			this.batch.SubmitSuccessCallBack			= new Batch.SubmitSuccessEvent(SubmitSuccess);
			this.batch.SubmitFailureCallBack			= new Batch.SubmitFailureEvent(SubmitFailure);
			this.batch.MoveToSuspendQFailureCallBack	= new Batch.MoveToSuspendQFailureEvent(MoveToSuspendQFailure);
			this.batch.MoveToSuspendQSuccessCallBack	= new Batch.MoveToSuspendQSuccessEvent(MoveToSuspendQSuccess);
			this.batch.EndBatchCompleteCallBack			= new Batch.EndBatchCompleteEvent(EndBatchComplete);
		}

		public void StartBatchComplete(Int32 hrStatus)
		{
			if ( hrStatus < 0 )
			{
				this.batchSucceeded = false;
				this.filteredBatch = new StandardReceiveBatch( this.transportProxy, this.batchComplete, this.suspendOnError);
			}
		}

		public void SubmitSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.Submit));

			if ( !this.batchSucceeded )
				this.filteredBatch.Batch.SubmitMessage(message, userData);
		}

		public void SubmitFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( this.suspendOnError )
			{
				IBaseMessage msgToSuspend;

				if ( null != this.aboutToSuspendMsgEvent )
					this.aboutToSuspendMsgEvent(message, userData, out msgToSuspend);
				else
					msgToSuspend = message;

				this.filteredBatch.Batch.MoveToSuspendQ(msgToSuspend, userData);
				this.failedArray.Add(new BatchMessage(msgToSuspend, userData, BatchOperationType.Submit));
			}
			else
			{
				this.failedArray.Add(new BatchMessage(message, userData, BatchOperationType.Submit));
			}
		}

		public void MoveToSuspendQSuccess(IBaseMessage message, Int32 hrStatus, object userData)
		{
			this.successArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToSuspendQ));

			if ( !this.batchSucceeded )
			{
				IBaseMessage msgToSuspend;

				if ( null != this.aboutToSuspendMsgEvent )
					this.aboutToSuspendMsgEvent(message, userData, out msgToSuspend);
				else
					msgToSuspend = message;

				this.filteredBatch.Batch.MoveToSuspendQ(msgToSuspend, userData);
			}
		}

		public void MoveToSuspendQFailure(IBaseMessage message, Int32 hrStatus, object userData)
		{
			IBaseMessage msgToSuspend;

			if ( null != this.aboutToSuspendMsgEvent )
				this.aboutToSuspendMsgEvent(message, userData, out msgToSuspend);
			else
				msgToSuspend = message;

			this.filteredBatch.Batch.MoveToSuspendQ(msgToSuspend, userData);

			this.failedArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToSuspendQ));
		}
		
		public void EndBatchComplete(object cookie)
		{
			if ( null != this.batchComplete )
			{
				this.batchComplete( this.batchSucceeded, this.filteredBatch, this.successArray, this.failedArray, cookie );
			}
		}
	}
}

