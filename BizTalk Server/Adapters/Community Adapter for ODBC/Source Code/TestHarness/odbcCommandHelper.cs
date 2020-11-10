using System;
using System.Data; 
using System.Data.Odbc;
using System.Collections;
using System.Xml;
using System.Xml.Schema; 
using System.IO;
using System.Text;


namespace Microsoft.BizTalk.Adapters.ODBC
{
	/// <summary>
	/// Summary description for odbcCommandHelper.
	/// </summary>
	public class odbcCommandHelper
	{
		public odbcCommandHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public string InputSchema = ""; // FIX THIS: SHould be read only
		public string OutputSchema = "";// FIX THIS: SHould be read only
		public AdapterCommandType QueryCommandType = AdapterCommandType.SQL;
		public AdapterStatementType QueryStatementType = AdapterStatementType.SELECT; 

		public enum AdapterCommandType
		{
			SQL,
			StoredProcedure
		}

		public enum PortType
		{
			Receive,
			Send
		}

		public enum AdapterStatementType
		{
			INSERT,
			UPDATE,
			DELETE,
			SELECT
		}

		private string m_strDBConnection = string.Empty;
		private string m_strScript = string.Empty;
		private string m_strTableName = string.Empty;
		private AdapterStatementType m_statementType = AdapterStatementType.SELECT;
		private AdapterCommandType m_updateType;//FIX THIS
		private PortType m_portType;
		private string m_strInputRoot = string.Empty;
		private string m_strOutputRoot = string.Empty;
		private string m_strSPName = string.Empty;
		private string m_strTargetNamespace = string.Empty;
		private string m_strGeneratedScript = string.Empty;
		private string m_strWSDL = string.Empty;
		#region properties

		public string strDBConnection 
		{
			get { return m_strDBConnection ; }
			set { m_strDBConnection  = value; }
		}

		public string strScript 
		{
			get { return m_strScript ; }
			set { m_strScript  = value; }
		}

		public string strTableName 
		{
			get { return m_strTableName ; }
			set { m_strTableName  = value; }
		}

		public AdapterStatementType SQLScriptType  
		{
			get { return m_statementType ; }
			set { m_statementType  = value; }
		}
		
		public AdapterCommandType UpdategramType   // FIX THIS:
		{
			get { return m_updateType ; }
			set { m_updateType  = value; }
		}

		public PortType portType 
		{
			get { return m_portType ; }
			set { m_portType  = value; }
		}

		public string strInputRoot 
		{
			get { return m_strInputRoot ; }
			set { m_strInputRoot  = value; }
		}

		public string strOutputRoot 
		{
			get { return m_strOutputRoot ; }
			set { m_strOutputRoot  = value; }
		}

		public string strSPName 
		{
			get { return m_strSPName ; }
			set { m_strSPName  = value; }
		}

		public string strTargetNamespace 
		{
			get { return m_strTargetNamespace ; }
			set { m_strTargetNamespace  = value; }
		}

		public string strGeneratedScript 
		{
			get { return m_strGeneratedScript ; }
			set { m_strGeneratedScript  = value; }
		}

		public string strWSDL 
		{
			get { return m_strWSDL; }
			set { m_strWSDL = value; }
		}
# endregion


		public DataSet ExtractODBCParametersSchema(OdbcCommand odbcCmd, string SchemaElementName)
		{
			DataSet dsCommand = new DataSet("Command");
			DataTable dtParameters = new DataTable(SchemaElementName);

			//Loop throught the ODBC Command object looking for in parameters
			foreach(OdbcParameter odbcParam in odbcCmd.Parameters)
			{

				//Build a Data Set based on the parameters so we can let ADO.NET
				//Take care of the type conversion between Database and XML

				//We are buiding the inbound schema so we don't want out parameters
				if(odbcParam.Direction == ParameterDirection.Input || odbcParam.Direction == ParameterDirection.InputOutput)
				{
					DataColumn dcParameter = new DataColumn(odbcParam.ParameterName.Replace("@",""),Type.GetType("System.String"),"",MappingType.Attribute);
					dtParameters.Columns.Add(dcParameter);
				}

			}

			dsCommand.Tables.Add(dtParameters);

			//Get the parameters schema out of the data set
			return dsCommand;
		}
		public string ExtractODBCParametersSchema(OdbcCommand odbcCmd)
		{
			XmlSchema xsBTSInboundMsg = new XmlSchema();
			
			//Create a stringwriter for output
			//FileStream file = new FileStream (FileName, FileMode.Create, FileAccess.ReadWrite);
			//XmlTextWriter myXmlWriter = new XmlTextWriter (file, new UTF8Encoding());

			// Setup schema formatting
			//myXmlWriter.Formatting = Formatting.Indented;
			//myXmlWriter.Indentation = 2;

			xsBTSInboundMsg.AttributeFormDefault = XmlSchemaForm.Unqualified;
			xsBTSInboundMsg.ElementFormDefault = XmlSchemaForm.Qualified;

			xsBTSInboundMsg.TargetNamespace = m_strTargetNamespace; //"http://ODBCADAPTERTEST.com"; //FIX THIS: needs to be a parameter
			xsBTSInboundMsg.Namespaces.Add("msbtsodbc", "http://schemas.microsoft.com/BizTalk/2003");
			//====================== SCHEMA ANNOTATION ================================
			// Create the Schema object necessary to contain the annotation info
			// this section will contain the SQL statements that the adapter processes

			//			<xs:annotation>
			//				<xs:appinfo>
			//					<msbtsodbc:SQLCMD xmlns:msbtsodbc="http://test">SELECT * FROM DEMO WHERE DEI = @DEI</msbtsodbc:SQLCMD> 
			//					<msbtsodbc:ODBCCMD xmlns:msbtsodbc="http://test">SELECT * FROM DEMO WHERE DEI = ?</msbtsodbc:ODBCCMD> 
			//				</xs:appinfo>
			//			</xs:annotation>

			XmlSchemaAnnotation xsAnnotation = new XmlSchemaAnnotation();
			XmlSchemaAppInfo xsAppInfo = new XmlSchemaAppInfo();	
            
			// AppInfo class uses an array of nodes to contain config data 
			// so we need to use the XMLDOM to create these nodes
			XmlDocument xmlWorkingDoc = new XmlDocument();
			
			XmlNode[] xmlNodeAdapterConfig = new XmlNode[2];

			// Save out the query with parameter information
			xmlNodeAdapterConfig[0] = xmlWorkingDoc.CreateNode(XmlNodeType.Element ,"msbtsodbc","SQLCMD","http://schemas.microsoft.com/BizTalk/2003");
			xmlNodeAdapterConfig[0].InnerText = m_strScript;
			
			// Save out generic query with ODBC escape sequence
			xmlNodeAdapterConfig[1] = xmlWorkingDoc.CreateNode(XmlNodeType.Element ,"msbtsodbc","ODBCCMD","http://schemas.microsoft.com/BizTalk/2003");
			xmlNodeAdapterConfig[1].InnerText = odbcCmd.CommandText;
			
			// Write the configuration to the schema
			xsBTSInboundMsg.Items.Add(xsAnnotation);
			xsAnnotation.Items.Add(xsAppInfo);
			xsAppInfo.Markup = xmlNodeAdapterConfig;
			 
			//==================== QUERY PARAMETERS SCHEMA SECTION ====================
            
			XmlSchemaElement xsElementParameters = new XmlSchemaElement();
			xsElementParameters.Name = m_strInputRoot;
        
            XmlSchemaComplexType xsComplexType = new XmlSchemaComplexType();
			
			xsBTSInboundMsg.Items.Add(xsElementParameters);		  //<xs:element name="InsertDocument">

			xsElementParameters.SchemaType = xsComplexType;	  //	<xs:complexType>
		
			// Loop through the parameter section of the command object and create a
			// schema attribute for each
			//Loop throught the ODBC Command object looking for in parameters
			foreach(OdbcParameter odbcParam in odbcCmd.Parameters)
			{
								
				XmlSchemaAttribute xsAttribute = new XmlSchemaAttribute();
				xsAttribute.Name = odbcParam.ParameterName.Replace("@","");
				xsAttribute.SchemaTypeName = ConvertODBCDataTypeToXMLDataType(odbcParam.OdbcType.ToString()); 
				
				// SAVE Out the meta data for this parameter in an appinfo section of the schema
				// this is done to deal with the name space problems we run into etx with special
				// attributes
				xsAnnotation = new XmlSchemaAnnotation(); // These vars were already define earlier
				xsAppInfo = new XmlSchemaAppInfo();	
        
				// AppInfo class uses an array of nodes to contain config data 
				// so we need to use the XMLDOM to create these nodes
				xmlWorkingDoc = new XmlDocument();
		
				xmlNodeAdapterConfig = new XmlNode[3];

				// Save out ODBC data type for parameter
				xmlNodeAdapterConfig[0] = xmlWorkingDoc.CreateNode(XmlNodeType.Element ,"msbtsdbc","ParamType","http://schemas.microsoft.com/BizTalk/2003");
				xmlNodeAdapterConfig[0].InnerText = odbcParam.OdbcType.ToString();
		
				// Save out the parameter direction
				xmlNodeAdapterConfig[1] = xmlWorkingDoc.CreateNode(XmlNodeType.Element ,"msbtsodbc","ParamDir","http://schemas.microsoft.com/BizTalk/2003");
				xmlNodeAdapterConfig[1].InnerText = odbcParam.Direction.ToString();

				// Save out the parameters data lenght
				xmlNodeAdapterConfig[2] = xmlWorkingDoc.CreateNode(XmlNodeType.Element ,"msbtsodbc","ParamSize","http://schemas.microsoft.com/BizTalk/2003");
				xmlNodeAdapterConfig[2].InnerText = odbcParam.Size.ToString();

				// Write the configuration to the schema
				xsAttribute.Annotation = xsAnnotation;
				xsAnnotation.Items.Add(xsAppInfo);
				xsAppInfo.Markup = xmlNodeAdapterConfig;
				
				xsComplexType.Attributes.Add(xsAttribute); //			<xs:attribute name="DocumentID" type="xs:string" /> 

															//			<xs:attribute name="Revision" type="xs:string" /> 
															// etc...


			}
		
			// Save the schema to the provided file
			//xsBTSInboundMsg.Write(myXmlWriter);
			//file.Flush();
			//file.Close();
 
			// Get the schema string
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			xsBTSInboundMsg.Write(sw);

			string sReturnSchema = sb.ToString();

			return sReturnSchema;
		}

		/*
		 * This function exists as the ODBC provider in .NET does not like
		 * named parameters as part of database calls. So we will convert
		 * the command parameters collection and command text into a generic SQL
		 * statement.
		 */
		public string BuildSQLSPCommandString(OdbcCommand odbcCmd)
		{
			string sSQLCommand = "";				//{ @RC = CALL StoredProc(@Param1, @Param2)}
													// This will be used to mapping to the XML
													// durning transmission

			string sGenericSPEscapeSequence = "";   //{ ? = CALL StoredProc(?, ?)}
													// This will be the actual call made by the
													// ODBC command object for the transmission

			// The first parameter in the parameters collection normally 
			// maintains the return value
			sSQLCommand = "{" + odbcCmd.Parameters[0].ParameterName + " = ";
 
			sGenericSPEscapeSequence = "{? =";

			//Embed the sp execute statement
			sSQLCommand += "CALL " + odbcCmd.CommandText;
			sGenericSPEscapeSequence += "CALL " + odbcCmd.CommandText;

			//Set up for input parameters section of SP
			sSQLCommand += "(";
			sGenericSPEscapeSequence += "(";

			//Now we loop thru the remaining parameters and build the SP statement
			for(int cnt = 1;cnt < odbcCmd.Parameters.Count; cnt++ )
			{
				sSQLCommand +=  odbcCmd.Parameters[cnt].ParameterName;
				sGenericSPEscapeSequence += "?";

				if(cnt+1 < odbcCmd.Parameters.Count)
				{
					sSQLCommand += ",";
					sGenericSPEscapeSequence += ",";
				}
			}

			//Complete the statement syntax
			sSQLCommand += ")}";
			sGenericSPEscapeSequence += ")}";

			m_strGeneratedScript = sGenericSPEscapeSequence;
			return sGenericSPEscapeSequence;
	}

		public OdbcCommand GenerateODBCCommandFromSP(string sql)
		{
			//To resove the parameters for a given SP we need a live connection
			OdbcConnection odbcCon = new OdbcConnection(m_strDBConnection);
			OdbcCommand  odbcCmd = new OdbcCommand(sql, odbcCon); 

			odbcCmd.CommandType = CommandType.StoredProcedure; 	

			odbcCon.Open();
			
			// Have the ODBC Namespace resolve the parameters
			OdbcCommandBuilder.DeriveParameters(odbcCmd);

			odbcCon.Close();

			//Now we'll loop thur the parameters set some bogus test data
			foreach(OdbcParameter odbcParam in odbcCmd.Parameters)
			{
				if(odbcParam.Direction == ParameterDirection.Input)
				{
					switch(odbcParam.OdbcType)
					{
						case OdbcType.DateTime:
							odbcParam.Value = "'1/1/2001'";
							break;
						case OdbcType.Int:
							odbcParam.Value ="1";
							break;
						case OdbcType.Text: 
							odbcParam.Value ="T";
							break;
						default:
							odbcParam.Value = "1"; // This is just a guess on a value type that seems
							break;                 // to work generically
					}
				}
				
			}

			// Now we need to process the command and generate a generic escape sequence for the SQL
			// the ODBC provider does NOT like named parameters

			odbcCmd.CommandText = BuildSQLSPCommandString(odbcCmd);

			return odbcCmd;

		
		}
		public OdbcCommand GenerateODBCCommandFromSQL(string sql)
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
			sql = sql.Replace("\r\n"," "); 

			OdbcConnection odbcCon = new OdbcConnection(m_strDBConnection);
			OdbcCommand dbCmdODBC = new OdbcCommand();
			dbCmdODBC.Connection = odbcCon;
			dbCmdODBC.CommandType = CommandType.Text; 

			// Prep the command to deal with no spaces between commands etc
			// (@VALUE1,@VALUE2) 

			sql = sql.Replace(","," ,");
			sql = sql.Replace("("," (");
			sql = sql.Replace(")"," )");
			
			do
			{
				//Now we need to shread the SQL the user provided us with to pull out
				//the parameters. These will be used to build in the input schema

				//                                            V
				// SELECT * FROM CUSTOMERS WHERE CUSTOMERID = @CUSTOMERID AND.....
				iParamStartIndex = sql.IndexOf("@",iParamEndIndex);

				//If we run out of parameters get out of the loop
				if(iParamStartIndex == -1)
					break;
//++++++ NEED TO ADD A SECTION OF TYPE OF QUERY UPDATE, SQL SP etc
				//                                                       V
				// SELECT * FROM CUSTOMERS WHERE CUSTOMERID = @CUSTOMERID AND.....
				iParamEndIndex = sql.IndexOf(" ",iParamStartIndex);



						//We could be at the end of the string so we need to check
						if(iParamEndIndex == -1)
					iParamEndIndex = sql.Length;

				//Figure out how big the parameter term is so we only extract it from the string
				iParamSize = iParamEndIndex - iParamStartIndex; 
				sParameterName = sql.Substring(iParamStartIndex,iParamSize);

				// ========================= Process Parameters =======================
				//Add a new parameter to our command object
				OdbcParameter odbcParam = new OdbcParameter();
				
				odbcParam.ParameterName = sParameterName;
				odbcParam.Value = "1";
				odbcParam.Direction = ParameterDirection.Input;
				//odbcParam.DbType = CANT DETERMINE THIS!!!

				dbCmdODBC.Parameters.Add(odbcParam);

				//====================== Setup Generic Command SQL ====================
				sODBCSQLEscapeSequence = sODBCSQLEscapeSequence.Replace(sParameterName,"?");
			}
			while(iParamEndIndex != sql.Length);

			//Now set the command objects command text to our generic SQL 
			dbCmdODBC.CommandText = sODBCSQLEscapeSequence;

			//Publish it up to the class level as a property
			m_strGeneratedScript = sODBCSQLEscapeSequence;

			return dbCmdODBC;
		}
		public string GenerateOutputSchema(OdbcCommand odbcCmd)
		{
			bool bOutParamsFound = false;

			// Call the command to get the output schema
			OdbcDataAdapter OdbcAdapter = new OdbcDataAdapter();
			
			// Need to call a transaction to prevent writes during the generation
			// of an output schema.
			// We want to wrap this call in a transaction that we don't commit
			odbcCmd.Connection.Open();
			OdbcTransaction odbcTrans = odbcCmd.Connection.BeginTransaction();	

			odbcCmd.Transaction = odbcTrans;

			OdbcAdapter.SelectCommand = odbcCmd;
			DataSet custDS = new DataSet(m_strOutputRoot);
 
			//Load the data set to get the output schema
			OdbcAdapter.Fill(custDS);
			
			// Throw away any junk we may have written
			odbcTrans.Rollback();
			odbcCmd.Connection.Close();
 
			string xsResultsSchema = custDS.GetXmlSchema();

			//Now we need to extend the schema to include any output parameters that are included
			StringReader sr = new StringReader(xsResultsSchema);
			//Load the data set schema into the XML Schema Objects
			XmlSchema xsdQueryConfiguration = XmlSchema.Read(sr,null);
			
			
			//Now we add a section for the return value and out parameters
			//==================== QUERY PARAMETERS SCHEMA SECTION ====================
            
			XmlSchemaElement xsElementParameters = new XmlSchemaElement();
			xsElementParameters.Name = "OutParameters";
        
			XmlSchemaComplexType xsComplexType = new XmlSchemaComplexType();
			
			xsElementParameters.SchemaType = xsComplexType;	  //	<xs:complexType>
		
			// Loop through the parameter section of the command object and create a
			// schema attribute for each
			//Loop throught the ODBC Command object looking for in parameters
			foreach(OdbcParameter odbcParam in odbcCmd.Parameters)
			{
				// We only want to create elements for out bound parameters since this is the
				// adapters output schema
				if(odbcParam.Direction != ParameterDirection.Input)
				{
					bOutParamsFound = true;
					XmlSchemaAttribute xsAttribute = new XmlSchemaAttribute();
					xsAttribute.Name = odbcParam.ParameterName.Replace("@","");
					xsAttribute.SchemaTypeName = ConvertODBCDataTypeToXMLDataType(odbcParam.OdbcType.ToString());
					xsComplexType.Attributes.Add(xsAttribute); //			<xs:attribute name="DocumentID" type="xs:string" /> 
				}
			}

			// See if we need to add the out parameters section to the schema
			if(bOutParamsFound)
			{
				xsdQueryConfiguration.Items.Add(xsElementParameters);		  //<xs:element name="OutParameters">
			}

			// Get the schema string
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			xsdQueryConfiguration.Write(sw);

			string sReturnSchema = sb.ToString();

			return sReturnSchema;

		}
		private XmlQualifiedName ConvertODBCDataTypeToXMLDataType(string ODBCDataType)
		{
			switch(ODBCDataType)
			{
				case "BigInt":
					return new XmlQualifiedName("long","http://www.w3.org/2001/XMLSchema");
					break;
				case "Binary":
					return new XmlQualifiedName("base64Binary","http://www.w3.org/2001/XMLSchema");
					break;
				case "Bit":
					return new XmlQualifiedName("boolean","http://www.w3.org/2001/XMLSchema");
					break;
				case "Char":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				case "DateTime":
					return new XmlQualifiedName("dateTime","http://www.w3.org/2001/XMLSchema");
					break;
				case "Decimal":
					return new XmlQualifiedName("decimal","http://www.w3.org/2001/XMLSchema");
					break;
				case "Double":
					return new XmlQualifiedName("double","http://www.w3.org/2001/XMLSchema");
					break;
				case "Image":
					return new XmlQualifiedName("base64Binary","http://www.w3.org/2001/XMLSchema");
					break;
				case "Int":
					return new XmlQualifiedName("int","http://www.w3.org/2001/XMLSchema");
					break;
				case "NChar":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				case "NText":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				case "Numeric":
					return new XmlQualifiedName("decimal","http://www.w3.org/2001/XMLSchema");
					break;
				case "NVarChar":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				case "Real":
					return new XmlQualifiedName("float","http://www.w3.org/2001/XMLSchema");
					break;
				case "SmallInt":
					return new XmlQualifiedName("short","http://www.w3.org/2001/XMLSchema");
					break;
				case "Text":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				case "TinyInt":
					return new XmlQualifiedName("unsignedByte","http://www.w3.org/2001/XMLSchema");
					break;
				case "UniqueIdentifier"://FIX THIS
					return new XmlQualifiedName("long","http://www.w3.org/2001/XMLSchema");
					break;
				case "VarBinary":
					return new XmlQualifiedName("base64Binary","http://www.w3.org/2001/XMLSchema");
					break;
				case "VarChar":
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
					break;
				default:
					return new XmlQualifiedName("string","http://www.w3.org/2001/XMLSchema");
				

			}
		}
		public bool CreateBizTalkSchema()
		{
			OdbcCommand OdbcCmd; 
			OdbcConnection OdbcCon = new OdbcConnection(m_strDBConnection); 
				
			if(QueryCommandType == AdapterCommandType.SQL)
			{
				OdbcCmd = this.GenerateODBCCommandFromSQL(m_strScript); 
			}
			else
			{
				OdbcCmd = this.GenerateODBCCommandFromSP(m_strScript);  
			}

			InputSchema = ExtractODBCParametersSchema(OdbcCmd);
			OutputSchema = GenerateOutputSchema(OdbcCmd);


			return true;
		}
	}
}
