//---------------------------------------------------------------------
// File: ThreadPool.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Threadpool, thin wrapper over the CLR thread pool
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
using System.Threading;
using System.Diagnostics;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public interface IThreadPool
	{
		void Initialize();
		void Stop();
		void AddItem(IThreadpoolWorkItem workItem);
	}

	/// <summary>
	/// Generic ThreadPool, work items must implement IThreadPool.
	/// </summary>
	public class ThreadPool : IThreadPool
	{
		public void Initialize()
		{
		}

		public void Stop()
		{
		}

		public void AddItem(IThreadpoolWorkItem workItem)
		{
			System.Threading.ThreadPool.QueueUserWorkItem( new WaitCallback(WorkerThreadThunk), workItem );
		}

		private void WorkerThreadThunk(object state)
		{
			IThreadpoolWorkItem workItem = (IThreadpoolWorkItem)state;
			workItem.ProcessWorkItem();
		}
	}
}
