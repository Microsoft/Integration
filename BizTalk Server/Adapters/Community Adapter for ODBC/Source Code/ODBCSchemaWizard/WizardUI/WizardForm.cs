//----------------------------------------------------------------------
//
// WizardForm.cs
//
// Microsoft BizTalk Server
// Copyright (c) 1999-2002 Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// THE ENTIRE RISK OF USE OR RESULTS IN CONNECTION WITH THE USE OF THIS CODE
// AND INFORMATION REMAINS WITH THE USER.
//
//----------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class WizardForm : System.Windows.Forms.Form
    {
        protected System.Windows.Forms.Panel panelWizard;
        private System.Windows.Forms.GroupBox groupBoxFooter;
        protected WizardUserControl _userControl = null;
        private System.Windows.Forms.Panel panelFooter;
        protected System.Windows.Forms.Button buttonHelp;
        protected System.Windows.Forms.Button buttonBack;
        protected System.Windows.Forms.Button buttonNext;
        protected System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.PictureBox picGraphic;
        protected System.Windows.Forms.Label lblTitle;
        protected System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.GroupBox groupBox1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public WizardForm( )
        {
            Application.EnableVisualStyles( );

            InitializeComponent( );

            //this.buttonHelp.Text = ODBCResourceHandler.GetResourceString("FormHelpButton");
            //this.buttonBack.Text = ODBCResourceHandler.GetResourceString("FormBackButton");
            //this.buttonNext.Text = ODBCResourceHandler.GetResourceString("FormNextButton");
            //this.buttonCancel.Text = ODBCResourceHandler.GetResourceString("FormCancelButton");
            //this.Text = ODBCResourceHandler.GetResourceString("FormTitle");
        }

        public bool BackButtonEnabled
        {
            get
            {
                return this.buttonBack.Enabled;
            }
            set
            {
                this.buttonBack.Enabled = value;
            }
        }

        public bool CancelButtonEnabled
        {
            get
            {
                return this.buttonCancel.Enabled;
            }
            set
            {
                this.buttonCancel.Enabled = value;
            }
        }

        public bool NextButtonEnabled
        {
            get
            {
                return this.buttonNext.Enabled;
            }
            set
            {
                this.buttonNext.Enabled = value;
            }
        }

        public string NextButtonText
        {
            get
            {
                return this.buttonNext.Text;
            }
            set
            {
                this.buttonNext.Text = value;
            }
        }

        public void PerformNextClick( )
        {
            this.buttonNext.PerformClick( );
        }

        protected virtual void SetNewPageProperties( WizardUserControl newUserControl )
        {
            SuspendLayout( );
            _userControl = newUserControl;

            newUserControl.Visible = true;
            newUserControl.Anchor = AnchorStyles.Left;
            newUserControl.Dock = DockStyle.Fill;

            //Force the UserControl to resize -- some of the user controls don't resize correctly.
            float scaleRatioX = ( ( float )panelWizard.Width ) / ( ( float )newUserControl.Width );
            float scaleRatioY = ( ( float )panelWizard.Height ) / ( ( float )newUserControl.Height );
            SizeF factor = new SizeF( scaleRatioX, scaleRatioY );
            newUserControl.Scale( factor );

            if ( !panelWizard.Controls.Contains( newUserControl ) )
                panelWizard.Controls.Add( ( UserControl )newUserControl );

            foreach ( UserControl ctrl in panelWizard.Controls )
            {
                if ( ctrl == newUserControl )
                    ctrl.Visible = true;
                else
                    ctrl.Visible = false;
            }

            ResumeLayout( false );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( WizardForm ) );
            this.panelWizard = new System.Windows.Forms.Panel( );
            this.buttonHelp = new System.Windows.Forms.Button( );
            this.buttonBack = new System.Windows.Forms.Button( );
            this.buttonNext = new System.Windows.Forms.Button( );
            this.buttonCancel = new System.Windows.Forms.Button( );
            this.panelFooter = new System.Windows.Forms.Panel( );
            this.groupBoxFooter = new System.Windows.Forms.GroupBox( );
            this.panel1 = new System.Windows.Forms.Panel( );
            this.lblScript = new System.Windows.Forms.Label( );
            this.lblTitle = new System.Windows.Forms.Label( );
            this.picGraphic = new System.Windows.Forms.PictureBox( );
            this.groupBox1 = new System.Windows.Forms.GroupBox( );
            this.panelFooter.SuspendLayout( );
            this.panel1.SuspendLayout( );
            ( ( System.ComponentModel.ISupportInitialize )( this.picGraphic ) ).BeginInit( );
            this.SuspendLayout( );
            // 
            // panelWizard
            // 
            this.panelWizard.Location = new System.Drawing.Point( 0, 56 );
            this.panelWizard.Name = "panelWizard";
            this.panelWizard.Size = new System.Drawing.Size( 587, 345 );
            this.panelWizard.TabIndex = 0;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point( 8, 8 );
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size( 96, 24 );
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.Visible = false;
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonBack.Location = new System.Drawing.Point( 273, 8 );
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size( 96, 24 );
            this.buttonBack.TabIndex = 4;
            this.buttonBack.Click += new System.EventHandler( this.OnBack );
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonNext.Location = new System.Drawing.Point( 377, 8 );
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size( 96, 24 );
            this.buttonNext.TabIndex = 5;
            this.buttonNext.Click += new System.EventHandler( this.OnNext );
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point( 481, 8 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size( 96, 24 );
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Click += new System.EventHandler( this.OnCancel );
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add( this.buttonHelp );
            this.panelFooter.Controls.Add( this.buttonBack );
            this.panelFooter.Controls.Add( this.buttonNext );
            this.panelFooter.Controls.Add( this.buttonCancel );
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point( 0, 401 );
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size( 585, 40 );
            this.panelFooter.TabIndex = 2;
            // 
            // groupBoxFooter
            // 
            this.groupBoxFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxFooter.Location = new System.Drawing.Point( 0, 399 );
            this.groupBoxFooter.Name = "groupBoxFooter";
            this.groupBoxFooter.Size = new System.Drawing.Size( 585, 2 );
            this.groupBoxFooter.TabIndex = 1;
            this.groupBoxFooter.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.lblScript );
            this.panel1.Controls.Add( this.lblTitle );
            this.panel1.Controls.Add( this.picGraphic );
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 587, 59 );
            this.panel1.TabIndex = 5;
            // 
            // lblScript
            // 
            this.lblScript.Location = new System.Drawing.Point( 16, 24 );
            this.lblScript.Name = "lblScript";
            this.lblScript.Size = new System.Drawing.Size( 400, 32 );
            this.lblScript.TabIndex = 2;
            this.lblScript.Text = "lblScript";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.lblTitle.Location = new System.Drawing.Point( 8, 8 );
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size( 408, 16 );
            this.lblTitle.TabIndex = 1;
            // 
            // picGraphic
            // 
            this.picGraphic.Image = ( ( System.Drawing.Image )( resources.GetObject( "picGraphic.Image" ) ) );
            this.picGraphic.Location = new System.Drawing.Point( 528, 6 );
            this.picGraphic.Name = "picGraphic";
            this.picGraphic.Size = new System.Drawing.Size( 56, 47 );
            this.picGraphic.TabIndex = 0;
            this.picGraphic.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point( 0, 56 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 587, 3 );
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // WizardForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 14 );
            this.ClientSize = new System.Drawing.Size( 585, 441 );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.panelWizard );
            this.Controls.Add( this.groupBoxFooter );
            this.Controls.Add( this.panelFooter );
            this.Controls.Add( this.panel1 );
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ODBC Transport Schema Generation";
            this.panelFooter.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            ( ( System.ComponentModel.ISupportInitialize )( this.picGraphic ) ).EndInit( );
            this.ResumeLayout( false );

        }
        #endregion

        protected virtual void OnCancel( object sender, System.EventArgs e )
        {
            //((WizardUserControl)userControl).CleanUpOnCancel();

            this.DialogResult = DialogResult.Cancel;
            this.Close( );
            return;
        }

        protected virtual void OnNext( object sender, System.EventArgs e ) { }

        protected virtual void OnBack( object sender, System.EventArgs e ) { }
    }
}