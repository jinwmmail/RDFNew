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

namespace RDFNew.Web.Admin.Sys.Function
{
    public partial class Sys_Function : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Function";
            B_ModuleName = "功能";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Function();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtFunctionID.Readonly = B_Action.ToLower() != "add";
            this.txtSeq.Readonly = B_Action.ToLower() == "view";
            this.txtNameC.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.txtIcon.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtFunctionID.CssStyle = this.txtFunctionID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtFunctionID.Text = String.Format("{0}", dr["FunctionID"]);
            this.txtSeq.Text = String.Format("{0}", dr["Seq"]);
            this.txtNameC.Text = String.Format("{0}", dr["FunctionName"]);
            this.txtIcon.Text = String.Format("{0}", dr["Icon"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_Function obj = new RDFNew.Module.Admin.Sys.Sys_Function();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_Function");
            DataRow dr;
            dr = dt.NewRow();
            dr["FunctionID"] = App_Com.Helper.InputText(this.txtFunctionID.Text, 500);
            dr["Seq"] = App_Com.Helper.InputText(this.txtSeq.Text, 500);
            dr["FunctionName"] = App_Com.Helper.InputText(this.txtNameC.Text, 500);
            dr["Icon"] = App_Com.Helper.InputText(this.txtIcon.Text, 500);
            FineUI.Icon xIcon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
            if (xIcon != FineUI.Icon.None)
                dr["IconUrl"] = IconHelper.GetIconUrl(xIcon);
            else
                dr["IconUrl"] = IconHelper.GetIconUrl(FineUI.Icon.Image);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Sys_Function", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_Function obj = new RDFNew.Module.Admin.Sys.Sys_Function();
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
                    dr["FunctionName"] = App_Com.Helper.InputText(this.txtNameC.Text, 500);
                    dr["Icon"] = App_Com.Helper.InputText(this.txtIcon.Text, 500);
                    FineUI.Icon xIcon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    if (xIcon != FineUI.Icon.None)
                        dr["IconUrl"] = IconHelper.GetIconUrl(xIcon);
                    else
                        dr["IconUrl"] = IconHelper.GetIconUrl(FineUI.Icon.Image);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Sys_Function", "edit"));
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

        protected override void AfterApply()
        {
            App_Com.Sys_User.ClearSys_Menu();
            App_Com.Sys_User.ClearSys_Function();
        }
    }
}
