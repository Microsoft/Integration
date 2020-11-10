//---------------------------------------------------------------------
// File: BatchAsyncResult.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Implementation of IAsyncResult to be used in the batch
// handler classes
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
using System.Threading;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for BatchAsyncResult.
	/// </summary>
	public class BatchAsyncResult : IAsyncResult
	{
		private bool				isCompleted			= false;
		private AutoResetEvent		waitHandle			= new AutoResetEvent(false);
		private object				state				= null;
		private BatchResult			batchResult			= null;

		public BatchAsyncResult(object state)
		{
			this.state = state;
		}

		public BatchResult BatchStatus
		{
			get { return this.batchResult; }
			set { this.batchResult = value; }
		}

		#region IAsyncResult
		
		// IAsyncResult.IsCompleted
		public bool IsCompleted
		{
			get { return this.isCompleted; }
		}

		// IAsyncResult.AsyncWaitHandle
		public WaitHandle AsyncWaitHandle
		{
			get 
			{ 
				return this.waitHandle;
			}
		}

		// IAsyncResult.AsyncState
		public Object AsyncState
		{
			get { return this.state; }
		}

		// IAsyncResult.CompletedSynchronously
		public bool CompletedSynchronously
		{
			get { return false; }
		}

		#endregion // IAsyncResult

	}
}
