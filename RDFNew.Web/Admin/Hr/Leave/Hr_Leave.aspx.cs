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

namespace RDFNew.Web.Admin.Hr.Leave
{
    public partial class Hr_Leave : App_Com.PageFlow
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Hr_Leave";
            B_ModuleName = "请假申请单";
            B_Window1 = this.Window1;
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Hr.Hr_Leave();

            F_FileName = "Hr_Leave.pdl";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select LeaveTypeID,LeaveTypeName 
                From Hr_LeaveType 
                Where 1=1 {0}                
            ", B_Action.ToUpper() == "ADD" ? " And Enabled=1 " : ""), null);
            this.ddlLeaveTypeID.DataSource = dt;
            this.ddlLeaveTypeID.DataTextField = "LeaveTypeName";
            this.ddlLeaveTypeID.DataValueField = "LeaveTypeID";
            this.ddlLeaveTypeID.DataBind();    

            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtEmployeeID.OnClientTriggerClick = Window1.GetSaveStateReference(
                    txtEmployeeID.ClientID, txtEmployeeName.ClientID)
                        + Window1.GetShowReference("../../Bas/Employee/Bas_EmployeeList.aspx?action=select");
            }
        }

        protected override void SetControlState()
        {
            this.txtLeaveID.Readonly = B_Action.ToLower() != "add";
            this.txtLeaveDate.Readonly = B_Action.ToLower() == "view";
            this.txtEmployeeID.Readonly = B_Action.ToLower() != "add";
            this.ddlLeaveTypeID.Enabled = B_Action.ToLower() == "add";
            this.txtDTFrom.Readonly = B_Action.ToLower() == "view";
            this.txtDTFromT.Readonly = B_Action.ToLower() == "view";
            this.txtDTTo.Readonly = B_Action.ToLower() == "view";
            this.txtDTToT.Readonly = B_Action.ToLower() == "view";
            this.txtTimes.Readonly = B_Action.ToLower() == "view";
            this.txtCause.Readonly = B_Action.ToLower() == "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtLeaveID.CssStyle = this.txtLeaveID.Readonly ? "background:#c0c0c0;" : "";
            }

            this.txtLeaveDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEmployeeID.Text = App_Com.Sys_User.GetUserInfo("EmployeeID");
            if (this.txtEmployeeID.Text.Trim()!="")
                this.txtEmployeeName.Text = App_Com.Sys_User.GetUserInfo("UserName");
            this.txtDTFrom.Text = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            this.txtDTFromT.Text = "08:30";
            this.txtDTTo.Text = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            this.txtDTToT.Text = "17:00";
            this.txtTimes.Text =App_Com.Helper.GetDiffMinutes(this.txtDTFrom.Text, this.txtDTFromT.Text, 
                this.txtDTTo.Text,this.txtDTToT.Text).ToString();
        }

        protected override void GetData(DataRow dr)
        {
            this.txtLeaveID.Text = String.Format("{0}", dr["LeaveID"]);
            this.txtLeaveDate.Text = String.Format("{0}", dr["LeaveDate"]);
            this.txtEmployeeID.Text = String.Format("{0}", dr["EmployeeID"]);
            this.txtEmployeeName.Text = String.Format("{0}", dr["EmployeeName"]);
            this.ddlLeaveTypeID.SelectedValue = String.Format("{0}", dr["LeaveTypeID"]);
            this.txtDTFrom.Text = Convert.ToDateTime(dr["DTFrom"]).ToString("yyyy-MM-dd");
            this.txtDTFromT.Text = Convert.ToDateTime(dr["DTFrom"]).ToString("HH:mm");
            this.txtDTTo.Text = Convert.ToDateTime(dr["DTTo"]).ToString("yyyy-MM-dd");
            this.txtDTToT.Text = Convert.ToDateTime(dr["DTTo"]).ToString("HH:mm");
            this.txtTimes.Text = String.Format("{0}", dr["Times"]);
            this.txtCause.Text = String.Format("{0}", dr["Cause"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {

            RDFNew.Module.Admin.Hr.Hr_Leave obj = new RDFNew.Module.Admin.Hr.Hr_Leave();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Hr_Leave");
            DataRow dr;
            dr = dt.NewRow();
            dr["LeaveID"] = App_Com.Helper.InputText(this.txtLeaveID.Text, 500);            
            dr["LeaveDate"] = App_Com.Helper.InputText(this.txtLeaveDate.Text, 500);
            dr["EmployeeID"] = App_Com.Helper.InputText(this.txtEmployeeID.Text, 500);
            dr["LeaveTypeID"] = this.ddlLeaveTypeID.SelectedValue;
            dr["DTFrom"] = this.txtDTFrom.Text + " " + this.txtDTFromT.Text;
            dr["DTTo"] = this.txtDTTo.Text + " " + this.txtDTToT.Text;
            App_Com.Helper.CheckDiffMinutes(this.txtDTFrom.Text, this.txtDTFromT.Text,
                this.txtDTTo.Text, this.txtDTToT.Text);
            this.txtTimes.Text = App_Com.Helper.GetDiffMinutes(this.txtDTFrom.Text, this.txtDTFromT.Text,
                this.txtDTTo.Text, this.txtDTToT.Text).ToString();
            dr["Times"] = this.txtTimes.Text;
            dr["Cause"] = this.txtCause.Text.Trim();
            dr["Remark"] = this.txtRemark.Text.Trim();     

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Hr_Leave", "add"),
                                        GetFlow_ToDoM());
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Hr.Hr_Leave obj = new RDFNew.Module.Admin.Hr.Hr_Leave();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["LeaveDate"] = App_Com.Helper.InputText(this.txtLeaveDate.Text, 500);
                    dr["EmployeeID"] = App_Com.Helper.InputText(this.txtEmployeeID.Text, 500);
                    dr["DTFrom"] = this.txtDTFrom.Text + " " + this.txtDTFromT.Text;
                    dr["DTTo"] = this.txtDTTo.Text + " " + this.txtDTToT.Text;
                    App_Com.Helper.CheckDiffMinutes(this.txtDTFrom.Text, this.txtDTFromT.Text,
                        this.txtDTTo.Text, this.txtDTToT.Text);
                    this.txtTimes.Text = App_Com.Helper.GetDiffMinutes(this.txtDTFrom.Text, this.txtDTFromT.Text,
                        this.txtDTTo.Text, this.txtDTToT.Text).ToString();
                    dr["Times"] = this.txtTimes.Text;
                    dr["Cause"] = this.txtCause.Text.Trim();
                    dr["Remark"] = this.txtRemark.Text.Trim();                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Hr_Leave", "edit"));
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

        DataTable GetFlow_ToDoM()
        {
            GetDeployInfo();
            String DeployKey = this.F_DeployKey;
            if (DeployKey == "")
                throw new Exception("未设置流程文件名称或流程未部署至服务器.");
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Flow_ToDoM");
            DataRow dr;
            dr = dt.NewRow();
            DS.XBPM.API.ProcessInstance instance = App_Com.FlowHelper.Engine.GetExecutionService().StartProcessInstance(
                DeployKey);
            if (instance == null)
            {
                throw new Exception("流程实例创建失败.");
            }
            else
            {
                dr["InstanceID"] = instance.InstanceId;
            }
            dr["Description"] = F_DeployName;
            dr["BillPage"] = Request.Url.AbsolutePath;
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            return dt;
        }

    }
}
