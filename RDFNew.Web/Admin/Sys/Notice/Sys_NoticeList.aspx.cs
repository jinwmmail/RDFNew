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

namespace RDFNew.Web.Admin.Sys.Notice
{
    public partial class Sys_NoticeList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Notice";
            B_ModuleName = "系统公告";
            B_PageDetail = "Sys_Notice.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Notice();
            B_TableKey = "Sys_Notice.NoticeID";
            B_OrderBy = "Sys_Notice.CrtOn Desc ";
            B_WinSize = new int[] { 750, 500 };
            B_WindowMaxSize = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtNoticeID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Notice.NoticeID", "Like", "NoticeID", "%" + Str + "%"));
            Str = this.txtNoticeTitle.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Notice.NoticeTitle", "Like", "NoticeTitle", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtNoticeID.Text = "";
            this.txtNoticeTitle.Text = "";
        }
    }
}
