//---------------------------------------------------------------------
// File: ODBCAdapterWizardForm.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.BizTalk.Adapters.ODBC.SchemaWizard;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class ODBCAdapterWizardForm : Microsoft.BizTalk.Adapters.ODBC.SchemaWizard.WizardForm
    {
        private System.ComponentModel.Container components = null;
        
        internal ODBCSchemaHelper _clsODBCCmdWizard = null;
        
        #region Properties

        public ODBCAdapterWizardForm( ) : base( )
        {
            Application.EnableVisualStyles( );

            _clsODBCCmdWizard = new ODBCSchemaHelper( );
        }

        public ODBCSchemaHelper.AdapterCommandType CommandType
        {
            get
            {
                return _clsODBCCmdWizard.QueryCommandType;
            }
            set
            {
                _clsODBCCmdWizard.QueryCommandType = value;
            }
        }

        public string strDBConnection
        {
            get
            {
                return _clsODBCCmdWizard.strDBConnection;
            }
            set
            {
                _clsODBCCmdWizard.strDBConnection = value;
            }
        }

        public string strInputSchema
        {
            get
            {
                return _clsODBCCmdWizard.InputSchema;
            }
        }

        public string strOutputSchema
        {
            get
            {
                return _clsODBCCmdWizard.OutputSchema;
            }
        }
        public bool QueryHasInputParameters
        {
            get
            {
                return _clsODBCCmdWizard.InputParameter;
            }
        }

        public bool OverrideDefaultQueryProcessing
        {
            set
            {
                _clsODBCCmdWizard.OverrideQueryProcessing = value;
            }
        }

        public string strScript
        {
            get
            {
                return _clsODBCCmdWizard.strScript;
            }
            set
            {
                _clsODBCCmdWizard.strScript = value;
            }
        }

        public ODBCSchemaHelper.PortType portType
        {
            get
            {
                return _clsODBCCmdWizard.portType;
            }
            set
            {
                _clsODBCCmdWizard.portType = value;
            }
        }

        public string strInputRoot
        {
            get
            {
                return _clsODBCCmdWizard.strInputRoot;
            }
            set
            {
                _clsODBCCmdWizard.strInputRoot = value;
            }
        }

        public string strOutputRoot
        {
            get
            {
                return _clsODBCCmdWizard.strOutputRoot;
            }
            set
            {
                _clsODBCCmdWizard.strOutputRoot = value;
            }
        }

        public string strSPName
        {
            get
            {
                return _clsODBCCmdWizard.strSPName;
            }
            set
            {
                _clsODBCCmdWizard.strSPName = value;
            }
        }

        public string strTargetNamespace
        {
            get
            {
                return _clsODBCCmdWizard.strTargetNamespace;
            }
            set
            {
                _clsODBCCmdWizard.strTargetNamespace = value;
            }
        }

        public string strGeneratedScript
        {
            get
            {
                return _clsODBCCmdWizard.strGeneratedScript;
            }
            set
            {
                _clsODBCCmdWizard.strGeneratedScript = value;
            }
        }

        public string strWSDL
        {
            get
            {
                return _clsODBCCmdWizard.strWSDL;
            }
            set
            {
                _clsODBCCmdWizard.strWSDL = value;
            }
        }

        public ODBCSchemaHelper.AdapterStatementType StatementType
        {
            get
            {
                return _clsODBCCmdWizard.StatementType;
            }
            set
            {
                _clsODBCCmdWizard.StatementType = value;
            }
        }

        #endregion

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

        #endregion

        protected virtual void CreateDatabasePage( )
        {
            buttonBack.Enabled = false;

            DBConnectionUserControl databaseControl = null;
            foreach ( UserControl userControl in panelWizard.Controls )
            {
                if ( userControl is DBConnectionUserControl )
                    databaseControl = ( DBConnectionUserControl )userControl;
            }
            if ( null == databaseControl )
                databaseControl = new DBConnectionUserControl( );

            lblTitle.Text = "Database Information";
            lblScript.Text = "Select the ODBC driver and connection string to be used to generate the BizTalk Schema";

            SetNewPageProperties( ( WizardUserControl )databaseControl );
        }

        protected virtual void CreateRootPage( )
        {
            buttonBack.Enabled = true;

            SchemaMetaDataUserControl rootControl = null;
            foreach ( UserControl userControl in panelWizard.Controls )
            {
                if ( userControl is SchemaMetaDataUserControl )
                    rootControl = ( SchemaMetaDataUserControl )userControl;
            }
            if ( null == rootControl )
                rootControl = new SchemaMetaDataUserControl( );

            lblTitle.Text = "Schema Information";
            lblScript.Text = "Specify the root name(s) and port type for the schema";

            SetNewPageProperties( ( WizardUserControl )rootControl );
        }

        protected virtual void CreateStatementPage( )
        {
            buttonNext.Enabled = false;

            QueryEditorUserControl statementControl = null;
            foreach ( UserControl userControl in panelWizard.Controls )
            {
                if ( userControl is QueryEditorUserControl )
                    statementControl = ( QueryEditorUserControl )userControl;
            }
            if ( null == statementControl )
                statementControl = new QueryEditorUserControl( );

            lblTitle.Text = "Statement Information";
            lblScript.Text = "Enter the statement to be executed by the ODBC transport";

            SetNewPageProperties( ( WizardUserControl )statementControl );
        }

        protected virtual void CreateQueryTypePage( )
        {
            buttonNext.Text = "Next >";

            QueryTypeUserControl QueryTypeControl = null;
            foreach ( UserControl userControl in panelWizard.Controls )
            {
                if ( userControl is QueryTypeUserControl )
                    QueryTypeControl = ( QueryTypeUserControl )userControl;
            }
            if ( null == QueryTypeControl )
                QueryTypeControl = new QueryTypeUserControl( );

            lblTitle.Text = "Statement Type Information";
            lblScript.Text = "Select the type of statement to be excuted by the ODBC transport";

            SetNewPageProperties( ( WizardUserControl )QueryTypeControl );
        }

        protected virtual void CreateFinishPage( )
        {
            buttonNext.Text = "Finish";

            FinishDisplayUserControl finishControl = null;
            foreach ( UserControl userControl in panelWizard.Controls )
            {
                if ( userControl is FinishDisplayUserControl )
                    finishControl = ( FinishDisplayUserControl )userControl;
            }
            if ( null == finishControl )
                finishControl = new FinishDisplayUserControl( );

            SetNewPageProperties( ( WizardUserControl )finishControl );
        }

        protected override void OnNext( object sender, System.EventArgs e )
        {
            if ( _userControl == null )
                return;

            if ( _userControl.SavePageInfo( ) == false )
            {
                _userControl.Select( );
                return;
            }

            if ( _userControl is DBConnectionUserControl )
            {
                CreateRootPage( );
            }
            else if ( _userControl is SchemaMetaDataUserControl )
            {
                CreateQueryTypePage( );
            }
            else if ( _userControl is QueryTypeUserControl )
            {
                CreateStatementPage( );
            }
            else if ( _userControl is QueryEditorUserControl )
            {
                CreateFinishPage( );
            }
            else if ( _userControl is FinishDisplayUserControl )
            {
                this.DialogResult = DialogResult.OK;
                this.Close( );
            }

            _userControl.ProcessLoad( );
            _userControl.Show( );
            _userControl.SetSelect( );
            _userControl.Select( );

            panelWizard.Invalidate( );
            panelWizard.Update( );

            this.buttonBack.Enabled = true;
            if ( buttonNext.Enabled )
                buttonNext.Focus( );

            panelWizard.Invalidate( );
            panelWizard.Update( );
        }

        protected override void OnBack( object sender, System.EventArgs e )
        {
            if ( _userControl == null )
                return;

            if ( _userControl.SavePageInfoWithoutValidation( ) == false )
            {
                _userControl.Select( );
                return;
            }

            if ( _userControl is SchemaMetaDataUserControl )
            {
                CreateDatabasePage( );
            }
            else if ( _userControl is QueryEditorUserControl )
            {
                CreateQueryTypePage( );
            }
            else if ( _userControl is QueryTypeUserControl )
            {
                CreateRootPage( );
            }
            else if ( _userControl is FinishDisplayUserControl )
            {
                CreateStatementPage( );
            }

            _userControl.ProcessLoad( );
            _userControl.Show( );
            _userControl.SetSelect( );
            _userControl.Select( );

            panelWizard.Invalidate( );
            panelWizard.Update( );

            if ( buttonNext.Enabled )
                buttonNext.Focus( );
            this.buttonNext.Enabled = true;

            panelWizard.Invalidate( );
            panelWizard.Update( );
        }

        public void Start( )
        {
            CreateDatabasePage( );
            _userControl.ProcessLoad( );
            this.buttonBack.Text = "< Back";
            this.buttonCancel.Text = "Cancel";
            this.buttonNext.Text = "Next >";
        }

        private void InitializeComponent( )
        {
            ( ( System.ComponentModel.ISupportInitialize )( this.picGraphic ) ).BeginInit( );
            this.SuspendLayout( );
            // 
            // panelWizard
            // 
            this.panelWizard.Location = new System.Drawing.Point( 0, 0 );
            this.panelWizard.Size = new System.Drawing.Size( 484, 312 );
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point( 16, 336 );
            this.buttonHelp.Size = new System.Drawing.Size( 80, 23 );
            this.buttonHelp.Visible = true;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point( 216, 336 );
            this.buttonBack.Size = new System.Drawing.Size( 80, 23 );
            // 
            // buttonNext
            // 
            this.buttonNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonNext.Location = new System.Drawing.Point( 296, 328 );
            this.buttonNext.Size = new System.Drawing.Size( 80, 33 );
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point( 384, 336 );
            this.buttonCancel.Size = new System.Drawing.Size( 80, 23 );
            // 
            // ODBCAdapterWizardForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.BackButtonEnabled = true;
            this.ClientSize = new System.Drawing.Size( 478, 366 );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "ODBCAdapterWizardForm";
            this.NextButtonEnabled = true;
            ( ( System.ComponentModel.ISupportInitialize )( this.picGraphic ) ).EndInit( );
            this.ResumeLayout( false );

        }
    }
}