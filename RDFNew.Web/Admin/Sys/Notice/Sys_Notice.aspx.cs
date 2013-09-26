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
namespace RDFNew.Web.Admin.Sys.Notice
{
    public partial class Sys_Notice : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_Notice";
            B_ModuleName = "系统公告";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL =new RDFNew .Module .Admin .Sys .Sys_Notice() ;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtNoticeID.Readonly = B_Action.ToLower() != "add";
            this.txtNoticeTitle.Readonly = B_Action.ToLower() == "view";
            this.txtNoticeContent.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtNoticeID.CssStyle = this.txtNoticeID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtNoticeID.Text = String.Format("{0}", dr["NoticeID"]);
            this.txtNoticeTitle.Text = String.Format("{0}", dr["NoticeTitle"]);
            this.txtNoticeContent.Text = String.Format("{0}", dr["NoticeContent"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
        }

        protected override string AddData()
        {
            RDFNew .Module .Admin .Sys .Sys_Notice obj = new RDFNew .Module .Admin .Sys .Sys_Notice();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, B_ModuleID);
            DataRow dr;
            dr = dt.NewRow();
            dr["NoticeID"] = App_Com.Helper.InputText(this.txtNoticeID.Text, 500);
            dr["NoticeTitle"] = App_Com.Helper.InputText(this.txtNoticeTitle.Text, 500);
            dr["NoticeContent"] = this.txtNoticeContent.Text.Trim();
            dr["Enabled"] = this.ckbEnabled.Checked;
            
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog(B_ModuleID, "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_Notice obj = new RDFNew.Module.Admin.Sys.Sys_Notice();
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["NoticeTitle"] = App_Com.Helper.InputText(this.txtNoticeTitle.Text, 500);
                    dr["NoticeContent"] = this.txtNoticeContent.Text.Trim();
                    dr["Enabled"] = this.ckbEnabled.Checked;

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog(B_ModuleID, "edit"));
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

        public string GetAction()
        {
            return B_Action.ToUpper();
        }
    }
}
