using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DS.XBPM.API;

namespace RDFNew.Web.Admin.Flow.ToDoM
{
    public partial class Flow_InstanceView :App_Com.PageSingle
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_ToDoM";                                               
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData(B_Keyword);
            }
        }

        private void LoadData(string instanceId)
        {
            string PDL = "";
            string INS = "";

            HistoryProcessInstance hins = App_Com.FlowHelper.Engine.GetHistoryService().FindHistoryProcessInstance(instanceId);
            if (hins != null)
            {
                PDL = hins.GetPDL();
                INS = hins.GetINS();
                {
                    List<HistoryProcessTransfer> list = hins.CreateHistoryProcessTransferQuery().List();
                    this.GridView1.DataSource = list;
                    this.GridView1.DataBind();
                }
                {
                    List<HistoryActivityTransfer> list = hins.CreateHistoryActivityTransferQuery().List();
                    this.GridView2.DataSource = list;
                    this.GridView2.DataBind();
                }
            }
            else
            {
                ProcessInstance ins = App_Com.FlowHelper.Engine.GetExecutionService().FindProcessInstance(instanceId);
                if (ins != null)
                {
                    PDL = ins.GetPDL();
                    INS = ins.GetINS();
                    {
                        List<HistoryProcessTransfer> list = ins.CreateHistoryProcessTransferQuery().List();
                        this.GridView1.DataSource = list;
                        this.GridView1.DataBind();
                    }
                    {
                        List<HistoryActivityTransfer> list = ins.CreateHistoryActivityTransferQuery().List();
                        this.GridView2.DataSource = list;
                        this.GridView2.DataBind();
                    }
                }
            }

            this.WorkflowViewer1.LoadPDL(PDL);
            this.WorkflowViewer1.LoadINS(INS);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Text = App_Com.FlowHelper.ReplaceConst(e.Row.Cells[1].Text);
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Text = App_Com.FlowHelper.ReplaceConst(e.Row.Cells[2].Text);
            e.Row.Cells[4].Text = App_Com.FlowHelper.ReplaceConst(e.Row.Cells[4].Text);
        }
    }
}
