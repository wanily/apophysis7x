﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Xyrus.Apophysis.Settings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    public sealed partial class EditorSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static EditorSettings defaultInstance = ((EditorSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new EditorSettings())));
        
        public static EditorSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.5")]
        public double MoveDistance {
            get {
                return ((double)(this["MoveDistance"]));
            }
            set {
                this["MoveDistance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public double RotateAngle {
            get {
                return ((double)(this["RotateAngle"]));
            }
            set {
                this["RotateAngle"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("125")]
        public double ScaleRatio {
            get {
                return ((double)(this["ScaleRatio"]));
            }
            set {
                this["ScaleRatio"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool LockAxes {
            get {
                return ((bool)(this["LockAxes"]));
            }
            set {
                this["LockAxes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowRulers {
            get {
                return ((bool)(this["ShowRulers"]));
            }
            set {
                this["ShowRulers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowVariationPreview {
            get {
                return ((bool)(this["ShowVariationPreview"]));
            }
            set {
                this["ShowVariationPreview"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoZoom {
            get {
                return ((bool)(this["AutoZoom"]));
            }
            set {
                this["AutoZoom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public double VariationPreviewDensity {
            get {
                return ((double)(this["VariationPreviewDensity"]));
            }
            set {
                this["VariationPreviewDensity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public double VariationPreviewRange {
            get {
                return ((double)(this["VariationPreviewRange"]));
            }
            set {
                this["VariationPreviewRange"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool VariationPreviewApplyPostTransform {
            get {
                return ((bool)(this["VariationPreviewApplyPostTransform"]));
            }
            set {
                this["VariationPreviewApplyPostTransform"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int CameraEditMode {
            get {
                return ((int)(this["CameraEditMode"]));
            }
            set {
                this["CameraEditMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CameraEditUseScale {
            get {
                return ((bool)(this["CameraEditUseScale"]));
            }
            set {
                this["CameraEditUseScale"] = value;
            }
        }
    }
}