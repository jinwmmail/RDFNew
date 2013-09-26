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
    public partial class Sys_User : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_User";
            B_ModuleName = "用户";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_User();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtUserID.Readonly = B_Action.ToLower() != "add";
            this.txtUserName.Readonly = B_Action.ToLower() == "view";
            this.txtNameE.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.ckbUpdatePwd.Enabled = B_Action.ToLower() != "view";
            this.ckbIsAdmin.Enabled = B_Action.ToLower() != "view";
            this.txtPwd.Readonly = B_Action.ToLower() == "view";
            this.txtUserCode.Readonly = B_Action.ToLower() == "view";
            this.txtEmail.Readonly = B_Action.ToLower() == "view";
            this.xAdd.Enabled = this.xAddAll.Enabled = this.xDel.Enabled = this.xDelAll.Enabled = B_Action.ToLower() != "view";
            this.xAdd2.Enabled = this.xAddAll2.Enabled = this.xDel2.Enabled = this.xDelAll2.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtUserID.CssStyle = this.txtUserID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
            this.ckbIsAdmin.Checked = false;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtUserID.Text = String.Format("{0}", dr["UserID"]);
            this.txtUserName.Text = String.Format("{0}", dr["UserName"]);
            this.txtNameE.Text = String.Format("{0}", dr["NameE"]);
            this.txtUserCode.Text = String.Format("{0}", dr["UserCode"]);
            this.txtEmail.Text = String.Format("{0}", dr["Email"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.ckbIsAdmin.Checked = Convert.ToBoolean(dr["IsAdmin"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);

            LoadDetail();
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_User obj = new RDFNew.Module.Admin.Sys.Sys_User();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_User");
            DataRow dr;
            dr = dt.NewRow();
            dr["UserID"] = App_Com.Helper.InputText(this.txtUserID.Text, 500);
            dr["UserName"] = App_Com.Helper.InputText(this.txtUserName.Text, 500);
            dr["NameE"] = App_Com.Helper.InputText(this.txtNameE.Text, 500);
            if (this.ckbUpdatePwd.Checked)
            {
                dr["Pwd"] = App_Com.Helper.StringToSHA1Hash(this.txtPwd.Text);
            }
            dr["UserCode"] = App_Com.Helper.InputText(this.txtUserCode.Text, 500);
            dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["IsAdmin"] = this.ckbIsAdmin.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                BuildSys_Role(),
                BuildSys_ModuleF(),
                App_Com.Helper.BuildLog("Sys_User", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_User obj = new RDFNew.Module.Admin.Sys.Sys_User();
            string Keyword = Request.QueryString["keyword"];
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["UserName"] = App_Com.Helper.InputText(this.txtUserName.Text, 500);
                    dr["NameE"] = App_Com.Helper.InputText(this.txtNameE.Text, 500);
                    if (this.ckbUpdatePwd.Checked)
                    {
                        dr["Pwd"] = App_Com.Helper.StringToSHA1Hash(this.txtPwd.Text);
                    }
                    dr["UserCode"] = App_Com.Helper.InputText(this.txtUserCode.Text, 500);
                    dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["IsAdmin"] = this.ckbIsAdmin.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                        BuildSys_Role(),
                        BuildSys_ModuleF(),
                        App_Com.Helper.BuildLog("Sys_User", "edit"));
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
            if (Action.ToLower() == "add")
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_Role Where Enabled=1 Order By Seq 
                    ", ""), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}-{2}", dr["Seq"], dr["RoleID"], dr["RoleName"]);
                    this.treeFr.Nodes.Add(tn);
                }
            }
            else
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Role a
	                        Left Join Sys_UserR b On b.RoleID=a.RoleID
		                        And b.UserID='{0}'
                        Where IsNull(b.RoleID,'')=''
                        Order By a.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}-{2}", dr["Seq"], dr["RoleID"], dr["RoleName"]);
                    this.treeFr.Nodes.Add(tn);
                }

                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Role a
	                        Left Join Sys_UserR b On b.RoleID=a.RoleID
		                        And b.UserID='{0}'
                        Where IsNull(b.RoleID,'')!=''
                        Order By a.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["RoleID"]);
                    tn.Text = String.Format("{0}-{1}-{2}", dr["Seq"], dr["RoleID"], dr["RoleName"]);
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
	                    Left Join Sys_UserMF b On b.ModuleFID=a.ModuleFID And b.UserID='{0}'
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
	                    Left Join Sys_UserMF b On b.ModuleFID=a.ModuleFID And b.UserID='{0}'
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

        DataTable BuildSys_Role()
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
