//---------------------------------------------------------------------
// File: TestForm.cs
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
using System.Data;
using System.Data.OleDb; 
using System.Data.Odbc; 
using System.Xml;
using System.Xml.Schema; 
using System.Xml.Serialization; 
using System.IO;
using System.Text;
using System.Reflection;

using Microsoft.BizTalk.Adapters.ODBC.SchemaWizard;
using Microsoft.BizTalk.Adapters.ODBC;

using ODBCDriverPrompt;

namespace Microsoft.BizTalk.Adapters.ODBC
{
    public class TestForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog digOpenFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpQueryEngine;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtInputSchemaLocation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInputInstanceName;
        private System.Windows.Forms.Button btnSchemaName;
        private System.Windows.Forms.Button btnInputInst;
        private System.Windows.Forms.Button btnExecuteQuery;
        private System.Windows.Forms.TextBox txtQueryResults;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.RadioButton rdoReceive;
        private System.Windows.Forms.RadioButton rdoSend;
        private System.Windows.Forms.Label lblSchemaName;
        private System.Windows.Forms.TextBox txtIterations;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tpSchemaWizard;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtOutputSchema;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInputSchema;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSQLStatement;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;

        private System.ComponentModel.Container components = null;

        public TestForm( )
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog( );
            this.digOpenFile = new System.Windows.Forms.OpenFileDialog( );
            this.tabControl1 = new System.Windows.Forms.TabControl( );
            this.tpSchemaWizard = new System.Windows.Forms.TabPage( );
            this.label12 = new System.Windows.Forms.Label( );
            this.label11 = new System.Windows.Forms.Label( );
            this.label7 = new System.Windows.Forms.Label( );
            this.button1 = new System.Windows.Forms.Button( );
            this.txtOutputSchema = new System.Windows.Forms.TextBox( );
            this.label3 = new System.Windows.Forms.Label( );
            this.txtInputSchema = new System.Windows.Forms.TextBox( );
            this.label2 = new System.Windows.Forms.Label( );
            this.txtSQLStatement = new System.Windows.Forms.TextBox( );
            this.label4 = new System.Windows.Forms.Label( );
            this.label1 = new System.Windows.Forms.Label( );
            this.tpQueryEngine = new System.Windows.Forms.TabPage( );
            this.label8 = new System.Windows.Forms.Label( );
            this.txtQueryResults = new System.Windows.Forms.TextBox( );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.label10 = new System.Windows.Forms.Label( );
            this.label5 = new System.Windows.Forms.Label( );
            this.txtIterations = new System.Windows.Forms.TextBox( );
            this.rdoSend = new System.Windows.Forms.RadioButton( );
            this.rdoReceive = new System.Windows.Forms.RadioButton( );
            this.btnConnection = new System.Windows.Forms.Button( );
            this.label9 = new System.Windows.Forms.Label( );
            this.txtConnectionString = new System.Windows.Forms.TextBox( );
            this.btnExecuteQuery = new System.Windows.Forms.Button( );
            this.btnInputInst = new System.Windows.Forms.Button( );
            this.btnSchemaName = new System.Windows.Forms.Button( );
            this.label6 = new System.Windows.Forms.Label( );
            this.txtInputInstanceName = new System.Windows.Forms.TextBox( );
            this.lblSchemaName = new System.Windows.Forms.Label( );
            this.txtInputSchemaLocation = new System.Windows.Forms.TextBox( );
            this.tabControl1.SuspendLayout( );
            this.tpSchemaWizard.SuspendLayout( );
            this.tpQueryEngine.SuspendLayout( );
            this.groupBox2.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.tpSchemaWizard );
            this.tabControl1.Controls.Add( this.tpQueryEngine );
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 744, 589 );
            this.tabControl1.TabIndex = 15;
            // 
            // tpSchemaWizard
            // 
            this.tpSchemaWizard.Controls.Add( this.label12 );
            this.tpSchemaWizard.Controls.Add( this.label11 );
            this.tpSchemaWizard.Controls.Add( this.label7 );
            this.tpSchemaWizard.Controls.Add( this.button1 );
            this.tpSchemaWizard.Controls.Add( this.txtOutputSchema );
            this.tpSchemaWizard.Controls.Add( this.label3 );
            this.tpSchemaWizard.Controls.Add( this.txtInputSchema );
            this.tpSchemaWizard.Controls.Add( this.label2 );
            this.tpSchemaWizard.Controls.Add( this.txtSQLStatement );
            this.tpSchemaWizard.Controls.Add( this.label4 );
            this.tpSchemaWizard.Controls.Add( this.label1 );
            this.tpSchemaWizard.Location = new System.Drawing.Point( 4, 22 );
            this.tpSchemaWizard.Name = "tpSchemaWizard";
            this.tpSchemaWizard.Size = new System.Drawing.Size( 736, 563 );
            this.tpSchemaWizard.TabIndex = 0;
            this.tpSchemaWizard.Text = "Schema Wizard Test";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point( 378, 116 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size( 167, 12 );
            this.label12.TabIndex = 27;
            this.label12.Text = "Output Schema";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point( 2, 114 );
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size( 256, 14 );
            this.label11.TabIndex = 26;
            this.label11.Text = "Input Schema";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point( 1, 37 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 151, 15 );
            this.label7.TabIndex = 25;
            this.label7.Text = "ADO.NET Escape Sequence";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 584, 8 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 144, 32 );
            this.button1.TabIndex = 24;
            this.button1.Text = "Test ODBC Wizard";
            this.button1.Click += new System.EventHandler( this.button1_Click );
            // 
            // txtOutputSchema
            // 
            this.txtOutputSchema.Location = new System.Drawing.Point( 376, 130 );
            this.txtOutputSchema.Multiline = true;
            this.txtOutputSchema.Name = "txtOutputSchema";
            this.txtOutputSchema.ReadOnly = true;
            this.txtOutputSchema.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutputSchema.Size = new System.Drawing.Size( 360, 430 );
            this.txtOutputSchema.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 368, 232 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 136, 16 );
            this.label3.TabIndex = 23;
            // 
            // txtInputSchema
            // 
            this.txtInputSchema.AcceptsReturn = true;
            this.txtInputSchema.AcceptsTab = true;
            this.txtInputSchema.Location = new System.Drawing.Point( 0, 129 );
            this.txtInputSchema.Multiline = true;
            this.txtInputSchema.Name = "txtInputSchema";
            this.txtInputSchema.ReadOnly = true;
            this.txtInputSchema.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInputSchema.Size = new System.Drawing.Size( 367, 431 );
            this.txtInputSchema.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 0, 232 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 160, 16 );
            this.label2.TabIndex = 21;
            // 
            // txtSQLStatement
            // 
            this.txtSQLStatement.Location = new System.Drawing.Point( 1, 54 );
            this.txtSQLStatement.Multiline = true;
            this.txtSQLStatement.Name = "txtSQLStatement";
            this.txtSQLStatement.ReadOnly = true;
            this.txtSQLStatement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQLStatement.Size = new System.Drawing.Size( 734, 53 );
            this.txtSQLStatement.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 0, 152 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 168, 16 );
            this.label4.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 2, 42 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 160, 16 );
            this.label1.TabIndex = 17;
            // 
            // tpQueryEngine
            // 
            this.tpQueryEngine.Controls.Add( this.label8 );
            this.tpQueryEngine.Controls.Add( this.txtQueryResults );
            this.tpQueryEngine.Controls.Add( this.groupBox2 );
            this.tpQueryEngine.Location = new System.Drawing.Point( 4, 22 );
            this.tpQueryEngine.Name = "tpQueryEngine";
            this.tpQueryEngine.Size = new System.Drawing.Size( 736, 563 );
            this.tpQueryEngine.TabIndex = 2;
            this.tpQueryEngine.Text = "Query Engine Test";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point( 0, 184 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 184, 16 );
            this.label8.TabIndex = 24;
            this.label8.Text = "Query Results:";
            // 
            // txtQueryResults
            // 
            this.txtQueryResults.Location = new System.Drawing.Point( 0, 200 );
            this.txtQueryResults.Multiline = true;
            this.txtQueryResults.Name = "txtQueryResults";
            this.txtQueryResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQueryResults.Size = new System.Drawing.Size( 736, 360 );
            this.txtQueryResults.TabIndex = 23;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.label10 );
            this.groupBox2.Controls.Add( this.label5 );
            this.groupBox2.Controls.Add( this.txtIterations );
            this.groupBox2.Controls.Add( this.rdoSend );
            this.groupBox2.Controls.Add( this.rdoReceive );
            this.groupBox2.Controls.Add( this.btnConnection );
            this.groupBox2.Controls.Add( this.label9 );
            this.groupBox2.Controls.Add( this.txtConnectionString );
            this.groupBox2.Controls.Add( this.btnExecuteQuery );
            this.groupBox2.Controls.Add( this.btnInputInst );
            this.groupBox2.Controls.Add( this.btnSchemaName );
            this.groupBox2.Controls.Add( this.label6 );
            this.groupBox2.Controls.Add( this.txtInputInstanceName );
            this.groupBox2.Controls.Add( this.lblSchemaName );
            this.groupBox2.Controls.Add( this.txtInputSchemaLocation );
            this.groupBox2.Location = new System.Drawing.Point( 8, 8 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 720, 160 );
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point( 567, 85 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size( 56, 16 );
            this.label10.TabIndex = 34;
            this.label10.Text = "Iterations:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 16, 16 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 96, 16 );
            this.label5.TabIndex = 33;
            this.label5.Text = "Query Direction";
            // 
            // txtIterations
            // 
            this.txtIterations.Location = new System.Drawing.Point( 624, 80 );
            this.txtIterations.MaxLength = 4;
            this.txtIterations.Name = "txtIterations";
            this.txtIterations.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtIterations.Size = new System.Drawing.Size( 32, 21 );
            this.txtIterations.TabIndex = 32;
            this.txtIterations.Text = "1";
            // 
            // rdoSend
            // 
            this.rdoSend.Checked = true;
            this.rdoSend.Location = new System.Drawing.Point( 24, 40 );
            this.rdoSend.Name = "rdoSend";
            this.rdoSend.Size = new System.Drawing.Size( 136, 16 );
            this.rdoSend.TabIndex = 31;
            this.rdoSend.TabStop = true;
            this.rdoSend.Text = "Send Query";
            this.rdoSend.CheckedChanged += new System.EventHandler( this.rdoSend_CheckedChanged );
            // 
            // rdoReceive
            // 
            this.rdoReceive.Location = new System.Drawing.Point( 24, 64 );
            this.rdoReceive.Name = "rdoReceive";
            this.rdoReceive.Size = new System.Drawing.Size( 144, 16 );
            this.rdoReceive.TabIndex = 30;
            this.rdoReceive.Text = "Receive Query";
            this.rdoReceive.CheckedChanged += new System.EventHandler( this.rdoReceive_CheckedChanged );
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point( 504, 128 );
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size( 32, 24 );
            this.btnConnection.TabIndex = 29;
            this.btnConnection.Text = "...";
            this.btnConnection.Click += new System.EventHandler( this.btnConnection_Click );
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point( 200, 112 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 200, 16 );
            this.label9.TabIndex = 28;
            this.label9.Text = "Connection String:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point( 200, 128 );
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size( 304, 21 );
            this.txtConnectionString.TabIndex = 27;
            // 
            // btnExecuteQuery
            // 
            this.btnExecuteQuery.Location = new System.Drawing.Point( 568, 24 );
            this.btnExecuteQuery.Name = "btnExecuteQuery";
            this.btnExecuteQuery.Size = new System.Drawing.Size( 136, 32 );
            this.btnExecuteQuery.TabIndex = 26;
            this.btnExecuteQuery.Text = "Run Query";
            this.btnExecuteQuery.Click += new System.EventHandler( this.btnExecuteQuery_Click );
            // 
            // btnInputInst
            // 
            this.btnInputInst.Location = new System.Drawing.Point( 504, 80 );
            this.btnInputInst.Name = "btnInputInst";
            this.btnInputInst.Size = new System.Drawing.Size( 32, 24 );
            this.btnInputInst.TabIndex = 25;
            this.btnInputInst.Text = "...";
            this.btnInputInst.Click += new System.EventHandler( this.btnInputInst_Click );
            // 
            // btnSchemaName
            // 
            this.btnSchemaName.Location = new System.Drawing.Point( 504, 32 );
            this.btnSchemaName.Name = "btnSchemaName";
            this.btnSchemaName.Size = new System.Drawing.Size( 32, 24 );
            this.btnSchemaName.TabIndex = 24;
            this.btnSchemaName.Text = "...";
            this.btnSchemaName.Click += new System.EventHandler( this.btnSchemaName_Click );
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point( 200, 64 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 178, 16 );
            this.label6.TabIndex = 23;
            this.label6.Text = "Input Instance Name:";
            // 
            // txtInputInstanceName
            // 
            this.txtInputInstanceName.Location = new System.Drawing.Point( 200, 80 );
            this.txtInputInstanceName.Name = "txtInputInstanceName";
            this.txtInputInstanceName.Size = new System.Drawing.Size( 304, 21 );
            this.txtInputInstanceName.TabIndex = 22;
            // 
            // lblSchemaName
            // 
            this.lblSchemaName.Location = new System.Drawing.Point( 200, 16 );
            this.lblSchemaName.Name = "lblSchemaName";
            this.lblSchemaName.Size = new System.Drawing.Size( 151, 16 );
            this.lblSchemaName.TabIndex = 21;
            this.lblSchemaName.Text = "Input Schema Name:";
            // 
            // txtInputSchemaLocation
            // 
            this.txtInputSchemaLocation.Location = new System.Drawing.Point( 200, 32 );
            this.txtInputSchemaLocation.Name = "txtInputSchemaLocation";
            this.txtInputSchemaLocation.Size = new System.Drawing.Size( 304, 21 );
            this.txtInputSchemaLocation.TabIndex = 20;
            // 
            // TestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.ClientSize = new System.Drawing.Size( 744, 589 );
            this.Controls.Add( this.tabControl1 );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BizTalk ODBC Adapter Test Harness";
            this.tabControl1.ResumeLayout( false );
            this.tpSchemaWizard.ResumeLayout( false );
            this.tpSchemaWizard.PerformLayout( );
            this.tpQueryEngine.ResumeLayout( false );
            this.tpQueryEngine.PerformLayout( );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout( );
            this.ResumeLayout( false );

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( )
        {
            Application.Run( new TestForm( ) );
        }

        private void button2_Click( object sender, System.EventArgs e )
        {
            ODBCAdapterWizardForm x = new ODBCAdapterWizardForm( );
            x.Start( );
            x.ShowDialog( this );
            txtInputSchema.Text = x.strInputSchema;
            txtOutputSchema.Text = x.strOutputSchema;
            txtSQLStatement.Text = x.strGeneratedScript;

            BTSODBCQueryEngine queryEngine = new BTSODBCQueryEngine( );
            queryEngine.ConnectionString = x.strDBConnection;

            queryEngine.RootNode = x.strOutputRoot;
            queryEngine.BeginTransaction( System.Data.IsolationLevel.ReadUncommitted );

            queryEngine.BTSExecuteODBCCallFromSQL( x.strGeneratedScript );

            queryEngine.Commit( );
        }

        private void btnSchemaName_Click( object sender, System.EventArgs e )
        {
            digOpenFile.CheckFileExists = true;
            digOpenFile.Filter = "Schema file (*.xsd)|*.xsd";
            DialogResult result = digOpenFile.ShowDialog( this );

            if ( result == DialogResult.OK )
            {
                txtInputSchemaLocation.Text = digOpenFile.FileName;
            }
        }

        private void btnExecuteQuery_Click( object sender, System.EventArgs e )
        {
            //Validate a few things
            if ( txtInputSchemaLocation.Text == "" )
            {
                MessageBox.Show( "You must select a schema!", "ODBC Adapter Test Harness" );
                return;
            }
            else if ( txtConnectionString.Text == "" )
            {
                MessageBox.Show( "You must select or enter and ODBC connection string!", "ODBC Adapter Test Harness" );
                return;
            }

            if ( rdoSend.Checked )
            {
                if ( txtInputInstanceName.Text == "" )
                {
                    MessageBox.Show( "You must select a XML input document that matches your selected schema", "ODBC Adapter Test Harness" );
                    return;
                }
            }

            try
            {
                BTSODBCQueryEngine QueryEngine = new BTSODBCQueryEngine( );
                QueryEngine.ConnectionString = txtConnectionString.Text;

                XmlDocument xmlSchema = new XmlDocument( );
                xmlSchema.Load( txtInputSchemaLocation.Text );

                int iterations = System.Convert.ToInt32( txtIterations.Text );

                if ( rdoReceive.Checked )
                {
                    StringReader sr = new StringReader( xmlSchema.OuterXml );
                    XmlSchema xsdQueryConfiguration = XmlSchema.Read( sr, null );

                    for ( int cnt = 0; cnt < iterations; cnt++ )
                    {
                        QueryEngine.RootNode = GetFirstStatement( xsdQueryConfiguration, "ResponseRootName" );
                        QueryEngine.Namespace = GetFirstStatement( xsdQueryConfiguration, "ResponseNS" ) + QueryEngine.RootNode;

                        XmlTextReader xtr = new XmlTextReader( sr );

                        Stream str = ( Stream )QueryEngine.BTSExecuteODBCCallFromSQL( GetFirstStatement( xsdQueryConfiguration, "ODBCCMD" ) );
                        str.Seek( 0, SeekOrigin.Begin );

                        StreamReader stReader = new StreamReader( str );
                        txtQueryResults.Text = stReader.ReadToEnd( );
                    }
                }
                else
                {
                    StreamReader sr = new StreamReader( txtInputInstanceName.Text );

                    for ( int cnt = 0; cnt < iterations; cnt++ )
                    {

                        XmlTextReader xtr = new XmlTextReader( sr );


                        Stream str = ( Stream )QueryEngine.BTSExecuteODBCCallFromSchema( xtr, xmlSchema.OuterXml );
                        str.Seek( 0, SeekOrigin.Begin );

                        StreamReader stReader = new StreamReader( str );
                        txtQueryResults.Text = stReader.ReadToEnd( );

                    }

                    sr.Close( );
                }

                if ( txtQueryResults.Text == "" )
                    txtQueryResults.Text = "No Results were Returned";
                else
                    MessageBox.Show( "Query Processing Complete!" );

            }
            catch ( Exception testex )
            {
                MessageBox.Show( "Your tested failed: " + testex.Message + " " + testex.InnerException, "ODBC Adapter Test Harness" );
            }
        }

        private void btnInputInst_Click( object sender, System.EventArgs e )
        {
            digOpenFile.CheckFileExists = true;
            digOpenFile.Filter = "Schema file (*.xml)|*.xml";
            DialogResult result = digOpenFile.ShowDialog( this );

            if ( result == DialogResult.OK )
            {
                txtInputInstanceName.Text = digOpenFile.FileName;
            }
        }

        private void btnConnection_Click( object sender, System.EventArgs e )
        {
            ODBCDriverUI diODBCConnection = new ODBCDriverUI( );
            txtConnectionString.Text = diODBCConnection.GetDSN( );
        }

        private void rdoReceive_CheckedChanged( object sender, System.EventArgs e )
        {
            lblSchemaName.Text = "Receive Schema Name";
            txtInputInstanceName.Enabled = false;
            btnInputInst.Enabled = false;
        }

        private void rdoSend_CheckedChanged( object sender, System.EventArgs e )
        {
            lblSchemaName.Text = "Input Schema Name:";
            txtInputInstanceName.Enabled = true;
            btnInputInst.Enabled = true;
        }

        private string GetFirstStatement( XmlSchemaObject parent, string LocalName )
        {
            if ( parent is XmlSchemaAppInfo )
            {
                foreach ( XmlNode child in ( ( XmlSchemaAppInfo )parent ).Markup )
                {
                    if ( child.LocalName == LocalName )
                    {
                        XmlAttribute att = child.Attributes[ "value" ];
                        if ( null != child.InnerText )
                            return child.InnerText;
                    }
                }
                return null;
            }
            else if ( parent is XmlSchema )
                foreach ( XmlSchemaObject child in ( ( XmlSchema )parent ).Items )
                {
                    string result = GetFirstStatement( child, LocalName );
                    if ( result != null )
                        return result;
                }
            else if ( parent is XmlSchemaAnnotation )
                foreach ( XmlSchemaObject child in ( ( XmlSchemaAnnotation )parent ).Items )
                {
                    string result = GetFirstStatement( child, LocalName );
                    if ( result != null )
                        return result;
                }
            return null;
        }

        private void button1_Click( object sender, System.EventArgs e )
        {
            try
            {
                ODBCAdapterWizardForm x = new ODBCAdapterWizardForm( );
                x.Start( );
                x.ShowDialog( this );
                txtInputSchema.Text = x.strInputSchema;
                txtOutputSchema.Text = x.strOutputSchema;
                txtSQLStatement.Text = x.strGeneratedScript;
            }
            catch ( Exception x )
            {
                MessageBox.Show( x.Message );
            }
        }
    }
}