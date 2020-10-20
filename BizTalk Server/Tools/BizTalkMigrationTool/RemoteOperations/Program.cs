using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.BizTalk.ExplorerOM;
using System.Reflection;
using System.Management;
using System.Data.SqlClient;
using System.Data;
using Microsoft.RuleEngine;
using Microsoft.Web.Administration;
using System.Security.Cryptography;
using System.Xml.Linq;
using Application = Microsoft.BizTalk.ExplorerOM.Application;
using ApplicationCollection = Microsoft.BizTalk.ExplorerOM.ApplicationCollection;

namespace RemoteOperations
{

    class Program
    {

        static int Main(string[] args)
        {
            //args 0 : rootPAth --- place of invocation, path of directroy from where this exe invoked
            //args 1 : operation --- ExportCert, ImportCert, ExportDll
            //args 2 : target machine Export Folder

            string rootPath = args[0];

            try
            {
                var appPath = rootPath;

                var certPath = appPath + @"\CERT";
                RemoteExportImportFunctions expo = new RemoteExportImportFunctions();

                switch (args[1])
                {
                    case "ExportCert":
                        if (!Directory.Exists(appPath + "\\Logs"))
                        {
                            //recreate
                            Directory.CreateDirectory(appPath + "\\Logs");
                            expo.LogInfo("Created log folder", rootPath);
                        }

                        if (!Directory.Exists(certPath))
                        {
                            //recreate                                            
                            Directory.CreateDirectory(certPath);
                            expo.LogInfo("Created cert folder", rootPath);
                        }

                        expo.btnExportCert_Click(rootPath, args[2]);
                        break;
                    case "ImportCert":
                        if (!Directory.Exists(appPath + "\\Logs"))
                        {
                            //recreate
                            Directory.CreateDirectory(appPath + "\\Logs");
                            expo.LogInfo("Created log folder", rootPath);
                        }

                        if (!Directory.Exists(certPath))
                        {
                            //recreate                                            
                            Directory.CreateDirectory(certPath);
                            expo.LogInfo("Created cert folder", rootPath);
                        }

                        expo.btnImportCert_Click(rootPath, args[2]);
                        break;
                    case "ExportDll":
                        expo.btnExportAssemblies_Click(rootPath, args[2], args[3], args[4], args[5]);
                        break;
                    case "ExportHandler":
                        expo.btnExportAdapterHandlers_Click(rootPath, args[2]);
                        break;
                    case "ExportAsmList":
                        expo.btnGetAssembliesList_Click(rootPath, args[2], args[3]);
                        break;
                    case "ExportAppList":
                        expo.btnGetApplicationList_Click(rootPath, args[2], args[3]);
                        break;
                    case "DstCustomDllList":
                        expo.btnDstCustomDllList_Click(rootPath, args[2]);
                        break;
                    case "DstDllList":
                        expo.btnDstDllList_Click(rootPath);
                        break;
                    case "SrcServiceList":
                    case "DstServiceList":
                        expo.btnGetServices_Click(rootPath, args[2], args[3], args[1]);
                        break;
                    case "ExportBREList":
                        expo.ExportBREPolicyVocabulary(rootPath, args[2]);
                        break;
                    case "ImportBREList":
                        expo.ImportBREPolicyVocabulary(rootPath, args[2]);
                        break;
                    case "ExportIISClientCert":
                        expo.ExportClientCertOnetOneMappings(rootPath);
                        break;
                    case "ImportHosts":
                        expo.ImportHostAndHostInstances(rootPath, args[2], args[3]);
                        break;
                    case "ImportIISClientCert":
                        expo.ImportClientCertOnetOneMappings(rootPath);
                        break;
                    case "ImportAppPool":
                        expo.ImportAppPools(rootPath);
                        break;
                    case "ImportWebSite":
                        expo.ImportWebsites(rootPath, args[2]);
                        break;
                    case "ImportWebApp":
                        expo.ImportWebApps(rootPath, args[2]);
                        break;
                    case "BTSInstallPath":
                        expo.BtsInstallPath(rootPath, args[2]);
                        break;
                    case "ExportBamDefinition":
                        expo.ExportBAMDefnition(rootPath, args[2]);
                        break;
                    case "ImportBamDefinition":
                        expo.ImportBAMDefinition(rootPath, args[2]);
                        break;
                    case "ExportBAMAccounts":
                        expo.ExportBAMAccounts(rootPath, args[2], args[3]);
                        break;
                    case "ImportBAMAccounts":
                        expo.AddBAMAccounts(rootPath, args[2], args[3], args[4]);
                        break;
                    case "ImportBTTList":
                        expo.ImportBTTDefiniton(rootPath, args[2], args[3]);
                        break;
                    case "ExportHostMapSettings":
                        expo.ExportHostMapSettings(rootPath, args[2], args[3]);
                        break;
                    case "ImportHostMapSettings":
                        expo.ImportHostMapSettings(rootPath, args[2]);
                        break;
                }

                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public class RemoteExportImportFunctions
        {
            readonly string _machineName = Environment.MachineName;

            public void btnExportCert_Click(string pRootPath, string pCertPass)
            {
                string thumbPrint = string.Empty, certFileName = string.Empty;
                var appPath = pRootPath;
                var certPath = appPath + @"\CERT";

                try
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
                                    LogInfo(iStoreLocation + "_" + iStoreName + "_Count:" + store.Certificates.Count,
                                        pRootPath);
                                    foreach (X509Certificate2 certificate in store.Certificates)
                                    {
                                        // Exporting EnhancedKEyUsage for Certificates
                                        var enhancedKeyUsage = new string[] {};
                                        foreach (X509Extension extension in certificate.Extensions.Cast<X509Extension>().Where(extension => extension.Oid.FriendlyName == "Enhanced Key Usage"))
                                        {
                                            try
                                            {
                                                enhancedKeyUsage = ((X509EnhancedKeyUsageExtension) extension)
                                                    .EnhancedKeyUsages
                                                    .Cast<Oid>()
                                                    .Where(oid =>
                                                        !string.IsNullOrEmpty(oid.FriendlyName) &&
                                                        !string.IsNullOrWhiteSpace(oid.FriendlyName))
                                                    .Select(oid => oid.FriendlyName.Trim())
                                                    .ToArray();
                                            }
                                            catch (Exception ex)
                                            {
                                                LogInfo(
                                                    "Exception Occured when Extracting Enhanced Key Usage: " +
                                                    certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                    certificate.Thumbprint, pRootPath);
                                                LogException(ex, pRootPath);
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
                                                thumbPrint = certificate.Thumbprint;
                                                LogInfo(
                                                    "File Name: " + certPath + @"\" + iStoreLocation + "_" +
                                                    iStoreName + "_" + thumbPrint + "_" + i, pRootPath);
                                                int dateCompare = DateTime.Compare(certificate.NotAfter, DateTime.Now);
                                                if (dateCompare >= 0)
                                                {
                                                    if (certificate.HasPrivateKey)
                                                    {
                                                        certFileName =
                                                            certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                            thumbPrint + "_" + i + ".PFX";
                                                        //certBytes = certificate.Export(X509ContentType.Pfx, strCertPass);
                                                        //File.WriteAllBytes(certFileName, certBytes);
                                                        string commandArguments;
                                                        if (iStoreLocation == "CurrentUser")
                                                            commandArguments =
                                                                " -f -exportpfx " + " -p " + "\"" + pCertPass + "\"" +
                                                                " -User " + iStoreName + " " + "\"" + thumbPrint +
                                                                "\"" + " " + certFileName + " " + "ExtendedProperties";
                                                        else
                                                            commandArguments =
                                                                " -f -exportpfx " + " -p " + "\"" + pCertPass + "\"" +
                                                                " " + iStoreName + " " + "\"" + thumbPrint + "\"" +
                                                                " " + certFileName + " " + "ExtendedProperties";
                                                        var returnCode = ExecuteCmd("certutil.exe", commandArguments,
                                                            pRootPath);
                                                        if (returnCode == 0)
                                                        {
                                                            LogInfo("Success: Exported Cert:" + certFileName,
                                                                pRootPath);
                                                        }
                                                        else
                                                            LogInfo("Failed Exporting Cert:" + certFileName, pRootPath);
                                                    }
                                                    else
                                                    {
                                                        certFileName =
                                                            certPath + @"\" + iStoreLocation + "_" + iStoreName + "_" +
                                                            thumbPrint + "_" + i + ".CER";
                                                        var certBytes = certificate.Export(X509ContentType.Cert);
                                                        File.WriteAllBytes(certFileName, certBytes);
                                                    }
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                LogInfo(
                                                    "File Name: " + certPath + @"\" + iStoreLocation + "_" +
                                                    iStoreName + "_" + thumbPrint + "_" + i, pRootPath);
                                                LogInfo("Exception in Cert: " + certFileName, pRootPath);
                                                LogException(ex, pRootPath);
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
                                    LogException(ex, pRootPath);
                                }
                                finally
                                {
                                    store.Close();
                                }
                            }
                        }
                    }
                    LogInfo("Exported Certificates from Server: " + _machineName, pRootPath);

                    if (File.Exists(pRootPath + "\\Logs\\Cert_log.txt")) //if already exist then delete
                        File.Delete(pRootPath + "\\Logs\\Cert_log.txt");
                    File.Move(pRootPath + "\\Logs\\RemoteOperation_log.txt",
                        pRootPath + "\\Logs\\Cert_log.txt"); //rename 
                }
                catch (Exception ex)
                {
                    LogInfo("Certificate Export Failed on Server: " + _machineName, pRootPath);
                    LogException(ex, pRootPath);

                    throw;
                }
            }

            public void btnImportCert_Click(string pRootPath, string pCertPass)
            {
                var appPath = pRootPath;
                var certPath = appPath + @"\CERT";

                try
                {
                    //BEGIN::new code for delta, get all cert name and write them in txt file,
                    X509Store store;
                    using (StreamWriter writer = new StreamWriter(appPath + @"\DstCertList.txt", false))
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
                                            writer.WriteLine(iStoreLocation + "_" + iStoreName + "_" + thumbPrint);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogException(ex, pRootPath);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException(ex, pRootPath);
                                }

                                finally
                                {
                                    store.Close();
                                }
                            }
                        }
                    }
                    //END::new code for delta, get all cert name and write them in txt file, 

                    string[] dstCertNameList = File.ReadAllLines(appPath + @"\DstCertList.txt"); //read all cert of Dst
                    //Creating CertificatesList with out StorLocation
                    var dstCertList = dstCertNameList.Select(t => t.Substring(t.IndexOf('_') + 1)).ToArray();
                    int certsImported = 0;
                    LogInfo("Calling Import Cert Function.", pRootPath);

                    foreach (string iStoreLocation in Enum.GetNames(typeof(StoreLocation)))
                    {
                        foreach (string iStoreName in Enum.GetNames(typeof(StoreName)))
                        {
                            LogInfo("Inside Cert Loop: StoreLocation: " + iStoreLocation + ", StoreName: " + iStoreName,
                                pRootPath);
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

                                var strFiles = Directory.GetFiles(certPath, iStoreLocation + "_" + iStoreName + "*");

                                foreach (string file in strFiles)
                                {
                                    try
                                    {
                                        string certName = file.Substring(file.LastIndexOf('\\') + 1);

                                        X509Certificate2 certificate;
                                        int returnCode;
                                        string commandArguments;
                                        if (storeNam == StoreName.My)
                                        {
                                            certName = certName.Substring(0, certName.LastIndexOf('_'));
                                            if (Array.IndexOf(dstCertNameList, certName) < 0
                                            ) //cert  already exists in Dst
                                            {
                                                if (Path.GetExtension(file) == ".PFX")
                                                {
                                                    if (file.Contains("CurrentUser"))
                                                        commandArguments =
                                                            "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                            " -p " + "\"" + pCertPass + "\"" + " -User " + storeNam +
                                                            " " + "\"" + file + "\"";
                                                    else
                                                        commandArguments =
                                                            "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                            " -p " + "\"" + pCertPass + "\"" + " " + storeNam + " " +
                                                            "\"" + file + "\"";

                                                    returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                                                    if (returnCode == 0)
                                                    {
                                                        LogInfo("Imported Cert: " + certName, pRootPath);
                                                    }
                                                    else
                                                        LogInfo("Failed Importing Cert:" + certName, pRootPath);
                                                    certsImported++;
                                                }
                                                else
                                                {
                                                    certificate = file.Contains("CurrentUser")
                                                        ? new X509Certificate2(file, pCertPass,
                                                            X509KeyStorageFlags.Exportable &
                                                            X509KeyStorageFlags.PersistKeySet &
                                                            X509KeyStorageFlags.UserKeySet)
                                                        : new X509Certificate2(file, pCertPass,
                                                            X509KeyStorageFlags.Exportable &
                                                            X509KeyStorageFlags.PersistKeySet &
                                                            X509KeyStorageFlags.MachineKeySet);
                                                    store.Add(certificate);
                                                    certsImported++;
                                                    LogInfo("Imported Cert: " + certName, pRootPath);
                                                }


                                            }
                                        }
                                        else
                                        {
                                            string certName1 = certName.Substring(0, certName.LastIndexOf('_'));
                                            certName = certName1.Substring(certName.IndexOf('_') + 1);
                                            if (Array.IndexOf(dstCertList, certName) < 0) //cert  already exists in Dst
                                            {
                                                if (Path.GetExtension(file) == ".PFX")
                                                {
                                                    if (file.Contains("CurrentUser"))
                                                        commandArguments =
                                                            "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                            " -p " + "\"" + pCertPass + "\"" + " -User " + storeNam +
                                                            " " + "\"" + file + "\"";
                                                    else
                                                        commandArguments =
                                                            "/C C:\\windows\\system32\\certutil.exe -f -importpfx " +
                                                            " -p " + "\"" + pCertPass + "\"" + " " + storeNam + " " +
                                                            "\"" + file + "\"";
                                                    returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                                                    if (returnCode == 0)
                                                    {
                                                        LogInfo("Imported Cert: " + certName1, pRootPath);
                                                    }
                                                    else
                                                        LogInfo("Failed Importing Cert:" + certName1, pRootPath);
                                                    certsImported++;
                                                }
                                                else
                                                {
                                                    certificate = file.Contains("CurrentUser")
                                                        ? new X509Certificate2(file, pCertPass,
                                                            X509KeyStorageFlags.Exportable &
                                                            X509KeyStorageFlags.PersistKeySet &
                                                            X509KeyStorageFlags.UserKeySet)
                                                        : new X509Certificate2(file, pCertPass,
                                                            X509KeyStorageFlags.Exportable &
                                                            X509KeyStorageFlags.PersistKeySet &
                                                            X509KeyStorageFlags.MachineKeySet);

                                                    store.Add(certificate);
                                                    certsImported++;
                                                    LogInfo("Imported Cert: " + certName1, pRootPath);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogInfo("Exception in cert:" + file, pRootPath);
                                        LogException(ex, pRootPath);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(ex, pRootPath);
                            }
                            finally
                            {
                                store.Close();
                            }
                        }
                    }

                    LogInfo(certsImported > 0 ? "Success: Imported Cert." : "No new certificates to import.",
                        pRootPath);

                    try
                    {
                        if (Directory.Exists(certPath))
                        {
                            Directory.Delete(certPath, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex, pRootPath);
                    }
                    LogInfo("Imported Certificates to Server: " + _machineName, pRootPath);
                    if (File.Exists(pRootPath + "\\Logs\\Cert_log.txt")) //if already exist then delete
                        File.Delete(pRootPath + "\\Logs\\Cert_log.txt");
                    File.Move(pRootPath + "\\Logs\\RemoteOperation_log.txt",
                        pRootPath + "\\Logs\\Cert_log.txt"); //rename
                }
                catch (Exception ex)
                {
                    LogInfo("Certificate Import Failed on Server: " + _machineName, pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void btnExportAssemblies_Click(string pRootPath, string pDllPath, string pCustomDllToInclude,
                string pCustomDllPath, string pServerType)
            {
                AssemblyList asmList = null;
                List<string> customDll = null;
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                int customDlls = 0;
                char[] chrSep = {','};
                try
                {
                    if (pServerType == "BizTalk")
                    {
                        asmList = DeserializeObject<AssemblyList>(xmlPath + @"\SrcBizTalkAssembly.xml");
                        var asmListCount = asmList.Assembly.Length;
                        LogInfo("Total Biztalk Assemblies: " + asmListCount, pRootPath);
                    }
                    LogInfo("Custom DLL Filter:" + pCustomDllToInclude, pRootPath);

                    var asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                    var asmPath2 = @"C:\Windows\assembly\GAC\";
                    var asmPath3 = @"C:\Windows\assembly\GAC_32\";
                    var asmPath4 = @"C:\Windows\assembly\GAC_64\";
                    var asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

                    if (pCustomDllToInclude != string.Empty)
                    {
                        customDll = new List<string>();
                        foreach (string filter in pCustomDllToInclude.Split(chrSep, StringSplitOptions.RemoveEmptyEntries))
                        {
//BEGIN::custom asm list code                         

                            customDll.AddRange(Directory.GetFiles(asmPath1, filter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath2, filter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath3, filter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath4, filter, SearchOption.AllDirectories));
                            customDll.AddRange(Directory.GetFiles(asmPath5, filter, SearchOption.AllDirectories));
                        }
                        LogInfo("Initial Custom Dll count: " + customDll.Count, pRootPath);

                        customDll = customDll.Distinct().ToList();
                        customDll.Sort();
                        //END::custom asm list code                                                                                    
                    }
                    string dir;
                    if (pServerType == "BizTalk")
                    {
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
                                    LogInfo("Did not Find Assembly:" + assembly.AsmName, pRootPath);
                                }
                                else
                                {
                                    foreach (string filePath in findDll)
                                    {
                                        try
                                        {
                                            if (pCustomDllToInclude != string.Empty)
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
                                            dir = pDllPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_" +
                                                  ver;
                                            LogInfo(dir, pRootPath);
                                            if (!Directory.Exists(dir))
                                            {
                                                Directory.CreateDirectory(dir);
                                            }
                                            File.Copy(filePath, dir + "\\" + Path.GetFileName(filePath), true);
                                            LogInfo("Assembly copied from : " + filePath + "...to..." + dir, pRootPath);
                                            bizTalkDll.Add(Path.GetFileNameWithoutExtension(filePath) + "_" + ver);

                                        }
                                        catch (Exception ex)
                                        {
                                            LogException(ex, pRootPath);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(ex, pRootPath);
                            }
                        }
                        string[] distinctBizTalkDll = bizTalkDll.Distinct().ToArray();
                        using (StreamWriter writer = new StreamWriter(xmlPath + @"\SrcBizTalkAssemblyList.txt", false))
                        {
                            foreach (string dll in distinctBizTalkDll)
                            {
                                writer.WriteLine(dll);
                            }
                        }

                    }

                    if (pCustomDllToInclude != string.Empty)
                    {
                        LogInfo("Final Custom Dll count: " + (customDll.Count - customDlls), pRootPath);
                        LogInfo("writing Src custom DLL File  : ", pRootPath);

                        using (StreamWriter writer = new StreamWriter(xmlPath + @"\SrcCustomAssemblyList.txt", false))
                        {

                            //copy custom dlls in CustomDll Folder

                            LogInfo("Copying Dlls back to : " + pCustomDllPath, pRootPath);
                            foreach (string filePath in customDll)
                            {
                                try
                                {

                                    if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrWhiteSpace(filePath))
                                    {
                                        string customDllVer = AssemblyName.GetAssemblyName(filePath).Version.ToString();
                                        dir = pCustomDllPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_" +
                                              customDllVer;

                                        if (!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                        File.Copy(filePath, dir + "\\" + Path.GetFileName(filePath), true);
                                        writer.WriteLine(
                                            Path.GetFileNameWithoutExtension(filePath) + "_" + customDllVer);
                                        //writer.WriteLine(filePath);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogInfo("Assembly copy failed: " + Path.GetFileName(filePath), pRootPath);
                                    LogException(ex, pRootPath);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex, pRootPath);
                    throw;
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

            public void btnDstCustomDllList_Click(string pRootPath, string pCustomDllToInclude)
            {
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                char[] chrSep = {','};
                try
                {
                    LogInfo("Inside btnDstCustomDllList_Click.", pRootPath);
                    var asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                    var asmPath2 = @"C:\Windows\assembly\GAC\";
                    var asmPath3 = @"C:\Windows\assembly\GAC_32\";
                    var asmPath4 = @"C:\Windows\assembly\GAC_64\";
                    var asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

                    ///////////////////////////////////////////////////////

                    var customDllDst = new List<string>();
                    foreach (string filter in pCustomDllToInclude.Split(chrSep, StringSplitOptions.RemoveEmptyEntries))
                    {
//BEGIN::custom asm list code                         

                        customDllDst.AddRange(Directory.GetFiles(asmPath1, filter, SearchOption.AllDirectories));
                        customDllDst.AddRange(Directory.GetFiles(asmPath2, filter, SearchOption.AllDirectories));
                        customDllDst.AddRange(Directory.GetFiles(asmPath3, filter, SearchOption.AllDirectories));
                        customDllDst.AddRange(Directory.GetFiles(asmPath4, filter, SearchOption.AllDirectories));
                        customDllDst.AddRange(Directory.GetFiles(asmPath5, filter, SearchOption.AllDirectories));
                    }
                    LogInfo("Custom Dll Count Dst: " + customDllDst.Count, pRootPath);

                    customDllDst = customDllDst.Distinct().ToList();
                    customDllDst.Sort();
                    ///////////////////////////////////////////////////////                                                        
                    //write custom dll paths in txt file
                    using (StreamWriter writer = new StreamWriter(xmlPath + @"\DstCustomAssemblyList.txt", false))
                    {
                        foreach (string filePath in customDllDst.Where(filePath => filePath != string.Empty))
                        {
                            try
                            {
                                var customDllVer = AssemblyName.GetAssemblyName(filePath).Version.ToString();
                                writer.WriteLine(Path.GetFileNameWithoutExtension(filePath) + "_" + customDllVer);
                            }
                            catch (Exception ex)
                            {
                                LogException(ex, pRootPath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void btnDstDllList_Click(string pRootPath)
            {
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                try
                {
                    LogInfo("Inside btnDstDllList_Click.", pRootPath);
                    string[] srcBizTalkDllLines = File.ReadAllLines(xmlPath + @"\SrcBizTalkAssemblyList.txt");
                    var asmPath1 = @"C:\Windows\Microsoft.NET\assembly\";
                    var asmPath2 = @"C:\Windows\assembly\GAC\";
                    var asmPath3 = @"C:\Windows\assembly\GAC_32\";
                    var asmPath4 = @"C:\Windows\assembly\GAC_64\";
                    var asmPath5 = @"C:\Windows\assembly\GAC_MSIL\";

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
                    using (StreamWriter writer = new StreamWriter(xmlPath + @"\DstBizTalkAssemblyList.txt", false))
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
                catch (Exception ex)
                {
                    LogException(ex, pRootPath);
                    throw;
                }

            }

            public void btnExportAdapterHandlers_Click(string pRootPath, string pSqlServerInstanceName)
            {
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                try
                {

                    LogInfo("Connecting to BizTalkMgmtdb...." + pSqlServerInstanceName, pRootPath);
                    RcvSndHandlers rcvSndHandlers = new RcvSndHandlers();

                    // instantiate new instance of Explorer OM
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + pSqlServerInstanceName +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        LogInfo("Conneceted", pRootPath);

                        rcvSndHandlers.RcvSndHandler = btsExp.ReceiveHandlers
                            .Cast<ReceiveHandler>()
                            .Where(rcvHandler => rcvHandler.Host.Name != "BizTalkServerApplication" &&
                                                 rcvHandler.Host.Name != "BizTalkServerIsolatedHost")
                            .Select(rcvHandler => new RcvSndHandlersRcvSndHandler
                            {
                                AdapterName = rcvHandler.TransportType.Name,
                                Direction = "0",
                                HostName = rcvHandler.Host.Name
                            })
                            .Concat(btsExp.SendHandlers.Cast<SendHandler>()
                                .Where(sndHandler => sndHandler.Host.Name != "BizTalkServerApplication" &&
                                                     sndHandler.Host.Name != "BizTalkServerIsolatedHost")
                                .Select(sndHandler => new RcvSndHandlersRcvSndHandler
                                {
                                    AdapterName = sndHandler.TransportType.Name,
                                    Direction = "1",
                                    HostName = sndHandler.Host.Name
                                }))
                            .ToArray();
                    }

                    SerializeObject(rcvSndHandlers, xmlPath + @"\Handlers.xml");
                    LogInfo("Handlers list genrated, please check XML folder", pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Adapter Handlers Export:: some Exception, pls check log.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void btnGetAssembliesList_Click(string pRootPath, string pSqlServerInstanceName,
                string pAppNameCollection)
            {
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                try
                {
                    LogInfo("Connecting to BizTalkMgmtDb..." + pSqlServerInstanceName, pRootPath);
                    // instantiate new instance of Explorer OM
                    AssemblyList asmList;
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + pSqlServerInstanceName +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        LogInfo("Connected.", pRootPath);

                        asmList = new AssemblyList
                        {
                            Assembly = (from Application app in btsExp.Applications
                                where pAppNameCollection.Contains(app.Name)
                                let asmCol = app.Assemblies
                                from BtsAssembly btAsm in asmCol
                                where !btAsm.IsSystem
                                select new AssemblyListAssembly
                                {
                                    AppName = app.Name,
                                    AsmName = btAsm.Name,
                                    AsmVer = btAsm.Version
                                }).ToArray()
                        };
                    }

                    SerializeObject(asmList, xmlPath + @"\SrcBizTalkAssembly.xml");
                    LogInfo("Asm list genrated in XML folder.", pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void btnGetApplicationList_Click(string pRootPath, string pSqlServerInstanceName,
                string pBizTalkAppToIgnore)
            {
                string appPath = pRootPath;
                string xmlPath = appPath + "\\ExportedData\\XMLFiles";

                try
                {
                    BizTalkApplications bizTalkApps = new BizTalkApplications();

                    LogInfo("Connecting to BizTalkMgmtdb...." + pSqlServerInstanceName, pRootPath);
                    // instantiate new instance of Explorer OM
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + pSqlServerInstanceName +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        var appCol = btsExp.Applications;
                        LogInfo("Connected.", pRootPath);

                        var htApps = new Dictionary<string, int>();
                        Msiapp(appCol, htApps, pBizTalkAppToIgnore);


                        bizTalkApps.BizTalkApplication = appCol.Cast<Application>()
                            .Where(app => !pBizTalkAppToIgnore.Contains(app.Name))
                            .Select(app => new BizTalkApplicationsBizTalkApplication
                            {
                                DependencyCode = htApps[app.Name].ToString(),
                                ApplicationName = app.Name
                            }).ToArray();

                    }

                    SerializeObject(bizTalkApps, xmlPath + @"\Apps.xml");
                    LogInfo("App list with dependency code genrated in XML folder.", pRootPath);
                }
                catch (Exception ex)
                {
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            private void Msiapp(ApplicationCollection appCol, IDictionary<string, int> htApps,
                string pBizTalkAppToIgnore)
            {
                foreach (Application app in appCol.Cast<Application>().Where(app => !pBizTalkAppToIgnore.Contains(app.Name)))
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
                        Msiapp(app.References, htApps, pBizTalkAppToIgnore);
                }
            }

            public void btnGetServices_Click(string pRootPath, string pWindowsServiceToIgnore, string pUserNameNoDomain,
                string pFileName)
            {
                try
                {
                    char[] chrSep = {','};
                    string appPath = pRootPath;
                    string xmlPath = appPath + "\\ExportedData\\XMLFiles";
                    string[] serviceName = pWindowsServiceToIgnore.Split(chrSep);

                    LogInfo("Starting WMI query to get list of services from remote server.", pRootPath);

                    SelectQuery query =
                        new SelectQuery("select name,startname,pathname,displayname from Win32_Service");

                    using (StreamWriter writer = new StreamWriter(xmlPath + "\\" + pFileName + ".txt", false))
                    {
                        using (ManagementObjectSearcher searcher =
                            new ManagementObjectSearcher(query))
                        {
                            foreach (var service in searcher.Get())
                            {
                                if (service["startname"] != null &&
                                    (service["startname"].ToString().ToLower().Contains(pUserNameNoDomain) ||
                                     service["startname"].ToString().ToLower().Contains(pUserNameNoDomain.ToLower())))
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
                    LogInfo("ServiceList Created:" + pFileName, pRootPath);
                }
                catch (Exception ex)
                {
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void ExportBREPolicyVocabulary(string pRootPath, string pSqlServerInstanceName)
            {
                string srcBRESqlInstance = null;
                string brePath = pRootPath + "\\ExportedData\\BRE";
                try
                {
                    //Getting the List of Policies associated to Application
                    string[] arrBREPolicies;
                    using (var sqlCon = new SqlConnection("Server=" + pSqlServerInstanceName + ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                    {
                        sqlCon.Open();
                        string sqlQuery =
                            "Select b.nvcName As ApplicationName,a.sdmType,a.luid,a.properties,a.files From adpl_sat AS a,bts_application AS b where a.sdmType='System.BizTalk:Rules' and b.nID= a.applicationId";
                        using (var sqlDataAd = new SqlDataAdapter(sqlQuery, sqlCon))
                        {
                            using (var ds = new DataSet())
                            {
                                sqlDataAd.Fill(ds);
                                arrBREPolicies = ds.Tables[0].Rows.Cast<DataRow>().Select(row =>
                                    row.ItemArray[2].ToString().Split('/')[1] + "." +
                                    row.ItemArray[2].ToString().Split('/')[2].Split('.')[0] + "." +
                                    row.ItemArray[2].ToString().Split('/')[2].Split('.')[1]).ToArray();
                            }
                        }

                        //Creating BRERuleEngineDb Connection
                        using (var sqlcmd = new SqlCommand("SELECT [RuleEngineDBServerName] FROM [adm_Group]", sqlCon))
                        {
                            using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (sqlRed.Read())
                                {
                                    srcBRESqlInstance = sqlRed.GetString(0);
                                }
                            }
                        }
                    }
                    //Creating Business RuleEngineDB COnnection
                    SqlConnectionStringBuilder sqlBRE = new SqlConnectionStringBuilder(
                        "Server = " + srcBRESqlInstance +
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
                        if (arrBREPolicies.Contains(policy))
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
                                    rulesetDepDriver.ExportRuleSetToFileRuleStore(ruleSetInfo,
                                        brePath + "/" + policyName);
                                    LogInfo(ruleSetInfo.Name + "Policy Exported", pRootPath);
                                }
                                catch (Exception ex)
                                {
                                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                                    LogException(ex, pRootPath);

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
                                                        var formattedVocabularyName = String.Format("{0}{1}.{2}.{3}.xml",
                                                            "Vocabualary_", vocabularyInfo.Name, vocabularyInfo.MajorRevision,
                                                            vocabularyInfo.MinorRevision);
                                                        rulesetDepDriver.ExportVocabularyToFileRuleStore(vocabularyInfo,
                                                            brePath + "/" + formattedVocabularyName);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                                                    LogException(ex, pRootPath);

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogInfo("Exception occured, please check log file for details.", pRootPath);
                                LogException(ex, pRootPath);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }

            }

            public void ImportBREPolicyVocabulary(string pRootPath, string pSqlServerInstanceName)
            {
                string dstBRESqlInstance = null;
                string brePath = pRootPath + "\\ExportedData\\BRE";
                try
                {
                    using (var sqlCon = new SqlConnection("Server=" + pSqlServerInstanceName + ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI"))
                    {
                        sqlCon.Open();
                        using (var sqlcmd = new SqlCommand("SELECT [RuleEngineDBServerName] FROM [adm_Group]", sqlCon))
                        {
                            using (var sqlRed = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (sqlRed.Read())
                                {
                                    dstBRESqlInstance = sqlRed.GetString(0);
                                }
                            }
                        }
                    }
                    SqlConnectionStringBuilder sqlBRE = new SqlConnectionStringBuilder(
                        "Server = " + dstBRESqlInstance +
                        "; Initial Catalog = BizTalkRuleEngineDb; Integrated Security = SSPI");
                    SqlRuleStore sqlRulesStore = new SqlRuleStore(sqlBRE.ConnectionString);
                    RuleSetDeploymentDriver rulesetDepDriver =
                        new RuleSetDeploymentDriver(sqlBRE.DataSource, "BizTalkRuleEngineDb");
                    RuleSetInfoCollection dstrulesetinfos = sqlRulesStore.GetRuleSets(RuleStore.Filter.All);
                    VocabularyInfoCollection dstvocabularyInfos = sqlRulesStore.GetVocabularies(RuleStore.Filter.All);

                    //Importing Vocabualries
                    var files = Directory.GetFiles(brePath, "Vocabualary*.xml");
                    if (files.Length != 0)
                    {
                        var vocabualrySet = dstvocabularyInfos.Cast<VocabularyInfo>()
                            .Select(dstvocabularyInfo => String.Format("{0}{1}.{2}.{3}.xml", "Vocabualary_",
                                dstvocabularyInfo.Name, dstvocabularyInfo.MajorRevision,
                                dstvocabularyInfo.MinorRevision)).ToArray();
                        //Creating a Set of Vocabularies which are Present in DstSqlInstance


                        foreach (string file in files)
                        {
                            //Checking Whether its Present in DstSQLInstance
                            string filename = Path.GetFileName(file);
                            if (vocabualrySet.Contains(filename))
                            {
                                LogInfo(file + "Already Published", pRootPath);
                            }
                            else
                            {
                                FileRuleStore fileRuleStore = new FileRuleStore(file);
                                VocabularyInfoCollection vocabularyInfoList =
                                    fileRuleStore.GetVocabularies(RuleStore.Filter.All);

                                foreach (VocabularyInfo vocabularyInfo in vocabularyInfoList)

                                {

                                    try
                                    {
                                        VocabularyInfo vi = new VocabularyInfo(vocabularyInfo.Name,
                                            vocabularyInfo.MajorRevision, vocabularyInfo.MinorRevision);
                                        Vocabulary oVoc = fileRuleStore.GetVocabulary(vi);
                                        sqlRulesStore.Add(oVoc, true);
                                        LogInfo(file + "Imported", pRootPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogInfo("Exception occured, please check log file for details.", pRootPath);
                                        LogException(ex, pRootPath);

                                    }

                                }
                            }

                        }
                    }
                    //Importing Policies
                    files = Directory.GetFiles(brePath, "Policy*.xml");
                    if (files.Length != 0)
                    {
                        var policySet = dstrulesetinfos.Cast<RuleSetInfo>()
                            .Select(dstruleSetInfo => String.Format("{0}{1}.{2}.{3}.xml", "Policy_",
                                dstruleSetInfo.Name, dstruleSetInfo.MajorRevision, dstruleSetInfo.MinorRevision)).ToArray();
                        //Creating Policies Present in DestSQLInstance
                        foreach (string file in files)
                        {
                            string filename = Path.GetFileName(file);

                            if (policySet.Contains(filename))
                            {
                                LogInfo(file + "Already Published", pRootPath);
                            }
                            else
                            {
                                try
                                {
                                    rulesetDepDriver.ImportAndPublishFileRuleStore(file);
                                    LogInfo(file + "Imported", pRootPath);
                                }
                                catch (Exception ex)
                                {
                                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                                    LogException(ex, pRootPath);

                                }
                            }



                        }

                    }
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }

            }

            public void ExportHostMapSettings(string pRootPath, string pSqlServerInstanceName, string srcServerList)
            {

                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                try
                {
                    using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                    {
                        btsExp.ConnectionString = "Server=" + pSqlServerInstanceName +
                                                  ";Initial Catalog=BizTalkMgmtDb;Integrated Security=SSPI";
                        string[] srcServers = srcServerList.Split(',');
                        foreach (string server in srcServers)
                        {
                            using (XmlWriter writer =
                                XmlWriter.Create(xmlPath + @"\" + "Src_" + server + "_HostMappings.xml"))
                            {
                                writer.WriteStartElement("SettingsMap");
                                writer.WriteStartElement("HostMappings");
                                foreach (Host host in btsExp.Hosts)
                                {
                                    writer.WriteStartElement("SourceHost");
                                    writer.WriteAttributeString("Name", host.Name);
                                    writer.WriteElementString("DestinationHosts", host.Name);
                                    writer.WriteEndElement();

                                }
                                writer.WriteEndElement();
                                //  Get all the HostInstances of the Destination Server
                                var hostInstancesArray = HostInstance.GetInstances()
                                    .Where(ht => ht.Name.EndsWith(server) || ht.Name.EndsWith(server.ToLower()))
                                    .Select(ht => ht.Name.Split(' ')[3]).ToList()
                                    .Where(x => !string.IsNullOrEmpty(x)).ToList();

                                writer.WriteStartElement("HostInstanceMappings");
                                foreach (string hostInstance in hostInstancesArray)
                                {
                                    writer.WriteStartElement("SourceHostInstance");
                                    writer.WriteAttributeString("Name", hostInstance + ":" + server);
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
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void ImportHostMapSettings(string pRootPath, string pSqlServerInstanceName)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                try
                {
                    LogInfo("Host Mappings: Creating Host Mappings.", pRootPath);
                    XmlDocument xd = new XmlDocument();
                    string[] srcservers;
                    //Getting the List of Source Servers
                    if (File.Exists(xmlPath + "\\" + "SrcServers.xml"))
                    {
                        xd.Load(xmlPath + "\\" + "SrcServers.xml");
                        XmlNode srcnodeList = xd.DocumentElement.SelectSingleNode("/Servers");

                        string srcServerList = srcnodeList.SelectSingleNode("SrcNodes").InnerText;
                        srcservers = srcServerList.Split(',');
                    }
                    else
                    {
                        throw new InvalidOperationException("SrcServers xml file is not available.");
                    }
                    //Getting the List of DestinationServers
                    xd.Load(xmlPath + "\\" + "Servers.xml");
                    XmlNode dstnodeList = xd.DocumentElement.SelectSingleNode("/Servers");

                    string dstServerList = dstnodeList.SelectSingleNode("DstNodes").InnerText;
                    var dstservers = dstServerList.Split(',');

                    String[] files = Directory.GetFiles(xmlPath, "Src_*_HostMappings.xml");
                    if (files.Length == 0)
                    {
                        throw new FileNotFoundException(" Source HostMapping xml file is not available.", "Src_*_HostMappings.xml");
                    }

                    if (dstservers.Length == srcservers.Length)
                    {
                        for (int i = 0; i < dstservers.Length; i++)
                        {
                            string srcHostMappingFile = xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml";
                            string dstHostMappingFile = xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                            // instantiate new instance of Explorer OM
                            string[] hostArray;
                            using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                            {
                                btsExp.ConnectionString =
                                    "Server=" + pSqlServerInstanceName +
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

                    if (dstservers.Length < srcservers.Length)
                    {
                        for (int i = 0; i < dstservers.Length; i++)
                        {
                            string srcHostMappingFile = xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml";
                            string dstHostMappingFile = xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                            // instantiate new instance of Explorer OM
                            string[] hostArray;
                            using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                            {
                                btsExp.ConnectionString =
                                    "Server=" + pSqlServerInstanceName +
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
                                ? xmlPath + @"\" + "Src_" + srcservers[i] + "_HostMappings.xml"
                                : xmlPath + @"\" + "Src_" + srcservers[0] + "_HostMappings.xml";
                            string dstHostMappingFile = xmlPath + @"\" + "Dst_" + dstservers[i] + "_HostMappings.xml";
                            // instantiate new instance of Explorer OM
                            string[] hostArray;
                            using (BtsCatalogExplorer btsExp = new BtsCatalogExplorer())
                            {
                                btsExp.ConnectionString =
                                    "Server=" + pSqlServerInstanceName +
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
                    LogInfo("Host Mappings: Created Host Mappings.", pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void ExportClientCertOnetOneMappings(string pRootPath)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";

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
                                ConfigurationSection iisClientCertificateMappingAuthenticationSection =
                                    config.GetSection(
                                        "system.webServer/security/authentication/iisClientCertificateMappingAuthentication",
                                        site.Name);
                                ConfigurationElementCollection oneToOneMappingsCollection =
                                    iisClientCertificateMappingAuthenticationSection.GetCollection("oneToOneMappings");
                                //Checking whether OneToOneCertifcationMappings are Present
                                if (oneToOneMappingsCollection.Count == 0)
                                {
                                    LogInfo(site.Name + " Website:Does not Contain  OneToOneCLientCertificateMappings",
                                        pRootPath);
                                }
                                else
                                {
                                    using (XmlWriter writer =
                                        XmlWriter.Create(xmlPath + @"\" + site.Name + "_ClientCertMappings.xml"))
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
                                LogInfo("Exception occured, please check log file for details.", pRootPath);
                                LogException(ex, pRootPath);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void ImportClientCertOnetOneMappings(string pRootPath)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                try
                {
                    using (ServerManager serverManager = new ServerManager())
                    {
                        foreach (Site site in serverManager.Sites)
                        {
                            try
                            {
                                string websiteMappingFile = site.Name + "_ClientCertMappings.xml";
                                DirectoryInfo di = new DirectoryInfo(xmlPath);
                                FileInfo[] files = di.GetFiles(websiteMappingFile);
                                if (files.Length == 0)
                                {
                                    LogInfo(site + "_ClientCertMappings are not Present", pRootPath);
                                }
                                else
                                {
                                    XmlDocument xmldoc = new XmlDocument();
                                    xmldoc.Load(xmlPath + @"\" + websiteMappingFile);
                                    XmlNodeList nodeList =
                                        xmldoc.DocumentElement.SelectNodes("/OneToOneMappings/OneToOneMapping");
                                    foreach (XmlNode node in nodeList)
                                    {
                                        Microsoft.Web.Administration.Configuration config =
                                            serverManager.GetApplicationHostConfiguration();

                                        ConfigurationSection iisClientCertificateMappingAuthenticationSection =
                                            config.GetSection(
                                                "system.webServer/security/authentication/iisClientCertificateMappingAuthentication",
                                                site.Name);
                                        iisClientCertificateMappingAuthenticationSection["enabled"] = true;
                                        iisClientCertificateMappingAuthenticationSection[
                                            "oneToOneCertificateMappingsEnabled"] = true;
                                        ConfigurationElementCollection oneToOneMappingsCollection =
                                            iisClientCertificateMappingAuthenticationSection.GetCollection(
                                                "oneToOneMappings");
                                        //Getting All the Certifcates which is already Present in Destination System
                                        var certificatearray = oneToOneMappingsCollection.Select(onetoone =>
                                            onetoone.GetAttributeValue("certificate").ToString()).ToList();
                                        if (certificatearray.Contains(node.SelectSingleNode("certificate").InnerText))
                                        {
                                            LogInfo(
                                                node.SelectSingleNode("certificate").InnerText +
                                                " already Exsists in Website: " + site.Name, pRootPath);
                                        }
                                        else
                                        {
                                            ConfigurationElement addElement =
                                                oneToOneMappingsCollection.CreateElement("add");
                                            addElement["enabled"] =
                                                Convert.ToBoolean(node.SelectSingleNode("enabled").InnerText);
                                            addElement["userName"] = node.SelectSingleNode("userName").InnerText;
                                            addElement["password"] =
                                                Decrypt(node.SelectSingleNode("password").InnerText);
                                            addElement["certificate"] = node.SelectSingleNode("certificate").InnerText;
                                            oneToOneMappingsCollection.Add(addElement);
                                            ConfigurationSection accessSection =
                                                config.GetSection("system.webServer/security/access", site.Name);
                                            accessSection["sslFlags"] = @"Ssl, SslNegotiateCert";
                                            LogInfo(
                                                node.SelectSingleNode("certificate").InnerText + " added in" +
                                                site.Name, pRootPath);
                                        }
                                    }
                                    serverManager.CommitChanges();
                                    LogInfo(site.Name + "OneToOneClientCert Mappings Completed", pRootPath);

                                }

                            }
                            catch (Exception ex)
                            {
                                LogInfo("Exception occured, please check log file for details.", pRootPath);
                                LogException(ex, pRootPath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                    throw;
                }
            }

            public void ImportHostAndHostInstances(string pRootPath, string strUserNameForHost,
                string strPasswordForHost)
            {

                //string dstHostList = string.Empty, dstHostInstanceList = string.Empty;
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                LogInfo("Host: Import started..", pRootPath);
                if (!File.Exists(xmlPath + @"\HostInstances.xml"))
                    throw new FileNotFoundException("HostInstances xml file does not exist.");
                //check file is empty or not
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath + "\\HostInstances.xml");
                if (doc.DocumentElement.ChildNodes.Count == 0) //if file not empty.                
                    throw new InvalidOperationException("HostInstances xml file is empty.");

                //Getting the List of DestinationServers
                XmlDocument xd = new XmlDocument();

                xd.Load(xmlPath + "\\" + "Servers.xml");
                XmlNode dstnodeList = xd.DocumentElement.SelectSingleNode("/Servers");

                string dstServerList = dstnodeList.SelectSingleNode("DstNodes").InnerText;
                var dstservers = dstServerList.Split(',');
                var input = DeserializeObject<Hosts>(xmlPath + @"\HostInstances.xml");
                //get all HostInstances of 'InProcess' type.

                EnumerationOptions enumOptions = new EnumerationOptions {ReturnImmediately = false};

                //Creating DestinationHostList
                List<string> dstHostList;
                using (var searchObjectHost = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_Host", enumOptions))
                {
                    dstHostList = searchObjectHost.Get()
                        .Cast<ManagementBaseObject>()
                        .Select(inst => inst["Name"].ToString().ToUpper())
                        .ToList();
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
                            host.isDefaultHost, pRootPath);
                    }
                    else
                        LogInfo("Host already exist: " + host.name, pRootPath);
                }

                foreach (string server in dstservers)
                {
//Creating DestinationHostInstanceListForeachnode
                    List<string> dstHostInstanceList;
                    using (var searchObjectHostInstance = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance", enumOptions))
                    {
                        dstHostInstanceList = searchObjectHostInstance.Get()
                            .Cast<ManagementObject>()
                            .Where(inst => inst["RunningServer"].ToString().ToUpper() == server)
                            .Select(inst => inst["HostName"].ToString().ToUpper())
                            .ToList();
                    }
                    //Create DestinationHostInstance for EachNode
                    foreach (HostsHost host in input.Host)
                    {
                        if (!dstHostInstanceList.Contains(host.name.ToUpper()))
                        {
                            CreateHostInstance(server, host.name, strUserNameForHost, strPasswordForHost,
                                pRootPath);
                        }
                        else
                            LogInfo("Host Instance already exist: " + host.name + " on " + server, pRootPath);
                    }
                }
            }

            private void CreateHost(string name, HostSetting.HostTypeValues hostType, string ntGroupName,
                bool authTrusted, bool hostTracking, bool is32Bit, bool defaultHost, string pRootPath)
            {
                try
                {
                    using (var myHostSetting = HostSetting.CreateInstance())
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

                    LogInfo("Host created successfully: " + name, pRootPath);
                }

                catch (Exception ex)
                {
                    LogInfo("Error Creating Host: " + name, pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            private void CreateHostInstance(string serverName, string name, string strUserNameForHost,
                string strPasswordForHost, string pRootPath)
            {
                try
                {
                    using (var myServerHost = ServerHost.CreateInstance())
                    {
                        myServerHost.ServerName = serverName;
                        myServerHost.HostName = name;
                        myServerHost.Map();
                    }


                    using (var myHostInstance = HostInstance.CreateInstance())
                    {
                        myHostInstance.Name = "Microsoft BizTalk Server " + name + " " + serverName;
                        myHostInstance.Install(true, strUserNameForHost, strPasswordForHost);
                    }

                    LogInfo("Host Instance created successfully: " + name + ", " + serverName, pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Error creating HostInstance: " + name + ", " + serverName, pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            public void ImportAppPools(string pRootPath)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                string commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add apppool /in /xml < \"" +
                                          xmlPath + "\\AppPoolToImport.xml" + "\"";

                int returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                if (returnCode == 0)
                    LogInfo("Success: Imported AppPools on Server " + _machineName, pRootPath);
                else
                    LogInfo("Failed: Importing AppPools on Server " + _machineName, pRootPath);


            }

            public void ImportWebsites(string pRootPath, string webSiteName)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                string commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add site /in /xml /< \"" +
                                          xmlPath + "\\WebSite_" + webSiteName + "_ToImport.xml" + "\"";

                int returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                if (returnCode == 0)
                    LogInfo("Success: Imported Website " + webSiteName, pRootPath);
                else
                    LogInfo("Failed: Importing Website " + webSiteName, pRootPath);
            }

            public void ImportWebApps(string pRootPath, string webSiteName)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                string commandArguments = "/C C:\\windows\\system32\\inetsrv\\appcmd.exe add app /in /xml /< \"" +
                                          xmlPath + "\\WebApps_" + webSiteName + "_ToImport.xml" + "\"";

                int returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                if (returnCode == 0)
                    LogInfo("Success: Imported WebApps for Website " + webSiteName, pRootPath);
                else
                    LogInfo("Failed: Importing WebApps for Website " + webSiteName, pRootPath);
            }

            public void BtsInstallPath(string pRootPath, string operation)
            {
                string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                try
                {
                    var commandArguments = operation == "Export"
                        ? "/C SET BTSINSTALLPATH > \"" + xmlPath + "\\SrcBTSInstallPath.txt" + "\""
                        : "/C SET BTSINSTALLPATH > \"" + xmlPath + "\\DstBTSInstallPath.txt" + "\"";

                    int returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                    LogInfo(
                        returnCode == 0
                            ? "Success:Created BTSInstallPath.txt."
                            : "Failed: Creating BTSInstallPath.txt.", pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Error creating BTSInstallPath", pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            public void ExportBAMDefnition(string pRootPath, string pSqlServerInstanceName)
            {
                try
                {
                    string bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
                    string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                    string commandArguments = " get-defxml -FileName:\"" + xmlPath + "\\BamDef.xml\" -Server:" +
                                              pSqlServerInstanceName + " -Database:BAMPrimaryImport";
                    int returnCode = ExecuteCmd(bamExePath, commandArguments, pRootPath);
                    if (returnCode == 0)
                        LogInfo("Success: BAM Definition Exported.", pRootPath);
                    else
                        throw new InvalidOperationException("Failed: BAM Definition Export");
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                }

            }

            public void ImportBAMDefinition(string pRootPath, string pSqlServerInstanceName)
            {
                try
                {

                    string bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
                    string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                    string commandArguments = " deploy-all -DefinitionFile:\"" + xmlPath +
                                              "\\BamDefToImport.xml\" -Server:" + pSqlServerInstanceName
                                              + " -Database:BAMPrimaryImport";
                    int returnCode = ExecuteCmd(bamExePath, commandArguments, pRootPath);
                    if (returnCode == 0)
                        LogInfo("Success: BamDef Imported.", pRootPath);
                    else
                        throw new InvalidOperationException("BamDef import failed, hence aborting account import.");
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            public void AddBAMAccounts(string pRootPath, string pSqlServerInstanceName, string accountName,
                string viewName)
            {
                try
                {
                    string bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
                    string commandArguments = " add-account -AccountName:\"" + accountName + "\" -View:\"" + viewName +
                                              "\" -Server:"
                                              + pSqlServerInstanceName
                                              + " -Database:BAMPrimaryImport";
                    int returnCode = ExecuteCmd(bamExePath, commandArguments, pRootPath);
                    if (returnCode == 0)
                        LogInfo("Account: " + accountName + " added to view: " + viewName, pRootPath);
                    else
                        LogInfo("Account: " + accountName + " could not be added to view: " + viewName, pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            public void ImportBTTDefiniton(string pRootPath, string pSqlServerInstanceName, string bttFile)
            {
                try
                {
                    string bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
                    string bttDeployExePath =
                        bamExePath.Substring(0, bamExePath.LastIndexOf("\\") + 1) + "bttDeploy.exe ";
                    string commandArguments = " /mgdb \"" + pSqlServerInstanceName + "\\BizTalkMgmtDb" + "\" \"" +
                                              bttFile + "\"";
                    int returnCode = ExecuteCmd(bttDeployExePath, commandArguments, pRootPath);
                    if (returnCode == 0)
                        LogInfo("Sucess: BTT File Imported " + bttFile.Substring(bttFile.LastIndexOf("\\") + 1),
                            pRootPath);
                    else
                        LogInfo("Failed: BTT File Import " + bttFile.Substring(bttFile.LastIndexOf("\\") + 1),
                            pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            public void ExportBAMAccounts(string pRootPath, string pSqlServerInstanceName, string viewName)
            {
                try
                {
                    string bamExePath = Environment.GetEnvironmentVariable("BTSINSTALLPATH") + @"Tracking\bm.exe";
                    string xmlPath = pRootPath + "\\ExportedData\\XMLFiles";
                    string commandArguments = "/C " + "\"\"" + bamExePath + "\"" + " get-accounts -View:\"" + viewName +
                                              "\" -Server:" + pSqlServerInstanceName
                                              + " -Database:BAMPrimaryImport > \"" + xmlPath + "\\BamView_" + viewName +
                                              ".txt\"\"";
                    int returnCode = ExecuteCmd("CMD.EXE", commandArguments, pRootPath);
                    if (returnCode == 0)
                        LogInfo("Success: Get BAM Accounts for View: " + viewName, pRootPath);
                    else
                        LogInfo("Failed: Get BAM Accounts for View: " + viewName, pRootPath);
                }
                catch (Exception ex)
                {
                    LogInfo("Exception occured, please check log file for details.", pRootPath);
                    LogException(ex, pRootPath);
                }
            }

            private string Encrypt(string data)
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

            private string Decrypt(string data)
            {
                var keyArray = Encoding.UTF8.GetBytes("M!grat!onkey1234");
                using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
                {
                    des.Mode = CipherMode.ECB;
                    des.Key = keyArray;
                    des.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform desEncrypt = des.CreateDecryptor())
                    {
                        Byte[] buffer = Convert.FromBase64String(data.Replace(" ", "+"));

                        return Encoding.UTF8.GetString(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                    }
                }
            }

            public void LogInfo(string strMsg, string pPath)
            {
                string appPath = pPath;
                string logPath = appPath + @"\Logs";

                if (strMsg != "")
                {
                    using (StreamWriter writer = new StreamWriter(logPath + @"\RemoteOperation_log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + strMsg);
                    }
                }
            }

            public void LogException(Exception ex, string pPath)
            {
                string appPath = pPath;
                string logPath = appPath + @"\Logs";

                using (StreamWriter writer = new StreamWriter(logPath + @"\RemoteOperation_log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss:::") + "Exception Message:  " +
                                     ex.Message);
                    writer.WriteLine("Inner Exception:  " + ex.InnerException);
                    writer.WriteLine("StackTrace:  " + ex.StackTrace);
                }
            }

            private int ExecuteCmd(string cmdName, string cmdArg, string pPath)
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
                        LogException(ex, pPath);
                    }
                }

                return 1;
            }

            void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                // LogInfo(e.Data,logPath);
            }

            void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                // LogInfo(e.Data, logPath);
            }

        }
    }
}
