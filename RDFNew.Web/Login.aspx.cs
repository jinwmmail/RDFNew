using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

namespace RDFNew.Web
{
    public partial class Login : App_Com.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            if (!this.IsPostBack)
            {
                SetStatusInfo();
            }
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            this.labInfo.Text = "";

            string UserID = App_Com.Helper.InputText(this.txtUser.Text.Trim(), 500);
            if (String.IsNullOrEmpty(UserID))
            {                
                FineUI.Alert.ShowInTop("登录帐号不可为空.","操作提示",FineUI.MessageBoxIcon.Error);                
                return ;
            }
            string Pwd = App_Com.Helper.InputText(this.txtPwd.Text.Trim(), 500);
            object[] rtn = App_Com.Sys_User.UserLogin(UserID, Pwd);
            if (rtn[0].ToString() == "0")
            {
                HttpCookie ck;
                ck = new HttpCookie("ck_UserName", UserID);
                ck.Expires = System.DateTime.Now.AddMonths(1);
                Response.Cookies.Add(ck);
                ck = new HttpCookie("ck_Remenber", this.ckbRemember.Checked ? "1" : "0");
                ck.Expires = System.DateTime.Now.AddMonths(1);
                Response.Cookies.Add(ck);

                //恢复最近打开的页面
                HttpCookie LastPageCookie = Request.Cookies["LastPage"];
                if (LastPageCookie != null && Request.Url.ToString().IndexOf(LastPageCookie.Value) == -1)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(UserID, true);
                    Response.Redirect(System.Web.Security.FormsAuthentication.DefaultUrl + "#" + LastPageCookie.Value);
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(UserID, false);
                }
            }
            else
            {
                Exception ex = rtn[1] as Exception;
                StringBuilder sb = new StringBuilder();
                while (ex != null)
                {
                    sb.Append(ex.Message + "\r\n");
                    ex = ex.InnerException;
                }
                this.labInfo.Text = sb.ToString();
                this.txtUser.Focus();
            }
        }

        void SetStatusInfo()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly(); 
           
            this.labCopyright.Text = String.Format(@"版本:v{0}.{1} @{2} Powered By [Phone:138-1845-9481]",                
                assembly.GetName().Version.Major,
                assembly.GetName().Version.Minor,
                System.DateTime.Now.ToString("yyyy"));
        }
    }
}
