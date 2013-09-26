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

namespace RDFNew.Web.Admin.Sys.FMonth
{
    public partial class Sys_FMonth : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Sys_FMonth";
            B_ModuleName = "财务月份";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Sys.Sys_FMonth();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtFMonthID.Readonly = B_Action.ToLower() != "add";
            this.ckbClosed.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtFMonthID.CssStyle = this.txtFMonthID.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtFMonthID.Text = String.Format("{0}", dr["FMonthID"]);
            this.ckbClosed.Checked = Convert.ToBoolean(dr["Closed"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Sys.Sys_FMonth obj = new RDFNew.Module.Admin.Sys.Sys_FMonth();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Sys_FMonth");
            DataRow dr;
            dr = dt.NewRow();
            dr["FMonthID"] = App_Com.Helper.InputText(this.txtFMonthID.Text, 500);
            dr["Closed"] = this.ckbClosed.Checked;
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Sys_FMonth", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_FMonth obj = new RDFNew.Module.Admin.Sys.Sys_FMonth();
            string Keyword = Request.QueryString["keyword"];
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["Closed"] = this.ckbClosed.Checked;
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Sys_FMonth", "edit"));
                    if (data[0].ToString() != "0") //正常                
                        throw data[1] as Exception;
                    else
                    {
                        return data[1].ToString();
                    }
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
    }
}
