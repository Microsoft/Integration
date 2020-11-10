using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class ADOCommandOverride : System.Windows.Forms.Form
    {
        private ListViewItem _lvSelectedItem;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.Button cmdApplyADO;

        //Vars used to communicate with the parent class ODBCSchemaHelper
        public System.Data.Odbc.OdbcCommand ADOCommand;
        public ODBCSchemaHelper ParentClass = null;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSampleData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDirection;
        private System.Windows.Forms.ComboBox cboDataType;
        private System.Windows.Forms.TextBox txtParamName;
        private System.Windows.Forms.ListView lvQueryParameters;
        private System.Windows.Forms.ColumnHeader Parameter;
        public System.Windows.Forms.ColumnHeader DataType;
        private System.Windows.Forms.ColumnHeader Direction;
        private System.Windows.Forms.ColumnHeader SampleData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtReqSchema;
        private System.Windows.Forms.TextBox txtRespSchema;
        private System.Windows.Forms.TextBox txtQueryResults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader SizeCol;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSize;

        private System.ComponentModel.Container components = null;

        public ADOCommandOverride( )
        {
            Application.EnableVisualStyles( );

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
            this.cmdClose = new System.Windows.Forms.Button( );
            this.cmdTest = new System.Windows.Forms.Button( );
            this.cmdApplyADO = new System.Windows.Forms.Button( );
            this.groupBox1 = new System.Windows.Forms.GroupBox( );
            this.txtSize = new System.Windows.Forms.TextBox( );
            this.label8 = new System.Windows.Forms.Label( );
            this.txtSampleData = new System.Windows.Forms.TextBox( );
            this.label4 = new System.Windows.Forms.Label( );
            this.cmdApply = new System.Windows.Forms.Button( );
            this.label3 = new System.Windows.Forms.Label( );
            this.label2 = new System.Windows.Forms.Label( );
            this.label1 = new System.Windows.Forms.Label( );
            this.cboDirection = new System.Windows.Forms.ComboBox( );
            this.cboDataType = new System.Windows.Forms.ComboBox( );
            this.txtParamName = new System.Windows.Forms.TextBox( );
            this.lvQueryParameters = new System.Windows.Forms.ListView( );
            this.Parameter = new System.Windows.Forms.ColumnHeader( );
            this.DataType = new System.Windows.Forms.ColumnHeader( );
            this.SizeCol = new System.Windows.Forms.ColumnHeader( );
            this.Direction = new System.Windows.Forms.ColumnHeader( );
            this.SampleData = new System.Windows.Forms.ColumnHeader( );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.label7 = new System.Windows.Forms.Label( );
            this.label6 = new System.Windows.Forms.Label( );
            this.label5 = new System.Windows.Forms.Label( );
            this.txtQueryResults = new System.Windows.Forms.TextBox( );
            this.txtRespSchema = new System.Windows.Forms.TextBox( );
            this.txtReqSchema = new System.Windows.Forms.TextBox( );
            this.groupBox1.SuspendLayout( );
            this.groupBox2.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point( 502, 76 );
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size( 104, 24 );
            this.cmdClose.TabIndex = 10;
            this.cmdClose.Text = "Close";
            this.cmdClose.Click += new System.EventHandler( this.cmdClose_Click );
            // 
            // cmdTest
            // 
            this.cmdTest.Location = new System.Drawing.Point( 502, 12 );
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size( 104, 24 );
            this.cmdTest.TabIndex = 11;
            this.cmdTest.Text = "Test Query";
            this.cmdTest.Click += new System.EventHandler( this.cmdTest_Click );
            // 
            // cmdApplyADO
            // 
            this.cmdApplyADO.AccessibleName = "";
            this.cmdApplyADO.Location = new System.Drawing.Point( 502, 44 );
            this.cmdApplyADO.Name = "cmdApplyADO";
            this.cmdApplyADO.Size = new System.Drawing.Size( 104, 24 );
            this.cmdApplyADO.TabIndex = 12;
            this.cmdApplyADO.Text = "Update Command";
            this.cmdApplyADO.Click += new System.EventHandler( this.cmdApplyADO_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.txtSize );
            this.groupBox1.Controls.Add( this.label8 );
            this.groupBox1.Controls.Add( this.txtSampleData );
            this.groupBox1.Controls.Add( this.label4 );
            this.groupBox1.Controls.Add( this.cmdApply );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.cboDirection );
            this.groupBox1.Controls.Add( this.cboDataType );
            this.groupBox1.Controls.Add( this.txtParamName );
            this.groupBox1.Controls.Add( this.lvQueryParameters );
            this.groupBox1.Location = new System.Drawing.Point( 8, 10 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 488, 224 );
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ADO.NET Command Parameters";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point( 160, 192 );
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size( 56, 21 );
            this.txtSize.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point( 160, 176 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 48, 16 );
            this.label8.TabIndex = 20;
            this.label8.Text = "Size";
            // 
            // txtSampleData
            // 
            this.txtSampleData.Location = new System.Drawing.Point( 176, 144 );
            this.txtSampleData.Name = "txtSampleData";
            this.txtSampleData.Size = new System.Drawing.Size( 200, 21 );
            this.txtSampleData.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 176, 128 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 104, 16 );
            this.label4.TabIndex = 18;
            this.label4.Text = "Sample Data";
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point( 392, 144 );
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size( 80, 24 );
            this.cmdApply.TabIndex = 17;
            this.cmdApply.Text = "Apply";
            this.cmdApply.Click += new System.EventHandler( this.cmdApply_Click );
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 264, 176 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 104, 16 );
            this.label3.TabIndex = 16;
            this.label3.Text = "Direction";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 4, 174 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 104, 16 );
            this.label2.TabIndex = 15;
            this.label2.Text = "Data Type:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 4, 126 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 128, 16 );
            this.label1.TabIndex = 14;
            this.label1.Text = "Parameter Name:";
            // 
            // cboDirection
            // 
            this.cboDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDirection.Items.AddRange( new object[ ] {
            "ReturnValue",
            "Input",
            "Output",
            "InputOutput"} );
            this.cboDirection.Location = new System.Drawing.Point( 264, 192 );
            this.cboDirection.Name = "cboDirection";
            this.cboDirection.Size = new System.Drawing.Size( 112, 21 );
            this.cboDirection.TabIndex = 13;
            // 
            // cboDataType
            // 
            this.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataType.Items.AddRange( new object[ ] {
            "BigInt",
            "Binary",
            "Bit",
            "Text",
            "DateTime",
            "Numeric",
            "Double",
            "Int",
            "Real",
            "SmallInt",
            "TinyInt",
            "UniqueIdentifier",
            "Char",
            "Date",
            "Decimal",
            "Image",
            "NChar",
            "NText",
            "NVarChar",
            "SmallDateTime",
            "Time",
            "Timestamp",
            "VarBinary",
            "VarChar"} );
            this.cboDataType.Location = new System.Drawing.Point( 4, 192 );
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size( 132, 21 );
            this.cboDataType.TabIndex = 12;
            // 
            // txtParamName
            // 
            this.txtParamName.Location = new System.Drawing.Point( 4, 144 );
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size( 160, 21 );
            this.txtParamName.TabIndex = 11;
            // 
            // lvQueryParameters
            // 
            this.lvQueryParameters.Columns.AddRange( new System.Windows.Forms.ColumnHeader[ ] {
            this.Parameter,
            this.DataType,
            this.SizeCol,
            this.Direction,
            this.SampleData} );
            this.lvQueryParameters.FullRowSelect = true;
            this.lvQueryParameters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvQueryParameters.LabelEdit = true;
            this.lvQueryParameters.Location = new System.Drawing.Point( 4, 22 );
            this.lvQueryParameters.MultiSelect = false;
            this.lvQueryParameters.Name = "lvQueryParameters";
            this.lvQueryParameters.Size = new System.Drawing.Size( 476, 88 );
            this.lvQueryParameters.TabIndex = 10;
            this.lvQueryParameters.UseCompatibleStateImageBehavior = false;
            this.lvQueryParameters.View = System.Windows.Forms.View.Details;
            this.lvQueryParameters.SelectedIndexChanged += new System.EventHandler( this.lvQueryParameters_SelectedIndexChanged );
            // 
            // Parameter
            // 
            this.Parameter.Text = "Parameter";
            this.Parameter.Width = 150;
            // 
            // DataType
            // 
            this.DataType.Text = "Data Type";
            this.DataType.Width = 80;
            // 
            // SizeCol
            // 
            this.SizeCol.Text = "Size";
            this.SizeCol.Width = 40;
            // 
            // Direction
            // 
            this.Direction.Text = "Direction";
            this.Direction.Width = 80;
            // 
            // SampleData
            // 
            this.SampleData.Text = "Sample Data";
            this.SampleData.Width = 120;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.label7 );
            this.groupBox2.Controls.Add( this.label6 );
            this.groupBox2.Controls.Add( this.label5 );
            this.groupBox2.Controls.Add( this.txtQueryResults );
            this.groupBox2.Controls.Add( this.txtRespSchema );
            this.groupBox2.Controls.Add( this.txtReqSchema );
            this.groupBox2.Location = new System.Drawing.Point( 8, 240 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 568, 240 );
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Results";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point( 384, 16 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 112, 16 );
            this.label7.TabIndex = 5;
            this.label7.Text = "Query Results";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point( 192, 16 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 112, 16 );
            this.label6.TabIndex = 4;
            this.label6.Text = "Output Schema";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 8, 16 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 128, 16 );
            this.label5.TabIndex = 3;
            this.label5.Text = "Input Schema";
            // 
            // txtQueryResults
            // 
            this.txtQueryResults.Location = new System.Drawing.Point( 376, 32 );
            this.txtQueryResults.Multiline = true;
            this.txtQueryResults.Name = "txtQueryResults";
            this.txtQueryResults.ReadOnly = true;
            this.txtQueryResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQueryResults.Size = new System.Drawing.Size( 184, 200 );
            this.txtQueryResults.TabIndex = 2;
            // 
            // txtRespSchema
            // 
            this.txtRespSchema.Location = new System.Drawing.Point( 192, 32 );
            this.txtRespSchema.Multiline = true;
            this.txtRespSchema.Name = "txtRespSchema";
            this.txtRespSchema.ReadOnly = true;
            this.txtRespSchema.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRespSchema.Size = new System.Drawing.Size( 184, 200 );
            this.txtRespSchema.TabIndex = 1;
            // 
            // txtReqSchema
            // 
            this.txtReqSchema.Location = new System.Drawing.Point( 8, 32 );
            this.txtReqSchema.Multiline = true;
            this.txtReqSchema.Name = "txtReqSchema";
            this.txtReqSchema.ReadOnly = true;
            this.txtReqSchema.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReqSchema.Size = new System.Drawing.Size( 184, 200 );
            this.txtReqSchema.TabIndex = 0;
            // 
            // ADOCommandOverride
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.ClientSize = new System.Drawing.Size( 613, 485 );
            this.ControlBox = false;
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.cmdApplyADO );
            this.Controls.Add( this.cmdTest );
            this.Controls.Add( this.cmdClose );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADOCommandOverride";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BizTalk ODBC Adapter Query Processor Override";
            this.Load += new System.EventHandler( this.ADOCommandOverride_Load );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout( );
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
            Application.Run( new ADOCommandOverride( ) );
        }

        private void ADOCommandOverride_Load( object sender, System.EventArgs e )
        {
            foreach ( System.Data.Odbc.OdbcParameter param in ADOCommand.Parameters )
            {
                ListViewItem lvItem = lvQueryParameters.Items.Add( param.ParameterName );
                lvItem.SubItems.Add( param.OdbcType.ToString( ) );
                lvItem.SubItems.Add( param.Size.ToString( ) );
                lvItem.SubItems.Add( param.Direction.ToString( ) );
                lvItem.SubItems.Add( "" );
            }
        }

        private void cmdTest_Click( object sender, System.EventArgs e )
        {
            try
            {
                txtReqSchema.Text = ParentClass.ExtractODBCParametersSchema( ADOCommand );
                txtRespSchema.Text = ParentClass.GenerateOutputSchema( ADOCommand );

                // Add any output param values to the query results dialog
                txtQueryResults.Text = "";
                foreach ( System.Data.Odbc.OdbcParameter param in ADOCommand.Parameters )
                {
                    if ( param.Direction != System.Data.ParameterDirection.Input )
                    {
                        txtQueryResults.Text += param.ParameterName + ": " + param.Value.ToString( ) + "\r\n";
                    }
                }
                txtQueryResults.Text += ParentClass.QueryOutput;
            }
            catch ( Exception ex )
            {
                txtQueryResults.Text = ex.Message + " " + ex.InnerException;
            }
        }

        private void cmdClose_Click( object sender, System.EventArgs e )
        {
            this.ParentClass = null;
            this.Hide( );
        }

        private void lvQueryParameters_SelectedIndexChanged( object sender, System.EventArgs e )
        {
            if ( lvQueryParameters.SelectedItems.Count > 0 )
            {
                _lvSelectedItem = lvQueryParameters.SelectedItems[ 0 ];//  Items[lvQueryParameters.SelectedIndices[0]];
                txtParamName.Text = _lvSelectedItem.Text;
                cboDataType.SelectedIndex = cboDataType.FindString( _lvSelectedItem.SubItems[ 1 ].Text );
                cboDirection.SelectedIndex = cboDirection.FindString( _lvSelectedItem.SubItems[ 3 ].Text );//_lvSelectedItem.SubItems[2].Text;
                txtSize.Text = _lvSelectedItem.SubItems[ 2 ].Text;
                txtSampleData.Text = _lvSelectedItem.SubItems[ 4 ].Text;
            }
        }

        private void cmdApply_Click( object sender, System.EventArgs e )
        {
            _lvSelectedItem.Text = txtParamName.Text;
            _lvSelectedItem.SubItems[ 1 ].Text = cboDataType.SelectedItem.ToString( );
            _lvSelectedItem.SubItems[ 3 ].Text = cboDirection.SelectedItem.ToString( );
            _lvSelectedItem.SubItems[ 2 ].Text = txtSize.Text;
            _lvSelectedItem.SubItems[ 4 ].Text = txtSampleData.Text;
        }

        private void cmdApplyADO_Click( object sender, System.EventArgs e )
        {
            int lvcount = 0;
            foreach ( System.Data.Odbc.OdbcParameter param in ADOCommand.Parameters )
            {
                ListViewItem lvItem = lvQueryParameters.Items[ lvcount ];
                param.ParameterName = lvItem.Text;

                param.OdbcType = ( System.Data.Odbc.OdbcType )Enum.Parse( typeof( System.Data.Odbc.OdbcType ), lvItem.SubItems[ 1 ].Text, true );
                param.Direction = ( System.Data.ParameterDirection )Enum.Parse( typeof( System.Data.ParameterDirection ), lvItem.SubItems[ 3 ].Text, true );
                param.Value = lvItem.SubItems[ 4 ].Text;

                //Only modify size only if theres a value present. //WE DO NOT WANT
                //to set the value to 0 as this will really mess with the command processing
                if ( lvItem.SubItems[ 2 ].Text != "0" )
                {
                    param.Size = Convert.ToInt16( lvItem.SubItems[ 2 ].Text );
                }

                lvcount++;
                //lvItem.SubItems.Add(param.OdbcType.ToString() ); 
                //lvItem.SubItems.Add(param.Direction.ToString());
                //lvItem.SubItems.Add(""); 
            }
        }
    }
}