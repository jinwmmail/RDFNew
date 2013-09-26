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

namespace RDFNew.Web.Admin.Bas.Type
{
    public partial class Bas_Type : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Type";
            B_ModuleName = "类别";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Type();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            if (B_Action.ToLower() == "add")
            {
                DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(@"Select Distinct TypeGroup From Bas_Type", null);
                this.ddlTypeGroup.DataSource = dt;
                this.ddlTypeGroup.DataTextField = "TypeGroup";
                this.ddlTypeGroup.DataValueField = "TypeGroup";
                this.ddlTypeGroup.DataBind();
                this.ddlTypeGroup.Items.Insert(0, new FineUI.ListItem("-快选大类-", ""));
            }
        }

        protected override void SetControlState()
        {
            this.txtTypeID.Readonly = B_Action.ToLower() != "add";            
            this.txtTypeGroup.Readonly = B_Action.ToLower() != "add";
            this.ddlTypeGroup.Hidden = B_Action.ToLower() != "add";
            this.txtSeq.Readonly = B_Action.ToLower() != "add";
            this.txtTypeName.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtTypeID.CssStyle = this.txtTypeID.Readonly?"background:#c0c0c0;":"";
                this.txtTypeGroup.CssStyle = this.txtTypeGroup.Readonly ? "background:#c0c0c0;" : "";
                this.txtSeq.CssStyle = this.txtSeq.Readonly ? "background:#c0c0c0;" : "";                
            }
            this.ckbEnabled.Checked = true; 
        }

        protected override void GetData(DataRow dr)
        {
            this.txtTypeID.Text = String.Format("{0}", dr["TypeID"]);
            this.txtTypeGroup.Text = String.Format("{0}", dr["TypeGroup"]);
            this.txtSeq.Text = String.Format("{0}", dr["Seq"]);
            this.txtTypeName.Text = String.Format("{0}", dr["TypeName"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Bas.Bas_Type obj = new RDFNew.Module.Admin.Bas.Bas_Type();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Bas_Type");
            DataRow dr;
            dr = dt.NewRow();
            dr["TypeID"] = App_Com.Helper.InputText(this.txtTypeID.Text, 500);
            dr["TypeGroup"] = App_Com.Helper.InputText(this.txtTypeGroup.Text, 500);
            dr["Seq"] = App_Com.Helper.InputText(this.txtSeq.Text, 500);
            dr["TypeName"] = App_Com.Helper.InputText(this.txtTypeName.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Bas_Type", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Bas.Bas_Type obj = new RDFNew.Module.Admin.Bas.Bas_Type();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];                   
                    dr["TypeName"] = App_Com.Helper.InputText(this.txtTypeName.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Bas_Type", "edit"));
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
        
        protected void ddlTypeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTypeGroup.SelectedIndex > 0)
                this.txtTypeGroup.Text = ddlTypeGroup.SelectedText;
            else
                this.txtTypeGroup.Text = "";
        }
    }
}
