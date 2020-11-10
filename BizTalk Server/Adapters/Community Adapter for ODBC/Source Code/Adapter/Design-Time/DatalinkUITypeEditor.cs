//---------------------------------------------------------------------
// File: DatalinkUITypeEditor.cs
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
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

using ODBCDriverPrompt; //Custom written dialog to get ODBC connections

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    #region DatalinkDialog

    public class DataLinkDialog
    {

        private string connectionString = String.Empty;

        public DataLinkDialog( )
        {

        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public DialogResult Show( System.IntPtr parent )
        {
            try
            {
                CDataLink datalink = new CDataLink( );

                System.Guid riid = new Guid( "0C733A8B-2A1C-11CE-ADE5-00AA0044773D" ); //IID_IDBInitialize
                IDataInitialize dataInitialize = ( IDataInitialize )datalink;

                object pIDBInit = null;
                dataInitialize.GetDataSource( null, 1, connectionString, ref riid, out pIDBInit );

                IDBPromptInitialize dbPrompt = ( IDBPromptInitialize )datalink;
                dbPrompt.PromptDataSource( null, parent, 18, 0, 0, null, ref riid, ref pIDBInit );

                dataInitialize.GetInitializationString( pIDBInit, true, out connectionString );
            }
            catch ( System.Runtime.InteropServices.COMException )
            {
                // A COM exception is thrown when user cancels
                return DialogResult.Cancel;
            }

            return DialogResult.OK;
        }
    }

    #endregion

    class LaunchForm : Form
    {
        private DataLinkDialog form;
        private DialogResult dialogResult;
        private delegate void NextDialogDelegate( );
        private NextDialogDelegate nextDialog;

        public LaunchForm( DataLinkDialog form )
        {
            this.form = form;
            this.dialogResult = DialogResult.Cancel;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

            //  Switch to using Opacity if it is supported (its not on win2k)
            if ( Environment.OSVersion.Version.Major > 5 || ( Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor > 0 ) )
                this.Opacity = 0.00;
            else
                this.WindowState = System.Windows.Forms.FormWindowState.Minimized;

            this.nextDialog = new NextDialogDelegate( NextDialog );
            this.Load += new EventHandler( LoadEvent );
        }
        
        private void NextDialog( )
        {
            this.dialogResult = this.form.Show( this.Handle );
            this.Close( );
        }
        
        private void LoadEvent( object sender, EventArgs e )
        {
            this.nextDialog.BeginInvoke( null, null );
        }
        
        public new DialogResult DialogResult
        {
            get
            {
                return dialogResult;
            }
        }
    }

    #region DatalinkTypeEditor

    public class DatalinkUITypeEditor : System.Drawing.Design.UITypeEditor
    {
        private DataLinkDialog _form;

        public DatalinkUITypeEditor( )
        {
            this._form = new DataLinkDialog( );
            this._form.ConnectionString = "";//"Provider=SQLOLEDB.1";
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

        public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value )
        {
            // If the value is null or it's an invalid connection string, we will discard it
            if ( null != value )
            {
                try
                {
                    Validator.ValidateConnectionString( value.ToString( ) );
                    this._form.ConnectionString = value.ToString( );
                }
                catch ( SqlValidationException ) { }
            }

            ODBCDriverUI DriverDialog = new ODBCDriverUI( );
            this._form.ConnectionString = DriverDialog.GetDSN( );

            //			//  this form used to launch the dialog - see comment on class
            //			LaunchForm launchForm = new LaunchForm(this._form);
            //
            //			//  seems to be only way to launch modal dialogs from the adapter framework
            //			IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //			DialogResult result = service.ShowDialog(launchForm);
            //
            //			DialogResult dialogResult = launchForm.DialogResult;

            //			switch (dialogResult) 
            //			{
            //				case System.Windows.Forms.DialogResult.OK:
            value = this._form.ConnectionString;

            // Use OleDbConnection to read the database name and server name, generates URI
            //					OleDbConnection myConnection = new OleDbConnection(this._form.ConnectionString);
            //					string uriString = string.Empty;
            ////					if (myConnection.DataSource.Length == 0)
            ////						uriString = "ODBC://" + "." + "/" + myConnection.Database + "/";
            ////					else
            //						uriString = "ODBC://" + value + "/";
            //
            //					// Writes to URI property
            //					object o = context.Instance;
            //					Type t = o.GetType();	
            //					System.Reflection.PropertyInfo p = t.GetProperty("uri");
            //					if (null != p)
            //						p.SetValue(context.Instance, uriString , null);

            //					break;
            //				case System.Windows.Forms.DialogResult.Cancel:
            //					break;
            //			}
            return value;
        }
    }

    #endregion

    #region Interop Classes

    [ComImport]
    [TypeLibType( TypeLibTypeFlags.FCanCreate )]
    [ClassInterface( ClassInterfaceType.None )]
    [Guid( "2206CDB2-19C1-11D1-89E0-00C04FD7A829" )]
    public class CDataLink { }

    [ComImport]
    [InterfaceTypeAttribute( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "2206CCB0-19C1-11D1-89E0-00C04FD7A829" )]
    [CLSCompliant( false )]
    public interface IDBPromptInitialize
    {
        void PromptDataSource(
            [MarshalAs( UnmanagedType.Interface )] Object pUnkOuter,
            IntPtr hWndParent,
            uint dwPromptOptions,
            uint cSourceTypeFilter,
            int rgSourceTypeFilter,
            string pwszszzProviderFilter,
            ref System.Guid riid,
            [MarshalAs( UnmanagedType.Interface )] ref Object ppDataSource );

    }

    [ComImport]
    [InterfaceTypeAttribute( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "2206CCB1-19C1-11D1-89E0-00C04FD7A829" )]
    [CLSCompliant( false )]
    public interface IDataInitialize
    {
        void GetDataSource(
            [MarshalAs( UnmanagedType.Interface )] Object pUnkOuter,
            uint dwClsCtx,
            string pwszInitializationString,
            ref System.Guid riid,
            [MarshalAs( UnmanagedType.Interface )] out Object ppDataSource );

        void GetInitializationString(
            [MarshalAs( UnmanagedType.Interface )] Object pDataSource,
            bool fIncludePassword,
            [MarshalAs( UnmanagedType.LPWStr )] out string ppwszInitString );
    }

    [ComImport]
    [InterfaceTypeAttribute( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "0C733A8B-2A1C-11CE-ADE5-00AA0044773D" )]
    public interface IDBInitialize { }

    #endregion
}