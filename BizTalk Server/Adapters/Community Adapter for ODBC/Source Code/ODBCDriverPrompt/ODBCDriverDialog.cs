using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ODBCDriverPrompt
{
    public class ODBCDriverDialog : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.CheckBox chkIncludeUID;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button cmdNewDSN;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.ListBox lstDSNs;

        private System.ComponentModel.Container components = null;
        
        private string mConnectionString = null;

        public ODBCDriverDialog( )
        {
            Application.EnableVisualStyles( );
            
            InitializeComponent( );
        }

        public string ConnectionString
        {
            get
            {
                return mConnectionString;
            }
            set
            {
                mConnectionString = value;
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
            this.cmdCancel = new System.Windows.Forms.Button( );
            this.chkIncludeUID = new System.Windows.Forms.CheckBox( );
            this.Label1 = new System.Windows.Forms.Label( );
            this.cmdNewDSN = new System.Windows.Forms.Button( );
            this.cmdOK = new System.Windows.Forms.Button( );
            this.lstDSNs = new System.Windows.Forms.ListBox( );
            this.SuspendLayout( );
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point( 308, 64 );
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size( 104, 24 );
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler( this.cmdCancel_Click );
            // 
            // chkIncludeUID
            // 
            this.chkIncludeUID.Checked = true;
            this.chkIncludeUID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeUID.Location = new System.Drawing.Point( 8, 185 );
            this.chkIncludeUID.Name = "chkIncludeUID";
            this.chkIncludeUID.Size = new System.Drawing.Size( 288, 16 );
            this.chkIncludeUID.TabIndex = 10;
            this.chkIncludeUID.Text = "Include UserID and Password";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point( 8, 8 );
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size( 408, 16 );
            this.Label1.TabIndex = 9;
            this.Label1.Text = "Please select a data source name";
            // 
            // cmdNewDSN
            // 
            this.cmdNewDSN.Location = new System.Drawing.Point( 308, 155 );
            this.cmdNewDSN.Name = "cmdNewDSN";
            this.cmdNewDSN.Size = new System.Drawing.Size( 104, 24 );
            this.cmdNewDSN.TabIndex = 8;
            this.cmdNewDSN.Text = "New DSN...";
            this.cmdNewDSN.Click += new System.EventHandler( this.cmdNewDSN_Click );
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point( 308, 32 );
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size( 104, 24 );
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
            // 
            // lstDSNs
            // 
            this.lstDSNs.Location = new System.Drawing.Point( 8, 32 );
            this.lstDSNs.Name = "lstDSNs";
            this.lstDSNs.Size = new System.Drawing.Size( 288, 147 );
            this.lstDSNs.TabIndex = 6;
            // 
            // ODBCDriverDialog
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size( 424, 206 );
            this.Controls.Add( this.cmdCancel );
            this.Controls.Add( this.chkIncludeUID );
            this.Controls.Add( this.Label1 );
            this.Controls.Add( this.cmdNewDSN );
            this.Controls.Add( this.cmdOK );
            this.Controls.Add( this.lstDSNs );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ODBCDriverDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Data Source";
            this.Load += new System.EventHandler( this.ODBCDriverDialog_Load );
            this.ResumeLayout( false );

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( )
        {
            Application.Run( new ODBCDriverDialog( ) );
        }

        private void cmdOK_Click( object sender, System.EventArgs e )
        {
            if ( lstDSNs.SelectedIndex == -1 )
            {
                MessageBox.Show( "Please select a database connection", "ODBC Driver Selection" );
            }
            else
            {
                if ( chkIncludeUID.Checked )
                {
                    PasswordDialog frmCredentials = new PasswordDialog( );
                    if ( frmCredentials.ShowDialog( ) == DialogResult.OK )
                    {
                        this.ConnectionString = "DSN=" + lstDSNs.SelectedItem + ";UID=" + frmCredentials.UserID + ";PWD=" + frmCredentials.Password;
                        this.DialogResult = DialogResult.OK;
                        this.Close( );
                    }
                    else
                    {
                        this.ConnectionString = "DSN=" + lstDSNs.SelectedItem;
                        this.DialogResult = DialogResult.OK;
                        this.Close( );
                    }
                }
                else
                {
                    this.ConnectionString = "DSN=" + lstDSNs.SelectedItem;
                    this.DialogResult = DialogResult.OK;
                    this.Close( );
                }
            }

        }

        private void cmdCancel_Click( object sender, System.EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close( );
        }

        private void cmdNewDSN_Click( object sender, System.EventArgs e )
        {
            //'-----------------------------------------------------------------------------------
            //'DESCRIPTION    :   Opens up system odbc dialog box, e.g: C:\Windows\system32\odbcad32.exe
            //'                   Crude but works!
            //'-----------------------------------------------------------------------------------

            string strErMsg = "Failed to open system ODBC dialogue box";
            string strPath = Environment.SystemDirectory + "\\odbcad32.exe";
            //Process proDSN;
            //Dim ipHandle As IntPtr

            try
            {
                //'open up the system dsn dialog and wait until it exits:
                System.Diagnostics.Process.Start( strPath ).WaitForExit( );
                LoadList( );
            }
            catch ( Exception ex )
            {
                //'possible exceptions include security violation or bust windows box:
                MessageBox.Show( "Error was: " + ex.Message, strErMsg, MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        private void ODBCDriverDialog_Load( object sender, System.EventArgs e )
        {
            LoadList( );
        }

        private void LoadList( )
        {
            ODBCHelper objODBCHelper = new ODBCHelper( );
            ArrayList strDSNs;

            try
            {
                //Get a new list of dsn instances
                strDSNs = objODBCHelper.GetDSNList( );

                lstDSNs.Items.Clear( );

                if ( strDSNs != null && strDSNs.Count != 0 )
                {
                    foreach ( string dsn in strDSNs )
                    {
                        lstDSNs.Items.Add( dsn.ToString( ) );
                    }
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "Unexpected Error Loading DSN List", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }
    }
}