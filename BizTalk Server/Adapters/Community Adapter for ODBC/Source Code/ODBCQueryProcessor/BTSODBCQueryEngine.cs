//---------------------------------------------------------------------
// File: BTSODBCQueryEngine.cs
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
using System.Data.Odbc;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Data; 
using System.Text;
using System.Diagnostics;
using System.Configuration.Install;

using Microsoft.Samples.BizTalk.Adapters.BaseAdapter;

namespace Microsoft.BizTalk.Adapters.ODBC
{
    public class BTSODBCQueryEngine
    {
        public bool ODBCTransaction = false;
        private string _strNamespace = "";

        private string _strDocumentRootNode = "";
        private OdbcConnection _OdbcConnection = new OdbcConnection(); 
        private OdbcTransaction _OdbcTransaction;
        private bool _OutParametersExist = false;

        //Performance Counters
        //private PerformanceCounter _MessageCounter = new PerformanceCounter( );
        //private PerformanceCounter _ErrorCounter = new PerformanceCounter( );
        //private PerformanceCounter _ConnectionCounter = new PerformanceCounter( );
        //private PerformanceCounter _MessagesPerSecond = new PerformanceCounter( );

        public bool RequestResponseProcess = false; // Need to know if we should send a response message as part of a send port
        public bool ReceivePort = false;			// Need to know if we are a receive port if so we don't want to send message if nothing is returned

        public string ConnectionString
        {
            set
            {
                _OdbcConnection.ConnectionString = value;
            }
        }

        public string Namespace
        {
            set
            {
                _strNamespace = value;
            }
        }

        public string RootNode
        {
            get
            {
                return _strDocumentRootNode;
            }
            set
            {
                _strDocumentRootNode = value;
            }
        }

        public void BeginTransaction( System.Data.IsolationLevel IsolationLevel )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::BTSODBCQueryEngine::BeginTransaction()"); 
            _OdbcConnection.Open( );
            _OdbcTransaction = _OdbcConnection.BeginTransaction( IsolationLevel );
        }

        public void Commit( )
        {
            _OdbcTransaction.Commit( );
        }

        public void Close( )
        {
            _OdbcConnection.Close( );
        }

        public void Rollback( )
        {
            _OdbcTransaction.Rollback( );
        }

        public BTSODBCQueryEngine( )
        {
            //Create PerfCounters if they don't already exist
            //initialize our perf counters
            //_MessageCounter.CategoryName = "BtsOdbcQueryEngine";
            //_MessageCounter.CounterName = "MessageCount";
            //_MessageCounter.ReadOnly = false;

            //_MessagesPerSecond.CategoryName = "BtsOdbcQueryEngine";
            //_MessagesPerSecond.CounterName = "MessagesPerSecond";
            //_MessagesPerSecond.ReadOnly = false;

            //_ConnectionCounter.CategoryName = "BtsOdbcQueryEngine";
            //_ConnectionCounter.CounterName = "DbConnectionCount";
            //_ConnectionCounter.ReadOnly = false;

            //_ErrorCounter.CategoryName = "BtsOdbcQueryEngine";
            //_ErrorCounter.CounterName = "ErrorCount";
            //_ErrorCounter.ReadOnly = false;

            //Create an event handler for the _OdbcConnection object
            _OdbcConnection.StateChange += new StateChangeEventHandler( _OdbcConnection_StateChange );
        }

        private OdbcCommand CreateODBCCommandFromSchema( string QuerySchema )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::BTSODBCQueryEngine::CreateODBCCommandFromSchema()");
            StringReader sr = new StringReader( QuerySchema );
            OdbcCommand odbcCmd = new OdbcCommand( );
            XmlSchema xsdQueryConfiguration = XmlSchema.Read( sr, null );

            foreach ( object item in xsdQueryConfiguration.Items )
            {
                if ( item is XmlSchemaAnnotation ) // Then we are at the section of the schema containing
                {								// the odbc query spec
                    XmlSchemaAnnotation xsAnnotation = ( XmlSchemaAnnotation )item;
                    XmlSchemaAppInfo xsAppInfo = ( XmlSchemaAppInfo )xsAnnotation.Items[ 0 ];
                    XmlNode[ ] xmlNodesQueryInfo = xsAppInfo.Markup;

                    // Create our new command object for query processing
                    odbcCmd.CommandText = xmlNodesQueryInfo[ 1 ].InnerText;

                    //Get the root node name for the response document from the schema
                    _strDocumentRootNode = xmlNodesQueryInfo[ 2 ].InnerText;

                    // pull the output documents namespace from the schema.
                    _strNamespace = xmlNodesQueryInfo[ 3 ].InnerText + "/" + _strDocumentRootNode;
                }
                if ( item is XmlSchemaElement )   // Root Section of the schema
                {
                    try
                    {
                        XmlSchemaComplexType xsComplex = ( XmlSchemaComplexType )( ( XmlSchemaElement )item ).SchemaType;
                        XmlSchemaSequence xsSeq = ( XmlSchemaSequence )xsComplex.Particle;

                        // Now we're down to the parameters section of the schema!
                        XmlSchemaElement xsParamsElement = ( XmlSchemaElement )xsSeq.Items[ 0 ];
                        XmlSchemaComplexType xsCT = ( XmlSchemaComplexType )xsParamsElement.SchemaType;

                        // IMPORTANT NOTE: If for some reason they change the order of the params in the 
                        // schmema and/or the document being processes then the results will be wrong
                        // as there is no way to map the data to the correct element of the query
                        // remember our SQL looks like SELECT * FROM CUSTOMER WHERE CUSTOMERID = ?

                        //NOTE: if statement added to account for possiblity that we have not have command parameters
                        if ( !( xsCT == null ) )
                        {
                            foreach ( XmlSchemaAttribute xsAttParams in xsCT.Attributes )
                            {
                                OdbcParameter queryParam = new OdbcParameter( );

                                queryParam.ParameterName = "@" + xsAttParams.Name;

                                //Now get all of the meta data from the parameter
                                XmlSchemaAppInfo xsParamInfo = ( XmlSchemaAppInfo )xsAttParams.Annotation.Items[ 0 ];
                                XmlNode[ ] xmlParamInfo = xsParamInfo.Markup;

                                queryParam.OdbcType = ConvertStringDataTypeToODBC( xmlParamInfo[ 0 ].InnerText );
                                queryParam.Direction = ConverStrToParamDir( xmlParamInfo[ 1 ].InnerText );

                                if ( System.Convert.ToInt32( xmlParamInfo[ 2 ].InnerText ) > 0 )
                                    queryParam.Size = System.Convert.ToInt32( xmlParamInfo[ 2 ].InnerText );

                                odbcCmd.Parameters.Add( queryParam );
                                //Add CommandTimeout
                                odbcCmd.CommandTimeout = 60; 
                                System.Diagnostics.Trace.WriteLine(string.Format("BizTalk ODBC Adapter::BTSODBCQueryEngine::CreateODBCCommandFromSchema()::QueryParam-->{0}", queryParam.ParameterName.ToString()));
                            }
                        }
                    }
                    catch { }
                }
            }

            return odbcCmd;
        }

        public VirtualStream BTSExecuteODBCCallFromSQL( string SQL )
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::BTSODBCQueryEngine::BTSExecuteODBCCallFromSQL()");
                //_MessagesPerSecond.Increment( );
                //_MessageCounter.Increment( );

                OdbcCommand odbcCmd = new OdbcCommand( );
                odbcCmd.Connection = _OdbcConnection;
                odbcCmd.CommandText = SQL;
                odbcCmd.CommandType = CommandType.Text;
                //Add CommandTimeout
                odbcCmd.CommandTimeout = 60;
                                

                return ODBCCommandProcessing( odbcCmd, null );
            }
            catch ( Exception ex )
            {
                //_ErrorCounter.Increment( );
                throw ex;
            }
        }

        public VirtualStream BTSExecuteODBCCallFromSchema( XmlTextReader xStreamPayload, string QuerySchema )
        {
            try
            {
                //_MessagesPerSecond.Increment( );
                //_MessageCounter.Increment( );

                OdbcCommand odbcCmd = CreateODBCCommandFromSchema( QuerySchema );
                return ODBCCommandProcessing( odbcCmd, xStreamPayload );
            }
            catch ( Exception ex )
            {
                //_ErrorCounter.Increment( );
                throw ex;
            }
        }

        //implements the same function as ExecuteDataSetCommand except uses a DataReader and XmlTextWriter to generate output
        private bool ExecuteDataReaderCommand( ref XmlTextWriter resultXml, OdbcCommand odbcCmd )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::BTSODBCQueryEngine::ExecuteDataReaderCommand()");
            OdbcDataReader dr;
            bool bHasResults = false;

            resultXml.Formatting = System.Xml.Formatting.Indented;

            try
            {
                //Open the connection and get the data reader
                odbcCmd.Connection = _OdbcConnection;

                if (_OdbcConnection.State != ConnectionState.Open)
                    odbcCmd.Connection.Open();

                if (_OdbcTransaction != null)
                    odbcCmd.Transaction = _OdbcTransaction;
                //Add CommandTimeout
                odbcCmd.CommandTimeout = 60;

                //Trace out command
                string cmdSQL = odbcCmd.CommandText;
                System.Diagnostics.Trace.WriteLine(string.Format("BizTalk ODBC Adapter::BTSODBCQueryEngine::ExecuteDataReaderCommand()::Command to be executed-->{0}", cmdSQL));
  

                dr = odbcCmd.ExecuteReader();

                int tableCount = 0;
                do
                {
                    while (dr.Read())
                    {
                        bHasResults = true;
                        //Write Record Element
                        if (tableCount == 0)
                            resultXml.WriteStartElement("Table");
                        else
                            resultXml.WriteStartElement("Table" + tableCount);

                        //Fill RowData
                        for (int i = 0; i <= dr.FieldCount - 1; i++)
                        {
                            resultXml.WriteStartElement(XmlConvert.EncodeName(dr.GetName(i)));

                            //handle BLOB fields.  We want to stream BLOB data 64K at a time to
                            //avoid pounding the server
                            if (dr.GetValue(i).GetType() == System.Type.GetType("System.Byte[]"))
                            {
                                byte[] buff = new byte[65536];
                                int bufferSize = 65536;
                                long retval = 0;

                                int startIndex = 0;

                                retval = dr.GetBytes(i, startIndex, buff, 0, bufferSize);

                                // Continue reading and writing while there are bytes beyond the size of the buffer.
                                while (retval == bufferSize)
                                {
                                    resultXml.WriteBase64(buff, 0, bufferSize);
                                    resultXml.Flush();

                                    // Reposition the start index to the end of the last buffer and fill the buffer.
                                    startIndex += bufferSize;
                                    retval = dr.GetBytes(i, startIndex, buff, 0, bufferSize);
                                }

                                // Write the remaining buffer.
                                resultXml.WriteBase64(buff, 0, (int)retval - 1);
                                resultXml.Flush();
                            }
                            else		//Non-BLOB data types
                            {
                                resultXml.WriteString(System.Convert.ToString(dr.GetValue(i)));
                            }
                            resultXml.WriteEndElement();
                            resultXml.Flush();
                        }

                        //Close the Record Element
                        resultXml.WriteEndElement();
                        resultXml.Flush();
                    }

                    tableCount += 1;
                }
                while (dr.NextResult());

                dr.Close();

                //Now add the out parameters
                if (_OutParametersExist)
                    AddSPParameters(odbcCmd, ref resultXml);
                return bHasResults;
            }
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.WriteLine(string.Format("BizTalk ODBC Adapter::BTSODBCQueryEngine::ExecuteDataReaderCommand()::Exception Caught-->{0}", ex.Message.ToString()));
            //    return false;
            //    //throw ex;
            //}
            finally
            {
                if (odbcCmd.Connection.State != ConnectionState.Closed)
                {
                    odbcCmd.Connection.Close();
                }
            }
        }

        private VirtualStream ODBCCommandProcessing( OdbcCommand odbcCmd, XmlTextReader xtrReader )
        {
            System.Diagnostics.Trace.WriteLine("BizTalk ODBC Adapter::BTSODBCQueryEngine::ODBCCommandProcessing()");
            VirtualStream Output = null;
            VirtualStream WorkingStream = new VirtualStream( );

            XmlTextWriter resultXml = new XmlTextWriter( WorkingStream, null );
            
            bool CmdComplete = false;
            bool isInputProcessed = false;
            bool hasInputParams = false;

            //Determine if there are input parameters
            for ( int i = 0; i < odbcCmd.Parameters.Count; i++ )
            {
                ParameterDirection dir = odbcCmd.Parameters[ i ].Direction;

                if ( dir == ParameterDirection.Input ||
                    dir == ParameterDirection.InputOutput )
                    hasInputParams = true;
            }

            //Generate the root element
            resultXml.WriteStartElement( "ns0", _strDocumentRootNode, _strNamespace );

            UnicodeEncoding Unicode = new UnicodeEncoding( );

            // Handle the situation where we don't have parameters but still want to process the query
            if ( xtrReader != null && odbcCmd.Parameters.Count > 0 )
            {
                while ( xtrReader.Read( ) )
                {
                    //we're only interested in those elements that have attributes...
                    if ( xtrReader.NodeType == XmlNodeType.Element && xtrReader.HasAttributes )
                    {
                        #region Build Command from XML Attributes
                        //iterate through the attributes--check if the names match any of our parameters
                        for ( int attCnt = 0; attCnt < xtrReader.AttributeCount; attCnt++ )
                        {
                            xtrReader.MoveToAttribute( attCnt );

                            for ( int cnt = 0; cnt < odbcCmd.Parameters.Count; cnt++ )
                            {
                                #region ODBC Parameters
                                if ( xtrReader.Name == odbcCmd.Parameters[ cnt ].ParameterName.Replace( "@", "" ) )
                                {
                                    if ( odbcCmd.Parameters[ cnt ].Direction != ParameterDirection.Input )
                                    {
                                        //we need to add output parameters to our response
                                        _OutParametersExist = true;
                                    }

                                    //Pass in values for input parameters
                                    if ( odbcCmd.Parameters[ cnt ].Direction != ParameterDirection.ReturnValue &&
                                        odbcCmd.Parameters[ cnt ].Direction != ParameterDirection.Output )
                                    {
                                        #region ODBC Type Converstion
                                        //Convert input parameter to the correct data type
                                        switch ( odbcCmd.Parameters[ cnt ].OdbcType.ToString( ) )
                                        {
                                            case "BigInt":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToInt64( xtrReader.Value );
                                                break;
                                            case "Binary": //Expects that we are receiving the data in the form of a Base64 string
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.FromBase64String( xtrReader.Value );
                                                break;
                                            case "Bit":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToBoolean( xtrReader.Value );
                                                break;
                                            case "Text":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToString( xtrReader.Value );
                                                break;
                                            case "DateTime":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDateTime( xtrReader.Value );
                                                break;
                                            case "Numeric":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDecimal( xtrReader.Value );
                                                break;
                                            case "Double":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDouble( xtrReader.Value );
                                                break;
                                            case "Int":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToInt32( xtrReader.Value );
                                                break;
                                            case "Real":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDouble( xtrReader.Value );
                                                break;
                                            case "SmallInt":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToInt16( xtrReader.Value );
                                                break;
                                            case "TinyInt":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToByte( xtrReader.Value );
                                                break;
                                            case "UniqueIdentiflier":
                                                System.Guid unqId = new Guid( xtrReader.Value );
                                                odbcCmd.Parameters[ cnt ].Value = unqId;
                                                break;
                                            case "Char": //FIX THIS NEEDS TO SUPPORT ARRAYS
                                                odbcCmd.Parameters[ cnt ].Value = xtrReader.Value;
                                                break;
                                            case "Date": // THIS IS A BEST GUESS
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDateTime( xtrReader.Value );
                                                break;
                                            case "Decimal":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDecimal( xtrReader.Value );
                                                break;
                                            case "Image":  //TODO:  Is this a Base64 encoding too?
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.FromBase64String( xtrReader.Value );
                                                break;
                                            case "NChar":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToString( xtrReader.Value );
                                                break;
                                            case "NText":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToString( xtrReader.Value );
                                                break;
                                            case "NVarChar":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToString( xtrReader.Value );
                                                break;
                                            case "SmallDateTime":  // BEST GUESS
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDateTime( xtrReader.Value );
                                                break;
                                            case "Time": // BEST GUESS
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDateTime( xtrReader.Value );
                                                break;
                                            case "Timestamp":
                                                odbcCmd.Parameters[ cnt ].Value = Unicode.GetBytes( xtrReader.Value );
                                                break;
                                            case "VarBinary": //Assumes data comes in as a Base64 string
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.FromBase64String( xtrReader.Value );
                                                break;
                                            case "VarChar":
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToString( xtrReader.Value );
                                                break;
                                            default:
                                                odbcCmd.Parameters[ cnt ].Value = System.Convert.ToDateTime( xtrReader.Value );
                                                break;
                                        }
                                        #endregion

                                        isInputProcessed = true;
                                    }
                                }
                                #endregion
                            }

                        }
                        #endregion

                        // Now that the Command object has been populated execute the query and get the results
                        try
                        {
                            //determine if input parameters have been processed.
                            CmdComplete = ( !hasInputParams || ( hasInputParams && isInputProcessed ) );

                            //Call to the DataReader-based implementation
                            if ( CmdComplete )
                            {
                                ExecuteDataReaderCommand( ref resultXml, odbcCmd );
                                Output = WorkingStream;
                                CmdComplete = false;
                            }
                        }
                        catch ( Exception e )
                        {
                            throw e;
                        }
                        //Ensure that the reader returns to the parent element once it's finished processing all of the attributes
                        xtrReader.MoveToElement( );
                    }
                }
            }
            else // This is a query from a receive port OR We're dealing with 
            // a no input values send port. These DO NOT have a input document
            {
                try
                {
                    //Did the query return any results. If it did not we don't want
                    //to send back an empty document
                    if ( ExecuteDataReaderCommand( ref resultXml, odbcCmd ) )
                    {
                        Output = WorkingStream;
                    }
                    else
                    {
                        Output = null;
                    }
                    CmdComplete = true;
                }
                catch ( Exception e )
                {
                    throw new Exception( "Receive Port query processing failed: " + e.Message + " " + e.InnerException );
                }
            }

            //close the root element
            resultXml.WriteEndElement( );
            resultXml.Flush( );

            return Output;
        }

        // This function exists to remove the <?xml version="1.0" encoding="utf-8" ?>
        // from the input documents. It appears that in some cases this creates a problem
        // for the XMLDOM
        private string CleanDocument( string msgIn )
        {
            if ( msgIn.StartsWith( "<?" ) )
            {
                msgIn = msgIn.Substring( msgIn.IndexOf( "?>", 1 ) + 2 );
            }
            return msgIn;
        }

        //overloaded to accomodate XmlTextWriter input in lieu of dataset data.
        private void AddSPParameters( OdbcCommand odbcCmd, ref XmlTextWriter xtw )
        {
            xtw.WriteStartElement( "OutParameters" );
            for ( int i = 0; i < odbcCmd.Parameters.Count; i++ )
            {
                if ( odbcCmd.Parameters[ i ].Direction != ParameterDirection.Input )
                {
                    xtw.WriteStartAttribute( odbcCmd.Parameters[ i ].ParameterName.Replace( "@", "" ), "" );
                    if ( odbcCmd.Parameters[ i ].OdbcType == OdbcType.Binary ||
                        odbcCmd.Parameters[ i ].OdbcType == OdbcType.Image ||
                        odbcCmd.Parameters[ i ].OdbcType == OdbcType.VarBinary )
                    {
                        byte[ ] buffer = ( byte[ ] )odbcCmd.Parameters[ i ].Value;  //Cast it as a byte array
                        xtw.WriteBase64( buffer, 0, buffer.Length );
                    }
                    else
                    {
                        xtw.WriteString( odbcCmd.Parameters[ i ].Value.ToString( ) );
                    }
                    xtw.WriteEndAttribute( );
                    xtw.Flush( );
                }
            }
            xtw.WriteEndElement( );
            xtw.Flush( );
        }

        private ParameterDirection ConverStrToParamDir( string sParamDir )
        {
            switch ( sParamDir )
            {
                case "Input":
                    return ParameterDirection.Input;
                case "Output":
                    return ParameterDirection.Output;
                case "InputOutput":
                    return ParameterDirection.InputOutput;
                case "ReturnValue":
                    return ParameterDirection.ReturnValue;
                default:
                    return ParameterDirection.Input;
            }
        }

        private OdbcType ConvertStringDataTypeToODBC( string sType )
        {
            switch ( sType )
            {
                case "BigInt":
                    return OdbcType.BigInt;
                case "Binary": // Also could be IMAGE
                    return OdbcType.Binary;
                case "Bit":
                    return OdbcType.Bit;
                case "Text": // Could also be NCHAR, NTEXT, NVARCHAR
                    return OdbcType.Text;
                case "DateTime":
                    return OdbcType.DateTime;
                case "Numeric": // Could also be DECIMAL, 
                    return OdbcType.Numeric;
                case "Double":
                    return OdbcType.Double;
                case "Int":
                    return OdbcType.Int;
                case "Real":
                    return OdbcType.Real;
                case "SmallInt":
                    return OdbcType.SmallInt;
                case "TinyInt":
                    return OdbcType.TinyInt;
                case "UniqueIdentifier":
                    return OdbcType.UniqueIdentifier;
                case "Char":
                    return OdbcType.Char;
                case "Date":
                    return OdbcType.Date;
                case "Decimal":
                    return OdbcType.Decimal;
                case "Image":
                    return OdbcType.Image;
                case "NChar":
                    return OdbcType.NChar;
                case "NText":
                    return OdbcType.NText;
                case "NVarChar":
                    return OdbcType.NVarChar;
                case "SmallDateTime":
                    return OdbcType.SmallDateTime;
                case "Time":
                    return OdbcType.Time;
                case "Timestamp":
                    return OdbcType.Timestamp;
                case "VarBinary":
                    return OdbcType.VarBinary;
                case "VarChar":
                    return OdbcType.VarChar;
                default:
                    return OdbcType.Text;
            }
        }

        private void _OdbcConnection_StateChange( object sender, StateChangeEventArgs e )
        {
            if ( e.CurrentState == ConnectionState.Open )
            {
                //_ConnectionCounter.Increment( );
            }
            else if ( e.CurrentState == ConnectionState.Closed )
            {
                //_ConnectionCounter.Decrement( );
            }
        
        }
    }
}