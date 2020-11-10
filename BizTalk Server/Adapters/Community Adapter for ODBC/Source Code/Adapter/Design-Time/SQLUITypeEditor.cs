//---------------------------------------------------------------------
// File: SQLUITypeEditor.cs
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
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

using Microsoft.BizTalk.Component.Interop;

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    public class SqlUITypeEditor : System.Drawing.Design.UITypeEditor
    {
        private SchemaPicker _form;

        public SqlUITypeEditor( )
        {
            this._form = new SchemaPicker( );
        }

        [EnvironmentPermissionAttribute( SecurityAction.LinkDemand, Unrestricted = false )]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

        [EnvironmentPermissionAttribute( SecurityAction.LinkDemand, Unrestricted = false )]
        public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value )
        {
            string target = null;
            string root = null;
            string uriString = string.Empty;

            switch ( this._form.ShowDialog( ) )
            {
                case System.Windows.Forms.DialogResult.OK:
                    value = this._form.sqlScript;
                    root = this._form.rootElementName;
                    target = this._form.targetNamespace;

                    //WARNING: This could be a problem if the query gets over 256 characters!
                    uriString = "ODBC://" + value + "/";

                    // Writes to URI property
                    object o = context.Instance;
                    Type t = o.GetType( );
                    System.Reflection.PropertyInfo p = t.GetProperty( "RootName" );

                    p.SetValue( context.Instance, root, null );
                    p = t.GetProperty( "Namespace" );
                    p.SetValue( context.Instance, target, null );

                    p = t.GetProperty( "uri" );
                    p.SetValue( context.Instance, uriString, null );

                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    break;
            }

            return value;
        }
    }
}