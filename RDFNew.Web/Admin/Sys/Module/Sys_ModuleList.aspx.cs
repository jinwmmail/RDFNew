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

namespace RDFNew.Web.Admin.Sys.Module
{
    public partial class Sys_ModuleList : App_Com.PageListSingle 
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Module";
            B_ModuleName = "模块";
            B_PageDetail = "Sys_Module.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Module();
            B_TableKey = "Sys_Module.ModuleID";
            B_OrderBy = " Sys_Module.CrtOn Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            if (B_Action.ToUpper() == "SELECT")
            {
                this.Region1.Height = 90;
            }
        }
        
        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtModuleID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Module.ModuleID", "Like", "ModuleID", "%" + Str + "%"));
            Str = this.txtCaption.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Module.Caption", "Like", "Caption", "%" + Str + "%"));
            Str = this.txtUrl.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Module.Url", "Like", "Url", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtModuleID.Text = "";
            this.txtCaption.Text = "";
            this.txtUrl.Text = "";
        }

        protected override void AfterDeleteData(DataTable dt)
        {
            App_Com.Sys_User.ClearSys_Menu();
            App_Com.Sys_User.ClearSys_Function();
        }

        protected override string[] BackSelectData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            return new string[] { row.DataKeys[0].ToString(), row.Values[1], row.Values[2] };
        }

        protected override string[] BackEmptyData()
        {
            return new string[] { "", "", "" };
        }
    }
}
