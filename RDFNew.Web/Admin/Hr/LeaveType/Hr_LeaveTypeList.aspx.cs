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

namespace RDFNew.Web.Admin.Hr.LeaveType
{
    public partial class Hr_LeaveTypeList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Hr_LeaveType";
            B_ModuleName = "请假类别";
            B_PageDetail = "Hr_LeaveType.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Hr.Hr_LeaveType();
            B_TableKey = "Hr_LeaveType.LeaveTypeID";
            B_OrderBy = " Hr_LeaveType.LeaveTypeID ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtLeaveTypeID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Hr_LeaveType.LeaveTypeID", "Like", "LeaveTypeID", "%" + Str + "%"));
            Str = this.txtLeaveTypeName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Hr_LeaveType.LeaveTypeName", "Like", "LeaveTypeName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtLeaveTypeID.Text = "";
            this.txtLeaveTypeName.Text = "";
        }
    }
}
