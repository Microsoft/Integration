//---------------------------------------------------------------------
// File: BatchResult.cs
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
// Description: This class holds the outcome of a batch of work submitted 
// to a bacth handler class
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
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for BatchResult.
	/// </summary>
	public class BatchResult
	{
		private ArrayList			successArray		= new ArrayList();
		private ArrayList			failedArray			= new ArrayList();
		private bool				batchSucceeded		= false;
		private IBTDTCCommitConfirm dtcCommitConfirm	= null;
		private BYOTTransaction		transaction			= null;

		public BatchResult(bool batchSucceeded, IBTDTCCommitConfirm dtcCommitConfirm, BYOTTransaction transaction, ArrayList successArray, ArrayList failedArray)
		{
			this.successArray		= successArray;
			this.dtcCommitConfirm	= dtcCommitConfirm;
			this.transaction		= transaction;
			this.failedArray		= failedArray;
			this.batchSucceeded		= batchSucceeded;
		}

		public bool BatchSucceeded
		{
			get 
			{ 
				if ( null == this.transaction )
					return this.batchSucceeded; 
				else
				{
					// If we were doing transactions, we were only successful if we committed the transaction...
					if ( this.batchSucceeded && BYOTTransaction.TxState.Committed == this.transaction.TransactionState )
						return this.batchSucceeded;
					else
						return false;
				}
			}
		}

		public IBTDTCCommitConfirm DTCCommitConfirm
		{
			get { return this.dtcCommitConfirm; }
		}

		public ArrayList SuccessBatchMsgArray
		{
			get { return this.successArray; }
		}

		public ArrayList FailedBatchMsgArray
		{
			get { return this.failedArray; }
		}
	}
}
