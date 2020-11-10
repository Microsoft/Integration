//---------------------------------------------------------------------
// File: QueryType.cs
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
    public class QueryTypeUserControl : WizardUserControl
    {
        private System.Windows.Forms.Label lblStatementType;
        private System.Windows.Forms.RadioButton rdoSQL;
        private System.Windows.Forms.RadioButton rdoStoredProc;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public QueryTypeUserControl( )
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
            this.lblStatementType = new System.Windows.Forms.Label( );
            this.rdoSQL = new System.Windows.Forms.RadioButton( );
            this.rdoStoredProc = new System.Windows.Forms.RadioButton( );
            this.SuspendLayout( );
            // 
            // lblStatementType
            // 
            this.lblStatementType.Location = new System.Drawing.Point( 16, 24 );
            this.lblStatementType.Name = "lblStatementType";
            this.lblStatementType.Size = new System.Drawing.Size( 448, 16 );
            this.lblStatementType.TabIndex = 0;
            this.lblStatementType.Text = "Select type of statement";
            // 
            // rdoSQL
            // 
            this.rdoSQL.Location = new System.Drawing.Point( 48, 99 );
            this.rdoSQL.Name = "rdoSQL";
            this.rdoSQL.Size = new System.Drawing.Size( 127, 27 );
            this.rdoSQL.TabIndex = 1;
            this.rdoSQL.Text = "SQL Script";
            // 
            // rdoStoredProc
            // 
            this.rdoStoredProc.Checked = true;
            this.rdoStoredProc.Location = new System.Drawing.Point( 48, 61 );
            this.rdoStoredProc.Name = "rdoStoredProc";
            this.rdoStoredProc.Size = new System.Drawing.Size( 127, 22 );
            this.rdoStoredProc.TabIndex = 2;
            this.rdoStoredProc.TabStop = true;
            this.rdoStoredProc.Text = "Stored Procedure";
            // 
            // QueryTypeUserControl
            // 
            this.Controls.Add( this.rdoStoredProc );
            this.Controls.Add( this.rdoSQL );
            this.Controls.Add( this.lblStatementType );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "QueryTypeUserControl";
            this.ResumeLayout( false );

        }
        #endregion

        public override bool SavePageInfoWithoutValidation( )
        {
            return base.SavePageInfoWithoutValidation( );
        }

        public override bool SavePageInfo( )
        {
            if ( rdoStoredProc.Checked )
                ( ( ODBCAdapterWizardForm )WizardParentForm ).CommandType = ODBCSchemaHelper.AdapterCommandType.StoredProcedure;
            else
                ( ( ODBCAdapterWizardForm )WizardParentForm ).CommandType = ODBCSchemaHelper.AdapterCommandType.SQL;

            return true;
        }
    }
}