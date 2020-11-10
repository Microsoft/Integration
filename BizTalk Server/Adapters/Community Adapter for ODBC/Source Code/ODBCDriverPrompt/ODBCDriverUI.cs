using System;

namespace ODBCDriverPrompt
{
    public class ODBCDriverUI
    {
        public ODBCDriverUI( ) { }

        public string GetDSN( )
        {
            ODBCDriverDialog frmObj = new ODBCDriverDialog( );

            if ( frmObj.ShowDialog( ) == System.Windows.Forms.DialogResult.OK )
                return frmObj.ConnectionString;
            else
                return null;
        }
    }
}