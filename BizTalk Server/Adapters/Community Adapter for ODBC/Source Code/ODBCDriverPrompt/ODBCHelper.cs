using System;
using System.Collections;

using Microsoft.Win32;

namespace ODBCDriverPrompt
{
    public class ODBCHelper
    {
        public ODBCHelper( ) { }

        private const string ODBC_LOC_IN_REGISTRY = "SOFTWARE\\ODBC\\";
        private const string ODBC_INI_LOC_IN_REGISTRY = ODBC_LOC_IN_REGISTRY + "ODBC.INI\\";
        private const string DSN_LOC_IN_REGISTRY = ODBC_INI_LOC_IN_REGISTRY + "ODBC Data Sources\\";
        private const string ODBCINST_INI_LOC_IN_REGISTRY = ODBC_LOC_IN_REGISTRY + "ODBCINST.INI\\";
        private const string ODBC_DRIVERS_LOC_IN_REGISTRY = ODBCINST_INI_LOC_IN_REGISTRY + "ODBC Drivers\\";

        public ArrayList GetDSNList( )
        {
            //'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //'DESCRIPTION    :   Returns an array of strings each containing
            //'                   a DSN name found in the registry. 
            //'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //RegistryKey userBaseKey; //= new RegistryKey(); 
            //RegistryKey localMacineBaseKey; //=  RegistryKey();
            ArrayList lstDSN = new ArrayList( );
            RegistryKey keyDsnNames;
            string[ ] dsnNames = null;
            ArrayList regKeys = new ArrayList( );

            regKeys.Add( Registry.CurrentUser );
            regKeys.Add( Registry.LocalMachine );

            //userBaseKey = ;
            //localMacineBaseKey = Registry.LocalMachine;

            //  '#1 Get a list of all DSN entries defined in DSN_LOC_IN_REGISTRY for each basekey

            foreach ( RegistryKey regKey in regKeys )
            {
                keyDsnNames = OpenComplexSubKey( regKey, DSN_LOC_IN_REGISTRY, false );

                if ( keyDsnNames != null )
                {
                    dsnNames = keyDsnNames.GetValueNames( );
                }

                if ( dsnNames != null )
                {
                    // 'Foreach DSN entry in the DSN_LOC_IN_REGISTRY, goto the
                    //'add the dsn name to the array
                    foreach ( string dsnName in dsnNames )
                    {
                        if ( dsnName != null )
                        {
                            lstDSN.Add( dsnName );
                        }
                    }
                }

                //FIXED BY RODOLFO
                if ( keyDsnNames != null )
                    keyDsnNames.Close( );
            }

            return lstDSN;

        }

        private RegistryKey OpenComplexSubKey( RegistryKey baseKey, string strComplexKey, bool writable )
        {
            //'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //'DESCRIPTION    :   Returns the registry key given the key string
            //'                   and base location
            //'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int iPrevLoc = 0;
            int iCurrLoc = 0;
            string strSubKey = strComplexKey;
            RegistryKey keyFinal = baseKey;

            if ( baseKey == null )
                return null;

            if ( strComplexKey == null )
                return keyFinal;

            //'and get all the characters upto "\\" from the start of search
            //'point (iPrevLoc) as the keyString. Open a key out of string 
            //'keyString.

            while ( iCurrLoc != -1 )
            {
                iCurrLoc = strComplexKey.IndexOf( "\\\\", iPrevLoc );

                if ( iCurrLoc != -1 )
                {
                    strSubKey = strComplexKey.Substring( iPrevLoc, ( iCurrLoc - iPrevLoc ) );
                    iPrevLoc = iCurrLoc + 2;
                }
                else
                {
                    strSubKey = strComplexKey.Substring( iPrevLoc );
                }

                if ( !strSubKey.Equals( String.Empty ) )
                {
                    keyFinal = keyFinal.OpenSubKey( strSubKey, writable );
                }
            }

            return keyFinal;
        }
    }
}