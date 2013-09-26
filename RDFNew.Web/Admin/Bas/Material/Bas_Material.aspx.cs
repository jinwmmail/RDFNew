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

namespace RDFNew.Web.Admin.Bas.Material
{
    public partial class Bas_Material : App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Material";
            B_ModuleName = "物料";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Material();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            if (B_Action.ToLower() == "add")
            {

            }
        }

        protected override void SetControlState()
        {
            this.txtMaterialID.Readonly = B_Action.ToLower() != "add";
            this.txtMaterialName.Readonly = B_Action.ToLower() == "view";
            this.txtMaterialSpec.Readonly = B_Action.ToLower() == "view";
            this.txtUnitID.Readonly = B_Action.ToLower() == "view";
            this.txtPricePur.Readonly = B_Action.ToLower() == "view";
            this.txtPriceCost.Readonly = B_Action.ToLower() == "view";
            this.txtPriceSal.Readonly = B_Action.ToLower() == "view";
            this.ckbEnabled.Enabled = B_Action.ToLower() != "view";
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtMaterialID.CssStyle = this.txtMaterialID.Readonly?"background:#c0c0c0;":"";                
            }
            this.ckbEnabled.Checked = true;

            this.txtMaterialSpec.Text = "0";
            this.txtPricePur.Text = "0";
            this.txtPriceCost.Text = "0";
            this.txtPriceSal.Text = "0";
        }

        protected override void GetData(DataRow dr)
        {
            this.txtMaterialID.Text = String.Format("{0}", dr["MaterialID"]);
            this.txtMaterialName.Text = String.Format("{0}", dr["MaterialName"]);
            this.txtMaterialSpec.Text = String.Format("{0}", dr["MaterialSpec"]);
            this.txtUnitID.Text = String.Format("{0}", dr["UnitID"]);
            this.txtPricePur.Text = String.Format("{0}", dr["PricePur"]);
            this.txtPriceCost.Text = String.Format("{0}", dr["PriceCost"]);
            this.txtPriceSal.Text = String.Format("{0}", dr["PriceSal"]);            
            this.ckbEnabled.Checked = Convert.ToBoolean(dr["Enabled"]);
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
        }

        protected override string AddData()
        {
            RDFNew.Module.Admin.Bas.Bas_Material obj = new RDFNew.Module.Admin.Bas.Bas_Material();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Bas_Material");
            DataRow dr;
            dr = dt.NewRow();
            dr["MaterialID"] = App_Com.Helper.InputText(this.txtMaterialID.Text, 500);
            dr["MaterialName"] = App_Com.Helper.InputText(this.txtMaterialName.Text, 500);
            dr["MaterialSpec"] = App_Com.Helper.InputText(this.txtMaterialSpec.Text, 500);
            dr["UnitID"] = App_Com.Helper.InputText(this.txtUnitID.Text, 500);
            dr["PricePur"] = App_Com.Helper.InputText(this.txtPricePur.Text, 500);
            dr["PriceCost"] = App_Com.Helper.InputText(this.txtPriceCost.Text, 500);
            dr["PriceSal"] = App_Com.Helper.InputText(this.txtPriceSal.Text, 500);
            dr["Enabled"] = this.ckbEnabled.Checked;
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Bas_Material", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Bas.Bas_Material obj = new RDFNew.Module.Admin.Bas.Bas_Material();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["MaterialName"] = App_Com.Helper.InputText(this.txtMaterialName.Text, 500);
                    dr["MaterialSpec"] = App_Com.Helper.InputText(this.txtMaterialSpec.Text, 500);
                    dr["UnitID"] = App_Com.Helper.InputText(this.txtUnitID.Text, 500);
                    dr["PricePur"] = App_Com.Helper.InputText(this.txtPricePur.Text, 500);
                    dr["PriceCost"] = App_Com.Helper.InputText(this.txtPriceCost.Text, 500);
                    dr["PriceSal"] = App_Com.Helper.InputText(this.txtPriceSal.Text, 500);
                    dr["Enabled"] = this.ckbEnabled.Checked;                  
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Bas_Material", "edit"));
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
