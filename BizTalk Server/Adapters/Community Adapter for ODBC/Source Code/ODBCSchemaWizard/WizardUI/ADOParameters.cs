using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.BizTalk.Adapter.ODBC.SchemaWizard.WizardUI
{
    public class ADOParameters : Microsoft.BizTalk.Adapters.ODBC.SchemaWizard.WizardUserControl
    {
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.ColumnHeader SizeCol;
        private System.Windows.Forms.ColumnHeader Direction;
        private System.Windows.Forms.ColumnHeader SampleData;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.IContainer components = null;

        public ADOParameters( )
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

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
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
            this.label5 = new System.Windows.Forms.Label( );
            this.SuspendLayout( );
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point( 160, 208 );
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size( 56, 21 );
            this.txtSize.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point( 160, 192 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 48, 16 );
            this.label8.TabIndex = 32;
            this.label8.Text = "Size";
            // 
            // txtSampleData
            // 
            this.txtSampleData.Location = new System.Drawing.Point( 176, 160 );
            this.txtSampleData.Name = "txtSampleData";
            this.txtSampleData.Size = new System.Drawing.Size( 200, 21 );
            this.txtSampleData.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 176, 144 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 104, 16 );
            this.label4.TabIndex = 30;
            this.label4.Text = "Sample Data";
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point( 392, 160 );
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size( 80, 24 );
            this.cmdApply.TabIndex = 29;
            this.cmdApply.Text = "Apply";
            this.cmdApply.Click += new System.EventHandler( this.cmdApply_Click );
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 264, 192 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 104, 16 );
            this.label3.TabIndex = 28;
            this.label3.Text = "Direction";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 8, 192 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 104, 16 );
            this.label2.TabIndex = 27;
            this.label2.Text = "Data Type:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 8, 144 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 128, 16 );
            this.label1.TabIndex = 26;
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
            this.cboDirection.Location = new System.Drawing.Point( 264, 208 );
            this.cboDirection.Name = "cboDirection";
            this.cboDirection.Size = new System.Drawing.Size( 112, 21 );
            this.cboDirection.TabIndex = 25;
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
            this.cboDataType.Location = new System.Drawing.Point( 8, 208 );
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size( 132, 21 );
            this.cboDataType.TabIndex = 24;
            // 
            // txtParamName
            // 
            this.txtParamName.Location = new System.Drawing.Point( 8, 160 );
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size( 160, 21 );
            this.txtParamName.TabIndex = 23;
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
            this.lvQueryParameters.Location = new System.Drawing.Point( 8, 40 );
            this.lvQueryParameters.MultiSelect = false;
            this.lvQueryParameters.Name = "lvQueryParameters";
            this.lvQueryParameters.Size = new System.Drawing.Size( 476, 88 );
            this.lvQueryParameters.TabIndex = 22;
            this.lvQueryParameters.UseCompatibleStateImageBehavior = false;
            this.lvQueryParameters.View = System.Windows.Forms.View.Details;
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
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 8, 16 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 216, 16 );
            this.label5.TabIndex = 34;
            this.label5.Text = "Over ride ADO.NET faults and Test Query";
            // 
            // ADOParameters
            // 
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.txtSize );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this.txtSampleData );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.cmdApply );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.cboDirection );
            this.Controls.Add( this.cboDataType );
            this.Controls.Add( this.txtParamName );
            this.Controls.Add( this.lvQueryParameters );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "ADOParameters";
            this.ResumeLayout( false );
            this.PerformLayout( );

        }
        #endregion

        private void cmdApply_Click( object sender, System.EventArgs e )
        {
       }
    }
}