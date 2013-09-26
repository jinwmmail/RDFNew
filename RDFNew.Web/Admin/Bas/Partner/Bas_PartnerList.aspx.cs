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
    public partial class Bas_PartnerList : App_Com.PageListSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Bas_Partner";
            B_ModuleName = "商业伙伴";
            B_PageDetail = "Bas_Partner.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Bas.Bas_Partner();
            B_TableKey = "Bas_Partner.PartnerID";
            B_OrderBy = " Bas_Partner.PartnerTypeID,Bas_Partner.CrtOn Desc ";
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
            ", ""), null);
            this.ddlPartnerTypeID.DataSource = dt;
            this.ddlPartnerTypeID.DataTextField = "TypeName";
            this.ddlPartnerTypeID.DataValueField = "TypeID";
            this.ddlPartnerTypeID.DataBind();
            this.ddlPartnerTypeID.Items.Insert(0, new FineUI.ListItem("<全部>", ""));

            if (B_Action.ToUpper() == "SELECT")
            {
                this.Region1.Height = 90;
                String PartnerTypeID = String.IsNullOrEmpty(Request.QueryString["PartnerTypeID"]) ? "" : Request.QueryString["PartnerTypeID"];
                if (PartnerTypeID != "")
                {
                    this.ddlPartnerTypeID.SelectedValue = PartnerTypeID;
                    this.ddlPartnerTypeID.Readonly = true;
                }
            }
        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            string Str = "";
            Str = this.txtPartnerID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Partner.PartnerID", "Like", "PartnerID", "%" + Str + "%"));
            Str = this.txtPartnerName.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Partner.PartnerName", "Like", "PartnerID", "%" + Str + "%"));
            Str = ddlPartnerTypeID.SelectedValue;
            if (ddlPartnerTypeID.SelectedIndex>0)
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Partner.PartnerTypeID", "=", "PartnerTypeID", Str));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtPartnerID.Text = "";
            this.txtPartnerName.Text = "";
        }
    }
}
