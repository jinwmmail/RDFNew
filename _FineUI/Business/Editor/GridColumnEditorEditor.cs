﻿
#region Comment

/*
 * Project：    FineUI
 * 
 * FileName:    FieldEditor.cs
 * CreatedOn:   2013-05-01
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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace FineUI
{
    public class GridColumnEditorEditor : CollectionEditor
    {
        private Type[] types;

        public GridColumnEditorEditor(Type type)
            : base(type)
        {
            types = new Type[] {
                typeof(CheckBox),
                typeof(CheckBoxList),
                typeof(HtmlEditor),
                typeof(Label),
                typeof(HyperLink),
                typeof(Image),
                typeof(LinkButton),
                typeof(RadioButton),
                typeof(RadioButtonList),
                typeof(DropDownList),
                typeof(DatePicker),
                typeof(FileUpload),
                typeof(HiddenField),
                typeof(NumberBox),
                typeof(TextArea),
                typeof(TextBox),
                typeof(TimePicker),
                typeof(TriggerBox),
                typeof(TwinTriggerBox)
                
            };
        }

        protected override Type[] CreateNewItemTypes()
        {
            return types;
        }

    }
}
