//---------------------------------------------------------------------
// File: ControlledTermination.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: This class is used to keep count of work in flight, an
// adapter should not return from terminate if it has work outstanding
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
using System.Threading;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public class ControlledTermination
	{
		private AutoResetEvent	e				= new AutoResetEvent(true);
		private int				activityCount	= 0;
		private bool			terminate		= false;

		//  to be called at the start of the activity
		//  returns false if terminate has been called
		public bool Enter ()
		{
			Trace.WriteLine("ControlledTermination.Enter() called", "Base Adapter: Info" );

			try
			{
				lock (this)
				{
					if (true == this.terminate)
						return false;
				}
				this.e.Reset();
				lock (this)
				{
					this.activityCount++;
				}
				return true;
			}
			finally
			{
				Trace.WriteLine("ControlledTermination.Enter() leaving", "Base Adapter: Info" );
			}
		}

		//  to be called at the end of the activity
		public void Leave ()
		{
			Trace.WriteLine("ControlledTermination.Leave() called", "Base Adapter: Info" );

			lock (this)
			{
				this.activityCount--;

				if (this.activityCount == 0)
					this.e.Set();
			}

			Trace.WriteLine("ControlledTermination.Leave() leaving", "Base Adapter: Info" );
		}

		//  this method blocks waiting for any activity to complete
		public void Terminate ()
		{
			Trace.WriteLine("ControlledTermination.Terminate() called", "Base Adapter: Info" );
            
			bool result;

			lock (this)
			{
				this.terminate = true;
				result = (this.activityCount == 0);
			}
			if (!result)
			{
				this.e.WaitOne();
			}

			Trace.WriteLine("ControlledTermination.Terminate() leaving", "Base Adapter: Info" );
		}

		public bool TerminateCalled
		{
			get 
			{ 
				lock (this)
				{
					return this.terminate;
				}
			}
		}
	}
}