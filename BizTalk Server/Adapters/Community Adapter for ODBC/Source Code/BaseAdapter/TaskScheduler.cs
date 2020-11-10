//---------------------------------------------------------------------
// File: TaskScheduler.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Simple task scheduler class, usefull for polling receive 
// adapters
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

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// A Task is a unit of scheduled work for the TaskScheduler.
	/// </summary>
	internal abstract class Task : IDisposable
	{
		protected string			uri;
		protected TimerCallback		onPerformTask;
		protected Timer				taskScheduler;
		protected int				dueTime;
		protected int				period;

		public Timer TaskScheduler
		{
			get { return this.taskScheduler; }
		}

		public void Dispose()
		{
			this.taskScheduler.Dispose();
		}

		public abstract void Renew();
	}

	internal class PeriodicTask : Task
	{
		public PeriodicTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			this.uri = uri;
			this.onPerformTask = onPerformTask;
			this.dueTime = interval * 1000;
			this.period = interval * 1000;
			this.taskScheduler = new Timer( this.onPerformTask, state, this.dueTime, this.period );
		}

		public override void Renew() {}
	}

	internal class SingularTask : Task
	{
		public SingularTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			this.uri = uri;
			this.onPerformTask = onPerformTask;
			this.dueTime = interval * 1000;
			this.taskScheduler = new Timer( this.onPerformTask, state, this.dueTime, Timeout.Infinite );
		}

		public override void Renew()
		{
			this.taskScheduler.Change( this.dueTime, Timeout.Infinite );
		}
	}

	/// <summary>
	/// The TaskScheduler manages Task to be performed by adapters. The
	/// Adapter may update or remove the Task after initially adding it.
	/// </summary>
	public class TaskScheduler
	{
		private Hashtable tasks;
		
		public TaskScheduler()
		{
			this.tasks = new Hashtable();
		}

		public void Stop()
		{
			lock(this.tasks)
			{
				for ( int c = 0; c < this.tasks.Count; c++ )
				{
					Task task = (Task)this.tasks[c];
					task.Dispose();
				}
				this.tasks.Clear();
			}
		}

		public void AddNewSingularTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			lock(this.tasks)
			{
				Task newTask = new SingularTask(uri, state, interval, onPerformTask);
				this.tasks.Add(uri, newTask);
			}
		}

		public void AddNewPeriodicTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			lock(this.tasks)
			{
				Task newTask = new PeriodicTask(uri, state, interval, onPerformTask);
				this.tasks.Add(uri, newTask);
			}
		}

		public void RemoveTask(string uri)
		{
			lock(this.tasks)
			{
				_RemoveTask(uri);
			}
		}

		public void UpdateSingularTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			lock(this.tasks)
			{
				_RemoveTask(uri);
				Task newTask = new SingularTask(uri, state, interval, onPerformTask);
				this.tasks.Add(uri, newTask);
			}
		}

		public void UpdatePeriodicTask(string uri, object state, int interval, TimerCallback onPerformTask)
		{
			lock(this.tasks)
			{
				_RemoveTask(uri);
				Task newTask = new PeriodicTask(uri, state, interval, onPerformTask);
				this.tasks.Add(uri, newTask);
			}
		}

		public void RenewTask( string uri )
		{
			lock( this.tasks )
			{
				Task task = (Task)this.tasks[uri];
				if ( null != task )
				{
					task.Renew();
				}
			}
		}

		private void _RemoveTask(string uri)
		{
			Task task = (Task)this.tasks[uri];
			if ( null != task )
			{
				this.tasks.Remove(uri);
				task.Dispose();
			}
		}
	}
}
