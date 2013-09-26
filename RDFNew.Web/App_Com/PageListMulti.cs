using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

using FineUI;

namespace RDFNew.Web.App_Com
{
    public class PageListMulti : App_Com.PageListSingle
    {                      
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_WinSize = new int[] { 650,550 };
        }

        protected override void  OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
     
            }                
        }        
    }
}
