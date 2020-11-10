//---------------------------------------------------------------------
// File: Validator.cs
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
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml.Schema;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    /// <summary>
    /// Validator class handles validation for connection string, root element name and target namespace
    /// </summary>
    public class Validator
    {
        public static void ValidateXlangKeyword( string input )
        {
            #region Reserved Xlang Keywords
            
            string[ ] reservedXlangKeywords = {  "activate",
												 "atomic",
												 "body",
												 "call",
												 "catch",
												 "checked",
												 "compensate",
												 "compensation",
												 "construct",
												 "correlation",
												 "correlationtype",
												 "delay",
												 "dynamic",
												 "else",
												 "exceptions",
												 "exec",
												 "exists",
												 "false",
												 "if",
												 "implements",
												 "initialize",
												 "internal",
												 "link",
												 "listen",
												 "longrunning",
												 "message",
												 "messagetype",
												 "method",
												 "module",
												 "new",
												 "null",
												 "oneway",
												 "out",
												 "parallel",
												 "port",
												 "porttype",
												 "private",
												 "public",
												 "receive",
												 "ref",
												 "request",
												 "requestresponse",
												 "response",
												 "scope",
												 "send",
												 "service",
												 "servicelink",
												 "servicelinktype",
												 "source",
												 "succeeded",
												 "suppressfailure",
												 "suspend",
												 "target",
												 "task",
												 "terminate",
												 "throw",
												 "timeout",
												 "transaction",
												 "transform",
												 "true",
												 "unchecked",
												 "until",
												 "uses",
												 "using",
												 "while",
												 "xpath",
			};

            #endregion

            for ( int i = 0; i < reservedXlangKeywords.Length; i++ )
                if ( input == reservedXlangKeywords[ i ] )
                    throw new SqlValidationException( "You are using a BizTalk reserver word as part of you naming convention" );
        }

        public static void ValidateRootElementName( string input )
        {
            char[ ] invalidChars = { '>', '<', '\'', '\"', '&', ':' };
            if ( input.IndexOfAny( invalidChars ) >= 0 )
                throw new SqlValidationException( "You are using invalid characters in your root element name" );
            ValidateXlangKeyword( input );
        }

        public static void ValidateTargetNamespace( string input )
        {
            XmlSchema xs = new XmlSchema( );
            xs.TargetNamespace = input;

            XmlSchemaSet xsSet = new XmlSchemaSet( );
            xsSet.ValidationEventHandler += new ValidationEventHandler( ValidationHandler );
            xsSet.Add( xs );
            xsSet.Compile( );
            
            ValidateXlangKeyword( input );
        }

        public static void ValidationHandler( object sender, ValidationEventArgs args )
        {
            throw new SqlValidationException( "Validation Error: " + "\n" + args.Message );
        }

        public static void ValidateConnectionString( string input )
        {
            if ( input.Length == 0 )
                throw new SqlValidationException( "Connection string validation error" );

            /*	
                OleDbConnection myConnection = null;
                try
                {
                    myConnection = new OleDbConnection(input);
                    myConnection.Open();
                    myConnection.Close();
                }
                catch(Exception e)
                {
                    throw new SqlValidationException(ODBCResourceHandler.GetResourceString("ValidationConnectionError")+" "+e.Message);
                }
                finally
                {
                    if (myConnection != null) 
                        myConnection.Close();
                }
            */
        }
    }

    [Serializable( )]
    public class SqlValidationException : Exception
    {
        public SqlValidationException( ) { }

        public SqlValidationException( string s, Exception e ) : base( s, e ) { }

        protected SqlValidationException( SerializationInfo si, StreamingContext sc ) : base( si, sc ) { }

        public SqlValidationException( string errorMsg ) : base( errorMsg )
        {
            this.Source = "ODBC Adapter Admin";
        }
    }
}