//---------------------------------------------------------------------
// File: TransactionDispenser.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: This class wraps a component that houses BYOT in a COM+ 
// library application. There is a bug Win2K (as far as SP3) that BYOT 
// will AV if used outside of a COM+ application. The safest route until 
// a fix is created is to wrap a separate component that sits in COM+ 
// and will delegate the calls to BYOT, hence this component. 
// 
// The code also contains the transaction dispenser that wraps the 
// DtcGetTransactionDispenserEx call. There is currently no way in managed 
// code to create a new transaction, so must use interop to enable this 
// at this time.
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

	public sealed class TransactionMgmtIIDs
	{
		public const string CLSID_TransactionDisperserImpl = "30C1F639-A3A6-4E0A-879D-0B15020754C6";
		public const string IID_ITransactionDisperserImpl  = "B051713B-736D-408D-8E0C-9C04EDE5A16C";
		public const string CLSID_WrapByot = "273D684A-6250-4F41-8281-DECE6711BD2D";
		public const string IID_IWrapByot = "846E5A00-9660-4443-B402-64FF4C722095";
	}
    
	[ComImport]
	[Guid(TransactionMgmtIIDs.CLSID_WrapByot)]
	[ClassInterface(ClassInterfaceType.None)]
	public class WrapByot {};

	[ComImport()]
	[Guid(TransactionMgmtIIDs.IID_IWrapByot)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CoClass(typeof(WrapByot))]
	public interface IWrapByot
	{
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object txn, Guid type);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid(TransactionMgmtIIDs.IID_ITransactionDisperserImpl)]
	[CoClass(typeof(TransactionDispenser))]
	public interface ITransactionDispenser
	{
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetTransaction
			(   
			[MarshalAs(UnmanagedType.BStr)]
			string  transactionName,
			int timeOut,
			int isolationLevel
			);
	}
 
	[ComImport]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid(TransactionMgmtIIDs.CLSID_TransactionDisperserImpl)]
	public class TransactionDispenser
	{
	}
}
