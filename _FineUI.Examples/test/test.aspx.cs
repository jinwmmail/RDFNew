using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

namespace FineUI.Examples.test
{
    public partial class test : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //cblone.Items.Add("1", "1");
                //cblone.Items.Add("2", "2");
            }

        }

        protected void btnHello_Click(object sender, EventArgs e)
        {
            if (cblone.Items.Count == 0)
            {
                cblone.Items.Add("1", "1");
                cblone.Items.Add("2", "2");
            }
            else
            {
                cblone.Items.Clear();
            }
        }



    }
}
