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

namespace RDFNew.Web.Admin.Sys.Function
{
    public partial class Sys_FunctionList : App_Com.PageListSingle 
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Function";
            B_ModuleName = "功能";
            B_PageDetail = "Sys_Function.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Function();
            B_TableKey = "Sys_Function.FunctionID";
            B_OrderBy = " Sys_Function.CrtOn Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtFunctionID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Function.FunctionID", "Like", "FunctionID", "%" + Str + "%"));
            Str = this.txtNameC.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Function.FunctionName", "Like", "FunctionName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtFunctionID.Text = "";
            this.txtNameC.Text = "";
        }

        protected override void AfterDeleteData(DataTable dt)
        {
            App_Com.Sys_User.ClearSys_Menu();
            App_Com.Sys_User.ClearSys_Function();
        }
    }
}
