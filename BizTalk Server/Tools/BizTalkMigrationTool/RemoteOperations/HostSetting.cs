using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;

namespace RemoteOperations {
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // An Early Bound class generated for the WMI class.MSBTS_HostSetting
    public class HostSetting : Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = "\\ROOT\\MicrosoftBizTalkServer";
        
        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "MSBTS_HostSetting";
        
        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static ManagementScope _statMgmtScope;
        
        private ManagementSystemProperties _privateSystemProperties;
        
        // Underlying lateBound WMI object.
        private ManagementObject _privateLateBoundObject;
        
        // Member variable to store the 'automatic commit' behavior for the class.
        private bool _autoCommitProp;
        
        // The current WMI object used
        private ManagementBaseObject _curObj;
        
        // Flag to indicate if the instance is an embedded object.
        private bool _isEmbedded;
        
        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public HostSetting() {
            InitializeObject(null, null, null);
        }
        
        public HostSetting(string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyName) {
            InitializeObject(null, new ManagementPath(ConstructPath(keyMgmtDbNameOverride, keyMgmtDbServerOverride, keyName)), null);
        }
        
        public HostSetting(ManagementScope mgmtScope, string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyName) {
            InitializeObject(mgmtScope, new ManagementPath(ConstructPath(keyMgmtDbNameOverride, keyMgmtDbServerOverride, keyName)), null);
        }
        
        public HostSetting(ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(null, path, getOptions);
        }
        
        public HostSetting(ManagementScope mgmtScope, ManagementPath path) {
            InitializeObject(mgmtScope, path, null);
        }
        
        public HostSetting(ManagementPath path) {
            InitializeObject(null, path, null);
        }
        
        public HostSetting(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(mgmtScope, path, getOptions);
        }
        
        public HostSetting(ManagementObject theObject) {
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
        
        public HostSetting(ManagementBaseObject theObject) {
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
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAuthTrustedNull {
            get {
                if (_curObj["AuthTrusted"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates whether the BizTalk Host is trusted to collect authentica" +
            "tion information.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool AuthTrusted {
            get {
                if (_curObj["AuthTrusted"] == null) {
                    return Convert.ToBoolean(0);
                }
                return (bool)_curObj["AuthTrusted"];
            }
            set {
                _curObj["AuthTrusted"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A short description (one-line string) of the CIM_Setting object.")]
        public string Caption {
            get {
                return (string)_curObj["Caption"];
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("When the host instances of this host are clustered, this property contains the cl" +
            "uster resource group name set by the Administrator.")]
        public string ClusterResourceGroupName {
            get {
                return (string)_curObj["ClusterResourceGroupName"];
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDbQueueSizeThresholdNull {
            get {
                if (_curObj["DBQueueSizeThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum number of items in the Database.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint DbQueueSizeThreshold {
            get {
                if (_curObj["DBQueueSizeThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["DBQueueSizeThreshold"];
            }
            set {
                _curObj["DBQueueSizeThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDbSessionThresholdNull {
            get {
                if (_curObj["DBSessionThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum number of DB Sessions (per CPU) allowed before throttling begins.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint DbSessionThreshold {
            get {
                if (_curObj["DBSessionThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["DBSessionThreshold"];
            }
            set {
                _curObj["DBSessionThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This is a comment field that allows to associate some friendly name with a decryp" +
            "tion certificate. Max length for this property is 256 characters.")]
        public string DecryptCertComment {
            get {
                return (string)_curObj["DecryptCertComment"];
            }
            set {
                _curObj["DecryptCertComment"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"ThumbPrint of the Decryption certificate. The Certificate Thumbprint is a digest of the certificate data and is found in the Certificate Details, and is expressed as a hexadecimal value. Example: 'FD36 90F0 EB49 F7B8 D3AB 1C69 8E02 ED84 5738 7868'. Max length for this property is 80 characters.")]
        public string DecryptCertThumbprint {
            get {
                return (string)_curObj["DecryptCertThumbprint"];
            }
            set {
                _curObj["DecryptCertThumbprint"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDeliveryQueueSizeNull {
            get {
                if (_curObj["DeliveryQueueSize"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Size of the in-memory Queue that the host maintains as a temporary placeholder fo" +
            "r delivering messages.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint DeliveryQueueSize {
            get {
                if (_curObj["DeliveryQueueSize"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["DeliveryQueueSize"];
            }
            set {
                _curObj["DeliveryQueueSize"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A description of the CIM_Setting object.")]
        public string Description {
            get {
                return (string)_curObj["Description"];
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsGlobalMemoryThresholdNull {
            get {
                if (_curObj["GlobalMemoryThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum System-wide Virtual Memory (in percent) usage allowed before throttling b" +
            "egins.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint GlobalMemoryThreshold {
            get {
                if (_curObj["GlobalMemoryThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["GlobalMemoryThreshold"];
            }
            set {
                _curObj["GlobalMemoryThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHostTrackingNull {
            get {
                if (_curObj["HostTracking"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates whether instances of this BizTalk Host will host the trac" +
            "king sub service.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool HostTracking {
            get {
                if (_curObj["HostTracking"] == null) {
                    return Convert.ToBoolean(0);
                }
                return (bool)_curObj["HostTracking"];
            }
            set {
                _curObj["HostTracking"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHostTypeNull {
            get {
                if (_curObj["HostType"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates which runtime model the instances of the BizTalk Host wil" +
            "l be running in.  This property is required for instance creation.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public HostTypeValues HostType {
            get {
                if (_curObj["HostType"] == null) {
                    return HostTypeValues.NullEnumValue;
                }
                return (HostTypeValues)Convert.ToInt32(_curObj["HostType"]);
            }

            set
            {
                _curObj["HostType"] = Convert.ToInt32(value);
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInflightMessageThresholdNull {
            get {
                if (_curObj["InflightMessageThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum number of in-memory in-flight messages allowed before throttling Message " +
            "Delivery begins.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint InflightMessageThreshold {
            get {
                if (_curObj["InflightMessageThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["InflightMessageThreshold"];
            }
            set {
                _curObj["InflightMessageThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIsDefaultNull {
            get {
                if (_curObj["IsDefault"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates whether the BizTalk Host represented by this WMI instance" +
            " is the default BizTalk Host in the BizTalk group.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool IsDefault {
            get {
                if (_curObj["IsDefault"] == null) {
                    return Convert.ToBoolean(0);
                }
                return (bool)_curObj["IsDefault"];
            }
            set {
                _curObj["IsDefault"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIsHost32BitOnlyNull {
            get {
                if (_curObj["IsHost32BitOnly"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates whether the host instance process should be created as 32" +
            "-bit on both 32-bit and 64-bit servers.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool IsHost32BitOnly {
            get {
                if (_curObj["IsHost32BitOnly"] == null) {
                    return Convert.ToBoolean(0);
                }
                return (bool)_curObj["IsHost32BitOnly"];
            }
            set {
                _curObj["IsHost32BitOnly"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains a default logon for the BizTalk Host instance creation UI." +
            " Max length for this property is 128 characters.")]
        public string LastUsedLogon {
            get {
                return (string)_curObj["LastUsedLogon"];
            }
            set {
                _curObj["LastUsedLogon"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessageDeliveryMaximumDelayNull {
            get {
                if (_curObj["MessageDeliveryMaximumDelay"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum Delay (in milliseconds) imposed for Message Delivery Throttling. Zero ind" +
            "icates disable Message Delivery Throttling.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessageDeliveryMaximumDelay {
            get {
                if (_curObj["MessageDeliveryMaximumDelay"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessageDeliveryMaximumDelay"];
            }
            set {
                _curObj["MessageDeliveryMaximumDelay"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessageDeliveryOverdriveFactorNull {
            get {
                if (_curObj["MessageDeliveryOverdriveFactor"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Percent factor by which the system will overdrive the Input rate for Message Deli" +
            "very Throttling.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessageDeliveryOverdriveFactor {
            get {
                if (_curObj["MessageDeliveryOverdriveFactor"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessageDeliveryOverdriveFactor"];
            }
            set {
                _curObj["MessageDeliveryOverdriveFactor"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessageDeliverySampleSpaceSizeNull {
            get {
                if (_curObj["MessageDeliverySampleSpaceSize"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates the number of samples that are used for determining the r" +
            "ate of the Message Delivery to all Service Classes of the Host.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessageDeliverySampleSpaceSize {
            get {
                if (_curObj["MessageDeliverySampleSpaceSize"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessageDeliverySampleSpaceSize"];
            }
            set {
                _curObj["MessageDeliverySampleSpaceSize"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessageDeliverySampleSpaceWindowNull {
            get {
                if (_curObj["MessageDeliverySampleSpaceWindow"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Time-window (in milliseconds) beyond which samples will be deemed invalid for con" +
            "sideration.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessageDeliverySampleSpaceWindow {
            get {
                if (_curObj["MessageDeliverySampleSpaceWindow"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessageDeliverySampleSpaceWindow"];
            }
            set {
                _curObj["MessageDeliverySampleSpaceWindow"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessagePublishMaximumDelayNull {
            get {
                if (_curObj["MessagePublishMaximumDelay"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum Delay (in milliseconds) imposed for Message Publishing Throttling. Zero i" +
            "ndicates disable Message Publishing Throttling.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessagePublishMaximumDelay {
            get {
                if (_curObj["MessagePublishMaximumDelay"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessagePublishMaximumDelay"];
            }
            set {
                _curObj["MessagePublishMaximumDelay"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessagePublishOverdriveFactorNull {
            get {
                if (_curObj["MessagePublishOverdriveFactor"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Percent Factor by which the system will overdrive the Input rate.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessagePublishOverdriveFactor {
            get {
                if (_curObj["MessagePublishOverdriveFactor"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessagePublishOverdriveFactor"];
            }
            set {
                _curObj["MessagePublishOverdriveFactor"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessagePublishSampleSpaceSizeNull {
            get {
                if (_curObj["MessagePublishSampleSpaceSize"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Number of samples that are used for determining the rate of the Message Publishin" +
            "g by the Service Classes.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessagePublishSampleSpaceSize {
            get {
                if (_curObj["MessagePublishSampleSpaceSize"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessagePublishSampleSpaceSize"];
            }
            set {
                _curObj["MessagePublishSampleSpaceSize"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMessagePublishSampleSpaceWindowNull {
            get {
                if (_curObj["MessagePublishSampleSpaceWindow"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Time-window (in milliseconds) beyond which samples will be deemed invalid for con" +
            "sideration.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MessagePublishSampleSpaceWindow {
            get {
                if (_curObj["MessagePublishSampleSpaceWindow"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["MessagePublishSampleSpaceWindow"];
            }
            set {
                _curObj["MessagePublishSampleSpaceWindow"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
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
        [Description("This property contains the name of the BizTalk Host.  This property is required f" +
            "or instance creation. Max length for this property is 80 characters.")]
        public string Name {
            get {
                return (string)_curObj["Name"];
                }

            set { _curObj["Name"] = value;
                }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property contains the name of the Windows group. It can be either a local or a domain Windows group. This group is granted access to the BizTalk Host Queue that is created for this BizTalk Host. All accounts used to host BizTalk Host instances of this type must be members of this group.  This property is required for instance creation. Max length for this property is 63 characters.")]
        public string NtGroupName {
            get {
                return (string)_curObj["NTGroupName"];
            }

            set
            {
                _curObj["NTGroupName"] = value;
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsProcessMemoryThresholdNull {
            get {
                if (_curObj["ProcessMemoryThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum Process Memory (in percent) allowed before throttling begins.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ProcessMemoryThreshold {
            get {
                if (_curObj["ProcessMemoryThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["ProcessMemoryThreshold"];
            }
            set {
                _curObj["ProcessMemoryThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The identifier by which the CIM_Setting object is known.")]
        public string SettingId {
            get {
                return (string)_curObj["SettingID"];
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsThreadPoolSizeNull {
            get {
                if (_curObj["ThreadPoolSize"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum number of messaging engine threads per CPU.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ThreadPoolSize {
            get {
                if (_curObj["ThreadPoolSize"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["ThreadPoolSize"];
            }
            set {
                _curObj["ThreadPoolSize"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsThreadThresholdNull {
            get {
                if (_curObj["ThreadThreshold"] == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Maximum number of threads in the process (per CPU) allowed before throttling begi" +
            "ns.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ThreadThreshold {
            get {
                if (_curObj["ThreadThreshold"] == null) {
                    return Convert.ToUInt32(0);
                }
                return (uint)_curObj["ThreadThreshold"];
            }
            set {
                _curObj["ThreadThreshold"] = value;
                if (_isEmbedded == false 
                    && _autoCommitProp) {
                    _privateLateBoundObject.Put();
                }
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
        
        private bool ShouldSerializeAuthTrusted() {
            if (IsAuthTrustedNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetAuthTrusted() {
            _curObj["AuthTrusted"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeDbQueueSizeThreshold() {
            if (IsDbQueueSizeThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetDbQueueSizeThreshold() {
            _curObj["DBQueueSizeThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeDbSessionThreshold() {
            if (IsDbSessionThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetDbSessionThreshold() {
            _curObj["DBSessionThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private void ResetDecryptCertComment() {
            _curObj["DecryptCertComment"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private void ResetDecryptCertThumbprint() {
            _curObj["DecryptCertThumbprint"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeDeliveryQueueSize() {
            if (IsDeliveryQueueSizeNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetDeliveryQueueSize() {
            _curObj["DeliveryQueueSize"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeGlobalMemoryThreshold() {
            if (IsGlobalMemoryThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetGlobalMemoryThreshold() {
            _curObj["GlobalMemoryThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeHostTracking() {
            if (IsHostTrackingNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetHostTracking() {
            _curObj["HostTracking"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeHostType() {
            if (IsHostTypeNull == false) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeInflightMessageThreshold() {
            if (IsInflightMessageThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetInflightMessageThreshold() {
            _curObj["InflightMessageThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeIsDefault() {
            if (IsIsDefaultNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetIsDefault() {
            _curObj["IsDefault"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeIsHost32BitOnly() {
            if (IsIsHost32BitOnlyNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetIsHost32BitOnly() {
            _curObj["IsHost32BitOnly"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private void ResetLastUsedLogon() {
            _curObj["LastUsedLogon"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessageDeliveryMaximumDelay() {
            if (IsMessageDeliveryMaximumDelayNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessageDeliveryMaximumDelay() {
            _curObj["MessageDeliveryMaximumDelay"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessageDeliveryOverdriveFactor() {
            if (IsMessageDeliveryOverdriveFactorNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessageDeliveryOverdriveFactor() {
            _curObj["MessageDeliveryOverdriveFactor"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessageDeliverySampleSpaceSize() {
            if (IsMessageDeliverySampleSpaceSizeNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessageDeliverySampleSpaceSize() {
            _curObj["MessageDeliverySampleSpaceSize"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessageDeliverySampleSpaceWindow() {
            if (IsMessageDeliverySampleSpaceWindowNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessageDeliverySampleSpaceWindow() {
            _curObj["MessageDeliverySampleSpaceWindow"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessagePublishMaximumDelay() {
            if (IsMessagePublishMaximumDelayNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessagePublishMaximumDelay() {
            _curObj["MessagePublishMaximumDelay"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessagePublishOverdriveFactor() {
            if (IsMessagePublishOverdriveFactorNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessagePublishOverdriveFactor() {
            _curObj["MessagePublishOverdriveFactor"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessagePublishSampleSpaceSize() {
            if (IsMessagePublishSampleSpaceSizeNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessagePublishSampleSpaceSize() {
            _curObj["MessagePublishSampleSpaceSize"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMessagePublishSampleSpaceWindow() {
            if (IsMessagePublishSampleSpaceWindowNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetMessagePublishSampleSpaceWindow() {
            _curObj["MessagePublishSampleSpaceWindow"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeProcessMemoryThreshold() {
            if (IsProcessMemoryThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetProcessMemoryThreshold() {
            _curObj["ProcessMemoryThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeThreadPoolSize() {
            if (IsThreadPoolSizeNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetThreadPoolSize() {
            _curObj["ThreadPoolSize"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeThreadThreshold() {
            if (IsThreadThresholdNull == false) {
                return true;
            }
            return false;
        }
        
        private void ResetThreadThreshold() {
            _curObj["ThreadThreshold"] = null;
            if (_isEmbedded == false 
                && _autoCommitProp) {
                _privateLateBoundObject.Put();
            }
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
        
        private static string ConstructPath(string keyMgmtDbNameOverride, string keyMgmtDbServerOverride, string keyName) {
            string strPath = "ROOT\\MicrosoftBizTalkServer:MSBTS_HostSetting";
            strPath = string.Concat(strPath, string.Concat(".MgmtDbNameOverride=", string.Concat("\"", string.Concat(keyMgmtDbNameOverride, "\""))));
            strPath = string.Concat(strPath, string.Concat(",MgmtDbServerOverride=", string.Concat("\"", string.Concat(keyMgmtDbServerOverride, "\""))));
            strPath = string.Concat(strPath, string.Concat(",Name=", string.Concat("\"", string.Concat(keyName, "\""))));
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
        public static IEnumerable<HostSetting> GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static IEnumerable<HostSetting> GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static IEnumerable<HostSetting> GetInstances(String [] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static IEnumerable<HostSetting> GetInstances(string condition, String [] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static IEnumerable<HostSetting> GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions) {
            if (mgmtScope == null)
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\MicrosoftBizTalkServer"}};
            }
            ManagementPath pathObj = new ManagementPath
            {
                ClassName = "MSBTS_HostSetting",
                NamespacePath = "root\\MicrosoftBizTalkServer"
            };
            using (ManagementClass clsObject = new ManagementClass(mgmtScope, pathObj, null))
            {
                if (enumOptions == null) {
                    enumOptions = new EnumerationOptions {EnsureLocatable = true};
                }
                return clsObject.GetInstances(enumOptions).Cast<HostSetting>();
            }
        }
        
        public static IEnumerable<HostSetting> GetInstances(ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static IEnumerable<HostSetting> GetInstances(ManagementScope mgmtScope, String [] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static IEnumerable<HostSetting> GetInstances(ManagementScope mgmtScope, string condition, String [] selectedProperties) {
            if (mgmtScope == null)
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\MicrosoftBizTalkServer"}};
            }
            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(mgmtScope,
                new SelectQuery("MSBTS_HostSetting", condition, selectedProperties),
                new EnumerationOptions {EnsureLocatable = true}))
            {
                return objectSearcher.Get().Cast<HostSetting>();
            }
        }
        
        [Browsable(true)]
        public static HostSetting CreateInstance() {
            var mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = CreatedWmiNamespace}};
            ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
            using (ManagementClass tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null))
            {
                return new HostSetting(tmpMgmtClass.CreateInstance());
            }
        }
        [Browsable(true)]
        public static HostSetting CreateInstance(string pServer, string pUserName,string pPassword, string pDomain)
        {
            var mgmtScope = _statMgmtScope ?? new ManagementScope
            {
                Path = {NamespacePath = "\\\\" + pServer + CreatedWmiNamespace},
                Options = new ConnectionOptions
                {
                    Username = pUserName,
                    Password = pPassword,
                    Authority = "ntlmdomain:" + pDomain
                }
            };
            ManagementPath mgmtPath = new ManagementPath(CreatedClassName);
            using (ManagementClass tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null))
            {
                return new HostSetting(tmpMgmtClass.CreateInstance());
            }
        }
        
        [Browsable(true)]
        public void Delete() {
            _privateLateBoundObject.Delete();
        }
        
        public enum HostTypeValues {
            
            InProcess = 1,
            
            Isolated = 2,
            
            NullEnumValue = 0,
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
                if (_baseType.BaseType == typeof(Enum))
                {
                    return value.GetType() == destinationType
                        ? value
                        : _baseConverter.ConvertTo(context, culture, value, destinationType);
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
