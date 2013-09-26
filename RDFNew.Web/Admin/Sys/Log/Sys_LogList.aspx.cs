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

namespace RDFNew.Web.Admin.Sys.Log
{
    public partial class Sys_LogList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Log";
            B_ModuleName = "系统日志";
            B_PageDetail = "Sys_Log.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Log();
            B_TableKey = "Sys_Log.LogID";
            B_OrderBy = " Sys_Log.DateTime Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            this.txtModuleID.OnClientTriggerClick = Window2.GetSaveStateReference(
                txtModuleID.ClientID)
                    + Window2.GetShowReference("../Module/Sys_ModuleList.aspx?action=select");

            this.txtUserID.OnClientTriggerClick = Window2.GetSaveStateReference(
                txtUserID.ClientID)
                    + Window2.GetShowReference("../User/Sys_UserList.aspx?action=select");
        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            
            Str = this.txtModuleID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Log.Module", "Like", "Module", "%" + Str + "%"));

            Str = this.txtUserID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Log.[User]", "Like", "User", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {            
            this.txtModuleID.Text = "";
            this.txtUserID.Text = "";
        }
    }
}
