//---------------------------------------------------------------------
// File: Batch.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: The Batch class is an abstraction over the Transport Proxy
// batch, it's main beneffit is to process the outcome of a given bacth
// and fire delegates letting a client handle different failured/successfull
// operations within a batch
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public class BatchMessage
	{
		private IBaseMessage		message;
		private object				userData;
		private string				correlationToken;
		private BatchOperationType	operationType;

		public BatchMessage (IBaseMessage message, object userData, BatchOperationType oppType)
		{
			this.message = message;
			this.userData = userData;
			this.operationType = oppType;
		}

		public BatchMessage (string correlationToken, object userData, BatchOperationType oppType)
		{
			this.correlationToken = correlationToken;
			this.userData = userData;
			this.operationType = oppType;
		}

		public IBaseMessage Message
		{
			get { return this.message; }
		}
		public object UserData
		{
			get { return this.userData; }
		}
		public string CorrelationToken
		{
			get { return this.correlationToken; }
		}
		public BatchOperationType OperationType
		{
			get { return this.operationType; }
		}
	}
	
	/// <summary>
    /// This class completely wraps the calls to a transportProxy batch. It does this so it can keep
    /// a trail of the messages that were submitted to that batch. As it has this it can then tie the
    /// async batch callback to a series of virtual function calls - all parameterized with the correct
    /// assocaited message object. Derived classes can then decide what they want to do in each case.
    /// </summary>
    public class Batch : IBTBatchCallBack
    {
        private Int32 hrStatus;

        public bool OverallSuccess
        {
            get { return (this.hrStatus >= 0); }    
        }

        // IBTBatchCallBack
        public void BatchComplete (Int32 hrStatus, Int16 nOpCount, BTBatchOperationStatus[] pOperationStatus, System.Object vCallbackCookie)
        {
			Trace.WriteLine(string.Format("Batch.BatchComplete( hrStatus:{0} ) called", hrStatus), "Base Adapter: Info" );
			
			this.hrStatus = hrStatus;

            StartBatchComplete(hrStatus);

            //  nothing at all failed in this batch so we are done
            if (hrStatus < 0 || this.verbose)
            {
                StartProcessOpperations();

                foreach (BTBatchOperationStatus status in pOperationStatus)
                {
                    if (status.Status >= 0 && !this.verbose)
                        continue;

                    switch (status.OperationType)
                    {
                        case BatchOperationType.Submit:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.submitArray[i];
                                if (status.MessageStatus[i] < 0)
                                    SubmitFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    SubmitSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.Delete:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.deleteArray[i];
                                if (status.MessageStatus[i] < 0)
                                    DeleteFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    DeleteSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.Resubmit:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.resubmitArray[i];
                                if (status.MessageStatus[i] < 0)
                                    ResubmitFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    ResubmitSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.MoveToSuspendQ:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.moveToSuspendQArray[i];
                                if (status.MessageStatus[i] < 0)
                                    MoveToSuspendQFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    MoveToSuspendQSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.MoveToNextTransport:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.moveToNextTransportArray[i];
                                if (status.MessageStatus[i] < 0)
                                    MoveToNextTransportFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    MoveToNextTransportSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.SubmitRequest:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.submitRequestArray[i];
                                if (status.MessageStatus[i] < 0)
                                    SubmitRequestFailure(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    SubmitRequestSuccess(batchMessage.Message, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                        case BatchOperationType.CancelRequestForResponse:
                        {
                            for (int i=0; i<status.MessageCount; i++)
                            {
                                BatchMessage batchMessage = (BatchMessage)this.cancelResponseMessageArray[i];
                                if (status.MessageStatus[i] < 0)
                                    CancelResponseMessageFailure(batchMessage.CorrelationToken, status.MessageStatus[i], batchMessage.UserData);
                                else if (this.verbose)
                                    CancelResponseMessageSuccess(batchMessage.CorrelationToken, status.MessageStatus[i], batchMessage.UserData);
                            }
                            break;
                        }
                    } // end switch
                } // end foreach

                EndProcessOpperations();
            } // end if

            EndBatchComplete(vCallbackCookie);
        }

		// Clients may set a delegate to receive the call back for a given event
		#region Delegates Declarations...

		public delegate void StartBatchCompleteEvent (Int32 hrStatus);
		public delegate void EndBatchCompleteEvent (object cookie);

		public delegate void StartProcessOpperationsEvent ();
		public delegate void EndProcessOpperationsEvent ();

		public delegate void SubmitFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);                    
		public delegate void DeleteFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);                    
		public delegate void ResubmitFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);                  
		public delegate void MoveToSuspendQFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);            
		public delegate void MoveToNextTransportFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);       
		public delegate void SubmitRequestFailureEvent (IBaseMessage message, Int32 hrStatus, object userData);             
		public delegate void CancelResponseMessageFailureEvent (string correlationToken, Int32 hrStatus, object userData);

		public delegate void SubmitSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);                     
		public delegate void DeleteSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);                     
		public delegate void ResubmitSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);                   
		public delegate void MoveToSuspendQSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);             
		public delegate void MoveToNextTransportSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);        
		public delegate void SubmitRequestSuccessEvent (IBaseMessage message, Int32 hrStatus, object userData);              
		public delegate void CancelResponseMessageSuccessEvent (string correlationToken, Int32 hrStatus, object userData); 

		#endregion // Delegates Declarations...

		#region Delegates Members...

		public /*private*/ StartBatchCompleteEvent _startBatchComplete;
		private EndBatchCompleteEvent _endBatchComplete;

		private StartProcessOpperationsEvent _startProcessOpperations;
		private EndProcessOpperationsEvent _endProcessOpperations;

		private SubmitFailureEvent _submitFailure;
		private DeleteFailureEvent _deleteFailure;
		private ResubmitFailureEvent _resubmitFailure;
		private MoveToSuspendQFailureEvent _moveToSuspendQFailure;
		private MoveToNextTransportFailureEvent _moveToNextTransportFailure;
		private SubmitRequestFailureEvent _submitRequestFailure;
		private CancelResponseMessageFailureEvent _cancelResponseMessageFailure;

		private SubmitSuccessEvent _submitSuccess;
		private DeleteSuccessEvent _deleteSuccess;
		private ResubmitSuccessEvent _resubmitSuccess;
		private MoveToSuspendQSuccessEvent _moveToSuspendQSuccess;
		private MoveToNextTransportSuccessEvent _moveToNextTransportSuccess;
		private SubmitRequestSuccessEvent _submitRequestSuccess;
		private CancelResponseMessageSuccessEvent _cancelResponseMessageSuccess;

		#endregion // Delegates Members...

		#region Delegates Accessor Properties...
		
		// Set properties
		public StartBatchCompleteEvent StartBatchCompleteCallBack
		{
			set { _startBatchComplete = value; }
		}
		public EndBatchCompleteEvent EndBatchCompleteCallBack
		{
			set { _endBatchComplete = value; }
		}
		public StartProcessOpperationsEvent StartProcessOpperationsCallBack
		{
			set { _startProcessOpperations = value; }
		}
		public EndProcessOpperationsEvent EndProcessOpperationsCallBack
		{
			set { _endProcessOpperations = value; }
		}
		public SubmitFailureEvent SubmitFailureCallBack
		{
			set { _submitFailure = value; }
		}
		public DeleteFailureEvent DeleteFailureCallBack
		{
			set { _deleteFailure = value; }
		}
		public ResubmitFailureEvent ResubmitFailureCallBack
		{
			set { _resubmitFailure = value; }
		}
		public MoveToSuspendQFailureEvent MoveToSuspendQFailureCallBack
		{
			set { _moveToSuspendQFailure = value; }
		}
		public MoveToNextTransportFailureEvent MoveToNextTransportFailureCallBack
		{
			set { _moveToNextTransportFailure = value; }
		}
		public SubmitRequestFailureEvent SubmitRequestFailureCallBack
		{
			set { _submitRequestFailure = value; }
		}
		public CancelResponseMessageFailureEvent CancelResponseMessageFailureCallBack
		{
			set { _cancelResponseMessageFailure = value; }
		}
		public SubmitSuccessEvent SubmitSuccessCallBack
		{
			set { _submitSuccess = value; }
		}
		public DeleteSuccessEvent DeleteSuccessCallBack
		{
			set { _deleteSuccess = value; }
		}
		public ResubmitSuccessEvent ResubmitSuccessCallBack
		{
			set { _resubmitSuccess = value; }
		}
		public MoveToSuspendQSuccessEvent MoveToSuspendQSuccessCallBack
		{
			set { _moveToSuspendQSuccess = value; }
		}
		public MoveToNextTransportSuccessEvent MoveToNextTransportSuccessCallBack
		{
			set { _moveToNextTransportSuccess = value; }
		}
		public SubmitRequestSuccessEvent SubmitRequestSuccessCallBack
		{
			set { _submitRequestSuccess = value; }
		}
		public CancelResponseMessageSuccessEvent CancelResponseMessageSuccessCallBack
		{
			set { _cancelResponseMessageSuccess = value; }
		}
	
		#endregion // Delegates Accessor Properties...

		#region BatchComplete Events

		protected virtual void StartBatchComplete (Int32 hrStatus)
		{
			if ( null != _startBatchComplete )
				_startBatchComplete(hrStatus);
		}
		protected virtual void EndBatchComplete (object cookie)
		{
			if ( null != _endBatchComplete )
				_endBatchComplete(cookie);
		}
		protected virtual void StartProcessOpperations ()
		{
			if ( null != _startProcessOpperations )
				_startProcessOpperations();
		}
		protected virtual void EndProcessOpperations ()
		{
			if ( null != _endProcessOpperations )
				_endProcessOpperations();
		}
		protected virtual void SubmitFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _submitFailure )
				_submitFailure(message, hrStatus, userData);
		}
		protected virtual void DeleteFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _deleteFailure )
				_deleteFailure(message, hrStatus, userData);
		}
		protected virtual void ResubmitFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _resubmitFailure )
				_resubmitFailure(message, hrStatus, userData);
		}
		protected virtual void MoveToSuspendQFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _moveToSuspendQFailure )
				_moveToSuspendQFailure(message, hrStatus, userData);
		}
		protected virtual void MoveToNextTransportFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _moveToNextTransportFailure )
				_moveToNextTransportFailure(message, hrStatus, userData);
		}
		protected virtual void SubmitRequestFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _submitRequestFailure )
				_submitRequestFailure(message, hrStatus, userData);
		}
		protected virtual void CancelResponseMessageFailure (string correlationToken, Int32 hrStatus, object userData)
		{
			if ( null != _cancelResponseMessageFailure )
				_cancelResponseMessageFailure(correlationToken, hrStatus, userData);
		}
		protected virtual void SubmitSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _submitSuccess )
				_submitSuccess(message, hrStatus, userData);
		}
		protected virtual void DeleteSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _deleteSuccess )
				_deleteSuccess(message, hrStatus, userData);
		}
		protected virtual void ResubmitSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _resubmitSuccess )
				_resubmitSuccess(message, hrStatus, userData);
		}
		protected virtual void MoveToSuspendQSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _moveToSuspendQSuccess )
				_moveToSuspendQSuccess(message, hrStatus, userData);
		}
		protected virtual void MoveToNextTransportSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _moveToNextTransportSuccess )
				_moveToNextTransportSuccess(message, hrStatus, userData);
		}
		protected virtual void SubmitRequestSuccess (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if ( null != _submitRequestSuccess )
				_submitRequestSuccess(message, hrStatus, userData);
		}
		protected virtual void CancelResponseMessageSuccess (string correlationToken, Int32 hrStatus, object userData)
		{
			if ( null != _cancelResponseMessageSuccess )
				_cancelResponseMessageSuccess(correlationToken, hrStatus, userData);
		}

		#endregion // BatchComplete Events

		public Batch (IBTTransportProxy transportProxy, object cookie, bool verbose)
        {
			//  initialize to FAIL
			this.hrStatus = -1;

            this.transportProxy = transportProxy;
            this.transportBatch = this.transportProxy.GetBatch(this, cookie);
            this.verbose = verbose;
        }

        public void SubmitMessage (IBaseMessage message, object userData)
        {
            this.transportBatch.SubmitMessage(message);
            if (null == this.submitArray)
                this.submitArray = new ArrayList();
            this.submitArray.Add(new BatchMessage(message, userData, BatchOperationType.Submit));
        }
        public void DeleteMessage (IBaseMessage message, object userData)                   
        { 
            this.transportBatch.DeleteMessage(message);
            if (null == this.deleteArray)
                this.deleteArray = new ArrayList();
            this.deleteArray.Add(new BatchMessage(message, userData, BatchOperationType.Delete));
        }
        public void Resubmit (IBaseMessage message, DateTime t, object userData)                
        {
            this.transportBatch.Resubmit(message, t);
            if (null == this.resubmitArray)
                this.resubmitArray = new ArrayList();
            this.resubmitArray.Add(new BatchMessage(message, userData, BatchOperationType.Resubmit));
        }
        public void MoveToSuspendQ (IBaseMessage message, object userData)           
        {
            this.transportBatch.MoveToSuspendQ(message);
            if (null == this.moveToSuspendQArray)
                this.moveToSuspendQArray = new ArrayList();
            this.moveToSuspendQArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToSuspendQ));
        }
        public void MoveToNextTransport (IBaseMessage message, object userData)      
        {
            this.transportBatch.MoveToNextTransport(message);
            if (null == this.moveToNextTransportArray)
                this.moveToNextTransportArray = new ArrayList();
            this.moveToNextTransportArray.Add(new BatchMessage(message, userData, BatchOperationType.MoveToNextTransport));
        }
        public void SubmitRequestMessage (IBaseMessage requestMsg, string correlationToken, bool firstResponseOnly, DateTime expirationTime, IBTTransmitter responseCallback, object userData)
        {
            this.transportBatch.SubmitRequestMessage(requestMsg, correlationToken, firstResponseOnly, expirationTime, responseCallback);
            if (null == this.submitRequestArray)
                this.submitRequestArray = new ArrayList();
            this.submitRequestArray.Add(new BatchMessage(requestMsg, userData, BatchOperationType.SubmitRequest));
        }
        public void CancelResponseMessage (string correlationToken, object userData) 
        {
            this.transportBatch.CancelResponseMessage(correlationToken);
            if (null == this.cancelResponseMessageArray)
                this.cancelResponseMessageArray = new ArrayList();
            this.cancelResponseMessageArray.Add(new BatchMessage(correlationToken, userData, BatchOperationType.CancelRequestForResponse));
        }
        public void SubmitResponseMessage (IBaseMessage solicitDocSent, IBaseMessage responseDocToSubmit, object userData) 
        {
            this.transportBatch.SubmitResponseMessage(solicitDocSent, responseDocToSubmit);
			if (null == this.submitArray)
				this.submitArray = new ArrayList();
			this.submitArray.Add(new BatchMessage(solicitDocSent, userData, BatchOperationType.Submit));
		}

        public void SubmitMessage (IBaseMessage message)
        {
            SubmitMessage(message, null);
        }
        public void DeleteMessage (IBaseMessage message)                  
        {
            DeleteMessage(message, null);
        }
        public void Resubmit (IBaseMessage message, DateTime t)
        {
            Resubmit(message, t, null);
        }
        public void MoveToSuspendQ (IBaseMessage message)
        {
            MoveToSuspendQ(message, null);
        }
        public void MoveToNextTransport (IBaseMessage message)
        {
            MoveToNextTransport(message, null);
        }
        public void SubmitRequestMessage (IBaseMessage requestMsg, string correlationToken, bool firstResponseOnly, DateTime expirationTime, IBTTransmitter responseCallback)
        {
            SubmitRequestMessage(requestMsg, correlationToken, firstResponseOnly, expirationTime, responseCallback, null);
        }
        public void CancelResponseMessage (string correlationToken)
        {
            CancelResponseMessage(correlationToken, null);
        }
        public void SubmitResponseMessage(IBaseMessage solicitDocSent, IBaseMessage responseDocToSubmit)
        {
            SubmitResponseMessage(solicitDocSent, responseDocToSubmit, null);
        }

        public IBTDTCCommitConfirm Done (object transaction)
        {
			// Note: BTS_E_MESSAGING_SHUTTING_DOWN can be returned 
			// from Done() and also be the failure status in 
			// BatchComplete().
            IBTDTCCommitConfirm dtccc = this.transportBatch.Done(transaction);
            
            //  Proactively release COM object
            if (Marshal.IsComObject(this.transportBatch))
            {
                Marshal.ReleaseComObject(this.transportBatch);
            }

			return dtccc;
        }

		public void Clear()
		{
			this.transportBatch.Clear();

			//  Proactively release COM object
			if (Marshal.IsComObject(this.transportBatch))
			{
				Marshal.ReleaseComObject(this.transportBatch);
			}
		}

        public IBTTransportProxy TransportProxy { get { return transportProxy; } }
        public IBTTransportBatch TransportBatch { get { return transportBatch; } }

        private IBTTransportProxy transportProxy;
        private IBTTransportBatch transportBatch;
        private bool verbose;

        private ArrayList submitArray;
        private ArrayList deleteArray;
        private ArrayList resubmitArray;
        private ArrayList moveToSuspendQArray;
        private ArrayList moveToNextTransportArray;
        private ArrayList submitRequestArray;
        private ArrayList cancelResponseMessageArray;
    }

    /// <summary>
    /// This class encapsulates the typical behavior we want in a Transmit Adapter running asynchronously.
    /// In summary our policy is:
    /// (1) on a resubmit failure Move to the next transport
    /// (2) on a move to next transport failure move to the suspend queue
    /// Otherwise:
    /// TODO: we should use SetErrorInfo on the transportProxy to log the error appropriately 
    /// </summary>
/*
	public class TypicalAsyncTransmitResponseBatch : Batch
    {
        public delegate void AllWorkDoneDelegate ();

        private AllWorkDoneDelegate allWorkDoneDelegate;

        public TypicalAsyncTransmitResponseBatch (IBTTransportProxy transportProxy, AllWorkDoneDelegate allWorkDoneDelegate) : base(transportProxy, false)
        {
            this.allWorkDoneDelegate = allWorkDoneDelegate;
        }
        protected override void StartProcessOpperations ()
        {
            this.batch = new TypicalAsyncTransmitResponseBatch(this.TransportProxy, this.allWorkDoneDelegate);
            this.allWorkDoneDelegate = null;
        }
        protected override void EndProcessOpperations ()
        {
            this.batch.Done(null);
        }
        protected override void EndBatchComplete (object cookie)
        {
            if (null != this.allWorkDoneDelegate)
                this.allWorkDoneDelegate();
        }
        protected override void ResubmitFailure (IBaseMessage message, Int32 hrStatus, object userData)
        {
            this.batch.MoveToNextTransport(message);
        }
        protected override void MoveToNextTransportFailure (IBaseMessage message, Int32 hrStatus, object userData)
        {
            this.batch.MoveToSuspendQ(message);
        }
        private Batch batch;
    }
	*/
}
