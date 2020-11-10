//---------------------------------------------------------------------
// File: AsyncTransmitterBatch.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Base implementation for a batch to be used by a batch 
// aware send adapter
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
using System.Xml;
using System.Diagnostics;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
    /// <summary>
    /// AsyncTransmitterBatch batch
    /// </summary>
    /// 
    public class AsyncTransmitterBatch : IThreadpoolWorkItem, IBTTransmitterBatch
    {
        private int							maxBatchSize;
        private string						propertyNamespace;
        protected IBTTransportProxy			transportProxy;
        protected AsyncBatchedTransmitter	asyncTransmitter;
		private ArrayList					messages;
		protected ConfigProperties.CreateProperties createProperties;

		public ArrayList Messages
		{
			get { return messages; }
		}

        public AsyncTransmitterBatch (int maxBatchSize, string propertyNamespace, IBTTransportProxy transportProxy, AsyncBatchedTransmitter asyncTransmitter)
        {
            this.maxBatchSize = maxBatchSize;
            this.propertyNamespace = propertyNamespace;
            this.transportProxy = transportProxy;
            this.asyncTransmitter = asyncTransmitter;
            this.createProperties = createProperties;
            this.messages = new ArrayList();
        }

        // IBTTransmitterBatch
        public object BeginBatch (out int maxBatchSize)
        {
			Trace.WriteLine(string.Format("AsyncTransmitter.BeginBatch( maxBatchSize:{0} ) called", this.maxBatchSize),"Base Adapter: Info" );
			
			maxBatchSize = this.maxBatchSize;
            return null;
        }

        // Just build a list of messages for this batch - return false means we are asynchronous
        public bool TransmitMessage (IBaseMessage message)
        {
			Trace.WriteLine("AsyncTransmitter.TransmitMessage() called", "Base Adapter: Info" );

			TransmitterMessage msg = new TransmitterMessage(message, propertyNamespace, createProperties);
            this.messages.Add(msg);
            return false;
        }

        public void Clear ()
        {
			Trace.WriteLine("AsyncTransmitter.Clear() called", "Base Adapter: Info" );
			
			this.messages.Clear();
        }

        public void Done (IBTDTCCommitConfirm commitConfirm)
        {
			Trace.WriteLine("AsyncTransmitter.Done() called", "Base Adapter: Info" );

			// this call blocks an EPM Terminate call while we still have work to complete
			this.asyncTransmitter.Enter();
		
			asyncTransmitter.ThreadPool.AddItem(this);
		}     

		public bool HandleFailedBatch()
		{
			// If this batch faield we need to resubmit for future transmission. Note, 
			// the StandardTransmitBatchHandler will do the right thing if there are not 
			// enough retries, no backup transports etc
			StandardTransmitBatchHandler btsBatch = new StandardTransmitBatchHandler( transportProxy, null );

			foreach( TransmitterMessage msg in Messages )
			{
				btsBatch.Resubmit( msg.Message, null );
			}

			BatchResult br = btsBatch.Done( null );
			return br.BatchSucceeded;
		}

		public virtual void ProcessWorkItem ()
		{
		}
    }
}