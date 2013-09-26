using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageMulti : App_Com.PageSingle
    {
        protected FineUI.Toolbar B_ToolBar2 = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void InitToolBar()
        {
            base.InitToolBar();
            if (B_ToolBar2 != null)
            {
                FineUI.Button btn;
                FineUI.ToolbarSeparator Sep;

                /**以下子表工具栏**/
                btn = new FineUI.Button(); btn.ID = "ViewDetail";                
                btn.Text = "查看"; btn.IconAlign = IconAlign.Top;
                btn.Click += new EventHandler(btn_Click);
                btn.OnClientClick = B_Grid1.GetNoSelectionAlertInTopReference("请首先选择一行记录。");
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "PageWhiteText"); B_ToolBar2.Items.Add(btn);

                if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
                {
                    Sep = new ToolbarSeparator();B_ToolBar2.Items.Add(Sep);

                    btn = new FineUI.Button(); btn.ID = "AddDetail";                    
                    btn.Text = "新增"; btn.IconAlign = IconAlign.Top;
                    btn.Click += new EventHandler(btn_Click);
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Add"); B_ToolBar2.Items.Add(btn);

                    btn = new FineUI.Button(); btn.ID = "EditDetail";
                    btn.Text = "修改"; btn.IconAlign = IconAlign.Top; 
                    btn.Click += new EventHandler(btn_Click);
                    btn.OnClientClick = B_Grid1.GetNoSelectionAlertInTopReference("请首先选择一行记录。");                                        
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Pencil"); B_ToolBar2.Items.Add(btn);

                    btn = new FineUI.Button(); btn.ID = "DeleteDetail";
                    btn.Text = "删除"; btn.IconAlign = IconAlign.Top; 
                    btn.Click += new EventHandler(btn_Click);
                    btn.OnClientClick = B_Grid1.GetNoSelectionAlertInTopReference("请首先选择一行记录。");                                     
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Delete"); B_ToolBar2.Items.Add(btn);
                }
                B_ToolBar2.Hidden = B_ToolBar2.Items.Count == 0;
            }
        }

        protected override void btn_Click(object sender, EventArgs e)
        {
            base.btn_Click(sender,e);

            FineUI.Button btn = sender as FineUI.Button;
            switch (btn.ID.ToUpper())
            {
                case "VIEWDETAIL":
                    ViweDataDetail();
                    break;
                case "ADDDETAIL":
                    AddDataDetail();
                    break;
                case "EDITDETAIL":
                    EditDataDetail();
                    break;
                case "DELETEDETAIL":
                    DeleteDataDetail();
                    break;                
            }
        }

        protected virtual void ViweDataDetail()
        {
            if (B_Grid1 != null && B_Window1 != null)
            { 
                if (B_Action.ToUpper()=="EDIT")
                    BuildDetail();
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}明细-[{1}-{2}]", B_TitleView, B_ModuleName, KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=view&keyword={1}", B_PageDetail, KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void AddDataDetail()
        {
            if (B_Window1 != null)
            {
                B_Window1.Title = String.Format("{0}明细-[{1}]", B_TitleAdd, B_ModuleName);
                B_Window1.IFrameUrl = String.Format("{0}?action=select", B_PageDetailAdd);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void EditDataDetail()
        {
            if (B_Grid1 != null && B_Window1 != null)
            {
                BuildDetail();
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}明细-[{1}-{2}]", B_TitleEdit, B_ModuleName, KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=edit&keyword={1}", B_PageDetail, KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void DeleteDataDetail()
        {

        }

        protected virtual void BuildDetail()
        {

        }
    }
}
