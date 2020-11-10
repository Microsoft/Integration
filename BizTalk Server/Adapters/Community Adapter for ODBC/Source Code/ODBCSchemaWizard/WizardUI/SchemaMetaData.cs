//---------------------------------------------------------------------
// File: SchemaMetaData.cs
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
    public class SchemaMetaDataUserControl : WizardUserControl
    {
        private System.Windows.Forms.TextBox txtTargetNS;
        private System.Windows.Forms.Label lblPortType;
        private System.Windows.Forms.RadioButton rdoSendPort;
        private System.Windows.Forms.RadioButton rdoReceivePort;
        private System.Windows.Forms.Label lblTargetNSPrompt;
        private System.Windows.Forms.Label lblRequestRootPrompt;
        private System.Windows.Forms.TextBox txtRequestRootName;
        private System.Windows.Forms.Label lblResponseRootName;
        private System.Windows.Forms.TextBox txtResponseRootName;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public override bool SavePageInfoWithoutValidation( )
        {
            return base.SavePageInfoWithoutValidation( );
        }

        public override bool SavePageInfo( )
        {
            if ( txtTargetNS.Text.Length == 0 )
            {
                MessageBox.Show( "You must enter a targetname space", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }
            try
            {
                Validator.ValidateTargetNamespace( txtTargetNS.Text );
            }
            catch ( SqlValidationException ex )
            {
                MessageBox.Show( "An error occured while validating the name specified for the namespace \n" + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }

            if ( rdoReceivePort.Checked )
            {
                if ( txtRequestRootName.Text.Length == 0 )
                {
                    MessageBox.Show( "You must enter a valid root element name for the response message.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
                try
                {
                    Validator.ValidateRootElementName( txtRequestRootName.Text );
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).strInputRoot = "ODBCQUERY";
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).strOutputRoot = txtRequestRootName.Text;
                }
                catch ( SqlValidationException ex )
                {
                    MessageBox.Show( ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
            }
            else if ( rdoSendPort.Checked )
            {
                if ( rdoSendPort.Checked && ( txtRequestRootName.Text.Length == 0 || txtResponseRootName.Text.Length == 0 ) )
                {
                    MessageBox.Show( "You must enter a root node name for both the request and response messages.", "Schema data validation error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
                if ( txtRequestRootName.Text == txtResponseRootName.Text )
                {
                    MessageBox.Show( "The root node names for both elements must be different", "Schema data validation error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
                try
                {
                    Validator.ValidateRootElementName( txtRequestRootName.Text );
                    Validator.ValidateRootElementName( txtResponseRootName.Text );
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).strInputRoot = txtRequestRootName.Text;
                    ( ( ODBCAdapterWizardForm )WizardParentForm ).strOutputRoot = txtResponseRootName.Text;
                }
                catch ( SqlValidationException ex )
                {
                    MessageBox.Show( ex.Message, "Schema data validation error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
            }

            ( ( ODBCAdapterWizardForm )WizardParentForm ).strTargetNamespace = txtTargetNS.Text;

            if ( rdoReceivePort.Checked )
                ( ( ODBCAdapterWizardForm )WizardParentForm ).portType = ODBCSchemaHelper.PortType.Receive;
            else if ( rdoSendPort.Checked )
                ( ( ODBCAdapterWizardForm )WizardParentForm ).portType = ODBCSchemaHelper.PortType.Send;
            rdoReceivePort_CheckedChanged( null, null );

            return true;
        }

        public SchemaMetaDataUserControl( )
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
            this.txtTargetNS = new System.Windows.Forms.TextBox( );
            this.lblPortType = new System.Windows.Forms.Label( );
            this.rdoSendPort = new System.Windows.Forms.RadioButton( );
            this.rdoReceivePort = new System.Windows.Forms.RadioButton( );
            this.lblTargetNSPrompt = new System.Windows.Forms.Label( );
            this.lblRequestRootPrompt = new System.Windows.Forms.Label( );
            this.txtRequestRootName = new System.Windows.Forms.TextBox( );
            this.lblResponseRootName = new System.Windows.Forms.Label( );
            this.txtResponseRootName = new System.Windows.Forms.TextBox( );
            this.SuspendLayout( );
            // 
            // txtTargetNS
            // 
            this.txtTargetNS.Location = new System.Drawing.Point( 16, 40 );
            this.txtTargetNS.Name = "txtTargetNS";
            this.txtTargetNS.Size = new System.Drawing.Size( 464, 21 );
            this.txtTargetNS.TabIndex = 0;
            // 
            // lblPortType
            // 
            this.lblPortType.Location = new System.Drawing.Point( 24, 72 );
            this.lblPortType.Name = "lblPortType";
            this.lblPortType.Size = new System.Drawing.Size( 448, 16 );
            this.lblPortType.TabIndex = 1;
            this.lblPortType.Text = "Select the port type:";
            // 
            // rdoSendPort
            // 
            this.rdoSendPort.Location = new System.Drawing.Point( 48, 128 );
            this.rdoSendPort.Name = "rdoSendPort";
            this.rdoSendPort.Size = new System.Drawing.Size( 392, 16 );
            this.rdoSendPort.TabIndex = 2;
            this.rdoSendPort.Text = "Send Port";
            // 
            // rdoReceivePort
            // 
            this.rdoReceivePort.Checked = true;
            this.rdoReceivePort.Location = new System.Drawing.Point( 48, 96 );
            this.rdoReceivePort.Name = "rdoReceivePort";
            this.rdoReceivePort.Size = new System.Drawing.Size( 392, 16 );
            this.rdoReceivePort.TabIndex = 3;
            this.rdoReceivePort.TabStop = true;
            this.rdoReceivePort.Text = "Receive Port";
            this.rdoReceivePort.CheckedChanged += new System.EventHandler( this.rdoReceivePort_CheckedChanged );
            // 
            // lblTargetNSPrompt
            // 
            this.lblTargetNSPrompt.Location = new System.Drawing.Point( 16, 16 );
            this.lblTargetNSPrompt.Name = "lblTargetNSPrompt";
            this.lblTargetNSPrompt.Size = new System.Drawing.Size( 464, 16 );
            this.lblTargetNSPrompt.TabIndex = 4;
            this.lblTargetNSPrompt.Text = "Target namespace (Example: http://DBReceiveProject)";
            // 
            // lblRequestRootPrompt
            // 
            this.lblRequestRootPrompt.Location = new System.Drawing.Point( 16, 168 );
            this.lblRequestRootPrompt.Name = "lblRequestRootPrompt";
            this.lblRequestRootPrompt.Size = new System.Drawing.Size( 464, 16 );
            this.lblRequestRootPrompt.TabIndex = 6;
            this.lblRequestRootPrompt.Text = "Response root element name";
            // 
            // txtRequestRootName
            // 
            this.txtRequestRootName.Location = new System.Drawing.Point( 16, 192 );
            this.txtRequestRootName.Name = "txtRequestRootName";
            this.txtRequestRootName.Size = new System.Drawing.Size( 464, 21 );
            this.txtRequestRootName.TabIndex = 5;
            // 
            // lblResponseRootName
            // 
            this.lblResponseRootName.Location = new System.Drawing.Point( 16, 240 );
            this.lblResponseRootName.Name = "lblResponseRootName";
            this.lblResponseRootName.Size = new System.Drawing.Size( 464, 16 );
            this.lblResponseRootName.TabIndex = 8;
            this.lblResponseRootName.Text = "Response root element name";
            this.lblResponseRootName.Visible = false;
            // 
            // txtResponseRootName
            // 
            this.txtResponseRootName.Location = new System.Drawing.Point( 16, 264 );
            this.txtResponseRootName.Name = "txtResponseRootName";
            this.txtResponseRootName.Size = new System.Drawing.Size( 464, 21 );
            this.txtResponseRootName.TabIndex = 7;
            this.txtResponseRootName.Visible = false;
            // 
            // SchemaMetaDataUserControl
            // 
            this.Controls.Add( this.lblResponseRootName );
            this.Controls.Add( this.txtResponseRootName );
            this.Controls.Add( this.lblRequestRootPrompt );
            this.Controls.Add( this.txtRequestRootName );
            this.Controls.Add( this.lblTargetNSPrompt );
            this.Controls.Add( this.rdoReceivePort );
            this.Controls.Add( this.rdoSendPort );
            this.Controls.Add( this.lblPortType );
            this.Controls.Add( this.txtTargetNS );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "SchemaMetaDataUserControl";
            this.Load += new System.EventHandler( this.SchemaMetaDataUserControl_Load );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }
        #endregion

        private void rdoReceivePort_CheckedChanged( object sender, System.EventArgs e )
        {
            if ( rdoReceivePort.Checked )
            {
                lblResponseRootName.Hide( );
                txtResponseRootName.Hide( );
                lblRequestRootPrompt.Text = "Response element root name";
            }
            else
            {
                //this.labelInput.Text = ODBCResourceHandler.GetResourceString("RootInputLabelSend");
                //this.labelOutput.Text = ODBCResourceHandler.GetResourceString("RootOutputLabelSend"); 
                lblResponseRootName.Show( );
                txtResponseRootName.Show( );
                lblRequestRootPrompt.Text = "Request element root name";
            }
        }

        private void SchemaMetaDataUserControl_Load( object sender, System.EventArgs e )
        {
        }
    }
}