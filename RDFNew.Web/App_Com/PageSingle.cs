using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageSingle : App_Com.PageBase
    {
        protected bool B_PrintDetail = false;

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

            /**身份检测********************************************************/            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!App_Com.Sys_User.CheckAuthorize(B_ModuleID, B_Action))
            {
                Response.Redirect("~/logout.aspx", true);
                return;
            }

            InitToolBar();

            if (!this.IsPostBack)
            {
                FillControlData();
                InitPage();                
            }
        }

        protected virtual void InitToolBar()
        {
            if (B_ToolBar1 != null)
            {                
                FineUI.Button btn;
                FineUI.ToolbarSeparator Sep;
                if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
                {
                    btn = new FineUI.Button(); btn.ID = "SaveAndExit";
                    btn.Click += new EventHandler(btn_Click);
                    btn.Text = "保存"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                    btn.ValidateForms = new string[] { "Form2" };
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "SystemSave"); B_ToolBar1.Items.Add(btn);

                    if (B_Action.ToLower() == "add")
                    {
                        btn = new FineUI.Button(); btn.ID = "SaveAndNew";
                        btn.Click += new EventHandler(btn_Click);
                        btn.Text = "保存并新增"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                        btn.ValidateForms = new string[] { "Form2" };
                        btn.Icon = (Icon)Enum.Parse(typeof(Icon), "SystemSaveNew"); B_ToolBar1.Items.Add(btn);
                    }

                    Sep = new ToolbarSeparator();
                    B_ToolBar1.Items.Add(Sep);
                }

                if (B_Action.ToLower() == "view" && B_PrintDetail && App_Com.Sys_User.CheckAuthorize(B_ModuleID, "Print"))
                {
                    btn = new FineUI.Button(); btn.ID = "Print";
                    btn.Click += new EventHandler(btn_Click);
                    btn.Text = "打印"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;                    
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Printer"); B_ToolBar1.Items.Add(btn);

                    Sep = new ToolbarSeparator();
                    B_ToolBar1.Items.Add(Sep);
                }

                btn = new FineUI.Button(); btn.ID = "Exit";
                btn.OnClientClick = ActiveWindow.GetHideReference();
                btn.Text = "关闭"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = false;
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "SystemClose"); B_ToolBar1.Items.Add(btn);
            }
        }



        protected virtual void FillControlData()
        {

        }

        protected virtual void InitPage()
        {
            if (B_Action.ToLower() == "add")
            {
                //初始化各控件
                SetControlState();                
                LoadDetail();
            }
            if (B_Action.ToLower() == "edit")
            {
                //初始化各控件
                SetControlState();
                //取数据
                LoadMater();
            }
            if (B_Action.ToLower() == "view")
            {
                //初始化各控件
                SetControlState();
                //取数据
                LoadMater();
            }
        }

        protected virtual void LoadDetail()
        {

        }

        protected virtual void SetControlState()
        {

        }

        protected virtual void LoadMater()
        {
            if (B_IDAL != null)
            {
                object[] data = B_IDAL.GetMaster(B_Keyword);
                if (data[0].ToString() == "0") //正常
                {
                    DataTable table = data[1] as DataTable;
                    if (table.Rows.Count > 0)
                    {
                        GetData(table.Rows[0]);
                    }
                }
                else
                {
                    Exception ex = data[1] as Exception;
                    StringBuilder sb = new StringBuilder();
                    while (ex != null)
                    {
                        sb.Append(ex.Message + "\r\n");
                        ex = ex.InnerException;
                    }
                    Alert.Show("发生如下错误:\r\n" + sb.ToString(), MessageBoxIcon.Error);
                }
            }
        }

        protected virtual void btn_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;            
            if (btn.ID == "SaveAndExit")
            {
                try
                {
                    string RtnKey = "";
                    if (B_Action.ToLower() == "add")
                        RtnKey = AddData();
                    if (B_Action.ToLower() == "edit")
                        RtnKey = UpdateData();
                    AfterApply();
                    //更新父页面                    
                    PageContext.RegisterStartupScript("parent.__doPostBack('','Changed');");
                    if (B_Action.ToLower() == "add")
                    {
                        PageContext.RegisterStartupScript(String.Format("parent.PageList.setWindowTitle('{0}-[{1}-{2}]');", B_TitleEdit, B_ModuleName, RtnKey));
                        string url = Request.Path + "?action=edit&keyword=" + RtnKey;
                        PageContext.Redirect(url);
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    while (ex != null)
                    {
                        sb.Append(ex.Message + "\r\n");
                        ex = ex.InnerException;
                    }
                    Alert.Show("发生如下错误:\r\n" + sb.ToString(), MessageBoxIcon.Error);
                }
            }
            if (btn.ID == "SaveAndNew")
            {
                try
                {
                    string RtnKey = "";
                    if (B_Action.ToLower() == "add")
                        RtnKey = AddData();
                    if (B_Action.ToLower() == "edit")
                        RtnKey = UpdateData();
                    AfterApply();
                    //更新父页面
                    PageContext.RegisterStartupScript("parent.__doPostBack('','Changed');");
                    PageContext.Redirect(Request.RawUrl);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    while (ex != null)
                    {
                        sb.Append(ex.Message + "\r\n");
                        ex = ex.InnerException;
                    }
                    Alert.Show("发生如下错误:\r\n" + sb.ToString(), MessageBoxIcon.Error);
                }
            }
            if (btn.ID == "Print")
            {
                PrintDetail();
            }
        }

        protected virtual void AfterApply()
        {

        }

        protected virtual void GetData(DataRow dr)
        {
            
        }

        protected virtual string AddData()
        {
            return "";
        }

        protected virtual string UpdateData()
        {
            return "";
        }

        protected virtual void PrintDetail()
        {

        }
    }
}
