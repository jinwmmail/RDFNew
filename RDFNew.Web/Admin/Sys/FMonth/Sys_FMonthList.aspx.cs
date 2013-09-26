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

namespace RDFNew.Web.Admin.Sys.FMonth
{
    public partial class Sys_FMonthList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_FMonth";
            B_ModuleName = "财务月份";
            B_PageDetail = "Sys_FMonth.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_FMonth();
            B_TableKey = "Sys_FMonth.FmonthID";
            B_OrderBy = " Sys_FMonth.FmonthID Desc ";
            B_WinSize = new int[] { 550, 550 };
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtFMonthID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_FMonth.FMonthID", "Like", "FMonthID", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtFMonthID.Text = "";                    
        }       
    }
}
