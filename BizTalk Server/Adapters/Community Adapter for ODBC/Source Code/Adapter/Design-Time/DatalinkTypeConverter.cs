/*************************************************************************
 * Copyright (c) 1999-2002 Microsoft Corporation. All rights reserved.   *
 *                                                                       *
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY *
 * KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE  *
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR *
 * PURPOSE. THE ENTIRE RISK OF USE OR RESULTS IN CONNECTION WITH THE USE *
 * OF THIS CODE AND INFORMATION REMAINS WITH THE USER.                   *
 *************************************************************************/
using System;
using System.ComponentModel;
using System.Globalization;

using Microsoft.BizTalk.Adapter.Framework;

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    /// <summary>
    /// Class <c>DatalinkTypeConverter</c>.
    /// </summary>
    public class DatalinkTypeConverter : StringConverter
    {
        #region Methods
        
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            return ( typeof( string ) == destinationType ) || base.CanConvertTo( context, destinationType );
        }

        /// <summary>
        /// Convert actual value to desired type for display.
        /// </summary>
        /// <remarks>
        /// The parameter value contains the actual connection string,
        /// including password.  For security reasons, the password
        /// must be masked.
        /// </remarks>
        /// <param name="context">
        /// Property grid cell context.
        /// </param>
        /// <param name="culture">
        /// Culture to use for localized formatting.
        /// </param>
        /// <param name="value">
        /// Actual value to display in grid cell.
        /// </param>
        /// <param name="destinationType">
        /// Desired type to display (grid cell's type).
        /// </param>
        /// <returns>
        /// Converted value with password masked.
        /// </returns>
        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
        {
            if ( typeof( string ) == destinationType && value is string )
            {
                string pass = base.ConvertTo( context, culture, value, destinationType ) as string;
                return SecurePassword( pass );//FIXConnectionString.SecurePassword(pass);
            }
            return base.ConvertTo( context, culture, value, destinationType );
        }

        private string SecurePassword( string ConnectionString )
        {
            if ( ConnectionString != "" )
            {
                int iPasswordIndex = ConnectionString.IndexOf( "PWD=", 1 );

                if ( iPasswordIndex > 0 )
                {
                    int iEndOfPassword = ConnectionString.IndexOf( ";", iPasswordIndex );

                    //Deal with the fact that we may not have a ; at the end of our statement
                    if ( iEndOfPassword < 0 )
                        iEndOfPassword = ConnectionString.Length;

                    string password = ConnectionString.Substring( iPasswordIndex + 4, iEndOfPassword - ( iPasswordIndex + 4 ) );
                    return ConnectionString.Replace( password, "*********" );

                }
                else
                {
                    return ConnectionString;
                }
            }
            else
            {
                return String.Empty;
            }

        }

        #endregion
    }
}