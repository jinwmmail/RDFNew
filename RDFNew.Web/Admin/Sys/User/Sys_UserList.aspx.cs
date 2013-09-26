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

namespace RDFNew.Web.Admin.Sys.User
{
    public partial class Sys_UserList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_User";
            B_ModuleName = "职位";
            B_PageDetail = "Sys_User.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_User();
            B_TableKey = "Sys_User.UserID";
            B_OrderBy = " Sys_User.CrtOn Desc ";
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
            Str = this.txtUserID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_User.UserID", "Like", "UserID", "%" + Str + "%"));
            Str = this.txtUserName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_User.UserName", "Like", "UserName", "%" + Str + "%"));
            Str = this.txtNameE.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_User.NameE", "Like", "NameE", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtUserID.Text = "";
            this.txtUserName.Text = "";
            this.txtNameE.Text = "";
        }      
    }
}
