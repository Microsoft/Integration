//---------------------------------------------------------------------
// File: SchemaPicker.cs
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

using Microsoft.Win32;

using Microsoft.BizTalk.ExplorerOM;
using Microsoft.BizTalk.Component.Interop;

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    /// <summary>
    /// Summary description for SchemaPicker.
    /// </summary>
    internal class SchemaPicker : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.ComboBox comboProject;
        private System.Windows.Forms.ComboBox comboSchema;

        private BtsCatalogExplorer ce;
        private ArrayList schemaNames = new ArrayList( );
        private System.Windows.Forms.Button buttonCancel;

        private string m_sqlScript = String.Empty;
        private string m_rootElementName = String.Empty;
        private string m_targetNamespace = String.Empty;

        public string sqlScript
        {
            get
            {
                return m_sqlScript;
            }
            set
            {
                m_sqlScript = value;
            }
        }

        public string targetNamespace
        {
            get
            {
                return m_targetNamespace;
            }
            set
            {
                m_targetNamespace = value;
            }
        }

        public string rootElementName
        {
            get
            {
                return m_rootElementName;
            }
            set
            {
                m_rootElementName = value;
            }
        }

        public SchemaPicker( )
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent( );

            //			this.label1.Text = ODBCResourceHandler.GetResourceString("SchemaPickerSelectLabel");
            //			this.label2.Text = ODBCResourceHandler.GetResourceString("SchemaPickerSelectSchemaLabel");
            //			this.buttonOK.Text = ODBCResourceHandler.GetResourceString("FormOKButton");
            //			this.label3.Text = ODBCResourceHandler.GetResourceString("SchemaPickerTitleLabel");
            //			this.buttonCancel.Text = ODBCResourceHandler.GetResourceString("FormCancelButton");
            //			this.Text = ODBCResourceHandler.GetResourceString("SchemaPickerTitle");

            RegistryKey bts30admin = Registry.LocalMachine.OpenSubKey( @"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration" );
            string mgmtDBName = ( string )bts30admin.GetValue( "MgmtDBName" );
            string mgmtDBServer = ( string )bts30admin.GetValue( "MgmtDBServer" );

            // Initialize ExplorerOM
            ce = new BtsCatalogExplorer( );
            ce.ConnectionString = "Server=" + mgmtDBServer + ";Database=" + mgmtDBName + ";Integrated Security=SSPI";

            // Get all the assembly names (project names)
            //comboProject.Items.Add(ODBCResourceHandler.GetResourceString("SchemaAllProjects"));
            ICollection assemblies = ce.GetCollection( CollectionType.Assembly );
            foreach ( IBtsAssembly assembly in assemblies )
            {
                comboProject.Items.Add( assembly.Name );
            }

            // Get all the schema names but don't show them yet
            ICollection schemas = ce.GetCollection( CollectionType.Schema );
            foreach ( IBtsSchema schema in schemas )
            {
                if ( SchemaType.Property != schema.Type && !schema.BtsAssembly.IsSystem )
                    schemaNames.Add( schema.AssemblyQualifiedName );
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( components != null )
                {
                    components.Dispose( );
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.label1 = new System.Windows.Forms.Label( );
            this.label2 = new System.Windows.Forms.Label( );
            this.comboProject = new System.Windows.Forms.ComboBox( );
            this.comboSchema = new System.Windows.Forms.ComboBox( );
            this.buttonOK = new System.Windows.Forms.Button( );
            this.label3 = new System.Windows.Forms.Label( );
            this.buttonCancel = new System.Windows.Forms.Button( );
            this.SuspendLayout( );
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 12, 52 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 100, 16 );
            this.label1.TabIndex = 10;
            this.label1.Text = "Assembly";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 12, 100 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 100, 16 );
            this.label2.TabIndex = 12;
            this.label2.Text = "Schema";
            // 
            // comboProject
            // 
            this.comboProject.AccessibleName = "comboProject";
            this.comboProject.Location = new System.Drawing.Point( 12, 72 );
            this.comboProject.Name = "comboProject";
            this.comboProject.Size = new System.Drawing.Size( 348, 21 );
            this.comboProject.TabIndex = 11;
            this.comboProject.SelectedIndexChanged += new System.EventHandler( this.comboProject_SelectedIndexChanged );
            // 
            // comboSchema
            // 
            this.comboSchema.AccessibleName = "comboSchema";
            this.comboSchema.Location = new System.Drawing.Point( 12, 120 );
            this.comboSchema.Name = "comboSchema";
            this.comboSchema.Size = new System.Drawing.Size( 348, 21 );
            this.comboSchema.TabIndex = 13;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point( 198, 156 );
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size( 76, 23 );
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler( this.buttonOK_Click );
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 12, 12 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 348, 32 );
            this.label3.TabIndex = 5;
            this.label3.Text = "Please select the BizTalk assembly and schema containing the database script to e" +
                "xecute";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point( 282, 156 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size( 76, 23 );
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            // 
            // SchemaPicker
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size( 370, 188 );
            this.Controls.Add( this.buttonCancel );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.buttonOK );
            this.Controls.Add( this.comboSchema );
            this.Controls.Add( this.comboProject );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SchemaPicker";
            this.Text = "Select Schema";
            this.ResumeLayout( false );

        }
        #endregion

        private void comboProject_SelectedIndexChanged( object sender, System.EventArgs e )
        {
            comboSchema.Items.Clear( );
            comboSchema.ResetText( );
            string strProjName = comboProject.SelectedItem.ToString( );

            // Show all the schemas
            if ( strProjName == "AllProjects" ) //ODBCResourceHandler.GetResourceString("SchemaAllProjects"))
                strProjName = "";

            // If the project name contains space, it will be converted to underscore in schema name
            strProjName = strProjName.Replace( " ", "_" );

            for ( int i = 0; i < schemaNames.Count; i++ )
                if ( schemaNames[ i ].ToString( ).IndexOf( strProjName ) >= 0 )
                    comboSchema.Items.Add( schemaNames[ i ].ToString( ) );
        }

        private void buttonOK_Click( object sender, System.EventArgs e )
        {
            if ( comboSchema.SelectedIndex < 0 )
            {
                MessageBox.Show( "Please select a schema before continuing", "ODBC Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            //Use the full schema name to get the content of the schema
            try
            {
                string assemblyQualifiedName = comboSchema.SelectedItem.ToString( );

                int index = assemblyQualifiedName.IndexOf( ',' );
                string docName = assemblyQualifiedName.Substring( 0, index );
                string assemblyName = assemblyQualifiedName.Substring( index + 1 );

                IDocumentSpec ds = new DocumentSpec( docName, assemblyName );

                //Get the first schema
                XmlSchemaCollection schemas = ds.GetSchemaCollection( );
                XmlSchemaCollectionEnumerator enumerator = schemas.GetEnumerator( );
                enumerator.MoveNext( );
                XmlSchema schema = enumerator.Current;
                sqlScript = GetFirstSQLStatement( schema );
                targetNamespace = GetTarget( schema );
                rootElementName = GetRoot( schema );

                if ( sqlScript == null )
                {
                    MessageBox.Show( "The selected schema does not contain a SQL script. Please select another", "ODBC Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                else
                {
                    // We don't support parameters as part of a receive process. How would someone
                    // populate the parameters on a receieve only port?

                    //WARNING: Need to fix this as they could be calling a proc with outparameters
                    //We'll skip the RC for a SP.
                    if ( sqlScript.IndexOf( "?", 3 ) > 0 )
                    {
                        MessageBox.Show( "The selected schema contain a SQL script input parameters. This is not supported for receive ports. Please select another", "ODBC Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close( );
                    }
                }
            }
            catch ( System.Runtime.InteropServices.COMException ) // Something wrong in schema cache
            {
                MessageBox.Show( "An error has occured connecting the BizTalk Configuration system", "ODBC Configruation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            catch ( Exception exp )
            {
                string msg = exp.Message;
                MessageBox.Show( "An error has occured connecting the BizTalk Configuration system" + " " + msg, "Big trouble", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
        }

        private string GetFirstSQLStatement( XmlSchemaObject parent )
        {
            if ( parent is XmlSchemaAppInfo )
            {
                foreach ( XmlNode child in ( ( XmlSchemaAppInfo )parent ).Markup )
                {
                    if ( child.LocalName == "ODBCCMD" )
                    {
                        XmlAttribute att = child.Attributes[ "value" ];
                        if ( null != child.InnerText )
                            return child.InnerText;
                    }
                }
                return null;
            }
            else if ( parent is XmlSchema )
            {
                foreach ( XmlSchemaObject child in ( ( XmlSchema )parent ).Items )
                {
                    string result = GetFirstSQLStatement( child );
                    if ( result != null )
                        return result;
                }
            }
            else if ( parent is XmlSchemaAnnotation )
            {
                foreach ( XmlSchemaObject child in ( ( XmlSchemaAnnotation )parent ).Items )
                {
                    string result = GetFirstSQLStatement( child );
                    if ( result != null )
                        return result;
                }
            }
            return null;
        }

        private string GetRoot( XmlSchema parent )
        {
            foreach ( XmlSchemaObject child in parent.Items )
            {
                if ( child is XmlSchemaElement )
                    return ( ( XmlSchemaElement )child ).Name;
            }
            return null;
        }

        private string GetTarget( XmlSchema parent )
        {
            return parent.TargetNamespace;
        }
    }
}