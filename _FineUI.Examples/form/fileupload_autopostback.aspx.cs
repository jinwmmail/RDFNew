﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUI.Examples.form
{
    public partial class fileupload_autopostback : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            if (filePhoto.HasFile)
            {
                string fileName = filePhoto.ShortFileName;
                fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
                fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

                filePhoto.SaveAs(Server.MapPath("~/upload/" + fileName));

                imgPhoto.ImageUrl = "~/upload/" + fileName;

                // 清空文件上传组件
                filePhoto.Reset();
            }

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (imgPhoto.ImageUrl == "~/images/blank.png")
            {
                filePhoto.MarkInvalid("请先上传个人头像！");

                Alert.ShowInTop("请先上传个人头像！");

                return;
            }

            labResult.Text = "<p>用户名：" + tbxUserName.Text + "</p>" +
                    "<p>邮箱：" + tbxEmail.Text + "</p>" +
                    "<p>头像地址：" + imgPhoto.ImageUrl + "</p>";

            // 清空表单字段（注意，不要清空imgPhoto，否则就看不到上传的头像了）
            filePhoto.Reset();
            tbxEmail.Reset();
            tbxUserName.Reset();

        }

    }
}