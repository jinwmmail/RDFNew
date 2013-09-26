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

namespace RDFNew.Web.Admin.Sys.Role
{
    public partial class Sys_Role : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Role";
            B_ModuleName = "职位";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Role();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtRoleID.Readonly = B_Action.ToLower() != "add";
            this.txtSeq.Readonly = B_Action.ToLower() == "view";
            this.txtRoleName.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.xAdd.Enabled = this.xAddAll.Enabled = this.xDel.Enabled = this.xDelAll.Enabled = B_Action.ToLower() != "view";
            this.xAdd2.Enabled = this.xAddAll2.Enabled = this.xDel2.Enabled = this.xDelAll2.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtRoleID.CssStyle = this.txtRoleID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtRoleID.Text = String.Format("{0}", dr["RoleID"]);
            this.txtSeq.Text = String.Format("{0}", dr["Seq"]);
            this.txtRoleName.Text = String.Format("{0}", dr["RoleName"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);

            LoadDetail();
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_Role obj = new RDFNew.Module.Admin.Sys.Sys_Role();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_Role");
            DataRow dr;
            dr = dt.NewRow();
            dr["RoleID"] = App_Com.Helper.InputText(this.txtRoleID.Text, 500);
            dr["Seq"] = App_Com.Helper.InputText(this.txtSeq.Text, 500);
            dr["RoleName"] = App_Com.Helper.InputText(this.txtRoleName.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                BuildSys_User(), BuildSys_ModuleF(),
                App_Com.Helper.BuildLog("Sys_Role", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_Role obj = new RDFNew.Module.Admin.Sys.Sys_Role();
            string Keyword = Request.QueryString["keyword"];
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["Seq"] = App_Com.Helper.InputText(this.txtSeq.Text, 500);
                    dr["RoleName"] = App_Com.Helper.InputText(this.txtRoleName.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                        BuildSys_User(),
                        BuildSys_ModuleF(),
                        App_Com.Helper.BuildLog("Sys_Role", "edit"));
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
            LoadUserR(B_Action, B_Keyword);
            LoadModuleF(B_Action, B_Keyword);
        }
      
        void LoadUserR(string Action, string Keyword)
        {
            FineUI.TreeNode tn;
            DataTable dt;
            this.treeFr.Nodes.Clear();
            this.treeTo.Nodes.Clear();
            if (Action.ToLower() == "add")
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_User Where Enabled=1 Order By UserID 
                    ", ""), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["UserID"]);
                    tn.Text = String.Format("{0}-{1}",dr["UserID"], dr["UserName"]);
                    this.treeFr.Nodes.Add(tn);
                }
            }
            else
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_User a
	                        Left Join Sys_UserR b On b.UserID=a.UserID
		                        And b.RoleID='{0}'
                        Where IsNull(b.UserID,'')=''
                        Order By a.UserID 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["UserID"]);
                    tn.Text = String.Format("{0}-{1}", dr["UserID"], dr["UserName"]);
                    this.treeFr.Nodes.Add(tn);
                }

                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_User a
	                        Left Join Sys_UserR b On b.UserID=a.UserID
		                        And b.RoleID='{0}'
                        Where IsNull(b.UserID,'')!=''
                        Order By a.UserID 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["UserID"]);
                    tn.Text = String.Format("{0}-{1}", dr["UserID"], dr["UserName"]);
                    this.treeTo.Nodes.Add(tn);
                }
            }
        }

        void LoadModuleF(string Action, string Keyword)
        {
            FineUI.TreeNode tn;
            DataTable dt;
            this.treeFr2.Nodes.Clear();
            this.treeTo2.Nodes.Clear();
            if (Action.ToLower() == "add")
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                    Select a.*,b.Caption As ModuleName,c.Icon,c.FunctionName 
                    From Sys_ModuleF a
	                    Left Join Sys_Module b On b.ModuleID=a.ModuleID
	                    Left Join Sys_Function c On c.FunctionID=a.FunctionID
                    Order By b.ModuleID,c.Seq 
                    ", ""), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["ModuleFID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["ModuleName"], dr["FunctionName"]);
                    this.treeFr2.Nodes.Add(tn);
                }
            }
            else
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                    Select a.*,c.Caption As ModuleName,d.Icon,d.FunctionName 
                    From Sys_ModuleF a
	                    Left Join Sys_RoleMF b On b.ModuleFID=a.ModuleFID And b.RoleID='{0}'
	                    Left Join Sys_Module c On c.ModuleID=a.ModuleID
	                    Left Join Sys_Function d On d.FunctionID=a.FunctionID
                    Where IsNull(b.ModuleFID,'')=''
                    Order By c.ModuleID,d.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["ModuleFID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["ModuleName"], dr["FunctionName"]);
                    this.treeFr2.Nodes.Add(tn);
                }

                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                    Select a.*,c.Caption As ModuleName,d.Icon,d.FunctionName 
                    From Sys_ModuleF a
	                    Left Join Sys_RoleMF b On b.ModuleFID=a.ModuleFID And b.RoleID='{0}'
	                    Left Join Sys_Module c On c.ModuleID=a.ModuleID
	                    Left Join Sys_Function d On d.FunctionID=a.FunctionID
                    Where IsNull(b.ModuleFID,'')!=''
                    Order By c.ModuleID,d.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["ModuleFID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["ModuleName"], dr["FunctionName"]);
                    this.treeTo2.Nodes.Add(tn);
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

        DataTable BuildSys_User()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("'',");
            foreach (FineUI.TreeNode tn in this.treeTo.Nodes)
            {
                sb.Append(String.Format("'{0}',", tn.NodeID));
            }
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_User Where UserID In ({0})
                ", sb.ToString().TrimEnd(',')), null);
            return dt;
        }

        protected void btnX2_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;
            FineUI.TreeNode tn;
            if (btn.ID.ToLower() == "xadd2")
            {
                foreach (string s in this.treeFr2.SelectedNodeIDArray)
                {
                    tn = this.treeFr2.FindNode(s);
                    this.treeFr2.Nodes.Remove(tn);
                    this.treeTo2.Nodes.Add(tn);
                }
            }
            if (btn.ID.ToLower() == "xaddall2")
            {
                foreach (FineUI.TreeNode s in this.treeFr2.Nodes)
                {
                    this.treeTo2.Nodes.Add(s);
                }
                this.treeFr2.Nodes.Clear();
            }
            if (btn.ID.ToLower() == "xdel2")
            {
                foreach (string s in this.treeTo2.SelectedNodeIDArray)
                {
                    tn = this.treeTo2.FindNode(s);
                    this.treeTo2.Nodes.Remove(tn);
                    this.treeFr2.Nodes.Add(tn);
                }
            }
            if (btn.ID.ToLower() == "xdelall2")
            {
                foreach (FineUI.TreeNode s in this.treeTo2.Nodes)
                {
                    this.treeFr2.Nodes.Add(s);
                }
                this.treeTo2.Nodes.Clear();
            }
        }

        DataTable BuildSys_ModuleF()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("'',");
            foreach (FineUI.TreeNode tn in this.treeTo2.Nodes)
            {
                sb.Append(String.Format("'{0}',", tn.NodeID));
            }
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_ModuleF Where ModuleFID In ({0})
                ", sb.ToString().TrimEnd(',')), null);
            return dt;
        }
    }
}
