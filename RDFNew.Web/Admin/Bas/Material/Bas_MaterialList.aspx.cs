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
    public partial class Bas_MaterialList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Material";
            B_ModuleName = "物料";
            B_PageDetail = "Bas_Material.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Material();
            B_TableKey = "Bas_Material.MaterialID";
            B_OrderBy = " Bas_Material.CrtOn Desc ";
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
    }
}
