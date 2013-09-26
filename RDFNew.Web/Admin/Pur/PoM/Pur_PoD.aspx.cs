using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.Admin.Pur.PoM
{
    public partial class Pur_PoD : App_Com.PageSingle 
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Pur_PoM";
            B_ModuleName = "采购订单";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Pur.Pur_PoD();

            B_DetailSessionKey = "Pur_PoDAdd";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetControlState()
        {            
            this.txtSeq.Readonly = B_Action.ToLower() == "view";            
            this.txtQty.Readonly = B_Action.ToLower() == "view";
            this.txtPrice.Readonly = B_Action.ToLower() == "view";         
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtSeq.CssStyle = this.txtSeq.Readonly ? "background:#c0c0c0;" : "";
                this.txtMaterialID.CssStyle = this.txtMaterialID.Readonly ? "background:#c0c0c0;" : "";
                this.txtMaterialName.CssStyle = this.txtMaterialName.Readonly ? "background:#c0c0c0;" : "";
                this.txtUnitID.CssStyle = this.txtUnitID.Readonly ? "background:#c0c0c0;" : "";
                this.txtQty.CssStyle = this.txtQty.Readonly ? "text-align:right;background:#c0c0c0;" : "text-align:right;";
                this.txtPrice.CssStyle = this.txtPrice.Readonly ? "text-align:right;background:#c0c0c0;" : "text-align:right;";
                this.txtAmt.CssStyle = this.txtAmt.Readonly ? "text-align:right;background:#c0c0c0;" : "text-align:right;";
                this.txtRemark.CssStyle = this.txtRemark.Readonly ? "background:#c0c0c0;" : "";
            }
        }

        protected override void LoadMater()
        {            
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt != null)
            {
                DataRow[] drs = dt.Select("PoDID='" + B_Keyword + "'");                
                if (drs.Length > 0)
                {
                    DataRow dr=drs[0];
                    this.txtSeq.Text = String.Format("{0}", dr["Seq"]);
                    this.txtMaterialID.Text = String.Format("{0}", dr["MaterialID"]);
                    this.txtMaterialName.Text = String.Format("{0}", dr["MaterialName"]);
                    this.txtUnitID.Text = String.Format("{0}", dr["UnitID"]);
                    this.txtQty.Text = String.Format("{0}", dr["Qty"]);
                    this.txtPrice.Text = String.Format("{0}", dr["Price"]);
                    this.txtAmt.Text = String.Format("{0}", dr["Amt"]);
                    this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
                }
            }
        }

        protected override string UpdateData()
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt != null)
            {
                DataRow[] drs = dt.Select("PoDID='" + B_Keyword + "'");
                if (drs.Length > 0)
                {
                    DataRow dr = drs[0];                    
                    dr["Seq"] = App_Com.Helper.InputText(this.txtSeq.Text, 500);
                    dr["MaterialID"] = App_Com.Helper.InputText(this.txtMaterialID.Text, 500);
                    dr["UnitID"] = App_Com.Helper.InputText(this.txtUnitID.Text, 500);
                    dr["Qty"] = App_Com.Helper.InputText(this.txtQty.Text, 500);
                    dr["Price"] = App_Com.Helper.InputText(this.txtPrice.Text, 500);                    
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);
                }
            }
            return "";
        }

        protected override void btn_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;
            if (btn.ID == "SaveAndExit")
            {
                try
                {
                    string RtnKey = "";
                    if (B_Action.ToLower() == "add")
                        RtnKey = AddData();
                    if (B_Action.ToLower() == "edit")
                        RtnKey = UpdateData();                    
                    //更新父页面                                        
                    PageContext.RegisterStartupScript(FineUI.ActiveWindow.GetHidePostBackReference("ChangedDetail"));
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    while (ex != null)
                    {
                        sb.Append(ex.Message + "\r\n");
                        ex = ex.InnerException;
                    }
                    Alert.Show("发生如下错误:\r\n" + sb.ToString(), MessageBoxIcon.Error);
                }
            }
        }
    }
}
