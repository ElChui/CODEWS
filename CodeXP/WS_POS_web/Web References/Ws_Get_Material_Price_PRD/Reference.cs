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

namespace WS_POS_web.Ws_Get_Material_Price_PRD {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ZWS_SDNETPR0_BINDING", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZWS_SDNETPR0 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ZRfcSdnetpr0OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ZWS_SDNETPR0() {
            this.Url = global::WS_POS_web.Properties.Settings.Default.WS_POS_web_Ws_Get_Material__Price_PRD_ZWS_SDNETPR0;
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
        public event ZRfcSdnetpr0CompletedEventHandler ZRfcSdnetpr0Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="urn:sap-com:document:sap:soap:functions:mc-style", ResponseNamespace="urn:sap-com:document:sap:soap:functions:mc-style", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("EBapireturnT", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Bapiret1[] ZRfcSdnetpr0([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string IKunnr, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] decimal IKwmeng, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string ILgort, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string IMatnr, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string ISpart, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string IVkbur, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string IVkorg, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string IVtweg, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] out Zststockprice EStockpriceS, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] out int ESubrc) {
            object[] results = this.Invoke("ZRfcSdnetpr0", new object[] {
                        IKunnr,
                        IKwmeng,
                        ILgort,
                        IMatnr,
                        ISpart,
                        IVkbur,
                        IVkorg,
                        IVtweg});
            EStockpriceS = ((Zststockprice)(results[1]));
            ESubrc = ((int)(results[2]));
            return ((Bapiret1[])(results[0]));
        }
        
        /// <remarks/>
        public void ZRfcSdnetpr0Async(string IKunnr, decimal IKwmeng, string ILgort, string IMatnr, string ISpart, string IVkbur, string IVkorg, string IVtweg) {
            this.ZRfcSdnetpr0Async(IKunnr, IKwmeng, ILgort, IMatnr, ISpart, IVkbur, IVkorg, IVtweg, null);
        }
        
        /// <remarks/>
        public void ZRfcSdnetpr0Async(string IKunnr, decimal IKwmeng, string ILgort, string IMatnr, string ISpart, string IVkbur, string IVkorg, string IVtweg, object userState) {
            if ((this.ZRfcSdnetpr0OperationCompleted == null)) {
                this.ZRfcSdnetpr0OperationCompleted = new System.Threading.SendOrPostCallback(this.OnZRfcSdnetpr0OperationCompleted);
            }
            this.InvokeAsync("ZRfcSdnetpr0", new object[] {
                        IKunnr,
                        IKwmeng,
                        ILgort,
                        IMatnr,
                        ISpart,
                        IVkbur,
                        IVkorg,
                        IVtweg}, this.ZRfcSdnetpr0OperationCompleted, userState);
        }
        
        private void OnZRfcSdnetpr0OperationCompleted(object arg) {
            if ((this.ZRfcSdnetpr0Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZRfcSdnetpr0Completed(this, new ZRfcSdnetpr0CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Bapiret1 {
        
        private string typeField;
        
        private string idField;
        
        private string numberField;
        
        private string messageField;
        
        private string logNoField;
        
        private string logMsgNoField;
        
        private string messageV1Field;
        
        private string messageV2Field;
        
        private string messageV3Field;
        
        private string messageV4Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Number {
            get {
                return this.numberField;
            }
            set {
                this.numberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LogNo {
            get {
                return this.logNoField;
            }
            set {
                this.logNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LogMsgNo {
            get {
                return this.logMsgNoField;
            }
            set {
                this.logMsgNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV1 {
            get {
                return this.messageV1Field;
            }
            set {
                this.messageV1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV2 {
            get {
                return this.messageV2Field;
            }
            set {
                this.messageV2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV3 {
            get {
                return this.messageV3Field;
            }
            set {
                this.messageV3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV4 {
            get {
                return this.messageV4Field;
            }
            set {
                this.messageV4Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Zststockprice {
        
        private string matnrField;
        
        private string arktxField;
        
        private decimal fkimgField;
        
        private string vrkmeField;
        
        private decimal netwrField;
        
        private decimal mwsbpField;
        
        private string waerkField;
        
        private string taxm1Field;
        
        private string pstyvField;
        
        private string werksField;
        
        private string vkorgField;
        
        private string vtwegField;
        
        private string spartField;
        
        private string vkburField;
        
        private decimal labstField;
        
        private string meinsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Matnr {
            get {
                return this.matnrField;
            }
            set {
                this.matnrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Arktx {
            get {
                return this.arktxField;
            }
            set {
                this.arktxField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal Fkimg {
            get {
                return this.fkimgField;
            }
            set {
                this.fkimgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vrkme {
            get {
                return this.vrkmeField;
            }
            set {
                this.vrkmeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal Netwr {
            get {
                return this.netwrField;
            }
            set {
                this.netwrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal Mwsbp {
            get {
                return this.mwsbpField;
            }
            set {
                this.mwsbpField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Waerk {
            get {
                return this.waerkField;
            }
            set {
                this.waerkField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Taxm1 {
            get {
                return this.taxm1Field;
            }
            set {
                this.taxm1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Pstyv {
            get {
                return this.pstyvField;
            }
            set {
                this.pstyvField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Werks {
            get {
                return this.werksField;
            }
            set {
                this.werksField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vkorg {
            get {
                return this.vkorgField;
            }
            set {
                this.vkorgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vtweg {
            get {
                return this.vtwegField;
            }
            set {
                this.vtwegField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Spart {
            get {
                return this.spartField;
            }
            set {
                this.spartField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vkbur {
            get {
                return this.vkburField;
            }
            set {
                this.vkburField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal Labst {
            get {
                return this.labstField;
            }
            set {
                this.labstField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Meins {
            get {
                return this.meinsField;
            }
            set {
                this.meinsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ZRfcSdnetpr0CompletedEventHandler(object sender, ZRfcSdnetpr0CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZRfcSdnetpr0CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZRfcSdnetpr0CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Bapiret1[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Bapiret1[])(this.results[0]));
            }
        }
        
        /// <remarks/>
        public Zststockprice EStockpriceS {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Zststockprice)(this.results[1]));
            }
        }
        
        /// <remarks/>
        public int ESubrc {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[2]));
            }
        }
    }
}

#pragma warning restore 1591