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

namespace RDFNew.Web.Admin.Flow.Org
{
    public partial class Flow_Org : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_Org";
            B_ModuleName = "组织";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_Org();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void FillControlData()
        {
            RDFNew.Module.Admin.Flow.Flow_Job da = new RDFNew.Module.Admin.Flow.Flow_Job();
            this.ddlJobID.DataSource = da.GetMaster(new RDFNew.Module.DALEntity.QuerySet())[1] as DataTable;
            this.ddlJobID.DataValueField = "JobID";
            this.ddlJobID.DataTextField = "JobName";
            this.ddlJobID.DataBind();

            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtRoleName.OnClientTriggerClick = Window1.GetSaveStateReference(
                this.hidRoleID.ClientID, this.txtRoleName.ClientID)
                        + Window1.GetShowReference("../../Sys/Role/Sys_RoleList.aspx?action=select");

                this.txtPOrgID.OnClientTriggerClick = Window1.GetSaveStateReference(
                this.txtPOrgID.ClientID, this.txtPOrgName.ClientID, this.hidPID.ClientID)
                        + Window1.GetShowReference("Flow_OrgList.aspx?action=select");
            }

            if (B_Action.ToLower() == "add")
            {
                this.txtPOrgID.Text = Request.QueryString["ParentID"];
                this.txtPOrgName.Text = Request.QueryString["ParentName"];
                this.txtPOrgName.Text = Server.UrlDecode(this.txtPOrgName.Text);
                this.hidPID.Text = Request.QueryString["ParentRID"];
            }
        }

        protected override void SetControlState()
        {
            this.txtOrgID.Readonly = B_Action.ToLower() != "add";
            this.txtOrgName.Readonly = B_Action.ToLower() == "view";
            this.txtPOrgID.Readonly = B_Action.ToLower() == "view";
            this.txtRoleName.Readonly = B_Action.ToLower() == "view";
            this.ddlJobID.Readonly = B_Action.ToLower() == "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            this.xAdd.Enabled = B_Action.ToLower() != "view";
            this.xAddAll.Enabled = B_Action.ToLower() != "view";
            this.xDel.Enabled = B_Action.ToLower() != "view";
            this.xDelAll.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtOrgID.CssStyle = this.txtOrgID.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtOrgID.Text = String.Format("{0}", dr["OrgID"]);
            this.txtOrgName.Text = String.Format("{0}", dr["OrgName"]);
            this.txtPOrgID.Text = String.Format("{0}", dr["POrgID"]);
            this.txtPOrgName.Text = String.Format("{0}", dr["POrgName"]);
            this.hidRoleID.Text = String.Format("{0}", dr["RoleID"]);
            this.txtRoleName.Text = String.Format("{0}", dr["RoleName"]);
            this.ddlJobID.SelectedValue = String.Format("{0}", dr["JobID"]);
            this.hidPID.Text = String.Format("{0}", dr["PID"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);

            LoadDetail();
        }
         
        protected override string AddData()
        {
            RDFNew.Module.Admin.Flow.Flow_Org obj = new RDFNew.Module.Admin.Flow.Flow_Org();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Flow_Org");
            DataRow dr;
            dr = dt.NewRow();
            dr["OrgID"] = App_Com.Helper.InputText(this.txtOrgID.Text, 500);            
            dr["OrgName"] = App_Com.Helper.InputText(this.txtOrgName.Text, 500);
            dr["RoleID"] = App_Com.Helper.InputText(this.hidRoleID.Text, 500);
            dr["JobID"] = this.ddlJobID.SelectedItem.Value;            
            dr["PID"] = this.hidPID.Text;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added), BuildSub(),
                                        App_Com.Helper.BuildLog("Flow_Org", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_Org obj = new RDFNew.Module.Admin.Flow.Flow_Org();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["OrgName"] = App_Com.Helper.InputText(this.txtOrgName.Text, 500);
                    dr["RoleID"] = App_Com.Helper.InputText(this.hidRoleID.Text, 500);                    
                    dr["JobID"] = this.ddlJobID.SelectedItem.Value;
                    dr["PID"] = this.hidPID.Text;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified), BuildSub(),
                                            App_Com.Helper.BuildLog("Flow_Org", "edit"));
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

        protected override void LoadDetail()
        {
            FineUI.TreeNode tn;
            DataTable dt;
            if (B_Action.ToLower() == "add")
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_Role Where Enabled=1 Order By Seq 
                    ", ""), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["RoleName"]);
                    this.treeFr.Nodes.Add(tn);
                }
            }
            else
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Role a
	                        Left Join Flow_OrgR b On b.RoleID=a.RoleID
		                        And b.OrgID='{0}'
                        Where IsNull(b.RoleID,'')=''
                        Order By a.Seq 
                    ", B_Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["RoleName"]);
                    this.treeFr.Nodes.Add(tn);
                }

                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Role a
	                        Left Join Flow_OrgR b On b.RoleID=a.RoleID
		                        And b.OrgID='{0}'
                        Where IsNull(b.RoleID,'')!=''
                        Order By a.Seq 
                    ", B_Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["RoleName"]);
                    this.treeTo.Nodes.Add(tn);
                }
            }
        }

        protected void btnX_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;
            FineUI.TreeNode tn;
            if (btn.ID.ToLower() == "xadd")
            {
                foreach (string s in this.treeFr.SelectedNodeIDArray)
                {
                    tn = this.treeFr.FindNode(s);
                    this.treeFr.Nodes.Remove(tn);
                    this.treeTo.Nodes.Add(tn);
                }
            }
            if (btn.ID.ToLower() == "xaddall")
            {
                foreach (FineUI.TreeNode s in this.treeFr.Nodes)
                {
                    this.treeTo.Nodes.Add(s);
                }
                this.treeFr.Nodes.Clear();
            }
            if (btn.ID.ToLower() == "xdel")
            {
                foreach (string s in this.treeTo.SelectedNodeIDArray)
                {
                    tn = this.treeTo.FindNode(s);
                    this.treeTo.Nodes.Remove(tn);
                    this.treeFr.Nodes.Add(tn);
                }
            }
            if (btn.ID.ToLower() == "xdelall")
            {
                foreach (FineUI.TreeNode s in this.treeTo.Nodes)
                {
                    this.treeFr.Nodes.Add(s);
                }
                this.treeTo.Nodes.Clear();
            }
        }

        DataTable BuildSub()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("'',");
            foreach (FineUI.TreeNode tn in this.treeTo.Nodes)
            {
                sb.Append(String.Format("'{0}',", tn.NodeID));
            }
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select * From Sys_Role Where RoleID In ({0})
                ", sb.ToString().TrimEnd(',')), null);
            return dt;
        }
    }
}
