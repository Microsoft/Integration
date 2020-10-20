using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Microsoft.BizTalk.ExplorerOM;
using Microsoft.RuleEngine;
using Microsoft.Web.Administration;
using Application = Microsoft.BizTalk.ExplorerOM.Application;
using ApplicationCollection = Microsoft.BizTalk.ExplorerOM.ApplicationCollection;

namespace MigrationTool
{
    public partial class BizTalkAdminOperations : Form
    {
        #region Variables

        //operations variable
        private string _strWebSite,
            _strAppPool,
            _strCertificate,
            _strHostInstance,
            _strFileCopy,
            _strBam,
            _strHandlers,
            _strGlobalPartyBinding,
            _strBizTalkAssemblies,
            _strServices;

        private string _strBizTalkApp, _strExport;

        //flags
        private readonly string _strPerformOperationYes, _strPerformOperationNo, _strSuccess;

        private string _isAppPoolExecuted, _isHostExecuted, _isHandlerExecuted, _isBizTalkAppExecuted;

        private readonly int _strRoboCopySuccessCode;

        //user account will be used in WMI remoting  or PsExec remoting, while service account will be used for host instance.
        private string _strUserName,
            _strUserNameForWMI,
            _strPassword,
            _strDomain,
            _strSrcNode,
            _strDstNode,
            _strIsUtilCopied;

        private string _strUserNameForHost, _strPasswordForHost
            ; //this will act as service account, while other one will work as user account

        private readonly string _fileFolderPath,
            _vDir,
            _machineName,
            _exportedDataPath,
            _appPath,
            _msiPath,
            _xmlPath,
            _certPath,
            _logPath,
            _serviceFolderPath,
            _brePath;

        private readonly string _asmPath, _customDllPath, _gacUtilPath, _psExecPath, _serverXmlPath;

        private readonly string _asmFolderName,
            _gacUtilFolderName,
            _certFolderName,
            _customDllFolderName,
            _remoteExeName,
            _specFileExt;

        private string _srcSqlInstance,
            _srcBAMSqlInstance,
            _dstBAMSqlInstance,
            _dstSqlInstance,
            _loginOperationName,
            _srcBRESqlInstance,
            _dstBRESqlInstance;

        //app config
        private string _bizTalkAppToIgnore;

        private readonly string _bamExePath;

        private string _remoteRootFolder,
            _baseBizTalkAppCol,
            _strCertPass,
            _strFoldersToCopy,
            _strFoldersToCopyNoFiles,
            _strWebsitesFolder,
            _strFoldersDrive,
            _strServicesDrive;

        private string _strCustomDllToInclude, _strToolMode, _strServerType, _strWindowsServiceToIgnore;

        public delegate void SetTextCallback(string strMsg);

        #endregion

        public BizTalkAdminOperations() //constructor
        {
            _strWebSite = "n";
            _strAppPool = "n";
            _strCertificate = "n";
            _strHandlers = "n";
            _strGlobalPartyBinding = "n";
            _strBizTalkAssemblies = "n";
            _strBizTalkApp = "n";
            _strFileCopy = "n";
            _strPerformOperationYes = "y";
            _strPerformOperationNo = "n";
            _strSuccess = "1";
            _strRoboCopySuccessCode = 8;
            InitializeComponent();

            _appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            _exportedDataPath = _appPath + @"\ExportedData";

            _fileFolderPath = _exportedDataPath + "\\FileFolder";
            _serviceFolderPath = _exportedDataPath + "\\ServiceFolder";
            _vDir = _exportedDataPath + "\\VDir";

            _msiPath = _exportedDataPath + @"\MSI";
            _xmlPath = _exportedDataPath + @"\XMLFiles"; //xmlpath renamed to XmlBinding
            _certFolderName = "\\CERT";
            _certPath = _exportedDataPath + _certFolderName; //@"\CERT";
            var breFolderName = "\\BRE";
            _brePath = _exportedDataPath + breFolderName;
            _asmFolderName = "\\DLL";
            _asmPath = _exportedDataPath + _asmFolderName; //@"\DLL";
            _customDllFolderName = "\\CustomDLL";
            _customDllPath = _exportedDataPath + _customDllFolderName;
            _logPath = _appPath + @"\Logs";
            _gacUtilFolderName = "\\Util";
            _gacUtilPath = _appPath + _gacUtilFolderName + "\\GacUtil.exe";
            _psExecPath = _appPath + _gacUtilFolderName + "\\PsExec.exe";
            _remoteExeName = "RemoteOperations.exe";
            _serverXmlPath = _xmlPath + "\\Servers.xml";

            _specFileExt = "_Spec.xml";
            _isAppPoolExecuted = _strPerformOperationYes;
            _isHostExecuted = _strPerformOperationYes;
            _isHandlerExecuted = _strPerformOperationYes;
            _isBizTalkAppExecuted = _strPerformOperationYes;
            _strIsUtilCopied = _strPerformOperationNo;

            _remoteRootFolder =
                ConfigurationManager.AppSettings
                    ["RemoteRootFolder"]; // +"\\" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            _bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
            _bizTalkAppToIgnore = ConfigurationManager.AppSettings["BizTalkAppToIgnore"];
            _baseBizTalkAppCol = ConfigurationManager.AppSettings["AppToRefer"];
            _strCertPass = ConfigurationManager.AppSettings["CertPass"];
            _strFoldersToCopy = ConfigurationManager.AppSettings["FoldersToCopy"];
            _strFoldersToCopyNoFiles = ConfigurationManager.AppSettings["FoldersToCopyNoFiles"];
            _strCustomDllToInclude = ConfigurationManager.AppSettings["CustomDllToInclude"];
            _strWindowsServiceToIgnore = ConfigurationManager.AppSettings["WindowsServiceToIgnore"];
            _strWebsitesFolder = ConfigurationManager.AppSettings["WebSitesDriveDestination"];
            _strFoldersDrive = ConfigurationManager.AppSettings["FoldersDriveDestination"];
            _strServicesDrive = ConfigurationManager.AppSettings["ServicesDriveDestination"];

            _strToolMode = "Backup";
            _strServerType = "BizTalk";
            _machineName = Environment.MachineName;
        }


        #region FileFolders

        private void btnExportFolders_Click(object sender, EventArgs e)
        {
            char[] chrSep = {','};

            try
            {
                LogInfo("Folders: Export started.");

                if (_strServerType == "BizTalk" &&
                    (cmbBoxServerSrc.Items.Count == 0 || cmbBoxServerSrc.SelectedItem == null))
                {
                    LogShortErrorMsg("Please mention Src Sql Instance and select node from DropDown.");
                }
                else if (_strServerType == "App" && _strSrcNode == string.Empty)
                {
                    LogShortErrorMsg("Please mention Src App Server.");
                }
                else
                {
                    try
                    {
                        if (Directory.Exists(_fileFolderPath))
                        {
                            Directory.Delete(_fileFolderPath, true);
                            Directory.CreateDirectory(_fileFolderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Error while cleaning file folder, hence aboting folder export " +
                                            ex.Message + ", " + ex.StackTrace);
                    }

                    if (_strToolMode == "Backup") //if backup mode is set then copy to local.
                    {
                        string strSrc;
                        string commandArguments;
                        string strDst;
                        int returnCode;
                        try
                        {
                            if (_strFoldersToCopyNoFiles != string.Empty)
                            {
                                //folders list from app.config, no files                                     
                                //Folders to copy, no files, from APP.Config                         
                                LogInfo("Exporting only folder structure with permissions, without files.");
                                string[] arrFoldersToCopyNoFiles =
                                    _strFoldersToCopyNoFiles.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string folderPath in arrFoldersToCopyNoFiles)
                                {
                                    string folderPathTrimed = folderPath.Trim();
                                    string driveInfo = Path.GetPathRoot(folderPathTrimed);
                                    string driveLetter = driveInfo.Substring(0, 1);
                                    string pathWithoutDrive =
                                        folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);
                                    strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                    strDst = _fileFolderPath + "\\" +
                                             folderPathTrimed.Substring(
                                                 folderPath.LastIndexOf(
                                                     '\\')); //"\\\\" + strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                    LogInfo("Copy started from:" + strSrc + " To " + strDst);
                                    commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                                       "/E /COPY:ATSOU /R:1"; //copy folders only with all permissons
                                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                    if (returnCode < _strRoboCopySuccessCode)
                                        LogShortSuccessMsg("Success: Exported Folders.");
                                    else
                                        LogShortErrorMsg("Failed: Exporting Folders");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogErrorInfo("Exception while copying folders as mentioned in other folder text box.");
                            LogException(ex);
                        }

                        try
                        {
                            //folders list from app.config                             
                            string genFolders = _strFoldersToCopy;
                            if (genFolders != string.Empty)
                            {
                                LogInfo("Exporting both folder structure with permissons and files.");
                                string[] genFolderPath =
                                    genFolders.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string folderPath in genFolderPath)
                                {
                                    string folderPathTrimed = folderPath.Trim();
                                    string driveInfo = Path.GetPathRoot(folderPathTrimed);
                                    string driveLetter = driveInfo.Substring(0, 1);
                                    string pathWithoutDrive =
                                        folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);
                                    strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                    strDst = _fileFolderPath + "\\" +
                                             folderPathTrimed.Substring(folderPath.LastIndexOf('\\'));
                                    LogInfo("Copy started from: " + strSrc + " To: " + strDst);
                                    commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" +
                                                       strDst + "\" " +
                                                       "/E /COPYALL /MIN:1 /R:1"; // /xc /xn /xo exclude existing file and folders
                                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                    if (returnCode < _strRoboCopySuccessCode)
                                        LogShortSuccessMsg("Success: Exported Folders/Files.");
                                    else
                                        LogShortErrorMsg("Failed: Exporting Folders/Files.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogErrorInfo(
                                "Exception while copying folders as mentioned in App.Config for FoldersToCopyWithFiles key.");
                            LogException(ex);
                        }
                    }
                    else
                    {
                        LogInfo(
                            "As Folder Copy Mode is set to Migration hence at Import Click Folders will be moved directly from Source to Destination.");
                    }
                }
                //SharePermissons
                GetSharePermission();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnImportFolders_Click(object sender, EventArgs e)
        {
            char[] chrSep = {','};

            try
            {
                LogInfo("Folders: Import Started.");
                if (_strServerType == "BizTalk" &&
                    (cmbBoxServerDst.Items.Count == 0 || cmbBoxServerDst.SelectedItem == null))
                {
                    LogShortErrorMsg("Please mention Dst Sql Instance and select node from DropDown.");
                }
                else if (_strServerType == "App" && _strDstNode == string.Empty)
                {
                    LogShortErrorMsg("Please mention Dst App Server.");
                }
                else
                {
                    if (_strToolMode == "Migrate" && string.IsNullOrEmpty(_strSrcNode))
                        throw new InvalidOperationException("Please select source node from dropdown.");

                    string strSrc;
                    string strDst;
                    string commandArguments;
                    int returnCode;
                    try
                    {
                        //folders list from app.config, no files                                     
                        //Folders to copy, no files, from APP.Config                        
                        if (_strFoldersToCopyNoFiles != string.Empty)
                        {
                            LogInfo("Importing only folder strutcture with permissons, no files.");
                            string[] arrFoldersToCopyNoFiles =
                                _strFoldersToCopyNoFiles.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string folderPath in arrFoldersToCopyNoFiles)
                            {
                                string folderPathTrimed = folderPath.Trim();
                                string driveInfo = Path.GetPathRoot(folderPathTrimed);
                                string driveLetter = driveInfo.Substring(0, 1);
                                string pathWithoutDrive = folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);

                                if (_strToolMode == "Migrate"
                                ) //if mirgration then source will be Src otherwise it will be local
                                    strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                else
                                    strSrc = _fileFolderPath + "\\" +
                                             folderPathTrimed.Substring(folderPath.LastIndexOf('\\'));
                                if (string.IsNullOrEmpty(_strFoldersDrive) ||
                                    string.IsNullOrWhiteSpace(_strFoldersDrive))
                                    strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                else
                                    strDst = "\\\\" + _strDstNode + "\\" + _strFoldersDrive.Trim().Substring(0, 1) +
                                             "$\\" + pathWithoutDrive;
                                LogInfo("Copy started from: " + strSrc + " To " + strDst);
                                commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                                   "/E /COPY:ATSOU /R:1"; //copy folders only with all permissons
                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                if (returnCode < _strRoboCopySuccessCode)
                                    LogShortSuccessMsg("Success: Imported Folders.");
                                else
                                    LogShortErrorMsg("Failed: Importing Folders.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogErrorInfo("Exception while copying folders as mentioned in other folder text box.");
                        LogException(ex);
                    }


                    try
                    {
                        //folders list from app.config                     
                        string genFolders = _strFoldersToCopy;
                        if (genFolders != string.Empty)
                        {
                            LogInfo("Importing both folder strutcture with permissons and files.");
                            string[] genFolderPath = genFolders.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string folderPath in genFolderPath)
                            {
                                string folderPathTrimed = folderPath.Trim();
                                string driveInfo = Path.GetPathRoot(folderPathTrimed);
                                string driveLetter = driveInfo.Substring(0, 1);
                                string pathWithoutDrive = folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);

                                if (_strToolMode == "Migrate")
                                    strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                                else
                                    strSrc = _fileFolderPath + "\\" +
                                             folderPathTrimed.Substring(folderPath.LastIndexOf('\\'));


                                strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;

                                LogInfo("Copy started from: " + strSrc + " To " + strDst);
                                commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst +
                                                   "\" " +
                                                   "/E /COPYALL /MIN:1 /R:1"; // /xc /xn /xo exclude existing file and folders
                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                if (returnCode < _strRoboCopySuccessCode)
                                    LogShortSuccessMsg("Success: Imported Folders/Files.");
                                else
                                    LogShortErrorMsg("Failed: Importing Folders/Files.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogErrorInfo(
                            "Exception while copying folders as mentioned in App.Config for FoldersToCopyWithFiles key.");
                        LogException(ex);
                    }
                }

                SetSharePermission();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion

        #region Host

        private void btnGetHost_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            try
            {
                LogInfo("Host Instances: Export started.");
                LogInfo("Connecting to BizTalkMgmtdb...." + txtConnectionString.Text);

                Hosts hosts;
                using (var connection = new SqlConnection("Server=" + txtConnectionString.Text.Trim() +
                                                          ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    connection.Open();
                    using (var sqlCmd = new SqlCommand(
                        "SELECT Name HostName,NTGroupName, cast(HostTracking as bit) HostTracking, cast(AuthTrusted as bit) Trusted, CASE WHEN HostType = 1 THEN 1 ELSE 0 END AS HostType, IsHost32BitOnly Is32Bit FROM adm_host",
                        connection))
                    {
                        using (var sqlDataAd = new SqlDataAdapter(sqlCmd))
                        {
                            using (var ds = new DataSet())
                            {
                                sqlDataAd.Fill(ds);

                                ds.Tables[0].TableName = "Host";

                                hosts = new Hosts
                                {
                                    Host = ds.Tables["Host"].Rows.Cast<DataRow>().Select(dRow => new HostsHost
                                    {
                                        authenticationTrusted = Convert.ToBoolean(dRow["Trusted"]),
                                        hostTracking = Convert.ToBoolean(dRow["HostTracking"]),
                                        inProcess = Convert.ToBoolean(dRow["HostType"]),
                                        is32bit = Convert.ToBoolean(dRow["Is32Bit"]),
                                        isDefaultHost = false,
                                        name = dRow["HostName"].ToString(),
                                        ntGroupName = dRow["NTGroupName"].ToString(),
                                        HostInstance = new[]
                                        {
                                            new HostsHostHostInstance
                                            {
                                                server = "",
                                                password = "",
                                                userName = ""
                                            }

                                        }
                                    }).ToArray()
                                };
                            }
                        }
                    }
                }

                SerializeObject(hosts, _xmlPath + @"\HostInstances.xml");

                LogShortSuccessMsg("Success: Exported Host Instances.");
                //Exporting HostSettings
                LogInfo("Host Settings: Export started.");
                string commandArguments;
                string xmlPathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(_xmlPath);
                int result;
                if (_machineName == _strSrcNode)
                {
                    commandArguments = "ExportSettings -Destination:\"" + xmlPathUnc + "\\" + "HostSettings.xml\"" +
                                       " -Server:\"" + txtConnectionString.Text.Trim() + "\" -Database:\"" +
                                       "BizTalkMgmtDb\"";
                    //Create and start BTSTask.exe process                    
                    result = ExecuteCmd("BTSTask.exe", commandArguments);
                }
                else
                {
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       " BTSTASK ExportSettings -Destination:\"" + xmlPathUnc + "\\" +
                                       "HostSettings.xml\"" + " -Server:\"" + txtConnectionString.Text.Trim() +
                                       "\" -Database:\"" + "BizTalkMgmtDb\"";
                    result = ExecuteCmd("CMD.EXE", commandArguments);
                }

                if (result == 0)
                    LogShortSuccessMsg("Success: Exported Host Settings");
                else
                    LogShortErrorMsg("Failed: Exporting Host Settings.");
                //Exporting HostMapping Settings
                String[] files = Directory.GetFiles(_xmlPath, "Src_*_HostMappings.xml");

                foreach (string file in files)
                {
                    File.Delete(file);
                }
                XmlDocument xd = new XmlDocument();
                if (_machineName == _strSrcNode)
                {
                    LogInfo("Host Mappings: Export started.");

                    ExportHostMapSettings();
                    LogShortSuccessMsg("Success: Exported Host Mappings.");
                }
                else //Remote
                {
                    LogInfo("Host Mappings: Export started.");
                    xd.Load(_xmlPath + "\\" + "Servers.xml");
                    XmlNode srcnodeList = xd.DocumentElement.SelectSingleNode("/Servers");
                    string srcServerList = srcnodeList.SelectSingleNode("SrcNodes").InnerText;
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ExportHostMapSettings\" \"" +
                                       txtConnectionString.Text.Trim() + "\" \"" + srcServerList + "\"\"";
                    int returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate app xml and save it back to local

                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Exported Host Mappings.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting Host Mappings.");

                    }

                }
                //Creating Src ServerList.xml
                xd.Load(_serverXmlPath);
                if (File.Exists(_xmlPath + "\\" + "SrcServers.xml"))
                    File.Delete(_xmlPath + "\\" + "SrcServers.xml");
                xd.Save(_xmlPath + "\\" + "SrcServers.xml");
                LogInfoInLogFile("Created SourceServerList");

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnSetHostAndHostInstances_Click(object sender, EventArgs e)
        {
            string commandArguments;

            //  string dstHostList = string.Empty, dstHostInstanceList = string.Empty;
            List<string> dstHostList = new List<string>();
            try
            {
                if (_machineName == _strDstNode)
                {
                    LogInfo("Host: Import started..");
                    if (!File.Exists(_xmlPath + @"\HostInstances.xml"))
                        throw new FileNotFoundException("HostInstances xml file does not exist.");
                    //check file is empty or not
                    XmlDocument doc = new XmlDocument();
                    doc.Load(_xmlPath + "\\HostInstances.xml");
                    if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                        throw new InvalidOperationException("HostInstances xml file is empty.");

                    var input = DeserializeObject<Hosts>(_xmlPath + @"\HostInstances.xml");

                    //get all HostInstances of 'InProcess' type.

                    EnumerationOptions enumOptions = new EnumerationOptions {ReturnImmediately = false};


                    //Creating DestinationHostList
                    using (var searchObjectHost = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_Host", enumOptions))
                    {
                        dstHostList.AddRange(from ManagementBaseObject inst in searchObjectHost.Get()
                            select inst["Name"].ToString().ToUpper());
                    }
                    //Creating DestinationHosts
                    foreach (HostsHost host in input.Host)
                    {
                        if (!dstHostList.Contains(host.name.ToUpper()))
                        {
                            CreateHost(host.name,
                                host.inProcess
                                    ? HostSetting.HostTypeValues.InProcess
                                    : HostSetting.HostTypeValues.Isolated, host.ntGroupName, host.authenticationTrusted,
                                host.hostTracking, host.is32bit,
                                host.isDefaultHost);
                        }
                        else
                            LogInfo("Host already exist: " + host.name);
                    }

                    foreach (object serverDstItem in cmbBoxServerDst.Items)
                    {
//Creating DestinationHostInstanceListForeachnode
                        List<string> dstHostInstanceList;
                        using (var searchObjectHostInstance = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance", enumOptions))
                        {
                            dstHostInstanceList = searchObjectHostInstance.Get()
                                .Cast<ManagementBaseObject>()
                                .Where(inst => inst["RunningServer"].ToString().ToUpper() == serverDstItem.ToString())
                                .Select(inst => inst["HostName"].ToString().ToUpper())
                                .ToList();
                        }
                        //Create DestinationHostInstance for EachNode
                        foreach (HostsHost host in input.Host)
                        {
                            if (!dstHostInstanceList.Contains(host.name.ToUpper()))
                            {
                                CreateHostInstance(serverDstItem.ToString(), host.name, _strUserName,
                                    _strPassword);
                            }
                            else
                                LogInfo("Host Instance already exist: " + host.name + " on " +
                                        serverDstItem);
                        }
                    }

                    _isHostExecuted = _strPerformOperationYes;
                }
                else
                {

                    string appPathUnc = ConvertPathToUncPath(_appPath);

                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ImportHosts\" \"" + _strUserNameForHost + "\" \"" +
                                       _strPasswordForHost + "\"\"";
                    int returnCode =
                        ExecuteCmd("CMD.EXE",
                            commandArguments); //dlls will be copied and pasted back to Local machine, hence machineName used in commandArgument.

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Imported Host and HostInstances.");
                    else
                        LogShortErrorMsg("Failed: Importing Host and HostInstances.");
                }
            }
            catch (Exception ex)
            {
                _isHostExecuted = _strPerformOperationNo;
                LogException(ex);
            }
            try
            {

                if (!File.Exists(_xmlPath + "\\" + "HostSettings.xml"))
                {
                    throw new FileNotFoundException("Host Settings xml is not Present.");
                }

                String[] files = Directory.GetFiles(_xmlPath, "Dst_*_HostMappings.xml");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                if (_machineName == _strDstNode)
                {
                    ImportHostMapSettings();
                }
                else
                {

                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\"  \"\\\\" + _machineName + "\\" +
                                       appPathUnc + "\" \"ImportHostMapSettings\" \"" +
                                       txtConnectionStringDst.Text.Trim() + "\"\"";
                    int returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Created Destination Host Mapping Xml File");
                    else
                        LogShortErrorMsg("Failed: Creating Destination Host Mapping Xml File");
                }
                files = Directory.GetFiles(_xmlPath, "Dst_*_HostMappings.xml");
                if (files.Length != 0)
                {
                    foreach (string file in files)
                    {
                        LogInfo("Host Settings: Import started.");
                        string xmlPathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(_xmlPath);
                        string filePathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(file);
                        int result;
                        if (_machineName == _strDstNode)
                        {
                            commandArguments = "ImportSettings -Source:\"" + xmlPathUnc + "\\" + "HostSettings.xml\"" +
                                               " -Map:\"" + filePathUnc + "\" -Server:\"" +
                                               txtConnectionStringDst.Text.Trim() + "\" -Database:\"" +
                                               "BizTalkMgmtDb\"";
                            //Create and start BTSTask.exe process            
                            result = ExecuteCmd("BTSTask.exe", commandArguments);
                        }
                        else //Remote
                        {
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode + " -u " + "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + " BTSTASK ImportSettings -Source:\"" + xmlPathUnc +
                                               "\\" + "HostSettings.xml\"" + " -Map:\"" + filePathUnc +
                                               "\" -Server:\"" + txtConnectionStringDst.Text.Trim() +
                                               "\" -Database:\"" + "BizTalkMgmtDb\"";
                            result = ExecuteCmd("CMD.EXE", commandArguments);
                        }

                        if (result == 0)
                            LogShortSuccessMsg("Success: Imported Host Settings on " +
                                               Path.GetFileNameWithoutExtension(file).Split('_')[1].Split('_')[0] +
                                               " Server.");
                        else
                            LogShortErrorMsg("Failed: Importing Host Settings on" +
                                             Path.GetFileNameWithoutExtension(file).Split('_')[1].Split('_')[0] +
                                             " Server.");
                    }
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private static T DeserializeObject<T>(string url)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (var xmlTextReader = new XmlTextReader(url))
            {
                return (T) xmlSerializer.Deserialize(xmlTextReader);
            }
        }

        private static void SerializeObject(object o, string outputFileName)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            //Add lib namespace with empty prefix
            namespaces.Add("", "");

            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
            XmlWriterSettings xmlWriterSetting =
                new XmlWriterSettings { NamespaceHandling = NamespaceHandling.OmitDuplicates };

            using (var xmlWriter = XmlWriter.Create(outputFileName, xmlWriterSetting))
            {
                xmlSerializer.Serialize(xmlWriter, o, namespaces);
            }
        }


        private void CreateHost(string name, HostSetting.HostTypeValues hostType, string ntGroupName, bool authTrusted,
            bool hostTracking, bool is32Bit, bool defaultHost)
        {
            try
            {
                using (var myHostSetting = _machineName == _strDstNode
                    ? HostSetting.CreateInstance()
                    : HostSetting.CreateInstance(_strDstNode, _strUserNameForWMI, _strPassword, _strDomain))
                {
                    myHostSetting.AutoCommit = false;

                    myHostSetting.Name = name;
                    myHostSetting.HostType = hostType;
                    myHostSetting.NtGroupName = ntGroupName;
                    myHostSetting.AuthTrusted = authTrusted;
                    myHostSetting.IsHost32BitOnly = is32Bit;
                    myHostSetting.HostTracking = hostTracking;
                    myHostSetting.IsDefault = defaultHost;

                    myHostSetting.CommitObject();
                }

                LogShortSuccessMsg("Host created successfully: " + name);
            }

            catch (Exception ex)
            {
                LogShortErrorMsg("Error Creating Host: " + name);
                LogException(ex);
            }
        }

        private void CreateHostInstance(string serverName, string name, string userName, string password)
        {
            try
            {
                using (var myServerHost = _machineName == _strDstNode
                    ? ServerHost.CreateInstance()
                    : ServerHost.CreateInstance(serverName, _strUserNameForWMI, _strPassword, _strDomain))
                {
                    myServerHost.ServerName = serverName;
                    myServerHost.HostName = name;
                    myServerHost.Map();
                }

                using (var myHostInstance = _machineName == _strDstNode
                    ? HostInstance.CreateInstance()
                    : HostInstance.CreateInstance(serverName, _strUserNameForWMI, _strPassword, _strDomain))
                {
                    myHostInstance.Name = "Microsoft BizTalk Server " + name + " " + serverName;
                    myHostInstance.Install(true, _strUserNameForHost, _strPasswordForHost);
                }

                LogShortSuccessMsg("Host Instance created successfully: " + name + ", " + serverName);
            }
            catch (Exception ex)
            {
                LogShortErrorMsg("Error creating HostInstance: " + name + ", " + serverName);
                LogException(ex);
            }
        }

        #endregion

        #region Handlers

        private void btnImportAdapterHandler_Click(object sender, EventArgs e)
        {
            string dstRcvHandlerList = "", dstSndHandlerList = "", hostName = "", adapterName = "";
            char[] charSeprator = {','};

            try
            {
                LogInfo("Handlers: Import started");
                if (!File.Exists(_xmlPath + @"\Handlers.xml"))
                    throw new FileNotFoundException("Handlers xml file does not exist.");

                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\Handlers.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("Handlers xml file is empty.");

                var rcvSndHandlers = DeserializeObject<RcvSndHandlers>(_xmlPath + @"\Handlers.xml");

                using (var objRcvHandlerClass = _machineName == _strDstNode
                    ? new ManagementClass("\\root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null)
                    : new ManagementClass("\\\\" + _strDstNode + "\\root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null))
                {
                    using (var objSndHandlerClass = _machineName == _strDstNode
                        ? new ManagementClass("\\root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null)
                        : new ManagementClass("\\\\" + _strDstNode + "\\root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null))
                    {
                        PutOptions options = new PutOptions {Type = PutType.CreateOnly};

                        //Get Rcv Handler List from Dst
                        EnumerationOptions enumOptions = new EnumerationOptions {ReturnImmediately = false};
                        using (var searchObjectRcv = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_ReceiveHandler", enumOptions))
                        {
                            dstRcvHandlerList = searchObjectRcv.Get().Cast<ManagementBaseObject>().Aggregate(dstRcvHandlerList,
                                (current, inst) => current + inst["HostName"].ToString().ToUpper() + "_" +
                                                   inst["AdapterName"].ToString().ToUpper() + ",");
                        }
                        string[] dstRcvHandlerArray =
                            dstRcvHandlerList.Split(charSeprator, StringSplitOptions.RemoveEmptyEntries);
                        //Get Snd Handler List from Dst                
                        enumOptions.ReturnImmediately = false;
                        using (var searchObjectSnd = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_SendHandler2", enumOptions))
                        {
                            dstSndHandlerList = searchObjectSnd.Get().Cast<ManagementBaseObject>().Aggregate(dstSndHandlerList,
                                (current, inst) => current + inst["HostName"].ToString().ToUpper() + "_" +
                                                   inst["AdapterName"].ToString().ToUpper() + ",");
                        }
                        string[] dstSndHandlerArray =
                            dstSndHandlerList.Split(charSeprator, StringSplitOptions.RemoveEmptyEntries);
                        //

                        //create a ManagementClass object and spawn a ManagementObject instance           
                        foreach (RcvSndHandlersRcvSndHandler handler in rcvSndHandlers.RcvSndHandler)
                        {
                            try
                            {
                                var direction = handler.Direction;
                                hostName = handler.HostName;
                                adapterName = handler.AdapterName;

                                ManagementObject objHandler;
                                if (direction == "0")
                                {
                                    if (Array.IndexOf(dstRcvHandlerArray, hostName.ToUpper() + "_" + adapterName.ToUpper()) < 0
                                    ) //!dstRcvHandlerList.Contains(HostName + "_" + AdapterName))
                                    {
                                        objHandler = objRcvHandlerClass.CreateInstance();
                                        //set the properties for the Managementobject
                                        objHandler["AdapterName"] = adapterName;
                                        objHandler["HostName"] = hostName;
                                        //create the Managementobject
                                        objHandler.Put(options);
                                        LogShortSuccessMsg("Success: Receive handler created for: " + adapterName + " and " +
                                                           hostName);
                                    }
                                    else
                                        LogInfo("Receive handler already exist for: " + adapterName + " and " + hostName);
                                }
                                else
                                {
                                    if (Array.IndexOf(dstSndHandlerArray, hostName.ToUpper() + "_" + adapterName.ToUpper()) < 0
                                    ) //if (!dstSndHandlerList.Contains(HostName.ToUpper() + "_" + AdapterName.ToUpper()))
                                    {
                                        objHandler = objSndHandlerClass.CreateInstance();
                                        //set the properties for the Managementobject
                                        objHandler["AdapterName"] = adapterName;
                                        objHandler["HostName"] = hostName;
                                        //create the Managementobject
                                        objHandler.Put(options);
                                        LogShortSuccessMsg("Success: Send handler created for: " + adapterName + " and " +
                                                           hostName);
                                    }
                                    else
                                        LogInfo("Send handler already exist for: " + adapterName + " and " + hostName);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogShortErrorMsg("Error creating Handler: Adapter Name: " + adapterName + ", HostName: " +
                                                 hostName);
                                LogException(ex);
                            }
                        }
                    }
                }
                _isHandlerExecuted = _strPerformOperationYes;
            }
            catch (Exception ex)
            {
                _isHandlerExecuted = _strPerformOperationNo;
                LogException(ex);
            }

            //Cursor.Current = Cursors.WaitCursor;
        }

        private void btnExportAdapterHandlers_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("Handlers: Export started.");
                if (_machineName == _strSrcNode) //local
                {
                    LogInfo("Connecting to BizTalkMgmtdb...." + txtConnectionString.Text);
                    RcvSndHandlers rcvSndHandlers = new RcvSndHandlers();

                    // instantiate new instance of Explorer OM
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + txtConnectionString.Text.Trim() +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        LogInfo("Conneceted");

                        rcvSndHandlers.RcvSndHandler = btsExp.ReceiveHandlers
                            .Cast<ReceiveHandler>()
                            .Where(rcvHandler =>
                                rcvHandler.Host.Name != "BizTalkServerApplication" &&
                                rcvHandler.Host.Name != "BizTalkServerIsolatedHost")
                            .Select(rcvHandler =>
                                new RcvSndHandlersRcvSndHandler
                                {
                                    AdapterName = rcvHandler.TransportType.Name,
                                    Direction = "0",
                                    HostName = rcvHandler.Host.Name
                                })
                            .Concat(btsExp.SendHandlers
                                .Cast<SendHandler>()
                                .Where(sndHandler =>
                                    sndHandler.Host.Name != "BizTalkServerApplication" &&
                                    sndHandler.Host.Name != "BizTalkServerIsolatedHost").Select(sndHandler =>
                                    new RcvSndHandlersRcvSndHandler
                                    {
                                        AdapterName = sndHandler.TransportType.Name,
                                        Direction = "1",
                                        HostName = sndHandler.Host.Name
                                    }))
                            .ToArray();
                    }

                    SerializeObject(rcvSndHandlers, _xmlPath + @"\Handlers.xml");
                    LogShortSuccessMsg("Success: Exported Handlers.");
                }
                else //remote
                {
                    LogInfo("Getting handlers from remote machine.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    string commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                              _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                              " -accepteula" +
                                              "  \"" +
                                              _remoteRootFolder + "\\" + _remoteExeName + "\"  \"\\\\" + _machineName +
                                              "\\" + appPathUnc + "\" \"ExportHandler\" \"" +
                                              txtConnectionString.Text.Trim() + "\"\"";
                    int returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Exported Handlers.");
                    else
                        LogShortErrorMsg("Failed: Exporting Handlers.");
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion

        #region Log Methods

        private void LogShortSuccessMsg(string pShortMsg)
        {
            int startIndex = richTextBoxLogs.TextLength;
            richTextBoxLogs.AppendText(pShortMsg);
            int endIndex = richTextBoxLogs.TextLength;

            richTextBoxLogs.Select(startIndex, endIndex - startIndex);
            richTextBoxLogs.SelectionColor = Color.DarkGreen;
            richTextBoxLogs.AppendText(Environment.NewLine);

            try
            {
                using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + pShortMsg);
                }
            }
            catch (Exception logEx)
            {
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.AppendText("Exception Message while writing in log file:  " + logEx.Message);
            }

        } //UI

        private void LogShortErrorMsg(string pShortErrorMsg) //UI
        {
            int startIndex = richTextBoxLogs.TextLength;
            richTextBoxLogs.AppendText(pShortErrorMsg);
            int endIndex = richTextBoxLogs.TextLength;

            richTextBoxLogs.Select(startIndex, endIndex - startIndex);
            richTextBoxLogs.SelectionColor = Color.DarkRed;

            richTextBoxLogs.AppendText(Environment.NewLine);

            try
            {
                using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + pShortErrorMsg);
                }
            }
            catch (Exception logEx)
            {
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.AppendText("Exception Message while writing in log file:  " + logEx.Message);
            }
        }

        private void LogException(Exception ex) //UI FILLE
        {
            int startIndex = richTextBoxLogs.TextLength;
            richTextBoxLogs.AppendText("Exception Message:  " + ex.Message);

            int endIndex = richTextBoxLogs.TextLength;

            richTextBoxLogs.Select(startIndex, endIndex - startIndex);
            richTextBoxLogs.SelectionColor = Color.DarkRed;
            richTextBoxLogs.AppendText(Environment.NewLine);

            richTextBoxLogs.Refresh();
            try
            {
                using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + "Exception Message:  " +
                                     ex.Message);
                    writer.WriteLine("Inner Exception:  " + ex.InnerException);
                    writer.WriteLine("StackTrace:  " + ex.StackTrace);
                }
            }
            catch (Exception logEx)
            {
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.AppendText("Exception Message while writing in log file:  " + logEx.Message);
            }
        }

        public void LogInfo(string strMsg) //UI FILLE
        {
            if (!string.IsNullOrEmpty(strMsg) && !string.IsNullOrWhiteSpace(strMsg))
            {
                richTextBoxLogs.AppendText(strMsg);
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.Refresh();

                try
                {
                    using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + strMsg);
                    }
                }
                catch (Exception logEx)
                {
                    richTextBoxLogs.AppendText("Exception while writing info log file:  " + logEx.Message);
                }
            }
        }

        public void LogInfoInLogFile(string strMsg) //FILE 
        {
            if (!string.IsNullOrEmpty(strMsg) && !string.IsNullOrWhiteSpace(strMsg))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + strMsg);
                    }
                }
                catch (Exception logEx)
                {
                    richTextBoxLogs.AppendText(Environment.NewLine);
                    richTextBoxLogs.AppendText("Exception while writing info log file:  " + logEx.Message);
                }
            }
        }

        public void LogInfoSyncronously(string strMsg) //FILLE
        {
            try
            {
                if (!string.IsNullOrEmpty(strMsg) && !string.IsNullOrWhiteSpace(strMsg))
                {
                    using (StreamWriter writer = new StreamWriter(_logPath + @"\MigrationTool_log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + strMsg);
                    }
                }

            }
            catch (Exception logEx)
            {
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.AppendText("Exception while writing info in log file:  " + logEx.Message);
            }
        }

        private void LogErrorInfo(string strMsg) //FILE
        {
            try
            {
                if (!string.IsNullOrEmpty(strMsg) && !string.IsNullOrWhiteSpace(strMsg))
                {
                    using (StreamWriter writer = new StreamWriter(_logPath + @"\RemoteOperation_log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") +
                                         strMsg); //"-------------------------------------------------------");                    
                    }
                }
            }
            catch (Exception logEx)
            {
                richTextBoxLogs.AppendText(Environment.NewLine);
                richTextBoxLogs.AppendText("Exception while writing info in log file:  " + logEx.Message);
            }
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogInfoSyncronously(e.Data);
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogErrorInfo(e.Data);
        }

        #endregion

        #region BizTalk App

        private int btnGetApplicationList_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("BizTalk App: Getting List.");
                try
                {

                    if (File.Exists(_xmlPath + @"\Apps.xml")) //delete older version
                    {
                        File.Delete(_xmlPath + @"\Apps.xml");
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    throw new InvalidOperationException("Error while deleting existing Apps.xml file.");
                }
                if (_machineName == _strSrcNode) //local
                {
                    BizTalkApplications bizTalkApps = new BizTalkApplications();

                    LogInfo("Connecting to BizTalkMgmtdb...." + txtConnectionString.Text);
                    // instantiate new instance of Explorer OM
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + txtConnectionString.Text.Trim() +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        var appCol = btsExp.Applications;
                        LogInfo("Connected.");

                        var htApps = new Dictionary<string, int>();
                        MsiApp(appCol, htApps);

                        bizTalkApps.BizTalkApplication = appCol
                            .Cast<Application>()
                            .Where(app => !_bizTalkAppToIgnore.Contains(app.Name))
                            .Select(app =>
                                new BizTalkApplicationsBizTalkApplication
                                {
                                    DependencyCode = htApps[app.Name].ToString(),
                                    ApplicationName = app.Name
                                })
                            .ToArray();
                    }

                    SerializeObject(bizTalkApps, _xmlPath + @"\Apps.xml");
                    LogInfo("Success: Created Apps.xml.");
                    return 0;
                }
                else //remote
                {
                    LogInfo("Getting App list from remote machine.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    string commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                              _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                              " -accepteula" +
                                              "  \"" +
                                              _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                              _machineName +
                                              "\\" + appPathUnc + "\" \"ExportAppList\" \"" +
                                              txtConnectionString.Text.Trim() + "\" \"" + _bizTalkAppToIgnore + "\"\"";
                    int returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate app xml and save it back to local

                    if (returnCode == 0)
                    {
                        LogInfo("Success: Created Apps.xml.");
                        return 0;
                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Creating Apps.xml.");
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                return 1;
            }
        }

        private void btnImportApps_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("BizTalk App: Import Started.");
                if (!File.Exists(_xmlPath + @"\Apps.xml"))
                    throw new FileNotFoundException("Apps xml file does not exist.");

                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\Apps.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("Apps xml file is empty.");

                try
                {
                    if (File.Exists(_xmlPath + @"\AppsToImport.xml")) //delete older version
                    {
                        File.Delete(_xmlPath + @"\AppsToImport.xml");
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    throw new InvalidOperationException("Error while deleting existing AppsToImport.xml file.");
                }

                UpdateAppXmlFile();

                //check file is empty or not                
                doc.Load(_xmlPath + "\\AppsToImport.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("AppsToImport xml file is empty.");

                //read new updated App List
                XElement xelement = XElement.Load(_xmlPath + @"\AppsToImport.xml");

                var appList = (from element in xelement.Descendants("BizTalkApplication")
                        let dCode = (int) element.Attribute("DependencyCode")
                        orderby dCode descending
                        select new
                        {
                            DcodeAppName = dCode + "," + element.Attribute("ApplicationName").Value
                        })
                    .ToList();

                char[] charSeprator = {','};

                LogInfo("Total Apps: " + appList.Count);

                string msiPathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(_msiPath);
                foreach (var app1 in appList)
                {
// instantiate new instance of Explorer OM                
                    string appName;
                    int result;
                    string commandArguments;
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + txtConnectionStringDst.Text.Trim() +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        appName = app1.DcodeAppName.Split(charSeprator)[1];

                        LogInfo(": App Name:" + appName);

                        if (_machineName == _strDstNode) //local
                        {
                            if (File.Exists(_msiPath + "\\" + appName + ".msi"))
                            {
                                //Import MSI
                                commandArguments = "ImportApp -Package:\"" + _msiPath + "\\" + appName + ".msi\"" +
                                                   " -ApplicationName:\"" +
                                                   appName + "\" -Overwrite -Server:\"" +
                                                   txtConnectionStringDst.Text.Trim() + "\" -Database:\"" +
                                                   "BizTalkMgmtDb\"";
                                //Create and start BTSTask.exe process                    
                                result = ExecuteCmd("BTSTask.exe", commandArguments);

                                if (result == 0)
                                    LogShortSuccessMsg("Success: Imported App.");
                                else
                                    LogShortErrorMsg("Failed: Importing App.");
                            }
                            else
                            {
                                //Create App
                                LogInfo("MSI file does not exist for: " + appName);
                                commandArguments = "addapp -ApplicationName:\"" + appName +
                                                   "\" -Description:\"BizTalk application for " + appName +
                                                   "\" -Server:\"" + txtConnectionStringDst.Text.Trim()
                                                   + "\" -Database:\"" + "BizTalkMgmtDb\"";
                                result = ExecuteCmd("BTSTask.exe", commandArguments);
                                if (result == 0) //success
                                {
                                    LogShortSuccessMsg("Success: Created App.");
                                    try
                                    {
                                        //start adding dependent app Ref
                                        ApplicationCollection appCol = btsExp.Applications;
                                        foreach (Application app in appCol)
                                        {
                                            if (app.Name == appName)
                                            {
                                                string[] baseBizTalkApp = _baseBizTalkAppCol.Split(charSeprator,
                                                    StringSplitOptions.RemoveEmptyEntries);
                                                foreach (Application baseApp in from baseBTSApp in baseBizTalkApp
                                                    from Application baseApp in appCol
                                                    where baseApp.Name.Equals(baseBTSApp.Trim())
                                                    select baseApp)
                                                {
                                                    app.AddReference(baseApp);
                                                    LogInfo("Success: Added reference of: " + baseApp.Name);
                                                }
                                            }
                                        }
                                        btsExp.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        LogShortErrorMsg("Failed: Adding reference of dependent Apps." + ex.Message);
                                    }
                                }
                                else
                                    LogShortErrorMsg("Failed: Creating App.");
                            }

                        }
                        else //remote
                        {
                            if (File.Exists(_msiPath + "\\" + appName + ".msi"))
                            {
                                //Import MSI, Set arguments for BTSTask.exe               
                                commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                                   "\"" +
                                                   _strUserName + "\"" + " -p " + "\"" + _strPassword
                                                   + "\"" + " -accepteula" + " BTSTask ImportApp -Package:\"" + msiPathUnc +
                                                   "\\" + appName + ".msi\"" + " -ApplicationName:\"" +
                                                   appName + "\" -Overwrite -Server:\"" +
                                                   txtConnectionStringDst.Text.Trim() + "\" -Database:\"" +
                                                   "BizTalkMgmtDb\"\"";

                                //Create and start BTSTask.exe process                    
                                result = ExecuteCmd("CMD.exe", commandArguments);

                                if (result == 0)
                                    LogShortSuccessMsg("Success: Imported App.");
                                else
                                    LogShortErrorMsg("Failed: Importing App.");
                            }
                            else
                            {
                                //Create App                            
                                LogInfo("MSI file does not exist for: " + appName);
                                commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                                   "\"" +
                                                   _strUserName + "\"" + " -p " + "\"" + _strPassword
                                                   + "\"" + " -accepteula" + " BTSTask addapp -ApplicationName:\"" +
                                                   appName + "\" -Description:\"BizTalk application for " + appName
                                                   + "\" -Server:\"" + txtConnectionStringDst.Text.Trim() +
                                                   "\" -Database:\"" + "BizTalkMgmtDb\"\"";
                                //Create and start BTSTask.exe process                    
                                result = ExecuteCmd("CMD.exe", commandArguments);
                                if (result == 0) //success
                                {
                                    LogShortSuccessMsg("Success: Created App.");
                                    try
                                    {
                                        ApplicationCollection appCol = btsExp.Applications;
                                        foreach (Application app in appCol)
                                        {
                                            if (app.Name == appName)
                                            {
                                                string[] baseBizTalkApp = _baseBizTalkAppCol.Split(charSeprator,
                                                    StringSplitOptions.RemoveEmptyEntries);
                                                foreach (Application baseApp in from baseBTSApp in baseBizTalkApp
                                                    from Application baseApp in appCol
                                                    where baseApp.Name == baseBTSApp.Trim()
                                                    select baseApp)
                                                {
                                                    app.AddReference(baseApp);
                                                    LogInfo("Success: Added reffernce of: " + baseApp);
                                                }
                                            }
                                        }
                                        btsExp.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        LogShortErrorMsg("Failed: Adding reference of dependent Apps." + ex.Message);
                                    }
                                }
                                else
                                    LogShortErrorMsg("Failed: Creating App.");
                            }
                        }
                    }
                    if (result != 0) //error
                    {
                        if (_baseBizTalkAppCol.Contains(appName))
                        {
                            _isBizTalkAppExecuted = _strPerformOperationNo;
                            LogShortErrorMsg("Application: " + appName +
                                             " import failed hence remainng apps won't be imported.");
                        }
                    }
                    else //import Binding
                    {
                        if (_machineName == _strDstNode) //local
                        {
                            commandArguments = "ImportBindings  -Source:\"" + msiPathUnc + "\\" + appName +
                                               "_Binding.xml\"" + " -ApplicationName:\"" +
                                               appName + "\" -Server:\"" + txtConnectionStringDst.Text.Trim() +
                                               "\" -Database:\"" + "BizTalkMgmtDb\"";
                            result = ExecuteCmd("BTSTask.exe", commandArguments);

                        }
                        else //remote
                        {
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                               "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword
                                               + "\"" + " -accepteula" + " BTSTask ImportBindings  -Source:\"" +
                                               msiPathUnc + "\\" + appName + "_Binding.xml\"" + " -ApplicationName:\"" +
                                               appName + "\" -Server:\"" + txtConnectionStringDst.Text.Trim() +
                                               "\" -Database:\"" + "BizTalkMgmtDb\"\"";

                            result = ExecuteCmd("CMD.exe", commandArguments);
                        }

                        if (result == 0)
                            LogShortSuccessMsg("Success: Imported Binding.");
                        else
                            LogShortErrorMsg("Failed: Importing Binding.");
                    }
                    //if there was  error while importing msi and msi is one show stopper msi then break loop, do not import other MSI. for example MSI EBIS
                    if (_isBizTalkAppExecuted == _strPerformOperationNo)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }


        }

        private void btnExportApps_Click(object sender, EventArgs e)
        {
            char[] charSeprator = {','};
            string cmdName = "CMD.EXE";

            try
            {
                LogInfo("BizTalk App: Export started.");
                if (btnGetApplicationList_Click(sender, e) == 0)
                {
                    XElement xelement = XElement.Load(_xmlPath + @"\Apps.xml");

                    var appList = (from element in xelement.Descendants("BizTalkApplication")
                            let dCode = (int) element.Attribute("DependencyCode")
                            orderby dCode descending
                            select new
                            {
                                DcodeAppName = dCode + "," + element.Attribute("ApplicationName").Value
                            })
                        .ToList();

                    LogInfo("Total Apps: " + appList.Count);
                    string msiPathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(_msiPath);

                    //clean MSI directory
                    try
                    {
                        Directory.Delete(_msiPath, true);
                        Directory.CreateDirectory(_msiPath);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Error while cleaning MSI folder, " + ex.Message + ", " + ex.StackTrace);
                    }
                    foreach (var appName in appList.Select(appL => appL.DcodeAppName.Split(charSeprator)[1]))
                    {
                        LogInfo("App Name:" + appName);

                        //get Spec File
                        int result;
                        string commandArguments;
                        if (_machineName == _strSrcNode)
                        {
                            commandArguments = "ListApp -ApplicationName:\"" + appName + "\" -ResourceSpec:\"" +
                                               msiPathUnc + "\\" + appName + _specFileExt + "\"" + " -Server:\"" +
                                               txtConnectionString.Text.Trim() + "\" -Database:\"" + "BizTalkMgmtDb\"";
                            result = ExecuteCmd("BTSTASK.exe", commandArguments);
                        }
                        else
                        {
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + " BTSTASK ListApp -ApplicationName:\"" + appName +
                                               "\" -ResourceSpec:\"" +
                                               msiPathUnc + "\\" + appName + _specFileExt + "\"" + " -Server:\"" +
                                               txtConnectionString.Text.Trim() + "\" -Database:\"" +
                                               "BizTalkMgmtDb\"\"";
                            result = ExecuteCmd(cmdName, commandArguments);
                        }

                        if (result == 0)
                            LogShortSuccessMsg("Success: Exported Spec file.");
                        else
                            LogShortErrorMsg("Failed: Exporting Spec file.");


                        // edit resource file 
                        UpdateResourceSpecFile(appName);

                        //export MSI using spec file.    
                        if (_machineName == _strSrcNode)
                        {
                            commandArguments = "ExportApp -ApplicationName:\"" + appName + "\" -Package:\"" +
                                               msiPathUnc + "\\" + appName + ".msi\"" + " -ResourceSpec:\"" +
                                               msiPathUnc + "\\" + appName + _specFileExt + "\"" + " -Server:\"" +
                                               txtConnectionString.Text.Trim() + "\" -Database:\"" + "BizTalkMgmtDb\"";
                            result = ExecuteCmd("BTSTASK.exe", commandArguments);
                        }
                        else
                        {
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + " BTSTASK ExportApp -ApplicationName:\"" + appName +
                                               "\" -Package:\"" +
                                               msiPathUnc + "\\" + appName + ".msi\"" + " -ResourceSpec:\"" +
                                               msiPathUnc + "\\" + appName + _specFileExt + "\"" + " -Server:\"" +
                                               txtConnectionString.Text.Trim() + "\" -Database:\"" +
                                               "BizTalkMgmtDb\"\"";
                            result = ExecuteCmd(cmdName, commandArguments);
                        }

                        if (result == 0)
                            LogShortSuccessMsg("Success: Exported MSI file.");
                        else
                            LogShortErrorMsg("Failed: Exporting MSI file.");

                        //export Binding   
                        if (_machineName == _strSrcNode)
                        {
                            commandArguments = "ExportBindings -Destination:\"" + msiPathUnc + "\\" + appName +
                                               "_Binding.xml\"" + " -ApplicationName:\"" + appName + "\"" +
                                               " -Server:\"" + txtConnectionString.Text.Trim() + "\" -Database:\"" +
                                               "BizTalkMgmtDb\"";
                            result = ExecuteCmd("BTSTASK.exe", commandArguments);
                        }
                        else
                        {
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + " BTSTASK ExportBindings -Destination:\"" + msiPathUnc +
                                               "\\" + appName + "_Binding.xml\"" + " -ApplicationName:\"" + appName +
                                               "\"" +
                                               " -Server:\"" + txtConnectionString.Text.Trim() + "\" -Database:\"" +
                                               "BizTalkMgmtDb\"\"";
                            result = ExecuteCmd(cmdName, commandArguments);
                        }

                        if (result == 0)
                            LogShortSuccessMsg("Success: Exported Binding file.");
                        else
                            LogShortErrorMsg("Failed: Exporting Binding file.");
                    }
                }
                else
                    LogShortErrorMsg("Failed: Exporting Apps.");

            }
            catch (Exception ex)
            {
                LogException(ex);
            }


        }

        #endregion

        #region GlobalPartyBinding

        private void btnExportGlobalPartyBinding_Click(object sender, EventArgs e)
        {
            string commandArguments;
            string cmdName;

            string xmlPathUnc = ConvertPathToUncPath(_xmlPath);

            if (_machineName == _strSrcNode) //local
            {
                cmdName = "BTSTASK.exe";
                LogInfo("Global PartyBinding: Export started.");
                commandArguments = "ExportBindings -Destination:\"" + _xmlPath + "\\"
                                   + "GlobalPartyBinding.xml\"  -GlobalParties " + " -Server:\"" +
                                   txtConnectionString.Text.Trim() + "\" -Database:\"" + "BizTalkMgmtDb\"";
            }
            else //remote
            {
                cmdName = "CMD.EXE";
                LogInfo("Global PartyBinding: Export started from remote server.");
                commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                   _strUserName +
                                   "\"" + " -p " + "\"" + _strPassword
                                   + "\"" + " -accepteula" + " BTSTask ExportBindings -Destination:\"" + "\\\\" +
                                   _machineName + "\\" + xmlPathUnc + "\\" +
                                   "GlobalPartyBinding.xml\"  -GlobalParties " + " -Server:\"" +
                                   txtConnectionString.Text.Trim() + "\" -Database:\"" + "BizTalkMgmtDb\"\"";
            }

            var returnCode = ExecuteCmd(cmdName, commandArguments);
            if (returnCode == 0) //BTSTASK success code
            {
                LogShortSuccessMsg("Success: Global PartyBinding Exported.");
            }
            else
            {
                LogShortErrorMsg("Failed:  Exporting Global PartyBinding.");
            }
            try
            {
                try
                {
                    if (Directory.Exists(_brePath))
                    {
                        Directory.Delete(_brePath, true);
                        Directory.CreateDirectory(_brePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(_brePath);
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error while cleaning Policies or Vocabulary folder, hence aborting  export " +
                                        ex.Message + ", " + ex.StackTrace);
                }
                if (_machineName == _strSrcNode)
                {
                    LogInfo("BRE: Export started.");
                    ExportBrePolicyVocabulary();
                    LogShortSuccessMsg("Success: Exported BRE.");
                }
                else
                {
                    LogInfo("BRE: Export started.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ExportBREList\" \"" + txtConnectionString.Text.Trim() +
                                       "\" \"";
                    returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local
                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Exported BRE Polices and Vocabualaries.xml.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting BrePolicies and Vocabularies");

                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }


        }

        private void btnImportGlobalPartyBinding_Click(object sender, EventArgs e)
        {
            string commandArguments;
            int returnCode;
            string xmlPathUnc = ConvertPathToUncPath(_xmlPath);

            try
            {
                LogInfo("Global Party Binding: Import started.");
                if (!File.Exists(_xmlPath + @"\GlobalPartyBinding.xml"))
                    throw new FileNotFoundException("GlobalPartyBinding xml file does not exist.");

                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\GlobalPartyBinding.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("GlobalPartyBinding xml file is empty.");

                string cmdName;
                if (_machineName == _strDstNode) //local
                {
                    cmdName = "BTSTASK.exe";
                    commandArguments = "ImportBindings -Source:\"" + _xmlPath + "\\" + "GlobalPartyBinding.xml\"" +
                                       " -Server:\"" + txtConnectionStringDst.Text.Trim() +
                                       "\" -Database:\"" + "BizTalkMgmtDb\"";
                }
                else //remote
                {
                    cmdName = "CMD.EXE";
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       " BTSTask ImportBindings -Source:\"" + "\\\\" + _machineName + "\\" +
                                       xmlPathUnc +
                                       "\\" + "GlobalPartyBinding.xml\"" +
                                       " -Server:\"" + txtConnectionStringDst.Text.Trim() + "\" -Database:\"" +
                                       "BizTalkMgmtDb\"\"";
                }

                returnCode = ExecuteCmd(cmdName, commandArguments);
                if (returnCode == 0)
                {
                    LogShortSuccessMsg("Success: Imported Global Party Binding.");
                }
                else
                {
                    LogShortErrorMsg("Failed: Importing Global Party Binding.");
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            try
            {
                if (_machineName == _strDstNode) //local
                {
                    LogInfo("BRE: Import Started.");
                    ImportBrePolicyVocabulary();
                    LogShortSuccessMsg("Success: Imported BRE.");
                }
                else
                {
                    LogInfo("BRE: Import Started.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ImportBREList\" \"" + txtConnectionStringDst.Text.Trim() +
                                       "\" \"";
                    returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local
                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Imported BRE Polices and Vocabualaries.xml.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Importing BRE Polices and Vocabularies");

                    }
                }
            }
            catch (Exception ex)
            {

                LogException(ex);
            }

        }

        #endregion

        #region WebSite AppPools

        private void btnExportAppPools_Click(object sender, EventArgs e)
        {
            LogInfo("App Pool: Export started.");

            if (File.Exists(_xmlPath + @"\AppPools.xml")) //Create a Fresh Copy
                File.Delete(_xmlPath + @"\AppPools.xml");

            string commandArguments;

            if (_machineName == _strSrcNode) //local            
                commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list apppool /config /xml /> \"" +
                                   _xmlPath + "\\AppPools.xml" + "\"";
            else //remote            
                commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strSrcNode + " -u " + "\"" +
                                   _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                   + "  C:\\windows\\system32\\inetsrv\\appcmd.exe list apppool /config /xml /> \"" +
                                   _xmlPath + "\\AppPools.xml" + "\"\"";

            int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
            if (returnCode == 0)
                LogShortSuccessMsg("Success: Exported AppPools.");
            else
                LogShortErrorMsg("Failed: Exporting AppPools.");
            try
            {
                //Encrypting Passwords
                if (File.Exists(_xmlPath + @"\AppPools.xml"))
                {
                    FileInfo fileInfo = new FileInfo(_xmlPath + @"\AppPools.xml") {IsReadOnly = false};
                    fileInfo.Refresh();
                    XmlDocument xmldoc = new XmlDocument();

                    xmldoc.Load(_xmlPath + @"\AppPools.xml");
                    XmlNodeList nodeList = xmldoc.DocumentElement.SelectNodes("/appcmd/APPPOOL/add/processModel");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes?["password"] != null)
                        {
                            string password = node.Attributes["password"].Value;
                            node.Attributes["password"].Value = Encrypt(password);
                        }
                    }
                    xmldoc.Save(_xmlPath + @"\AppPools.xml");
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnImportAppPools_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("App Pool: Import started.");
                if (!File.Exists(_xmlPath + @"\AppPools.xml"))
                    throw new FileNotFoundException("AppPools xml file does not exist.");

                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\AppPools.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("AppPools xml file is empty.");

                //Genrate AppPool List from Dst
                string commandArguments;
                if (_machineName == _strDstNode) //local                
                    commandArguments =
                        "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list apppool /text:APPPOOL.NAME  > \"" +
                        _xmlPath + "\\AppPoolList.txt" + "\"";
                else //remote                
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                       + "  C:\\windows\\system32\\inetsrv\\appcmd.exe list apppool /text:APPPOOL.NAME  > \"" +
                                       _xmlPath + "\\AppPoolList.txt" + "\"\"";
                //execute
                var returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogInfo("Success: Created AppPoolList.txt.");
                else
                {
                    try
                    {
                        string[] dstAppPoolTemp = File.ReadAllLines(_xmlPath + "\\AppPoolList.txt");
                        if (dstAppPoolTemp.Length == 0)
                        {
                            //empty file, may be no web site in destination. in this case non zere error code of appcmd can be ignored.
                            //if no file is genrated in that case first line in try will genrate exception which will abort import process.
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            "Failed: Creating AppPoolList.txt from Dst." + Environment.NewLine + ex.Message,
                            ex.InnerException);
                    }
                }
                //Delete AppPools from Xmls which alread there in Dst
                UpdateAppPoolXml();

                doc = new XmlDocument();
                doc.Load(_xmlPath + "\\AppPoolToImport.xml");


                if (doc.DocumentElement.ChildNodes.Count > 0) //if file not empty.
                {
                    // Decrypting Password
                    XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/appcmd/APPPOOL/add/processModel");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes?["password"] != null)
                        {
                            string password = node.Attributes["password"].Value;
                            node.Attributes["password"].Value = Decrypt(password);
                        }
                    }
                    doc.Save(_xmlPath + "\\AppPoolToImport.xml");


                    //actual AppPool Import
                    if (_machineName == _strDstNode) //local                
                        commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add apppool /in /xml < \"" +
                                           _xmlPath + "\\AppPoolToImport.xml" + "\"";
                    else //remote 
                    {
                        //commandArguments = "/C " + "\"\"" + psExecPath + "\" -s \\\\" + strDstNode + " -u " + "\"" + strUserName + "\"" + " -p " + "\"" + strPassword + "\"" + " -accepteula"
                        //    + "  C:\\windows\\system32\\inetsrv\\appcmd.exe add apppool /in /xml < \"" + xmlPath + "\\AppPoolToImport.xml" + "\"\"";
                        string appPathUnc = ConvertPathToUncPath(_appPath);
                        commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " + "\"" +
                                           _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                           "  \"" +
                                           _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                           "\\" + appPathUnc + "\"" + " \"ImportAppPool\"\"";
                    }
                    //execute  
                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Imported AppPools.");
                    else
                        LogShortErrorMsg("Failed: Importing AppPools.");

                    //Encrypting Back Password
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes?["password"] != null)
                        {
                            string password = node.Attributes["password"].Value;
                            node.Attributes["password"].Value = Encrypt(password);
                        }
                    }
                    doc.Save(_xmlPath + "\\AppPoolToImport.xml");

                }
                else
                    LogInfo("AppPoolToImport.xml is empty.");
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnExportWebSites_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("WebSite: Export started.");

                if (_strToolMode == "Backup")
                {
                    try
                    {
                        if (Directory.Exists(_vDir))
                        {
                            Directory.Delete(_vDir, true);
                            Directory.CreateDirectory(_vDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            "Error while cleaning Virtual Directory folder, hence aborting folder export " +
                            ex.Message + ", " + ex.StackTrace);
                    }
                }

                //export website list in txt file
                string commandArguments;
                int returnCode;
                if (_machineName == _strSrcNode) //local
                {
                    //get list of Website in txt file, one line one website                
                    commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list site /text:name /> \"" +
                                       _xmlPath + "\\SrcWebSiteList.txt" + "\"";
                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                    if (returnCode == 0)
                        LogInfoInLogFile("Created SrcWebSiteList.txt.");
                    else
                    {
                        try
                        {
                            string[] srcLinesTemp = File.ReadAllLines(_xmlPath + "\\SrcWebSiteList.txt");
                            if (srcLinesTemp.Length == 0)
                            {
                                //empty file, may be no web site in destination. in this case non zere error code of appcmd can be ignored.
                                //if no file is genrated in that case first line in try will genrate exception which will abort import process.
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Failed: Creating SrcWebSiteList.txt" + Environment.NewLine + ex.Message,
                                ex.InnerException);
                        }

                    }

                    commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list site /config /xml /> \"" +
                                       _xmlPath + "\\WebSites.xml" + "\"";
                }
                else //remote
                {
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                       + " C:\\windows\\system32\\inetsrv\\appcmd.exe list site /text:name /> \"" +
                                       _xmlPath + "\\SrcWebSiteList.txt" + "\"\"";
                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogInfoInLogFile("Created SrcWebSiteList.txt.");
                    else
                    {
                        try
                        {
                            string[] srcLinesTemp = File.ReadAllLines(_xmlPath + "\\SrcWebSiteList.txt");
                            if (srcLinesTemp.Length == 0)
                            {
                                //empty file, may be no web site in destination. in this case non zere error code of appcmd can be ignored.
                                //if no file is genrated in that case first line in try will genrate exception which will abort import process.
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Failed: Creating SrcWebSiteList.txt" + Environment.NewLine + ex.Message,
                                ex.InnerException);
                        }
                    }
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                       + "  C:\\windows\\system32\\inetsrv\\appcmd.exe list site /config /xml /> \"" +
                                       _xmlPath + "\\WebSites.xml" + "\"\"";
                }

                //execute actual WebSite export
                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogShortSuccessMsg("Success: Exported WebSites.");
                else
                {
                    throw new InvalidOperationException("Failed: Exporting WebSites.");
                }

                //split website xml
                UpdateSrcWebSiteXml();


                string[] srcWebSites =
                    Directory.GetFiles(_xmlPath, "WebSite_*_ToExport.xml", SearchOption.AllDirectories);
                foreach (string webSiteFilePath in srcWebSites)
                {
                    string webSiteFileName = Path.GetFileNameWithoutExtension(webSiteFilePath);
                    string webSiteName = webSiteFileName.Substring(8, webSiteFileName.Length - 17);
                    //Move vDir to Local
                    MoveWebAppFolders(webSiteFilePath, webSiteName);

                    //split and export web app per website   
                    LogInfo("Web App: Export started.");
                    if (_machineName == _strSrcNode) //local                         
                    {
                        commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list apps /site.name:\"" +
                                           webSiteName + "\" /config /xml /> \""
                                           + _xmlPath + "\\WebApps_" + webSiteName + "_ToExport.xml" + "\"";
                    }
                    else //remote                           
                    {
                        commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strSrcNode + " -u " + "\"" +
                                           _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                           + "  C:\\windows\\system32\\inetsrv\\appcmd.exe list apps /site.name:\"" +
                                           webSiteName + "\" /config /xml /> \""
                                           + _xmlPath + "\\WebApps_" + webSiteName + "_ToExport.xml" + "\"\"";
                    }

                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Exported WebApps.");
                    else
                        LogShortErrorMsg("Failed: Exporting WebApps.");
                }

                if (_machineName == _strSrcNode) //local  
                {
                    LogInfo("IISClientCertMappings: Export started.");
                    ExportClientCertOnetOneMappings();
                    LogShortSuccessMsg("Success: Exported IISClientCertMappings.");
                }
                else
                {
                    LogInfo("IISClientCertMappings: Export started.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\"" + " \"ExportIISClientCert\"\"";

                    returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local
                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Exported IISClientCertMappings.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting IISClientCertMappings.");

                    }
                }
                LogInfoInLogFile("SrcBTSInstallPath:Export Started.");
                //Export BTSInstallPath into txt file
                if (_machineName == _strSrcNode) //local  
                {
                    commandArguments = "/C SET BTSINSTALLPATH > \"" + _xmlPath + "\\SrcBTSInstallPath.txt" + "\"";
                }
                else //Remote
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"BTSInstallPath\" \"" + "Export" + "\" \"";
                }
                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogInfoInLogFile("Success: Exported SrcBTSInstallPath.");
                else
                {
                    LogShortErrorMsg("Failed: Exporting SrcBTSInstallPath.");
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }


        private void btnImportWebSites_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("Website: Import started.");
                if (!File.Exists(_xmlPath + @"\WebSites.xml"))
                    throw new FileNotFoundException("WebSites xml file does not exist.");

                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\WebSites.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("WebSites xml file is empty.");

                if (_strServerType == "BizTalk" &&
                    (cmbBoxServerDst.Items.Count == 0 || cmbBoxServerDst.SelectedItem == null))
                    throw new InvalidOperationException("Please select Dst node.");

                if (_strServerType == "App" && (txtConnectionStringDst.Text == "SERVER NAME" ||
                                                txtConnectionStringDst.Text.Trim() == ""))
                    throw new InvalidOperationException("Please enter Dst App server.");

                //Export Destination BTSInstallPath into txt file
                string commandArguments;
                if (_machineName == _strDstNode) //local  
                {
                    commandArguments = "/C SET BTSINSTALLPATH > \"" + _xmlPath + "\\DstBTSInstallPath.txt" + "\"";
                }
                else
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"BTSInstallPath\" \"" + "Import" + "\" \"";
                }
                var returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogInfoInLogFile("Success: Exported DstBTSInstallPath.");
                else
                {
                    throw new InvalidOperationException("Failed: Exporting DstBTSInstallPath.");
                }
                //Get WebSite Name Only from Destiantion in Txt file
                if (_machineName == _strDstNode) //local                            
                    commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list site /text:name /> \"" +
                                       _xmlPath + "\\DstWebSiteList.txt" + "\"";
                else //remote            
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                       + " C:\\windows\\system32\\inetsrv\\appcmd.exe list site /text:name /> \"" +
                                       _xmlPath + "\\DstWebSiteList.txt" + "\"\"";

                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogInfo("Created DstWebSiteList.txt.");
                else
                {
                    try
                    {
                        string[] dstLinesTemp = File.ReadAllLines(_xmlPath + "\\DstWebSiteList.txt");
                        if (dstLinesTemp.Length == 0)
                        {
                            //empty file, may be no web site in destination. in this case non zere error code of appcmd can be ignored.
                            //if no file is genrated in that case first line in try will genrate exception which will abort import process.
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed: Creating DstWebSiteList.txt" + Environment.NewLine + ex.Message,
                            ex.InnerException);
                    }
                }

                //split into multiple website.
                UpdateWebSiteXml();
                LogInfo("Generated Delta of Website to be imported.");

                string[] dstLines = File.ReadAllLines(_xmlPath + "\\DstWebSiteList.txt");
                string[] srcLines = File.ReadAllLines(_xmlPath + "\\SrcWebSiteList.txt");

                foreach (string srcLine in srcLines)
                {
                    int matches = 0;
                    if(dstLines.Any(dstLine => dstLine == srcLine))
                    {
                        matches = 1;
                    }

                    if (matches != 1
                    ) //if no match which mean website is not existing in dest then create website in dest
                    {
                        //actual web site import.
                        if (_machineName == _strDstNode) //local
                        {
                            MoveWebAppFolders(_xmlPath + "\\WebSite_" + srcLine + "_ToImport.xml",
                                srcLine); //while mvoing DstWebSite Folders subfolders which correspond to App folders, might also move, need to stop that
                            commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add site /in /xml /< \"" +
                                               _xmlPath + "\\WebSite_" + srcLine + "_ToImport.xml" + "\"";
                        }
                        else //remote
                        {
                            MoveWebAppFolders(_xmlPath + "\\WebSite_" + srcLine + "_ToImport.xml", srcLine);
                            //commandArguments = "/C " + "\"\"" + psExecPath + "\" -s \\\\" + strDstNode + " -u " + "\"" + strUserName + "\"" + " -p " + "\"" + strPassword + "\"" + " -accepteula"
                            //    + "  C:\\windows\\system32\\inetsrv\\appcmd.exe add site /in /xml /< \"" + xmlPath + "\\WebSite_" + srcLines[srcLineCount] + "_ToImport.xml" + "\"\"";
                            string appPathUnc = ConvertPathToUncPath(_appPath);
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                               "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + "  \"" +
                                               _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                               _machineName + "\\" + appPathUnc + "\" \"ImportWebSite\" \"" + srcLine +
                                               "\" \"";
                        }

                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode == 0)
                            LogShortSuccessMsg("Success: Imported WebSite: " + srcLine);
                        else
                            LogShortErrorMsg("Failed: Importing Website: " + srcLine);

                    }
                    else
                        LogInfo("WebSite already exist: " + srcLine);
                }

                //Generate DstWebAppList
                LogInfo("WebApp: Import started.");
                if (_machineName == _strDstNode) //local                
                    commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe list app /text:app.name /> \"" +
                                       _xmlPath + "\\DstWebAppList.txt" + "\"";
                else //remote                
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                       + " C:\\windows\\system32\\inetsrv\\appcmd.exe list app /text:app.name /> \"" +
                                       _xmlPath + "\\DstWebAppList.txt" + "\"\"";

                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                if (returnCode == 0)
                    LogInfo("Created DstWebAppList.txt");
                else
                {
                    try
                    {
                        string[] dstLinesTemp = File.ReadAllLines(_xmlPath + "\\DstWebAppList.txt");
                        if (dstLinesTemp.Length == 0)
                        {
                            //empty file, may be no web site in destination. in this case non zere error code of appcmd can be ignored.
                            //if no file is genrated in that case first line in try will genrate exception which will abort import process.
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed: Creating DstWebAppList.txt" + Environment.NewLine + ex.Message,
                            ex.InnerException);
                    }
                }

                string[] srcWebApps =
                    Directory.GetFiles(_xmlPath, "WebApps_*_ToExport.xml", SearchOption.AllDirectories);
                foreach (string webAppFilePath in srcWebApps)
                {
                    string webAppFileName = Path.GetFileNameWithoutExtension(webAppFilePath);
                    string webSiteName = webAppFileName.Substring(8, webAppFileName.Length - 17);
                    //Update WebApps xml, remove Apps which are already there in Dst
                    UpdateWebAppXml(webAppFileName + ".xml", webSiteName);
                    //WebApps_DefaultWebsite2_ToImport.xml
                    doc = new XmlDocument();
                    doc.Load(_xmlPath + "\\WebApps_" + webSiteName + "_ToImport.xml");

                    if (doc.DocumentElement.ChildNodes.Count > 0) //if file not empty
                    {
                        LogInfo("Generated Delta of WebApps to be imported.");
                        //actual web app import
                        if (_machineName == _strDstNode) //local
                        {
                            MoveWebAppFolders(_xmlPath + "\\WebApps_" + webSiteName + "_ToImport.xml", webSiteName);
                            commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add app /in /xml /< \"" +
                                               _xmlPath + "\\WebApps_" + webSiteName + "_ToImport.xml" + "\"";
                        }
                        else //remote
                        {
                            MoveWebAppFolders(_xmlPath + "\\WebApps_" + webSiteName + "_ToImport.xml", webSiteName);
                            //commandArguments = "/C " + "\"\"" + psExecPath + "\" -s \\\\" + strDstNode + " -u " + "\"" + strUserName + "\"" + " -p " + "\"" + strPassword + "\"" + " -accepteula"
                            //    + "  C:\\windows\\system32\\inetsrv\\appcmd.exe add site /in /xml /< \"" + "\\WebApps_" + webSiteName + "_ToImport.xml" + "\"\"";
                            string appPathUnc = ConvertPathToUncPath(_appPath);
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                               "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + "  \"" +
                                               _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                               _machineName + "\\" + appPathUnc + "\" \"ImportWebApp\" \"" +
                                               webSiteName + "\" \"";
                        }

                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode == 0)
                            LogShortSuccessMsg("Success: Imported WebApps.");
                        else
                            LogShortErrorMsg("Failed: Importing WebApps.");
                    }
                    else
                        LogInfo("WebAppsToImport.xml is empty.");
                }

                if (_machineName == _strDstNode) //local
                {
                    LogInfo("IISClientCertMappings: Import started.");
                    ImportClientCertOnetOneMappings();
                    LogShortSuccessMsg("Success: Imported IISClientCertMappings.");
                }
                else
                {
                    LogInfo("IISClientCertMappings: Import Started.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\"" + " \"ImportIISClientCert\"\"";
                    returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local
                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Imported IISClientCertMappings on " + _strDstNode + " Server.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Importing IISClientCertMappings on " + _strDstNode + " Server.");

                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion

        #region Certificate

        private void btnExportCert_Click(object sender, EventArgs e)
        {
            string certFileName = string.Empty;
            try
            {
                //Clean Cert Folder   
                LogInfo("CERT: Export started.");
                try
                {
                    if (Directory.Exists(_certPath))
                    {
                        Directory.Delete(_certPath, true);
                        Directory.CreateDirectory(_certPath);
                    }
                    else
                        Directory.CreateDirectory(_certPath);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error while cleaning cert folder, hence aborting CERT export " + ex.Message +
                                        ", " + ex.StackTrace);
                }

                int returnCode;
                string commandArguments;
                if (_machineName == _strSrcNode) //local
                {
                    int i = 0;
                    foreach (string iStoreLocation in Enum.GetNames(typeof(StoreLocation)))
                    {
                        foreach (string iStoreName in Enum.GetNames(typeof(StoreName)))
                        {
                            StoreLocation storeLoc; Enum.TryParse(iStoreLocation, out storeLoc);
                            StoreName storeNam; Enum.TryParse(iStoreName, out storeNam);
                            if (storeLoc == StoreLocation.LocalMachine ||
                                storeLoc == StoreLocation.CurrentUser && storeNam == StoreName.My)
                            {
                                var store = new X509Store(storeNam, storeLoc);

                                try

                                {
                                    store.Open(OpenFlags.ReadOnly);
                                    LogInfo(iStoreLocation + "_" + iStoreName + "_Count: " + store.Certificates.Count);
                                    foreach (X509Certificate2 certificate in store.Certificates)
                                    {
                                        // Exporting EnhancedKEyUsage and KeyUsage for Certificates
                                        var enhancedKeyUsage = new string[] { };

                                        foreach (X509Extension extension in certificate.Extensions.Cast<X509Extension>()
                                            .Where(extension => extension.Oid.FriendlyName == "Enhanced Key Usage"))
                                        {
                                            try
                                            {
                                                enhancedKeyUsage = ((X509EnhancedKeyUsageExtension) extension)
                                                    .EnhancedKeyUsages.Cast<Oid>()
                                                    .Where(oid =>
                                                        !string.IsNullOrEmpty(oid.FriendlyName) &&
                                                        !string.IsNullOrWhiteSpace(oid.FriendlyName))
                                                    .Select(oid => oid.FriendlyName.Trim()).ToArray();
                                            }
                                            catch (Exception ex)
                                            {
                                                LogInfo("Exception Occured when Extracting Enhanced Key Usage: " +
                                                        _certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                        certificate.Thumbprint);
                                                LogException(ex);
                                            }
                                        }

                                        if (enhancedKeyUsage.Contains("ITG Smartcard") ||
                                            enhancedKeyUsage.Contains("Smart Card Logon") ||
                                            certificate.Issuer.Contains("login.windows.net"))
                                        {
                                            //Do Nothing
                                        }

                                        else
                                        {
                                            try
                                            {
                                                var thumbPrint = certificate.Thumbprint;
                                                int dateCompare = DateTime.Compare(certificate.NotAfter, DateTime.Now);
                                                if (dateCompare >= 0)
                                                {
                                                    if (certificate.HasPrivateKey)
                                                    {
                                                        certFileName =
                                                            _certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                            thumbPrint + "_" + i + ".PFX";
                                                        //certBytes = certificate.Export(X509ContentType.Pfx, strCertPass);
                                                        //File.WriteAllBytes(certFileName, certBytes);
                                                        if (iStoreLocation == "CurrentUser")
                                                            commandArguments =
                                                                "/C C:\\windows\\system32\\certutil.exe -f -exportpfx " +
                                                                " -p " + "\"" + _strCertPass + "\"" + " -User " +
                                                                iStoreName + " " + "\"" + thumbPrint + "\"" + " " +
                                                                certFileName + " " + "ExtendedProperties";
                                                        else
                                                            commandArguments =
                                                                "/C C:\\windows\\system32\\certutil.exe -f -exportpfx " +
                                                                " -p " + "\"" + _strCertPass + "\"" + " " + iStoreName +
                                                                " " + "\"" + thumbPrint + "\"" + " " + certFileName +
                                                                " " + "ExtendedProperties";

                                                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                                        if (returnCode == 0)
                                                        {
                                                            LogInfoInLogFile("Success:Exported Cert:" + certFileName);
                                                        }
                                                        else
                                                            LogShortErrorMsg("Failed:Exporting Cert:" + certFileName);
                                                    }
                                                    else
                                                    {
                                                        certFileName =
                                                            _certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                            thumbPrint + "_" + i + ".CER";
                                                        var certBytes = certificate.Export(X509ContentType.Cert);
                                                        File.WriteAllBytes(certFileName, certBytes);

                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogInfo("Exception: Cert File Name: " + certFileName);
                                                LogException(ex);
                                            }
                                            finally
                                            {
                                                i++;
                                            }
                                        }

                                    }
                                }



                                catch (Exception ex)
                                {
                                    LogException(ex);
                                }

                                finally
                                {
                                    store.Close();
                                }
                            }
                        }
                    }
                    LogShortSuccessMsg("Success: Exported Cert.");
                }
                else //remote
                {
                    //export cert on remote machine, locally       
                    LogInfo("CERT: Exporting from Remote Machine.");
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserNameForHost + "\"" + " -p " + "\"" + _strPasswordForHost + "\"" +
                                       " -accepteula" + "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + _remoteRootFolder +
                                       "\" \"ExportCert\" \"" + _strCertPass + "\"\"";

                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Exported Cert.");

                        LogInfo("Moving cert files to local: " + _machineName + " from remote: " + _strSrcNode);
                        //bring back that cert folder to local machine                
                        string remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                        var strSrc = "\\\\" + _strSrcNode + "\\" + remoteRootFolderUnc + _certFolderName;
                        commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + _certPath + "\" " +
                                           "/MOVE /R:1";
                        ExecuteCmd("CMD.EXE", commandArguments); //copy Cert Folder to Remote destination server


                        strSrc = "\\\\" + _strSrcNode + "\\" + remoteRootFolderUnc + "\\Logs";
                        var strDst = _logPath;
                        commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/MOVE /R:1";
                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                        if (returnCode < _strRoboCopySuccessCode)
                            LogShortSuccessMsg("Success: Copied Certificates to local.");
                        else
                            LogShortErrorMsg("Failed: Copying Certificates to local ");
                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting Cert.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

        }

        private void btnImportCert_Click(object sender, EventArgs e)
        {
            int returnCode;
            string commandArguments;

            if (_machineName == _strDstNode) //local
            {
                LogInfo("CERT: Import started.");
                //BEGIN::new code for delta, get all cert name and write them in txt file,
                X509Store store;
                using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstCertList.txt", false))
                {
                    foreach (string iStoreLocation in Enum.GetNames(typeof(StoreLocation)))
                    {
                        foreach (string iStoreName in Enum.GetNames(typeof(StoreName)))
                        {
                            StoreLocation storeLoc; Enum.TryParse(iStoreLocation, out storeLoc);
                            StoreName storeNam; Enum.TryParse(iStoreName, out storeNam);

                            store = new X509Store(storeNam, storeLoc);

                            try
                            {
                                store.Open(OpenFlags.ReadOnly);
                                foreach (X509Certificate2 certificateDst in store.Certificates)
                                {
                                    try
                                    {
                                        var thumbPrint = certificateDst.Thumbprint;
                                        if (certificateDst.HasPrivateKey)
                                        {
                                            writer.WriteLine(iStoreLocation + "_" + iStoreName + "_" + thumbPrint);
                                        }
                                        else
                                        {
                                            writer.WriteLine(iStoreLocation + "_" + iStoreName + "_" + thumbPrint);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogException(ex);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(ex);
                            }

                            finally
                            {
                                store.Close();
                            }
                        }
                    }
                }
                //END::new code for delta, get all cert name and write them in txt file, 

                string[] dstCertNameList = File.ReadAllLines(_xmlPath + @"\DstCertList.txt"); //read all cert of Dst
                //Creating CertificatesList with out StorLocation
                var dstCertList = dstCertNameList.Select(t => t.Substring(t.IndexOf('_') + 1)).ToArray();
                int certsImported = 0;
                foreach (string iStoreLocation in Enum.GetNames(typeof(StoreLocation)))
                {
                    foreach (string iStoreName in Enum.GetNames(typeof(StoreName)))
                    {
                        StoreLocation storeLoc; Enum.TryParse(iStoreLocation, out storeLoc);
                        StoreName storeNam; Enum.TryParse(iStoreName, out storeNam);
                        //if (storeNam == StoreName.Root && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.Disallowed && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.AuthRoot && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.CertificateAuthority && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.AddressBook && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.TrustedPeople && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        //if (storeNam == StoreName.TrustedPublisher && storeLoc == StoreLocation.CurrentUser)
                        //    storeLoc = StoreLocation.LocalMachine;
                        store = new X509Store(storeNam, storeLoc);

                        try
                        {
                            store.Open(OpenFlags.ReadWrite);

                            var strFiles = Directory.GetFiles(_certPath, iStoreLocation + "_" + iStoreName + "*");

                            foreach (string strFile in strFiles)
                            {
                                string certName = strFile.Substring(strFile.LastIndexOf('\\') + 1);
                                try
                                {
                                    X509Certificate2 certificate;
                                    if (storeNam == StoreName.My)
                                    {
                                        certName = certName.Substring(0, certName.LastIndexOf('_'));
                                        if (Array.IndexOf(dstCertNameList, certName) < 0) //cert  already exists in Dst
                                        {
                                            if (Path.GetExtension(strFile) == ".PFX")
                                            {
                                                if (strFile.Contains("CurrentUser"))
                                                    commandArguments =
                                                        "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                        " -p " + "\"" + _strCertPass + "\"" + " -User " + storeNam +
                                                        " " + "\"" + strFile + "\"";
                                                else
                                                    commandArguments =
                                                        "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                        " -p " + "\"" + _strCertPass + "\"" + " " + storeNam + " " +
                                                        "\"" + strFile + "\"";

                                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                                if (returnCode == 0)
                                                {
                                                    LogInfoInLogFile("Success: Imported Cert:" + certName);
                                                }
                                                else
                                                    LogShortErrorMsg("Failed Importing Cert File Name:" + certName);
                                                certsImported++;
                                            }
                                            else
                                            {
                                                certificate = strFile.Contains("CurrentUser")
                                                    ? new X509Certificate2(strFile, _strCertPass,
                                                        X509KeyStorageFlags.Exportable &
                                                        X509KeyStorageFlags.PersistKeySet &
                                                        X509KeyStorageFlags.UserKeySet)
                                                    : new X509Certificate2(strFile, _strCertPass,
                                                        X509KeyStorageFlags.Exportable &
                                                        X509KeyStorageFlags.PersistKeySet &
                                                        X509KeyStorageFlags.MachineKeySet);
                                                store.Add(certificate);
                                                certsImported++;
                                                LogInfoInLogFile("Imported Cert: " + certName);
                                            }


                                        }
                                    }
                                    else
                                    {
                                        string certName1 = certName.Substring(0, certName.LastIndexOf('_'));
                                        certName = certName1.Substring(certName.IndexOf('_') + 1);
                                        if (Array.IndexOf(dstCertList, certName) < 0) //cert  already exists in Dst
                                        {
                                            if (Path.GetExtension(strFile) == ".PFX")
                                            {
                                                if (strFile.Contains("CurrentUser"))
                                                    commandArguments =
                                                        "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                        " -p " + "\"" + _strCertPass + "\"" + " -User " + storeNam +
                                                        " " + "\"" + strFile + "\"";
                                                else
                                                    commandArguments =
                                                        "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                        " -p " + "\"" + _strCertPass + "\"" + " " + storeNam + " " +
                                                        "\"" + strFile + "\"";
                                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                                                if (returnCode == 0)
                                                {
                                                    LogInfoInLogFile("Success: Imported Cert" + certName);
                                                }
                                                else
                                                    LogShortErrorMsg("Failed Importing Cert:" + certName);
                                                certsImported++;
                                            }
                                            else
                                            {
                                                certificate = strFile.Contains("CurrentUser")
                                                    ? new X509Certificate2(strFile, _strCertPass,
                                                        X509KeyStorageFlags.Exportable &
                                                        X509KeyStorageFlags.PersistKeySet &
                                                        X509KeyStorageFlags.UserKeySet)
                                                    : new X509Certificate2(strFile, _strCertPass,
                                                        X509KeyStorageFlags.Exportable &
                                                        X509KeyStorageFlags.PersistKeySet &
                                                        X509KeyStorageFlags.MachineKeySet);

                                                store.Add(certificate);
                                                certsImported++;
                                                LogInfoInLogFile("Imported Cert: " + certName1);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogInfo("Exception: Cert File Name: " + strFile);
                                    LogException(ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }
                        finally
                        {
                            store.Close();
                        }
                    }
                }
                if (certsImported > 0)
                    LogShortSuccessMsg("Success: Imported Cert.");
                else
                    LogInfo("No new certificates to import.");

            }
            else //remote
            {
                LogInfo("CERT: Import started.");
                LogInfo("Copying certificates to remote Machine: " + _strDstNode);
                //copy cert from local to remote
                string strSrc = _certPath;
                string remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                string strDst = "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + _certFolderName;
                commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " + "/E /COPYALL /R:1";
                returnCode = ExecuteCmd("CMD.EXE", commandArguments); //copy cert Folder to Remote destination server

                if (returnCode < _strRoboCopySuccessCode)
                {
                    LogShortSuccessMsg("Success: Certificates copied to remote.");
                    LogInfo("Certificate Import started on Remote Machine.");
                    //now execute .net exe on remote machine
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                       _strUserNameForHost + " -p " + "\"" + _strPasswordForHost + "\"" +
                                       " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + _remoteRootFolder +
                                       "\" \"ImportCert\" \"" + _strCertPass + "\"\"";

                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Certificates Imported.");
                    else
                        LogShortErrorMsg("Failed: Certificates Import.");
                }
                else
                    LogShortErrorMsg("Failed: Certificate copy to remote.");

                strSrc = "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + "\\Logs";
                strDst = _logPath;
                commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " + "/MOVE /R:1";
                returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                if (returnCode < _strRoboCopySuccessCode)
                    LogInfoInLogFile("Copied Certificate log to local.");
                else
                    LogShortErrorMsg("Failed: Copying Cert Logs to local ");
            }


        }

        #endregion

        #region Assemblies

        private int btnGetAssembliesList_Click(object sender, EventArgs e)
        {
            string appNameCollectionString = string.Empty;
            char[] charSeprator = {','};

            try
            {
                LogInfo("Assembly: Getting Assembly List.");
                if (File.Exists(_xmlPath + @"\SrcBizTalkAssembly.xml")) //delete if existing, create fresh copy
                {
                    File.Delete(_xmlPath + @"\SrcBizTalkAssembly.xml");
                }
                //read App.xml
                //if App.xml is not existing then create one.
                if (!File.Exists(_xmlPath + @"\Apps.xml")) //if Apps.xml does not exist then create
                {
                    if (btnGetApplicationList_Click(sender, e) == 1)
                        throw new FileNotFoundException("Failed: Creating Apps.xml");
                }

                XElement xelement = XElement.Load(_xmlPath + @"\Apps.xml");

                var appList = (from element in xelement.Descendants("BizTalkApplication")
                        let dCode = (string) element.Attribute("DependencyCode")
                        orderby dCode descending
                        select new
                        {
                            DcodeAppName = dCode + "," + element.Attribute("ApplicationName").Value
                        })
                    .ToList();

                appNameCollectionString = appList.Aggregate(appNameCollectionString,
                    (current, app) => current.TrimStart(',') + ',' + app.DcodeAppName.Split(charSeprator)[1]);


                if (_machineName == _strSrcNode) //local
                {
                    LogInfo("Connecting to BizTalkMgmtDb..." + txtConnectionString.Text);
                    // instantiate new instance of Explorer OM
                    AssemblyList asmList;
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + txtConnectionString.Text.Trim() +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        LogInfo("Connected.");

                        asmList = new AssemblyList
                        {
                            Assembly = btsExp.Applications.Cast<Application>()
                                .Where(app => appNameCollectionString.Contains(app.Name))
                                .SelectMany(app => app.Assemblies.Cast<BtsAssembly>()
                                    .Where(btAsm => !btAsm.IsSystem), (app, btAsm) => new AssemblyListAssembly
                                {
                                    AppName = app.Name,
                                    AsmVer = btAsm.Version,
                                    AsmName = btAsm.Name
                                })
                                .ToArray()
                        };
                    }

                    //BEGIN::asm Custom list


                    //END::asm Custom list

                    SerializeObject(asmList, _xmlPath + @"\SrcBizTalkAssembly.xml");
                    LogInfo("Success: Created SrcBizTalkAssembly.xml.");
                    return 0;
                }
                else //remote
                {
                    LogInfo("Assembly: Getting Assembly list.");
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    string commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                              _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                              " -accepteula" +
                                              "  \"" +
                                              _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                              _machineName +
                                              "\\" + appPathUnc + "\" \"ExportAsmList\" \"" +
                                              txtConnectionString.Text.Trim() + "\" \"" + appNameCollectionString +
                                              "\"\"";
                    var returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                    {
                        LogInfo("Success: Created SrcBizTalkAssembly.xml.");
                        return 0;
                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Creating SrcBizTalkAssembly.xml.");
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogShortErrorMsg("Failed: Creating Assembly.xml.");
                LogException(ex);
                return 1;
            }
        }

        private void btnExportAssemblies_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            AssemblyList asmList = null;
            List<string> customDll = null;
            int customDlls = 0;
            char[] chrSep = {','};

            try
            {
                LogInfo("Assemblies: Export started.");
                if (_strServerType == "BizTalk")
                {
                    if (btnGetAssembliesList_Click(sender, e) == 1)
                        throw new InvalidOperationException("Failed: Creating Assembly List");

                    if (!File.Exists(_xmlPath + @"\SrcBizTalkAssembly.xml"))
                        throw new FileNotFoundException("File: " + _xmlPath +
                                            @"\SrcBizTalkAssembly.xml does not exist, Assembly Export is termintated please check logs for root cause.");

                    asmList = DeserializeObject<AssemblyList>(_xmlPath + @"\SrcBizTalkAssembly.xml");
                }
                var asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                var asmPath2 = @"C:\Windows\assembly\GAC\";
                var asmPath3 = @"C:\Windows\assembly\GAC_32\";
                var asmPath4 = @"C:\Windows\assembly\GAC_64\";
                var asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

                try
                {
                    if (Directory.Exists(_asmPath)) //Recreate DLL Folder
                    {
                        Directory.Delete(_asmPath, true);
                        Directory.CreateDirectory(_asmPath);
                    }

                    if (Directory.Exists(_customDllPath)) //Recreate Custom Dll Folder
                    {
                        Directory.Delete(_customDllPath, true);
                        Directory.CreateDirectory(_customDllPath);
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    throw new InvalidOperationException("Error while cleaning DLL Folders.");
                }

                if (_machineName == _strSrcNode) //local
                {
                    if (_strCustomDllToInclude != string.Empty) //if custom Dll filter not empty
                    {
                        customDll = new List<string>();
                        foreach (string customDllFilter in _strCustomDllToInclude.Split(chrSep, StringSplitOptions.RemoveEmptyEntries))
                        {
                            //BEGIN::custom asm list code                         
                            customDll.AddRange(Directory.GetFiles(asmPath1, customDllFilter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath2, customDllFilter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath3, customDllFilter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath4, customDllFilter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath5, customDllFilter, SearchOption.AllDirectories));
                        }
                        LogInfo("Initial Custom Assembly count: " + customDll.Count);

                        customDll = customDll.Distinct().ToList();
                        customDll.Sort();

                        //END::custom asm list code
                    }
                    string dir;
                    if (_strServerType == "BizTalk")
                    {
                        LogInfo("BizTalk Dll: Export started.");
                        List<string> bizTalkDll = new List<string>();
                        foreach (AssemblyListAssembly assembly in asmList.Assembly)
                        {
                            try
                            {
                                var findDll = Directory.GetFiles(asmPath1, assembly.AsmName + ".dll",
                                    SearchOption.AllDirectories);
                                if (findDll.Length == 0)
                                    findDll = Directory.GetFiles(asmPath2, assembly.AsmName + ".dll",
                                        SearchOption.AllDirectories);
                                if (findDll.Length == 0)
                                    findDll = Directory.GetFiles(asmPath3, assembly.AsmName + ".dll",
                                        SearchOption.AllDirectories);
                                if (findDll.Length == 0)
                                    findDll = Directory.GetFiles(asmPath4, assembly.AsmName + ".dll",
                                        SearchOption.AllDirectories);
                                if (findDll.Length == 0)
                                    findDll = Directory.GetFiles(asmPath5, assembly.AsmName + ".dll",
                                        SearchOption.AllDirectories);

                                if (findDll.Length == 0)
                                {
                                    LogShortErrorMsg("Did not Find Assembly:" + assembly.AsmName);
                                }
                                else
                                {
                                    foreach (string filePath in findDll)
                                    {
                                        try
                                        {
                                            if (_strCustomDllToInclude != string.Empty
                                            ) //if custom Dll fileter not empty
                                            {
                                                //BEGIN::custom asm list code
                                                while (customDll.Contains(filePath)
                                                ) //if same dll is part of custom DLL then empty that entry in list.
                                                {

                                                    customDll[customDll.IndexOf(filePath)] = string.Empty;
                                                    customDlls++;
                                                }
                                                //END::custom asm list code
                                            }
                                            var ver = AssemblyName.GetAssemblyName(filePath).Version.ToString();
                                            dir = _asmPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_" +
                                                  ver;

                                            if (!Directory.Exists(dir))
                                            {
                                                Directory.CreateDirectory(dir);
                                            }
                                            //dir = ConvertPathToUncPath(dir);
                                            File.Copy(filePath, dir + "\\" + Path.GetFileName(filePath), true);
                                            LogInfoInLogFile(
                                                "Assembly copied: " + Path.GetFileName(filePath) + ", " + ver);
                                            // writer.WriteLine(Path.GetFileNameWithoutExtension(filePath) + "_" + ver);
                                            bizTalkDll.Add(Path.GetFileNameWithoutExtension(filePath) + "_" + ver);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogException(ex);
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(ex);
                            }
                        }
                        string[] distinctBizTalkDll = bizTalkDll.Distinct().ToArray();
                        using (StreamWriter writer = new StreamWriter(_xmlPath + @"\SrcBizTalkAssemblyList.txt", false))
                        {
                            foreach (string dll in distinctBizTalkDll)
                            {
                                writer.WriteLine(dll);
                            }
                        }
                        LogShortSuccessMsg("Success: Exported BizTalk Dll");

                    }

                    if (_strCustomDllToInclude != string.Empty) //if custom Dll fileter not empty
                    {
                        LogInfo("Final Custom Assembly count: " + (customDll.Count - customDlls));
                        LogInfo("Custom Dll: Export started.");
                        //write custom dll paths in txt file
                        try
                        {
                            using (StreamWriter writer =
                                new StreamWriter(_xmlPath + @"\SrcCustomAssemblyList.txt", false))
                            {
                                //copy custom dlls in CustomDll Folder
                                foreach (string filePath in customDll)
                                {
                                    string customDllVer = "";
                                    try
                                    {

                                        if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrWhiteSpace(filePath))
                                        {
                                            customDllVer = AssemblyName.GetAssemblyName(filePath).Version.ToString();
                                            dir = _customDllPath + "\\" + Path.GetFileNameWithoutExtension(filePath) +
                                                  "_" + customDllVer;

                                            if (!Directory.Exists(dir))
                                            {
                                                Directory.CreateDirectory(dir);
                                            }
                                            //dir = ConvertPathToUncPath(dir);
                                            File.Copy(filePath, dir + "\\" + Path.GetFileName(filePath), true);
                                            writer.WriteLine(Path.GetFileNameWithoutExtension(filePath) + "_" +
                                                             customDllVer);
                                            LogInfoInLogFile("Assembly copied: " + Path.GetFileName(filePath) + ", " +
                                                             customDllVer);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogShortErrorMsg("Assembly copy failed: " + Path.GetFileName(filePath) + ", " +
                                                         customDllVer);
                                        LogException(ex);
                                    }
                                }
                            }
                            LogShortSuccessMsg("Success: Exported Custom Dll.");
                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }

                    }
                }
                else //remote
                {
                    string customDllPathUnc = "\\\\" + _machineName + "\\" + ConvertPathToUncPath(_customDllPath);
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    string asmPathUnc = ConvertPathToUncPath(_asmPath);
                    string commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                              _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                              " -accepteula" +
                                              "  \"" +
                                              _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" +
                                              _machineName +
                                              "\\" + appPathUnc + "\"" + " \"ExportDll\" \"\\\\" + _machineName + "\\" +
                                              asmPathUnc + "\" \"" + _strCustomDllToInclude + "\" \"" +
                                              customDllPathUnc + "\" \"" + _strServerType + "\"\"";
                    var returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Biztalk Assemblies Exported.");
                    else
                        LogShortErrorMsg("Failed: Biztalk Assemblies Export.");
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnImportAssemblies_Click(object sender, EventArgs e)
        {
            // XmlSerializer configSerializer = null;
            //  AssemblyList asmList = null;
            int returnCode = 0;
            //  int asmListCount = 0;
            char[] chrSep = {','};

            try
            {
                LogInfo("Assembly: Import started.");

                if (!File.Exists(_xmlPath + @"\SrcBizTalkAssemblyList.txt"))
                    throw new FileNotFoundException("SrcBizTalkAssemblyList txt file does not exist.");



                try
                {
                    if (File.Exists(_xmlPath + @"\BizTalkAssemblyToImport.txt")) //delete older version
                    {
                        File.Delete(_xmlPath + @"\BizTalkAssemblyToImport.txt");
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    throw new InvalidOperationException("Error while deleting BizTalkAssemblyToImport txt file.", ex);
                }

                if (_strServerType == "BizTalk")
                {
                    //update AssemblyList, remove those rows where App already exists in Dst
                    UpdateAssemblyFile();
                    LogInfo("Generated Delta of BizTalk Assembly list to be imported.");

                }

                string commandArguments;
                if (_strCustomDllToInclude != string.Empty)
                {
                    // BEGIN::custom DLL Code::Get  "DstCustomAssemblyList.txt"
                    if (_machineName == _strDstNode) //local
                    {
                        string asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                        string asmPath2 = @"C:\Windows\assembly\GAC\";
                        string asmPath3 = @"C:\Windows\assembly\GAC_32\";
                        string asmPath4 = @"C:\Windows\assembly\GAC_64\";
                        string asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

                        var customDllDst = new List<string>();
                        foreach (string customDllFilter in _strCustomDllToInclude.Split(chrSep, StringSplitOptions.RemoveEmptyEntries))
                        {
//BEGIN::custom asm list code                         

                            customDllDst.AddRange(Directory.GetFiles(asmPath1, customDllFilter, SearchOption.AllDirectories));
                            customDllDst.AddRange(Directory.GetFiles(asmPath2, customDllFilter, SearchOption.AllDirectories));
                            customDllDst.AddRange(Directory.GetFiles(asmPath3, customDllFilter, SearchOption.AllDirectories));
                            customDllDst.AddRange(Directory.GetFiles(asmPath4, customDllFilter, SearchOption.AllDirectories));
                            customDllDst.AddRange(Directory.GetFiles(asmPath5, customDllFilter, SearchOption.AllDirectories));
                        }

                        customDllDst = customDllDst.Distinct().ToList();
                        customDllDst.Sort();

                        //write custom dll paths in txt file
                        using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstCustomAssemblyList.txt", false))
                        {
                            foreach (string filePathCusDll in customDllDst)
                            {
                                if (filePathCusDll != string.Empty)
                                {
                                    string customDllVer =
                                        AssemblyName.GetAssemblyName(filePathCusDll).Version.ToString();
                                    writer.WriteLine(Path.GetFileNameWithoutExtension(filePathCusDll) + "_" +
                                                     customDllVer);
                                }

                            }
                        }
                    }
                    else //remote, get list of Custom Dlls from Dst
                    {
                        string appPathUnc = ConvertPathToUncPath(_appPath);
                        commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strDstNode + " -u " + "\"" +
                                           _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                           "  \"" +
                                           _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                           "\\" + appPathUnc + "\"" + " \"DstCustomDllList\" \"" +
                                           _strCustomDllToInclude + "\"\"";
                        returnCode =
                            ExecuteCmd("CMD.EXE",
                                commandArguments); //DstCustomAssemblyList.txt will be Generated in XML Folder

                        if (returnCode == 0)
                            LogInfo("Created DstCustomAssemblyList.txt.");
                        else
                        {
                            LogShortErrorMsg("Failed: Creating DstCustomAssemblyList.txt.");
                            LogInfoInLogFile("Remote Exe return code: " + returnCode);
                        }
                    }
                }
                //now compare both DstCustomAssemblyList.txt and SrcCustomAssemblyList.txt

                if (_strCustomDllToInclude != string.Empty)
                {

                    string[] srcLines = File.ReadAllLines(_xmlPath + @"\SrcCustomAssemblyList.txt");
                    string[] dstLines = File.ReadAllLines(_xmlPath + @"\DstCustomAssemblyList.txt");

                    foreach (string srcFilePath in srcLines.Where(srcFilePath => Array.IndexOf(dstLines, srcFilePath) > -1))
                    {
                        srcLines[Array.IndexOf(srcLines, srcFilePath)] = string.Empty;
                    }
                    using (StreamWriter writer = new StreamWriter(_xmlPath + @"\CustomAssemblyToImport.txt", false))
                    {
                        foreach (string filePathDll in srcLines.Where(filePathDll => filePathDll != string.Empty))
                        {
                            writer.WriteLine(filePathDll);
                        }
                    }

                    //END::custom DLL Code
                }
                //actual gacing code
                //remote copy 
                if (_machineName != _strDstNode
                ) //if gacing has to be done on remote box then copy BizTalk DLL and Custom Dll folder on remote
                {
                    string strSrc, strDst;
                    var remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                    if (_strServerType == "BizTalk")
                    {
                        //copy BizTalk Dll Folder
                        strSrc = _asmPath;

                        strDst = "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + _asmFolderName;
                        commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/E /COPYALL /R:1";
                        returnCode =
                            ExecuteCmd("CMD.EXE", commandArguments); //copy DLL Folder to Remote destination server 
                        if (returnCode < _strRoboCopySuccessCode)
                            LogShortSuccessMsg("Success: Copied DLL folder to remote.");
                        else
                            LogShortErrorMsg("Failed: Copying DLL folder to remote.");
                    }
                    //custom Dll Folder
                    strSrc = _customDllPath;
                    strDst = "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + _customDllFolderName;
                    commandArguments = @"/C robocopy " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                       "/E /COPYALL /R:1";
                    returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //copy Custom DLL Folder to Remote destination server 
                    if (returnCode < _strRoboCopySuccessCode)
                        LogShortSuccessMsg("Success: Copied Custom DLL folder to remote.");
                    else
                        LogShortErrorMsg("Failed: Copying Custom DLL folder to remote.");
                }

                string filePath;
                if (_strCustomDllToInclude != string.Empty)
                {
                    string[] srcLines = File.ReadAllLines(_xmlPath + @"\CustomAssemblyToImport.txt");
                    int flagCustomDllExists = 0;
                    LogInfo("Custom Dll Import Started.");
                    foreach (string srcLine in srcLines.Where(srcLine => srcLine != string.Empty))
                    {
                        flagCustomDllExists++;
                        try
                        {
                            string dllName = srcLine.Substring(0, srcLine.LastIndexOf("_"));
                            string dllNameRemote = srcLine + "\\" + dllName + ".dll";
                            filePath = _customDllPath + "\\" + srcLine + "\\" + dllName + ".dll";
                            if (_machineName == _strDstNode) //local
                            {
                                if (File.Exists(filePath))
                                {
                                    returnCode = ExecuteCmd(_gacUtilPath, String.Format("/i \"{0}\"", filePath));
                                }
                            }
                            else //remote, Custom DLL folder already copied above, now just execute gacutil using PsExec
                            {
                                commandArguments =
                                    "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                    _strUserName + "\"" + " -p " + "\"" + _strPassword + "\" " + " -accepteula" +
                                    " " +
                                    _remoteRootFolder + "\\GacUtil.exe -i " + " \"" + _remoteRootFolder +
                                    _customDllFolderName + "\\" + dllNameRemote + "\"\"";
                                returnCode = ExecuteCmd("CMD.EXE", commandArguments); //gac dll remotely
                            }

                            if (returnCode == 0)
                            {
                                LogInfo("Gaced Assembly: " + srcLine);

                            }
                            else
                                LogShortErrorMsg("Failed: Gac Assembly: " + srcLine);

                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }
                    }
                    if (flagCustomDllExists == 0)
                        LogInfo("Custom Dll list is empty.");
                    else
                        LogShortSuccessMsg("Success: Custom Assemblies Imported");
                }


                if (_strServerType == "BizTalk")
                {
                    string[] srcLines = File.ReadAllLines(_xmlPath + @"\BizTalkAssemblyToImport.txt");
                    int flagBizTalkDllExists = 0;
                    LogInfo("BizTalk Assembly Import Started.");
                    foreach (string srcLine in srcLines.Where(srcLine => srcLine != string.Empty))
                    {
                        flagBizTalkDllExists++;
                        try
                        {
                            string dllName = srcLine.Substring(0, srcLine.LastIndexOf("_"));
                            string dllNameRemote = srcLine + "\\" + dllName + ".dll";
                            filePath = _asmPath + "\\" + srcLine + "\\" + dllName + ".dll";

                            if (_machineName == _strDstNode) //local
                            {
                                if (File.Exists(filePath))
                                {
                                    returnCode = ExecuteCmd(_gacUtilPath, String.Format("/i \"{0}\"", filePath));
                                }
                            }
                            else //remote 
                            {
                                commandArguments =
                                    "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                    _strUserName + "\"" + " -p " + "\"" + _strPassword + "\" " + " -accepteula" +
                                    " " +
                                    _remoteRootFolder + "\\GacUtil.exe -i " + " \"" + _remoteRootFolder +
                                    _asmFolderName + "\\" + dllNameRemote + "\"\"";
                                returnCode = ExecuteCmd("CMD.EXE", commandArguments); //gac dll remotely
                            }

                            if (returnCode == 0)
                            {
                                LogInfo("Gaced Assembly: " + srcLine);

                            }
                            else
                                LogShortErrorMsg("Failed: Gac Assembly: " + srcLine);

                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }
                    }
                    if (flagBizTalkDllExists == 0)
                        LogInfo("BizTalk Assembly list is empty.");
                    else
                        LogShortSuccessMsg("Success: BizTalk Assemblies Imported");
                }
                if (_machineName != _strDstNode)
                {
                    //Delete Folders
                    try
                    {
                        string remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                        string asmFolder = "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + _asmFolderName;
                        string customDllFolder =
                            "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc + _customDllFolderName;
                        if (Directory.Exists(asmFolder))
                        {
                            Directory.Delete(asmFolder, true);
                        }
                        if (Directory.Exists(customDllFolder))
                        {
                            Directory.Delete(customDllFolder, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion

        #region Bam

        private string btnBamExport_Click(object sender, EventArgs e)
        {
            try
            {
                LogInfo("BAM: Export started.");

                LogInfo("Cleaning already existing Bam Files.");
                try //cleaning process before starting export.
                {
                    if (File.Exists(_xmlPath + "\\BamDef.xml"))
                    {
                        File.Delete(_xmlPath + "\\BamDef.xml");
                        LogInfoInLogFile("Deleted BamDef.xml.");
                    }

                    if (File.Exists(_xmlPath + "\\BamDefToImport.xml"))
                    {
                        File.Delete(_xmlPath + "\\BamDefToImport.xml");
                        LogInfoInLogFile("Deleted BamDefToImport.xml.");
                    }

                    if (File.Exists(_xmlPath + @"\SrcBamViewsList.txt"))
                    {
                        File.Delete(_xmlPath + @"\SrcBamViewsList.txt");
                        LogInfoInLogFile("Deleted SrcBamViewsList.txt.");
                    }

                    if (File.Exists(_xmlPath + @"\DstBamActivitiesList.txt"))
                    {
                        File.Delete(_xmlPath + @"\DstBamActivitiesList.txt");
                        LogInfoInLogFile("Deleted DstBamActivitiesList.txt.");
                    }

                    string[] bamViewFiles = Directory.GetFiles(_xmlPath, "BamView_*.txt", SearchOption.AllDirectories);
                    foreach (string bamViewFile in bamViewFiles)
                    {
                        File.Delete(bamViewFile);
                        LogInfoInLogFile("Deleted " + bamViewFile);
                    }

                    string[] bttFiles = Directory.GetFiles(_xmlPath, "BTT_*.xml", SearchOption.AllDirectories);
                    foreach (string bttFile in bttFiles)
                    {
                        File.Delete(bttFile);
                        LogInfoInLogFile("Deleted " + bttFile);
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "Error while cleaning Bam Files, please clean them manually and resume operation." +
                        ex.Message);
                }

                using (var sqlCon = new SqlConnection("Server=" + txtConnectionString.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    sqlCon.Open();

                    using (var sqlcmd = new SqlCommand("SELECT [BamDBServerName] FROM [adm_Group]", sqlCon))
                    {
                        using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (sqlRed.Read())
                            {
                                _srcBAMSqlInstance = sqlRed.GetString(0);
                            }
                        }
                    }
                }

                LogInfo("Connecting to BamPrimaryImport...." + _srcBAMSqlInstance);
                string commandArguments;
                int returnCode;
                if (_machineName == _strSrcNode)
                {
                    commandArguments = " get-defxml -FileName:\"" + _xmlPath + "\\BamDef.xml\" -Server:" +
                                       _srcBAMSqlInstance + " -Database:BAMPrimaryImport";
                    returnCode = ExecuteCmd(_bamExePath, commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: BAM Definition Exported.");
                    else
                    {
                        throw new InvalidOperationException("Failed: BAM Definition Export");
                    }
                }
                else
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ExportBamDefinition\" \"" + _srcBAMSqlInstance + "\" \"";
                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg(
                            "Success: Triggered BAM Defintion Export Remotely,Please Check Remote operation Log for Further Details.");
                    else
                    {
                        throw new InvalidOperationException("Failed: Triggering BAM Defintion Export");
                    }
                }


                if (!File.Exists(_xmlPath + @"\BamDef.xml"))
                    throw new FileNotFoundException("BamDef.xml is not Present");

                //Get all Views
                LogInfo("BAM: Get all views.");

                using (var connection = new SqlConnection("Server=" + _srcBAMSqlInstance +
                                                          ";Initial Catalog=BamPrimaryImport;Integrated Security=SSPI"))
                {
                    connection.Open();
                    using (var sqlCmd =
                        new SqlCommand(
                            "SELECT ViewName FROM bam_Metadata_Views;SELECT ProfileXml, ActivityName FROM bam_Metadata_TrackingProfiles;",
                            connection))
                    {
                        using (var sqlDataAd = new SqlDataAdapter(sqlCmd))
                        {
                            using (var ds = new DataSet())
                            {
                                sqlDataAd.Fill(ds);


                                ds.Tables[0].TableName = "BamViews";
                                ds.Tables[1].TableName = "BttFiles";
                                using (StreamWriter writer =
                                    new StreamWriter(_xmlPath + @"\SrcBamViewsList.txt", false))
                                {
                                    foreach (DataRow dRow in ds.Tables["BamViews"].Rows)
                                    {
                                        writer.WriteLine(dRow["ViewName"].ToString());
                                    }
                                }
                                LogInfo("Success: Created SrcBamViewsList.txt.");
                                //get all accounts for each view
                                foreach (DataRow dRow in ds.Tables["BamViews"].Rows)
                                {
                                    LogInfoInLogFile("BAM: Get Accounts for View: " + dRow["ViewName"]);
                                    if (_machineName == _strSrcNode)
                                    {
                                        commandArguments =
                                            "/C " + "\"\"" + _bamExePath + "\"" + " get-accounts -View:\"" +
                                            dRow["ViewName"] + "\" -Server:" + _srcBAMSqlInstance
                                            + " -Database:BAMPrimaryImport > \"" + _xmlPath + "\\BamView_" +
                                            dRow["ViewName"] + ".txt\"\"";
                                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                        if (returnCode == 0)
                                            LogShortSuccessMsg(
                                                "Success: Get BAM Accounts for View: " + dRow["ViewName"]);
                                        else
                                            LogShortErrorMsg("Failed: Get BAM Accounts for View: " + dRow["ViewName"]);
                                    }
                                    else
                                    {
                                        string appPathUnc = ConvertPathToUncPath(_appPath);
                                        commandArguments =
                                            "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strSrcNode + " -u " + "\"" +
                                            _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                            "  \"" +
                                            _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                            _machineName +
                                            "\\" + appPathUnc + "\" \"ExportBAMAccounts\" \"" + _srcBAMSqlInstance +
                                            "\" \"" + dRow["ViewName"] + "\"\"";

                                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                        if (returnCode == 0)
                                            LogShortSuccessMsg(
                                                "Success:  Remotely Trigggered  BAM Accounts for View: " +
                                                dRow["ViewName"] +
                                                ",Please Check Remote operation Log for Further Details.");
                                        else
                                            LogShortErrorMsg("Failed: Triggering Remotely BAM Accounts for View: " +
                                                             dRow["ViewName"]);
                                    }


                                }

                                //BTT Files
                                LogInfo("Starting BTT file export.");
                                foreach (DataRow dRow in ds.Tables["BttFiles"].Rows)
                                {
                                    string bttXml = dRow["ProfileXml"].ToString();
                                    string activityName = dRow["ActivityName"].ToString();
                                    XmlDocument xDoc = new XmlDocument();
                                    xDoc.LoadXml(bttXml);
                                    xDoc.Save(_xmlPath + "\\BTT_" + activityName + ".xml");
                                }
                            }
                        }
                    }
                }
                LogShortSuccessMsg("Success: Exported BTT files.");

                return string.Empty;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return _strSuccess;
        }

        private string btnBamImport_Click(object sender, EventArgs e)
        {
            string commandArguments;
            int returnCode;
            try
            {
                LogInfo("BAM: Import started.");

                if (!File.Exists(_xmlPath + @"\BamDef.xml"))
                    throw new FileNotFoundException("BamDef xml file does not exist.");

                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlPath + "\\BamDef.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file is empty.                
                    throw new InvalidOperationException("BamDef xml file is empty.");

                LogInfo("Cleaning already existing Bam Files: BamDefToImport.xml and DstBamActivitiesList.txt.");
                try //cleaning process before starting import, 2 files have to be deleted which are Generated anew.
                {
                    if (File.Exists(_xmlPath + "\\BamDefToImport.xml"))
                    {
                        File.Delete(_xmlPath + "\\BamDefToImport.xml");
                        LogInfo("Deleted BamDefToImport.xml.");
                    }

                    if (File.Exists(_xmlPath + @"\DstBamActivitiesList.txt"))
                    {
                        File.Delete(_xmlPath + @"\DstBamActivitiesList.txt");
                        LogInfo("Deleted DstBamActivitiesList.txt.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "Error while cleaning Bam Files, please clean them manually and resume operation." +
                        ex.Message);
                }

                using (SqlConnection sqlCon = new SqlConnection(
                    "Server=" + txtConnectionStringDst.Text.Trim() +
                    ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    sqlCon.Open();

                    using (var sqlcmd = new SqlCommand("SELECT [BamDBServerName] FROM [adm_Group]", sqlCon))
                    {
                        using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (sqlRed.Read())
                            {
                                _dstBAMSqlInstance = sqlRed.GetString(0);
                            }
                        }
                    }
                }

                LogInfo("Connecting Dst Sql..." + _dstBAMSqlInstance);
                //get existing activities from Dst machine and write into DstBamActivitiesList.txt file under XmlBinding Folder
                using (var connection = new SqlConnection("Server=" + _dstBAMSqlInstance +
                                                          ";Initial Catalog=BamPrimaryImport;Integrated Security=SSPI"))
                {
                    connection.Open();
                    using (var sqlCmd =
                        new SqlCommand(
                            "SELECT ActivityName FROM bam_Metadata_Activities;SELECT ProfileXml, ActivityName FROM bam_Metadata_TrackingProfiles;",
                            connection))
                    {
                        using (var sqlDataAd = new SqlDataAdapter(sqlCmd))
                        {
                            using (var ds = new DataSet())
                            {
                                sqlDataAd.Fill(ds);
                                ds.Tables[0].TableName = "Activites";
                                ds.Tables[1].TableName = "BttFiles";

                                //create DstBamActivitiesList.txt
                                using (StreamWriter writer =
                                    new StreamWriter(_xmlPath + @"\DstBamActivitiesList.txt", false))
                                {
                                    foreach (DataRow dRow in ds.Tables["Activites"].Rows)
                                    {
                                        writer.WriteLine(dRow["ActivityName"].ToString());
                                    }
                                }
                                //create DstBamBttList.txt
                                using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstBamBttList.txt", false))
                                {
                                    foreach (DataRow dRow in ds.Tables["BttFiles"].Rows)
                                    {
                                        writer.WriteLine(dRow["ActivityName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                LogInfo("Generated BamActivities list of Dst.");
                //update BamDef File
                UpdateBamDefFile(_xmlPath + "\\BamDef.xml", _xmlPath + "\\DstBamActivitiesList.txt");

                //check file is empty or not
                doc = new XmlDocument();
                doc.Load(_xmlPath + "\\BamDefToImport.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("BamDefToImport xml file is empty.");

                LogInfo("Generated BamActivities Delta to be imported.");


                //Now import Bam Def xml
                if (_machineName == _strDstNode)
                {
                    commandArguments = " deploy-all -DefinitionFile:\"" + _xmlPath + "\\BamDefToImport.xml\" -Server:" +
                                       _dstBAMSqlInstance
                                       + " -Database:BAMPrimaryImport";
                    returnCode = ExecuteCmd(_bamExePath, commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: BamDef Imported.");
                    else
                        throw new InvalidOperationException("BamDef import failed, hence aborting account import.");
                }
                else
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\" \"ImportBamDefinition\" \"" + _dstBAMSqlInstance + "\" \"";

                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg(
                            "Success: Triggered BamDef Import Remotely,Please check Remote Operations Log for More Details.");
                    else
                        throw new InvalidOperationException("Failed: Triggering BamDef Import , hence aborting account import.");
                }



                //read all view name from exported SrcBamViewsList.txt file
                string[] linesViewName = File.ReadAllLines(_xmlPath + @"\SrcBamViewsList.txt");

                foreach (string lineViewName in linesViewName)
                {
                    try
                    {
                        string[] lines;
                        try
                        {
                            //read file with ViewName to import Accounts
                            lines = File.ReadAllLines(_xmlPath + "\\BamView_" + lineViewName + ".txt");
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Could not read file: " + _xmlPath + "\\" + lineViewName + ".txt. " +
                                                ex.Message);
                        }
                        for (int i = 6; i < lines.Length; i++)
                        {
                            if (_machineName == _strDstNode)
                            {
                                commandArguments = " add-account -AccountName:\"" + lines[i] + "\" -View:\"" +
                                                   lineViewName + "\" -Server:"
                                                   + _dstBAMSqlInstance
                                                   + " -Database:BAMPrimaryImport";
                                returnCode = ExecuteCmd(_bamExePath, commandArguments);
                                if (returnCode == 0)
                                    LogShortSuccessMsg("Account: " + lines[i] + " added to view: " + lineViewName);
                                else
                                    LogShortErrorMsg("Account: " + lines[i] + " could not be added to view: " +
                                                     lineViewName);
                            }
                            else
                            {
                                string appPathUnc = ConvertPathToUncPath(_appPath);
                                commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                                   "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                                   " -accepteula" + "  \"" +
                                                   _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" +
                                                   _machineName + "\\" + appPathUnc + "\"" +
                                                   " \"ImportBAMAccounts\" \"" + _dstBAMSqlInstance + "\" \"" +
                                                   lines[i] + "\" \"" + lineViewName + "\"\"";

                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                if (returnCode == 0)
                                    LogShortSuccessMsg(" Triggered Remotely for Account: " + lines[i] +
                                                       " to add to view: " + lineViewName +
                                                       ", Please Check Remote operation Log for Further Details");
                                else
                                    LogShortErrorMsg("Account: " + lines[i] + " could not be added to view: " +
                                                     lineViewName + " Remotely");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
                }


            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            try
            {
                //btt import
                string bttDeployExePath =
                    _bamExePath.Substring(0, _bamExePath.LastIndexOf("\\") + 1) + "bttDeploy.exe ";
                string[] bttFiles = Directory.GetFiles(_xmlPath, "BTT_*.xml", SearchOption.AllDirectories);
                string[] dstBttActivities = File.ReadAllLines(_xmlPath + @"\DstBamBttList.txt");

                foreach (string bttFile in bttFiles)
                {
                    string srcActName = Path.GetFileNameWithoutExtension(bttFile);
                    srcActName =
                        srcActName.Substring(srcActName.IndexOf('_') +
                                             1); //remove btt_ prefix and get Src Activity Name.
                    if (!dstBttActivities.Contains(srcActName)) //if destination does not have that btt then import
                    {
                        if (_machineName == _strDstNode)
                        {
                            commandArguments = " /mgdb \"" + _dstSqlInstance + "\\BizTalkMgmtDb" + "\" \"" + bttFile +
                                               "\"";
                            returnCode = ExecuteCmd(bttDeployExePath, commandArguments);
                            if (returnCode == 0)
                                LogInfo("Sucess: BTT File Imported " +
                                        bttFile.Substring(bttFile.LastIndexOf("\\") + 1));
                            else
                                LogShortErrorMsg("Failed: BTT File Import " +
                                                 bttFile.Substring(bttFile.LastIndexOf("\\") + 1));
                        }
                        else
                        {
                            string appPathUnc = ConvertPathToUncPath(_appPath);
                            string bttFileUnc = ConvertPathToUncPath(bttFile);
                            commandArguments = "/C " + "\"\"" + _psExecPath + "\" -h \\\\" + _strDstNode + " -u " +
                                               "\"" +
                                               _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                               " -accepteula" + "  \"" +
                                               _remoteRootFolder + "\\" + _remoteExeName + "\" \"" + "\\\\" +
                                               _machineName + "\\" + appPathUnc + "\" \"ImportBTTList\" \"" +
                                               _dstSqlInstance + "\" \"" + "\\\\" + _machineName + "\\" + bttFileUnc +
                                               "\"\"";
                            returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                            if (returnCode == 0)
                                LogInfo("Sucess: Trigerred Remotely BTT File Import " +
                                        bttFile.Substring(bttFile.LastIndexOf("\\") + 1) +
                                        ", Please Check Remote operation Log for Further Details.");
                            else
                                LogShortErrorMsg("Failed: Triggering Remotely BTT File Import " +
                                                 bttFile.Substring(bttFile.LastIndexOf("\\") + 1));
                        }


                    }
                }

                // return strSuccess;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return _strSuccess;
        }

        #endregion

        #region Services

        private string btnServicesExport_Click(object sender, EventArgs e)
        {
            char[] chrSep = {','};
            string[] serviceName = _strWindowsServiceToIgnore.Split(chrSep);
            string userNameNoDomain =
                _strUserNameForHost.Substring(_strUserNameForHost.LastIndexOf("\\") +
                                              1); //userName with out domain like ectest.

            try
            {
                LogInfo("Services: Export started.");
                try
                {
                    if (Directory.Exists(_serviceFolderPath))
                    {
                        Directory.Delete(_serviceFolderPath, true);
                        Directory.CreateDirectory(_serviceFolderPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error while cleaning folder, hence aboting folder export " + ex.Message +
                                        ", " + ex.StackTrace);
                }

                int returnCode;
                string commandArguments;
                if (_machineName == _strSrcNode) //local
                {
                    SelectQuery query =
                        new SelectQuery("select name, startname, pathname, displayname from Win32_Service");
                    using (StreamWriter writer = new StreamWriter(_xmlPath + @"\SrcServiceList.txt", false))
                    {
                        using (ManagementObjectSearcher searcher =
                            new ManagementObjectSearcher(query))
                        {
                            foreach (var service in searcher.Get())
                            {
                                if (service["startname"] != null &&
                                    (service["startname"].ToString().ToLower().Contains(userNameNoDomain) ||
                                     service["startname"].ToString().ToLower().Contains(userNameNoDomain.ToLower())))
                                {
                                    if (!serviceName.Any(name => service["name"].ToString().Contains(name)))
                                    {
                                        string strPathName = service["pathname"].ToString();
                                        writer.WriteLine(service["name"] + "," + strPathName.Trim('"') + "," +
                                                         service["displayname"]);
                                    }
                                }
                            }
                        }
                    }
                    LogShortSuccessMsg("Success: SrcServicesList Exported.");
                }
                else //remote
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\"" + " \"SrcServiceList\" \""
                                       + _strWindowsServiceToIgnore + "\" \"" + userNameNoDomain + "\"\"";

                    returnCode =
                        ExecuteCmd("CMD.EXE",
                            commandArguments); //dlls will be copied and pasted back to Local machine, hence machineName used in commandArgument.

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: SrcServicesList Exported.");
                    else
                        LogShortErrorMsg("Failed: SrcServicesList Export.");
                }

                if (_strToolMode == "Backup")
                {
                    //read Source File and copy exe folder to local.
                    string[] srcLines = File.ReadAllLines(_xmlPath + @"\SrcServiceList.txt");

                    foreach (string srcLine in srcLines)
                    {
                        string[] srvDetails = srcLine.Split(chrSep);
                        string folderPathTrimed =
                            srvDetails[1]
                                .Substring(0, srvDetails[1].LastIndexOf("\\")); //go one step back from exe path
                        string driveInfo = Path.GetPathRoot(folderPathTrimed);
                        string driveLetter = driveInfo.Substring(0, 1);
                        string pathWithoutDrive = folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);
                        var strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                        var strDst = _serviceFolderPath + "\\" +
                                     folderPathTrimed.Substring(folderPathTrimed.LastIndexOf('\\'));
                        LogInfo("Copy started from:" + strSrc + " To " + strDst);
                        commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/E /COPYALL /MIN:1 /R:1"; //copy folders only with all permissons
                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode < _strRoboCopySuccessCode)
                            LogShortSuccessMsg("Success: Exported Folders.");
                        else
                            LogShortErrorMsg("Failed: Exporting Folders");
                    }
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return "";
        }

        private string btnServicesImport_Click(object sender, EventArgs e)
        {
            char[] chrSep = {','};
            string[] serviceName = _strWindowsServiceToIgnore.Split(chrSep);
            string userNameNoDomain =
                _strUserNameForHost.Substring(_strUserNameForHost.LastIndexOf("\\") +
                                              1); //userName with out domain like ectest.

            try
            {
                int returnCode;
                string commandArguments;
                if (_machineName == _strDstNode) //local
                {
                    try
                    {
                        SelectQuery query = new SelectQuery("select name, startname, pathname  from Win32_Service");
                        using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstServiceList.txt", false))
                        {
                            using (ManagementObjectSearcher searcher =
                                new ManagementObjectSearcher(query))
                            {
                                foreach (var service in searcher.Get()
                                    .Cast<ManagementBaseObject>()
                                    .Where(service =>
                                        service["startname"] != null &&
                                        (service["startname"].ToString().ToLower().Contains(userNameNoDomain) ||
                                         service["startname"]
                                             .ToString()
                                             .ToLower()
                                             .Contains(userNameNoDomain.ToLower())))
                                    .Where(service =>
                                        !serviceName.Any(name => service["name"].ToString().Contains(name))))
                                {
                                    writer.WriteLine(service["name"] + "," + service["pathname"]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(ex.Message, ex.InnerException);
                    }
                }
                else //remote
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       "  \"" +
                                       _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" + _machineName +
                                       "\\" +
                                       appPathUnc + "\"" + " \"DstServiceList\" \""
                                       + _strWindowsServiceToIgnore + "\" \"" + userNameNoDomain + "\"\"";

                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: DstServicesList Exported.");
                    else
                    {
                        LogShortErrorMsg("Failed: DstServicesList Export.");
                        throw new InvalidOperationException("Service Import aborted as DstServicesList.txt failed to generate.");
                    }
                }

                //compare source and destination
                string[] srcLines = File.ReadAllLines(_xmlPath + @"\SrcServiceList.txt");
                string[] dstLines = File.ReadAllLines(_xmlPath + @"\DstServiceList.txt");

                foreach (string srcLine in srcLines)
                {
                    string[] srvDetails = srcLine.Split(chrSep);

                    if (!dstLines.Any(dstLine => dstLine.Contains(srvDetails[0]))) //not found in Destination Machine, then copy folder and create service.
                    {
                        //copy folder
                        string folderPathTrimed = srvDetails[1].Substring(0, srvDetails[1].LastIndexOf("\\"));
                        string driveInfo = Path.GetPathRoot(folderPathTrimed);
                        string driveLetter = driveInfo.Substring(0, 1);
                        string pathWithoutDrive = folderPathTrimed.Substring(3, folderPathTrimed.Length - 3);

                        string strSrc;
                        if (_strToolMode == "Migrate")
                            strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                        else
                            strSrc = _serviceFolderPath + "\\" +
                                     folderPathTrimed.Substring(folderPathTrimed.LastIndexOf('\\'));
                        string strDst;
                        if (string.IsNullOrEmpty(_strServicesDrive) || string.IsNullOrWhiteSpace(_strServicesDrive))
                            strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                        else
                            strDst = "\\\\" + _strDstNode + "\\" + _strServicesDrive.Trim().Substring(0, 1) + "$\\" +
                                     pathWithoutDrive;
                        LogInfo("Copy started from: " + strSrc + " To " + strDst);
                        commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/E /COPYALL /MIN:1 /R:1"; // /xc /xn /xo exclude existing file and folders
                        returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode < _strRoboCopySuccessCode)
                        {
                            LogShortSuccessMsg("Success: Imported Service Folders/Files.");
                            //copy successful hence create Service
                            if (_strDstNode == _machineName) //local
                                commandArguments = @"/C sc create " + "\"" + srvDetails[0] + "\" DisplayName=\"" +
                                                   srvDetails[2] + "\" binPath=\"" + srvDetails[1] +
                                                   "\" start=auto obj=\"" + _strUserNameForHost + "\" password=\"" +
                                                   _strPasswordForHost + "\"";
                            else //remote
                                commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " +
                                                   "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                                   " -accepteula"
                                                   + " sc create " + "\"" + srvDetails[0] + "\" DisplayName=\"" +
                                                   srvDetails[2] + "\" binPath=\"" + srvDetails[1] +
                                                   "\" start=auto o=\"" + _strUserNameForHost + "\" password=\"" +
                                                   _strPasswordForHost + "\"\"";

                            returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                            if (returnCode == 0)
                                LogShortSuccessMsg("Success: Imported Service: " + srvDetails[0]);
                            else
                                LogShortErrorMsg("Failed: Importing Service: " + srvDetails[0]);
                        }
                        else
                            LogShortErrorMsg("Failed: Importing Service Folders/Files.");
                    }
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return "";
        }

        #endregion

        #region readioButton Checked Changed Events

        private void rbWebsiteYes_CheckedChanged(object sender, EventArgs e)
        {
            _strWebSite = rbWebsiteYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbAppPoolYes_CheckedChanged(object sender, EventArgs e)
        {
            _strAppPool = rbAppPoolYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbCertificateYes_CheckedChanged(object sender, EventArgs e)
        {
            _strCertificate = rbCertificateYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbHostInstanceYes_CheckedChanged(object sender, EventArgs e)
        {
            _strHostInstance = rbHostInstanceYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbHandlersYes_CheckedChanged(object sender, EventArgs e)
        {
            _strHandlers = rbHandlersYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbGlobalPartyBindingYes_CheckedChanged(object sender, EventArgs e)
        {
            _strGlobalPartyBinding = rbGlobalPartyBindingYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbBizTalkAssembliesYes_CheckedChanged(object sender, EventArgs e)
        {
            _strBizTalkAssemblies = rbBizTalkAssembliesYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbBizTalkAppYes_CheckedChanged(object sender, EventArgs e)
        {
            _strBizTalkApp = rbBizTalkAppYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbBamYes_CheckedChanged(object sender, EventArgs e)
        {
            _strBam = rbBamYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbFileCopyYes_CheckedChanged(object sender, EventArgs e)
        {
            _strFileCopy = rbFileCopyYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        private void rbMigrate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMigrate.Checked)
            {
                _strToolMode = "Migrate";
                LogInfo("Mode is set to Migration. Folder / VDir operation will be between Source and Destination.");
            }
            else
            {
                _strToolMode = "Backup";
                LogInfo("Mode is set to Backup. Folder / VDir operation will be between Source and Local.");
            }

        }

        private void rbApp_CheckedChanged(object sender, EventArgs e)
        {
            if (rbApp.Checked) //if App
            {
                _strServerType = "App";
                panel4.Enabled = false;
                panel5.Enabled = false;
                panel8.Enabled = false;
                panel9.Enabled = false;
                panel10.Enabled = false;
                cmbBoxServerDst.Visible = false;
                cmbBoxServerSrc.Visible = false;
                label2.Text = "Server  (Source)";
                label15.Text = "Server  (Destination)";
                txtConnectionString.Text = "SERVER NAME";
                txtConnectionStringDst.Text = "SERVER NAME";

                if (File.Exists(_serverXmlPath)) //reloading BizTalk Connection Info
                {
                    var srv = DeserializeObject<Servers>(_serverXmlPath);
                    txtConnectionString.Text = srv.SrcAppNode;
                    txtConnectionStringDst.Text = srv.DstAppNode;
                }

                if (txtConnectionString.Text != string.Empty && txtConnectionString.Text != "SERVER NAME")
                    _strSrcNode = txtConnectionString.Text;

                if (txtConnectionStringDst.Text != string.Empty && txtConnectionStringDst.Text != "SERVER NAME")
                    _strDstNode = txtConnectionStringDst.Text;
            }
            else //BizTalk
            {
                _strServerType = "BizTalk";
                panel4.Enabled = true;
                panel5.Enabled = true;
                panel8.Enabled = true;
                panel9.Enabled = true;
                panel10.Enabled = true;
                cmbBoxServerDst.Visible = true;
                cmbBoxServerSrc.Visible = true;
                label2.Text = "BizTalk Mgmt Grp (Src)";
                label15.Text = "BizTalk Mgmt Grp (Dst)";
                txtConnectionString.Text = "SQL INSTANCE";
                txtConnectionStringDst.Text = "SQL INSTANCE";

                if (File.Exists(_serverXmlPath)) //reloading BizTalk Connection Info
                {
                    char[] chrSep = {','};

                    var srv = DeserializeObject<Servers>(_serverXmlPath);
                    cmbBoxServerSrc.Items.Clear();
                    cmbBoxServerDst.Items.Clear();
                    if (srv.SrcSqlInstance != null)
                    {
                        txtConnectionString.Text = srv.SrcSqlInstance;
                        //lblServers.Text = obj.SrcNodes;
                        _srcSqlInstance = srv.SrcSqlInstance;
                        string[] srcNodes = srv.SrcNodes.Split(chrSep);
                        foreach (string srcNode in srcNodes)
                        {
                            cmbBoxServerSrc.Items.Add(srcNode);
                        }
                        cmbBoxServerSrc.Visible = true;
                    }
                    else
                    {
                        //  LogShortErrorMsg("Source Sql Instance is missing."); 
                    }


                    if (srv.DstSqlInstance != null)
                    {
                        txtConnectionStringDst.Text = srv.DstSqlInstance;
                        string[] dstNodes = srv.DstNodes.Split(chrSep);
                        foreach (string dstNode in dstNodes)
                        {
                            cmbBoxServerDst.Items.Add(dstNode);
                        }
                        cmbBoxServerDst.Visible = true;
                        _dstSqlInstance = srv.DstSqlInstance;
                    }
                    else
                    {
                        //  LogShortErrorMsg("Destination Sql Instance missing."); 
                    }

                }
            }
        }

        private void rbServicesYes_CheckedChanged(object sender, EventArgs e)
        {
            _strServices = rbServicesYes.Checked ? _strPerformOperationYes : _strPerformOperationNo;
        }

        #endregion

        #region Events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            richTextBoxLogs.AppendText("Esc Pressed");
            richTextBoxLogs.Refresh();
        }

        private void richTextBoxLogs_TextChanged_1(object sender, EventArgs e)
        {
            //if(richTextBoxLogs.len)
            richTextBoxLogs.ScrollToCaret();
            richTextBoxLogs.Refresh();
        }

        private void btnExportOperations_Click(object sender, EventArgs e)
        {
            try
            {
                _strExport = _strPerformOperationYes;

                if (_strServerType == "BizTalk" && (txtConnectionString.Text == "SQL INSTANCE" ||
                                                    txtConnectionString.Text.Trim() == "" ||
                                                    cmbBoxServerSrc.Items.Count == 0 ||
                                                    cmbBoxServerSrc.SelectedItem == null))
                {
                    LogShortErrorMsg("Please mention Source Sql Instance and select node from DropDown.");
                }
                else if (_strServerType == "App" &&
                         (txtConnectionString.Text == "SERVER NAME" || txtConnectionString.Text.Trim() == ""))
                {
                    LogShortErrorMsg("Please mention Source App Server.");
                }
                else
                {
                    if (_strFileCopy == _strPerformOperationYes || _strAppPool == _strPerformOperationYes ||
                        _strWebSite == _strPerformOperationYes || _strCertificate == _strPerformOperationYes ||
                        _strHostInstance == _strPerformOperationYes || _strHandlers == _strPerformOperationYes ||
                        _strBizTalkApp == _strPerformOperationYes ||
                        _strGlobalPartyBinding == _strPerformOperationYes ||
                        _strBizTalkAssemblies == _strPerformOperationYes || _strBam == _strPerformOperationYes ||
                        _strServices == _strPerformOperationYes)
                    {
                        #region PreRequisite Check

                        if (_machineName != _strSrcNode && _strIsUtilCopied == _strPerformOperationNo
                        ) //remote, hence exe has to be copied.
                        {
                            LogInfo("Its Remote Export.");

                            LogInfo("Copying required artifacts to remote machine: " + _strSrcNode);

                            string remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                            var commandArguments = @"/C robocopy " + "\"" + _appPath + _gacUtilFolderName + "\"" +
                                                   " \"" +
                                                   "\\\\" + _strSrcNode + "\\" + remoteRootFolderUnc + "\" " + "\"" +
                                                   _remoteExeName + "\"" + " /IS /R:1";

                            var returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                            if (returnCode < _strRoboCopySuccessCode) //robocopy errorcode 1 means success
                            {
                                LogShortSuccessMsg("Copy completed from: " + _appPath + _gacUtilFolderName + " To " +
                                                   "\\\\" + _strSrcNode + "\\" + remoteRootFolderUnc);
                                _strIsUtilCopied = _strPerformOperationYes;
                            }
                            else
                            {
                                throw new InvalidOperationException(
                                    "Copying required artifacts to remote machine failed: " + _strSrcNode);
                            }
                        }

                        #endregion

                        EnableControls(false);

                        if (string.IsNullOrEmpty(_strUserName) && _machineName != _strSrcNode) //remote operation
                        {
                            panelLoginDialog.Visible = true;
                            _loginOperationName = "useraccount";
                            lblLoginDialog.Text =
                                "For remote deployment please provide credentials which has admin rights on destination server.";
                            txtUserName.Text = WindowsIdentity.GetCurrent().Name;
                            txtPassword.Text = "";
                            txtPassword.Focus();
                            goto Outer;
                        }

                        if ((_strHostInstance == _strPerformOperationYes ||
                             _strCertificate == _strPerformOperationYes && _machineName != _strSrcNode ||
                             _strServices == _strPerformOperationYes) && string.IsNullOrEmpty(_strUserNameForHost))
                        {
                            panelLoginDialog.Visible = true;
                            _loginOperationName = "serviceaccount";
                            lblLoginDialog.Text =
                                "You have selected Host/Certificate/WindowsService Please provide service account details..";
                            txtUserName.Text = ""; //redmond\ectest
                            txtPassword.Text = "";
                            txtUserName.Focus();
                            goto Outer;
                        }

                        if (_strCertificate == _strPerformOperationYes && _machineName == _strSrcNode)
                        {
                            MessageBox.Show(
                                "To Export the Service Account Certificates, Login Server with Service Account and rerun the Tool.");

                        }


                        if (_strServices == _strPerformOperationYes)
                        {
                            btnServicesExport_Click(sender, e);
                        }
                        /***************************************/
                        if (_strFileCopy == _strPerformOperationYes)
                        {
                            btnExportFolders_Click(sender, e);
                        }
                        /***************************************/
                        if (_strAppPool == _strPerformOperationYes)
                        {
                            btnExportAppPools_Click(sender, e);
                        }
                        /***************************************/
                        if (_strWebSite == _strPerformOperationYes)
                        {
                            btnExportWebSites_Click(sender, e);
                        }
                        /***************************************/
                        if (_strCertificate == _strPerformOperationYes)
                        {
                            btnExportCert_Click(sender, e);
                        }
                        /***************************************/
                        if (_strHostInstance == _strPerformOperationYes)
                        {
                            btnGetHost_Click(sender, e);
                        }
                        /***************************************/
                        if (_strHandlers == _strPerformOperationYes)
                        {
                            btnExportAdapterHandlers_Click(sender, e);
                        }
                        /***************************************/
                        if (_strBizTalkApp == _strPerformOperationYes)
                        {
                            btnExportApps_Click(sender, e);
                        }
                        /***************************************/
                        if (_strBizTalkAssemblies == _strPerformOperationYes)
                        {
                            btnExportAssemblies_Click(sender, e);
                        }
                        /***************************************/
                        if (_strGlobalPartyBinding == _strPerformOperationYes)
                        {
                            btnExportGlobalPartyBinding_Click(sender, e);
                        }
                        /***************************************/
                        if (_strBam == _strPerformOperationYes)
                        {
                            btnBamExport_Click(sender, e);
                        }

                        //reset all radio buttons to know
                        rbFileCopyNo.Checked = true;
                        rbAppPoolNo.Checked = true;
                        rbBizTalkAppNo.Checked = true;
                        rbBizTalkAssembliesNo.Checked = true;
                        rbCertificateNo.Checked = true;
                        rbGlobalPartyBindingNo.Checked = true;
                        rbHandlersNo.Checked = true;
                        rbHostInstanceNo.Checked = true;
                        rbWebsiteNo.Checked = true;
                        rbBamNo.Checked = true;
                        rbServicesNo.Checked = true;
                        EnableControls(true);

                        Outer:
                        ;
                    }
                    else
                    {
                        LogShortErrorMsg("Please select atleast one operation.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void btnImportOperations_Click(object sender, EventArgs e)
        {
            try
            {
                _strExport = _strPerformOperationNo;
                if (_strServerType == "BizTalk" && (txtConnectionStringDst.Text == "SQL INSTANCE" ||
                                                    txtConnectionStringDst.Text.Trim() == "" ||
                                                    cmbBoxServerDst.Items.Count == 0 ||
                                                    cmbBoxServerDst.SelectedItem == null))
                {
                    LogShortErrorMsg("Please mention Destination Sql Instance and select node from Drop Down.");
                }
                else if (_strServerType == "App" &&
                         (txtConnectionStringDst.Text == "SERVER NAME" || txtConnectionStringDst.Text.Trim() == ""))
                {
                    LogShortErrorMsg("Please mention Destination App Server.");
                }
                else
                {
                    if (_strFileCopy == _strPerformOperationYes || _strAppPool == _strPerformOperationYes ||
                        _strWebSite == _strPerformOperationYes || _strCertificate == _strPerformOperationYes ||
                        _strHostInstance == _strPerformOperationYes || _strHandlers == _strPerformOperationYes ||
                        _strBizTalkApp == _strPerformOperationYes ||
                        _strGlobalPartyBinding == _strPerformOperationYes ||
                        _strBizTalkAssemblies == _strPerformOperationYes || _strBam == _strPerformOperationYes ||
                        _strServices == _strPerformOperationYes)
                    {
                        #region PreRequisite Check

                        if (_machineName != _strDstNode && _strIsUtilCopied == _strPerformOperationNo
                        ) //remote, hence exe has to be copied.
                        {
                            LogInfo("Its Remote Import.");
                            LogInfo("Copying required artifacts to remote machine: " + _strDstNode);

                            string remoteRootFolderUnc = ConvertPathToUncPath(_remoteRootFolder);
                            string commandArguments = @"/C robocopy " + "\"" + _appPath + _gacUtilFolderName + "\"" +
                                                      " \"" + "\\\\" + _strDstNode + "\\" + remoteRootFolderUnc +
                                                      "\" " +
                                                      " /IS /R:1";

                            var returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                            if (returnCode < _strRoboCopySuccessCode) //robocopy returnCode 1 means success
                            {
                                LogShortSuccessMsg("RemoteOperations EXE Copy completed from: " + _appPath +
                                                   _gacUtilFolderName + " To " + "\\\\" + _strDstNode + "\\" +
                                                   remoteRootFolderUnc);
                                _strIsUtilCopied = _strPerformOperationYes;
                            }
                            else
                            {
                                throw new InvalidOperationException("Copy encountered errors. Remote import can not be triggered.");
                            }
                        }

                        #endregion

                        EnableControls(false);

                        if ((_strHostInstance == _strPerformOperationYes ||
                             _strCertificate == _strPerformOperationYes && _machineName != _strDstNode ||
                             _strServices == _strPerformOperationYes) && string.IsNullOrEmpty(_strUserNameForHost))
                        {
                            _strExport = _strPerformOperationNo;
                            panelLoginDialog.Visible = true;
                            _loginOperationName = "serviceaccount";
                            lblLoginDialog.Text =
                                "You have selected Host/Certificate/WindowsService Please provide service account details..";
                            txtUserName.Text = "";
                            txtPassword.Text = "";
                            goto Outer;
                        }

                        if (_strCertificate == _strPerformOperationYes && _machineName == _strDstNode)
                        {
                            MessageBox.Show(
                                "To Import the Service Account Certificates, Login Server with Service Account and rerun the Tool.");
                        }

                        if (_machineName != _strDstNode && string.IsNullOrEmpty(_strUserName)) //remote operation
                        {
                            panelLoginDialog.Visible = true;
                            _loginOperationName = "useraccount";
                            lblLoginDialog.Text =
                                "For remote deployment please provide credentials which has admin rights on destination server.";
                            txtUserName.Text = WindowsIdentity.GetCurrent().Name;
                            txtPassword.Text = "";
                            goto Outer;
                        }

                        //windows service
                        if (_strServices == _strPerformOperationYes)
                            btnServicesImport_Click(sender, e);

                        //FileCopy
                        if (_strFileCopy == _strPerformOperationYes)
                        {
                            btnImportFolders_Click(sender, e);
                        }

                        //appPools
                        if (_strAppPool == _strPerformOperationYes)
                        {
                            btnImportAppPools_Click(sender, e);
                        }

                        //webSites
                        if (_strWebSite == _strPerformOperationYes)
                        {
                            if (_isAppPoolExecuted == _strPerformOperationYes)
                            {
                                btnImportWebSites_Click(sender, e);
                            }
                            else
                            {
                                LogShortErrorMsg("WebApplication will be skipped as AppPool import failed.");
                            }
                        }

                        //Certificate
                        if (_strCertificate == _strPerformOperationYes)
                        {
                            btnImportCert_Click(sender, e);
                        }

                        //Host
                        //HostInstance:
                        if (_strHostInstance == _strPerformOperationYes)
                        {
                            //   strFromPanel10 = strPerformOperationNo;
                            btnSetHostAndHostInstances_Click(sender, e);
                        }

                        //Handlers
                        if (_strHandlers == _strPerformOperationYes)
                        {
                            if (_isHostExecuted == _strPerformOperationYes)
                            {
                                btnImportAdapterHandler_Click(sender, e);
                            }
                            else
                            {
                                LogShortErrorMsg("Handlers will be skipped as Host has failed.");
                            }
                        }

                        //Assemblies
                        if (_strBizTalkAssemblies == _strPerformOperationYes)
                        {
                            btnImportAssemblies_Click(sender, e);
                        }

                        //BizTalk Applications
                        if (_strBizTalkApp == _strPerformOperationYes)
                        {
                            if (_isHandlerExecuted == _strPerformOperationYes &&
                                _isHostExecuted == _strPerformOperationYes)
                            {
                                btnImportApps_Click(sender, e);
                            }
                            else
                            {
                                LogShortErrorMsg(
                                    "BizTalk Applications Import will be skipped as Host/Handler has failed");
                            }
                        }

                        //Global Party Binding
                        if (_strGlobalPartyBinding == _strPerformOperationYes)
                        {
                            if (_isHandlerExecuted == _strPerformOperationYes &&
                                _isHostExecuted == _strPerformOperationYes &&
                                _isBizTalkAppExecuted == _strPerformOperationYes)
                            {
                                btnImportGlobalPartyBinding_Click(sender, e);
                            }
                            else
                            {
                                LogShortErrorMsg(
                                    "Global Party Binding Import will be skipped as stleast one of Host/Handler/BizTalk App has failed");
                            }
                        }

                        //BAM
                        if (_strBam == _strPerformOperationYes)
                        {
                            btnBamImport_Click(sender, e);
                        }

                        rbFileCopyNo.Checked = true;
                        rbAppPoolNo.Checked = true;
                        rbBizTalkAppNo.Checked = true;
                        rbBizTalkAssembliesNo.Checked = true;
                        rbCertificateNo.Checked = true;
                        rbGlobalPartyBindingNo.Checked = true;
                        rbHandlersNo.Checked = true;
                        rbHostInstanceNo.Checked = true;
                        rbWebsiteNo.Checked = true;
                        rbBamNo.Checked = true;
                        rbServicesNo.Checked = true;
                        EnableControls(true);
                        _isHostExecuted = _strPerformOperationYes;
                        _isHandlerExecuted = _strPerformOperationYes;
                        _isAppPoolExecuted = _strPerformOperationYes;
                        Outer:
                        ;
                    }
                    else
                    {
                        LogShortErrorMsg("Please select atleast one operation.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void BizTalkAdminOperations_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(_exportedDataPath))
                Directory.CreateDirectory(_exportedDataPath);

            if (!Directory.Exists(_msiPath))
                Directory.CreateDirectory(_msiPath);

            if (!Directory.Exists(_xmlPath))
                Directory.CreateDirectory(_xmlPath);

            if (!Directory.Exists(_certPath))
                Directory.CreateDirectory(_certPath);

            if (!Directory.Exists(_asmPath))
                Directory.CreateDirectory(_asmPath);

            if (!Directory.Exists(_logPath))
                Directory.CreateDirectory(_logPath);

            if (!Directory.Exists(_customDllPath))
                Directory.CreateDirectory(_customDllPath);

            if (!Directory.Exists(_vDir))
                Directory.CreateDirectory(_vDir);

            if (!Directory.Exists(_fileFolderPath))
                Directory.CreateDirectory(_fileFolderPath);

            if (!Directory.Exists(_serviceFolderPath))
                Directory.CreateDirectory(_serviceFolderPath);

            if (File.Exists(_serverXmlPath))
            {
                char[] chrSep = {','};

                var srv = DeserializeObject<Servers>(_serverXmlPath);

                if (srv.SrcSqlInstance != null)
                {
                    txtConnectionString.Text = srv.SrcSqlInstance;
                    //lblServers.Text = obj.SrcNodes;
                    _srcSqlInstance = srv.SrcSqlInstance;
                    string[] srcNodes = srv.SrcNodes.Split(chrSep);
                    foreach (string srcNode in srcNodes)
                    {
                        cmbBoxServerSrc.Items.Add(srcNode);
                    }
                    cmbBoxServerSrc.Visible = true;
                }
                else
                {
                    //LogShortErrorMsg("Source Sql Instance is missing."); 
                }


                if (srv.DstSqlInstance != null)
                {
                    txtConnectionStringDst.Text = srv.DstSqlInstance;
                    string[] dstNodes = srv.DstNodes.Split(chrSep);
                    foreach (string dstNode in dstNodes)
                    {
                        cmbBoxServerDst.Items.Add(dstNode);
                    }
                    cmbBoxServerDst.Visible = true;
                    _dstSqlInstance = srv.DstSqlInstance;
                }
                else
                {
                    // cmbBoxServerDst.Visible = true;
                    // LogShortErrorMsg("Destination Sql Instance missing."); 
                }

            }
            ActiveControl = txtConnectionString;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            label18.Visible = false;
            char[] chrSep = {'\\'};
            if (txtUserName.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                label18.Text = "UserName or Password missing.";
                label18.Visible = true;
            }
            else
            {
                if (_loginOperationName == "useraccount") //for remote WMI, remote BizTalkApp 
                {
                    _strUserName = txtUserName.Text.Trim();
                    _strUserNameForWMI = txtUserName.Text.Split(chrSep)[1];
                    _strPassword = txtPassword.Text.Trim();
                    _strDomain = txtUserName.Text.Split(chrSep)[0];
                }
                else //Host
                {
                    _strUserNameForHost = txtUserName.Text.Trim();
                    _strPasswordForHost = txtPassword.Text.Trim();
                }

                panelLoginDialog.Visible = false;

                if (_strExport == _strPerformOperationYes)
                    btnExportOperations_Click(sender, e);
                else
                    btnImportOperations_Click(sender, e);
            }
        }

        private void cmbBoxServerSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            _strSrcNode = cmbBoxServerSrc.SelectedItem.ToString();
        }

        private void cmbBoxServerDst_SelectedIndexChanged(object sender, EventArgs e)
        {
            _strDstNode = cmbBoxServerDst.SelectedItem.ToString();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSubmit_Click(sender, e);
        }

        private void txtConnectionString_Validating(object sender, CancelEventArgs e)
        {
            if (_strServerType == "App" && _strSrcNode != txtConnectionString.Text.Trim() &&
                txtConnectionString.Text.Trim() != "SERVER NAME" && txtConnectionString.Text.Trim() != "SQL INSTANCE")
                TstSrcSqlConnection(true);

            if (_strServerType == "BizTalk" && _srcSqlInstance != txtConnectionString.Text.Trim() &&
                txtConnectionString.Text.Trim() != "SERVER NAME" && txtConnectionString.Text.Trim() != "SQL INSTANCE")
                TstSrcSqlConnection(true);
        }

        private void txtConnectionStringDst_Validating(object sender, CancelEventArgs e)
        {
            if (_strServerType == "App" && _strDstNode != txtConnectionStringDst.Text.Trim() &&
                txtConnectionStringDst.Text.Trim() != "SERVER NAME" &&
                txtConnectionStringDst.Text.Trim() != "SQL INSTANCE")
                TstDstSqlConnection(true);

            if (_strServerType == "BizTalk" && _dstSqlInstance != txtConnectionStringDst.Text.Trim() &&
                txtConnectionStringDst.Text.Trim() != "SERVER NAME" &&
                txtConnectionStringDst.Text.Trim() != "SQL INSTANCE")
                TstDstSqlConnection(true);
        }

        private void BizTalkAdminOperations_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new Settings(this))
            {
                settingsForm.ShowDialog();
            }
        }

        #endregion

        #region Functions

        private void GetSharePermission()
        {
            char[] chrSep = {','};
            if (_strFoldersToCopyNoFiles != string.Empty)
            {
                string[] arrFoldersToCopyNoFiles =
                    _strFoldersToCopyNoFiles.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);
                foreach (string folderName in arrFoldersToCopyNoFiles.Select(folderPath => folderPath.Trim().Substring(folderPath.LastIndexOf('\\') + 1)))
                {
                    string commandArguments;
                    if (_machineName == _strSrcNode) //local
                    {
                        commandArguments = "/C net share " + "\"\"" + folderName + " > \"" + _xmlPath +
                                           "\\SharePermission_" + folderName + ".txt\"\"";
                    }
                    else //remote
                    {
                        commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strSrcNode + " -u " + "\"" +
                                           _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                           + "  net share  \"" + folderName + "\" > \"" + _xmlPath +
                                           "\\SharePermission_" + folderName + ".txt\"\"";
                    }

                    int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Exported Shared Permissions: " + folderName);
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting Shared Permissions: " + folderName);
                        if (returnCode == 2)
                            LogShortErrorMsg("This shared resource does not exist.");
                    }
                }
            }
        }


        private void SetSharePermission()
        {
            char[] chrSep = {','};
            if (_strFoldersToCopyNoFiles != string.Empty)
            {
                string[] arrFoldersToCopyNoFiles =
                    _strFoldersToCopyNoFiles.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);
                foreach (string folderPath in arrFoldersToCopyNoFiles)
                {
                    var grantString = " ";
                    string folderName = folderPath.Substring(folderPath.LastIndexOf('\\') + 1);
                    string dstFolderPath;
                    if (string.IsNullOrEmpty(_strFoldersDrive) || string.IsNullOrWhiteSpace(_strFoldersDrive))
                    {
                        dstFolderPath = folderPath;
                    }
                    else
                        dstFolderPath = _strFoldersDrive.Trim().Substring(0, 1) + folderPath.Substring(1);
                    string[] srcLines = File.ReadAllLines(_xmlPath + "\\SharePermission_" + folderName + ".txt");
                    int permissionLineIndex = -1;
                    for (int i = 0; i < srcLines.Length; i++)
                    {
                        if (srcLines[i] == string.Empty
                        ) //once u got empty line, u exit bcos that mean permission section is over.
                            break;

                        if (srcLines[i].Contains("Permission")) //u got the line having word Permission
                            permissionLineIndex = i;

                        if (permissionLineIndex > 0) //start reading permission from this line
                        {
                            string[] permission = srcLines[i].Split(chrSep, StringSplitOptions.RemoveEmptyEntries);

                            if (i == permissionLineIndex)
                                grantString = grantString + "\"/GRANT:" + permission[0].Substring(18).Trim() + "," +
                                              permission[1].Trim() + "\" ";
                            else //GRANT:Everyone, FULL
                                grantString = grantString + "\"/GRANT:" + permission[0].Trim() + "," +
                                              permission[1].Trim() + "\" ";
                        }
                    }
                    string commandArguments;
                    if (_machineName == _strDstNode) //local
                    {
                        commandArguments = "/C net share " + folderName + "=\"" + dstFolderPath.Trim() + "\"" +
                                           grantString;
                        //net share sharename=c:\temp "/GRANT:fareast\v-somish,FULL" "/GRANT:fareast\sdhar,READ"
                    }
                    else //remote
                    {
                        commandArguments = "/C " + "\"\"" + _psExecPath + "\" -s \\\\" + _strDstNode + " -u " + "\"" +
                                           _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula"
                                           + " net share " + folderName + "=\"" + dstFolderPath.Trim() + "\"" +
                                           grantString.TrimEnd();
                    }

                    int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                    if (returnCode == 0)
                        LogShortSuccessMsg("Success: Imported Shared Permissions: " + folderName);
                    else if (returnCode == 2)
                        LogInfo("Share name already exist: " + folderName);
                    else
                        LogShortErrorMsg("Failed: Importing Shared Permissions: " + folderName);
                }
            }

        }

        private void UpdateAppPoolXml()
        {
            //delete existing Apppools from Xml which are there in Dst
            try
            {
                XElement root = XElement.Load(_xmlPath + "\\AppPools.xml");
                XNamespace ns = root.GetDefaultNamespace();
                string[] lines = File.ReadAllLines(_xmlPath + "\\AppPoolList.txt");

                foreach (var applicationList in lines.Select(item => root.Elements(ns + "APPPOOL")
                    .Where(el => el.Attribute("APPPOOL.NAME").Value.Equals(item))))
                {
                    applicationList.Remove();
                }
                root.Save(_xmlPath + "\\AppPoolToImport.xml");
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta of AppPools to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }

        private void UpdateWebAppXml(string pWebAppsFileName, string pWebSiteName)
        {
            string srcBTSInstallPath = string.Empty;
            string dstBTSInstallPath = string.Empty;
            string srcBTSInstallPath1 = string.Empty;
            //delete existing WebApps from Xml which are there in Dst
            try
            {
                //Reading the Src BTSInstallPath
                if (File.Exists(_xmlPath + "\\" + "SrcBTSInstallPath.txt"))
                {
                    string[] srcBTSInstall = File.ReadAllLines(_xmlPath + @"\srcBTSInstallPath.txt");

                    foreach (string srcBTS in srcBTSInstall)
                    {
                        var srcDrive = srcBTS.Split('=')[1].Split(':')[0];
                        srcBTSInstallPath = srcDrive.ToLower() + ":" + srcBTS.Split('=')[1].Split(':')[1];
                        srcBTSInstallPath1 = srcDrive.ToUpper() + ":" + srcBTS.Split('=')[1].Split(':')[1];
                    }
                }

                //Reading the Dst BTSInstallPath
                if (File.Exists(_xmlPath + "\\" + "DstBTSInstallPath.txt"))
                {
                    string[] dstBTSInstall = File.ReadAllLines(_xmlPath + @"\DstBTSInstallPath.txt");

                    foreach (string dstBTS in dstBTSInstall)
                    {
                        var dstDrive = dstBTS.Split('=')[1].Split(':')[0];
                        dstBTSInstallPath = dstDrive.ToUpper() + ":" + dstBTS.Split('=')[1].Split(':')[1];
                    }
                }
                XElement root = XElement.Load(_xmlPath + "\\" + pWebAppsFileName);
                XNamespace ns = root.GetDefaultNamespace();
                string[] lines = File.ReadAllLines(_xmlPath + "\\DstWebAppList.txt");

                foreach (var applicationList in lines.Select(item => (from el in root.Elements(ns + "APP")
                    where el.Attribute("APP.NAME").Value.Equals(item)
                    select el)))
                {
                    applicationList.Remove();
                }
                var result = from ele in root.Elements(ns + "APP")
                    select ele;
                foreach (var element in result)
                {
                    string physicalPath = element.Element(ns + "application").Element("virtualDirectory")
                        .Attribute("physicalPath").Value;
                    if (physicalPath.Contains(srcBTSInstallPath))
                    {
                        physicalPath = physicalPath.Replace(srcBTSInstallPath, dstBTSInstallPath);
                        element.Element(ns + "application").Element("virtualDirectory").Attribute("physicalPath")
                            .SetValue(physicalPath);

                    }
                    if (physicalPath.Contains(srcBTSInstallPath1))
                    {
                        physicalPath = physicalPath.Replace(srcBTSInstallPath1, dstBTSInstallPath);
                        element.Element(ns + "application").Element("virtualDirectory").Attribute("physicalPath")
                            .SetValue(physicalPath);

                    }
                    if (string.IsNullOrEmpty(_strWebsitesFolder) || string.IsNullOrWhiteSpace(_strWebsitesFolder))
                    {

                    }
                    else
                    {
                        physicalPath = element.Element(ns + "application").Element("virtualDirectory")
                            .Attribute("physicalPath").Value;
                        physicalPath = _strWebsitesFolder + ":" + physicalPath.Split(':')[1];
                        element.Element(ns + "application").Element("virtualDirectory").Attribute("physicalPath")
                            .SetValue(physicalPath);
                    }
                }
                root.Save(_xmlPath + "\\WebApps_" + pWebSiteName + "_ToImport.xml");
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta of WebApps to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }

        private void UpdateWebSiteXml()
        {
            //split website xml into indivudual websites with new ID value for each website.
            string[] dstLines = File.ReadAllLines(_xmlPath + "\\DstWebSiteList.txt");
            int dstSiteCount = dstLines.Length; //get count of websites existing in Dst
            int i = 1;
            string srcBTSInstallPath = string.Empty;
            string srcBTSInstallPath1 = string.Empty;
            string dstBTSInstallPath = string.Empty;
            try
            {

                //Reading the Src BTSInstallPath
                if (File.Exists(_xmlPath + "\\" + "SrcBTSInstallPath.txt"))
                {
                    string[] srcBTSInstall = File.ReadAllLines(_xmlPath + @"\srcBTSInstallPath.txt");

                    foreach (string srcBTS in srcBTSInstall)
                    {
                        var srcDrive = srcBTS.Split('=')[1].Split(':')[0];
                        srcBTSInstallPath = srcDrive.ToLower() + ":" + srcBTS.Split('=')[1].Split(':')[1];
                        srcBTSInstallPath1 = srcDrive.ToUpper() + ":" + srcBTS.Split('=')[1].Split(':')[1];
                    }
                }

                //Reading the Dst BTSInstallPath
                if (File.Exists(_xmlPath + "\\" + "DstBTSInstallPath.txt"))
                {
                    string[] dstBTSInstall = File.ReadAllLines(_xmlPath + @"\DstBTSInstallPath.txt");

                    foreach (string dstBTS in dstBTSInstall)
                    {
                        var dstDrive = dstBTS.Split('=')[1].Split(':')[0];
                        dstBTSInstallPath = dstDrive.ToUpper() + ":" + dstBTS.Split('=')[1].Split(':')[1];
                    }
                }

                XElement root = XElement.Load(_xmlPath + "\\WebSites.xml");
                XNamespace ns = root.GetDefaultNamespace();

                var applicationList =
                    from el in root.Elements(ns + "SITE").Elements(ns + "site").Elements(ns + "application")
                    where !el.Attribute("path").Value.Equals("/")
                    select el;
                applicationList.Remove();

                var result = from ele in root.Elements(ns + "SITE")
                    select ele;

                foreach (var element in result)
                {
                    element.Attribute("SITE.ID").SetValue(dstSiteCount + i);
                    element.Element(ns + "site").Attribute("id").SetValue(dstSiteCount + i);
                    string physicalPath = element.Element(ns + "site").Element(ns + "application")
                        .Element(ns + "virtualDirectory").Attribute("physicalPath").Value;
                    if (physicalPath.Contains(srcBTSInstallPath))
                    {
                        physicalPath = physicalPath.Replace(srcBTSInstallPath, dstBTSInstallPath);
                        element.Element(ns + "site").Element(ns + "application").Element(ns + "virtualDirectory")
                            .Attribute("physicalPath").SetValue(physicalPath);

                    }
                    if (physicalPath.Contains(srcBTSInstallPath1))
                    {
                        physicalPath = physicalPath.Replace(srcBTSInstallPath1, dstBTSInstallPath);
                        element.Element(ns + "site").Element(ns + "application").Element(ns + "virtualDirectory")
                            .Attribute("physicalPath").SetValue(physicalPath);

                    }
                    if (string.IsNullOrEmpty(_strWebsitesFolder) || string.IsNullOrWhiteSpace(_strWebsitesFolder))
                    {

                    }
                    else
                    {
                        physicalPath = element.Element(ns + "site").Element(ns + "application")
                            .Element(ns + "virtualDirectory").Attribute("physicalPath").Value;
                        physicalPath = _strWebsitesFolder.Trim().Substring(0, 1) + ":" + physicalPath.Split(':')[1];
                        element.Element(ns + "site").Element(ns + "application").Element(ns + "virtualDirectory")
                            .Attribute("physicalPath").SetValue(physicalPath);
                    }
                    XElement rootAppCmd = new XElement("appcmd", element);
                    rootAppCmd.Save(_xmlPath + "\\WebSite_" + element.Attribute("SITE.NAME").Value + "_ToImport.xml");
                    i++;
                }
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta for WebSites to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }

        private void UpdateSrcWebSiteXml()
        {
            try
            {
                XElement root = XElement.Load(_xmlPath + "\\WebSites.xml");
                XNamespace ns = root.GetDefaultNamespace();

                var result = from ele in root.Elements(ns + "SITE")
                    select ele;

                foreach (var element in result)
                {
                    XElement rootAppCmd = new XElement("appcmd", element);
                    rootAppCmd.Save(_xmlPath + "\\WebSite_" + element.Attribute("SITE.NAME").Value + "_ToExport.xml");
                }
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta for WebSites to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }


        private void UpdateAppXmlFile()
        {
            //this is genrating two file one is DstAppList.txt and second one is AppsToImport.xml            
            //Get all App name from Dst BizTalk Mgmt DB
            char[] chrSep = {','};
            try
            {
                using (var connection = new SqlConnection("Server=" + txtConnectionStringDst.Text.Trim() +
                                                          ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    connection.Open();
                    using (var sqlCmd =
                        new SqlCommand("SELECT nvcName AS AppName FROM bts_application ORDER BY nvcName ASC;",
                            connection))
                    {
                        using (var sqlDataAd = new SqlDataAdapter(sqlCmd))
                        {
                            using (var ds = new DataSet())
                            {
                                sqlDataAd.Fill(ds);

                                ds.Tables[0].TableName = "AppNames";

                                //write all apps in txt file, one app each line.
                                using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstAppList.txt", false))
                                {
                                    foreach (DataRow dRow in ds.Tables["AppNames"].Rows)
                                    {
                                        writer.WriteLine(dRow["AppName"].ToString());
                                    }

                                    string[] arrAppsToIgnore =
                                        _bizTalkAppToIgnore.Split(chrSep, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (string appToIgnore in (from appToIgnore in arrAppsToIgnore
                                        let find = "AppName = '" + appToIgnore.Trim() + "'"
                                        let foundRows = ds.Tables["AppNames"].Select(find)
                                        where !(foundRows.Length > 0)
                                        select appToIgnore).ToList())
                                    {
                                        writer.WriteLine(appToIgnore.Trim());
                                    }
                                }
                            }
                        }
                    }
                }

                //remove apps which already existing in destination
                XElement root = XElement.Load(_xmlPath + "\\Apps.xml");
                XNamespace ns = root.GetDefaultNamespace();
                string[] lines = File.ReadAllLines(_xmlPath + @"\DstAppList.txt");
                foreach (var activityList in lines.Select(item => (from el in root.Elements(ns + "BizTalkApplication")
                    where el.Attribute("ApplicationName").Value.Equals(item)
                    select el)))
                {
                    activityList.Remove();
                }
                root.Save(_xmlPath + "\\AppsToImport.xml");
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta for Assembly list to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }

        private void UpdateAssemblyFile()
        {
            try
            {
                string[] srcBizTalkDllLines = File.ReadAllLines(_xmlPath + @"\SrcBizTalkAssemblyList.txt");
                if (_machineName == _strDstNode) //local
                {
                    string asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                    string asmPath2 = @"C:\Windows\assembly\GAC\";
                    string asmPath3 = @"C:\Windows\assembly\GAC_32\";
                    string asmPath4 = @"C:\Windows\assembly\GAC_64\";
                    string asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

                    //Creating DestinationBiztalk Assembly List
                    var dllDst = new List<string>();
                    foreach (string biztalkDllName in srcBizTalkDllLines.Select(srcBizTalkDll => srcBizTalkDll.Substring(0, srcBizTalkDll.LastIndexOf('_'))))
                    {
                        dllDst.AddRange(Directory.GetFiles(asmPath1, biztalkDllName + "*.dll",
                            SearchOption.AllDirectories));
                        dllDst.AddRange(Directory.GetFiles(asmPath2, biztalkDllName + "*.dll",
                            SearchOption.AllDirectories));
                        dllDst.AddRange(Directory.GetFiles(asmPath3, biztalkDllName + "*.dll",
                            SearchOption.AllDirectories));
                        dllDst.AddRange(Directory.GetFiles(asmPath4, biztalkDllName + "*.dll",
                            SearchOption.AllDirectories));
                        dllDst.AddRange(Directory.GetFiles(asmPath5, biztalkDllName + "*.dll",
                            SearchOption.AllDirectories));
                    }
                    dllDst = dllDst.Distinct().ToList();
                    dllDst.Sort();

                    //write custom dll paths in txt file
                    using (StreamWriter writer = new StreamWriter(_xmlPath + @"\DstBizTalkAssemblyList.txt", false))
                    {
                        foreach (string filePathDll in dllDst)
                        {
                            if (filePathDll != string.Empty)
                            {
                                string dllVer = AssemblyName.GetAssemblyName(filePathDll).Version.ToString();
                                writer.WriteLine(Path.GetFileNameWithoutExtension(filePathDll) + "_" + dllVer);
                            }

                        }
                    }
                }
                else
                {
                    string appPathUnc = ConvertPathToUncPath(_appPath);
                    string commandArguments = "/C " + "\"\"" + _psExecPath + "\" \\\\" + _strDstNode + " -u " + "\"" +
                                              _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                              " -accepteula" +
                                              "  \"" +
                                              _remoteRootFolder + "\\" + _remoteExeName + "\" " + "\"\\\\" +
                                              _machineName +
                                              "\\" + appPathUnc + "\"" + " \"DstDllList\" \"";

                    int returnCode =
                        ExecuteCmd("CMD.EXE", commandArguments); //genrate handler xml and save it back to local
                    if (returnCode == 0)
                    {
                        LogShortSuccessMsg("Success: Exported Destination Biztalk AssemblyList.");

                    }
                    else
                    {
                        LogShortErrorMsg("Failed: Exporting Destination Biztalk AssemblyList.");

                    }
                }
                string[] dstBiztalkDllLines = File.ReadAllLines(_xmlPath + @"\DstBizTalkAssemblyList.txt");

                foreach (string srcFilePath in srcBizTalkDllLines.Where(srcFilePath => Array.IndexOf(dstBiztalkDllLines, srcFilePath) > -1))
                {
                    srcBizTalkDllLines[Array.IndexOf(srcBizTalkDllLines, srcFilePath)] = string.Empty;
                }
                using (StreamWriter writer = new StreamWriter(_xmlPath + @"\BizTalkAssemblyToImport.txt", false))
                {
                    foreach (string filePathDll in srcBizTalkDllLines.Where(filePathDll => filePathDll != string.Empty))
                    {
                        //string CustomDllVer = AssemblyName.GetAssemblyName(filePathDll).Version.ToString();
                        writer.WriteLine(filePathDll);
                    }
                }

            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta for Assembly list to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }




        private void UpdateBamDefFile(string pBamDefXmlFilePath, string pActivityNameTxtFilePath)
        {
            try
            {
                XElement root = XElement.Load(pBamDefXmlFilePath);
                XNamespace ns = root.GetDefaultNamespace();
                string[] lines = File.ReadAllLines(pActivityNameTxtFilePath);
                foreach (var activityList in lines.Select(item => (from el in root.Elements(ns + "Activity")
                    where el.Attribute("Name").Value.Equals(item)
                    select el)))
                {
                    foreach (var viewList in activityList.Select(activity => (from el in root.Elements(ns + "View").Elements(ns + "ActivityView")
                        where el.Attribute("ActivityRef").Value.Equals(activity.Attribute("ID").Value)
                        select el.Parent)))
                    {
                        foreach (var view in viewList)
                        {
                            var viewRefList = from el in root.Elements(ns + "Alerts").Elements(ns + "ViewAlert")
                                    .Elements(ns + "ViewRef")
                                where el.Value.Equals(view.Attribute("ID").Value)
                                select el;
                            viewRefList.Ancestors(ns + "ViewAlert").Remove();
                            ///////////////////////////////////////////////////////////////
                            var activityViewList = from el in view.Elements(ns + "ActivityView")
                                select el;

                            foreach (var cubeList in activityViewList.Select(activityView => (from el in root.Elements(ns + "Cube")
                                where el.Attribute("ActivityViewRef").Value
                                    .Equals(activityView.Attribute("ID").Value)
                                select el)))
                            {
                                cubeList.Remove();
                            }
                        }

                        viewList.Remove();
                    }
                    activityList.Remove();
                }
                root.Save(_xmlPath + "\\BamDefToImport.xml");
            }
            catch (Exception ex)
            {
                LogShortErrorMsg(
                    "Exception occured while genrating Delta for BamDef to be Imported, please check log file for details.");
                LogException(ex);
                throw;
            }
        }

        private void MsiApp(ApplicationCollection appCol, IDictionary<string, int> htApps)
        {
            foreach (Application app in appCol.Cast<Application>().Where(app => !_bizTalkAppToIgnore.Contains(app.Name)))
            {
                int i;
                if (htApps.TryGetValue(app.Name, out i))
                {
                    i++;
                    htApps[app.Name] = i;
                }
                else
                {
                    htApps.Add(app.Name, 1);
                }
                if (app.References.Count > 1)
                    MsiApp(app.References, htApps);
            }
        }

        private int UpdateResourceSpecFile(string pAppName)
        {
            //this will remove certain resources from spec file like certificates, web directory, and binding as they will be installed seprately
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_msiPath + "\\" + pAppName + _specFileExt);
                foreach (XmlNode resources in doc.DocumentElement.ChildNodes)
                {
                    for (int iNode = 0; iNode < resources.ChildNodes.Count;)
                    {
                        if (resources.ChildNodes[iNode].Attributes["Type"].Value == "System.BizTalk:BizTalkBinding" ||
                            resources.ChildNodes[iNode].Attributes["Type"].Value == "System.BizTalk:Certificate" ||
                            resources.ChildNodes[iNode].Attributes["Type"].Value == "System.BizTalk:WebDirectory")
                        {
                            resources.ChildNodes[iNode].ParentNode.RemoveChild(resources.ChildNodes[iNode]);
                            iNode--;
                        }
                        iNode++;
                    }
                }
                doc.Save(_msiPath + "\\" + pAppName + _specFileExt);
                return 0;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return 1;
            }

        }

        private void EnableControls(bool pTrueFalse)
        {
            if (_strServerType == "App" && pTrueFalse) //App
            {
                panel12.Enabled = true;
                panel13.Enabled = true;
                btnExportOperations.Enabled = true;
                btnImportOperations.Enabled = true;
                txtConnectionString.Enabled = true;
                txtConnectionStringDst.Enabled = true;
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;
                panel6.Enabled = true;
                panel7.Enabled = true;
                panel11.Enabled = true;
                panel14.Enabled = true;
            }
            else if (_strServerType == "App" && !pTrueFalse) //App
            {
                panel4.Enabled = false;
                panel5.Enabled = false;
                panel8.Enabled = false;
                panel9.Enabled = false;
                panel10.Enabled = false;

            }
            else if (_strServerType == "BizTalk")
            {
                panel12.Enabled = pTrueFalse;
                panel13.Enabled = pTrueFalse;
                btnExportOperations.Enabled = pTrueFalse;
                btnImportOperations.Enabled = pTrueFalse;
                txtConnectionString.Enabled = pTrueFalse;
                txtConnectionStringDst.Enabled = pTrueFalse;
                panel1.Enabled = pTrueFalse;
                panel2.Enabled = pTrueFalse;
                panel3.Enabled = pTrueFalse;
                panel4.Enabled = pTrueFalse;
                panel5.Enabled = pTrueFalse;
                panel6.Enabled = pTrueFalse;
                panel7.Enabled = pTrueFalse;
                panel9.Enabled = pTrueFalse;
                panel8.Enabled = pTrueFalse;
                panel10.Enabled = pTrueFalse;
                panel14.Enabled = pTrueFalse;
                panel4.Enabled = pTrueFalse;
                panel5.Enabled = pTrueFalse;
                panel8.Enabled = pTrueFalse;
                panel9.Enabled = pTrueFalse;
                panel10.Enabled = pTrueFalse;
                cmbBoxServerDst.Enabled = pTrueFalse;
                cmbBoxServerSrc.Enabled = pTrueFalse;
            }
        }

        private string ConvertPathToUncPath(string pFolderPath)
        {
            //this will convert normal local path to unc path
            try
            {
                string driveInfo = Path.GetPathRoot(pFolderPath);
                string driveLetter = driveInfo.Substring(0, 1);
                string pathWithoutDrive = pFolderPath.Substring(3, pFolderPath.Length - 3);

                return driveLetter + "$\\" + pathWithoutDrive;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return "";
        }

        private void MoveWebAppFolders(string pFileName, string pWebSite)
        {
            string webAppConfigFilePath = pFileName;
            string srcDriveLetter = string.Empty;


            try
            {

                XPathDocument doc = new XPathDocument(webAppConfigFilePath);
                XPathNavigator nav = doc.CreateNavigator();

                // Compile a standard XPath expression
                var expr = nav.Compile("//@physicalPath");
                XPathNodeIterator iterator = nav.Select(expr);

                //string[] srcServer = lblServers.Text.Split(chrSep);
                string strSrc;
                string folderPath;
                string driveLetter;
                string pathWithoutDrive;
                string driveInfo;
                string commandArguments;
                string strDst;
                if (_strToolMode == "Migrate" && _strExport == _strPerformOperationNo
                ) // migration = yes, and import = yes
                {
                    string scrFileName;
                    if (pFileName.Contains("WebSite"))
                    {
                        // Reading the Path From Source.xml
                        scrFileName = pFileName.Substring(0, pFileName.LastIndexOf('_')) + "_ToExport.xml";
                        XElement webRoot = XElement.Load(scrFileName);
                        XNamespace ns = webRoot.GetDefaultNamespace();
                        var webResult = from ele in webRoot.Elements(ns + "SITE").Elements(ns + "site")
                                .Elements(ns + "application")
                            select ele;
                        foreach (string srcDriveInfo in from element in webResult
                            where element.Attribute("path").Value == "/"
                            select element.Element("virtualDirectory").Attribute("physicalPath").Value
                            into srcFolderPath
                            select Path.GetPathRoot(srcFolderPath))
                        {
                            srcDriveLetter = srcDriveInfo.Substring(0, 1);
                        }
                        //XPathDocument srcDoc = new XPathDocument(scrFileName);
                        //XPathNavigator srcNav = srcDoc.CreateNavigator();
                        //XPathExpression srcExpr;
                        //srcExpr = srcNav.Compile("//@physicalPath");
                        //XPathNodeIterator srciterator = srcNav.Select(srcExpr);
                        //while (srciterator.MoveNext())
                        //{
                        //    string srcFolderPath = srciterator.Current.InnerXml;
                        //    string srcDriveInfo = Path.GetPathRoot(srcFolderPath);
                        //    srcDriveLetter = srcDriveInfo.Substring(0, 1);
                        //}
                        LogInfo("Copying Virtual directories.");
                        while (iterator.MoveNext())
                        {
                            folderPath = iterator.Current.InnerXml;
                            driveInfo = Path.GetPathRoot(folderPath);
                            driveLetter = driveInfo.Substring(0, 1);
                            pathWithoutDrive = folderPath.Substring(3, folderPath.Length - 3);
                            strSrc = "\\\\" + _strSrcNode + "\\" + srcDriveLetter + "$\\" + pathWithoutDrive;
                            strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;

                            LogInfo("Copy started from: " + strSrc + " To " + strDst);

                            commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst +
                                               "\" " + "/E /COPYALL /R:1";
                            int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                            if (returnCode >= _strRoboCopySuccessCode)
                                LogShortErrorMsg("Failed: Importing Virtual Directory.");
                        }
                        LogShortSuccessMsg("Success: Imported Virtual Directory.");
                    }
                    if (pFileName.Contains("WebApps"))
                    {
                        // Reading the Path From Source.xml
                        scrFileName = pFileName.Substring(0, pFileName.LastIndexOf('_')) + "_ToExport.xml";
                        List<string> webAppArray = new List<string>();
                        XElement root = XElement.Load(scrFileName);
                        XNamespace ns = root.GetDefaultNamespace();


                        var result = from ele in root.Elements(ns + "APP")
                            select ele;
                        string physicalPath;
                        string webAppName;
                        foreach (var element in result.Where(element => element.Attribute("path").Value != "/"))
                        {
                            webAppName = element.Attribute("APP.NAME").Value;
                            physicalPath = element.Element(ns + "application").Element("virtualDirectory")
                                .Attribute("physicalPath").Value;
                            webAppArray.Add(webAppName + "_" + physicalPath);
                        }
                        XElement importRoot = XElement.Load(webAppConfigFilePath);
                        XNamespace importns = importRoot.GetDefaultNamespace();
                        result = from ele in importRoot.Elements(importns + "APP")
                            select ele;
                        LogInfo("Copying Virtual directories.");
                        foreach (var element in result)
                        {
                            webAppName = element.Attribute("APP.NAME").Value;
                            foreach (string srcWebApp in webAppArray.Where(srcWebApp => webAppName == srcWebApp.Split('_')[0]))
                            {
                                driveInfo = Path.GetPathRoot(srcWebApp.Split('_')[1]);
                                srcDriveLetter = driveInfo.Substring(0, 1);
                            }
                            physicalPath = element.Element(ns + "application").Element("virtualDirectory")
                                .Attribute("physicalPath").Value;
                            driveInfo = Path.GetPathRoot(physicalPath);
                            driveLetter = driveInfo.Substring(0, 1);
                            pathWithoutDrive = physicalPath.Substring(3, physicalPath.Length - 3);
                            strSrc = "\\\\" + _strSrcNode + "\\" + srcDriveLetter + "$\\" + pathWithoutDrive;
                            strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;

                            LogInfo("Copy started from: " + strSrc + " To " + strDst);

                            commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst +
                                               "\" " + "/E /COPYALL /R:1";
                            int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                            if (returnCode >= _strRoboCopySuccessCode)
                                LogShortErrorMsg("Failed: Importing Virtual Directory.");

                        }
                        LogShortSuccessMsg("Success: Imported Virtual Directory.");
                    }
                }

                if (_strToolMode == "Backup" && _strExport == _strPerformOperationNo
                ) // migration = no, and import = yes
                {
                    LogInfo("Copying Virtual directories.");
                    while (iterator.MoveNext())
                    {
                        folderPath = iterator.Current.InnerXml;
                        driveInfo = Path.GetPathRoot(folderPath);
                        driveLetter = driveInfo.Substring(0, 1);
                        pathWithoutDrive = folderPath.Substring(3, folderPath.Length - 3);
                        strSrc = _vDir + "\\" + pWebSite + "\\" + folderPath.Substring(folderPath.LastIndexOf('\\'));
                        strDst = "\\\\" + _strDstNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;

                        LogInfo("Copy started from: " + strSrc + " To " + strDst);

                        commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/E /COPYALL /R:1";
                        int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode >= _strRoboCopySuccessCode)
                            LogShortErrorMsg("Failed: Importing Virtual Directory.");
                    }
                    LogShortSuccessMsg("Success: Imported Virtual Directory.");
                }

                if (_strToolMode == "Backup" && _strExport == _strPerformOperationYes
                ) // staging = yes, and export = yes  
                {
                    LogInfo("Copying Virtual directories.");
                    while (iterator.MoveNext())
                    {
                        folderPath = iterator.Current.InnerXml;
                        driveInfo = Path.GetPathRoot(folderPath);
                        driveLetter = driveInfo.Substring(0, 1);
                        pathWithoutDrive = folderPath.Substring(3, folderPath.Length - 3);
                        strSrc = "\\\\" + _strSrcNode + "\\" + driveLetter + "$\\" + pathWithoutDrive;
                        strDst = _vDir + "\\" + pWebSite + "\\" + folderPath.Substring(folderPath.LastIndexOf('\\'));

                        LogInfo("Copy started from: " + strSrc + " To " + strDst);

                        commandArguments = @"/C robocopy /xc /xn /xo " + "\"" + strSrc + "\"" + " \"" + strDst + "\" " +
                                           "/E /COPYALL /R:1";
                        int returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                        if (returnCode >= _strRoboCopySuccessCode)
                            LogShortErrorMsg("Failed: Exporting Virtual Directory.");
                    }
                    LogShortSuccessMsg("Success: Exported Virtual Directory to local.");
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

        }

        private int ExecuteCmd(string cmdName, string cmdArg)
        {
            string commandArguments = cmdArg;
            using (var p = new Process())
            {
                try
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = cmdName;
                    p.StartInfo.Arguments = commandArguments;
                    p.StartInfo.RedirectStandardError = true;
                    p.ErrorDataReceived += p_ErrorDataReceived;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.OutputDataReceived += p_OutputDataReceived;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    //Write result
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                    return p.ExitCode;

                    //return strSuccess;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }

            return 1;
        }

        private void SaveSrcSqlConnection()
        {
            try
            {
                Servers srv = File.Exists(_serverXmlPath) ? DeserializeObject<Servers>(_serverXmlPath) : new Servers();

                srv.SrcSqlInstance = txtConnectionString.Text.Trim();
                string strNodes = "";
                for (int i = 0; i < cmbBoxServerSrc.Items.Count; i++)
                {
                    if (i > 0)
                        strNodes = strNodes + ",";

                    strNodes = strNodes + cmbBoxServerSrc.Items[i];
                }
                srv.SrcNodes = strNodes;

                SerializeObject(srv, _serverXmlPath);
            }

            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while writing / reading server xml file.  " + ex.Message);

            }
        }

        private void SaveDstSqlConnection()
        {
            try
            {
                Servers srv = File.Exists(_serverXmlPath) ? DeserializeObject<Servers>(_serverXmlPath) : new Servers();

                srv.DstSqlInstance = txtConnectionStringDst.Text.Trim();
                string strNodes = "";
                for (int i = 0; i < cmbBoxServerDst.Items.Count; i++)
                {
                    if (i > 0)
                        strNodes = strNodes + ",";

                    strNodes = strNodes + cmbBoxServerDst.Items[i];
                }
                srv.DstNodes = strNodes;
                SerializeObject(srv, _serverXmlPath);
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while writing / reading server xml file.  " + ex.Message);

            }
        }

        private void SaveAppServers()
        {
            try
            {
                Servers srv = File.Exists(_serverXmlPath) ? DeserializeObject<Servers>(_serverXmlPath) : new Servers();


                srv.DstAppNode = txtConnectionStringDst.Text.Trim();
                srv.SrcAppNode = txtConnectionString.Text.Trim();
                SerializeObject(srv, _serverXmlPath);
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while writing / reading server xml file.  " + ex.Message);

            }
        }

        private void TstDstSqlConnection(bool pSaveConnectionInfo)
        {
            try
            {
                if (_strServerType == "BizTalk")
                {
                    if (txtConnectionStringDst.Text != "SQL INSTANCE" && txtConnectionStringDst.Text != "" &&
                        txtConnectionStringDst.Text != _dstSqlInstance)
                    {
                        EnableControls(false);
                        cmbBoxServerDst.Items.Clear();
                        cmbBoxServerDst.Text = "";
                        LogInfo("Validating Sql Instance.");
                        using (var sqlCon = new SqlConnection("Server=" + txtConnectionStringDst.Text.Trim() +
                                                              ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI")
                        )
                        {
                            sqlCon.Open();

                            using (var sqlcmd = new SqlCommand("SELECT [Id],[Name] FROM [adm_Server]", sqlCon))
                            {
                                using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                                {
                                    while (sqlRed.Read())
                                    {
                                        cmbBoxServerDst.Items.Add(sqlRed.GetString(1).ToUpper());
                                    }
                                }
                            }
                        }
                        cmbBoxServerDst.Visible = true;
                        LogShortSuccessMsg("Connection Verified.");

                        _dstSqlInstance = txtConnectionStringDst.Text;
                        if (pSaveConnectionInfo)
                            SaveDstSqlConnection();
                    }
                    else
                    {
                        if (txtConnectionStringDst.Text == "SQL INSTANCE" || txtConnectionStringDst.Text == "")
                        {
                            _dstSqlInstance = txtConnectionStringDst.Text;
                            cmbBoxServerDst.Items.Clear();
                            cmbBoxServerDst.Text = "";
                            SaveDstSqlConnection();
                        }
                    }

                    EnableControls(true);
                }
                else //App
                {
                    if (txtConnectionStringDst.Text != "SQL INSTANCE" && txtConnectionStringDst.Text != "" &&
                        txtConnectionStringDst.Text != "SERVER NAME")
                    {
                        SaveAppServers();
                        _strDstNode = txtConnectionStringDst.Text.Trim();
                    }
                    else
                    {
                        _strDstNode = string.Empty;
                        SaveAppServers();
                    }
                }

            }
            catch (Exception ex)
            {
                EnableControls(true);
                _dstSqlInstance = txtConnectionStringDst.Text;
                cmbBoxServerDst.Items.Clear();
                cmbBoxServerDst.Text = "";
                LogShortErrorMsg(ex.Message);
            }
        }

        private void TstSrcSqlConnection(bool pSaveConnectionInfo)
        {
            string serverName = "";

            try
            {
                if (_strServerType == "BizTalk") //BizTalk
                {
                    if (txtConnectionString.Text != "SQL INSTANCE" && txtConnectionString.Text != "" &&
                        txtConnectionString.Text != _srcSqlInstance)
                    {
                        EnableControls(false);
                        cmbBoxServerSrc.Items.Clear();
                        cmbBoxServerSrc.Text = "";

                        LogInfo("Validating Sql Instance.");
                        using (var sqlCon = new SqlConnection("Server=" + txtConnectionString.Text.Trim() +
                                                              ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI")
                        )
                        {
                            sqlCon.Open();

                            using (var sqlcmd = new SqlCommand("SELECT [Id],[Name] FROM [adm_Server]", sqlCon))
                            {
                                using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                                {
                                    while (sqlRed.Read())
                                    {
                                        cmbBoxServerSrc.Items.Add(sqlRed.GetString(1).ToUpper());
                                        if (serverName == "")
                                            serverName = sqlRed.GetString(1).ToUpper();
                                        else
                                            serverName = serverName + "," + sqlRed.GetString(1).ToUpper();
                                    }
                                }
                            }
                        }
                        cmbBoxServerSrc.Visible = true;
                        LogShortSuccessMsg("Connection Verified.");

                        _srcSqlInstance = txtConnectionString.Text;
                        if (pSaveConnectionInfo)
                            SaveSrcSqlConnection();
                    }
                    else
                    {
                        _srcSqlInstance = txtConnectionString.Text;
                        if (txtConnectionString.Text == "SQL INSTANCE" || txtConnectionString.Text == "")
                        {
                            cmbBoxServerSrc.Items.Clear();
                            cmbBoxServerSrc.Text = "";
                            SaveSrcSqlConnection();
                        }

                    }
                    EnableControls(true);
                }
                else //App
                {
                    if (txtConnectionString.Text != "SQL INSTANCE" && txtConnectionString.Text != "" &&
                        txtConnectionString.Text != "SERVER NAME")
                    {
                        SaveAppServers();
                        _strSrcNode = txtConnectionString.Text.Trim();
                    }
                    else
                    {
                        _strSrcNode = string.Empty;
                        SaveAppServers();
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(true);
                _srcSqlInstance = txtConnectionString.Text;
                cmbBoxServerSrc.Items.Clear();
                cmbBoxServerSrc.Text = "";
                LogShortErrorMsg(ex.Message);
            }
        }

        public void UpdateSettings()
        {
            string configPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFile = Path.Combine(configPath, "MigrationTool.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile};
            System.Configuration.Configuration config =
                ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            _remoteRootFolder = config.AppSettings.Settings["RemoteRootFolder"].Value;
            _bizTalkAppToIgnore = config.AppSettings.Settings["BizTalkAppToIgnore"].Value;
            _baseBizTalkAppCol = config.AppSettings.Settings["AppToRefer"].Value;
            _strCertPass = config.AppSettings.Settings["CertPass"].Value;
            _strFoldersToCopy = config.AppSettings.Settings["FoldersToCopy"].Value;
            _strFoldersToCopyNoFiles = config.AppSettings.Settings["FoldersToCopyNoFiles"].Value;
            _strCustomDllToInclude = config.AppSettings.Settings["CustomDllToInclude"].Value;
            _strWindowsServiceToIgnore = config.AppSettings.Settings["WindowsServiceToIgnore"].Value;
            _strWebsitesFolder = config.AppSettings.Settings["WebSitesDriveDestination"].Value;
            _strFoldersDrive = config.AppSettings.Settings["FoldersDriveDestination"].Value;
            _strServicesDrive = config.AppSettings.Settings["ServicesDriveDestination"].Value;

        }

        private void ExportBrePolicyVocabulary()
        {
            try
            {
                //Getting the List of Policies associated to Application
                string[] arrBrePolicies;
                using (var sqlCon = new SqlConnection("Server=" + txtConnectionString.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    sqlCon.Open();
                    string sqlQuery =
                        "Select b.nvcName As ApplicationName,a.sdmType,a.luid,a.properties,a.files From adpl_sat AS a,bts_application AS b where a.sdmType='System.BizTalk:Rules' and b.nID= a.applicationId";
                    using (var sqlDataAd = new SqlDataAdapter(sqlQuery, sqlCon))
                    {
                        using (var ds = new DataSet())
                        {
                            sqlDataAd.Fill(ds);
                            arrBrePolicies = ds.Tables[0].Rows.Cast<DataRow>()
                                .Select(row => row.ItemArray[2].ToString().Split('/')[1] + "." +
                                               row.ItemArray[2].ToString().Split('/')[2]
                                                   .Split('.')[0] + "." + row.ItemArray[2].ToString().Split('/')[2]
                                                   .Split('.')[1]).ToArray();
                        }
                    }

                    //Creating BRERuleEngineDb Connection
                    using (var sqlcmd = new SqlCommand("SELECT [RuleEngineDBServerName] FROM [adm_Group]", sqlCon))
                    {
                        using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (sqlRed.Read())
                            {
                                _srcBRESqlInstance = sqlRed.GetString(0);
                            }
                        }
                    }
                }
                //Creating Business RuleEngineDB COnnection
                SqlConnectionStringBuilder sqlBRE = new SqlConnectionStringBuilder(
                    "Server = " + _srcBRESqlInstance +
                    "; Initial Catalog = BizTalkRuleEngineDb; Integrated Security = SSPI");
                SqlRuleStore sqlRulesStore = new SqlRuleStore(sqlBRE.ConnectionString);
                RuleSetDeploymentDriver rulesetDepDriver =
                    new RuleSetDeploymentDriver(sqlBRE.DataSource, "BizTalkRuleEngineDb");
                RuleSetInfoCollection rulesetinfos = sqlRulesStore.GetRuleSets(RuleStore.Filter.All);
                // Creating Folders to Export Polices and Vocabualries

                foreach (RuleSetInfo ruleSetInfo in rulesetinfos)
                {
                    string policy = String.Format("{0}.{1}.{2}", ruleSetInfo.Name, ruleSetInfo.MajorRevision,
                        ruleSetInfo.MinorRevision);

                    if (arrBrePolicies.Contains(policy))
                    {
                        //Do Nothing as Polices will be Exported By MSI
                    }
                    else
                    {
                        try
                        {
                            try
                            {
                                //Exporting Policy
                                var policyName = String.Format("{0}{1}.{2}.{3}.xml", "Policy_", ruleSetInfo.Name,
                                    ruleSetInfo.MajorRevision, ruleSetInfo.MinorRevision);
                                rulesetDepDriver.ExportRuleSetToFileRuleStore(ruleSetInfo, _brePath + "/" + policyName);
                                LogInfoInLogFile(ruleSetInfo.Name + "Policy Exported");
                            }
                            catch (Exception ex)
                            {
                                LogShortErrorMsg("Exception Occured while Exporting " + ruleSetInfo.Name +
                                                 "please check log file for details.");
                                LogInfoSyncronously("Exception while Exporting Policy " + ex.Message);

                            }
                            //Exporting vocabularyAssocated to Policy

                            string vocabQuery =
                                "select A.nRuleSetID,A.strName as policyName,B.nVocabularyID,C.nVocabularyID,C.strName as VocabularyName from re_ruleset(nolock) as A,re_ruleset_to_vocabulary_links As B,re_vocabulary As C where A.nRuleSetID = B.nReferingRuleset and B.nVocabularyID = C.nVocabularyID and C.strName not in('Predicates','Functions','Common Values','Common Sets') and A.strName = @strPolicyName";
                            using (var sqlVocabad = new SqlDataAdapter(vocabQuery, sqlBRE.ConnectionString))
                            {
                                sqlVocabad.SelectCommand.Parameters.AddWithValue("@strPolicyName", ruleSetInfo.Name);
                                using (var dsVocab = new DataSet())
                                {
                                    sqlVocabad.Fill(dsVocab);
                                    VocabularyInfoCollection vocabularyInfos =
                                        sqlRulesStore.GetVocabularies(RuleStore.Filter.All);
                                    foreach (DataRow dr in dsVocab.Tables[0].Rows)
                                    {
                                        string vocabularyName = dr["VocabularyName"].ToString();
                                        foreach (VocabularyInfo vocabularyInfo in vocabularyInfos)
                                        {
                                            try
                                            {
                                                if (vocabularyName == vocabularyInfo.Name)
                                                {
                                                    var vocabularyFileName = String.Format("{0}{1}.{2}.{3}.xml",
                                                        "Vocabualary_", vocabularyInfo.Name,
                                                        vocabularyInfo.MajorRevision, vocabularyInfo.MinorRevision);
                                                    rulesetDepDriver.ExportVocabularyToFileRuleStore(vocabularyInfo,
                                                        _brePath + "/" + vocabularyFileName);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogShortErrorMsg(
                                                    "Exception Occured while Exporting " + vocabularyInfo.Name +
                                                    "please check log file for details.");
                                                LogInfoSyncronously(
                                                    "Exception while Exporting Vocabulary " + ex.Message);

                                            }

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogShortErrorMsg(
                                "Exception occured while Exporting Policy or Vocabualry, please check log file for details.");
                            LogException(ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogShortErrorMsg("Exception Occured while Exporting BREPolicyVocabulary");
                LogInfoSyncronously("Exception Occured while Exporting BREPolicyVocabulary " + ex.Message);
                throw;
            }

        }

        private void ImportBrePolicyVocabulary()
        {
            try
            {
                using (var sqlCon = new SqlConnection("Server=" + txtConnectionStringDst.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                {
                    sqlCon.Open();
                    using (var sqlcmd = new SqlCommand("SELECT [RuleEngineDBServerName] FROM [adm_Group]", sqlCon))
                    {
                        using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (sqlRed.Read())
                            {
                                _dstBRESqlInstance = sqlRed.GetString(0);
                            }
                        }
                    }
                }
                SqlConnectionStringBuilder sqlBRE = new SqlConnectionStringBuilder(
                    "Server = " + _dstBRESqlInstance +
                    "; Initial Catalog = BizTalkRuleEngineDb; Integrated Security = SSPI");
                SqlRuleStore sqlRulesStore = new SqlRuleStore(sqlBRE.ConnectionString);
                RuleSetDeploymentDriver rulesetDepDriver =
                    new RuleSetDeploymentDriver(sqlBRE.DataSource, "BizTalkRuleEngineDb");
                RuleSetInfoCollection dstrulesetinfos = sqlRulesStore.GetRuleSets(RuleStore.Filter.All);
                VocabularyInfoCollection dstvocabularyInfos = sqlRulesStore.GetVocabularies(RuleStore.Filter.All);

                //Importing Vocabualries
                var files = Directory.GetFiles(_brePath, "Vocabualary*.xml");
                if (files.Length != 0)
                {
                    var vocabualrySet = dstvocabularyInfos.Cast<VocabularyInfo>()
                        .Select(dstvocabularyInfo => String.Format("{0}{1}.{2}.{3}.xml", "Vocabualary_",
                            dstvocabularyInfo.Name, dstvocabularyInfo.MajorRevision, dstvocabularyInfo.MinorRevision)).ToArray();
                    //Creating a Set of Vocabularies which are Present in DstSqlInstance


                    foreach (string file in files)
                    {
                        string filename = Path.GetFileName(file);
                        if (vocabualrySet.Contains(filename))
                        {
                            LogInfoInLogFile(filename + " Already Published");
                        }
                        else
                        {
                            FileRuleStore fileRuleStore = new FileRuleStore(file);
                            VocabularyInfoCollection vocabularyInfoList =
                                fileRuleStore.GetVocabularies(RuleStore.Filter.All);


                            foreach (VocabularyInfo vocabularyInfo in vocabularyInfoList)

                            {
                                //Checking Whether its Present in DstSQLInstance

                                try
                                {
                                    VocabularyInfo vi = new VocabularyInfo(vocabularyInfo.Name,
                                        vocabularyInfo.MajorRevision, vocabularyInfo.MinorRevision);
                                    Vocabulary oVoc = fileRuleStore.GetVocabulary(vi);
                                    sqlRulesStore.Add(oVoc, true);
                                    LogInfoInLogFile(filename + " Imported");
                                }
                                catch (Exception ex)
                                {
                                    LogShortErrorMsg("Failed:Importing " + filename);
                                    LogInfoSyncronously("Exception while Importing " + filename + ":" + ex.Message);

                                }

                            }
                        }

                    }
                }
                //Importing Policies
                files = Directory.GetFiles(_brePath, "Policy*.xml");
                if (files.Length != 0)
                {
                    var policySet = dstrulesetinfos.Cast<RuleSetInfo>()
                        .Select(dstruleSetInfo => String.Format("{0}{1}.{2}.{3}.xml", "Policy_", dstruleSetInfo.Name,
                            dstruleSetInfo.MajorRevision, dstruleSetInfo.MinorRevision)).ToArray();
                    //Creating Policies Present in DestSQLInstance
                    foreach (string file in files)
                    {
                        string filename = Path.GetFileName(file);

                        if (policySet.Contains(filename))
                        {
                            LogInfoInLogFile(filename + " Already Published");
                        }
                        else
                        {
                            try
                            {
                                rulesetDepDriver.ImportAndPublishFileRuleStore(file);
                                LogInfoInLogFile(filename + " Imported");
                            }
                            catch (Exception ex)
                            {
                                LogShortErrorMsg("Failed:Importing  " + filename);
                                LogInfoSyncronously("Exception while Importing " + filename + ":" + ex.Message);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogShortErrorMsg("Failed:Importing BREPolicyVocabulary.");
                LogInfoSyncronously("Exception Occured while Importing BREPolicyVocabulary " + ex.Message);
                throw;
            }

        }

        private void ExportClientCertOnetOneMappings()
        {
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    foreach (Site site in serverManager.Sites)
                    {
                        try
                        {
                            Microsoft.Web.Administration.Configuration config =
                                serverManager.GetApplicationHostConfiguration();
                            Microsoft.Web.Administration.ConfigurationSection
                                iisClientCertificateMappingAuthenticationSection = config.GetSection(
                                    "system.webServer/security/authentication/iisClientCertificateMappingAuthentication",
                                    site.Name);
                            Microsoft.Web.Administration.ConfigurationElementCollection oneToOneMappingsCollection =
                                iisClientCertificateMappingAuthenticationSection.GetCollection("oneToOneMappings");
                            //Checking whether OneToOneCertifcationMappings are Present
                            if (oneToOneMappingsCollection.Count == 0)
                            {
                                LogInfoInLogFile(
                                    site.Name + " Website:Does not Contain OneToOneCLientCertificateMappings");
                            }
                            else
                            {
                                using (XmlWriter writer =
                                    XmlWriter.Create(_xmlPath + @"\" + site.Name + "_ClientCertMappings.xml"))
                                {
                                    writer.WriteStartElement("OneToOneMappings");
                                    foreach (var onetoone in oneToOneMappingsCollection)
                                    {
                                        writer.WriteStartElement("OneToOneMapping");
                                        writer.WriteElementString("enabled",
                                            onetoone.GetAttributeValue("enabled").ToString());
                                        writer.WriteElementString("userName",
                                            onetoone.GetAttributeValue("userName").ToString());
                                        writer.WriteElementString("password",
                                            Encrypt(onetoone.GetAttributeValue("password").ToString()));
                                        writer.WriteElementString("certificate",
                                            onetoone.GetAttributeValue("certificate").ToString());
                                        writer.WriteEndElement();

                                    }
                                    writer.WriteEndElement();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogShortErrorMsg("Exception Occured for " + site.Name +
                                             ": please check log file for details.");
                            LogInfoSyncronously("Exception while Exporting IISClientCertificateOneToOneMappings for " +
                                                site.Name + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Exporting IISClientCertificateOneToOneMappings  " + ex.Message);
                throw;
            }
        }

        private void ImportClientCertOnetOneMappings()
        {
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    foreach (Site site in serverManager.Sites)
                    {
                        try
                        {
                            string websiteMappingFile = site.Name + "_ClientCertMappings.xml";
                            DirectoryInfo di = new DirectoryInfo(_xmlPath);
                            FileInfo[] files = di.GetFiles(websiteMappingFile);
                            if (files.Length == 0)
                            {
                                LogInfoInLogFile(site + "_ClientCertMappings are not Present");
                            }
                            else
                            {
                                XmlDocument xmldoc = new XmlDocument();
                                xmldoc.Load(_xmlPath + @"\" + websiteMappingFile);
                                XmlNodeList nodeList =
                                    xmldoc.DocumentElement.SelectNodes("/OneToOneMappings/OneToOneMapping");
                                foreach (XmlNode node in nodeList)
                                {
                                    Microsoft.Web.Administration.Configuration config =
                                        serverManager.GetApplicationHostConfiguration();

                                    Microsoft.Web.Administration.ConfigurationSection
                                        iisClientCertificateMappingAuthenticationSection =
                                            config.GetSection(
                                                "system.webServer/security/authentication/iisClientCertificateMappingAuthentication",
                                                site.Name);
                                    iisClientCertificateMappingAuthenticationSection["enabled"] = true;
                                    iisClientCertificateMappingAuthenticationSection[
                                        "oneToOneCertificateMappingsEnabled"] = true;
                                    Microsoft.Web.Administration.ConfigurationElementCollection
                                        oneToOneMappingsCollection =
                                            iisClientCertificateMappingAuthenticationSection.GetCollection(
                                                "oneToOneMappings");
                                    var certificatearray = oneToOneMappingsCollection.Select(onetoone =>
                                        onetoone.GetAttributeValue("certificate").ToString()).ToArray();
                                    //Getting All the Certifcates which is already Present in Destination System
                                    if (certificatearray.Contains(node.SelectSingleNode("certificate").InnerText))
                                    {
                                        LogInfoInLogFile(node.SelectSingleNode("certificate").InnerText +
                                                         " already Exsists in Website: " + site.Name);
                                    }
                                    else
                                    {
                                        Microsoft.Web.Administration.ConfigurationElement addElement =
                                            oneToOneMappingsCollection.CreateElement("add");
                                        addElement["enabled"] =
                                            Convert.ToBoolean(node.SelectSingleNode("enabled").InnerText);
                                        addElement["userName"] = node.SelectSingleNode("userName").InnerText;
                                        addElement["password"] = Decrypt(node.SelectSingleNode("password").InnerText);
                                        addElement["certificate"] = node.SelectSingleNode("certificate").InnerText;
                                        oneToOneMappingsCollection.Add(addElement);
                                        Microsoft.Web.Administration.ConfigurationSection accessSection =
                                            config.GetSection("system.webServer/security/access", site.Name);
                                        accessSection["sslFlags"] = @"Ssl, SslNegotiateCert";
                                        LogInfoInLogFile(node.SelectSingleNode("certificate").InnerText + " added in" +
                                                         site.Name);
                                    }
                                }
                                serverManager.CommitChanges();
                                LogInfoInLogFile(site.Name + " OneToOneClientCert Mappings Completed");

                            }
                        }
                        catch (Exception ex)
                        {
                            LogShortErrorMsg("Exception Occured for " + site.Name +
                                             ": please check log file for details.");
                            LogInfoSyncronously("Exception while Importing IISClientCertificateOneToOneMappings for " +
                                                site.Name + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Importing IISClientCertificateOneToOneMappings  " + ex.Message);
                throw;
            }
        }

        private string Encrypt(string data)
        {
            try
            {
                var keyArray = Encoding.UTF8.GetBytes("M!grat!onkey1234");
                using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
                {
                    des.Mode = CipherMode.ECB;
                    des.Key = keyArray;
                    des.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform desEncrypt = des.CreateEncryptor())
                    {
                        Byte[] buffer = Encoding.ASCII.GetBytes(data);

                        return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Encrypting Credentials  " + ex.Message);
                throw;
            }

        }

        private string Decrypt(string data)
        {
            try
            {
                var keyArray = Encoding.UTF8.GetBytes("M!grat!onkey1234");
                using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
                {
                    des.Mode = CipherMode.ECB;
                    des.Key = keyArray;
                    des.Padding = PaddingMode.PKCS7;
                    using (var desEncrypt = des.CreateDecryptor())
                    {
                        Byte[] buffer = Convert.FromBase64String(data.Replace(" ", "+"));

                        return Encoding.UTF8.GetString(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Decrypting Credentials " + ex.Message);
                throw;
            }

        }

        private void ExportHostMapSettings()
        {
            try
            {
                using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                {
                    btsExp.ConnectionString = "Server=" + txtConnectionString.Text.Trim() +
                                              ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                    var hosts = btsExp.Hosts;
                    foreach (object cmbBox in cmbBoxServerSrc.Items)
                    {
                        using (XmlWriter writer =
                            XmlWriter.Create(_xmlPath + @"\" + "Src_" + cmbBox + "_HostMappings.xml"))
                        {
                            writer.WriteStartElement("SettingsMap");
                            writer.WriteStartElement("HostMappings");
                            foreach (Host host in hosts)
                            {
                                writer.WriteStartElement("SourceHost");
                                writer.WriteAttributeString("Name", host.Name);
                                writer.WriteElementString("DestinationHosts", host.Name);
                                writer.WriteEndElement();

                            }
                            writer.WriteEndElement();

                            //  Get all the HostInstances of the Destination Server

                            var hostInstancesArray = HostInstance.GetInstances()
                                .Where(ht =>
                                    ht.Name.EndsWith(cmbBox.ToString()) ||
                                    ht.Name.EndsWith(cmbBox.ToString().ToLower()))
                                .Select(ht => ht.Name.Split(' ')[3])
                                .Where(x => !string.IsNullOrEmpty(x));

                            writer.WriteStartElement("HostInstanceMappings");
                            foreach (string hostInstance in hostInstancesArray)
                            {
                                writer.WriteStartElement("SourceHostInstance");
                                writer.WriteAttributeString("Name", hostInstance + ":" + cmbBox);
                                writer.WriteElementString("DestinationHostInstances",
                                    hostInstance + ":" + "{ServerName}");
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Exporting Export HostMapping Settings  " + ex.Message);
                throw;
            }
        }

        private void ImportHostMapSettings()
        {

            try
            {
                LogInfo("Host Mappings: Creating Host Mappings.");
                XmlDocument xd = new XmlDocument();
                string[] srcservers;
                //Getting the List of Source Servers
                if (File.Exists(_xmlPath + "\\" + "SrcServers.xml"))
                {
                    xd.Load(_xmlPath + "\\" + "SrcServers.xml");
                    XmlNode srcnodeList = xd.DocumentElement.SelectSingleNode("/Servers");

                    string srcServerList = srcnodeList.SelectSingleNode("SrcNodes").InnerText;
                    srcservers = srcServerList.Split(',');
                }
                else
                {
                    throw new InvalidOperationException("SrcServers xml file is not available.");
                }
                //Getting the List of DestinationServers
                xd.Load(_xmlPath + "\\" + "Servers.xml");
                XmlNode dstnodeList = xd.DocumentElement.SelectSingleNode("/Servers");

                string dstServerList = dstnodeList.SelectSingleNode("DstNodes").InnerText;
                var dstservers = dstServerList.Split(',');

                String[] files = Directory.GetFiles(_xmlPath, "Src_*_HostMappings.xml");
                if (files.Length == 0)
                {
                    throw new FileNotFoundException(" Source HostMapping xml file is not available.", _xmlPath + "\\" + "Src_*_HostMappings.xml");
                }
                if (dstservers.Length == srcservers.Length)
                {
                    for (int i = 0; i < dstservers.Length; i++)
                    {
                        string srcHostMappingFile = _xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml";
                        string dstHostMappingFile = _xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                        // instantiate new instance of Explorer OM
                        string[] hostArray;
                        using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                        {
                            btsExp.ConnectionString = "Server=" + txtConnectionStringDst.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                            hostArray = btsExp.Hosts.Cast<Host>().Select(ht => ht.Name).ToArray();
                        }
                        //Get all the HostInstances of the Destination Server
                        var hostInstancesArray = HostInstance.GetInstances()
                            .Where(ht =>
                                ht.HostType == HostInstance.HostTypeValues.InProcess &&
                                (ht.Name.EndsWith(dstservers[i]) || ht.Name.EndsWith(dstservers[i].ToLower())))
                            .Select(ht => ht.Name.Split(' ')[3]).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        XDocument doc = XDocument.Load(srcHostMappingFile);
                        //Removing SourceHost Which are Not Present in Destination
                        var hostName = from node in doc.Descendants("SourceHost")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostArray.Contains(attr.Value)
                            select node;
                        hostName.ToList().ForEach(x => x.Remove());

                        //Removing SourceHostInstances Which are Not Present in DestinationHostInstances
                        var hostInstanceNameToRemove = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        hostInstanceNameToRemove.ToList().ForEach(x => x.Remove());

                        //Updating Host Instances of Destination with ComputerName
                        var hostInstanceName = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        foreach (XElement itemElement in hostInstanceName)
                        {

                            itemElement.Element("DestinationHostInstances").Value =
                                itemElement.Element("DestinationHostInstances").Value.Split(':')[0] + ":" +
                                dstservers[i];
                        }
                        doc.Save(dstHostMappingFile);
                    }
                }
                if (dstservers.Length < srcservers.Length)
                {
                    for (int i = 0; i < dstservers.Length; i++)
                    {
                        string srcHostMappingFile = _xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml";
                        string dstHostMappingFile = _xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                        // instantiate new instance of Explorer OM
                        string[] hostArray;
                        using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                        {
                            btsExp.ConnectionString = "Server=" + txtConnectionStringDst.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                            hostArray = btsExp.Hosts.Cast<Host>().Select(ht => ht.Name).ToArray();
                        }

                        //Get all the HostInstances of the Destination Server

                        var hostInstancesArray = HostInstance.GetInstances()
                            .Where(ht =>
                                ht.HostType == HostInstance.HostTypeValues.InProcess &&
                                (ht.Name.EndsWith(dstservers[i]) || ht.Name.EndsWith(dstservers[i].ToLower())))
                            .Select(ht => ht.Name.Split(' ')[3]).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        XDocument doc = XDocument.Load(srcHostMappingFile);
                        //Removing SourceHost Which are Not Present in Destination
                        var hostName = from node in doc.Descendants("SourceHost")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostArray.Contains(attr.Value)
                            select node;
                        hostName.ToList().ForEach(x => x.Remove());

                        //Removing SourceHostInstances Which are Not Present in DestinationHostInstances
                        var hostInstanceNameToRemove = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        hostInstanceNameToRemove.ToList().ForEach(x => x.Remove());

                        //Updating Host Instances of Destination wiht ComputerName
                        var hostInstanceName = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        foreach (XElement itemElement in hostInstanceName)
                        {

                            itemElement.Element("DestinationHostInstances").Value =
                                itemElement.Element("DestinationHostInstances").Value.Split(':')[0] + ":" +
                                dstservers[i];
                        }
                        doc.Save(dstHostMappingFile);
                    }
                }
                if (dstservers.Length > srcservers.Length)
                {
                    for (int i = 0; i < dstservers.Length; i++)
                    {
                        var srcHostMappingFile = i < srcservers.Length
                            ? _xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml"
                            : _xmlPath + @"\" + "Src_" + srcservers[0] + "_HostMappings.xml";
                        string dstHostMappingFile = _xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                        // instantiate new instance of Explorer OM
                        string[] hostArray;
                        using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                        {
                            btsExp.ConnectionString = "Server=" + txtConnectionStringDst.Text.Trim() +
                                                      ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                            hostArray = btsExp.Hosts.Cast<Host>().Select(ht => ht.Name).ToArray();
                        }

                        //Get all the HostInstances of the Destination Server

                        var hostInstancesArray = HostInstance.GetInstances()
                            .Where(ht =>
                                ht.HostType == HostInstance.HostTypeValues.InProcess &&
                                (ht.Name.EndsWith(dstservers[i]) || ht.Name.EndsWith(dstservers[i].ToLower())))
                            .Select(ht => ht.Name.Split(' ')[3]).Where(x => !string.IsNullOrEmpty(x)).ToArray();



                        XDocument doc = XDocument.Load(srcHostMappingFile);
                        //Removing SourceHost Which are Not Present in Destination
                        var hostName = from node in doc.Descendants("SourceHost")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostArray.Contains(attr.Value)
                            select node;
                        hostName.ToList().ForEach(x => x.Remove());

                        //Removing SourceHostInstances Which are Not Present in DestinationHostInstances
                        var hostInstanceNameToRemove = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && !hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        hostInstanceNameToRemove.ToList().ForEach(x => x.Remove());

                        //Updating Host Instances of Destination wiht ComputerName
                        var hostInstanceName = from node in doc.Descendants("SourceHostInstance")
                            let attr = node.Attribute("Name")
                            where attr != null && hostInstancesArray.Contains(attr.Value.Split(':')[0])
                            select node;
                        foreach (XElement itemElement in hostInstanceName)
                        {

                            itemElement.Element("DestinationHostInstances").Value =
                                itemElement.Element("DestinationHostInstances").Value.Split(':')[0] + ":" +
                                dstservers[i];
                        }
                        doc.Save(dstHostMappingFile);
                    }
                }

                LogInfo("Host Mappings: Created Host Mappings.");
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Importing HostMapping Settings  " + ex.Message);
                LogException(ex);
                throw;
            }
        }

        private void ExportSSOApps()
        {
            //Creating List of SSO Apps
            string fileName = @"C:\Program Files\Common Files\Enterprise Single Sign-On\ssomanage.exe";

            try
            {
                string commandArguments;
                if (_machineName == _strSrcNode) //local
                {
                    commandArguments = "/C " + "\"\"" + fileName + "\"" + " -listapps />\"" + _xmlPath +
                                       "\\SrcSSOAppsList.txt" + "\"";
                }
                else
                {
                    string xmlPathUnc = ConvertPathToUncPath(_xmlPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       " " +
                                       "\"" + fileName + "\"" + " -listapps />\"" + "\\\\" + _machineName + "\\" +
                                       xmlPathUnc + "\\SrcSSOAppsList.txt" + "\"";

                }

                var returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                if (returnCode == 0)
                {
                    LogShortSuccessMsg("Success: Exported SSO Affiliate Application List.");

                }
                else
                {
                    LogShortErrorMsg("Failed: Exporting SSO Affiliate Application List.");

                }
                if (File.Exists(_xmlPath + "\\" + "SrcSSOAppsList.txt"))
                {
                    string[] ssoAppsList = File.ReadAllLines(_xmlPath + @"\SrcSSOAppsList.txt");
                    foreach (string ssoApps in ssoAppsList.Where(ssoApps => !string.IsNullOrWhiteSpace(ssoApps) &&
                                                                            !ssoApps.Contains("Using SSO server") && 
                                                                            !ssoApps.Contains("Applications available for") &&
                                                                            !ssoApps.Contains("applications available for")))
                    {
                        try
                        {
                            string ssoApp = ssoApps.Split(':')[0].Split(')')[1].Trim();
                            //DisplayInforMation of SSOApp

                            if (_machineName == _strSrcNode) //local
                            {
                                commandArguments = "/C " + "\"\"" + fileName + "\"" + " -displayapp " + ssoApp +
                                                   " />\"" + _xmlPath + "\\SSOApp_" + ssoApp + ".txt" + "\"";
                            }
                            else
                            {
                                string xmlPathUnc = ConvertPathToUncPath(_xmlPath);
                                commandArguments =
                                    "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " +
                                    "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                    " -accepteula" + fileName + "\"" + " -displayapp " + ssoApp +
                                    " />\"" + "\\\\" + _machineName + "\\" + xmlPathUnc +
                                    "\\SSOApp_" + ssoApp + ".txt" + "\"";
                            }

                            returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                            if (returnCode == 0)
                            {
                                LogShortSuccessMsg("Success: Exported " + ssoApp +
                                                   " Affiliate Application Information.");
                            }
                            else
                            {
                                LogShortErrorMsg("Failed: Exporting " + ssoApp +
                                                 " Affiliate Application Information.");
                            }
                            //ListMappings
                            if (_machineName == _strSrcNode) //local
                            {
                                commandArguments = "/C " + "\"\"" + fileName + "\"" + " -listMappings " + ssoApp +
                                                   " />\"" + _xmlPath + "\\SSOMap_" + ssoApp + ".txt" + "\"";
                            }
                            else
                            {
                                string xmlPathUnc = ConvertPathToUncPath(_xmlPath);
                                commandArguments =
                                    "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strSrcNode + " -u " +
                                    "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                    fileName + "\"" + " -accepteula" + " -listMappings " + ssoApp +
                                    " />\"" + "\\\\" + _machineName + "\\" + xmlPathUnc +
                                    "\\SSOMap_" + ssoApp + ".txt" + "\"";
                            }

                            returnCode = ExecuteCmd("CMD.EXE", commandArguments);
                            if (returnCode == 0)
                            {
                                LogShortSuccessMsg("Success: Exported " + ssoApp + " Mapping File.");
                            }
                            else
                            {
                                LogShortErrorMsg("Failed: Exporting " + ssoApp + " Mapping File.");
                            }
                            //
                        }
                        catch (Exception ex)
                        {
                            LogInfoSyncronously("Exception while Exporting SSO Affiliate Application " +
                                                ex.Message);
                            LogException(ex);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("SSO Affiliate Application List is not Present");
                }


                string[] files = Directory.GetFiles(_xmlPath, "SSOApp_*.txt");
                if (files.Length == 0)
                {
                    throw new FileNotFoundException("SSO Information Files are not Present", _xmlPath + "\\" + "SSOApp_*.txt");
                }
                foreach (string file in files)
                {
                    try
                    {
                        string[] ssoAppInformation = File.ReadAllLines(file);
                        //Extarcting Inofrmation from TextFile
                        string applicationName = string.Empty;
                        string userId = string.Empty;
                        string password = string.Empty;
                        string groupApp = string.Empty;
                        string applicationEnabled = string.Empty;
                        string allowLocalAccounts = string.Empty;
                        string adminAccountSame = string.Empty;
                        string ticketsAllowed = string.Empty;
                        string validateTickets = string.Empty;
                        string timeoutTickets = string.Empty;

                        foreach (string line in ssoAppInformation)
                        {
                            if (line.Contains("Application name"))
                                applicationName = line.Split(':')[1].Trim();
                            if (line.Contains("User Id"))
                            {
                                userId = line.Split(':')[1].Trim() == "(Not Masked)" ? "no" : "yes";
                            }
                            if (line.Contains("Password"))
                            {
                                password = line.Split(':')[1].Trim() == "(Not Masked)" ? "no" : "yes";
                            }
                            if (line.Contains("Application type"))
                            {
                                groupApp = line.Split(':')[1].Trim() == "Group" ? "yes" : "no";
                            }
                            if (line.Contains("Application enabled"))
                                applicationEnabled = line.Split(':')[1].Trim();

                            if (line.Contains("Allow local accounts"))
                                allowLocalAccounts = line.Split(':')[1].Trim();
                            if (line.Contains("Admin account same"))
                                adminAccountSame = line.Split(':')[1].Trim();
                            if (line.Contains("tickets allowed"))
                                ticketsAllowed = line.Split(':')[1].Trim();
                            if (line.Contains("validate tickets"))
                                validateTickets = line.Split(':')[1].Trim();
                            if (line.Contains("timeout tickets"))
                                timeoutTickets = line.Split(':')[1].Trim();


                        }

                        string appName = Path.GetFileNameWithoutExtension(file).Split('_')[1];
                        //create SQL COnnection
                        string srcSSOSqlInstance;
                        using (var sqlCon = new SqlConnection(
                            "Server=" + txtConnectionString.Text.Trim() +
                            ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                        {
                            sqlCon.Open();
                            using (var sqlcmd = new SqlCommand(
                                "select distinct(ServerName) from [dbo].[adm_OtherDatabases] where DefaultDatabaseName = 'SSO' and ServerName not like '%.com%'",
                                sqlCon))
                            {
                                using (var sqlRed = sqlcmd.ExecuteReader())
                                {
                                    srcSSOSqlInstance = string.Empty;
                                    while (sqlRed.Read())
                                    {
                                        srcSSOSqlInstance = sqlRed.GetString(0);
                                    }
                                }
                            }
                        }
                        using (var sqlCon = new SqlConnection(
                            "Server=" + srcSSOSqlInstance +
                            ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                        {
                            sqlCon.Open();
                            string text =
                                "SELECT  [ai_app_name],[ai_description],[ai_contact_info],[ai_user_group_name],[ai_admin_group_name],[ai_flags],[ai_num_fields],[ai_purge_id],[ai_audit_id],[ai_ticket_timeout] FROM [SSODB].[dbo].[SSOX_ApplicationInfo] where [ai_app_name] = @AppName";
                            using (var sqlCmd = new SqlCommand(text, sqlCon))
                            {
                                sqlCmd.Parameters.AddWithValue("@AppName", appName);
                                using (var ds = new DataSet())
                                {
                                    using (var sqlDataAd = new SqlDataAdapter(sqlCmd))
                                    {
                                        sqlDataAd.Fill(ds);
                                    }

                                    using (XmlWriter writer =
                                        XmlWriter.Create(
                                            _xmlPath + @"\" + "SSOApp_" + appName + "_ToImport" + ".xml"))
                                    {
                                        writer.WriteStartElement("SSO");
                                        writer.WriteStartElement("application");
                                        writer.WriteAttributeString("name", applicationName);
                                        writer.WriteElementString("description",
                                            ds.Tables[0].Rows[0].ItemArray[1].ToString());
                                        writer.WriteElementString("Contact",
                                            ds.Tables[0].Rows[0].ItemArray[2].ToString());
                                        writer.WriteElementString("appUserAccount",
                                            ds.Tables[0].Rows[0].ItemArray[3].ToString());
                                        writer.WriteElementString("appAdminAccount",
                                            ds.Tables[0].Rows[0].ItemArray[4].ToString());
                                        //  writer.WriteElementString("ticketTimeout", ticketTimeOut);
                                        writer.WriteStartElement("field");
                                        writer.WriteAttributeString("ordinal", "0");
                                        writer.WriteAttributeString("label", "User Id");
                                        writer.WriteAttributeString("masked", userId);
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("field");
                                        writer.WriteAttributeString("ordinal", "1");
                                        writer.WriteAttributeString("label", "Password");
                                        writer.WriteAttributeString("masked", password);
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("flags");
                                        writer.WriteAttributeString("groupApp", groupApp);
                                        writer.WriteAttributeString("adminAccountSame", adminAccountSame.ToLower());
                                        writer.WriteAttributeString("allowLocalAccounts",
                                            allowLocalAccounts.ToLower());
                                        writer.WriteAttributeString("enableApp", applicationEnabled.ToLower());
                                        writer.WriteAttributeString("allowTickets", ticketsAllowed.ToLower());
                                        if (validateTickets != string.Empty)
                                            writer.WriteAttributeString("validateTickets",
                                                validateTickets.ToLower());
                                        if (timeoutTickets != string.Empty)
                                            writer.WriteAttributeString("timeoutTickets", timeoutTickets.ToLower());
                                        writer.WriteEndElement();
                                        writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogInfoSyncronously("Exception while Exporting SSO XMl: " + file + ex.Message);
                        LogException(ex);
                    }
                }
                LogShortSuccessMsg("Success: Exported SSO Affiliate Apps Xmls");
                files = Directory.GetFiles(_xmlPath, "SSOMap_*.txt");
                if (files.Length == 0)
                {
                    throw new FileNotFoundException("SSO Mapping Files are not Present", _xmlPath + "\\" + "SSOMap_*.txt");
                }
                foreach (string file in files)
                {
                    string[] ssoMappingList = File.ReadAllLines(file);
                    List<string> ssoMapping = new List<string>();
                    try
                    {
                        ssoMapping.AddRange(ssoMappingList.Where(ssoMap =>
                            !string.IsNullOrEmpty(ssoMap) && !string.IsNullOrWhiteSpace(ssoMap) &&
                            !ssoMap.Contains("Using SSO server") &&
                            !ssoMap.Contains("Existing mappings for application") &&
                            !ssoMap.Contains("existing mappings for application")));
                    }
                    catch (Exception ex)
                    {
                        LogInfoSyncronously("Exception while Reading  SSO Mapping File : " + file + ex.Message);
                        LogException(ex);
                    }
                    if (ssoMapping.Count > 0)
                    {
                        //Creating SSO Mapping FIle
                        string appName = Path.GetFileNameWithoutExtension(file).Split('_')[1];
                        using (XmlWriter writer =
                            XmlWriter.Create(_xmlPath + @"\" + "SSOMap_" + appName + "_ToImport" + ".xml"))
                        {
                            writer.WriteStartElement("SSO");
                            foreach (string mapping in ssoMapping)
                            {
                                try
                                {
                                    writer.WriteStartElement("mapping");
                                    writer.WriteElementString("windowsDomain",
                                        mapping.Split(':')[0].Split(')')[1].Split('\\')[0].Trim());
                                    writer.WriteElementString("windowsUserId",
                                        mapping.Split(':')[0].Split(')')[1].Split('\\')[1].Trim());
                                    writer.WriteElementString("externalApplication", appName);
                                    writer.WriteElementString("externalUserId", mapping.Split(':')[1].Trim());
                                    writer.WriteEndElement();
                                }
                                catch (Exception ex)
                                {
                                    LogInfoSyncronously("Exception while Writing into  SSO Mapping File : " + file +
                                                        ex.Message);
                                    LogException(ex);
                                }
                            }
                            writer.WriteEndElement();
                        }
                    }

                    else
                    {
                        LogInfo("No Mappings are present for  " + file);
                    }
                }
                LogShortSuccessMsg("Success: Exported SSO Mapping Xmls");
            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Exporting SSO Affiliate Application " + ex.Message);
                LogException(ex);
                throw;
            }

        }

        private void ImportSSOApps()
        {
            string fileName = @"C:\Program Files\Common Files\Enterprise Single Sign-On\ssomanage.exe";
            //Creating the DestinationAppList
            try
            {
                string commandArguments;
                if (_machineName == _strDstNode) //local
                {
                    commandArguments = "/C " + "\"\"" + fileName + "\"" + " -listapps />\"" + _xmlPath +
                                       "\\DstSSOAppsList.txt" + "\"";
                }
                else
                {
                    string xmlPathUnc = ConvertPathToUncPath(_xmlPath);
                    commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode + " -u " + "\"" +
                                       _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" + " -accepteula" +
                                       fileName + "\"" + " -listapps />\"" + "\\\\" + _machineName + "\\" + xmlPathUnc +
                                       "\\DstSSOAppsList.txt" + "\"";

                }

                var returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                if (returnCode == 0)
                {
                    LogShortSuccessMsg("Success: Created Destination SSO Affiliate Application List.");

                }
                else
                {
                    LogShortErrorMsg("Failed: Created Destination SSO Affiliate Application List.");

                }
                List<string> dstSsoApps = new List<string>();
                if (File.Exists(_xmlPath + "\\" + "DstSSOAppsList.txt"))
                {
                    string[] dstSsoAppsList = File.ReadAllLines(_xmlPath + @"\DstSSOAppsList.txt");

                    foreach (string ssoApp in dstSsoAppsList)
                    {
                        if (string.IsNullOrEmpty(ssoApp) || string.IsNullOrWhiteSpace(ssoApp) ||
                            ssoApp.Contains("Using SSO server") || ssoApp.Contains("Applications available for") ||
                            ssoApp.Contains("applications available for"))

                        {
                        }
                        else
                        {
                            dstSsoApps.Add(ssoApp.Split(':')[0].Split(')')[1].Trim());

                        }
                    }
                }
                string[] files = Directory.GetFiles(_xmlPath, "SSOApp_*.xml");
                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        string appName = Path.GetFileNameWithoutExtension(file).Split('_')[1].Split('_')[0];
                        try
                        {
                            if (dstSsoApps.Contains(appName))
                            {
                                LogInfoInLogFile(appName + " is already present");
                            }
                            else
                            {
                                //Import the App and Mappings associated to App

                                if (_machineName == _strDstNode) //local
                                {
                                    commandArguments =
                                        "/C " + "\"\"" + fileName + "\"" + " -createapps \"" + file + "\"";
                                }
                                else
                                {
                                    string filePathUnc = ConvertPathToUncPath(file);
                                    commandArguments =
                                        "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode + " -u " +
                                        "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                        " -accepteula" + fileName + "\"" + " -createapps \"" + "\\\\" +
                                        _machineName + "\\" + filePathUnc + "\"";

                                }
                                returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                if (returnCode == 0)
                                {
                                    LogInfoInLogFile(
                                        "Success: Created Destination SSO Affiliate Application:" + appName);

                                }
                                else
                                {
                                    LogInfoInLogFile("Failed: Created Destination SSO Affiliate Application List:" +
                                                     appName);

                                }

                                string[] mappingFiles =
                                    Directory.GetFiles(_xmlPath, "SSOMap_" + appName + "_ToImport" + ".xml");
                                foreach (string mappingFile in mappingFiles)
                                {
                                    if (_machineName == _strDstNode) //local
                                    {
                                        commandArguments =
                                            "/C " + "\"\"" + fileName + "\"" + " -createmappings \"" + mappingFile +
                                            "\"";
                                    }
                                    else
                                    {
                                        string mappingFileUnc = ConvertPathToUncPath(mappingFile);
                                        commandArguments = "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode +
                                                           " -u " + "\"" + _strUserName + "\"" + " -p " + "\"" +
                                                           _strPassword + "\"" + " -accepteula" + fileName + "\"" +
                                                           " -createMappings />\"" + "\\\\" + _machineName + "\\" +
                                                           mappingFileUnc + "\"";

                                    }
                                    returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                    if (returnCode == 0)
                                    {
                                        LogInfoInLogFile(
                                            "Success: Created Destination SSO Affiliate Application Mappings:" +
                                            mappingFile);

                                    }
                                    else
                                    {
                                        LogInfoInLogFile("Failed: Created Destination SSO Affiliate Application List:" +
                                                         mappingFile);

                                    }
                                    //Enabling the Mapping
                                    XmlDocument xmldoc = new XmlDocument();
                                    xmldoc.Load(mappingFile);
                                    XmlNodeList nodeList = xmldoc.DocumentElement.SelectNodes("/SSO/mapping");

                                    foreach (string windowsAccount in from XmlNode node in nodeList select node.SelectSingleNode("windowsDomain").InnerText + "\\" +
                                                                                                           node.SelectSingleNode("windowsUserId").InnerText)
                                    {
                                        try
                                        {
                                            if (_machineName == _strDstNode) //local
                                            {
                                                commandArguments =
                                                    "/C " + "\"\"" + fileName + "\"" + " -enablemapping \"" +
                                                    windowsAccount + "\"" + " " + "\"" + appName + "\"";
                                            }
                                            else
                                            {
                                                commandArguments =
                                                    "/C " + "\"\"" + _psExecPath + "\"  \\\\" + _strDstNode + " -u " +
                                                    "\"" + _strUserName + "\"" + " -p " + "\"" + _strPassword + "\"" +
                                                    " -accepteula" + fileName + "\"" + " -enablemapping \"" +
                                                    windowsAccount + "\"" + appName + "\"";

                                            }
                                            returnCode = ExecuteCmd("CMD.EXE", commandArguments);

                                            if (returnCode == 0)
                                            {
                                                LogInfoInLogFile(
                                                    "Success: Enabled SSO Affiliate Application Mappings:" +
                                                    mappingFile);

                                            }
                                            else
                                            {
                                                LogInfoInLogFile(
                                                    "Failed: Enabled SSO Affiliate Application List:" + mappingFile);

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogInfoSyncronously(
                                                "Exception while Enabling SSO Affiliate Application Mapping " +
                                                appName + " :" + ex.Message);
                                            LogException(ex);
                                        }
                                    }

                                }
                                LogShortSuccessMsg(
                                    "Success: Created Destination SSO Affiliate Applications and Mappings.");
                            }

                        }
                        catch (Exception ex)
                        {
                            LogInfoSyncronously("Exception while Importing SSO Affiliate Application " + appName +
                                                " :" + ex.Message);
                            LogException(ex);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("No SSO Information Files are present");
                }

            }
            catch (Exception ex)
            {
                LogInfoSyncronously("Exception while Impoting SSO Affiliate Applications " + ex.Message);
                LogException(ex);
                throw;
            }
        }

        #endregion

    }
}
