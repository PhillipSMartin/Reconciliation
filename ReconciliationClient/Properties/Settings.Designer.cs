﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReconciliationClient.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WSB820TT.CST138PO.CSV")]
        public string PositionFileName {
            get {
                return ((string)(this["PositionFileName"]));
            }
            set {
                this["PositionFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WSB821TT.CST138TR.CSV")]
        public string ConfirmationFileName {
            get {
                return ((string)(this["ConfirmationFileName"]));
            }
            set {
                this["ConfirmationFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\gargoyle-mw05\\downloads")]
        public string ImportDirectory {
            get {
                return ((string)(this["ImportDirectory"]));
            }
            set {
                this["ImportDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("JNikolova")]
        public string TraderName {
            get {
                return ((string)(this["TraderName"]));
            }
            set {
                this["TraderName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WSB822TT.CST138BK.CSV")]
        public string BookkeepingFileName {
            get {
                return ((string)(this["BookkeepingFileName"]));
            }
            set {
                this["BookkeepingFileName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ReconciliationClient")]
        public string ApplicationName {
            get {
                return ((string)(this["ApplicationName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Hugo")]
        public string Database {
            get {
                return ((string)(this["Database"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("jnikolova@gargoylestrategic.com")]
        public string MailFrom {
            get {
                return ((string)(this["MailFrom"]));
            }
            set {
                this["MailFrom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MLAMEXBREAKS@win.ml.com,jnikolova@gargoylestrategic.com")]
        public string MailTo {
            get {
                return ((string)(this["MailTo"]));
            }
            set {
                this["MailTo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Gargoyle corrections")]
        public string MailSubject {
            get {
                return ((string)(this["MailSubject"]));
            }
            set {
                this["MailSubject"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\temp")]
        public string ReportsDirectory {
            get {
                return ((string)(this["ReportsDirectory"]));
            }
            set {
                this["ReportsDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UpgradeSettings {
            get {
                return ((bool)(this["UpgradeSettings"]));
            }
            set {
                this["UpgradeSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WST423TM.CST13ASG.CSV")]
        public string AssignmentsFileName {
            get {
                return ((string)(this["AssignmentsFileName"]));
            }
            set {
                this["AssignmentsFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SendReportToTraders {
            get {
                return ((bool)(this["SendReportToTraders"]));
            }
            set {
                this["SendReportToTraders"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DebugMode {
            get {
                return ((bool)(this["DebugMode"]));
            }
            set {
                this["DebugMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ConsolidateStocks {
            get {
                return ((bool)(this["ConsolidateStocks"]));
            }
            set {
                this["ConsolidateStocks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ConsolidateOptions {
            get {
                return ((bool)(this["ConsolidateOptions"]));
            }
            set {
                this["ConsolidateOptions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("POTL_05212012.POTL")]
        public string TaxlotsFileName {
            get {
                return ((string)(this["TaxlotsFileName"]));
            }
            set {
                this["TaxlotsFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("server=172.20.151.21;userid=gargoyle;password=3CoNeOTo;database=gargoyle")]
        public string LiquidConnectionString {
            get {
                return ((string)(this["LiquidConnectionString"]));
            }
            set {
                this["LiquidConnectionString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("F1957488.Corporate_Actions.20180426.csv")]
        public string DividendsFileName {
            get {
                return ((string)(this["DividendsFileName"]));
            }
            set {
                this["DividendsFileName"] = value;
            }
        }
    }
}