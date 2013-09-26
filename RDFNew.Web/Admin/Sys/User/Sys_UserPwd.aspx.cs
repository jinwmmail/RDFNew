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

namespace RDFNew.Web.Admin.Sys.User
{
    public partial class Sys_UserPwd : App_Com.PageBase
    {
        const string ModuleID = "Sys_UserPwd";

        protected void Page_Load(object sender, EventArgs e)
        {
            /**身份检测********************************************************/
            string Action = Request.QueryString["action"];
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!App_Com.Sys_User.CheckAuthorize(ModuleID, Action))
            {
                Response.Redirect("~/logout.aspx", true);
                return;
            }
            /**身份检测********************************************************/
            InitToolBar();
            if (!this.IsPostBack)
            {
                InitPage();
            }            
        }

        void InitPage()
        {
            string Action = Request.QueryString["action"];
            string Keyword = App_Com.Sys_User.GetUserInfo("UserID");
            if (Action.ToLower() == "add")
            {
                //初始化各控件
                SetControlState(Action);
            }
            if (Action.ToLower() == "edit" || Action.ToLower() == "view")
            {
                //初始化各控件
                SetControlState(Action);
                //取数据
                LoadMater(Keyword);
            }
        }

        void SetControlState(string Action)
        {
            this.txtUserID.Readonly = true;
            this.txtUserCode.Readonly = true;
            this.txtUserName.Readonly = true;
            this.txtNameE.Readonly = true;
            this.txtEmail.Readonly = true;
            this.txtPwd.Enabled = true;

            this.txtUserID.CssStyle = "background:#c0c0c0;";
            this.txtUserCode.CssStyle = "background:#c0c0c0;";
            this.txtUserName.CssStyle = "background:#c0c0c0;";
            this.txtNameE.CssStyle = "background:#c0c0c0;";
            this.txtEmail.CssStyle = "background:#c0c0c0;";

            this.txtPwd.Text = "";
            this.txtPwdNew.Text = "";
            this.txtPwdCfg.Text = "";            
        }

        void LoadMater(string Keyword)
        {
            RDFNew.Module.Admin.Sys.Sys_User obj = new RDFNew.Module.Admin.Sys.Sys_User();
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {                
                DataTable table = data[1] as DataTable;
                if (table.Rows.Count > 0)
                {
                    this.txtUserID.Text = String.Format("{0}", table.Rows[0]["UserID"]);
                    this.txtUserName.Text = String.Format("{0}", table.Rows[0]["UserName"]);
                    this.txtNameE.Text = String.Format("{0}", table.Rows[0]["NameE"]);
                    this.txtUserCode.Text = String.Format("{0}", table.Rows[0]["UserCode"]);
                    this.txtEmail.Text = String.Format("{0}", table.Rows[0]["Email"]);               
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

        void InitToolBar()
        {
            string Action = Request.QueryString["action"];

            FineUI.Button btn;            
            if (Action.ToLower() == "add" || Action.ToLower() == "edit")
            {
                btn = new FineUI.Button(); btn.ID = "SaveAndExit";
                btn.Click += new EventHandler(btn_Click);                
                btn.Text = "保存"; btn.IconAlign = IconAlign.Top; btn.EnablePostBack = true;
                btn.ValidateForms=new string[]{"Form2"};
                btn.Icon = (Icon)Enum.Parse(typeof(Icon), "SystemSave"); this.Toolbar1.Items.Add(btn);
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            FineUI.Button btn = sender as FineUI.Button;
            string Action = Request.QueryString["action"];
            if (btn.ID == "SaveAndExit")
            {
                try
                {
                    if (Action.ToLower()=="add")
                        AddData();
                    if (Action.ToLower() == "edit")
                        UpdateData();
                    SetControlState(Action);
                    Alert.Show("修改密码成功,请牢记新密码!", MessageBoxIcon.Information);                    
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
                    if (Action.ToLower() == "add")
                        RtnKey = AddData();
                    if (Action.ToLower() == "edit")
                        RtnKey = UpdateData();
                    //更新父页面
                    PageContext.RegisterStartupScript("parent.__doPostBack('','Changed');");
                    if (Action.ToLower() == "edit")
                    {
                        string url = Request.RawUrl.Replace("action=edit", "action=add") + "&keyword=";
                        PageContext.Redirect(url);
                    }
                    else
                    {
                        PageContext.Redirect(Request.RawUrl);
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

        string AddData()
        {
            return "";
        }

        string UpdateData()
        {
            RDFNew.Module.Admin.Sys.Sys_User obj = new RDFNew.Module.Admin.Sys.Sys_User();
            string Keyword = App_Com.Sys_User.GetUserInfo("UserID");
            object[] data = obj.GetMaster(Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    if (App_Com.Helper.StringToSHA1Hash(this.txtPwd.Text) != (dr["Pwd"] == System.DBNull.Value ? "" : dr["Pwd"].ToString()))                    
                        throw new Exception("您输入的原密码不正确.");                    
                    if (this.txtPwdNew.Text.Trim()!=this.txtPwdCfg.Text.Trim())
                        throw new Exception("您输入的新密码不一致,请重新输入.");
                    if (this.txtPwd.Text.Trim() == this.txtPwdNew.Text.Trim() && this.txtPwdNew.Text.Trim() == this.txtPwdCfg.Text.Trim())
                        throw new Exception("您输入的新密码不可与原密码相同,请重新输入.");
                    dr["Pwd"] = App_Com.Helper.StringToSHA1Hash(this.txtPwdNew.Text);
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified), null, null,
                           App_Com.Helper.BuildLog("Sys_User", "edit"));
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
    }
}
