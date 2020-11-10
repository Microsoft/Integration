//---------------------------------------------------------------------
// File: QueryEditor.cs
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

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class QueryEditorUserControl : WizardUserControl
    {
        private System.Windows.Forms.TextBox txtRawSQL;
        private System.Windows.Forms.Button cmdValidate;
        private System.Windows.Forms.TextBox txtODBCCommand;
        private System.Windows.Forms.Label lblQueryPrompt;
        private System.Windows.Forms.Label lblCommandPrompt;
        private System.Windows.Forms.RadioButton rdoNoResponse;
        private System.Windows.Forms.RadioButton rdoResponse;
        private System.Windows.Forms.Label lblQueryType;
        private System.Windows.Forms.CheckBox chkOverRide;

        private System.ComponentModel.Container components = null;

        public QueryEditorUserControl( )
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
            this.txtRawSQL = new System.Windows.Forms.TextBox( );
            this.cmdValidate = new System.Windows.Forms.Button( );
            this.txtODBCCommand = new System.Windows.Forms.TextBox( );
            this.lblQueryPrompt = new System.Windows.Forms.Label( );
            this.lblCommandPrompt = new System.Windows.Forms.Label( );
            this.rdoNoResponse = new System.Windows.Forms.RadioButton( );
            this.rdoResponse = new System.Windows.Forms.RadioButton( );
            this.lblQueryType = new System.Windows.Forms.Label( );
            this.chkOverRide = new System.Windows.Forms.CheckBox( );
            this.SuspendLayout( );
            // 
            // txtRawSQL
            // 
            this.txtRawSQL.Location = new System.Drawing.Point( 10, 113 );
            this.txtRawSQL.Multiline = true;
            this.txtRawSQL.Name = "txtRawSQL";
            this.txtRawSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRawSQL.Size = new System.Drawing.Size( 480, 128 );
            this.txtRawSQL.TabIndex = 4;
            // 
            // cmdValidate
            // 
            this.cmdValidate.Location = new System.Drawing.Point( 360, 30 );
            this.cmdValidate.Name = "cmdValidate";
            this.cmdValidate.Size = new System.Drawing.Size( 128, 32 );
            this.cmdValidate.TabIndex = 5;
            this.cmdValidate.Text = "Generate";
            this.cmdValidate.Click += new System.EventHandler( this.cmdValidate_Click );
            // 
            // txtODBCCommand
            // 
            this.txtODBCCommand.Location = new System.Drawing.Point( 12, 267 );
            this.txtODBCCommand.Multiline = true;
            this.txtODBCCommand.Name = "txtODBCCommand";
            this.txtODBCCommand.ReadOnly = true;
            this.txtODBCCommand.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtODBCCommand.Size = new System.Drawing.Size( 480, 34 );
            this.txtODBCCommand.TabIndex = 6;
            // 
            // lblQueryPrompt
            // 
            this.lblQueryPrompt.Location = new System.Drawing.Point( 10, 97 );
            this.lblQueryPrompt.Name = "lblQueryPrompt";
            this.lblQueryPrompt.Size = new System.Drawing.Size( 88, 14 );
            this.lblQueryPrompt.TabIndex = 7;
            this.lblQueryPrompt.Text = "Query";
            // 
            // lblCommandPrompt
            // 
            this.lblCommandPrompt.Location = new System.Drawing.Point( 9, 247 );
            this.lblCommandPrompt.Name = "lblCommandPrompt";
            this.lblCommandPrompt.Size = new System.Drawing.Size( 144, 16 );
            this.lblCommandPrompt.TabIndex = 8;
            this.lblCommandPrompt.Text = "Processed Command";
            // 
            // rdoNoResponse
            // 
            this.rdoNoResponse.Location = new System.Drawing.Point( 43, 68 );
            this.rdoNoResponse.Name = "rdoNoResponse";
            this.rdoNoResponse.Size = new System.Drawing.Size( 296, 17 );
            this.rdoNoResponse.TabIndex = 9;
            this.rdoNoResponse.Text = "INSERT, UPDATE, DELETE (No response expected)";
            // 
            // rdoResponse
            // 
            this.rdoResponse.Checked = true;
            this.rdoResponse.Location = new System.Drawing.Point( 43, 45 );
            this.rdoResponse.Name = "rdoResponse";
            this.rdoResponse.Size = new System.Drawing.Size( 296, 17 );
            this.rdoResponse.TabIndex = 10;
            this.rdoResponse.TabStop = true;
            this.rdoResponse.Text = "SELECT or Compound (Response expected)";
            // 
            // lblQueryType
            // 
            this.lblQueryType.Location = new System.Drawing.Point( 8, 16 );
            this.lblQueryType.Name = "lblQueryType";
            this.lblQueryType.Size = new System.Drawing.Size( 104, 17 );
            this.lblQueryType.TabIndex = 11;
            this.lblQueryType.Text = "Query Type";
            // 
            // chkOverRide
            // 
            this.chkOverRide.Location = new System.Drawing.Point( 292, 247 );
            this.chkOverRide.Name = "chkOverRide";
            this.chkOverRide.Size = new System.Drawing.Size( 196, 16 );
            this.chkOverRide.TabIndex = 12;
            this.chkOverRide.Text = "Override default query processing";
            // 
            // QueryEditorUserControl
            // 
            this.Controls.Add( this.chkOverRide );
            this.Controls.Add( this.lblQueryType );
            this.Controls.Add( this.rdoResponse );
            this.Controls.Add( this.rdoNoResponse );
            this.Controls.Add( this.lblCommandPrompt );
            this.Controls.Add( this.lblQueryPrompt );
            this.Controls.Add( this.txtODBCCommand );
            this.Controls.Add( this.cmdValidate );
            this.Controls.Add( this.txtRawSQL );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "QueryEditorUserControl";
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private void cmdValidate_Click( object sender, System.EventArgs e )
        {
            if ( txtRawSQL.Text.Length > 0 )
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    //Only check this if the user is entering in raw SQL
                    if ( ( ( ODBCAdapterWizardForm )WizardParentForm ).CommandType == ODBCSchemaHelper.AdapterCommandType.SQL )
                    {
                        //Make sure the adapter knows whether to generate a response shcema. If this is only
                        //Inserts, deletes and updates then we don't need a response document
                        if ( rdoResponse.Checked )
                            ( ( ODBCAdapterWizardForm )WizardParentForm ).StatementType = ODBCSchemaHelper.AdapterStatementType.InputOutput;
                        else
                            ( ( ODBCAdapterWizardForm )WizardParentForm ).StatementType = ODBCSchemaHelper.AdapterStatementType.Input;
                    }

                    ( ( ODBCAdapterWizardForm )WizardParentForm ).OverrideDefaultQueryProcessing = chkOverRide.Checked;
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).strScript = txtRawSQL.Text;
                    txtODBCCommand.Text = ( ( ODBCAdapterWizardForm )WizardParentForm ).strGeneratedScript;
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).NextButtonEnabled = true;

                    // Now we need to validate that the user has not entered a query that will require input parameters
                    // for a receive port type. There is no way in this version of the code for the users to enter the
                    // input values for a receive port
                    if ( ( ( ODBCAdapterWizardForm )WizardParentForm ).QueryHasInputParameters && ( ( ODBCAdapterWizardForm )WizardParentForm ).portType == ODBCSchemaHelper.PortType.Receive )
                    {
                        MessageBox.Show( "You cannot use a database query where input parameters would need to be generated in conjunction with a receive port type", "ODBC Schema Generation Wizard Validation Error" );
                        ( ( ODBCAdapterWizardForm )WizardParentForm ).NextButtonEnabled = false;
                    }

                    //((ODBCAdapterWizardForm)WizardParentForm)._clsODBCCmdWizard.CreateBizTalkSchema();

                    this.Cursor = Cursors.Default;
                }
                catch ( Exception ex )
                {
                    this.Cursor = Cursors.Default;

                    MessageBox.Show( "An error occured processing the specified query: " + ex.Message, "Validation Error", MessageBoxButtons.OK );
                }
            }
            else
            {
                MessageBox.Show( "You must enter a query to be validated", "Validation Error", MessageBoxButtons.OK );
            }
        }

        public override bool SavePageInfoWithoutValidation( )
        {
            return base.SavePageInfoWithoutValidation( );
        }

        public override bool SavePageInfo( )
        {
            return true;
        }

        protected override void LoadPageInfo( )
        {
            //Setup for display based on the type of query we are processing
            if ( ( ( ODBCAdapterWizardForm )WizardParentForm ).CommandType == ODBCSchemaHelper.AdapterCommandType.SQL && ( ( ODBCAdapterWizardForm )WizardParentForm ).portType != ODBCSchemaHelper.PortType.Receive )
            {
                rdoResponse.Visible = true;
                rdoNoResponse.Visible = true;
                lblQueryType.Visible = true;
                chkOverRide.Visible = true;
            }
            else
            {
                rdoResponse.Visible = false;
                rdoNoResponse.Visible = false;
                lblQueryType.Visible = false;
                chkOverRide.Visible = false;
            }
            base.LoadPageInfo( );
        }
    }
}