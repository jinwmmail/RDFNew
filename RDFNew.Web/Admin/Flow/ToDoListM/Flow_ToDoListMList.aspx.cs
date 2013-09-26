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

namespace RDFNew.Web.Admin.Flow.ToDoListM
{
    public partial class Flow_ToDoListMList : App_Com.PageListMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_ToDoListM";
            B_ModuleName = "流程列表";
            B_PageDetail = "Flow_ToDoListM.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_ToDoListM();
            B_TableKey = "Flow_ToDoListM.ToDoListMID";
            B_OrderBy = " Flow_ToDoListM.ToDoListMID Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            
        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtToDoListMID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_ToDoListM.ToDoListMID", "Like", "ToDoListMID", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtToDoListMID.Text = "";            
        }
    }
}
