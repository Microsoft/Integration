using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ODBCDriverPrompt
{
    public class PasswordDialog : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.TextBox txtUserID;
        internal System.Windows.Forms.TextBox txtConfirmPwd;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.TextBox txtPassword;

        private System.ComponentModel.Container components = null;

        public string UserID = "";
        public string Password = "";

        public PasswordDialog( )
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
            this.Label3 = new System.Windows.Forms.Label( );
            this.Label2 = new System.Windows.Forms.Label( );
            this.Label1 = new System.Windows.Forms.Label( );
            this.cmdCancel = new System.Windows.Forms.Button( );
            this.txtUserID = new System.Windows.Forms.TextBox( );
            this.txtConfirmPwd = new System.Windows.Forms.TextBox( );
            this.cmdOK = new System.Windows.Forms.Button( );
            this.txtPassword = new System.Windows.Forms.TextBox( );
            this.SuspendLayout( );
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point( 8, 8 );
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size( 168, 16 );
            this.Label3.TabIndex = 0;
            this.Label3.Text = "User ID";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point( 8, 104 );
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size( 144, 16 );
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Confirm Password";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point( 8, 56 );
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size( 152, 16 );
            this.Label1.TabIndex = 2;
            this.Label1.Text = "Password";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point( 232, 56 );
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size( 80, 24 );
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler( this.cmdCancel_Click );
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point( 8, 24 );
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size( 208, 21 );
            this.txtUserID.TabIndex = 1;
            // 
            // txtConfirmPwd
            // 
            this.txtConfirmPwd.Location = new System.Drawing.Point( 8, 120 );
            this.txtConfirmPwd.Name = "txtConfirmPwd";
            this.txtConfirmPwd.PasswordChar = '*';
            this.txtConfirmPwd.Size = new System.Drawing.Size( 208, 21 );
            this.txtConfirmPwd.TabIndex = 5;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point( 232, 24 );
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size( 80, 23 );
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point( 8, 72 );
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size( 208, 21 );
            this.txtPassword.TabIndex = 3;
            // 
            // PasswordDialog
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size( 324, 153 );
            this.ControlBox = false;
            this.Controls.Add( this.Label3 );
            this.Controls.Add( this.Label2 );
            this.Controls.Add( this.Label1 );
            this.Controls.Add( this.cmdCancel );
            this.Controls.Add( this.txtUserID );
            this.Controls.Add( this.txtConfirmPwd );
            this.Controls.Add( this.txtPassword );
            this.Controls.Add( this.cmdOK );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password Dialog";
            this.ResumeLayout( false );
            this.PerformLayout( );

        }
        #endregion

        private void cmdOK_Click( object sender, System.EventArgs e )
        {
            if ( txtPassword.Text == txtConfirmPwd.Text )
            {
                this.Password = txtPassword.Text.ToString( );
                this.UserID = txtUserID.Text;

                txtPassword.Text = "";
                txtConfirmPwd.Text = "";
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close( );
            }
            else
            {
                MessageBox.Show( "Passwords do not match please try again" );
            }
        }

        private void cmdCancel_Click( object sender, System.EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close( );
        }
    }
}