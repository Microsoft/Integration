//---------------------------------------------------------------------
// File: BTSExplorerHelper.cs
// 
// Summary: Implementation of an adapter framework sample adapter using the ODBC provider for ADO.NET. 
//
// Sample: Adapter framework runtime adapter.
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

using Microsoft.Win32;

using Microsoft.BizTalk.ExplorerOM;

namespace Microsoft.BizTalk.Adapters.ODBC.RunTime.ODBCTransmitAdapter
{
    /// <summary>
    /// This Class provides helper functions as relates to the the scehmas stored in biztalk assemblies
    /// </summary>
    public class BtsSchemaHelper
    {
        RegistryKey btsAdmin = Registry.LocalMachine.OpenSubKey( @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration" );
        string mgmtDBName;
        string mgmtDBServer;
        BtsCatalogExplorer ce;

        public BtsSchemaHelper( )
        {
            // Initialize ExplorerOM
            // NB: The ce.ConnectionString call can take several seconds to return, hence it is done
            // here and can be cached by caller for later use.

            mgmtDBName = ( string )btsAdmin.GetValue( "MgmtDBName" );
            mgmtDBServer = ( string )btsAdmin.GetValue( "MgmtDBServer" );
            ce = new BtsCatalogExplorer( );
            ce.ConnectionString = "Server=" + mgmtDBServer + ";Database=" + mgmtDBName + ";Integrated Security=SSPI";
        }

        public string GetSchema( string sAssemblyQualifiedName )
        //Input string containing namespace
        //Returns matching schema's XML or "" if no matches found
        {
            //NOTE: This is a fix to addess formatting inconsitencies between the context object and
            //the BizTalk Explorer
            sAssemblyQualifiedName = sAssemblyQualifiedName.Remove( sAssemblyQualifiedName.IndexOf( " ", 1 ), 1 );

            string strResult = string.Empty;

            System.Collections.ICollection schemas = ce.GetCollection( CollectionType.Schema );

            foreach ( IBtsSchema schema in schemas )
            {
                if ( schema.AssemblyQualifiedName == sAssemblyQualifiedName )
                {
                    strResult = schema.XmlContent;
                    break;
                }
            }
            return strResult;

        }
    }
}