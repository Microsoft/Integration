//---------------------------------------------------------------------
// File: BatchState.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0
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

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public class BatchState
	{
		private ArrayList submit;
		private ArrayList delete;
		private ArrayList resubmit;
		private ArrayList moveToSuspendQ;
		private ArrayList moveToNextTransport;
		private ArrayList submitRequest;
		private ArrayList cancelRequestForResponse;

		public ArrayList Submit 
		{
			get
			{
				if (null == submit)
					submit = new ArrayList();
				return submit;
			}
		}
		public ArrayList Delete
		{
			get
			{
				if (null == delete)
					delete = new ArrayList();
				return delete;
			}
		}
		public ArrayList Resubmit
		{
			get
			{
				if (null == resubmit)
					resubmit = new ArrayList();
				return resubmit;
			}
		}
		public ArrayList MoveToSuspendQ
		{
			get
			{
				if (null == moveToSuspendQ)
					moveToSuspendQ = new ArrayList();
				return moveToSuspendQ;
			}
		}
		public ArrayList MoveToNextTransport
		{
			get
			{
				if (null == moveToNextTransport)
					moveToNextTransport = new ArrayList();
				return moveToNextTransport;
			}
		}
		public ArrayList SubmitRequest
		{
			get
			{
				if (null == submitRequest)
					submitRequest = new ArrayList();
				return submitRequest;
			}
		}
		public ArrayList CancelRequestForResponse
		{
			get
			{
				if (null == cancelRequestForResponse)
					cancelRequestForResponse = new ArrayList();
				return cancelRequestForResponse;
			}
		}
	}
}