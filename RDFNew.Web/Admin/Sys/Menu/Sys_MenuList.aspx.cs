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

namespace RDFNew.Web.Admin.Sys.Menu
{
    public partial class Sys_MenuList : App_Com.PageListSingle 
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Menu";
            B_ModuleName = "菜单";
            B_PageDetail = "Sys_Menu.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Menu();
            B_TableKey = "Sys_Menu.MenuID";
            B_OrderBy = " Sys_Menu.RID ";            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtMenuID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Menu.MenuID", "Like","MenuID", "%" + Str + "%"));
            Str = this.txtMenuName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Menu.MenuName", "Like", "MenuName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtMenuID.Text = "";
            this.txtMenuName.Text = "";   
        }

        protected override void QueryData()
        {
            //LoadMater();
        }

        protected override void AddData()
        {
            String ParentID = "";
            String ParentName = "";
            String ParentRID = "";
            if (this.Grid1.SelectedRowIndexArray.Length > 0)
            {
                ParentID = this.Grid1.DataKeys[this.Grid1.SelectedRowIndex][0].ToString();
                ParentName = this.Grid1.Rows[this.Grid1.SelectedRowIndex].Values[1];
                ParentRID = this.Grid1.Rows[this.Grid1.SelectedRowIndex].Values[2];
            }
            B_Window1.Title = String.Format("{0}-[{1}]", B_TitleAdd, B_ModuleName);
            B_Window1.IFrameUrl = String.Format("{0}?action=add&parentID={1}&parentName={2}&parentrid={3}",
                B_PageDetail, ParentID, Server.UrlEncode(ParentName), ParentRID);
            B_Window1.Hidden = false;
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
