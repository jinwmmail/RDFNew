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

namespace RDFNew.Web.Admin.Bas.Partner
{
    public partial class Bas_Partner : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Partner";
            B_ModuleName = "商业伙伴";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Partner();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select TypeID,TypeName 
                From Bas_Type 
                Where TypeGroup='PartnerTypeID' {0}
                Order By Seq
            ",B_Action.ToUpper()=="ADD"?" And Enabled=1 ":""), null);
            this.ddlPartnerTypeID.DataSource = dt;
            this.ddlPartnerTypeID.DataTextField = "TypeName";
            this.ddlPartnerTypeID.DataValueField = "TypeID";
            this.ddlPartnerTypeID.DataBind();           
        }

        protected override void SetControlState()
        {
            this.txtPartnerID.Readonly = B_Action.ToLower() != "add";
            this.txtPartnerName.Readonly = B_Action.ToLower() == "view";
            this.ddlPartnerTypeID.Readonly = B_Action.ToLower() != "add";
            this.txtAddress.Readonly = B_Action.ToLower() == "view";
            this.txtLinker.Readonly = B_Action.ToLower() == "view";
            this.txtTel.Readonly = B_Action.ToLower() == "view";
            this.txtFax.Readonly = B_Action.ToLower() == "view";
            this.txtPhone.Readonly = B_Action.ToLower() == "view";
            this.txtEmail.Readonly = B_Action.ToLower() == "view";
            this.txtQQ.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtPartnerID.CssStyle = this.txtPartnerID.Readonly?"background:#c0c0c0;":"";                
            }
            this.ckbEnabled.Checked = true;            
        }

        protected override void GetData(DataRow dr)
        {
            this.txtPartnerID.Text = String.Format("{0}", dr["PartnerID"]);
            this.txtPartnerName.Text = String.Format("{0}", dr["PartnerName"]);
            this.ddlPartnerTypeID.SelectedValue = String.Format("{0}", dr["PartnerTypeID"]);
            this.txtAddress.Text = String.Format("{0}", dr["Address"]);
            this.txtLinker.Text = String.Format("{0}", dr["Linker"]);
            this.txtTel.Text = String.Format("{0}", dr["Tel"]);
            this.txtFax.Text = String.Format("{0}", dr["Fax"]);
            this.txtPhone.Text = String.Format("{0}", dr["Phone"]);
            this.txtEmail.Text = String.Format("{0}", dr["Email"]);
            this.txtQQ.Text = String.Format("{0}", dr["QQ"]);
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Bas.Bas_Partner obj = new RDFNew.Module.Admin.Bas.Bas_Partner();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Bas_Partner");
            DataRow dr;
            dr = dt.NewRow();
            dr["PartnerID"] = App_Com.Helper.InputText(this.txtPartnerID.Text, 500);
            dr["PartnerName"] = App_Com.Helper.InputText(this.txtPartnerName.Text, 500);
            dr["PartnerTypeID"] = this.ddlPartnerTypeID.SelectedValue;
            dr["Address"] = App_Com.Helper.InputText(this.txtAddress.Text, 500);
            dr["Linker"] = App_Com.Helper.InputText(this.txtLinker.Text, 500);
            dr["Tel"] = App_Com.Helper.InputText(this.txtTel.Text, 500);
            dr["Fax"] = App_Com.Helper.InputText(this.txtFax.Text, 500);
            dr["Phone"] = App_Com.Helper.InputText(this.txtPhone.Text, 500);
            dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
            dr["QQ"] = App_Com.Helper.InputText(this.txtQQ.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Bas_Partner", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Bas.Bas_Partner obj = new RDFNew.Module.Admin.Bas.Bas_Partner();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["PartnerName"] = App_Com.Helper.InputText(this.txtPartnerName.Text, 500);
                    dr["Address"] = App_Com.Helper.InputText(this.txtAddress.Text, 500);
                    dr["Linker"] = App_Com.Helper.InputText(this.txtLinker.Text, 500);
                    dr["Tel"] = App_Com.Helper.InputText(this.txtTel.Text, 500);
                    dr["Fax"] = App_Com.Helper.InputText(this.txtFax.Text, 500);
                    dr["Phone"] = App_Com.Helper.InputText(this.txtPhone.Text, 500);
                    dr["Email"] = App_Com.Helper.InputText(this.txtEmail.Text, 500);
                    dr["QQ"] = App_Com.Helper.InputText(this.txtQQ.Text, 500);

                    dr["Enabled"] = this.ckbEnabled.Checked;                  
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Bas_Partner", "edit"));
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
