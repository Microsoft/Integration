//---------------------------------------------------------------------
// File: NativeCalls.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
// Description: Native call wrappers
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
using System.Text;
using System.Runtime.InteropServices;
using SD = System.Diagnostics;
using Microsoft.BizTalk.Message.Interop;

namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
    //
    // Wrapper for a couple of native calls.
    //
    public class NativeCalls
    {

        [DllImport("kernel32.dll",EntryPoint="GetCurrentProcess")]
        private static extern IntPtr GetCurrentProcess_Native();

        public static IntPtr GetCurrentProcess()
        {
            return GetCurrentProcess_Native();
        }

        [DllImport("kernel32.dll",EntryPoint="GetProcessAffinityMask")]
        private static extern bool GetProcessAffinityMask_Native
        (
            IntPtr			hProcess,
            out uint		procMask,
            out uint		systemMask
        );

		[DllImport("kernel32.dll")]
		internal static extern UInt32 GetTempFileName
		(
			string			path,
			string			prefix,
			UInt32			unique,
			StringBuilder	name
		);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr CreateFile
		(
			string			name,
			UInt32			accessMode,
			UInt32			shareMode,
			IntPtr			security,
			UInt32			createMode,
			UInt32			flags,
			IntPtr			template
		);
		
		public static bool GetProcessAffinityMask
            (
            IntPtr			hProcess,
            out uint		procMask,
            out uint		systemMask
            )
        {
            return GetProcessAffinityMask_Native
            (
                hProcess,
                out procMask,
                out systemMask
            );
        }

        public static int GetNumberOfProcessors()
        {
            uint procMask, systemMask;

            IntPtr hProc = GetCurrentProcess();
            if (!GetProcessAffinityMask( hProc, out procMask, out systemMask))
                throw new Exception("GetProcessAffinityMask");

            int procs = 0;
            while (procMask != 0)
            {
                if ((1 & procMask) != 0)
                    procs++;
                procMask >>= 1;
            }
            return procs;
        }
    }
}
