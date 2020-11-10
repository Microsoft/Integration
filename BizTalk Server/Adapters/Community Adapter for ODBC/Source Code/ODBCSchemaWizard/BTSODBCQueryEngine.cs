using System;
using System.Data.Odbc;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Data; 
using System.Text;
 
namespace Microsoft.BizTalk.Adapters.ODBC.SchemaWizard
{
	/// <summary>
	/// Summary description for BTSODBCQueryEngine.
	/// </summary>
	public class BTSODBCQueryEngine
	{
		public string ODBCConnectionString = "";
		public bool ODBCTransaction = false;
		
		public string DocumentRootNode ="";
		public string Namespace = "";

#if(DEBUG)
		public string DebugOutLocation = "";
#endif
		
		public BTSODBCQueryEngine()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private OdbcCommand CreateODBCCommandFromSchema(string QuerySchema)
		{
			StringReader sr = new StringReader(QuerySchema);
			OdbcCommand odbcCmd = new OdbcCommand();
			XmlSchema xsdQueryConfiguration = XmlSchema.Read(sr,null);
			
			foreach(object item in xsdQueryConfiguration.Items)
			{
				if(item is XmlSchemaAnnotation) // Then we are at the section of the schema containing
				{								// the odbc query spec
					XmlSchemaAnnotation xsAnnotation = (XmlSchemaAnnotation)item;
					XmlSchemaAppInfo xsAppInfo = (XmlSchemaAppInfo)xsAnnotation.Items[0]; 
					XmlNode[] xmlNodesQueryInfo =  xsAppInfo.Markup; 

					// Create our new command object for query processing
					odbcCmd.CommandText = xmlNodesQueryInfo[1].InnerText;

					//HACK TO ADDRESS OUT PARAMETERS FOR SP FIX THIS
//					OdbcParameter outParam = new OdbcParameter();
//					outParam.Direction= ParameterDirection.ReturnValue;
//					outParam.Size = 10; // THIS DOES MATTER FIX THIS:
//					odbcCmd.Parameters.Add(outParam);
 
				}
				if(item is XmlSchemaElement)   // Query Parameters Section of the schema
				{
					XmlSchemaElement xsParamsElement = (XmlSchemaElement)item;
					XmlSchemaComplexType xsCT = (XmlSchemaComplexType)xsParamsElement.SchemaType;
					// IMPORTANT NOTE: If for some reason they change the order of the params in the 
					// schmema and/or the document being processes then the results will be wrong
					// as there is no way to map the data to the correct element of the query
					// remember our SQL looks like SELECT * FROM CUSTOMER WHERE CUSTOMERID = ?
					foreach(XmlSchemaAttribute xsAttParams in xsCT.Attributes)
					{
						OdbcParameter queryParam = new OdbcParameter();

							queryParam.ParameterName = "@" + xsAttParams.Name;

							//Now get all of the meta data from the parameter
							XmlSchemaAppInfo xsParamInfo = (XmlSchemaAppInfo)xsAttParams.Annotation.Items[0];
							XmlNode[] xmlParamInfo = xsParamInfo.Markup;
                        
							queryParam.OdbcType = ConvertStringDataTypeToODBC(xmlParamInfo[0].InnerText);
							queryParam.Direction = ConverStrToParamDir(xmlParamInfo[1].InnerText); 
							
							if(System.Convert.ToInt32(xmlParamInfo[2].InnerText) > 0)
								queryParam.Size = System.Convert.ToInt32(xmlParamInfo[2].InnerText); 

                            odbcCmd.Parameters.Add(queryParam);
					}
				}

			}
			  
			return odbcCmd;
		}
		/*
		 * SIMULATES BTS CODE EXECUTION FOR MESSGAE PROCESSING
		 */
		public string BTSExecuteODBCCall(string BTSDocument, string QuerySchema)
		{
			OdbcCommand odbcCmd = CreateODBCCommandFromSchema(QuerySchema);
			odbcCmd.Connection = new OdbcConnection(ODBCConnectionString);
			string Output = "";
			bool bOutputParametersInQuery = false;
			OdbcTransaction localDBTransaction = null;
			
			XmlDocument xmlDoc = new XmlDocument();
			
			XmlNamespaceManager xmlNSM = new XmlNamespaceManager(xmlDoc.NameTable);

			// WARNING: Should be converted to stream code I would suggest.
			xmlDoc.LoadXml(BTSDocument);

			// Need to lookup the NS prefix for the payload section of the schema.
			// Concerned that the root node will not always know what the NS is for our playload
			// section of the document
			// WARNING: Possible issue below
			XmlNode root = xmlDoc.FirstChild;

			string sNSPrefix = root.GetPrefixOfNamespace(Namespace); 
			xmlNSM.AddNamespace(sNSPrefix, Namespace);
			
			// Now lets get the payload section of the document and process the queries
			XmlNodeList xmlNL = xmlDoc.SelectNodes("/" + sNSPrefix + ":" + DocumentRootNode, xmlNSM);
			
			UnicodeEncoding Unicode = new UnicodeEncoding();
			
			foreach(XmlNode xmlN in xmlNL)
			{
				int xmlInboudMsgAttribCnt = 0;
				//WE NEED TO SKIP THIS FIRST ELEMENT WHICH IS THE ReturnValue
				for(int cnt = 0; cnt < odbcCmd.Parameters.Count; cnt++)
				{
					if(odbcCmd.Parameters[cnt].Direction == ParameterDirection.ReturnValue 
						|| odbcCmd.Parameters[cnt].Direction == ParameterDirection.Output 
						||odbcCmd.Parameters[cnt].Direction == ParameterDirection.InputOutput) 
					{
						// Add a flag so we know that we need to add out parameters to our ouput xml document
						bOutputParametersInQuery = true;
					}

					//WE also need to check if this is an input or output value
					if(odbcCmd.Parameters[cnt].Direction != ParameterDirection.ReturnValue && odbcCmd.Parameters[cnt].Direction != ParameterDirection.Output) 
					{
						
						// Even though the command object has some capabilities to perform
						// data type conversions, we still need to manually convert many of them.
						// So insted of picking and choosing we are just going to convert them all!
						switch(odbcCmd.Parameters[cnt].OdbcType.ToString())
						{
							case "BigInt":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToInt64(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Binary": 
								odbcCmd.Parameters[cnt].Value =  Unicode.GetBytes(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Bit":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToBoolean(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Text": 
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "DateTime":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDateTime(xmlN.Attributes[xmlInboudMsgAttribCnt].Value); 
								break;
							case "Numeric":  
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDecimal(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Double":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDouble(xmlN.Attributes[xmlInboudMsgAttribCnt].Value); 
								break;
							case "Int":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToInt32(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Real":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDouble(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "SmallInt":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToInt16(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "TinyInt":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToByte(xmlN.Attributes[xmlInboudMsgAttribCnt].Value); 
								break;
							case "UniqueIdentifier": //FIX THIS: Need to do something else!
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Char" : //FIX THIS NEEDS TO SUPPORT ARRAYS
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);//.ToCharArray();
								break;
							case "Date": // THIS IS A BEST GUESS
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDateTime(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Decimal":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDecimal(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Image":
								odbcCmd.Parameters[cnt].Value =  Unicode.GetBytes(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "NChar":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "NText":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "NVarChar": 
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "SmallDateTime":  // BEST GUESS
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDateTime(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Time": // BEST GUESS
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDateTime(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "Timestamp":
								odbcCmd.Parameters[cnt].Value =  Unicode.GetBytes(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "VarBinary": //FIX THIS
								odbcCmd.Parameters[cnt].Value =  Unicode.GetBytes(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							case "VarChar":
								odbcCmd.Parameters[cnt].Value = System.Convert.ToString(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
							default:
								odbcCmd.Parameters[cnt].Value = System.Convert.ToDateTime(xmlN.Attributes[xmlInboudMsgAttribCnt].Value);
								break;
						
						}
						xmlInboudMsgAttribCnt ++;
					}
				}
				
				try
				{
					// HERE we actually execute the query against the target database
					// We're leveraging ADO.NET to do alot of the heave lifting so it can deal with 
					// generateing the XML output for us
					OdbcDataAdapter odbcAdapter = new OdbcDataAdapter();
				
					//================================ BEGIN TRANSACTION ===================================
					if(ODBCTransaction) // Do we need to enlist the operation in a transaction
						localDBTransaction = odbcCmd.Connection.BeginTransaction ();

					odbcAdapter.SelectCommand = odbcCmd;

					DataSet ds = new DataSet();
					odbcAdapter.Fill(ds);
					Output = ds.GetXml();
#if(DEBUG)// write output to a specified file 
					if(DebugOutLocation != null)
					{
						ds.WriteXml("C:\\TEMP\\ADAPTEROUT.XML");
					}
#endif
					// =============================== END TRANSACTION =======================================
					if(ODBCTransaction) // Do we need to complete a transcation
						localDBTransaction.Commit(); 
				}
				catch(Exception e)
				{
					try
					{
						if(ODBCTransaction)
						{
							// ======================= ROLLBACK TRANSACTION ==================================
							localDBTransaction.Rollback();
						}
					}
					catch (Exception ex)
					{
						
					}

				}
				

				//Once we have the return data set and its corrisponding XML we need to add any outparameter
				//values to the end of that XML document

				

				
			}

 
			return Output; 
		}
		private ParameterDirection ConverStrToParamDir(string sParamDir)
		{
			switch(sParamDir)
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
		private OdbcType ConvertStringDataTypeToODBC(string sType)
		{
			switch(sType)
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
				case "Char" :
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
		
	}
}
