namespace WebStudy.Services
{
    using DevExpress.XtraReports.UI;
    using DevExpress.XtraReports.Web.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    public class ReportStorage : ReportStorageWebExtension
    {
        public override bool IsValidUrl(string url)
        {
            return false;
        }

        public override bool CanSetData(string url)
        {
            return false;
        }

        public override void SetData(XtraReport report, Stream stream)
        {

        }

        public override void SetData(XtraReport report, string url)
        {

        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            return "";
        }

        public override byte[] GetData(string url)
        {
            return null;
        }

        public override Dictionary<string, string> GetUrls()
        {
            return null;
        }
    }
}


//https://docs.devexpress.com/XtraReports/400042/create-end-user-reporting-applications/web-reporting/asp-net-core-reporting/end-user-report-designer/quick-start/add-an-end-user-report-designer-to-an-asp.net-core-application?v=18.1