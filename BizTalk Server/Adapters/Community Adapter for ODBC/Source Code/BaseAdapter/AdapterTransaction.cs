//---------------------------------------------------------------------
// File: IAdapterTransaction.cs
// 
// Summary: Utilities for MSMQC adapter. 
//
//---------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.EnterpriseServices;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	public interface IAdapterTransaction : IDisposable
	{
		void Commit();
		void Abort();
		bool IsDTCTransaction { get; }
		ITransaction Transaction { get; }
		Microsoft.BizTalk.TransportProxy.Interop.IBTDTCCommitConfirm DTCCommitConfirm { get; set; }
	}

	public class BYOTTransaction : IAdapterTransaction
	{
		public enum TxState { Pending, Aborted, Committed };
		private TxState currentState = TxState.Pending;

		private ITransaction tx = null;
		private Microsoft.BizTalk.TransportProxy.Interop.IBTDTCCommitConfirm commitConfirm = null;
		private static ITransactionDispenser txnDispenser = new ITransactionDispenser();
		private static IWrapByot byot = new IWrapByot();

		public BYOTTransaction()
		{
			// NOTE: The transaction isolation level is set to Serializable due to a W2K issue. 
			// For W2K3 ReadCommitted is suffiecient and will give better performance.
			this.tx = (ITransaction)txnDispenser.GetTransaction(null, 0, (int)System.Data.IsolationLevel.Serializable); 
		}

		public BYOTTransaction( ITransaction transaction )
		{
			this.tx = transaction;
		}

		public TxState TransactionState
		{
			get { return currentState; }
		}

		public object CreateInstance( System.Guid guid )
		{
			// Create instance will create a new serviced component with this transaction
			return byot.CreateInstance( this.tx, guid );
		}

		#region IAdapterTransaction Members

		public void Commit()
		{
			if ( TxState.Pending == this.currentState )
			{
				try
				{
					this.tx.Commit( 0, 1, 0 );
					this.currentState = TxState.Committed;
					this.commitConfirm.DTCCommitConfirm( this.tx, true );
				}
				catch( Exception e )
				{
					EventLog.WriteEntry( "MSMQC", "Commit failed. Exception:\n " + e.Message, EventLogEntryType.Error );
					this.currentState = TxState.Aborted;
					this.commitConfirm.DTCCommitConfirm( this.tx, false );
				}
			}
		}

		public void Abort()
		{
			if ( TxState.Pending == this.currentState )
			{
				try
				{
					BOID boid = new BOID();
					boid.rgb = null;
					this.tx.Abort( ref boid, 0, 0 );
				}
				finally
				{
					lock( this )
					{
						if ( null != this.commitConfirm )
						{
							this.commitConfirm.DTCCommitConfirm( this.tx, false );
						}
					}
				}

				this.currentState = TxState.Aborted;
			}
		}

		public bool IsDTCTransaction
		{
			get { return true; }
		}

		public ITransaction Transaction
		{
			get { return this.tx; }
		}

		public Microsoft.BizTalk.TransportProxy.Interop.IBTDTCCommitConfirm DTCCommitConfirm
		{
			get { return this.commitConfirm; }
			set { this.commitConfirm = value; }
		}

		#endregion

		public void Dispose()
		{
			this.tx = null;
		}

	}
}