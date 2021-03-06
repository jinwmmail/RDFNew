﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace FineUI.Examples.grid
{
    public partial class grid_checkboxfield_autopostback : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #region BindGrid

        private void BindGrid()
        {
            DataTable table = GetDataTable();

            Grid1.DataSource = table;
            Grid1.DataBind();

        }

        #endregion

        #region Events


        protected void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            if (e.CommandName == "CheckBox1")
            {
                CheckBoxField checkField = (CheckBoxField)Grid1.FindColumn(e.ColumnIndex);
                bool checkState = checkField.GetCheckedState(e.RowIndex);
                Alert.ShowInTop(String.Format("你点击了第 {0} 行，第 {1} 列，选中状态：{2}", e.RowIndex + 1, e.ColumnIndex + 1, checkState));
            }
            else if (e.CommandName == "Action1" || e.CommandName == "Action2")
            {
                Alert.ShowInTop(String.Format("你点击了第 {0} 行，第 {1} 列，行命令是 {2}", e.RowIndex + 1, e.ColumnIndex + 1, e.CommandName));
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            CheckBoxField field2 = (CheckBoxField)Grid1.FindColumn("CheckBoxField2");

            StringBuilder sb = new StringBuilder();
            int selectedCount = Grid1.SelectedRowIndexArray.Length;
            if (selectedCount > 0)
            {
                sb.AppendFormat("共选中了 {0} 行：", selectedCount);
                sb.Append("<ol class=\"result\">");
                for (int i = 0; i < selectedCount; i++)
                {
                    int rowIndex = Grid1.SelectedRowIndexArray[i];
                    sb.Append("<li><ul>");
                    
                    sb.AppendFormat("<li>行号：{0}</li>", rowIndex + 1);
                    // Grid1.Rows[rowIndex].States[field1.ColumnIndex] 和 field1.GetCheckedState(rowIndex) 的结果相同
                    sb.AppendFormat("<li>是否在校（自动会发）：{0}</li>", field2.GetCheckedState(rowIndex));
                    
                    sb.Append("</ul></li>");
                }
                sb.Append("</ol>");
            }
            else
            {
                sb.Append("<strong>没有选中任何一行！</strong>");
            }

            labResult.Text = sb.ToString();
        }


        #endregion


    }
}
