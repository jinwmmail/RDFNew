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

namespace RDFNew.Web.Admin.Bas.Employee
{
    public partial class Bas_Employee : App_Com.PageSingle 
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Employee";
            B_ModuleName = "雇员";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Employee();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtDeptID.OnClientTriggerClick = Window1.GetSaveStateReference(
                    txtDeptID.ClientID, txtDeptName.ClientID)
                    + Window1.GetShowReference("../Dept/Bas_DeptList.aspx?action=select");
            }
        }

        protected override void SetControlState()
        {
            this.txtEmployeeID.Readonly = B_Action.ToLower() != "add";
            this.txtEmployeeName.Readonly = B_Action.ToLower() == "view";
            this.txtNameE.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.ckbIsUser.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            this.txtPhone.Readonly = B_Action.ToLower() == "view";
            this.txtEmail.Readonly = B_Action.ToLower() == "view";
            this.txtDeptID.Readonly = B_Action.ToLower() == "view";
            this.idFile.Enabled = B_Action.ToLower() != "view";
            this.ckbDeleteImg.Enabled = B_Action.ToLower() != "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtEmployeeID.CssStyle = this.txtEmployeeID.Readonly ? "background:#c0c0c0;" : "";
            }
         
            this.ckbEnabled.Checked = true;
            this.ckbIsUser.Checked = true;
        }

        protected override void GetData(DataRow dr)
        {
            this.txtEmployeeID.Text = String.Format("{0}", dr["EmployeeID"]);
            this.txtEmployeeName.Text = String.Format("{0}", dr["EmployeeName"]);
            this.txtNameE.Text = String.Format("{0}", dr["NameE"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.ckbIsUser.Checked = Convert.ToBoolean(dr["IsUser"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
            this.txtPhone.Text = String.Format("{0}", dr["Phone"]);
            this.txtEmail.Text = String.Format("{0}", dr["Email"]);
            this.txtDeptID.Text = String.Format("{0}", dr["DeptID"]);
            this.txtDeptName.Text = String.Format("{0}", dr["DeptName"]);
            this.idImg.ImageUrl = String.Format("{0}", dr["ImageS"]);
        }

        protected override string AddData()
        {
                RDFNew.Module.Admin.Bas.Bas_Employee obj = new RDFNew.Module.Admin.Bas.Bas_Employee();
                DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Bas_Employee");
                DataRow dr;
                dr = dt.NewRow();
                dr["EmployeeID"] = App_Com.Helper.InputText(this.txtEmployeeID.Text, 500);                
                dr["EmployeeName"] = App_Com.Helper.InputText(this.txtEmployeeName.Text, 500);
                dr["NameE"] = App_Com.Helper.InputText(this.txtNameE.Text, 500);                
                dr["Enabled"] = this.ckbEnabled.Checked;
                dr["IsUser"] = this.ckbIsUser.Checked;
                dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                dr["Phone"] = App_Com.Helper.InputText(this.txtPhone.Text, 500);
                dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
                dr["DeptID"] = App_Com.Helper.InputText(this.txtDeptID.Text, 500);
                if (this.idFile.FileName != "")
                {
                    object[] os = App_Com.ImageHelper.UploadImage(this.idFile, String.Format("~/_Upload/Images/{0}/", this.GetType().BaseType.Name),"SML");
                    if (os[0].ToString() == "0")
                    {
                        dr["ImageL"] = os[1];
                        dr["ImageM"] = os[2];
                        dr["ImageS"] = os[3];
                    }
                }
                dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
                object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                            App_Com.Helper.BuildLog("Bas_Employee", "add"));
                if (data[0].ToString() != "0") //正常                
                    throw data[1] as Exception;
                else
                    return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Bas.Bas_Employee obj = new RDFNew.Module.Admin.Bas.Bas_Employee();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];                    
                    dr["EmployeeName"] = App_Com.Helper.InputText(this.txtEmployeeName.Text, 500);
                    dr["NameE"] = App_Com.Helper.InputText(this.txtNameE.Text, 500);                    
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["IsUser"] = this.ckbIsUser.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                    dr["Phone"] = App_Com.Helper.InputText(this.txtPhone.Text, 500);
                    dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
                    dr["DeptID"] = App_Com.Helper.InputText(this.txtDeptID.Text, 500);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (this.idFile.FileName != "")
                    {
                        object[] os = App_Com.ImageHelper.UploadImage(this.idFile, String.Format("~/_Upload/Images/{0}/", this.GetType().BaseType.Name), "SML");
                        if (os[0].ToString() == "0")
                        {
                            dr["ImageL"] = os[1];
                            dr["ImageM"] = os[2];
                            dr["ImageS"] = os[3];
                        }
                    }
                    if (this.ckbDeleteImg.Checked)
                        App_Com.ImageHelper.DeleteImage(dr, true);               
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Bas_Employee", "edit"));
                    if (data[0].ToString() != "0") //正常                
                        throw data[1] as Exception;
                    else
                    {
                        if (this.idFile.FileName != "") //删除旧图片
                            App_Com.ImageHelper.DeleteImage(dr, false);
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
