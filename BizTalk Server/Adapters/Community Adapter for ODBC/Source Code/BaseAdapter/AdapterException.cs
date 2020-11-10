//---------------------------------------------------------------------
// File: AdapterException.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Adapter exception class
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
using System.Runtime.Serialization;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public class AdapterException : ApplicationException
	{
		// Error messages...
		public static string Error_AlreadyInitialized = "The Adapter was initialize more than once.";
		public static string Error_NotInitialized = "Termiante was called on the uninitialized adapter.";
		public static string Error_EndPointCreation = "Unable to create endpoint location URI:{0}";
		public static string Error_NoAdapterConfig = "No adapter configuration XML was found on the configuration when one was expected.";
		public static string Error_NoSuchProperty = "The required property:{0} was not found on adapter configuration XML.";
		public static string Error_UnInitialized = "The Adapter has not been initialized.";
		public static string Error_EndPointAlreadyExists = "The endpoint:{0} already exists.";
		public static string Error_EndPointCreateFailed = "Unable to create the endpoint for location URI:{0}.";
		public static string Error_EndPointNotExists = "The endpoint {0} does not exist.";
		public static string Error_ErrorThresholdExceeded = "The error threshold has been exceeded, the receive location:{0} will be shutdown";

		public AdapterException () { }

		public AdapterException (string msg) : base(msg) { }

		public AdapterException (Exception inner) : base(String.Empty, inner) { }

		public AdapterException (string msg, Exception e) : base(msg, e) { }

		protected AdapterException (SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

