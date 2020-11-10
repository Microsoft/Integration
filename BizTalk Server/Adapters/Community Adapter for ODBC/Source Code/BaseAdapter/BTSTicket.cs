//---------------------------------------------------------------------
// File: BTSTicket.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
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
using System.Runtime.InteropServices;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	// only include those flags that apply to these interfaces
	// see ssoflags.idl

	internal enum BTSTicketFlags
	{
		SSO_FLAG_NONE										= 0x00000000,
		SSO_FLAG_REFRESH									= 0x00000001,
	};

	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("8DA848D0-E703-4043-9AF7-C569AC1F4507")]
	internal class BTSTicket
	{
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("54596C7F-D343-4F20-BF7A-0722C5DA1F7D")]
	[CoClass(typeof(BTSTicket))]
	internal interface IBTSTicket
	{
		string[] ValidateAndRedeemTicket(
			[In, MarshalAs(UnmanagedType.IUnknown)] object message,
			string applicationName,
			int flags,
			out string externalUserName);
	};
}
