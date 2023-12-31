﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace WS_POS_web.Ws_ValidateCreaAccount_PRD {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MetodosBilleteraSoap", Namespace="http://tempuri.org/")]
    public partial class MetodosBilletera : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ValidaSocioOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidaPingOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MetodosBilletera() {
            this.Url = global::WS_POS_web.Properties.Settings.Default.WS_POS_web_Ws_ValidateCreaAccount_PRD_MetodosBilletera;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ValidaSocioCompletedEventHandler ValidaSocioCompleted;
        
        /// <remarks/>
        public event ValidaPingCompletedEventHandler ValidaPingCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidaSocio", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] ValidaSocio(string sIdentificador, decimal sValor, string sReferencia, string sDispositivo, string sPlataforma, string sIpDispositivo, string sCodigoUsuario, string Almacen) {
            object[] results = this.Invoke("ValidaSocio", new object[] {
                        sIdentificador,
                        sValor,
                        sReferencia,
                        sDispositivo,
                        sPlataforma,
                        sIpDispositivo,
                        sCodigoUsuario,
                        Almacen});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void ValidaSocioAsync(string sIdentificador, decimal sValor, string sReferencia, string sDispositivo, string sPlataforma, string sIpDispositivo, string sCodigoUsuario, string Almacen) {
            this.ValidaSocioAsync(sIdentificador, sValor, sReferencia, sDispositivo, sPlataforma, sIpDispositivo, sCodigoUsuario, Almacen, null);
        }
        
        /// <remarks/>
        public void ValidaSocioAsync(string sIdentificador, decimal sValor, string sReferencia, string sDispositivo, string sPlataforma, string sIpDispositivo, string sCodigoUsuario, string Almacen, object userState) {
            if ((this.ValidaSocioOperationCompleted == null)) {
                this.ValidaSocioOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidaSocioOperationCompleted);
            }
            this.InvokeAsync("ValidaSocio", new object[] {
                        sIdentificador,
                        sValor,
                        sReferencia,
                        sDispositivo,
                        sPlataforma,
                        sIpDispositivo,
                        sCodigoUsuario,
                        Almacen}, this.ValidaSocioOperationCompleted, userState);
        }
        
        private void OnValidaSocioOperationCompleted(object arg) {
            if ((this.ValidaSocioCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidaSocioCompleted(this, new ValidaSocioCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidaPing", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] ValidaPing(string sReferencia, int sPing, string sIpDispositivo, string sCodigoUsuario, string Almacen) {
            object[] results = this.Invoke("ValidaPing", new object[] {
                        sReferencia,
                        sPing,
                        sIpDispositivo,
                        sCodigoUsuario,
                        Almacen});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void ValidaPingAsync(string sReferencia, int sPing, string sIpDispositivo, string sCodigoUsuario, string Almacen) {
            this.ValidaPingAsync(sReferencia, sPing, sIpDispositivo, sCodigoUsuario, Almacen, null);
        }
        
        /// <remarks/>
        public void ValidaPingAsync(string sReferencia, int sPing, string sIpDispositivo, string sCodigoUsuario, string Almacen, object userState) {
            if ((this.ValidaPingOperationCompleted == null)) {
                this.ValidaPingOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidaPingOperationCompleted);
            }
            this.InvokeAsync("ValidaPing", new object[] {
                        sReferencia,
                        sPing,
                        sIpDispositivo,
                        sCodigoUsuario,
                        Almacen}, this.ValidaPingOperationCompleted, userState);
        }
        
        private void OnValidaPingOperationCompleted(object arg) {
            if ((this.ValidaPingCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidaPingCompleted(this, new ValidaPingCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ValidaSocioCompletedEventHandler(object sender, ValidaSocioCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidaSocioCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidaSocioCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ValidaPingCompletedEventHandler(object sender, ValidaPingCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidaPingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidaPingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591