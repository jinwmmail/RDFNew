using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageBase:System.Web.UI.Page
    {
        protected string B_Action="";
        protected string B_Keyword = "";
        protected string B_ModuleID = "";
        protected string B_ModuleName = "";
        protected FineUI.Toolbar B_ToolBar1 = null;
        protected FineUI.Window B_Window1 = null;
        protected bool B_WindowMaxSize = false;
        protected FineUI.Grid B_Grid1 = null;
        protected string B_TitleView = "查看";
        protected string B_TitlePrint = "打印";
        protected string B_TitleAdd = "新增";
        protected string B_TitleEdit = "修改";

        protected int[] B_WinSize = new int[] { 500, 450 };
        protected RDFNew.Module.Admin.ISingle B_IDAL = null;
        protected string B_PageDetail = "";
        protected string B_PageDetailAdd = "";
        protected string B_DetailSessionKey = ""; 

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_Action = String.IsNullOrEmpty(Request.QueryString["action"]) ? "" : Request.QueryString["action"];
            B_Keyword = String.IsNullOrEmpty(Request.QueryString["keyword"]) ? "" : Request.QueryString["keyword"];
            
            if (!IsPostBack)
            {
                if (PageManager.Instance != null)
                {
                    HttpCookie themeCookie = Request.Cookies["Theme"];
                    if (themeCookie != null)
                    {
                        string themeValue = themeCookie.Value;

                        if (IsSystemTheme(themeValue))
                        {
                            PageManager.Instance.Theme = (Theme)Enum.Parse(typeof(Theme), themeValue, true);
                        }
                        else
                        {
                            PageManager.Instance.CustomTheme = themeValue;
                        }
                    }

                    HttpCookie langCookie = Request.Cookies["Language"];
                    if (langCookie != null)
                    {
                        string langValue = langCookie.Value;
                        PageManager.Instance.Language = (Language)Enum.Parse(typeof(Language), langValue, true);
                    }
                }
            }
        }

        private bool IsSystemTheme(string themeName)
        {
            themeName = themeName.ToLower();
            string[] themes = Enum.GetNames(typeof(Theme));
            foreach (string theme in themes)
            {
                if (theme.ToLower() == themeName)
                    return true;
            }
            return false;
        }

        protected virtual bool GetImageVisible(object src)
        {
            if (src != null && src.ToString() != "")
                return true;
            return false;
        }
    }
}
