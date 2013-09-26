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

namespace RDFNew.Web.Admin.Bas.Employee
{
    public partial class Bas_EmployeeList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Employee";
            B_ModuleName = "雇员";
            B_PageDetail = "Bas_Employee.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Employee();
            B_TableKey = "Bas_Employee.EmployeeID";
            B_OrderBy = " Bas_Employee.CrtOn Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtEmployeeID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Employee.EmployeeID", "Like", "EmployeeID", "%" + Str + "%"));
            Str = this.txtEmployeeName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Employee.EmployeeName", "Like", "EmployeeName", "%" + Str + "%"));
            Str = this.txtNameE.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Employee.NameE", "Like", "NameE", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtEmployeeID.Text = "";
            this.txtEmployeeName.Text = "";
            this.txtNameE.Text = "";
        }

        protected override void AfterDeleteData(DataTable dt)
        {
            App_Com.ImageHelper.DeleteImage(dt.Rows[0], true); //删除图片
        }
    }
}
