
#region Comment

/*
 * Project��    FineUI
 * 
 * FileName:    GridColumn.cs
 * CreatedOn:   2008-05-19
 * CreatedBy:   30372245@qq.com
 * 
 * 
 * Description��
 *      ->
 *   
 * History��
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
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;


namespace FineUI
{
    /// <summary>
    /// ����л��ࣨ�����ࣩ
    /// </summary>
    [ToolboxItem(false)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [DefaultProperty("HeaderText")]
    public abstract class GridColumn : ControlBase
    {
        #region Grid/ColumnIndex

        private Grid _grid;

        /// <summary>
        /// ������
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("������")]
        public Grid Grid
        {
            get
            {
                if (_grid == null)
                {
                    _grid = GetParentGrid();
                }
                return _grid;
            }
        }

        private Grid GetParentGrid()
        {
            if (Parent is Grid)
            {
                return (Grid)Parent;
            }
            else
            {
                return ResolveParentGrid(Parent as GridGroupColumn);
            }
        }

        private Grid ResolveParentGrid(GridGroupColumn groupColumn)
        {
            if (groupColumn != null)
            {
                if (groupColumn.Parent is Grid)
                {
                    return (Grid)groupColumn.Parent;
                }
                else
                {
                    return ResolveParentGrid(groupColumn.Parent as GridGroupColumn);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("������")]
        public int ColumnIndex
        {
            get
            {
                return Grid.AllColumns.IndexOf(this);
            }
        }

        #endregion

        #region SortField

        ///// <summary>
        ///// ��ǰ�е�������ʽ
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string SortExpression
        //{
        //    get
        //    {
        //        return String.Format("{0} {1}", SortField, SortDirection);
        //    }
        //}

        //public string _sortDirection = "ASC";

        ///// <summary>
        ///// ������
        ///// </summary>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string SortDirection
        //{
        //    get
        //    {
        //        return _sortDirection;
        //    }
        //    set
        //    {
        //        _sortDirection = value;
        //    }
        //}

        private string _sortField = String.Empty;

        /// <summary>
        /// �����ֶ�
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("�����ֶ�")]
        public string SortField
        {
            get
            {
                return _sortField;
            }
            set
            {
                _sortField = value;
            }
        }

        #endregion

        #region Properties

        private bool _hidden = false;

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(false)]
        [Description("�Ƿ�������")]
        public override bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                _hidden = value;
            }
        }



        //private string _columnID = String.Empty;

        ///// <summary>
        ///// ��ID
        ///// </summary>
        //[Category(CategoryName.OPTIONS)]
        //[DefaultValue("")]
        //[Description("��ID")]
        //public string ColumnID
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_columnID))
        //        {
        //            return String.Format("ct{0}", ColumnIndex);
        //        }
        //        return _columnID;
        //    }
        //    set
        //    {
        //        _columnID = value;
        //    }
        //}

        private string _columnID = String.Empty;

        /// <summary>
        /// ��ID�����û�����ã���ΪClientID��
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("��ID�����û�����ã���ΪClientID��")]
        public string ColumnID
        {
            get
            {
                if (String.IsNullOrEmpty(_columnID))
                {
                    return ClientID;
                }
                return _columnID;
            }
            set
            {
                _columnID = value;
            }
        }


        private string _headerText = String.Empty;

        /// <summary>
        /// ��������ʾ������
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue("")]
        [Description("��������ʾ������")]
        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
            }
        }


        private Unit _width = Unit.Empty;

        /// <summary>
        /// �п��
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(typeof(Unit), "")]
        [Description("�п��")]
        public Unit Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }


        private bool _expandUnusedSpace = false;

        /// <summary>
        /// ���л���չ����δʹ�õĿ��
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(false)]
        [Description("���л���չ����δʹ�õĿ��")]
        public bool ExpandUnusedSpace
        {
            get
            {
                return _expandUnusedSpace;
            }
            set
            {
                _expandUnusedSpace = value;
            }
        }


        private TextAlign _textalign = TextAlign.Left;

        /// <summary>
        /// �ı�������λ��
        /// </summary>
        [Category(CategoryName.OPTIONS)]
        [DefaultValue(TextAlign.Left)]
        [Description("�ı�������λ��")]
        public TextAlign TextAlign
        {
            get
            {
                return _textalign;
            }
            set
            {
                _textalign = value;
            }
        }


        #endregion

        #region virtual GetColumnValue/GetColumnState/PersistState

        /// <summary>
        /// ȡ����ͷ��Ⱦ���HTML
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal virtual string GetHeaderValue()
        {
            return String.IsNullOrEmpty(HeaderText) ? "&nbsp;" : HeaderText;
        }

        /// <summary>
        /// ȡ������Ⱦ���HTML
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal virtual string GetColumnValue(GridRow row)
        {
            return String.Empty;
        }


        /// <summary>
        /// �����Ƿ���Ҫ����״̬��Ŀǰֻ��CheckBoxFieldʵ����������壩
        /// </summary>
        internal virtual bool PersistState
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// ��ȡ�е�״̬
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal virtual object GetColumnState(GridRow row)
        {
            return null;
        }

        #endregion

        #region OnPreRender

        protected override void OnAjaxPreRender()
        {
            // ����пؼ������������Եĸı�
        }

        protected override void OnFirstPreRender()
        {
            base.OnFirstPreRender();

            // ����չ����Ҫ���⴦��
            if (this is TemplateField && (this as TemplateField).RenderAsRowExpander)
            {
                //string tplStr = String.Format(RowExpander.DataFormatString.Replace("{", "{{{").Replace("}", "}}}"), RowExpander.DataFields);
                //expanderScript = String.Format("var {0}=new Ext.ux.grid.RowExpander({{tpl:new Ext.Template({1})}});", Render_GridRowExpanderID, JsHelper.Enquote(tplStr));
                //expanderScript = String.Format("var {0}=new Ext.ux.grid.RowExpander({{tpl:new Ext.Template(\"{{{1}}}\")}});", Grid.Render_GridRowExpanderID, Grid.Render_GridRowExpanderID);


                string jsContent = String.Format("var {0}=new Ext.ux.grid.RowExpander({{tpl:new Ext.Template(\"{{{1}}}\")}});", XID, ColumnID);
                AddStartupScript(jsContent);
            }
            else
            {
                JsObjectBuilder columnBuilder = new JsObjectBuilder();

                OB.AddProperty("header", GetHeaderValue());

                if (Hidden)
                {
                    OB.AddProperty("hidden", true);
                }

                if (Grid.AllowSorting)
                {
                    if (!String.IsNullOrEmpty(SortField))
                    {
                        //// �Զ���JavaScript����
                        //OB.AddProperty("x_serverSortable", true);
                        OB.AddProperty("sortable", true);
                    }
                }

                if (PersistState)
                {
                    OB.AddProperty("x_persistState", true);
                    OB.AddProperty("x_persistStateType", "checkbox");
                }

                

                ////If not specified, the column's index is used as an index into the Record's data Array.
                //columnBuilder.AddProperty(OptionName.DataIndex, field.DataField);
                OB.AddProperty("dataIndex", ColumnID);
                OB.AddProperty("id", ColumnID);

                if (TextAlign != TextAlign.Left)
                {
                    OB.AddProperty("align", TextAlignName.GetName(TextAlign));
                }

                if (Width != Unit.Empty)
                {
                    OB.AddProperty("width", Width.Value);
                }

            }

            
        }

        #endregion

       

    }
}



