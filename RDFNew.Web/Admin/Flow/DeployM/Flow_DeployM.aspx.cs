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
using System.Collections;
using System.Reflection;

namespace RDFNew.Web.Admin.Flow.DeployM
{
    public partial class Flow_DeployM : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_DeployM";
            B_ModuleName = "流程部署";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_DeployM();

            B_DetailSessionKey = "Flow_DeployD";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtDeployMID.Readonly = B_Action.ToLower() != "add";
            this.txtDeployName.Readonly = B_Action.ToLower() == "view";
            this.txtFileName.Readonly = B_Action.ToLower() != "add";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtDeployMID.CssStyle = this.txtDeployMID.Readonly ? "background:#c0c0c0;" : "";                
                this.txtFileName.CssStyle = this.txtFileName.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtDeployMID.Text = String.Format("{0}", dr["DeployMID"]);
            this.txtDeployName.Text = String.Format("{0}", dr["DeployName"]);
            this.hidDeployKey.Text = String.Format("{0}", dr["DeployKey"]);
            this.txtFileName.Text = String.Format("{0}", dr["FileName"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);

            LoadDetail();
        }

        protected override void LoadDetail()
        {
            String DeployKey = this.hidDeployKey.Text.Trim();
            if (DeployKey != "")
            {
                DS.XBPM.API.DeploymentQuery query = App_Com.FlowHelper.Engine.GetRepositoryService().CreateDeploymentQuery();
                List<DS.XBPM.API.Deployment> list = query.DeploymentId(DeployKey).List();
                if (list.Count > 0)
                {
                    List<DS.XBPM.API.ProcessActivity> x = new List<DS.XBPM.API.ProcessActivity>();
                    foreach (DS.XBPM.API.ProcessActivity a in list[0].ManualActivities)
                        x.Add(a);                    
                    x.Sort(StringCompare);
                                        
                    this.Grid1.DataSource = x;                    
                    this.Grid1.DataBind();                                       
                }
            }
        }

        int StringCompare(DS.XBPM.API.ProcessActivity obj1, DS.XBPM.API.ProcessActivity obj2)
        {
            int result = 0;
            if ((obj1 == null) && (obj2 == null))
                return 0;
            else if ((obj1 != null) && (obj2 == null))
                return 1;
            else if ((obj1 == null) && (obj2 != null))
                return -1;
            result = String.Compare(obj1.ProcessName.ToUpper() + obj1.Name.ToUpper(), obj2.ProcessName.ToUpper() + obj2.Name.ToUpper());
            return result;
        }
        
        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_DeployM obj = new RDFNew.Module.Admin.Flow.Flow_DeployM();
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["DeployName"] = App_Com.Helper.InputText(this.txtDeployName.Text, 500);
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),GetDetail(),
                                            App_Com.Helper.BuildLog("Flow_DeployM", "edit"));
                    if (data[0].ToString() != "0") //正常                
                        throw data[1] as Exception;
                    else
                        return data[1].ToString();
                }
                else
                {
                    throw new Exception("需要修改的记录已不存在,请刷新后再试.");
                }
            }
            else
            {
                throw data[1] as Exception;
            }
        }

        DataTable GetDetail()
        {
            DataTable dt = null;
            RDFNew.Module.Admin.Flow.Flow_DeployD da = new RDFNew.Module.Admin.Flow.Flow_DeployD();
            dt = da.GetDataByParent(B_Keyword)[1] as DataTable;
            dt.Constraints.Clear();
            DataTable dtGrid = dt.Clone();
            
            DataRow drNew;
            System.Web.UI.WebControls.TextBox txt;  
            foreach (FineUI.GridRow gr in this.Grid1.Rows)
            {
                drNew = dtGrid.NewRow();
                drNew["DeployKey"] = this.hidDeployKey.Text.Trim();
                drNew["ProcessName"] = gr.Values[0];
                drNew["ActivityName"] = gr.Values[1];
                drNew["Description"] = gr.Values[2];
                txt = gr.FindControl("txtOperator") as System.Web.UI.WebControls.TextBox;
                drNew["WhoCanSubmit"] = txt.Text.Trim();
                dtGrid.Rows.Add(drNew);
            }

            DataRow[] drs;
            foreach (DataRow dr in dtGrid.Rows)
            {
                drs = dt.Select(String.Format(" DeployKey='{0}' And ProcessName='{1}' And ActivityName='{2}' ",
                    dr["DeployKey"], dr["ProcessName"], dr["ActivityName"]));
                if (drs.Length > 0)
                {
                    drs[0]["WhoCanSubmit"] = dr["WhoCanSubmit"];
                }
                else
                {
                    drNew = dt.NewRow();
                    drNew["DeployKey"] = dr["DeployKey"];
                    drNew["ProcessName"] = dr["ProcessName"];
                    drNew["ActivityName"] = dr["ActivityName"];
                    drNew["Description"] = dr["Description"];
                    drNew["WhoCanSubmit"] = dr["WhoCanSubmit"];
                    dt.Rows.Add(drNew);
                }
            } 

            return dt;
        }

        DataTable Tmp_ForBound = null;
        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            DS.XBPM.API.ProcessActivity row = e.DataItem as DS.XBPM.API.ProcessActivity;
            if (row != null)
            {
                System.Web.UI.WebControls.TextBox txt;
                txt = this.Grid1.Rows[e.RowIndex].FindControl("txtOperator") as System.Web.UI.WebControls.TextBox;
                if (Tmp_ForBound == null)
                {
                    RDFNew.Module.Admin.Flow.Flow_DeployD da = new RDFNew.Module.Admin.Flow.Flow_DeployD();
                    Tmp_ForBound = da.GetDataByParent(B_Keyword)[1] as DataTable;
                }
                DataRow[] drs = Tmp_ForBound.Select(String.Format(" ProcessName='{0}' And ActivityName='{1}' ",                    
                    row.ProcessName,
                    row.Name),"Seq Desc ");
                if (drs.Length > 0)
                    txt.Text = drs[0]["WhoCanSubmit"].ToString();
            }
        }
    }  
}
