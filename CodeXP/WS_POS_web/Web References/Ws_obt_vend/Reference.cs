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

namespace WS_POS_web.Ws_obt_vend {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    // CODEGEN: No se controló el elemento de extensión WSDL opcional 'Policy' del espacio de nombres 'http://schemas.xmlsoap.org/ws/2004/09/policy'.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ZSDWS_POS_CONSULTA_VENDEDORES_BINDING", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZSDWS_POS_CONSULTA_VENDEDORES : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ZsdrfcPosConsultaVendedoresOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ZSDWS_POS_CONSULTA_VENDEDORES() {
            this.Url = global::WS_POS_web.Properties.Settings.Default.WS_POS_web_Ws_obt_vend_ZSDWS_POS_CONSULTA_VENDEDORES;
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
        public event ZsdrfcPosConsultaVendedoresCompletedEventHandler ZsdrfcPosConsultaVendedoresCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="urn:sap-com:document:sap:soap:functions:mc-style", ResponseNamespace="urn:sap-com:document:sap:soap:functions:mc-style", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ZsdrfcPosConsultaVendedores([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string PIcnum) {
            object[] results = this.Invoke("ZsdrfcPosConsultaVendedores", new object[] {
                        PIcnum});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ZsdrfcPosConsultaVendedoresAsync(string PIcnum) {
            this.ZsdrfcPosConsultaVendedoresAsync(PIcnum, null);
        }
        
        /// <remarks/>
        public void ZsdrfcPosConsultaVendedoresAsync(string PIcnum, object userState) {
            if ((this.ZsdrfcPosConsultaVendedoresOperationCompleted == null)) {
                this.ZsdrfcPosConsultaVendedoresOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZsdrfcPosConsultaVendedoresOperationCompleted);
            }
            this.InvokeAsync("ZsdrfcPosConsultaVendedores", new object[] {
                        PIcnum}, this.ZsdrfcPosConsultaVendedoresOperationCompleted, userState);
        }
        
        private void OnZsdrfcPosConsultaVendedoresOperationCompleted(object arg) {
            if ((this.ZsdrfcPosConsultaVendedoresCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZsdrfcPosConsultaVendedoresCompleted(this, new ZsdrfcPosConsultaVendedoresCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ZsdrfcPosConsultaVendedoresCompletedEventHandler(object sender, ZsdrfcPosConsultaVendedoresCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZsdrfcPosConsultaVendedoresCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZsdrfcPosConsultaVendedoresCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591