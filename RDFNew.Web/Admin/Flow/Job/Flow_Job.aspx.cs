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

namespace RDFNew.Web.Admin.Flow.Job
{
    public partial class Flow_Job : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_Job";
            B_ModuleName = "职位";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_Job();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtJobID.Readonly = B_Action.ToLower() != "add";
            this.txtJobName.Readonly = B_Action.ToLower() == "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtJobID.CssStyle = this.txtJobID.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtJobID.Text = String.Format("{0}", dr["JobID"]);
            this.txtJobName.Text = String.Format("{0}", dr["JobName"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {

            RDFNew.Module.Admin.Flow.Flow_Job obj = new RDFNew.Module.Admin.Flow.Flow_Job();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Flow_Job");
            DataRow dr;
            dr = dt.NewRow();
            dr["JobID"] = App_Com.Helper.InputText(this.txtJobID.Text, 500);            
            dr["JobName"] = App_Com.Helper.InputText(this.txtJobName.Text, 500);            
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Flow_Job", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_Job obj = new RDFNew.Module.Admin.Flow.Flow_Job();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["JobName"] = App_Com.Helper.InputText(this.txtJobName.Text, 500);
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Flow_Job", "edit"));
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
    }
}
