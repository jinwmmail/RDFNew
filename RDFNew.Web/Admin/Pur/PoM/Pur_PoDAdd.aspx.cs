using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using FineUI;

namespace RDFNew.Web.Admin.Pur.PoM
{
    public partial class Pur_PoDAdd : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
            B_ModuleID = "Pur_PoM";
            B_ModuleName = "采购订单";
            B_ToolBar1 = this.Toolbar1;            
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Material();
            B_TableKey = "Bas_Material.MaterialID";
            B_OrderBy = " Bas_Material.CrtOn Desc ";

            B_DetailSessionKey = "Pur_PoDAdd";
            B_ReturnEmpty = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtMaterialID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Material.MaterialID", "Like", "MaterialID", "%" + Str + "%"));
            Str = this.txtMaterialName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Material.MaterialName", "Like", "MaterialName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtMaterialID.Text = "";
            this.txtMaterialName.Text = "";
        }

        protected override void SelectData(string Fr)
        {
            if (B_Grid1 != null)
            {
                if (Fr.ToUpper() == "SELECT")
                {
                    int selectedCount = B_Grid1.SelectedRowIndexArray.Length;
                    if (selectedCount == 0)
                    {
                        PageContext.RegisterStartupScript(B_Grid1.GetNoSelectionAlertInTopReference("请至少选择一项!"));
                        return;
                    }
                    DataTable dt;
                    dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
                    if (dt != null)
                    {
                        DataRow drNew;
                        GridRow row;
                        System.Web.UI.WebControls.TextBox nb;
                        foreach (int i in Grid1.SelectedRowIndexArray)
                        {
                            row = Grid1.Rows[i];
                            drNew = dt.NewRow();
                            drNew["PoDID"] = GetRowID(dt, "PoDID");
                            drNew["Seq"] = GetSeq(dt,"Seq");
                            drNew["MaterialID"] = row.Values[0];
                            drNew["MaterialName"] = row.Values[1];
                            drNew["UnitID"] = row.Values[2];
                            nb = row.FindControl("txtQty") as System.Web.UI.WebControls.TextBox;
                            drNew["Qty"] = nb.Text;
                            nb = row.FindControl("txtPrice") as System.Web.UI.WebControls.TextBox;
                            drNew["Price"] = nb.Text;
                            dt.Rows.Add(drNew);
                        }
                    }
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference("ChangedDetail")); 
                }
            }
        }
    }
}
