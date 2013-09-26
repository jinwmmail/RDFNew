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

namespace RDFNew.Web.Admin.Sys.Function
{
    public partial class Sys_FunctionListPrint : App_Com.PrintBase
    {        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Function";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string RptName = "";
                string contendPath = System.Web.HttpContext.Current.Request.MapPath("");
                RptName = contendPath + "\\Sys_FunctionList.aspx.frx";
                this.WebReport1.ReportFile = RptName;
                this.WebReport1.StartReport += new EventHandler(WebReport1_StartReport);
            }
        }

        void WebReport1_StartReport(object sender, EventArgs e)
        {
            RDFNew.Module.Admin.Sys.Sys_Function obj = new RDFNew.Module.Admin.Sys.Sys_Function();
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();            
            qs.QueryInfos = GetQueryInfo();
            qs.OrderBy = " Sys_Function.CrtOn Desc ";                
            object[] data = obj.GetMaster(qs);
            if (data[0].ToString() == "0") //正常
            {                
                DataTable dt = data[1] as DataTable;
                Report FReport = (sender as FastReport.Web.WebReport).Report;
                FReport.RegisterData(dt, "Table");
            }            
        }

        List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            string pm1 = Request["pm1"] == "null" ? "" : Request["pm1"];
            string pm2 = Request["pm2"] == "null" ? "" : Request["pm2"];

            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = pm1.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("FunctionID", "Like", "%" + Str + "%"));
            Str = pm2.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("NameC", "Like", "%" + Str + "%"));            
            return qi;
        }
    }
}
