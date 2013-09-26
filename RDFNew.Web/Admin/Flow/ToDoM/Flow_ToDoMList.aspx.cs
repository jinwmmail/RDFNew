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

namespace RDFNew.Web.Admin.Flow.ToDoM
{
    public partial class Flow_ToDoMList : App_Com.PageListMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_ToDoM";
            B_ModuleName = "流程列表";
            B_PageDetail = "Flow_ToDoM.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_ToDoM();
            B_TableKey = "Flow_ToDoM.ToDoMID";
            B_OrderBy = " Flow_ToDoM.ToDoMID Desc ";
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
            Str = this.txtToDoMID.Text.Trim();
            if (Str != "")
                qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_ToDoM.ToDoMID", "Like", "ToDoMID", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtToDoMID.Text = "";            
        }

        protected override void BeforeDataBind(DataTable dt)
        {
            DataColumn dc;
            dc = new DataColumn("State", typeof(string)); dt.Columns.Add(dc);
            dc = new DataColumn("ResultDesc", typeof(string)); dt.Columns.Add(dc);

            foreach (DataRow dr in dt.Rows)
            {
                string InstanceID = dr["InstanceID"].ToString();                
                string state = "";
                string resultDesc = "";                
                DS.XBPM.API.HistoryProcessInstance hins = App_Com.FlowHelper.Engine.GetHistoryService().FindHistoryProcessInstance(InstanceID);
                if (hins != null)
                {
                    state = App_Com.FlowHelper.ReplaceConst(hins.EndState);
                    resultDesc = hins.EndDescription;                    
                }
                else
                {
                    DS.XBPM.API.ProcessInstance ins = App_Com.FlowHelper.Engine.GetExecutionService().FindProcessInstance(InstanceID);
                    if (ins != null)
                    {
                        state = App_Com.FlowHelper.ReplaceConst(ins.CurrentState);
                        resultDesc = ins.CurrentDescription;                        
                    }
                }                
                dr["State"] = state;
                dr["ResultDesc"] = resultDesc;
            }
        }

        protected override void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            base.Grid1_RowCommand(sender, e);
            //if (e.CommandName == "Submit")
            //{
            //    string KeyVal = "";
            //    KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
            //    B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleEdit, B_ModuleName, KeyVal);
            //    B_Window1.IFrameUrl = String.Format("Flow_Submit.aspx?action=edit&keyword={1}", B_PageDetail, KeyVal);
            //    B_Window1.Hidden = false;
            //    if (B_WindowMaxSize)
            //        PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            //}
            if (e.CommandName == "InstanceView")
            {
                String InstanceID=this.Grid1.Rows[e.RowIndex].Values[0];
                B_Window1.Title = String.Format("{0}", "查看流程走向");
                B_Window1.IFrameUrl = String.Format("Flow_InstanceView.aspx?action=edit&keyword={0}", InstanceID);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        //RDFNew.Module.Admin.Flow.Flow_DeployD Tmp_DALD=null ;
        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            //LinkButtonField lkbDeploy = Grid1.FindColumn("lkbSubmit") as LinkButtonField;
            //lkbDeploy.Enabled =false ;
            //DataRowView row = e.DataItem as DataRowView;
            //if (row != null)
            //{
            //    DS.XBPM.API.ProcessInstance ins = App_Com.FlowHelper.Engine.GetExecutionService().FindProcessInstance(row.Row["InstanceID"].ToString());
            //    if (ins != null)
            //    {
            //        string[] curActNames = ins.FindActiveExecutionNames();
            //        for (int j = 0; j < curActNames.Length; j++)
            //        {
            //            string act = curActNames[j];
            //            if (Tmp_DALD == null)
            //                Tmp_DALD = new RDFNew.Module.Admin.Flow.Flow_DeployD();
            //            string owner = Tmp_DALD.GetActivityOwner(ins.DeploymentId, ins.ProcessName, act);

            //            if (owner.ToUpper() == "APPLICANT" && row.Row["CrtBy"].ToString() == App_Com.Sys_User.GetUserInfo("UserID"))
            //            {
            //                lkbDeploy.Enabled = true;
            //                break;
            //            }
                        
            //            if (owner == App_Com.Sys_User.GetUserInfo("UserID"))
            //            {
            //                lkbDeploy.Enabled = true;
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        protected override void ViweData()
        {
            if (B_Grid1 != null && B_Window1 != null)
            {
                FineUI.GridRow r = B_Grid1.Rows[B_Grid1.SelectedRowIndex];
                String ProcessName = r.Values[2];
                String BillPage = r.Values[4];
                String BillID = r.Values[3];
                String ToDoMID = r.Values[5];

                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleView, ProcessName, BillID);
                B_Window1.IFrameUrl = String.Format("{0}?action=view&keyword={1}&todomid={2}", BillPage, BillID, ToDoMID);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }
    }
}
