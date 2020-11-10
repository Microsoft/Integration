//---------------------------------------------------------------------
// File: TransmitterMessage.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: This class wraps a message to be transmitted, retrieving 
// the configuration form the message context
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
using System.Xml;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.XLANGs.BaseTypes;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for TransmitterMessage
	/// </summary>
	public class TransmitterMessage
	{
		private IBaseMessage						message;
		private ConfigProperties					properties;
		private string								propertyNamespace;
		private SystemMessageContext				context;
		private string								uri;
		private bool								portIsTwoWay				= false;
		private ConfigProperties.CreateProperties	createProperties;
		private static readonly PropertyBase IsSolicitResponseProperty			= new BTS.IsSolicitResponse();

		public virtual ConfigProperties CreateProperties() {return null;}

		public TransmitterMessage(IBaseMessage message, string propertyNamespace, ConfigProperties.CreateProperties createProperties)
		{
			this.message = message;
			this.propertyNamespace = propertyNamespace;

			this.context = new SystemMessageContext(this.message.Context);
			this.uri = this.context.OutboundTransportLocation;
			// NOTE: the system context property IsSolicitResponse indicates whether the
			// port is one way or two way...
			object obj = this.message.Context.Read(IsSolicitResponseProperty.Name.Name, IsSolicitResponseProperty.Name.Namespace);
			if ( null != obj )
				this.portIsTwoWay = (bool)obj;

			this.createProperties = createProperties;
		}

		public IBaseMessage Message 
		{ 
			set { this.message = value; }
			get { return this.message; }
		}

		public ConfigProperties Properties 
		{
			get 
			{ 
				if ( null == this.properties )
					LoadConfiguration();

				return this.properties; 
			}
		}

		public bool PortIsTwoWay
		{
			get { return this.portIsTwoWay; }
		}

		private void LoadConfiguration()
		{
			XmlDocument locationConfigDom = null;

			//  get the adapter configuration off the message
			string config = (string)this.message.Context.Read("AdapterConfig", this.propertyNamespace);

			//  the config can be null all that means is that we are doing a dynamic send
			if (null != config)
			{
				locationConfigDom = new XmlDocument();
				locationConfigDom.LoadXml(config);

				//  For Dynamic Sends the destination is taken from the outboundLocation URI
				this.properties = this.createProperties(this.uri);

				if (null != locationConfigDom)
				{
					//  For Dynamic Sends the Location config can be null
					//  Location properties - possibly override some handler properties
					this.properties.LocationConfiguration(locationConfigDom);
				}
			}
		}
	}
}
