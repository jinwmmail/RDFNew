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
using Newtonsoft.Json.Linq;

namespace RDFNew.Web.Admin.Pur.PoM
{
    public partial class Pur_PoM : App_Com.PageMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Pur_PoM";
            B_ModuleName = "采购订单";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Pur.Pur_PoM();

            B_ToolBar2 = this.Toolbar2;
            B_Grid1 = this.Grid1;
            B_Window1 = this.Window1;
            B_PageDetail = "Pur_PoD.aspx";
            B_PageDetailAdd = "Pur_PoDAdd.aspx";

            B_DetailSessionKey = "Pur_PoDAdd";
            B_PrintDetail = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack && Request.Form["__EVENTARGUMENT"] == "ChangedDetail")
            {                                
                LoadDetail();
            }
        }

        protected override void FillControlData()
        {
            App_Com.Helper.SetSession(B_DetailSessionKey, null);

            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                this.txtPartnerID.OnClientTriggerClick = Window1.GetSaveStateReference(
                    txtPartnerID.ClientID, txtPartnerName.ClientID)
                        + Window1.GetShowReference("../../Bas/Partner/Bas_PartnerList.aspx?action=select&PartnerTypeID=201300000001");                
            }
        }
       
        protected override void SetControlState()
        {
            this.txtPoMID.Readonly = B_Action.ToLower() != "add";
            this.txtPoMDate.Readonly = B_Action.ToLower() == "view";
            this.txtDeliveryDate.Readonly = B_Action.ToLower() == "view";
            this.txtPartnerID.Readonly = B_Action.ToLower() == "view";
            
            this.txtRemark.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtPoMID.CssStyle = this.txtPoMID.Readonly?"background:#c0c0c0;":"";                
            }
            this.txtPoMDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected override void GetData(DataRow dr)
        {
            this.txtPoMID.Text = String.Format("{0}", dr["PoMID"]);            
            this.txtPoMDate.Text = String.Format("{0}", dr["PoMDate"]);
            this.txtPartnerID.Text = String.Format("{0}", dr["PartnerID"]);
            this.txtPartnerName.Text = String.Format("{0}", dr["PartnerName"]);
            this.txtDeliveryDate.Text = String.Format("{0}", dr["DeliveryDate"]);            
            this.txtRemark.Text = String.Format("{0}", dr["Remark"]);
            
            LoadDetail();
        }

        protected override void LoadDetail()
        {            
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt == null)
            {
                RDFNew.Module.Admin.Pur.Pur_PoD da = new RDFNew.Module.Admin.Pur.Pur_PoD();
                dt = da.GetDataByParent(B_Keyword)[1] as DataTable;
                DataColumn dc;
                dc = new DataColumn("Amt", typeof(decimal), "Qty*Price"); dt.Columns.Add(dc);
                App_Com.Helper.SetSession(B_DetailSessionKey, dt);
            }
            OutputSummaryData(dt);
            this.Grid1.DataSource = dt;
            this.Grid1.DataBind();
        }
        
        protected override string AddData()
        {
            RDFNew.Module.Admin.Pur.Pur_PoM obj = new RDFNew.Module.Admin.Pur.Pur_PoM();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Pur_PoM");
            DataRow dr;
            dr = dt.NewRow();
            dr["PoMID"] = App_Com.Helper.InputText(this.txtPoMID.Text, 500);
            dr["PoMDate"] = App_Com.Helper.InputText(this.txtPoMDate.Text, 500);
            dr["PartnerID"] = App_Com.Helper.InputText(this.txtPartnerID.Text, 500);
            dr["DeliveryDate"] = App_Com.Helper.InputText(this.txtDeliveryDate.Text, 500);            
            dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);

            BuildDetail();
            DataTable dtDetail = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),dtDetail,
                                        App_Com.Helper.BuildLog("Pur_PoM", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Pur.Pur_PoM obj = new RDFNew.Module.Admin.Pur.Pur_PoM();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];

                    dr["PoMDate"] = App_Com.Helper.InputText(this.txtPoMDate.Text, 500);
                    dr["PartnerID"] = App_Com.Helper.InputText(this.txtPartnerID.Text, 500);
                    dr["DeliveryDate"] = App_Com.Helper.InputText(this.txtDeliveryDate.Text, 500);            
                    dr["Remark"] = App_Com.Helper.InputText(this.txtRemark.Text, 500);                   

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    BuildDetail();
                    DataTable dtDetail = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),dtDetail,
                                            App_Com.Helper.BuildLog("Pur_PoM", "edit"));
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

        protected override void DeleteDataDetail()
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt != null && this.Grid1.SelectedRowIndexArray.Length > 0)
            {
                object[] o = this.Grid1.DataKeys[this.Grid1.SelectedRowIndex];
                string ID = o[0].ToString();
                DataRow[] drs = dt.Select("PoDID='" + ID + "'");
                if (drs.Length > 0)
                {
                    drs[0].Delete();
                    LoadDetail();
                    PageContext.RegisterStartupScript("PageList.grid_SelectRowFocus();");       
                }
            }
        }
        
        protected void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            if (e.CommandName == "View")
                ViweDataDetail();
        }

        protected override void BuildDetail()
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            object ID;
            System.Web.UI.WebControls.TextBox txt;            
            DataRow[] drs;
            if (dt != null)
            {
                foreach (FineUI.GridRow gr in this.Grid1.Rows)
                {
                    ID = gr.DataKeys[0];
                    drs = dt.Select(String.Format("PoDID='{0}'", ID));
                    if (drs.Length > 0)
                    {
                        txt = gr.FindControl("txtQty") as System.Web.UI.WebControls.TextBox;
                        drs[0]["Qty"] = App_Com.Helper.InputText(txt.Text.Trim(), 500);

                        txt = gr.FindControl("txtPrice") as System.Web.UI.WebControls.TextBox;
                        drs[0]["Price"] = App_Com.Helper.InputText(txt.Text.Trim(), 500);                                                
                    }
                }
                OutputSummaryData(dt);
            }
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {            
            if (B_Action.ToUpper() == "VIEW")
            {
                System.Web.UI.WebControls.TextBox txt;
                txt = Grid1.Rows[e.RowIndex].FindControl("txtQty") as System.Web.UI.WebControls.TextBox;
                txt.ReadOnly = true;
                txt.BorderWidth = 0;
                txt = Grid1.Rows[e.RowIndex].FindControl("txtPrice") as System.Web.UI.WebControls.TextBox;
                txt.ReadOnly = true;
                txt.BorderWidth = 0;
            }
        }

        protected override void PrintDetail()
        {
            PageContext.RegisterStartupScript(String.Format(@"top.Ext.getCmp('Window1').box_show('{0}','{1}');",
                String.Format("/Admin/Pur/PoM/{0}?action=print&keyword={1}", "Pur_PoMPrint.aspx", B_Keyword),
                 String.Format("{0}明细-[{1}-{2}]", B_TitlePrint, B_ModuleName, B_Keyword)
                ));
        }

        void OutputSummaryData(DataTable data)
        {
            float donateTotal = 0.0f;
            float feeTotal = 0.0f;
            foreach (DataRow row in data.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    donateTotal += Convert.ToInt32(row["Qty"]);
                    feeTotal += Convert.ToInt32(row["Amt"]);
                }                
            }

            JObject jo = new JObject();
            jo.Add("Qty", donateTotal);
            jo.Add("Amt", feeTotal);

            hfGrid1Summary.Text = jo.ToString(Newtonsoft.Json.Formatting.None);


        }
    }
}
