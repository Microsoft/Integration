//---------------------------------------------------------------------
// File: Adapter.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: This class is the root object for an adapter, it 
// implements IBTTransport, IBTTransportControl, IPersistPropertyBag
// which are required for all adapters, though IPersistPropertyBag is
// optional
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
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for Adapter.
	/// </summary>
	public abstract class Adapter :
		IBTTransport,
		IBTTransportControl,
		IPersistPropertyBag
	{
		//  core member data
		private string propertyNamespace;
		private IBTTransportProxy transportProxy;
		private IPropertyBag handlerPropertyBag;
		private bool initialized;

		//  member data for implementing IBTTransport
		private string name;
		private string version;
		private string description;
		private string transportType;
		private Guid   clsid;

		protected Adapter (
			string name,
			string version,
			string description,
			string transportType,
			Guid clsid,
			string propertyNamespace)
		{
			this.transportProxy     = null;
			this.handlerPropertyBag = null;
			this.initialized        = false;

			this.name               = name;
			this.version            = version;
			this.description        = description;
			this.transportType      = transportType;
			this.clsid              = clsid;

			this.propertyNamespace  = propertyNamespace;
		}

		protected string            PropertyNamespace  { get { return propertyNamespace; } }
		protected IBTTransportProxy TransportProxy     { get { return transportProxy; } }
		protected IPropertyBag      HandlerPropertyBag { get { return handlerPropertyBag; } }
		protected bool              Initialized        { get { return initialized; } }

		//  IBTTransport
		public string Name { get { return name; } }
		public string Version { get { return version; } }
		public string Description { get { return description; } }
		public string TransportType { get { return transportType; } }
		public Guid ClassID { get { return clsid; } }

		//  IBTransportControl
		public virtual void Initialize (IBTTransportProxy transportProxy)
		{
			//  this is a Singleton and this should only ever be called once
			if (this.initialized)
			{
				Trace.WriteLine("Adapter already initialized!","Base Adapter: Error" );
				throw new AdapterException(AdapterException.Error_AlreadyInitialized);
			}

			this.transportProxy = transportProxy;
			this.initialized = true;
		}
		public virtual void Terminate ()
		{
			// NOTE: The adapter should not return from Terminate
			// until it has completed all outstanding work items.
			// The messaging engine will kill the adapter once it 
			// returns from Terminate
			if (!this.initialized)
			{
				Trace.WriteLine("Adapter not initialized!","Base Adapter: Error" );
				throw new AdapterException(AdapterException.Error_NotInitialized);
			}
			
			this.transportProxy = null;
		}

		protected virtual void HandlerPropertyBagLoaded ()
		{
			// let any derived classes know the property bag has now been loaded
		}

		// IPersistPropertyBag
		public void GetClassID (out Guid classid) { classid = this.clsid; }
		public void InitNew () { }
		public void Load (IPropertyBag pb, int pErrorLog)
		{
			this.handlerPropertyBag = pb;
			HandlerPropertyBagLoaded();
		}
		public void Save (IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties) { }
	}
}
