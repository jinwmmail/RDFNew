using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FineUI;
using System.IO;
using FastReport;

namespace RDFNew.Web.Admin.Pur.PoM
{
    public partial class Pur_PoMPrint : App_Com.PrintBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Pur_PoM";
        }

        protected void Page_Load(object sender, EventArgs e)
        {        
            if (!IsPostBack)
            {
                string RptName = "";
                string contendPath = System.Web.HttpContext.Current.Request.MapPath("");
                RptName = contendPath + "\\Pur_PoM.aspx.frx";
                this.WebReport1.ReportFile = RptName;
                this.WebReport1.StartReport += new EventHandler(WebReport1_StartReport);
            }
        }

        void WebReport1_StartReport(object sender, EventArgs e)
        {
            Report FReport = (sender as FastReport.Web.WebReport).Report;
            FReport.RegisterData(GetMaster(), "Table");
            FReport.RegisterData(GetDetail(), "Detail");
        }

        DataTable GetMaster()
        {
            RDFNew.Module.Admin.Pur.Pur_PoM obj = new RDFNew.Module.Admin.Pur.Pur_PoM();
            DataTable dt = obj.GetMaster(B_Keyword)[1] as DataTable;            
            return dt;
        }

        DataTable GetDetail()
        {
            RDFNew.Module.Admin.Pur.Pur_PoD obj = new RDFNew.Module.Admin.Pur.Pur_PoD();
            DataTable dt = obj.GetDataByParent(B_Keyword)[1] as DataTable;
            if (dt.Rows.Count == 0)
                dt.Rows.Add(dt.NewRow());
            return dt;
        }
    }
}
