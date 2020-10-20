using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;

namespace MigrationTool {
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to System.DateTime and vice-versa.
    // An Early Bound class generated for the WMI class.MSBTS_ServerHost
    public class ServerHost : Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = "ROOT\\MicrosoftBizTalkServer";
        
        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "MSBTS_ServerHost";
        
        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static ManagementScope _statMgmtScope;
        
        private ManagementSystemProperties _privateSystemProperties;
        
        // Underlying lateBound WMI object.
        private ManagementObject _privateLateBoundObject;
        
        // Member variable to store the 'automatic commit' behavior for the class.
        private bool _autoCommitProp;
        
        // Private variable to hold the embedded property representing the instance.

        // The current WMI object used
        private ManagementBaseObject _curObj;
        
        // Flag to indicate if the instance is an embedded object.
        private bool _isEmbedded;
        
        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public ServerHost() {
            InitializeObject(null, null, null);
        }
        
        public ServerHost(string keyHostName, string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyServerName) {
            InitializeObject(null, new ManagementPath(ConstructPath(keyHostName, keyMgmtDbNameOverride, keyMgmtDbServerOverride, keyServerName)), null);
        }
        
        public ServerHost(ManagementScope mgmtScope, string keyHostName, string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyServerName) {
            InitializeObject(mgmtScope, new ManagementPath(ConstructPath(keyHostName, keyMgmtDbNameOverride, keyMgmtDbServerOverride, keyServerName)), null);
        }
        
        public ServerHost(ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(null, path, getOptions);
        }
        
        public ServerHost(ManagementScope mgmtScope, ManagementPath path) {
            InitializeObject(mgmtScope, path, null);
        }
        
        public ServerHost(ManagementPath path) {
            InitializeObject(null, path, null);
        }
        
        public ServerHost(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(mgmtScope, path, getOptions);
        }
        
        public ServerHost(ManagementObject theObject) {
            Initialize();
            if (CheckIfProperClass(theObject)) {
                _privateLateBoundObject = theObject;
                _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
                _curObj = _privateLateBoundObject;
            }
            else {
                throw new ArgumentException("Class name does not match.");
            }
        }
        
        public ServerHost(ManagementBaseObject theObject) {
            Initialize();
            if (CheckIfProperClass(theObject)) {
                _privateSystemProperties = new ManagementSystemProperties(theObject);
                _curObj = theObject;
                _isEmbedded = true;
            }
            else {
                throw new ArgumentException("Class name does not match.");
            }
        }
        
        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "ROOT\\MicrosoftBizTalkServer";
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if (_curObj?.ClassPath != null) {
                    strRet = (string)_curObj["__CLASS"];
                    if (string.IsNullOrEmpty(strRet)) {
                        strRet = CreatedClassName;
                    }
                }
                return strRet;
            }
        }
        
        // Property pointing to an embedded object to get System properties of the WMI object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties {
            get {
                return _privateSystemProperties;
            }
        }
        
        // Property returning the underlying lateBound object.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementBaseObject LateBoundObject {
            get {
                return _curObj;
            }
        }
        
        // ManagementScope of the object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementScope Scope {
            get {
                if (_isEmbedded == false) {
                    return _privateLateBoundObject.Scope;
                }
                else {
                    return null;
                }
            }
            set {
                if (_isEmbedded == false) {
                    _privateLateBoundObject.Scope = value;
                }
            }
        }
        
        // Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return _autoCommitProp;
            }
            set {
                _autoCommitProp = value;
            }
        }
        
        // The ManagementPath of the underlying WMI object.
        [Browsable(true)]
        public ManagementPath Path {
            get {
                if (_isEmbedded == false) {
                    return _privateLateBoundObject.Path;
                }
                else {
                    return null;
                }
            }
            set {
                if (_isEmbedded == false) {
                    if (CheckIfProperClass(null, value, null) != true) {
                        throw new ArgumentException("Class name does not match.");
                    }
                    _privateLateBoundObject.Path = value;
                }
            }
        }
        
        // Public static scope property which is used by the various methods.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static ManagementScope StaticScope {
            get {
                return _statMgmtScope;
            }
            set {
                _statMgmtScope = value;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Caption property is a short description (one-line string) of the object.")]
        public string Caption {
            get {
                return (string)_curObj["Caption"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Description property provides a description of the object. ")]
        public string Description {
            get {
                return (string)_curObj["Description"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains the name of the BizTalk Host. Max length for this property" +
            " is 80 characters.")]
        public string HostName {
            get {
                return (string)_curObj["HostName"];
            }

            set
            {
                _curObj["HostName"] = value;
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull {
            get {
                if (_curObj["InstallDate"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InstallDate property is a datetime value indicating when the object was insta" +
            "lled. The lack of a value does not indicate that the object is not installed.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public DateTime InstallDate {
            get {
                if (_curObj["InstallDate"] != null) {
                    return ToDateTime((string)_curObj["InstallDate"]);
                }
                else {
                    return DateTime.MinValue;
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIsMappedNull {
            get {
                if (_curObj["IsMapped"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property indicates whether the specified server works with the specified MessageBox. The server takes messages from the MessageBox when this property is set to TRUE. The server does not take messages from the MessageBox when this property is set to FALSE.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool IsMapped {
            get {
                if (_curObj["IsMapped"] == null) {
                    return Convert.ToBoolean(0);
                }
                return (bool)_curObj["IsMapped"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This optional property can be used to override the initial catalog part of the Bi" +
            "zTalk Messaging Management database connect string, and represents the database " +
            "name. Max length for this property is 123 characters.")]
        public string MgmtDbNameOverride {
            get {
                return (string)_curObj["MgmtDbNameOverride"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This optional property can be used to override the data source part of the BizTal" +
            "k Messaging Management database connect string. Max length for this property is " +
            "80 characters.")]
        public string MgmtDbServerOverride {
            get {
                return (string)_curObj["MgmtDbServerOverride"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Name property defines the label by which the object is known. When subclassed" +
            ", the Name property can be overridden to be a Key property.")]
        public string Name {
            get {
                return (string)_curObj["Name"];
            }
            set
            {
                _curObj["Name"] = value;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains the name of the server. Max length for this property is 63" +
            " characters.")]
        public string ServerName {
            get {
                return (string)_curObj["ServerName"];
            }

            set
            {
                _curObj["ServerName"] = value;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The Status property is a string indicating the current status of the object. Various operational and non-operational statuses can be defined. Operational statuses are ""OK"", ""Degraded"" and ""Pred Fail"". ""Pred Fail"" indicates that an element may be functioning properly but predicting a failure in the near future. An example is a SMART-enabled hard drive. Non-operational statuses can also be specified. These are ""Error"", ""Starting"", ""Stopping"" and ""Service"". The latter, ""Service"", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is on-line, yet the managed element is neither ""OK"" nor in one of the other states.")]
        public string Status {
            get {
                return (string)_curObj["Status"];
            }
        }
        
        private bool CheckIfProperClass(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions optionsParam) {
            if (path != null 
                && string.Compare(path.ClassName, ManagementClassName, true, CultureInfo.InvariantCulture) == 0) {
                return true;
            }
            else
            {
                using (var managementBaseObject = new ManagementObject(mgmtScope, path, optionsParam))
                {
                    return CheckIfProperClass(managementBaseObject);
                }
            }
        }
        
        private bool CheckIfProperClass(ManagementBaseObject theObj) {
            if (theObj != null 
                && string.Compare((string)theObj["__CLASS"], ManagementClassName, true, CultureInfo.InvariantCulture) == 0) {
                return true;
            }
            else {
                Array parentClasses = (Array)theObj["__DERIVATION"];
                if (parentClasses != null) {
                    return parentClasses.Cast<string>().Any(parentClass =>
                        string.Compare(parentClass, ManagementClassName, true, CultureInfo.InvariantCulture) == 0);
                }
            }
            return false;
        }
        
        // Converts a given datetime in DMTF format to System.DateTime object.
        static DateTime ToDateTime(string dmtfDate) {
            DateTime initializer = DateTime.MinValue;
            int year = initializer.Year;
            int month = initializer.Month;
            int day = initializer.Day;
            int hour = initializer.Hour;
            int minute = initializer.Minute;
            int second = initializer.Second;
            long ticks = 0;
            string dmtf = dmtfDate;
            if (dmtf == null)
            {
                throw new ArgumentNullException(nameof(dmtfDate));
            }
            if (dmtf.Length == 0)
            {
                throw new ArgumentException("", nameof(dmtfDate));
            }
            if (dmtf.Length != 25)
            {
                throw new ArgumentException("", nameof(dmtfDate));
            }
            var tempString = dmtf.Substring(0, 4);
            if ("****" != tempString) {
                year = int.Parse(tempString);
            }
            tempString = dmtf.Substring(4, 2);
            if ("**" != tempString) {
                month = int.Parse(tempString);
            }
            tempString = dmtf.Substring(6, 2);
            if ("**" != tempString) {
                day = int.Parse(tempString);
            }
            tempString = dmtf.Substring(8, 2);
            if ("**" != tempString) {
                hour = int.Parse(tempString);
            }
            tempString = dmtf.Substring(10, 2);
            if ("**" != tempString) {
                minute = int.Parse(tempString);
            }
            tempString = dmtf.Substring(12, 2);
            if ("**" != tempString) {
                second = int.Parse(tempString);
            }
            tempString = dmtf.Substring(15, 6);
            if ("******" != tempString) {
                ticks = long.Parse(tempString) * (TimeSpan.TicksPerMillisecond / 1000);
            }
            if (year < 0 
                || month < 0 
                || day < 0 
                || hour < 0 
                || minute < 0 
                || minute < 0 
                || second < 0 
                || ticks < 0) {
                throw new ArgumentException("", nameof(dmtfDate));
            }
            var datetime = new DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            long offsetMins = tickOffset.Ticks / TimeSpan.TicksPerMinute;
            tempString = dmtf.Substring(22, 3);
            if (tempString != "******") {
                tempString = dmtf.Substring(21, 4);
                int utcOffset;
                try {
                    utcOffset = int.Parse(tempString);
                }
                catch (Exception e) {
                    throw new ArgumentOutOfRangeException(null, e.Message);
                }
                var offsetToBeAdjusted = (int)(offsetMins - utcOffset);
                datetime = datetime.AddMinutes(offsetToBeAdjusted);
            }
            return datetime;
        }
        
        // Converts a given System.DateTime object to DMTF datetime format.
        static string ToDmtfDateTime(DateTime date) {
            string utcString;
            TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            long offsetMins = tickOffset.Ticks / TimeSpan.TicksPerMinute;
            if (Math.Abs(offsetMins) > 999) {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else {
                if (tickOffset.Ticks >= 0) {
                    utcString = string.Concat("+", (tickOffset.Ticks / TimeSpan.TicksPerMinute).ToString().PadLeft(3, '0'));
                }
                else {
                    string strTemp = offsetMins.ToString();
                    utcString = string.Concat("-", strTemp.Substring(1, strTemp.Length - 1).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = date.Year.ToString().PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, date.Month.ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Day.ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Hour.ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Minute.ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Second.ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            DateTime dtTemp = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            long microsec = (date.Ticks - dtTemp.Ticks) 
                            * 1000 
                            / TimeSpan.TicksPerMillisecond;
            string strMicrosec = microsec.ToString();
            if (strMicrosec.Length > 6) {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }
        
        private bool ShouldSerializeInstallDate() {
            if (IsInstallDateNull == false) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeIsMapped() {
            if (IsIsMappedNull == false) {
                return true;
            }
            return false;
        }
        
        [Browsable(true)]
        public void CommitObject() {
            if (_isEmbedded == false) {
                _privateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject(PutOptions putOptions) {
            if (_isEmbedded == false) {
                _privateLateBoundObject.Put(putOptions);
            }
        }
        
        private void Initialize() {
            _autoCommitProp = true;
            _isEmbedded = false;
        }
        
        private static string ConstructPath(string keyHostName, string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyServerName) {
            string strPath = "ROOT\\MicrosoftBizTalkServer:MSBTS_ServerHost";
            strPath = string.Concat(strPath, string.Concat(".HostName=", string.Concat("\"", string.Concat(keyHostName, "\""))));
            strPath = string.Concat(strPath, string.Concat(",MgmtDbNameOverride=", string.Concat("\"", string.Concat(keyMgmtDbNameOverride, "\""))));
            strPath = string.Concat(strPath, string.Concat(",MgmtDbServerOverride=", string.Concat("\"", string.Concat(keyMgmtDbServerOverride, "\""))));
            strPath = string.Concat(strPath, string.Concat(",ServerName=", string.Concat("\"", string.Concat(keyServerName, "\""))));
            return strPath;
        }
        
        private void InitializeObject(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions) {
            Initialize();
            if (path != null) {
                if (CheckIfProperClass(mgmtScope, path, getOptions) != true) {
                    throw new ArgumentException("Class name does not match.");
                }
            }
            _privateLateBoundObject = new ManagementObject(mgmtScope, path, getOptions);
            _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
            _curObj = _privateLateBoundObject;
        }
        
        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static IEnumerable<ServerHost> GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static IEnumerable<ServerHost> GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static IEnumerable<ServerHost> GetInstances(String [] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static IEnumerable<ServerHost> GetInstances(string condition, String [] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static IEnumerable<ServerHost> GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions) {
            if (mgmtScope == null)
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\MicrosoftBizTalkServer"}};
            }
            ManagementPath pathObj = new ManagementPath
            {
                ClassName = "MSBTS_ServerHost",
                NamespacePath = "root\\MicrosoftBizTalkServer"
            };
            using (ManagementClass clsObject = new ManagementClass(mgmtScope, pathObj, null))
            {
                if (enumOptions == null) {
                    enumOptions = new EnumerationOptions {EnsureLocatable = true};
                }
                return clsObject.GetInstances(enumOptions).Cast<ServerHost>();
            }
        }
        
        public static IEnumerable<ServerHost> GetInstances(ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static IEnumerable<ServerHost> GetInstances(ManagementScope mgmtScope, String [] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static IEnumerable<ServerHost> GetInstances(ManagementScope mgmtScope, string condition, String [] selectedProperties) {
            if (mgmtScope == null)
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\MicrosoftBizTalkServer"}};
            }
            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("MSBTS_ServerHost", condition, selectedProperties)))
            {
                EnumerationOptions enumOptions = new EnumerationOptions {EnsureLocatable = true};
                objectSearcher.Options = enumOptions;
                return objectSearcher.Get().Cast<ServerHost>();
            }
        }
        
        [Browsable(true)]
        public static ServerHost CreateInstance() {
            var mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = CreatedWmiNamespace}};
            ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
            using (ManagementClass tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null))
            {
                return new ServerHost(tmpMgmtClass.CreateInstance());
            }
        }

        [Browsable(true)]
        public static ServerHost CreateInstance(string pServer, string pUserName,string pPassword, string pDomain)
        {
            ManagementScope mgmtScope;
            if (_statMgmtScope == null)
            {
                mgmtScope = new ManagementScope
                {
                    Path = {NamespacePath = "\\\\" + pServer + "\\" + CreatedWmiNamespace},
                    Options = new ConnectionOptions
                    {
                        Username = pUserName,
                        Password = pPassword,
                        Authority = "ntlmdomain:" + pDomain
                    }
                };


            }
            else
            {
                mgmtScope = _statMgmtScope;
            }
            ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
            using (ManagementClass tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null))
            {
                return new ServerHost(tmpMgmtClass.CreateInstance());
            }
        }
        
        [Browsable(true)]
        public void Delete() {
            _privateLateBoundObject.Delete();
        }
        
        public uint ForceUnmap() {
            if (_isEmbedded == false) {
                ManagementBaseObject outParams = _privateLateBoundObject.InvokeMethod("ForceUnmap", null, null);
                return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return Convert.ToUInt32(0);
            }
        }
        
        public uint Map() {
            if (_isEmbedded == false) {
                ManagementBaseObject outParams = _privateLateBoundObject.InvokeMethod("Map", null, null);
                return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return Convert.ToUInt32(0);
            }
        }
        
        public uint Unmap() {
            if (_isEmbedded == false) {
                ManagementBaseObject outParams = _privateLateBoundObject.InvokeMethod("Unmap", null, null);
                return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return Convert.ToUInt32(0);
            }
        }
        
        // TypeConverter to handle null values for ValueType properties
        public class WMIValueTypeConverter : TypeConverter {
            
            private readonly TypeConverter _baseConverter;
            
            private readonly Type _baseType;
            
            public WMIValueTypeConverter(Type inBaseType) {
                _baseConverter = TypeDescriptor.GetConverter(inBaseType);
                _baseType = inBaseType;
            }
            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType) {
                return _baseConverter.CanConvertFrom(context, srcType);
            }
            
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                return _baseConverter.CanConvertTo(context, destinationType);
            }
            
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                return _baseConverter.ConvertFrom(context, culture, value);
            }
            
            public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
                return _baseConverter.CreateInstance(context, propertyValues);
            }
            
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetCreateInstanceSupported(context);
            }
            
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
                return _baseConverter.GetProperties(context, value, attributes);
            }
            
            public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetPropertiesSupported(context);
            }
            
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValues(context);
            }
            
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValuesExclusive(context);
            }
            
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValuesSupported(context);
            }
            
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
                if (_baseType.BaseType == typeof(Enum)) {
                    if (value.GetType() == destinationType) {
                        return value;
                    }
                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (_baseType == typeof(bool) 
                    && _baseType.BaseType == typeof(ValueType)) {
                    if (value == null 
                        && context != null 
                        && context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false) {
                        return "";
                    }
                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (context != null 
                    && context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false) {
                    return "";
                }
                return _baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }
        
        // Embedded class to represent WMI system Properties.
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ManagementSystemProperties {
            
            private readonly ManagementBaseObject _privateLateBoundObject;
            
            public ManagementSystemProperties(ManagementBaseObject managedObject) {
                _privateLateBoundObject = managedObject;
            }
            
            [Browsable(true)]
            public int Genus {
                get {
                    return (int)_privateLateBoundObject["__GENUS"];
                }
            }
            
            [Browsable(true)]
            public string Class {
                get {
                    return (string)_privateLateBoundObject["__CLASS"];
                }
            }
            
            [Browsable(true)]
            public string Superclass {
                get {
                    return (string)_privateLateBoundObject["__SUPERCLASS"];
                }
            }
            
            [Browsable(true)]
            public string Dynasty {
                get {
                    return (string)_privateLateBoundObject["__DYNASTY"];
                }
            }
            
            [Browsable(true)]
            public string Relpath {
                get {
                    return (string)_privateLateBoundObject["__RELPATH"];
                }
            }
            
            [Browsable(true)]
            public int PropertyCount {
                get {
                    return (int)_privateLateBoundObject["__PROPERTY_COUNT"];
                }
            }
            
            [Browsable(true)]
            public IEnumerable<string> Derivation {
                get {
                    return (string[])_privateLateBoundObject["__DERIVATION"];
                }
            }
            
            [Browsable(true)]
            public string Server {
                get {
                    return (string)_privateLateBoundObject["__SERVER"];
                }
            }
            
            [Browsable(true)]
            public string Namespace {
                get {
                    return (string)_privateLateBoundObject["__NAMESPACE"];
                }
            }
            
            [Browsable(true)]
            public string Path {
                get {
                    return (string)_privateLateBoundObject["__PATH"];
                }
            }
        }
    }
}
