//---------------------------------------------------------------------
// File: DBConnection.cs
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;

using ODBCDriverPrompt;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class DBConnectionUserControl : WizardUserControl
    {
        private System.Windows.Forms.Button cmdSet;
        private System.Windows.Forms.Label lblConnectionStringPrompt;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtDBConnectionString;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private string _sConnection = string.Empty;

        public DBConnectionUserControl( )
        {
            InitializeComponent( );
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.txtDBConnectionString = new System.Windows.Forms.TextBox( );
            this.cmdSet = new System.Windows.Forms.Button( );
            this.lblConnectionStringPrompt = new System.Windows.Forms.Label( );
            this.lblConnectionString = new System.Windows.Forms.Label( );
            this.SuspendLayout( );
            // 
            // txtDBConnectionString
            // 
            this.txtDBConnectionString.Location = new System.Drawing.Point( 8, 88 );
            this.txtDBConnectionString.Multiline = true;
            this.txtDBConnectionString.Name = "txtDBConnectionString";
            this.txtDBConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDBConnectionString.Size = new System.Drawing.Size( 480, 216 );
            this.txtDBConnectionString.TabIndex = 0;
            // 
            // cmdSet
            // 
            this.cmdSet.Location = new System.Drawing.Point( 392, 56 );
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.Size = new System.Drawing.Size( 96, 24 );
            this.cmdSet.TabIndex = 1;
            this.cmdSet.Text = "Set";
            this.cmdSet.Click += new System.EventHandler( this.button1_Click );
            // 
            // lblConnectionStringPrompt
            // 
            this.lblConnectionStringPrompt.Location = new System.Drawing.Point( 8, 16 );
            this.lblConnectionStringPrompt.Name = "lblConnectionStringPrompt";
            this.lblConnectionStringPrompt.Size = new System.Drawing.Size( 480, 16 );
            this.lblConnectionStringPrompt.TabIndex = 2;
            this.lblConnectionStringPrompt.Text = "To set the connection string, click set";
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.Location = new System.Drawing.Point( 8, 64 );
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size( 256, 16 );
            this.lblConnectionString.TabIndex = 3;
            this.lblConnectionString.Text = "Connection string";
            // 
            // DBConnectionUserControl
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add( this.lblConnectionString );
            this.Controls.Add( this.lblConnectionStringPrompt );
            this.Controls.Add( this.cmdSet );
            this.Controls.Add( this.txtDBConnectionString );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "DBConnectionUserControl";
            this.Load += new System.EventHandler( this.DBConnectionUserControl_Load );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }
        #endregion

        private void DBConnectionUserControl_Load( object sender, System.EventArgs e )
        {
        }

        public override bool SavePageInfo( )
        {
            OdbcConnection myConnection = null;
            //	_sConnection = txtDBConnectionString.Text;
            try
            {
                myConnection = new OdbcConnection( _sConnection );
                myConnection.Open( );
                // Commented out as it seems some ODBC drives do not support these parameters
                //				if ( myConnection.Database == null || myConnection.Database.Length == 0)
                //				{
                //					myConnection.Close();
                //					MessageBox.Show("Connection String failed validation","Connection Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //					return false;
                //				}	
                myConnection.Close( );
            }
            catch ( System.Data.Odbc.OdbcException e )
            {
                MessageBox.Show( "The database connection is invalid \n" + e.Message, "Connection Vaidation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }
            finally
            {
                if ( myConnection != null )
                    myConnection.Close( );
            }

            ( ( ODBCAdapterWizardForm )WizardParentForm ).strDBConnection = _sConnection;

            return true;
        }

        public override void ProcessLoad( )
        {
            // Connection string may have been initialized by selecting a port before launching the wizard.
            _sConnection = ( ( ODBCAdapterWizardForm )WizardParentForm ).strDBConnection;
            txtDBConnectionString.Text = SecurePassword( _sConnection );

            if ( _sConnection.Length > 0 )
            {
                WizardParentForm.NextButtonEnabled = true;
            }
        }

        private void button1_Click( object sender, System.EventArgs e )
        {
            ODBCDriverUI DriverDialog = new ODBCDriverUI( );
            _sConnection = DriverDialog.GetDSN( );
            txtDBConnectionString.Text = SecurePassword( _sConnection );
            WizardParentForm.NextButtonEnabled = true;
        }

        private string SecurePassword( string ConnectionString )
        {
            if ( ConnectionString != "" )
            {
                int iPasswordIndex = ConnectionString.IndexOf( "PWD=", 1 );

                if ( iPasswordIndex > 0 )
                {
                    int iEndOfPassword = ConnectionString.IndexOf( ";", iPasswordIndex );

                    //Deal with the fact that we may not have a ; at the end of our statement
                    if ( iEndOfPassword < 0 )
                        iEndOfPassword = ConnectionString.Length;

                    string password = ConnectionString.Substring( iPasswordIndex + 4, iEndOfPassword - ( iPasswordIndex + 4 ) );

                    return ConnectionString.Replace( password, "*********" );
                }
                else
                    return ConnectionString;
            }
            else
                return "";
       }
    }
}