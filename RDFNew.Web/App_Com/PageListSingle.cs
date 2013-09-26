using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageListSingle : App_Com.PageBase
    {      
        protected string B_TableKey = "";
        protected string B_OrderBy = "";
        protected bool B_ReturnEmpty = true;
              
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void  OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //Select模式不记忆
                if (B_Action.ToUpper() != "SELECT")
                {
                    HttpCookie cookie = new HttpCookie("LastPage", Request.RawUrl);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    if (!App_Com.Sys_User.CheckAuthorize(B_ModuleID, "View"))
                        cookie.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie);
                }
            }
            
            /**身份检测********************************************************/
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!App_Com.Sys_User.CheckAuthorize(B_ModuleID, "View"))
            {
                Response.Redirect("~/logout.aspx", true);
                return;
            }
            /**选择模式时加载不同工具条********************************************************/            
            if(B_Action.ToUpper() == "SELECT")
                InitToolBarSelect();
            else
                InitToolBarList();
            //
            if (!this.IsPostBack)
            {
                FillControlData();
                InitPage();
                LoadMater();
            }
            //
            if (this.IsPostBack && Request.Form["__EVENTARGUMENT"] == "Changed")
            {
                LoadMater();
            }
        }

        protected virtual void LoadMater()
        {
            if (B_Grid1 != null && B_IDAL!=null)
            {
                RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
                qs.PageInfo = new RDFNew.Module.DALEntity.PageInfo(B_Grid1.PageSize, B_Grid1.PageIndex);
                qs.QueryInfos = GetQueryInfo();
                if (B_Grid1.SortColumn != "")
                    qs.OrderBy = B_Grid1.SortColumn + " " + B_Grid1.SortDirection;
                else
                    qs.OrderBy = B_OrderBy;
                object[] data = B_IDAL.GetMaster(qs);

                if (data[0].ToString() == "0") //正常
                {
                    B_Grid1.RecordCount = Convert.ToInt32(data[2]);
                    DataTable table = data[1] as DataTable;
                    BeforeDataBind(table);
                    B_Grid1.DataSource = table;
                    B_Grid1.DataBind();
                }
                else
                {
                    B_Grid1.DataSource = null;
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

        protected virtual List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            return null;
        }

        protected virtual void FillControlData()
        {

        }

        protected virtual void BeforeDataBind(DataTable dt)
        {

        }

        protected virtual void InitPage()
        {            
            if (B_Window1 != null)
            {
                B_Window1.Width = B_WinSize[0];
                B_Window1.Height = B_WinSize[1];
            }
        }

        protected virtual void InitToolBarList()
        {
            if (B_ToolBar1 != null && B_Grid1 != null)
            {
                FineUI.Button btn;
                FineUI.ToolbarSeparator Sep;
                DataTable dt = App_Com.Sys_User.GetSys_Function();
                string FunctionID;
                foreach (DataRow dr in dt.Select("ModuleID='" + B_ModuleID + "'", "Seq"))
                {
                    FunctionID = dr["FunctionID"].ToString();
                    if (App_Com.Sys_User.CheckAuthorize(B_ModuleID, FunctionID))
                    {
                        if (FunctionID.ToLower() == "add" || FunctionID.ToLower() == "edit" || FunctionID.ToLower() == "delete")
                        {
                            if (B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].ID.ToLower() == "view")
                            {
                                Sep = new ToolbarSeparator();
                                B_ToolBar1.Items.Add(Sep);
                            }
                        }
                        if (FunctionID.ToLower() == "print" || FunctionID.ToLower() == "export")
                        {
                            if (B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].ID.ToLower() == "view" ||
                                B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].ID.ToLower() == "add" ||
                                B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].ID.ToLower() == "edit" ||
                                B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].ID.ToLower() == "delete")
                            {
                                Sep = new ToolbarSeparator();
                                B_ToolBar1.Items.Add(Sep);
                            }
                        }

                        btn = new FineUI.Button(); btn.ID = FunctionID;
                        btn.Text = dr["FunctionName"].ToString();
                        btn.IconAlign = IconAlign.Top;
                        btn.Icon = (Icon)Enum.Parse(typeof(Icon), dr["Icon"].ToString());
                        if (FunctionID.ToLower() == "view")
                        {
                            btn.Click += new EventHandler(btn_Click);
                            btn.OnClientClick = B_Grid1.GetNoSelectionAlertReference("请首先选择一行记录。");
                        }
                        if (FunctionID.ToLower() == "add")
                        {
                            btn.Click += new EventHandler(btn_Click);
                        }
                        if (FunctionID.ToLower() == "edit")
                        {
                            btn.Click += new EventHandler(btn_Click);
                            btn.OnClientClick = B_Grid1.GetNoSelectionAlertReference("请首先选择一行记录。");
                        }
                        if (FunctionID.ToLower() == "delete")
                        {
                            btn.Click += new EventHandler(btn_Click);
                            btn.OnClientClick = B_Grid1.GetNoSelectionAlertReference("请首先选择一行记录。");
                            btn.ConfirmText = String.Format("你确定要删除选中的项吗?", "");
                        }
                        if (FunctionID.ToLower() == "print")
                        {
                            btn.OnClientClick = "btnPrint_onclick()";
                        }
                        if (FunctionID.ToLower() == "export")
                        {
                            btn.Click += new EventHandler(btn_Click);
                            btn.DisableControlBeforePostBack = false;
                            btn.EnableAjax = false;
                            btn.EnablePostBack = true;
                        }
                        B_ToolBar1.Items.Add(btn);
                    }
                }
                if (B_ToolBar1.Items[B_ToolBar1.Items.Count - 1].GetType() != typeof(ToolbarSeparator))
                {
                    Sep = new ToolbarSeparator();
                    B_ToolBar1.Items.Add(Sep);
                }
                btn = new FineUI.Button(); btn.ID = "Zoom"; btn.Click += new EventHandler(btn_Click);
                btn.Text = "查询"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Zoom"); B_ToolBar1.Items.Add(btn);

                btn = new FineUI.Button(); btn.ID = "Erase"; btn.Click += new EventHandler(btn_Click);
                btn.Text = "清除"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true; btn.ToolTip = "清除查询条件.";
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Erase"); B_ToolBar1.Items.Add(btn);
            }
        }

        protected virtual void InitToolBarSelect()
        {
            if (B_ToolBar1 != null && B_Grid1 != null)
            {
                FineUI.Button btn;
                btn = new FineUI.Button(); btn.ID = "Select";
                btn.Click += new EventHandler(btn_Click);
                btn.Text = "选择"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Accept"); B_ToolBar1.Items.Add(btn);
                if (B_ReturnEmpty)
                {
                    btn = new FineUI.Button(); btn.ID = "Empty";
                    btn.Click += new EventHandler(btn_Click);
                    btn.Text = "返回空值"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                    btn.Icon = (Icon)Enum.Parse(typeof(Icon), "PageWhite"); B_ToolBar1.Items.Add(btn);
                }

                B_ToolBar1.Items.Add(new ToolbarSeparator());
                btn = new FineUI.Button(); btn.ID = "Zoom"; btn.Click += new EventHandler(btn_Click);
                btn.Text = "查询"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Zoom"); B_ToolBar1.Items.Add(btn);
                btn = new FineUI.Button(); btn.ID = "Erase"; btn.Click += new EventHandler(btn_Click);
                btn.Text = "清除"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true; btn.ToolTip = "清除查询条件.";
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "Erase"); B_ToolBar1.Items.Add(btn);
                B_ToolBar1.Items.Add(new ToolbarSeparator());
                btn = new FineUI.Button(); btn.ID = "Exit";
                btn.OnClientClick = ActiveWindow.GetHideReference();
                btn.Text = "关闭"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = false;
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "SystemClose"); B_ToolBar1.Items.Add(btn);
            }
        }

        protected virtual void btn_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;
            switch (btn.ID.ToUpper())
            {
                case "VIEW":
                    ViweData();
                    break;
                case "ADD":
                    AddData();
                    break;
                case "EDIT":
                    EditData();
                    break;
                case "DELETE":
                    DeleteData();
                    break;
                case "ZOOM":
                    QueryData();
                    break;
                case "ERASE":
                    ClearControls();
                    break;
                case "EXPORT":
                    ExportData(); ;
                    break;
                case "SELECT":
                    SelectData(btn.ID.ToUpper());
                    break;
                case "EMPTY":
                    SelectData(btn.ID.ToUpper());
                    break;
            }
        }

        protected virtual void ClearControls()
        {

        }

        protected virtual void ViweData()
        {
            if (B_Grid1 != null && B_Window1!=null)
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleView, B_ModuleName, KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=view&keyword={1}", B_PageDetail, KeyVal);                
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void AddData()
        {            
            if (B_Window1 != null)
            {
                B_Window1.Title = String.Format("{0}-[{1}]", B_TitleAdd, B_ModuleName);
                B_Window1.IFrameUrl = String.Format("{0}?action=add", B_PageDetail);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void EditData()
        {
            if (B_Grid1 != null && B_Window1 != null)
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleEdit, B_ModuleName, KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=edit&keyword={1}", B_PageDetail, KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
        }

        protected virtual void DeleteData()
        {
            if(B_Grid1 != null && B_IDAL !=null)
            {
            string KeyVal = "";
            KeyVal = B_Grid1.DataKeys[this.B_Grid1.SelectedRowIndex][0].ToString();            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo(B_TableKey,"=","DeleteKey", KeyVal));
            try
            {
                object[] data = B_IDAL.GetMaster(qs);
                if (data[0].ToString() == "0") //正常
                {
                    DataTable table = data[1] as DataTable;
                    if (table.Rows.Count > 0)
                    {
                        table.Rows[0].Delete();
                        data = B_IDAL.ApplyMaster(table.GetChanges(DataRowState.Deleted),App_Com.Helper.BuildLog(B_ModuleID, "delete"));
                        if (data[0].ToString() == "0") //正常
                        {
                            AfterDeleteData(table);
                            LoadMater();                            
                            PageContext.RegisterStartupScript("PageList.grid_SelectRowFocus();");                            
                        }
                        else
                        {
                            throw data[1] as Exception;
                        }
                    }
                }
                else
                {
                    throw data[1] as Exception;
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
        }

        protected virtual void AfterDeleteData(DataTable dt)
        {

        }

        protected virtual void QueryData()
        {
            LoadMater();
        }

        protected virtual void ExportData()
        {            
            if (B_Grid1 != null)
            {
                ExportToExcel(B_Grid1);
            }
        }

        protected virtual void SelectData(string Fr)
        {
            if (B_Grid1 != null)
            {
                if (Fr.ToUpper() == "SELECT")
                {
                    int selectedCount = B_Grid1.SelectedRowIndexArray.Length;
                    if (selectedCount == 0)
                    {
                        PageContext.RegisterStartupScript(B_Grid1.GetNoSelectionAlertInTopReference("请至少选择一项!"));
                        return;
                    }
                    PageContext.RegisterStartupScript(
                        ActiveWindow.GetWriteBackValueReference(BackSelectData()) +
                        ActiveWindow.GetHideReference());
                }
                if (Fr.ToUpper() == "EMPTY")
                {
                    PageContext.RegisterStartupScript(
                        ActiveWindow.GetWriteBackValueReference(BackEmptyData()) +
                        ActiveWindow.GetHideReference());
                }
            }
        }

        protected virtual string[] BackSelectData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            return new string[]{row.DataKeys[0].ToString(), row.Values[1]};
        }

        protected virtual string[] BackEmptyData()
        {
            return new string[] { "", "" };
        }

        protected virtual void ExportToExcel(FineUI.Grid Grid1)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Response.AddHeader("content-disposition", string.Format("{0}; filename={1}.xls", "attachment", System.DateTime.Now.ToString("MMddHHmmss")));
            Response.Write(App_Com.Helper.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected virtual void Grid1_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {
            FineUI.Grid Grid1 = sender as FineUI.Grid;
            Grid1.PageIndex = e.NewPageIndex;
            LoadMater();
        }

        protected virtual void Grid1_Sort(object sender, FineUI.GridSortEventArgs e)
        {
            FineUI.Grid Grid1 = sender as FineUI.Grid;
            Grid1.SortColumn = e.SortField;
            Grid1.SortDirection = e.SortDirection;
            LoadMater();
        }

        protected virtual void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {                
                if (B_Action.ToUpper() == "SELECT")
                {
                    SelectData(B_Action);
                }
                else
                {
                    ViweData();
                }
            }
        }

        protected virtual string GetRowID(DataTable dt, string KeyField)
        {
            String NewRID = new System.Random(App_Com.Helper.GetRandomSeed()).Next(0, 99999999).ToString();
            if (dt.Rows.Count == 0)
                return NewRID;
            while (true)
            {
                DataRow[] drs = dt.Select(String.Format("{0}='{1}'", KeyField, NewRID));
                if (drs.Length == 0)
                    return NewRID;
                else
                    NewRID = new System.Random(App_Com.Helper.GetRandomSeed()).Next(0, 99999999).ToString();
            }
        }

        protected virtual string GetSeq(DataTable dt, string SeqField)
        {
            if (dt.Rows.Count == 0)
                return "0001";
            DataRow[] drs = dt.Select("1=1", SeqField + " Desc");
            int s = Convert.ToInt32(drs[0][SeqField].ToString()) + 1;
            return s.ToString().PadLeft(4, '0');
        }
    }
}
