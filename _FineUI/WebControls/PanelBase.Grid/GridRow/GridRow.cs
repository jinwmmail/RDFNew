#region Comment

/*
 * Project��    FineUI
 * 
 * FileName:    GridRow.cs
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
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace FineUI
{
    // ȥ�� GridRow �� INamingContainer�������������������Ӷ�����ClientID�ĳ���
    [ToolboxItem(false)]
    public class GridRow : WebControl
    {
        #region Constructor

        public GridRow()
        {

        }

        public GridRow(Grid grid, object dataItem, int rowIndex)
        {
            _grid = grid;
            _dataItem = dataItem;
            _rowIndex = rowIndex;
        }

        #endregion

        #region Grid/DataItem/RowIndex

        private Grid _grid;

        public Grid Grid
        {
            get
            {
                return _grid;
            }
        }

        private object _dataItem = null;

        /// <summary>
        /// ���ж�Ӧ������Դ����ά��״̬��
        /// </summary>
        public object DataItem
        {
            get { return _dataItem; }
            set { _dataItem = value; }
        }


        private int _rowIndex = 0;

        /// <summary>
        /// �ڼ���
        /// </summary>
        public int RowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }


        #endregion

        #region Properties

        private string[] _values = null;

        /// <summary>
        /// ���е�״̬��Ϣ
        /// </summary>
        public string[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        private object[] _dataKeys = null;

        /// <summary>
        /// ����DataKeyNames�ֶε�ֵ
        /// </summary>
        public object[] DataKeys
        {
            get
            {
                return _dataKeys;
            }
            set
            {
                _dataKeys = value;
            }
        }

        private object[] _states = null;
        internal object[] States
        {
            get
            {
                return _states;
            }
            set
            {
                _states = value;
            }
        }

        //private object[] _extraValues = null;
        ///// <summary>
        ///// ����Ҫ�����ֵ������CheckBoxField��Ҫ�����Ƿ�ѡ�е�״̬��
        ///// </summary>
        //internal object[] ExtraValues
        //{
        //    get
        //    {
        //        return _extraValues;
        //    }
        //    set
        //    {
        //        _extraValues = value;
        //    }
        //}

        #endregion

        #region ToShortStates/FromShortStates

        /// <summary>
        /// ��ǰ����״̬�б�
        /// </summary>
        /// <returns></returns>
        internal object[] ToShortStates()
        {
            List<object> shortStates = new List<object>();
            Collection<GridColumn> columns = _grid.AllColumns;
            for (int i = 0, count = columns.Count; i < count; i++)
            {
                if (columns[i].PersistState)
                {
                    shortStates.Add(States[i]);
                }
            }
            return shortStates.ToArray();
        }

        /// <summary>
        /// �ָ���ǰ����״̬�б�ͬʱ������Ӧ��Valuesֵ��
        /// </summary>
        /// <param name="shortStates"></param>
        internal void FromShortStates(object[] shortStates)
        {
            Collection<GridColumn> columns = _grid.AllColumns;
            States = new object[columns.Count];
            int shortStateIndex = 0;
            for (int i = 0, count = columns.Count; i < count; i++)
            {
                GridColumn column = columns[i];
                if (column.PersistState)
                {
                    object state = shortStates[shortStateIndex++];
                    if (state is JValue)
                    {
                        States[i] = (state as JValue).Value;
                    }
                    else
                    {
                        States[i] = state;
                    }


                    UpdateValuesAt(i);
                }
            }
        }


        /// <summary>
        /// ���µ�ǰ��ĳ�е���Ⱦ���HTML
        /// </summary>
        /// <param name="columnIndex"></param>
        internal void UpdateValuesAt(int columnIndex)
        {
            Values[columnIndex] = RemoveNewLine(_grid.AllColumns[columnIndex].GetColumnValue(this));
        }

        #endregion

        #region TemplateContainers

        private GridRowControl[] _templateContainers = null;
        public GridRowControl[] TemplateContainers
        {
            get
            {
                return _templateContainers;
            }
            set
            {
                _templateContainers = value;
            }
        }

        public void InitTemplateContainers()
        {
            Collection<GridColumn> columns = _grid.AllColumns;
            TemplateContainers = new GridRowControl[columns.Count];

            for (int i = 0, count = columns.Count; i < count; i++)
            {
                GridColumn column = columns[i];
                if (column is TemplateField)
                {
                    TemplateField field = column as TemplateField;
                    GridRowControl control = new GridRowControl(DataItem, RowIndex);
                    //control.ID = String.Format("{0}_{1}_{2}", Grid.ID, RowIndex, column.ColumnIndex);
                    control.ID = String.Format("c{0}r{1}", column.ColumnIndex, RowIndex);

                    field.ItemTemplate.InstantiateIn(control);

                    Controls.Add(control);
                    TemplateContainers[column.ColumnIndex] = control;

                }

            }
        }

        #endregion

        #region DataBindRow

        /// <summary>
        /// ���ӿؼ�
        /// </summary>
        protected override void DataBindChildren()
        {
            base.DataBindChildren();

            DataBindRow();
        }

        /// <summary>
        /// ���е�ֵ
        /// </summary>
        internal void DataBindRow()
        {
            Collection<GridColumn> columns = _grid.AllColumns;

            // ����ÿ�е�ֵ
            Values = new string[columns.Count];
            States = new object[columns.Count];
            
            for (int i = 0, count = columns.Count; i < count; i++)
            {
                GridColumn column = columns[i];
                Values[i] = RemoveNewLine(column.GetColumnValue(this));

                if (column.PersistState)
                {
                    States[i] = column.GetColumnState(this);
                }
            }

            // ����DataKeys��ֵ
            if (_grid.DataKeyNames != null)
            {
                string[] keyNames = _grid.DataKeyNames;
                DataKeys = new object[keyNames.Length];
                for (int j = 0, count = keyNames.Length; j < count; j++)
                {
                    DataKeys[j] = GetPropertyValue(keyNames[j]);
                }
            }
        }

        
        internal object GetPropertyValue(string propertyName)
        {
            return ObjectUtil.GetPropertyValue(DataItem, propertyName);
        }

        private string RemoveNewLine(string columnValue)
        {
            // ɾ������HTML�е� "\r\n     "
            return Regex.Replace(columnValue, "\r?\n\\s*", "");
        }

        #endregion

        #region RenderBeginTag

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //base.RenderBeginTag(writer);

            //writer.Write("<div id=\"ok\">");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //base.RenderEndTag(writer);

            //writer.Write("</div>");
        }

        #endregion

        #region FindControl

        public override Control FindControl(string id)
        {
            foreach (GridRowControl control in TemplateContainers)
            {
                if (control != null)
                {
                    Control found = control.FindControl(id);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        #endregion

        #region old code

        ///// <summary>
        ///// ȡ�����Ե�ֵ
        ///// </summary>
        ///// <param name="rowObj"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public object GetPropertyValue(string propertyName)
        //{
        //    object rowObj = _dataItem;
        //    object result = null;

        //    try
        //    {
        //        if (rowObj is DataRow)
        //        {
        //            result = (rowObj as DataRow)[propertyName];
        //        }
        //        else
        //        {
        //            result = ObjectUtil.GetPropertyValueFormObject(rowObj, propertyName);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // �Ҳ���������
        //    }

        //    return result;
        //}




        #endregion

        #region old code

        ///// <summary>
        ///// Returns a value from the item indexed by the field name or index.
        ///// </summary>
        ///// <param name="obj">Field name or numeric index.</param>
        ///// <returns>Cell value</returns>
        //public object this[object obj]
        //{
        //    get
        //    {
        //        if (obj is string)
        //        {
        //            if (_columns != null && _values != null)
        //            {
        //                int iColumnIndex = _columns.IndexOf((string)obj);
        //                if (iColumnIndex >= 0)
        //                {
        //                    return _values[iColumnIndex];
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else if (obj is int)
        //        {
        //            return _values[(int)obj];
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Only a string (field name) or integer index is permitted.");
        //        }
        //    }
        //    set
        //    {
        //        if (obj is string)
        //        {
        //            if (_columns != null && _values != null)
        //            {
        //                _values[_columns.IndexOf((string)obj)] = value;
        //            }
        //        }
        //        else if (obj is int)
        //        {
        //            _values[(int)obj] = value;
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Only a string (column name) or int parameter are permitted.");
        //        }
        //    }
        //}



        ///// <summary>
        ///// Returns whether this item equals the passed-in item.
        ///// </summary>
        ///// <param name="o">A GridItem.</param>
        ///// <returns>Whether this item equals the passed-in item.</returns>
        //public override bool Equals(object o)
        //{
        //    if (o is GridItem && o != null)
        //    {
        //        GridItem other = (GridItem)o;

        //        for (int i = 0; i < _values.Length; i++)
        //        {
        //            if (!Object.Equals(this[i], other[i]))
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }

        //    return false;
        //} 

        #endregion

    }
}



