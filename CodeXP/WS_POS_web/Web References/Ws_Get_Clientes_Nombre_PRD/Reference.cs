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

namespace WS_POS_web.Ws_Get_Clientes_Nombre_PRD {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    // CODEGEN: No se controló el elemento de extensión WSDL opcional 'Policy' del espacio de nombres 'http://schemas.xmlsoap.org/ws/2004/09/policy'.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ZDS_BUSCAR_CLIE_NOM_BINDING", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZDS_BUSCAR_CLIE_NOM : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ZsdrfcConClieNomOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ZDS_BUSCAR_CLIE_NOM() {
            this.Url = global::WS_POS_web.Properties.Settings.Default.WS_POS_web_Ws_Get_Clientes_Nombre_PRD_ZDS_BUSCAR_CLIE_NOM;
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
        public event ZsdrfcConClieNomCompletedEventHandler ZsdrfcConClieNomCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="urn:sap-com:document:sap:soap:functions:mc-style", ResponseNamespace="urn:sap-com:document:sap:soap:functions:mc-style", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("Resultado", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZestResultadoClientes[] ZsdrfcConClieNom([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string PNomcli) {
            object[] results = this.Invoke("ZsdrfcConClieNom", new object[] {
                        PNomcli});
            return ((ZestResultadoClientes[])(results[0]));
        }
        
        /// <remarks/>
        public void ZsdrfcConClieNomAsync(string PNomcli) {
            this.ZsdrfcConClieNomAsync(PNomcli, null);
        }
        
        /// <remarks/>
        public void ZsdrfcConClieNomAsync(string PNomcli, object userState) {
            if ((this.ZsdrfcConClieNomOperationCompleted == null)) {
                this.ZsdrfcConClieNomOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZsdrfcConClieNomOperationCompleted);
            }
            this.InvokeAsync("ZsdrfcConClieNom", new object[] {
                        PNomcli}, this.ZsdrfcConClieNomOperationCompleted, userState);
        }
        
        private void OnZsdrfcConClieNomOperationCompleted(object arg) {
            if ((this.ZsdrfcConClieNomCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZsdrfcConClieNomCompleted(this, new ZsdrfcConClieNomCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZestResultadoClientes {
        
        private string cedulaField;
        
        private string nombreField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Cedula {
            get {
                return this.cedulaField;
            }
            set {
                this.cedulaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ZsdrfcConClieNomCompletedEventHandler(object sender, ZsdrfcConClieNomCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZsdrfcConClieNomCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZsdrfcConClieNomCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ZestResultadoClientes[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ZestResultadoClientes[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591