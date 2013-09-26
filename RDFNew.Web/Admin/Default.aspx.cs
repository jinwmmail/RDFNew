using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using FineUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RDFNew.Web.Admin
{
    public partial class Default : App_Com.PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            string menuType = "menu";
            HttpCookie menuCookie = Request.Cookies["MenuStyle"];
            if (menuCookie != null)
            {
                menuType = menuCookie.Value;
            }

            // 注册客户端脚本，服务器端控件ID和客户端ID的映射关系
            JObject ids = GetClientIDS(Window1, mainTabStrip);

            if (menuType == "accordion")
            {
                Accordion accordionMenu = InitAccordionMenu();
                ids.Add("mainMenu", accordionMenu.ClientID);
                ids.Add("menuType", "accordion");
            }
            else
            {
                Tree treeMenu = InitTreeMenu();
                ids.Add("mainMenu", treeMenu.ClientID);
                ids.Add("menuType", "menu");
            }

            // 只在页面第一次加载时注册客户端用到的脚本
            if (!Page.IsPostBack)
            {
                string idsScriptStr = String.Format("window.IDS={0};", ids.ToString(Newtonsoft.Json.Formatting.None));
                PageContext.RegisterStartupScript(idsScriptStr);
            }
            this.menuExit.Click += new EventHandler(menuExit_Click);
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", true);
        }

        private JObject GetClientIDS(params ControlBase[] ctrls)
        {
            JObject jo = new JObject();
            foreach (ControlBase ctrl in ctrls)
            {
                jo.Add(ctrl.ID, ctrl.ClientID);
            }
            return jo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                RestoreMenuStatus();
                SetStatusInfo();
            }
        }

        void RestoreMenuStatus()
        {
            HttpCookie ck;
            ck = Request.Cookies["MenuStyle"];
            if (ck != null)
            {
                switch (ck.Value)
                {
                    case "tree":
                        this.MenuStyleTree.Checked = true;
                        break;
                    default:
                        this.MenuStyleAccordion.Checked = true;
                        break;
                }
            }
            else
                this.MenuStyleTree.Checked = true;
            ck = Request.Cookies["Theme"];
            if (ck != null)
            {
                switch (ck.Value)
                {
                    case "gray":
                        this.MenuThemeGray.Checked = true;
                        break;
                    case "access":
                        this.MenuThemeAccess.Checked = true;
                        break;
                    default:
                        this.MenuThemeBlue.Checked = true;
                        break;
                }
            }
            else
                this.MenuThemeBlue.Checked = true;
        }

        protected void MenuTheme_CheckedChanged(object sender, EventArgs e)
        {

            MenuCheckBox menuCheckBox = sender as MenuCheckBox;
            if (!menuCheckBox.Checked)
            {
                // CheckedChanged会触发两次，一次是取消选中的菜单项，另一次是选中的菜单项；
                // 这里不处理取消选中的菜单项的事件，从而防止此函数重复执行两次
                return;
            }

            string themeValue = "Blue";
            string themeID = GetSelectedMenuID(MenuTheme);

            switch (themeID)
            {
                case "MenuThemeBlue":
                    themeValue = "blue";
                    break;
                case "MenuThemeGray":
                    themeValue = "gray";
                    break;
                case "MenuThemeAccess":
                    themeValue = "access";
                    break;
            }

            SaveToCookieAndRefresh("Theme", themeValue);
        }

        protected void MenuStyle_CheckedChanged(object sender, EventArgs e)
        {
            MenuCheckBox menuCheckBox = sender as MenuCheckBox;
            if (!menuCheckBox.Checked)
            {
                // CheckedChanged会触发两次，一次是取消选中的菜单项，另一次是选中的菜单项；
                // 这里不处理取消选中的菜单项的事件，从而防止此函数重复执行两次
                return;
            }

            string menuValue = "menu";
            string menuStyleID = GetSelectedMenuID(MenuStyle);

            switch (menuStyleID)
            {
                case "MenuStyleTree":
                    menuValue = "tree";
                    break;
                case "MenuStyleAccordion":
                    menuValue = "accordion";
                    break;

            }
            SaveToCookieAndRefresh("MenuStyle", menuValue);
        }

        private string GetSelectedMenuID(MenuButton menuButton)
        {
            foreach (FineUI.MenuItem item in menuButton.Menu.Items)
            {
                if (item is MenuCheckBox && (item as MenuCheckBox).Checked)
                {
                    return item.ID;
                }
            }
            return null;
        }

        private void SetSelectedMenuID(MenuButton menuButton, string selectedMenuID)
        {
            foreach (FineUI.MenuItem item in menuButton.Menu.Items)
            {
                MenuCheckBox menu = (item as MenuCheckBox);
                if (menu != null && menu.ID == selectedMenuID)
                {
                    menu.Checked = true;
                }
                else
                {
                    menu.Checked = false;
                }
            }
        }

        private void SaveToCookieAndRefresh(string cookieName, string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);

            PageContext.Refresh();
        }

        private Tree InitTreeMenu()
        {
            Tree treeMenu = new Tree();
            treeMenu.ID = "treeMenu";
            treeMenu.EnableArrows = true;
            treeMenu.ShowBorder = false;
            treeMenu.ShowHeader = false;
            treeMenu.EnableIcons = false;
            treeMenu.AutoScroll = true;

            BuildTree(App_Com.Sys_User.GetSys_Menu(), "", treeMenu, null);

            Region2.Items.Add(treeMenu);
            return treeMenu;
        }

        void BuildTree(DataTable dt, string pId, FineUI.Tree tree, FineUI.TreeNode pn)
        {
            FineUI.TreeNode tn;
            string ModuleID;
            foreach (DataRow dr in dt.Select("IsNull(PID,'')='" + pId + "'", "RID"))
            {
                if (Convert.ToBoolean(dr["ModuleEnabled"]))
                {
                    ModuleID = dr["ModuleID"].ToString().Trim();
                    if (ModuleID != "")
                    {
                        if (!App_Com.Sys_User.CheckAuthorize(ModuleID, "View"))
                            continue;
                    }
                    tn = new FineUI.TreeNode();
                    tn.NodeID = dr["MenuID"].ToString();
                    tn.Text = dr["MenuName"].ToString();
                    tn.Icon = IconHelper.String2Icon(dr["Icon"].ToString(), true);
                    tn.NavigateUrl = dr["Url"].ToString() + (dr["UrlParameter"].ToString() == "" ? "" : "?" + dr["UrlParameter"].ToString());
                    if (!Convert.ToBoolean(dr["Enabled"]))
                    {
                        tn.Enabled = false;
                        tn.ToolTip = "此模块尚未启用,请稍候再试.";
                        tn.Icon = Icon.BulletWrench;
                    }
                    if (pn != null)
                        pn.Nodes.Add(tn);
                    else
                        tree.Nodes.Add(tn);
                    BuildTree(dt, dr["RID"].ToString(), tree, tn);

                    if (ModuleID == "" && tn.Nodes.Count == 0)
                    {
                        if (tn.ParentNode != null)
                            tn.ParentNode.Nodes.Remove(tn);
                        else
                            tree.Nodes.Remove(tn);
                    }
                }
            }
        }

        private Accordion InitAccordionMenu()
        {
            Accordion accordionMenu = new Accordion();
            accordionMenu.ID = "accordionMenu";
            accordionMenu.EnableFill = true;
            accordionMenu.ShowBorder = false;
            accordionMenu.ShowHeader = false;
            Region2.Items.Add(accordionMenu);

            AccordionPane accordionPane;
            DataTable dt = App_Com.Sys_User.GetSys_Menu();
            foreach (DataRow dr in dt.Select("IsNull(PID,'')=''", "RID"))
            {
                accordionPane = new AccordionPane();
                accordionPane.Title = dr["MenuName"].ToString();
                accordionPane.Layout = Layout.Fit;
                accordionPane.ShowBorder = false;
                accordionPane.BodyPadding = "2px 0 0 0";
                accordionMenu.Items.Add(accordionPane);

                Tree treeMenu = new Tree();
                treeMenu.EnableArrows = true;
                treeMenu.ShowBorder = false;
                treeMenu.ShowHeader = false;
                treeMenu.EnableIcons = false;
                treeMenu.AutoScroll = true;
                BuildTree(dt, dr["RID"].ToString(), treeMenu, null);
                accordionPane.Items.Add(treeMenu);
            }
            return accordionMenu;
        }

        void SetStatusInfo()
        {
            this.labUser.Text = String.Format(@"{0}[{1}]", 
                App_Com.Sys_User.GetUserInfo("UserID"),
                App_Com.Sys_User.GetUserInfo("UserName")                
                );
            this.labDateTime.Text = String.Format(@"{0}",                
                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")
                );

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.labCopyright.Text = String.Format(@"版本:v{0}.{1} @{2} Powered By [Phone:138-1845-9481]",
                assembly.GetName().Version.Major,
                assembly.GetName().Version.Minor,
                System.DateTime.Now.ToString("yyyy"));
        }
    }
}
