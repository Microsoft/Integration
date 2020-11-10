//---------------------------------------------------------------------
// File: AsyncTransmitter.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Base implementation for a non-batch aware send adapter, the 
// messages are sent a-sync, meaning that they will be sent on the 
// adapters thread and not on the engines thread. This is the recomended 
// approach for performance reasons
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
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for Transmitter.
	/// </summary>
	public abstract class AsyncTransmitter : Adapter, IBTTransmitter
	{
		protected IThreadPool				threadPool;
		private ControlledTermination		control;

		protected AsyncTransmitter 
			( string name, string version, string description, string transportType,
			Guid clsid, string propertyNamespace )
			: base( name, version, description, transportType, clsid, propertyNamespace )
		{
			this.control = new ControlledTermination();
			InitializeThreadPool();
		}

		public IThreadPool ThreadPool
		{
			get { return threadPool; }
		}

		public abstract void EnQueueWorkItem(IBaseMessage msg);
		public abstract void InitializeThreadPool();

		// IBTTransmitter
		public bool TransmitMessage(IBaseMessage msg)
		{
			Trace.WriteLine("AsyncTransmitter.TransmitMessage() called","Base Adapter: Info" );

			EnQueueWorkItem(msg);

			// Note: returning false indicates to the messaging engine
			// that the adapter is taking responsibility for deleting the
			// message once sent as well as handling any failures while
			// transmitting. This is refered to as sending the message 
			// asynchronously since we are not using the messaging engines
			// thread.
			return false;
		}

		public override void Terminate ()
		{
			Trace.WriteLine("AsyncTransmitter.Terminate() called","Base Adapter: Info" );

			//  Block until we are done...
			this.control.Terminate();
			base.Terminate();

			if ( null != threadPool )
				threadPool.Stop();
			
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public bool Enter ()
		{
			return this.control.Enter();
		}

		public void Leave ()
		{
			this.control.Leave();
		}
	}
}
