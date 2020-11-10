//----------------------------------------------------------------------
//
// WizardUserControl.cs
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class WizardUserControl : System.Windows.Forms.UserControl
    {
        private WizardForm parentForm = null;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public WizardUserControl( )
        {
            InitializeComponent( );
        }

        protected WizardForm WizardParentForm
        {
            get
            {
                if ( parentForm == null )
                    parentForm = ( WizardForm )this.FindForm( );
                return parentForm;
            }
        }

        public virtual void ProcessLoad( )
        {
            LoadPageInfo( );
            UpdateNextBackButton( );
        }

        public virtual bool SavePageInfo( )
        {
            if ( !ValidatePageInput( ) )
                return false;

            return SavePageInfoWithoutValidation( );
        }

        public virtual bool SavePageInfoWithoutValidation( )
        {
            return true;
        }

        public virtual void CleanUpOnCancel( )
        {
        }

        protected virtual bool ValidatePageInput( )
        {
            return true;
        }

        protected virtual void LoadPageInfo( )
        {
        }

        protected virtual void UpdateNextBackButton( )
        {
        }

        public virtual void SetSelect( )
        {
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
            this.SuspendLayout( );
            // 
            // WizardUserControl
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.Name = "WizardUserControl";
            this.Size = new System.Drawing.Size( 497, 318 );
            this.ResumeLayout( false );

        }
        #endregion
    }
}