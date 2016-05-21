﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Json.Schema {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Json.Schema.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema does not define a property named &quot;{0}&quot;, and the schema does not permit additional properties..
        /// </summary>
        internal static string ErrorAdditionalPropertiesProhibited {
            get {
                return ResourceManager.GetString("ErrorAdditionalPropertiesProhibited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This schema does not contain the sub-schema definition referred to by the $ref value &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorDefinitionDoesNotExist {
            get {
                return ResourceManager.GetString("ErrorDefinitionDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON schema requires the value of &quot;additionalProperties&quot; to be either a Boolean or a schema, but a token of type {0} was seen..
        /// </summary>
        internal static string ErrorInvalidAdditionalProperties {
            get {
                return ResourceManager.GetString("ErrorInvalidAdditionalProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value &apos;{0}&apos; does not match any of the enum values &apos;{1}&apos;..
        /// </summary>
        internal static string ErrorInvalidEnumValue {
            get {
                return ResourceManager.GetString("ErrorInvalidEnumValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instance did not satisfy all of the {0} schemas specified by &quot;allOf&quot; as required by the schema..
        /// </summary>
        internal static string ErrorNotAllOf {
            get {
                return ResourceManager.GetString("ErrorNotAllOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is not a multiple of {1} as required by the schema..
        /// </summary>
        internal static string ErrorNotAMultiple {
            get {
                return ResourceManager.GetString("ErrorNotAMultiple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instance did not satisfy any of the {0} schemas specified by &quot;anyOf&quot; as required by the schema..
        /// </summary>
        internal static string ErrorNotAnyOf {
            get {
                return ResourceManager.GetString("ErrorNotAnyOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON schema requires the value of the property &quot;{0}&quot; to be a string, but the token is of type {1}..
        /// </summary>
        internal static string ErrorNotAString {
            get {
                return ResourceManager.GetString("ErrorNotAString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instance satisfied {0} of the {1} schemas specified by &quot;oneOf&quot;, instead of satisfying exactly one of them as required by the schema..
        /// </summary>
        internal static string ErrorNotOneOf {
            get {
                return ResourceManager.GetString("ErrorNotOneOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The elements of the are are not unique, as required by the schema..
        /// </summary>
        internal static string ErrorNotUnique {
            get {
                return ResourceManager.GetString("ErrorNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This partial implementation of JSON Schema only accepts $ref values in the form of fragments that refer to sub-properties of the &quot;definitions&quot; property, for example &quot;#/definitions/def1&quot;. The URI reference &quot;{0}&quot; is not supported..
        /// </summary>
        internal static string ErrorOnlyDefinitionFragmentsSupported {
            get {
                return ResourceManager.GetString("ErrorOnlyDefinitionFragmentsSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The required property &quot;{0}&quot; is missing..
        /// </summary>
        internal static string ErrorRequiredPropertyMissing {
            get {
                return ResourceManager.GetString("ErrorRequiredPropertyMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string &apos;{0}&apos; does not match the regular expression &apos;{1}&apos; as required by the schema..
        /// </summary>
        internal static string ErrorStringDoesNotMatchPattern {
            get {
                return ResourceManager.GetString("ErrorStringDoesNotMatchPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string &apos;{0}&apos; has length {1}, which is greater than the maximum length {2} permitted by the schema.
        ///.
        /// </summary>
        internal static string ErrorStringTooLong {
            get {
                return ResourceManager.GetString("ErrorStringTooLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string &apos;{0}&apos; has length {1}, which is less than the minimum length {2} permitted by the schema..
        /// </summary>
        internal static string ErrorStringTooShort {
            get {
                return ResourceManager.GetString("ErrorStringTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema requires at least {0} array items, but there are only {1}..
        /// </summary>
        internal static string ErrorTooFewArrayItems {
            get {
                return ResourceManager.GetString("ErrorTooFewArrayItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema requires at least {0} object properties, but there are only {1}..
        /// </summary>
        internal static string ErrorTooFewProperties {
            get {
                return ResourceManager.GetString("ErrorTooFewProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema permits at most {0} array items, but there are {1}..
        /// </summary>
        internal static string ErrorTooManyArrayItems {
            get {
                return ResourceManager.GetString("ErrorTooManyArrayItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema permits at most {0} object properties, but there are {1}..
        /// </summary>
        internal static string ErrorTooManyProperties {
            get {
                return ResourceManager.GetString("ErrorTooManyProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is greater than the maximum value of {1} permitted by the schema..
        /// </summary>
        internal static string ErrorValueTooLarge {
            get {
                return ResourceManager.GetString("ErrorValueTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is greater than or equal to the exclusive maximum value of {1} permitted by the schema..
        /// </summary>
        internal static string ErrorValueTooLargeExclusive {
            get {
                return ResourceManager.GetString("ErrorValueTooLargeExclusive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is less than the minimum value of {1} permitted by the schema..
        /// </summary>
        internal static string ErrorValueTooSmall {
            get {
                return ResourceManager.GetString("ErrorValueTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} is less than or equal to the exclusive minimum value of {1} permitted by the schema..
        /// </summary>
        internal static string ErrorValueTooSmallExclusive {
            get {
                return ResourceManager.GetString("ErrorValueTooSmallExclusive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ({0},{1}): error {2}: {3}.
        /// </summary>
        internal static string ErrorWithLineInfo {
            get {
                return ResourceManager.GetString("ErrorWithLineInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schema requires &apos;{0}&apos; to be one of the types [{1}], but the schema validator found a token of type {2}..
        /// </summary>
        internal static string ErrorWrongType {
            get {
                return ResourceManager.GetString("ErrorWrongType", resourceCulture);
            }
        }
    }
}
