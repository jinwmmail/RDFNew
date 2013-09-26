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

namespace RDFNew.Web.Admin.Sys.Menu
{
    public partial class Sys_Menu : App_Com.PageSingle  
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Menu";
            B_ModuleName = "菜单";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Menu();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtModuleID.OnClientTriggerClick = Window1.GetSaveStateReference(
                    txtModuleID.ClientID, txtMenuName.ClientID, txtURL.ClientID)
                        + Window1.GetShowReference("../Module/Sys_ModuleList.aspx?action=select");

                this.txtPMenuID.OnClientTriggerClick = Window1.GetSaveStateReference(
                this.txtPMenuID.ClientID, this.txtPMenuName.ClientID, this.hidPID.ClientID)
                        + Window1.GetShowReference("Sys_MenuList.aspx?action=select");
            }

            if (B_Action.ToLower() == "add")
            {
                this.txtPMenuID.Text = Request.QueryString["ParentID"];
                this.txtPMenuName.Text = Request.QueryString["ParentName"];
                this.txtPMenuName.Text = Server.UrlDecode(this.txtPMenuName.Text);
                this.hidPID.Text = Request.QueryString["ParentRID"];
            }
        }

        protected override void SetControlState()
        {
            this.txtMenuID.Readonly = B_Action.ToLower() != "add";
            this.txtMenuName.Readonly = B_Action.ToLower() == "view";
            this.txtUrlParameter.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.txtModuleID.Readonly = B_Action.ToLower() == "view";
            this.txtIcon.Readonly = B_Action.ToLower() == "view";
            this.ckbVisibled.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtMenuID.CssStyle = this.txtMenuID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
            this.ckbVisibled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtMenuID.Text = String.Format("{0}", dr["MenuID"]);
            this.txtMenuName.Text = String.Format("{0}", dr["MenuName"]);
            this.txtUrlParameter.Text = String.Format("{0}", dr["UrlParameter"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
            this.txtModuleID.Text = String.Format("{0}", dr["ModuleID"]);
            this.txtIcon.Text = String.Format("{0}", dr["Icon"]);
            this.ckbVisibled.Checked = Convert.ToBoolean(dr["Visibled"]);            
            this.txtURL.Text = String.Format("{0}", dr["Url"]);
            this.txtPMenuID.Text = String.Format("{0}", dr["PMenuID"]);
            this.txtPMenuName.Text = String.Format("{0}", dr["PMenuName"]);
            this.hidPID.Text = String.Format("{0}", dr["PID"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_Menu obj = new RDFNew.Module.Admin.Sys.Sys_Menu();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_Menu");
            DataRow dr;
            dr = dt.NewRow();
            dr["MenuID"] = App_Com.Helper.InputText(this.txtMenuID.Text, 500);
            dr["MenuName"] = App_Com.Helper.InputText(this.txtMenuName.Text, 500);
            dr["UrlParameter"] = App_Com.Helper.InputText(this.txtUrlParameter.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
            dr["ModuleID"] = App_Com.Helper.InputText(this.txtModuleID.Text, 500);
            dr["Icon"] = App_Com.Helper.InputText(this.txtIcon.Text, 500);
            FineUI.Icon xIcon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
            if (xIcon != FineUI.Icon.None)
                dr["IconUrl"] = IconHelper.GetIconUrl(xIcon);
            else
                dr["IconUrl"] = IconHelper.GetIconUrl(FineUI.Icon.Image);
            dr["Visibled"] = this.ckbVisibled.Checked;            
            dr["PID"] = this.hidPID.Text;
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Sys_Menu", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_Menu obj = new RDFNew.Module.Admin.Sys.Sys_Menu();
            string Keyword = Request.QueryString["keyword"];
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["MenuName"] = App_Com.Helper.InputText(this.txtMenuName.Text, 500);
                    dr["UrlParameter"] = App_Com.Helper.InputText(this.txtUrlParameter.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["ModuleID"] = App_Com.Helper.InputText(this.txtModuleID.Text, 500);
                    dr["Icon"] = App_Com.Helper.InputText(this.txtIcon.Text, 500);
                    FineUI.Icon xIcon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    if (xIcon != FineUI.Icon.None)
                        dr["IconUrl"] = IconHelper.GetIconUrl(xIcon);
                    else
                        dr["IconUrl"] = IconHelper.GetIconUrl(FineUI.Icon.Image);
                    dr["Visibled"] = this.ckbVisibled.Checked;
                    dr["PID"] = this.hidPID.Text;
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Sys_Menu", "edit"));
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
