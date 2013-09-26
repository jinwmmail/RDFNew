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

namespace RDFNew.Web.Admin.Bas.Job
{
    public partial class Bas_JobList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Job";
            B_ModuleName = "职位";
            B_PageDetail = "Bas_Job.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Job();
            B_TableKey = "Bas_Job.JobID";
            B_OrderBy = " Bas_Job.JobID ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtJobID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Job.JobID", "Like", "JobID", "%" + Str + "%"));
            Str = this.txtJobName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Job.JobName", "Like", "JobName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtJobID.Text = "";
            this.txtJobName.Text = "";
        }
    }
}
