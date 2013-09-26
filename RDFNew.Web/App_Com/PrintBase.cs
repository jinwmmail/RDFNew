using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PrintBase : App_Com.PageBase
    {
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
        }    
    }
}
