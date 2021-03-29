using System;

namespace DocuShareIndexingWorker.Entities
{
    public class DeclarationMessageResponse
    {
        public string metaPath {get; set;}
        public string title {get;set;}
        public string shipmentType {get;set;}
        public string branchCode {get;set;}
        public string refNo {get;set;}
        public string decNo {get;set;}
        public string commercialInvoices {get;set;}
        public string cmpTaxNo {get;set;}
        public string cmpBranch {get;set;}
        public string cmpName {get;set;}
        public string vesselName {get;set;}
        public string voyNumber {get;set;}
        public string masterBL {get;set;}
        public string houseBL {get;set;}
        public string destCountry {get;set;}
        public string deptCountry {get;set;}
        public DateTime eta {get;set;}
        public DateTime etd {get;set;}
        public DateTime uDateDeclare {get;set;}
        public DateTime uDateRelease {get;set;}
        public string materialType {get;set;}
        public string period {get;set;}

    }
}