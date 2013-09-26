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
    public partial class Pur_PoMListPrint : App_Com.PrintBase
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
                RptName = contendPath + "\\Pur_PoMList.aspx.frx";
                this.WebReport1.ReportFile = RptName;
                this.WebReport1.StartReport += new EventHandler(WebReport1_StartReport);
            }
        }

        void WebReport1_StartReport(object sender, EventArgs e)
        {
            RDFNew.Module.Admin.Pur.Pur_PoM obj = new RDFNew.Module.Admin.Pur.Pur_PoM();
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();            
            qs.QueryInfos = GetQueryInfo();
            qs.OrderBy = " Pur_PoM.PoMID Desc ";                
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
            
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = pm1.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Pur_PoM.PoMID", "Like", "PoMID", "%" + Str + "%")); 
            return qi;
        }
    }
}
