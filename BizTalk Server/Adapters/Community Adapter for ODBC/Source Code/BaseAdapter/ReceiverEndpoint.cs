//---------------------------------------------------------------------
// File: ReceiverEndpoint.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Abstractio class for a receiver's endpoint
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
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Base class for end point configurations - a home for contains various helper functions.
	/// </summary>
	public abstract class ReceiverEndpoint
	{
		public abstract void Initialize (string uri, IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag, IBTTransportProxy transportProxy, string transportType, string propertyNamespace, IManageEndpoints manageEndpoints);
		public abstract void Update (IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag);
		public virtual void Remove () { }
	}
}
