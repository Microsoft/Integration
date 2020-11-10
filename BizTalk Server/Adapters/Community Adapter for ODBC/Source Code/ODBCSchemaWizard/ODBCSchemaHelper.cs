//---------------------------------------------------------------------
// File: ODBCSchemaHelp.cs
// 
// Summary: ADO.NET ODBC Provider based BizTalk adapter. 
//
// Sample: Adapter framework runtime adapter.
//
//---------------------------------------------------------------------
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
using System.Data; 
using System.Data.Odbc;
using System.Collections;
using System.Xml;
using System.Xml.Schema; 
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
    public class ODBCSchemaHelper
    {
        public ODBCSchemaHelper( ) { }

        public string InputSchema = ""; // FIX THIS: SHould be read only
        public string OutputSchema = "";// FIX THIS: SHould be read only
        public AdapterCommandType QueryCommandType = AdapterCommandType.SQL;
        private bool _HasInputParameters = false;
        private string _QueryProcessingOutPut = string.Empty; // 

        public string QueryOutput
        {
            get
            {
                return _QueryProcessingOutPut;
            }
        }

        public enum AdapterCommandType
        {
            SQL,
            StoredProcedure
        }

        public enum PortType
        {
            Receive = 1,
            Send = 2
        }

        public enum AdapterStatementType
        {
            Input,
            InputOutput
        }

        private string _strDBConnection = string.Empty;
        private string _strScript = string.Empty;
        private string _strTableName = string.Empty;
        private AdapterCommandType _updateType;//FIX THIS
        private PortType _portType;
        private string _strInputRoot = string.Empty;
        private string _strOutputRoot = string.Empty;
        private string _strSPName = string.Empty;
        private string _strTargetNamespace = string.Empty;
        private string _strGeneratedScript = string.Empty;
        private string _strWSDL = string.Empty;
        private bool _ModifyCommand = false;

        private AdapterStatementType _StatementType = AdapterStatementType.InputOutput;

        #region Properties

        public AdapterStatementType StatementType
        {
            get
            {
                return _StatementType;
            }
            set
            {
                _StatementType = value;
            }
        }

        public bool OverrideQueryProcessing
        {
            set
            {
                _ModifyCommand = value;
            }
        }

        public string strDBConnection
        {
            get
            {
                return _strDBConnection;
            }
            set
            {
                _strDBConnection = value;
            }
        }

        public bool InputParameter
        {
            get
            {
                return _HasInputParameters;
            }
        }

        public string strScript
        {
            get
            {
                return _strScript;
            }
            set
            {
                _strScript = value;
                // This will init all of the schemas and other things!
                this.CreateBizTalkSchema( );
            }
        }

        public bool ModifyCommand
        {
            get
            {
                return _ModifyCommand;
            }
            set
            {
                _ModifyCommand = value;
            }

        }

        public string strTableName
        {
            get
            {
                return _strTableName;
            }
            set
            {
                _strTableName = value;
            }
        }

        public AdapterCommandType UpdategramType   // FIX THIS:
        {
            get
            {
                return _updateType;
            }
            set
            {
                _updateType = value;
            }
        }

        public PortType portType
        {
            get
            {
                return _portType;
            }
            set
            {
                _portType = value;
            }
        }

        public string strInputRoot
        {
            get
            {
                return _strInputRoot;
            }
            set
            {
                _strInputRoot = value;
            }
        }

        public string strOutputRoot
        {
            get
            {
                return _strOutputRoot;
            }
            set
            {
                _strOutputRoot = value;
            }
        }

        public string strSPName
        {
            get
            {
                return _strSPName;
            }
            set
            {
                _strSPName = value;
            }
        }

        public string strTargetNamespace
        {
            get
            {
                return _strTargetNamespace;
            }
            set
            {
                _strTargetNamespace = value;
            }
        }

        public string strGeneratedScript
        {
            get
            {
                return _strGeneratedScript;
            }
            set
            {
                _strGeneratedScript = value;
            }
        }

        public string strWSDL
        {
            get
            {
                return _strWSDL;
            }
            set
            {
                _strWSDL = value;
            }
        }
        
        #endregion

        private XmlSchemaAnnotation AdapterHeaderAnnotationSection( string ProcessedCommandScript )
        {
            //====================== SCHEMA ANNOTATION ================================
            // Create the Schema object necessary to contain the annotation info
            // this section will contain the SQL statements that the adapter processes
            //			<xs:annotation>
            //				<xs:appinfo>
            //					<msbtsodbc:SQLCMD xmlns:msbtsodbc="http://test">SELECT * FROM DEMO WHERE DEI = @DEI</msbtsodbc:SQLCMD> 
            //					<msbtsodbc:ODBCCMD xmlns:msbtsodbc="http://test">SELECT * FROM DEMO WHERE DEI = ?</msbtsodbc:ODBCCMD> 
            //					<msbtsodbc:ResponseRootName xmlns:msbtsodbc="http://test">ResponseDocRootName</msbtsodbc:ResponseRootName>
            //					<msbtsodbc:ResponseNS xmlns:msbtsodbc="http://test">http://ResponseNS</msbtsodbc:ResponseNS>
            //				</xs:appinfo>
            //			</xs:annotation>

            XmlSchemaAnnotation xsAnnotation = new XmlSchemaAnnotation( );
            XmlSchemaAppInfo xsAppInfo = new XmlSchemaAppInfo( );

            // AppInfo class uses an array of nodes to contain config data 
            // so we need to use the XMLDOM to create these nodes
            XmlDocument xmlWorkingDoc = new XmlDocument( );

            XmlNode[ ] xmlNodeAdapterConfig = new XmlNode[ 4 ];

            // Save out the query with parameter information
            xmlNodeAdapterConfig[ 0 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "SQLCMD", "http://schemas.microsoft.com/BizTalk/2003" );
            xmlNodeAdapterConfig[ 0 ].InnerText = _strScript;

            // Save out generic query with ODBC escape sequence
            xmlNodeAdapterConfig[ 1 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "ODBCCMD", "http://schemas.microsoft.com/BizTalk/2003" );
            xmlNodeAdapterConfig[ 1 ].InnerText = ProcessedCommandScript;

            // Save out root node name for the response document generated by this query
            xmlNodeAdapterConfig[ 2 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "ResponseRootName", "http://schemas.microsoft.com/BizTalk/2003" );
            xmlNodeAdapterConfig[ 2 ].InnerText = _strOutputRoot;

            // Save out root node name for the response document generated by this query
            xmlNodeAdapterConfig[ 3 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "ResponseNS", "http://schemas.microsoft.com/BizTalk/2003" );
            xmlNodeAdapterConfig[ 3 ].InnerText = _strTargetNamespace;

            xsAppInfo.Markup = xmlNodeAdapterConfig;
            xsAnnotation.Items.Add( xsAppInfo );

            return xsAnnotation;
        }

        public string ExtractODBCParametersSchema( OdbcCommand odbcCmd )
        {
            //First check to see if there are any input parameters 
            //if not we may only need to genterate a response schema!
            _HasInputParameters = false;

            //			if(odbcCmd.Parameters.Count == 0)
            //			{
            //				return null;
            //			}
            //			else
            //			{
            XmlSchema xsBTSInboundMsg = new XmlSchema( );
            XmlSchemaAnnotation xsAnnotation;
            XmlSchemaAppInfo xsAppInfo;
            // AppInfo class uses an array of nodes to contain config data 
            // so we need to use the XMLDOM to create these nodes
            XmlDocument xmlWorkingDoc = new XmlDocument( );
            XmlNode[ ] xmlNodeAdapterConfig;

            xsBTSInboundMsg.AttributeFormDefault = XmlSchemaForm.Unqualified;
            xsBTSInboundMsg.ElementFormDefault = XmlSchemaForm.Qualified;

            xsBTSInboundMsg.TargetNamespace = _strTargetNamespace + "/" + _strInputRoot; //"http://ODBCADAPTERTEST.com"; //FIX THIS: needs to be a parameter
            xsBTSInboundMsg.Namespaces.Add( "msbtsodbc", "http://schemas.microsoft.com/BizTalk/2003" );

            // Write the configuration to the schema
            xsBTSInboundMsg.Items.Add( AdapterHeaderAnnotationSection( odbcCmd.CommandText ) );

            //==================== QUERY PARAMETERS SCHEMA SECTION ====================
            XmlSchemaElement xsQueryRoot = new XmlSchemaElement( );
            //Setup document root
            xsQueryRoot.Name = _strInputRoot;

            xsBTSInboundMsg.Items.Add( xsQueryRoot );

            if ( odbcCmd.Parameters.Count > 0 )
            {
                //<xs:element name="InsertDocument">
                XmlSchemaComplexType xsRepeatingComplexType = new XmlSchemaComplexType( );

                xsQueryRoot.SchemaType = xsRepeatingComplexType;						//    <xs:complexType:>

                XmlSchemaElement xsElementParameters = new XmlSchemaElement( );

                XmlSchemaSequence xsSeq = new XmlSchemaSequence( );
                xsSeq.Items.Add( xsElementParameters );

                //Setup the repeating section of the query
                xsElementParameters.Name = "QueryParameters";
                xsElementParameters.MaxOccursString = "unbounded";
                xsElementParameters.MinOccursString = "1";

                XmlSchemaComplexType xsComplexType = new XmlSchemaComplexType( );

                xsRepeatingComplexType.Particle = ( XmlSchemaParticle )xsSeq;				//		<xs:element name="QueryParameters">

                xsElementParameters.SchemaType = xsComplexType;							//			<xs:complexType>

                // Loop through the parameter section of the command object and create a
                // schema attribute for each
                //Loop throught the ODBC Command object looking for in parameters
                foreach ( OdbcParameter odbcParam in odbcCmd.Parameters )
                {
                    XmlSchemaAttribute xsAttribute = new XmlSchemaAttribute( );
                    xsAttribute.Name = odbcParam.ParameterName.Replace( "@", "" );
                    xsAttribute.SchemaTypeName = ConvertODBCDataTypeToXMLDataType( odbcParam.OdbcType.ToString( ) );

                    // SAVE Out the meta data for this parameter in an appinfo section of the schema
                    // this is done to deal with the name space problems we run into etx with special
                    // attributes
                    xsAnnotation = new XmlSchemaAnnotation( ); // These vars were already define earlier
                    xsAppInfo = new XmlSchemaAppInfo( );

                    // AppInfo class uses an array of nodes to contain config data 
                    // so we need to use the XMLDOM to create these nodes
                    xmlWorkingDoc = new XmlDocument( );

                    xmlNodeAdapterConfig = new XmlNode[ 3 ];

                    // Save out ODBC data type for parameter
                    xmlNodeAdapterConfig[ 0 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsdbc", "ParamType", "http://schemas.microsoft.com/BizTalk/2003" );
                    xmlNodeAdapterConfig[ 0 ].InnerText = odbcParam.OdbcType.ToString( );

                    // Save out the parameter direction
                    xmlNodeAdapterConfig[ 1 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "ParamDir", "http://schemas.microsoft.com/BizTalk/2003" );
                    xmlNodeAdapterConfig[ 1 ].InnerText = odbcParam.Direction.ToString( );

                    // Save out the parameters data lenght
                    xmlNodeAdapterConfig[ 2 ] = xmlWorkingDoc.CreateNode( XmlNodeType.Element, "msbtsodbc", "ParamSize", "http://schemas.microsoft.com/BizTalk/2003" );
                    xmlNodeAdapterConfig[ 2 ].InnerText = odbcParam.Size.ToString( );

                    // Write the configuration to the schema
                    xsAttribute.Annotation = xsAnnotation;
                    xsAnnotation.Items.Add( xsAppInfo );
                    xsAppInfo.Markup = xmlNodeAdapterConfig;

                    xsComplexType.Attributes.Add( xsAttribute ); //			<xs:attribute name="DocumentID" type="xs:string" /> 

                    if ( odbcParam.Direction == ParameterDirection.Input || odbcParam.Direction == ParameterDirection.InputOutput )
                        _HasInputParameters = true;

                    //			<xs:attribute name="Revision" type="xs:string" /> 
                    // etc...
                }
            }
            // Get the schema string
            StringBuilder sb = new StringBuilder( );
            StringWriter sw = new StringWriter( sb );
            xsBTSInboundMsg.Write( sw );

            string sReturnSchema = sb.ToString( );

            return sReturnSchema;
            //}
        }

        /*
         * This function exists as the ODBC provider in .NET does not like
         * named parameters as part of database calls. So we will convert
         * the command parameters collection and command text into a generic SQL
         * statement.
         */
        public string BuildSQLSPCommandString( OdbcCommand odbcCmd )
        {
            //bool bReturnVal = false;				// Need to know this as some SP's do not return these
            string sSQLCommand = "";				//{ @RC = CALL StoredProc(@Param1, @Param2)}
            // This will be used to mapping to the XML
            // durning transmission

            string sGenericSPEscapeSequence = "";   //{ ? = CALL StoredProc(?, ?)}
            // This will be the actual call made by the
            // ODBC command object for the transmission

            // The first parameter in the parameters collection normally 
            // maintains the return value

            sSQLCommand = "{";
            sGenericSPEscapeSequence = "{";

            if ( odbcCmd.Parameters.Count > 0 && odbcCmd.Parameters[ 0 ].Direction == System.Data.ParameterDirection.ReturnValue )
            {
                sSQLCommand += odbcCmd.Parameters[ 0 ].ParameterName + " = ";
                sGenericSPEscapeSequence += "? =";
                //bReturnVal = true;
            }

            //Embed the sp execute statement
            sSQLCommand += "CALL " + odbcCmd.CommandText;
            sGenericSPEscapeSequence += "CALL " + odbcCmd.CommandText;

            //Now we loop thru the remaining parameters and build the SP statement
            for ( int cnt = 0; cnt < odbcCmd.Parameters.Count; cnt++ )
            {
                if ( cnt == 0 )
                {
                    //Set up for input parameters section of SP
                    sSQLCommand += "(";
                    sGenericSPEscapeSequence += "(";
                }

                if ( odbcCmd.Parameters[ cnt ].Direction != ParameterDirection.ReturnValue )
                {
                    sSQLCommand += odbcCmd.Parameters[ cnt ].ParameterName;
                    sGenericSPEscapeSequence += "?";
                }

                // Do we have another parameter after this parameter? If so we need to build an additional parameters
                if ( ( cnt + 1 ) < odbcCmd.Parameters.Count &&
                    odbcCmd.Parameters[ cnt ].Direction != ParameterDirection.ReturnValue )
                {
                    sSQLCommand += ",";
                    sGenericSPEscapeSequence += ",";
                }

                //Finish up the formating for the parameters if this is the last param
                if ( ( cnt + 1 ) == odbcCmd.Parameters.Count )
                {
                    //Set up for input parameters section of SP
                    sSQLCommand += ")";
                    sGenericSPEscapeSequence += ")";
                }
            }

            //Complete the statement syntax
            sSQLCommand += "}";
            sGenericSPEscapeSequence += "}";

            //FIXED BY RODOLFO
            sGenericSPEscapeSequence = sGenericSPEscapeSequence.Replace( "()", "" );
            
            _strGeneratedScript = sGenericSPEscapeSequence;
            
            return sGenericSPEscapeSequence;
        }

        public OdbcCommand GenerateODBCCommandFromSP( string sql )
        {
            //To resove the parameters for a given SP we need a live connection
            OdbcConnection odbcCon = new OdbcConnection( _strDBConnection );
            OdbcCommand odbcCmd = new OdbcCommand( sql, odbcCon );
            UnicodeEncoding Unicode = new UnicodeEncoding( );

            odbcCmd.CommandType = CommandType.StoredProcedure;

            odbcCon.Open( );

            //Have the ODBC Namespace resolve the parameters
            OdbcCommandBuilder.DeriveParameters( odbcCmd );

            odbcCon.Close( );

            //Now we'll loop thur the parameters set some bogus test data
            foreach ( OdbcParameter odbcParam in odbcCmd.Parameters )
            {
                switch ( odbcParam.OdbcType.ToString( ) )
                {
                    case "BigInt":
                        odbcParam.Value = System.Convert.ToInt64( "1" );
                        break;
                    case "Binary":
                        odbcParam.Value = Unicode.GetBytes( "1" );
                        break;
                    case "Bit":
                        odbcParam.Value = System.Convert.ToBoolean( "true" );
                        break;
                    case "Text":
                        odbcParam.Value = System.Convert.ToString( "1" );
                        break;
                    case "DateTime":
                        odbcParam.Value = System.Convert.ToDateTime( "1/1/2000" );
                        break;
                    case "Numeric":
                        odbcParam.Value = System.Convert.ToDecimal( "1" );
                        break;
                    case "Double":
                        odbcParam.Value = System.Convert.ToDouble( "1" );
                        break;
                    case "Int":
                        odbcParam.Value = System.Convert.ToInt32( "1" );
                        break;
                    case "Real":
                        odbcParam.Value = System.Convert.ToDouble( "1" );
                        break;
                    case "SmallInt":
                        odbcParam.Value = System.Convert.ToInt16( "1" );
                        break;
                    case "TinyInt":
                        odbcParam.Value = System.Convert.ToByte( "1" );
                        break;
                    case "UniqueIdentifier": //FIX THIS: Need to do something else!
                        odbcParam.Value = Guid.NewGuid( );//  System.Convert.ToString("1");
                        break;
                    case "Char": //FIX THIS NEEDS TO SUPPORT ARRAYS
                        odbcParam.Value = System.Convert.ToString( "1" );//.ToCharArray();
                        break;
                    case "Date": // THIS IS A BEST GUESS
                        odbcParam.Value = System.Convert.ToDateTime( "1/1/2000" );
                        break;
                    case "Decimal":
                        odbcParam.Value = System.Convert.ToDecimal( "1" );
                        break;
                    case "Image":
                        odbcParam.Value = Unicode.GetBytes( "1" );
                        break;
                    case "NChar":
                        odbcParam.Value = System.Convert.ToString( "1" );
                        break;
                    case "NText":
                        odbcParam.Value = System.Convert.ToString( "1" );
                        break;
                    case "NVarChar":
                        odbcParam.Value = System.Convert.ToString( "1" );
                        break;
                    case "SmallDateTime":  // BEST GUESS
                        odbcParam.Value = System.Convert.ToDateTime( "1/1/2000" );
                        break;
                    case "Time": // BEST GUESS
                        odbcParam.Value = System.Convert.ToDateTime( "1/1/2000" );
                        break;
                    case "Timestamp":
                        odbcParam.Value = Unicode.GetBytes( "1" );
                        break;
                    case "VarBinary": //FIX THIS
                        odbcParam.Value = Unicode.GetBytes( "1" );
                        break;
                    case "VarChar":
                        odbcParam.Value = System.Convert.ToString( "1" );
                        break;
                    default:
                        odbcParam.Value = System.Convert.ToDateTime( "1" );
                        break;

                }
            }

            // Now we need to process the command and generate a generic escape sequence for the SQL
            // the ODBC provider does NOT like named parameters
            odbcCmd.CommandText = BuildSQLSPCommandString( odbcCmd );

            return odbcCmd;
        }

        public OdbcCommand GenerateODBCCommandFromSQL( string sql )
        {
            int iParamStartIndex = 0;
            int iParamEndIndex = 1;
            int iParamSize = 0;
            string sParameterName = "";

            // We need to conver the SQL sytax that the user provided to something
            //	that the ODBC namespace can handle:
            //		FROM: 
            //			SELECT * FROM CUSTOMERS WHERER CUSTOMERID = @CUSTOMERID
            //		TO:
            //			 SELECT * FROM CUSTOMERS WHERE CUSTOMERID = ?
            // Thats what will be contained in the sODBCSQLEscapeSequence
            string sODBCSQLEscapeSequence = sql;

            //Get rid of the \r\n stuff and convert is to normal whitespace
            //or we will end up with missing text
            sql = sql.Replace( "\r\n", " " );

            OdbcConnection odbcCon = new OdbcConnection( _strDBConnection );
            OdbcCommand dbCmdODBC = new OdbcCommand( );
            dbCmdODBC.Connection = odbcCon;
            dbCmdODBC.CommandType = CommandType.Text;

            // Prep the command to deal with no spaces between commands etc
            // (@VALUE1,@VALUE2) 
            sql = sql.Replace( ",", " ," );
            sql = sql.Replace( "(", " (" );
            sql = sql.Replace( ")", " )" );

            do
            {
                //Now we need to shread the SQL the user provided us with to pull out
                //the parameters. These will be used to build in the input schema
                //                                            V
                // SELECT * FROM CUSTOMERS WHERE CUSTOMERID = @CUSTOMERID AND.....
                iParamStartIndex = sql.IndexOf( "@", iParamEndIndex );

                //If we run out of parameters get out of the loop
                if ( iParamStartIndex == -1 )
                    break;
                //++++++ NEED TO ADD A SECTION OF TYPE OF QUERY UPDATE, SQL SP etc
                //                                                       V
                // SELECT * FROM CUSTOMERS WHERE CUSTOMERID = @CUSTOMERID AND.....
                iParamEndIndex = sql.IndexOf( " ", iParamStartIndex );

                //We could be at the end of the string so we need to check
                if ( iParamEndIndex == -1 )
                    iParamEndIndex = sql.Length;

                //Figure out how big the parameter term is so we only extract it from the string
                iParamSize = iParamEndIndex - iParamStartIndex;
                sParameterName = sql.Substring( iParamStartIndex, iParamSize );

                // ========================= Process Parameters =======================
                //Add a new parameter to our command object
                OdbcParameter odbcParam = new OdbcParameter( );

                odbcParam.ParameterName = sParameterName;
                odbcParam.Value = "1";
                odbcParam.Direction = ParameterDirection.Input;
                //odbcParam.DbType = CANT DETERMINE THIS!!!

                dbCmdODBC.Parameters.Add( odbcParam );

                //====================== Setup Generic Command SQL ====================
                sODBCSQLEscapeSequence = sODBCSQLEscapeSequence.Replace( sParameterName, "?" );
            }
            while ( iParamEndIndex != sql.Length );

            //Now set the command objects command text to our generic SQL 
            dbCmdODBC.CommandText = sODBCSQLEscapeSequence;

            //Publish it up to the class level as a property
            _strGeneratedScript = sODBCSQLEscapeSequence;

            return dbCmdODBC;
        }

        public string GenerateOutputSchema( OdbcCommand odbcCmd )
        {
            bool bOutParamsFound = false;
            try
            {
                // Call the command to get the output schema
                OdbcDataAdapter OdbcAdapter = new OdbcDataAdapter( );

                // Need to call a transaction to prevent writes during the generation
                // of an output schema.
                // We want to wrap this call in a transaction that we don't commit
                odbcCmd.Connection.Open( );
                OdbcTransaction odbcTrans = odbcCmd.Connection.BeginTransaction( );

                odbcCmd.Transaction = odbcTrans;

                OdbcAdapter.SelectCommand = odbcCmd;

                DataSet custDS = new DataSet( _strOutputRoot );

                //Load the data set to get the output schema
                OdbcAdapter.Fill( custDS );

                // Throw away any junk we may have written

                //odbcTrans.Rollback(); //WARNING: These have been commited out as during testing some of the
                // open source DBs fail to implement transaction correctly. Most commercial DBS
                // will roll these transaction back if we fail to call Commit. So in most cases
                // we should be golden
                //odbcCmd.Transaction.Rollback(); 

                // Save the query Output for testing purposed
                _QueryProcessingOutPut = custDS.GetXml( );

                odbcCmd.Connection.Close( );

                string xsResultsSchema = custDS.GetXmlSchema( );

                //Now we need to extend the schema 
                StringReader sr = new StringReader( xsResultsSchema );
                //Load the data set schema into the XML Schema Objects
                XmlSchema xsdQueryConfiguration = XmlSchema.Read( sr, null );

                //Add target namespace
                //.DOC added to the name to prevent a collision between the ns ID and the root note name generated
                // by the dataset.
                xsdQueryConfiguration.TargetNamespace = _strTargetNamespace + "/" + _strOutputRoot;
                xsdQueryConfiguration.Id = _strOutputRoot + "Schema";

                //Now we need to disect the XSD from the data set so we can add our SP 
                //Parameters. The first element in the schema is an elemement
                // xsd:element
                XmlSchemaElement xsParentElement = ( XmlSchemaElement )xsdQueryConfiguration.Items[ 0 ];

                //Next get the complex type
                //xsd:ComplexType
                XmlSchemaComplexType xsParentComplexType = ( XmlSchemaComplexType )xsParentElement.SchemaType;

                //Next get the choice
                //xsd:Choice
                XmlSchemaChoice xsParentChoice = ( XmlSchemaChoice )xsParentComplexType.Particle;

                //Now we can insert the new SP parameters section into the 
                //choice selection

                //include any output parameters that are include
                //Now we add a section for the return value and out parameters
                //==================== QUERY PARAMETERS SCHEMA SECTION ====================

                XmlSchemaElement xsElementParameters = new XmlSchemaElement( );
                xsElementParameters.Name = "OutParameters";

                XmlSchemaComplexType xsComplexType = new XmlSchemaComplexType( );

                xsElementParameters.SchemaType = xsComplexType;	  //	<xs:complexType>

                // Loop through the parameter section of the command object and create a
                // schema attribute for each
                //Loop throught the ODBC Command object looking for in parameters
                foreach ( OdbcParameter odbcParam in odbcCmd.Parameters )
                {
                    // We only want to create elements for out bound parameters since this is the
                    // adapters output schema
                    if ( odbcParam.Direction != ParameterDirection.Input )
                    {
                        bOutParamsFound = true;
                        XmlSchemaAttribute xsAttribute = new XmlSchemaAttribute( );
                        xsAttribute.Name = odbcParam.ParameterName.Replace( "@", "" );
                        xsAttribute.SchemaTypeName = ConvertODBCDataTypeToXMLDataType( odbcParam.OdbcType.ToString( ) );
                        xsComplexType.Attributes.Add( xsAttribute ); //			<xs:attribute name="DocumentID" type="xs:string" /> 
                    }
                }

                // See if we need to add the out parameters section to the schema
                if ( bOutParamsFound )
                {
                    xsParentChoice.Items.Add( xsElementParameters );		  //<xs:element name="OutParameters">
                }

                //Check to see if this is a receive port. If it is we need to add the adapter meta data to the header
                //of the schema.
                if ( _portType == PortType.Receive )
                {
                    XmlSchemaObject objTmp = xsdQueryConfiguration.Items[ 0 ];
                    //Hack to force the header infor to be at the top of the schema
                    xsdQueryConfiguration.Items.RemoveAt( 0 );
                    xsdQueryConfiguration.Items.Add( AdapterHeaderAnnotationSection( odbcCmd.CommandText ) );
                    xsdQueryConfiguration.Items.Add( objTmp );
                }

                // Get the schema string
                StringBuilder sb = new StringBuilder( );
                StringWriter sw = new StringWriter( sb );
                xsdQueryConfiguration.Write( sw );

                string sReturnSchema = sb.ToString( );

                return sReturnSchema;
            }
            catch ( Exception e )
            {
                if ( odbcCmd.Connection.State == ConnectionState.Open )
                    odbcCmd.Connection.Close( );

                throw new Exception( "Error Occured while attempting to generate the output schema for the query: " + e.Message );
            }
        }

        private XmlQualifiedName ConvertODBCDataTypeToXMLDataType( string ODBCDataType )
        {
            switch ( ODBCDataType )
            {
                case "BigInt":
                    return new XmlQualifiedName( "long", "http://www.w3.org/2001/XMLSchema" );
                case "Binary":
                    return new XmlQualifiedName( "base64Binary", "http://www.w3.org/2001/XMLSchema" );
                case "Bit":
                    return new XmlQualifiedName( "boolean", "http://www.w3.org/2001/XMLSchema" );
                case "Char":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                case "DateTime":
                    return new XmlQualifiedName( "dateTime", "http://www.w3.org/2001/XMLSchema" );
                case "Decimal":
                    return new XmlQualifiedName( "decimal", "http://www.w3.org/2001/XMLSchema" );
                case "Double":
                    return new XmlQualifiedName( "double", "http://www.w3.org/2001/XMLSchema" );
                case "Image":
                    return new XmlQualifiedName( "base64Binary", "http://www.w3.org/2001/XMLSchema" );
                case "Int":
                    return new XmlQualifiedName( "int", "http://www.w3.org/2001/XMLSchema" );
                case "NChar":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                case "NText":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                case "Numeric":
                    return new XmlQualifiedName( "decimal", "http://www.w3.org/2001/XMLSchema" );
                case "NVarChar":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                case "Real":
                    return new XmlQualifiedName( "float", "http://www.w3.org/2001/XMLSchema" );
                case "SmallInt":
                    return new XmlQualifiedName( "short", "http://www.w3.org/2001/XMLSchema" );
                case "Text":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                case "TinyInt":
                    return new XmlQualifiedName( "unsignedByte", "http://www.w3.org/2001/XMLSchema" );
                case "UniqueIdentifier"://FIX THIS
                    return new XmlQualifiedName( "long", "http://www.w3.org/2001/XMLSchema" );
                case "VarBinary":
                    return new XmlQualifiedName( "base64Binary", "http://www.w3.org/2001/XMLSchema" );
                case "VarChar":
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
                default:
                    return new XmlQualifiedName( "string", "http://www.w3.org/2001/XMLSchema" );
            }
        }

        public bool CreateBizTalkSchema( )
        {
            OdbcCommand OdbcCmd;
            OdbcConnection OdbcCon = new OdbcConnection( _strDBConnection );

            if ( QueryCommandType == AdapterCommandType.SQL )
            {
                OdbcCmd = this.GenerateODBCCommandFromSQL( _strScript );
                //If they want to modify the command then we present the user with a dialog
                //that will allow them to override out default command settings. This is done
                //to help addess data type and testing issues with different databases
                if ( _ModifyCommand )
                {
                    ADOCommandOverride test = new ADOCommandOverride( );
                    test.ADOCommand = OdbcCmd;
                    test.ParentClass = this;
                    test.ShowDialog( );
                }
            }
            else
            {
                OdbcCmd = this.GenerateODBCCommandFromSP( _strScript );
            }

            InputSchema = ExtractODBCParametersSchema( OdbcCmd );

            //Only call the output schema functions if we need an output schema
            if ( this._StatementType != AdapterStatementType.Input )
            {
                OutputSchema = GenerateOutputSchema( OdbcCmd );
            }

            return true;
        }
    }
}