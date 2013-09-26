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

namespace RDFNew.Web.Admin.Pur.PoM
{
    public partial class Pur_PoMList : App_Com.PageListMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Pur_PoM";
            B_ModuleName = "采购订单";
            B_PageDetail = "Pur_PoM.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Pur.Pur_PoM();
            B_TableKey = "Pur_PoM.PoMID";
            B_OrderBy = " Pur_PoM.PoMID Desc ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void FillControlData()
        {
            
        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtPoMID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Pur_PoM.PoMID", "Like", "PoMID", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtPoMID.Text = "";            
        }
    }
}
