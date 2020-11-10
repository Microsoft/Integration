//---------------------------------------------------------------------
// File: Receiver.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Base implementation for a receive adapter
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
using System.Diagnostics;
using System.Collections;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
    /// <summary>
    /// Abstract base class for receiver adapters. Provides stock implementations of
    /// core interfaces needed to comply with receiver adapter contract.
    /// (1) This class is actually a Singleton. That is there will only ever be one
    /// instance of it created however many locations of this type are actually defined.
    /// (2) Individual locations are identified by a URI, the derived class must provide
    /// the Type name and this class acts as a Factory using the .NET Activator
    /// (3) It is legal to have messages from different locations submitted in a single
    /// batch and this may be an important optimization. And this is fundamentally why
    /// the Receiver is a singleton.
    /// </summary>
    public abstract class Receiver : Adapter, IBTTransportConfig, IManageEndpoints
    {
        //  core member data
        private Hashtable	endpoints;
        private Type		endpointType;

        private ControlledTermination control;

        protected Receiver (
            string name,
            string version,
            string description,
            string transportType,
            Guid clsid,
            string propertyNamespace,
            Type endpointType)
        : base(
            name,
            version,
            description,
            transportType,
            clsid,
            propertyNamespace)
        {
            this.endpointType = endpointType;
            this.control = new ControlledTermination();
        }

        //  IBTTransportConfig
        public void AddReceiveEndpoint (string bstrURL, IPropertyBag pConfig, IPropertyBag pBizTalkConfig)
        {
			Trace.WriteLine(string.Format("Receiver.AddReceiveEndpoint( bstrURL:{0} ) called", bstrURL), "Base Adapter: Info" );
			
			try
            {
                if (!this.Initialized)
                    throw new AdapterException(AdapterException.Error_UnInitialized);

                if (this.endpoints.Contains(bstrURL))
                    throw new AdapterException(string.Format(AdapterException.Error_EndPointAlreadyExists,bstrURL));

                ReceiverEndpoint endpoint = (ReceiverEndpoint)Activator.CreateInstance(this.endpointType);

                if (null == endpoint)
                    throw new AdapterException(string.Format(AdapterException.Error_EndPointCreateFailed, bstrURL));

                endpoint.Initialize(bstrURL, pConfig, pBizTalkConfig, this.HandlerPropertyBag, this.TransportProxy, this.TransportType, this.PropertyNamespace, (IManageEndpoints)this);

                this.endpoints[bstrURL] = endpoint;
            }
            catch (AdapterException exception)
            {
				Trace.WriteLine(string.Format("Exception caught in ControlledTermination.AddReceiveEndpoint(): {0}", exception), "Base Adapter: Error" );
				
				this.TransportProxy.SetErrorInfo(exception);
                throw exception;
            }
        }

        public void UpdateEndpointConfig (string bstrURL, IPropertyBag pConfig, IPropertyBag pBizTalkConfig)
        {
			Trace.WriteLine(string.Format("Receiver.UpdateEndpointConfig( bstrURL:{0} ) called", bstrURL), "Base Adapter: Info" );
			
			if (!this.Initialized)
                throw new AdapterException(AdapterException.Error_UnInitialized);

            ReceiverEndpoint endpoint = (ReceiverEndpoint)this.endpoints[bstrURL];

            if (null == endpoint)
                throw new AdapterException(string.Format(AdapterException.Error_EndPointNotExists,bstrURL));

            //  delegate the update call to the endpoint instance itself
            endpoint.Update(pConfig, pBizTalkConfig, this.HandlerPropertyBag);
        }

        public void RemoveReceiveEndpoint (string bstrURL)
        {
			Trace.WriteLine(string.Format("Receiver.RemoveReceiveEndpoint( bstrURL:{0} ) called", bstrURL), "Base Adapter: Info" );
			
			if (!this.Initialized)
				throw new AdapterException(AdapterException.Error_UnInitialized);

            ReceiverEndpoint endpoint = (ReceiverEndpoint)this.endpoints[bstrURL];

            if (null == endpoint)
                return;

            this.Remove(bstrURL);
            endpoint.Remove();
        }

        //  IBTransportControl
        public override void Initialize (IBTTransportProxy transportProxy)
        {
			Trace.WriteLine("Receiver.Initialize() called", "Base Adapter: Info" );
			
			base.Initialize(transportProxy);
            this.endpoints = new Hashtable();
        }

        public override void Terminate ()
        {
			Trace.WriteLine("Receiver.Terminate() called", "Base Adapter: Info" );
			
			//  Block until we are done...
            this.control.Terminate();

            base.Terminate();
            foreach (ReceiverEndpoint endpoint in this.endpoints.Values)
                endpoint.Remove();

            this.endpoints.Clear();
            this.endpoints = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // IManageEndpoints
        public void Remove (string uri)
        {
            this.endpoints.Remove(uri);
        }

        public bool Enter ()
        {
            return this.control.Enter();
        }

        public void Leave ()
        {
            this.control.Leave();
        }

        public bool TerminateCalled
        { 
            get { return this.control.TerminateCalled; }
        }
    }
}
