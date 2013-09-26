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
    public partial class Bas_TypeList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Type";
            B_ModuleName = "类别";
            B_PageDetail = "Bas_Type.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Type();
            B_TableKey = "Bas_Type.TypeID";
            B_OrderBy = " Bas_Type.TypeGroup,Bas_Type.Seq ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtTypeID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Type.TypeID", "Like", "TypeID", "%" + Str + "%"));
            Str = this.txtTypeGroup.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Type.TypeGroup", "Like", "TypeGroup", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtTypeID.Text = "";
            this.txtTypeGroup.Text = "";
        }
    }
}
