//---------------------------------------------------------------------
// File: ConfigProperties.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Base implementation for an adapters configuration properties 
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
using System.Text;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for EndPoint.
	/// </summary>
	public class ConfigProperties
	{
		private string uri;
		public delegate ConfigProperties CreateProperties(string uri);

		public ConfigProperties(IBaseMessage msg)
		{
			SystemMessageContext smc = new SystemMessageContext(msg.Context);
			this.uri = smc.InboundTransportLocation;
		}

		public ConfigProperties(string uri)
		{
			this.uri = uri;
		}

		public virtual void LocationConfiguration (XmlDocument configDOM)
		{
		}

		public virtual void UpdateUriForDynamicSend()
		{
		}

		public string Uri 
		{ 
			set { this.uri = value; }
			get { return this.uri; }
		}


		// Various useful helper functions
		public static XmlDocument ExtractConfigDomImpl (IPropertyBag pConfig, bool required)
		{
			object obj = null;
			pConfig.Read("AdapterConfig", out obj, 0);
			if (!required && null == obj)
				return null;
			if (null == obj)
				throw new AdapterException(AdapterException.Error_NoAdapterConfig);

			XmlDocument configDom = new XmlDocument();

			string adapterConfig = (string)obj;
			configDom.LoadXml(adapterConfig);

			return configDom;
		}

		public static XmlDocument ExtractConfigDom (IPropertyBag pConfig)
		{
			return ExtractConfigDomImpl(pConfig, true);
		}

		public static XmlDocument IfExistsExtractConfigDom (IPropertyBag pConfig)
		{
			return ExtractConfigDomImpl(pConfig, false);
		}

		public static string ExtractImpl (XmlDocument document, string path, bool required)
		{
			XmlNode node = document.SelectSingleNode(path);
			if (!required && null == node)
				return String.Empty;
			if (null == node)
				throw new AdapterException(string.Format(AdapterException.Error_NoSuchProperty,path));
			return node.InnerText;
		}

		public static string Extract (XmlDocument document, string path)
		{
			return ExtractImpl(document, path, true);
		}
		
		public static string IfExistsExtract (XmlDocument document, string path)
		{
			return ExtractImpl(document, path, false);
		}

		public static int ExtractInt (XmlDocument document, string path)
		{
			string s = Extract(document, path);
			return int.Parse(s);
		}

		public static int IfExistsExtractInt (XmlDocument document, string path)
		{
			string s = IfExistsExtract(document, path);
			if (0 == s.Length)
				return 0;
			return int.Parse(s);
		}

		public static long ExtractLong (XmlDocument document, string path)
		{
			string s = Extract(document, path);
			return long.Parse(s);
		}

		public static long IfExistsExtractLong (XmlDocument document, string path)
		{
			string s = IfExistsExtract(document, path);
			if (0 == s.Length)
				return 0;
			return long.Parse(s);
		}

		public static bool ExtractBool (XmlDocument document, string path)
		{
			string s = Extract(document, path);
			return bool.Parse(s);
		}

		public static bool IfExistsExtractBool (XmlDocument document, string path)
		{
			string s = IfExistsExtract(document, path);
			if (0 == s.Length)
				return false;
			return bool.Parse(s);
		}

		public static string CreateFileName (IBaseMessage message, string uri)
		{
			string uriNew = ReplaceMessageID(message, uri);
         
			return uriNew;
		}

		private static string ReplaceMessageID (IBaseMessage message, string uri)
		{
			Guid msgId = message.MessageID;

			string res = uri.Replace("%MessageID%", msgId.ToString());
			if ( res != null )
				return res;
			else 
				return uri;
		}
	}
}

