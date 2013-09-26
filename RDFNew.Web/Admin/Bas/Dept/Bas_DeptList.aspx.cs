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

namespace RDFNew.Web.Admin.Bas.Dept
{
    public partial class Bas_DeptList : App_Com.PageListSingle
    {                
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Dept";
            B_ModuleName = "部门";
            B_PageDetail = "Bas_Dept.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Dept();
            B_TableKey = "Bas_Dept.DeptID";
            B_OrderBy = " Bas_Dept.RID ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtDeptID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Dept.DeptID", "Like", "DeptID", "%" + Str + "%"));
            Str = this.txtDeptName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Dept.DeptName", "Like", "DeptName", "%" + Str + "%"));
            return qi;
        }
   
        protected override void QueryData()
        {
            //LoadMater();
        }

        protected override void ClearControls()
        {
            this.txtDeptID.Text = "";
            this.txtDeptName.Text = "";
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

        protected override string[] BackSelectData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            return new string[] { row.DataKeys[0].ToString(), row.Values[1], row.Values[2] };
        }

        protected override string[] BackEmptyData()
        {
            return new string[] { "", "","" };
        }
    }
}
