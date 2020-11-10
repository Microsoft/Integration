//---------------------------------------------------------------------
// File: SSOResult.cs
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
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
    /// <summary>
    /// SSOResult
    /// </summary>
    /// 
    public class SSOResult
    {
        private string		userName;
        private string[]	result;

        public SSOResult (IBaseMessage message, string affiliateApplication)
        {
            // SSO Results
            string   userName = null;
            string[] result   = null;

            // Ask SSO to redeem the ticket
            // Redeem the sso ticket in the context with the affiliate application name
            IBTSTicket ticket = new IBTSTicket();
            // Validate and redeem the ticket
            result = ticket.ValidateAndRedeemTicket(
                message,
                affiliateApplication,
                0,
                out userName);

            this.userName = userName;
            this.result   = result;
        }

        public string UserName 
        {
            get { return userName; }
        }
        public string[] Result 
        {
            get { return result; }
        }
    }
}
