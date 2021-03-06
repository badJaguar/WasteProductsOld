﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WasteProducts.Logic.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UserResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WasteProducts.Logic.Resources.UserResources", typeof(UserResources).Assembly);
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
        ///   Looks up a localized string similar to На нашем сайте была оставлена заявка на регистрацию аккаунта, привязанного к этому емейлу. Если вы не регистрировались на нашем сайте, просим вас проигнорировать это письмо. Иначе, для того, чтобы завершить регистрацию, вы должны подтвердить свой email. Для этого в форме регистрации введите код, указанный ниже.
        ///
        ///Код подтверждения email:
        ///{0}.
        /// </summary>
        internal static string EmailConfirmationBody {
            get {
                return ResourceManager.GetString("EmailConfirmationBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Подтверждение регистрации на WasteProducts.
        /// </summary>
        internal static string EmailConfirmationHeader {
            get {
                return ResourceManager.GetString("EmailConfirmationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to На нашем сайте был сделан запрос на изменение электронной почты аккаунта на этот email. Если вы не делали этого запроса, просим вас проигнорировать это письмо. Иначе, для того, чтобы изменить email, на странице настроек аккаунта вы должны ввести код, указанный ниже.
        ///
        ///Код подтверждения смены email:
        ///{0}.
        /// </summary>
        internal static string NewEmailConfirmationBody {
            get {
                return ResourceManager.GetString("NewEmailConfirmationBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Подтверждение нового email на WasteProducts.
        /// </summary>
        internal static string NewEmailConfirmationHeader {
            get {
                return ResourceManager.GetString("NewEmailConfirmationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to На нашем сайте был сделан запрос на восстановление пароля аккаунта, привязанного к этому email. Если вы не делали этого запроса, просим вас проигнорировать это письмо. Иначе, для того, чтобы восстановить пароль, на странице запроса восстановления пароля вы должны ввести код, указанный ниже, и там же ввести новый пароль.
        ///
        ///Код подтверждения восстановления пароля:
        ///{0}.
        /// </summary>
        internal static string ResetPasswordBody {
            get {
                return ResourceManager.GetString("ResetPasswordBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Запрос на восстановление пароля.
        /// </summary>
        internal static string ResetPasswordHeader {
            get {
                return ResourceManager.GetString("ResetPasswordHeader", resourceCulture);
            }
        }
    }
}
