using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageFlow : App_Com.PageMulti
    {
        protected String F_FileName = "";
        protected String F_DeployKey = "";
        protected String F_DeployName = "";
        protected String F_ToDoMID = "";
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            F_ToDoMID = String.IsNullOrEmpty(Request.QueryString["todomid"]) ? "" : Request.QueryString["todomid"];
            B_PageDetail = "Admin/Flow/ToDoM/Flow_Submit.aspx";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                
            }
        }

        protected override void InitToolBar()
        {
            base.InitToolBar();
            if (B_ToolBar1!=null)
            {
                FineUI.Button btn;
                FineUI.ToolbarSeparator Sep;

                if (B_Action.ToLower() == "edit" || B_Action.ToLower() == "view")
                {
                    Int16 iBtnLoc = 1;
                    if (B_Action.ToLower() == "view")
                        iBtnLoc = 0;
                                                                   
                    btn = new FineUI.Button(); btn.ID = "Trend";
                    btn.Text = "查看进度"; btn.IconAlign = IconAlign.Top;
                    btn.Click += new EventHandler(btn_Click);
                    btn.Icon = FineUI.Icon.ChartLine; B_ToolBar1.Items.Insert(iBtnLoc, btn);

                    btn = new FineUI.Button(); btn.ID = "Return";
                    btn.Text = "退回"; btn.IconAlign = IconAlign.Top;
                    btn.Click += new EventHandler(btn_Click);
                    btn.Icon = FineUI.Icon.ArrowTurnLeft; B_ToolBar1.Items.Insert(iBtnLoc, btn);

                    btn = new FineUI.Button(); btn.ID = "Submit";
                    btn.Text = "提交"; btn.IconAlign = IconAlign.Top;
                    btn.Click += new EventHandler(btn_Click);
                    btn.Icon = FineUI.Icon.Accept; B_ToolBar1.Items.Insert(iBtnLoc, btn);

                    if (B_Action.ToLower() == "view")
                        iBtnLoc = 3;
                    Sep = new ToolbarSeparator(); B_ToolBar1.Items.Insert(iBtnLoc, Sep);
                }
            }
        }

        protected override void btn_Click(object sender, EventArgs e)
        {
            base.btn_Click(sender, e);

            FineUI.Button btn = sender as FineUI.Button;
            switch (btn.ID.ToUpper())
            {
                case "SUBMIT":
                    FlowSubmit();
                    break;
                case "RETURN":
                    FlowReturn();
                    break;
                case "TRENT":
                    FlowTrend();
                    break;                                    
            }
        }

        protected virtual void FlowSubmit()
        {
            GetDeployInfo();
            B_Window1.Title = String.Format("{0}-[{1}-{2}]", "提交", F_DeployName, F_ToDoMID);
            B_Window1.IFrameUrl = String.Format("~/{0}?action=edit&keyword={1}", B_PageDetail, F_ToDoMID);
            B_Window1.Hidden = false;
            if (B_WindowMaxSize)
                PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
        }

        protected virtual void FlowReturn()
        {

        }

        protected virtual void FlowTrend()
        {

        }

        protected virtual void GetDeployInfo()
        {
            if (F_FileName != "")
            {
                DataTable dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select Top 1 DeployKey,DeployName 
                From Flow_DeployM Where [FileName]='{0}' And IsNull(DeployKey,'')!=''
                ", F_FileName));
                if (dt.Rows.Count > 0)
                {
                    F_DeployKey=dt.Rows[0]["DeployKey"].ToString();
                    F_DeployName = dt.Rows[0]["DeployName"].ToString();                    
                }
            }            
        }
    }
}
