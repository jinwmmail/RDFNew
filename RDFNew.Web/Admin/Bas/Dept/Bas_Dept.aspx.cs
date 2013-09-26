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

namespace RDFNew.Web.Admin.Bas.Dept
{
    public partial class Bas_Dept : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Dept";
            B_ModuleName = "部门";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Dept();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void FillControlData()
        {
            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtPDeptID.OnClientTriggerClick = Window1.GetSaveStateReference(
                this.txtPDeptID.ClientID, this.txtPDeptName.ClientID, this.hidPID.ClientID)
                        + Window1.GetShowReference("Bas_DeptList.aspx?action=select");
            }

            if (B_Action.ToLower() == "add")
            {
                this.txtPDeptID.Text = Request.QueryString["ParentID"];
                this.txtPDeptName.Text = Request.QueryString["ParentName"];
                this.txtPDeptName.Text = Server.UrlDecode(this.txtPDeptName.Text);
                this.hidPID.Text = Request.QueryString["ParentRID"];
            }
        }

        protected override void SetControlState()
        {
            this.txtDeptID.Readonly = B_Action.ToLower() != "add";
            this.txtDeptName.Readonly = B_Action.ToLower() == "view";
            this.txtPDeptID.Readonly = B_Action.ToLower() == "view";            
            this.txtRemark.Readonly = B_Action.ToLower() == "view";

            if (B_Action.ToLower() != "view")
            {
                this.txtDeptID.CssStyle = this.txtDeptID.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void GetData(DataRow dr)
        {
            this.txtDeptID.Text = String.Format("{0}", dr["DeptID"]);
            this.txtDeptName.Text = String.Format("{0}", dr["DeptName"]);
            this.txtPDeptID.Text = String.Format("{0}", dr["PDeptID"]);
            this.txtPDeptName.Text = String.Format("{0}", dr["PDeptName"]);                
            this.hidPID.Text = String.Format("{0}", dr["PID"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);            
        }
         
        protected override string AddData()
        {
            RDFNew.Module.Admin.Bas.Bas_Dept obj = new RDFNew.Module.Admin.Bas.Bas_Dept();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Bas_Dept");
            DataRow dr;
            dr = dt.NewRow();
            dr["DeptID"] = App_Com.Helper.InputText(this.txtDeptID.Text, 500);            
            dr["DeptName"] = App_Com.Helper.InputText(this.txtDeptName.Text, 500);            
            dr["PID"] = this.hidPID.Text;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Bas_Dept", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Bas.Bas_Dept obj = new RDFNew.Module.Admin.Bas.Bas_Dept();
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["DeptName"] = App_Com.Helper.InputText(this.txtDeptName.Text, 500);                    
                    dr["PID"] = this.hidPID.Text;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Bas_Dept", "edit"));
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
