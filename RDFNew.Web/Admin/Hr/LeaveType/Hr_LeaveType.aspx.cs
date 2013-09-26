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

namespace RDFNew.Web.Admin.Hr.LeaveType
{
    public partial class Hr_LeaveType : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Hr_LeaveType";
            B_ModuleName = "请假类别";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Hr.Hr_LeaveType();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {
            this.txtLeaveTypeID.Readonly = B_Action.ToLower() != "add";
            this.txtLeaveTypeName.Readonly = B_Action.ToLower() == "view";
            this.ckbIsSalary.Enabled = B_Action.ToLower() == "add";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtLeaveTypeID.CssStyle = this.txtLeaveTypeID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.ckbEnabled.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtLeaveTypeID.Text = String.Format("{0}", dr["LeaveTypeID"]);
            this.txtLeaveTypeName.Text = String.Format("{0}", dr["LeaveTypeName"]);
            this.ckbIsSalary.Checked = Convert.ToBoolean(dr["IsSalary"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {

            RDFNew.Module.Admin.Hr.Hr_LeaveType obj = new RDFNew.Module.Admin.Hr.Hr_LeaveType();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Hr_LeaveType");
            DataRow dr;
            dr = dt.NewRow();
            dr["LeaveTypeID"] = App_Com.Helper.InputText(this.txtLeaveTypeID.Text, 500);            
            dr["LeaveTypeName"] = App_Com.Helper.InputText(this.txtLeaveTypeName.Text, 500);
            dr["IsSalary"] = this.ckbIsSalary.Checked;
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Hr_LeaveType", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Hr.Hr_LeaveType obj = new RDFNew.Module.Admin.Hr.Hr_LeaveType();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["LeaveTypeName"] = App_Com.Helper.InputText(this.txtLeaveTypeName.Text, 500);
                    dr["IsSalary"] = this.ckbIsSalary.Checked;
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Hr_LeaveType", "edit"));
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
