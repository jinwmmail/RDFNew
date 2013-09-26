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

namespace RDFNew.Web.Admin.Hr.Leave
{
    public partial class Hr_LeaveList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Hr_Leave";
            B_ModuleName = "请假申请单";
            B_PageDetail = "Hr_Leave.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Hr.Hr_Leave();
            B_TableKey = "Hr_Leave.LeaveID";
            B_OrderBy = " Hr_Leave.LeaveID Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtLeaveID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Hr_Leave.LeaveID", "Like", "LeaveID", "%" + Str + "%"));
            Str = this.txtLeaveDate.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Hr_Leave.LeaveDate", "Like", "LeaveDate", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtLeaveID.Text = "";
            this.txtLeaveDate.Text = "";
        }
    }
}
