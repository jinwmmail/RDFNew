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

namespace RDFNew.Web.Admin.Flow.DeployM
{
    public partial class Flow_DeployMList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_DeployM";
            B_ModuleName = "流程部署";
            B_PageDetail = "Flow_Designer.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_DeployM();
            B_TableKey = "Flow_DeployM.DeployMID";
            B_OrderBy = " Flow_DeployM.CrtOn Desc ";
            
            //B_WinSize = new int[] { 800,500 };
            B_WindowMaxSize = true;
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

            Str = this.txtDeployMID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_DeployM.DeployMID", "Like", "DeployMID", "%" + Str + "%"));

            Str = this.txtDeployName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_DeployM.DeployName", "Like", "DeployName", "%" + Str + "%"));

            return qi;
        }

        protected override void ClearControls()
        {
            this.txtDeployMID.Text = "";
            this.txtDeployName.Text = "";
        }

        protected override void AfterDeleteData(DataTable dt)
        {
            String FilePath = App_Com.FlowHelper.PDL_Folder + "\\" +
                dt.Rows[0]["FileName",DataRowVersion.Original].ToString();
            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);
        }

        protected override void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Install")     //部署
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                RDFNew.Module.Admin.Flow.Flow_DeployM obj = new RDFNew.Module.Admin.Flow.Flow_DeployM();
                object[] data = obj.GetMaster(KeyVal);
                if (data[0].ToString() == "0") //正常
                {
                    DataTable dt = data[1] as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr;
                        dr = dt.Rows[0];
                        if (dr["DeployKey"] == System.DBNull.Value || dr["DeployKey"].ToString() == "")
                        {
                            DS.XBPM.API.Deployment dep = App_Com.FlowHelper.Engine.GetRepositoryService().CreateDeployment();
                            dep.AddResourceFromFile(Path.Combine(App_Com.FlowHelper.PDL_Folder, dr["FileName"].ToString()));
                            dr["DeployKey"] = dep.Deploy();
                        }
                        dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                        dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                                App_Com.Helper.BuildLog("Flow_DeployM", "edit"));

                        LoadMater();
                    }
                }
            }
            if (e.CommandName == "Deploy")
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleEdit, B_ModuleName, KeyVal);
                B_Window1.IFrameUrl = String.Format("Flow_DeployM.aspx?action=edit&keyword={1}", B_PageDetail, KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            LoadMater();
        }

        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            LinkButtonField lkbDeploy = Grid1.FindColumn("lkbDeploy") as LinkButtonField;
            lkbDeploy.ConfirmText = "您确信执行此操作吗?";
            LinkButtonField lkbConfig = Grid1.FindColumn("lkbConfig") as LinkButtonField;            
            DataRowView row = e.DataItem as DataRowView;
            if (row != null)
            {
                if (row.Row["DeployKey"].ToString() != "")
                {
                    lkbDeploy.Enabled = false;
                    lkbConfig.Enabled = true;
                }
                else
                {
                    lkbDeploy.Enabled = true;
                    lkbConfig.Enabled = false;
                }
            }                            
        }
    }
}
