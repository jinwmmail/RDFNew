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

namespace RDFNew.Web.Admin.Sys.Module
{
    public partial class Sys_Module : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Module";
            B_ModuleName = "模块";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Module();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtModuleID.Readonly = B_Action.ToLower() != "add";
            this.txtCaption.Readonly = B_Action.ToLower() == "view";
            this.txtUrl.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.xAdd.Enabled = this.xAddAll.Enabled = this.xDel.Enabled = this.xDelAll.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtModuleID.CssStyle = this.txtModuleID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtModuleID.Text = String.Format("{0}", dr["ModuleID"]);
            this.txtCaption.Text = String.Format("{0}", dr["Caption"]);
            this.txtUrl.Text = String.Format("{0}", dr["Url"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);

            LoadDetail();
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_Module obj = new RDFNew.Module.Admin.Sys.Sys_Module();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_Module");
            DataRow dr;
            dr = dt.NewRow();
            dr["ModuleID"] = App_Com.Helper.InputText(this.txtModuleID.Text, 500);
            dr["Caption"] = App_Com.Helper.InputText(this.txtCaption.Text, 500);
            dr["Url"] = App_Com.Helper.InputText(this.txtUrl.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added), BuildSys_Function(),
                                        App_Com.Helper.BuildLog("Sys_Module", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_Module obj = new RDFNew.Module.Admin.Sys.Sys_Module();
            string Keyword = Request.QueryString["keyword"];
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["Caption"] = App_Com.Helper.InputText(this.txtCaption.Text, 500);
                    dr["Url"] = App_Com.Helper.InputText(this.txtUrl.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified), BuildSys_Function(),
                                            App_Com.Helper.BuildLog("Sys_Module", "edit"));
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
            LoadModuleF(B_Action, B_Keyword);
        }
      
        void LoadModuleF(string Action, string Keyword)
        {
            FineUI.TreeNode tn;
            DataTable dt;
            if (Action.ToLower() == "add")
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select * From Sys_Function Where Enabled=1 Order By Seq 
                    ",""), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["FunctionID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["FunctionName"]); 
                    this.treeFr.Nodes.Add(tn);
                }
            }
            else
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Function a
	                        Left Join Sys_ModuleF b On b.FunctionID=a.FunctionID
		                        And b.ModuleID='{0}'
                        Where IsNull(b.FunctionID,'')=''
                        Order By a.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["FunctionID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["FunctionName"]);
                    this.treeFr.Nodes.Add(tn);
                }

                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                        Select a.* 
                        From Sys_Function a
	                        Left Join Sys_ModuleF b On b.FunctionID=a.FunctionID
		                        And b.ModuleID='{0}'
                        Where IsNull(b.FunctionID,'')!=''
                        Order By a.Seq 
                    ", Keyword), null);
                foreach (DataRow dr in dt.Rows)
                {
                    tn = new FineUI.TreeNode();
                    tn.NodeID = String.Format("{0}", dr["FunctionID"]);
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.Text = String.Format("{0}-{1}", dr["Seq"], dr["FunctionName"]);
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

        DataTable BuildSys_Function()
        {            
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("'',");
            foreach (FineUI.TreeNode tn in this.treeTo.Nodes)
            {
                sb.Append(String.Format("'{0}',", tn.NodeID));
            }
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select * From Sys_Function Where FunctionID In ({0})
                ",sb.ToString().TrimEnd(',')),null);
            return dt;
        }

        protected override void AfterApply()
        {
            App_Com.Sys_User.ClearSys_Menu();
            App_Com.Sys_User.ClearSys_Function();
        }
    }
}
