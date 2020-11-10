//---------------------------------------------------------------------
// File: StandardRequestResponseHandler.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Batch helper class to to be used by a receive adapter 
// performing two-way receives, handles batch errors in the standard mannor.
// This class does not handle multiple request-responses in the same 
// batches, better performance would be possible if it did
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
	/// This class handles request-response message exchanges. It 
	/// is designed to only handle a single request-response pair
	/// </summary>
	public class StandardRequestResponseHandler : IBTTransmitter
	{
		private IBaseMessage		responseMsg				= null;
		private AutoResetEvent		requestCompleted		= new AutoResetEvent(false);
		private AutoResetEvent		responseReady			= new AutoResetEvent(false);
		private bool				requestSubmitted		= false;
		private bool				requestSuccessful		= false;
		private IBTTransportProxy	transportProxy			= null;
		private bool				isDirty					= false;

		public StandardRequestResponseHandler(IBTTransportProxy transportProxy)
		{
			this.transportProxy = transportProxy;
		}

		public void Initialize(IBTTransportProxy transportProxy)
		{
			// Stub implementation..
		}
		public void Terminate()
		{
			// Stub implementation..
		}

		public bool TransmitMessage(IBaseMessage msg)
		{
			Trace.WriteLine("StandardRequestResponseHandler.TransmitMessage() called", "Base Adapter: Info" );

			// Keep a reference to the response message...
			this.responseMsg = msg;
			this.responseReady.Set();

			// Return false to indicate we will delete the response message 
			// since we are doing an asynchronous transmission...
			return false;
		}

		public IBaseMessage GetResponseMessage()
		{
			Trace.WriteLine("StandardRequestResponseHandler.GetResponseMessage() called", "Base Adapter: Info" );

			// If the request was not submitted don't bother waiting...
			if ( false == this.requestSubmitted )
				return null;

			// Wait for the request batch callback...
			this.requestCompleted.WaitOne();

			// If the request was successfully submitted we need to wait
			// for the response message...
			if ( this.requestSuccessful )
			{
				this.responseReady.WaitOne();
				return this.responseMsg; 
			}
			// Otherwise if the request message failed there will not be a repsonse,
			// returning null indicates we failed
			else
				return null;
		}

		public void ResponseMessageComplete()
		{
			Trace.WriteLine("StandardRequestResponseHandler.ResponseMessageComplete() called", "Base Adapter: Info" );

			StandardTransmitBatchHandler th = new StandardTransmitBatchHandler(this.transportProxy, null);
			th.DeleteMessage(this.responseMsg, null);
			th.Done(null);
		}

		public void SubmitRequestMessage(IBaseMessage requestMsg, object userData)
		{
			Trace.WriteLine("StandardRequestResponseHandler.SubmitRequestMessage() called", "Base Adapter: Info" );

			// This implementation can only handle a single request-response pair. 
			// Ideally, this batch handler should deal with multiple request-response 
			// pairs for performance reasons
			if ( isDirty )
				throw new ApplicationException("StandardRequestResponseHandler only handles a single request-response pair!");

			isDirty = true;

			// Create a new Transport Proxy batch wrapper and initialize
			Batch batch = new Batch(this.transportProxy, null, false);
			batch.StartBatchCompleteCallBack = new Batch.StartBatchCompleteEvent(BatchComplete);

			string correlationToken = Guid.NewGuid().ToString();
			batch.SubmitRequestMessage(requestMsg, correlationToken, true, new DateTime(0), this, userData);
			batch.Done(null);
			this.requestSubmitted = true;
		}

		public void BatchComplete(Int32 hrStatus)
		{
			Trace.WriteLine(string.Format("StandardRequestResponseHandler.BatchComplete( hrStatus:{0} ) called", hrStatus), "Base Adapter: Info" );

			// If hrStatus is greater than or equal to zero, the request
			// message was successfully submitted/persisted to the BizTalk store
			if ( hrStatus >= 0 )
				this.requestSuccessful = true;

			this.requestCompleted.Set();
		}
	}
}

