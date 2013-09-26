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

namespace RDFNew.Web.Admin.Sys.Role
{
    public partial class Sys_RoleList : App_Com.PageListSingle
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Role";
            B_ModuleName = "角色";
            B_PageDetail = "Sys_Role.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Role();
            B_TableKey = "Sys_Role.RoleID";
            B_OrderBy = " Sys_Role.RoleID ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtRoleID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Role.RoleID", "Like", "RoleID", "%" + Str + "%"));
            Str = this.txtRoleName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Role.RoleName", "Like", "RoleName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtRoleID.Text = "";
            this.txtRoleName.Text = "";
        }
        protected override string[] BackSelectData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            return new string[] { row.DataKeys[0].ToString(),  row.Values[2] };
        }

        protected override string[] BackEmptyData()
        {
            return new string[] { "", "" };
        }
    }
}