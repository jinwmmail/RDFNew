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

namespace RDFNew.Web.Admin.Sys.Log
{
    public partial class Sys_Log : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Log";
            B_ModuleName = "系统日志";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_Log();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtLogID.Readonly = B_Action.ToLower() != "add";
            this.txtModule.Readonly = B_Action.ToLower() == "view";
            this.txtPage.Readonly = B_Action.ToLower() == "view";
            this.txtAction.Readonly = B_Action.ToLower() == "view";
            this.txtTable.Readonly = B_Action.ToLower() == "view";
            this.txtKey.Readonly = B_Action.ToLower() == "view";
            this.txtValue.Readonly = B_Action.ToLower() == "view";
            this.txtUser.Readonly = B_Action.ToLower() == "view";
            this.txtWlanIP.Readonly = B_Action.ToLower() == "view";
            this.txtLanIP.Readonly = B_Action.ToLower() == "view";
            this.txtMacAddr.Readonly = B_Action.ToLower() == "view";
            this.txtPCName.Readonly = B_Action.ToLower() == "view";
            this.txtOS.Readonly = B_Action.ToLower() == "view";
            this.txtBrowser.Readonly = B_Action.ToLower() == "view";
            this.txtDateTime.Readonly = B_Action.ToLower() == "view";
            this.txtBackup.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtLogID.CssStyle = this.txtLogID.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtLogID.Text = String.Format("{0}", dr["LogID"]);
            this.txtModule.Text = String.Format("{0}", dr["Module"]);
            this.txtPage.Text = String.Format("{0}", dr["Page"]);
            this.txtAction.Text = String.Format("{0}", dr["Action"]);
            this.txtTable.Text = String.Format("{0}", dr["Table"]);
            this.txtKey.Text = String.Format("{0}", dr["Key"]);
            this.txtValue.Text = String.Format("{0}", dr["Value"]);
            this.txtUser.Text = String.Format("{0}", dr["User"]);
            this.txtWlanIP.Text = String.Format("{0}", dr["WlanIP"]);
            this.txtLanIP.Text = String.Format("{0}", dr["LanIP"]);
            this.txtMacAddr.Text = String.Format("{0}", dr["MacAddr"]);
            this.txtPCName.Text = String.Format("{0}", dr["PCName"]);
            this.txtOS.Text = String.Format("{0}", dr["OS"]);
            this.txtBrowser.Text = String.Format("{0}", dr["Browser"]);
            this.txtDateTime.Text = String.Format("{0}", dr["DateTime"]);
            this.txtBackup.Text = String.Format("{0}", dr["Backup"]);
        }  
    }
}
