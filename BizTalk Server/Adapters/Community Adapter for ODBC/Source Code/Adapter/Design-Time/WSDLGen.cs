//---------------------------------------------------------------------
// File: WSDLGen.cs
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
using System.Xml;
using System.IO;
using System.Reflection;

namespace Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime
{
    public class WSDLGen
    {
        private static string WsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";

        public WSDLGen( ) { }

        private static string CreateWSDLFromTemplate( string MessageSchema, string targetNamespace, string RootElementName, string wsdlTemplate )
        {
            try
            {
                XmlDocument xsdDocument = new XmlDocument( );
                xsdDocument.LoadXml( MessageSchema );

                //  get the WSDL from the resources and set the targetNamespace
                XmlDocument wsdlDocument = GetWSDL( targetNamespace, wsdlTemplate );

                //  add the schema to the WSDL
                XmlNamespaceManager nsManager = new XmlNamespaceManager( new NameTable( ) );
                nsManager.AddNamespace( "wsdl", WsdlNamespace );

                XmlNode types = wsdlDocument.SelectSingleNode( "wsdl:definitions/wsdl:types", nsManager );
                XmlNode schema = wsdlDocument.ImportNode( xsdDocument.DocumentElement, true );
                types.AppendChild( schema );

                //  add other WSDL junk into the WSDL document
                XmlNode part = wsdlDocument.SelectSingleNode( "wsdl:definitions/wsdl:message/wsdl:part", nsManager );
                part.Attributes["element"].Value = "ODBC_" + RootElementName;

                // Make sure we change the schema name by modifing the ServiceName element in the WSDL
                XmlNode Servicepart = wsdlDocument.SelectSingleNode( "wsdl:definitions/wsdl:service", nsManager );
                Servicepart.Attributes["name"].Value = "ODBC_" + RootElementName;

                return wsdlDocument.OuterXml;
            }
            catch
            {
                return "";
            }
        }

        public static string CreateWSDL( string MessageSchema, string targetNamespace, string RootElementName, bool isRequestMessage )
        {
            if (isRequestMessage)
            {
                return CreateWSDLFromTemplate(MessageSchema, targetNamespace, RootElementName, "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.wsdlODBCSchemaReq.xml");
            }
            else
            {
                return CreateWSDLFromTemplate(MessageSchema, targetNamespace, RootElementName, "Microsoft.BizTalk.Adapters.ODBC.ODBCDesignTime.wsdlODBCSchemaResp.xml");
            }
        }

        private static XmlDocument GetWSDL( string targetNamespace, string wsdlTemplateName )
        {
            Stream stream = Assembly.GetExecutingAssembly( ).GetManifestResourceStream( wsdlTemplateName );
            StreamReader reader = null;
            string wsdlTemplate = null;

            using ( reader = new StreamReader( stream ) )
            {
                wsdlTemplate = reader.ReadToEnd( );
            }

            XmlDocument wsdlDocument = new XmlDocument( );
            wsdlDocument.LoadXml( wsdlTemplate );

            wsdlDocument.DocumentElement.SetAttribute( "xmlns", targetNamespace );
            wsdlDocument.DocumentElement.SetAttribute( "targetNamespace", targetNamespace );

            return wsdlDocument;
        }
    }
}