
#region Comment

/*
 * Project：    FineUI
 * 
 * FileName:    CheckBoxField.cs
 * CreatedOn:   2008-05-27
 * CreatedBy:   30372245@qq.com
 * 
 * 
 * Description：
 *      ->
 *   
 * History：
 *      ->
 * 
 * 
 * 
 * 
 */

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;


namespace FineUI
{
    /// <summary>
    /// 表格复选框列
    /// </summary>
    [ToolboxItem(false)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class CheckBoxField : BaseField
    {

        #region Properties

        private bool _enabled = true;

        /// <summary>
        /// 是否可用（只在RenderAsStaticField=false时有效）
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(true)]
        [Description("是否可用（只在RenderAsStaticField=false时有效）")]
        public override bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        private bool _autoPostBack = false;

        /// <summary>
        /// 是否自动回发（只在RenderAsStaticField=false时有效）
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(false)]
        [Description("是否自动回发（只在RenderAsStaticField=false并且ShowHeaderCheckBox=false时有效）")]
        public bool AutoPostBack
        {
            get
            {
                return _autoPostBack;
            }
            set
            {
                _autoPostBack = value;
            }
        }


        public string _dataField = "";

        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("字段名称")]
        public string DataField
        {
            get
            {
                return _dataField;
            }
            set
            {
                _dataField = value;
            }
        }


        public bool _renderAsStaticField = true;

        /// <summary>
        /// 渲染为静态图片，否则渲染为可编辑的复选框
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(true)]
        [Description("渲染为静态图片，否则渲染为可编辑的复选框")]
        public bool RenderAsStaticField
        {
            get
            {
                return _renderAsStaticField;
            }
            set
            {
                _renderAsStaticField = value;
            }
        }


        public bool _showHeaderCheckBox = false;

        /// <summary>
        /// 显示列头复选框（只在RenderAsStaticField=false时有效）
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(false)]
        [Description("显示列头复选框（只在RenderAsStaticField=false时有效）")]
        public bool ShowHeaderCheckBox
        {
            get
            {
                return _showHeaderCheckBox;
            }
            set
            {
                _showHeaderCheckBox = value;
            }
        }

        #endregion

        #region CommandName

        private string _commandName = "";

        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("")]
        public string CommandName
        {
            get
            {
                return _commandName;
            }
            set
            {
                _commandName = value;
            }
        }

        private string _commandArgument = "";

        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("")]
        public string CommandArgument
        {
            get
            {
                return _commandArgument;
            }
            set
            {
                _commandArgument = value;
            }
        }


        #endregion

        #region GetHeaderValue/GetColumnValue

        internal override string GetHeaderValue()
        {
            if (!RenderAsStaticField && ShowHeaderCheckBox)
            {
                string result = String.Empty;

                string textAlignClass = String.Empty;
                if (TextAlign != TextAlign.Left)
                {
                    textAlignClass = "box-grid-checkbox-" + TextAlignName.GetName(TextAlign);
                }

                string onClickScript = "Ext.get(this).toggleClass('box-grid-checkbox-unchecked');";
                onClickScript += "X.util.stopEventPropagation.apply(null, arguments);";

                //string tooltip = String.Empty;
                //if (!String.IsNullOrEmpty(HeaderText))
                //{
                //    tooltip = String.Format(" ext:qtip=\"{0}\" ", HeaderText);
                //}

                result = String.Format("<div class=\"box-grid-checkbox box-grid-checkbox-unchecked {0}\" onclick=\"{1}\">{2}</div>", textAlignClass, onClickScript, HeaderText);

                return result;
            }
            else
            {
                return base.GetHeaderValue();
            }
        }


        internal override string GetColumnValue(GridRow row)
        {
            string result = String.Empty;

            bool checkState = Convert.ToBoolean(GetColumnState(row));

            result = GetColumnValue(row, checkState);

            string tooltip = GetTooltipString(row);
            if (!String.IsNullOrEmpty(tooltip))
            {
                result = result.ToString().Insert("<div".Length, tooltip);
            }

            return result;
        }

        /// <summary>
        /// 取得单元格的数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        private string GetColumnValue(GridRow row, bool checkState)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(DataField))
            {
                string textAlignClass = String.Empty;
                if (TextAlign != TextAlign.Left)
                {
                    textAlignClass = "box-grid-checkbox-" + TextAlignName.GetName(TextAlign);
                }

                if (RenderAsStaticField)
                {
                    if (checkState)
                    {
                        result = "<div class=\"box-grid-static-checkbox " + textAlignClass + "\"></div>";
                    }
                    else
                    {
                        result = "<div class=\"box-grid-static-checkbox box-grid-static-checkbox-unchecked " + textAlignClass + "\"></div>";
                    }
                }
                else
                {
                    string paramStr = String.Format("Command${0}${1}${2}${3}", row.RowIndex, ColumnIndex, CommandName.Replace("'", "\""), CommandArgument.Replace("'", "\""));
                    // 延迟执行
                    string postBackReference = JsHelper.GetDeferScript(Grid.GetPostBackEventReference(paramStr), 0);

                    // string onClickScript = String.Format("{0}_checkbox{1}(event,this,{2});", Grid.XID, ColumnIndex, row.RowIndex);
                    string onClickScript = "Ext.get(this).toggleClass('box-grid-checkbox-unchecked');";
                    if (!ShowHeaderCheckBox && AutoPostBack)
                    {
                        onClickScript += postBackReference;
                    }

                    onClickScript += "X.util.stopEventPropagation.apply(null, arguments);";

                    if (checkState)
                    {
                        if (Enabled)
                        {
                            result = String.Format("<div class=\"box-grid-checkbox {0}\" onclick=\"{1}\"></div>", textAlignClass, onClickScript);
                        }
                        else
                        {
                            result = String.Format("<div class=\"box-grid-checkbox box-grid-checkbox-disabled {0}\"></div>", textAlignClass);
                        }
                    }
                    else
                    {
                        if (Enabled)
                        {
                            result = String.Format("<div class=\"box-grid-checkbox box-grid-checkbox-unchecked {0}\" onclick=\"{1}\"></div>", textAlignClass, onClickScript);
                        }
                        else
                        {
                            result = String.Format("<div class=\"box-grid-checkbox box-grid-checkbox-disabled box-grid-checkbox-unchecked-disabled {0}\"></div>", textAlignClass);
                        }
                    }
                }
            }

            return result;
        }


        //public override string GetFieldType()
        //{
        //    return "string";
        //}

        #endregion

        #region SaveColumnState/LoadColumnState

        internal override bool PersistState
        {
            get
            {
                if (RenderAsStaticField)
                {
                    return false;
                }
                return true;
            }
        }

        internal override object GetColumnState(GridRow row)
        {
            if (row.DataItem == null)
            {
                return row.States[ColumnIndex];
            }
            else
            {
                return row.GetPropertyValue(DataField);
            }
        }

        #endregion

        #region GetCheckedState/SetCheckedState

        /// <summary>
        /// 本行的复选框是否处于选中状态
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <returns>选中状态</returns>
        public bool GetCheckedState(int rowIndex)
        {
            GridRow row = this.Grid.Rows[rowIndex];

            return Convert.ToBoolean(row.States[ColumnIndex]);
        }

        /// <summary>
        /// 设置本列复选框的选中状态
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="isChecked">是否选中</param>
        public void SetCheckedState(int rowIndex, bool isChecked)
        {
            GridRow row = this.Grid.Rows[rowIndex];

            row.States[ColumnIndex] = isChecked;

            // 更新列状态的同时，要记着更新列渲染后的HTML
            row.UpdateValuesAt(ColumnIndex);
        } 

        #endregion

        #region old code

        ///// <summary>
        ///// 维持页面上CheckBox的值
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public List<bool> Checked
        //{
        //    get
        //    {
        //        object obj = ViewState["Checked"];
        //        return obj == null ? (new List<bool>()) : (List<bool>)obj;
        //    }
        //    set
        //    {
        //        ViewState["Checked"] = value;
        //    }
        //}

        #endregion
    }
}



