#region License
// Copyright (c) 2010 Tiago Costa
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PerForms.Util;
using System.Web.UI;

namespace PerForms.Fields
{
    public abstract class PrField
    {
        protected virtual PrForm Form { get; set; }
        internal virtual string FieldKey { get; set; }
        private List<Validation> Validations { get; set; }

        public PrField(PrForm form, string fieldKey)
        {
            Form = form;
            FieldKey = fieldKey;
            Validations = new List<Validation>();
        }

        public abstract string Render();

        internal virtual string RenderValidations()
        {
            if (Validations.Count > 0)
            {
                string str = "";
                foreach (Validation val in Validations)
                {
                    str += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "')." + val.Render();
                }
                return str;
            }
            else return string.Empty;
        }

        public virtual PrField AddValidation(string regex, string message)
        {
            Validations.Add(new Validation(regex, message));
            return this;
        }

        #region FluentAPI

        public PrForm AddGroup(string groupKey, params string[] fieldKeys)
        {
            return Form.AddGroup(groupKey, fieldKeys);
        }

        public PrFieldText AddFieldText(string fieldKey, string label)
        {
            return Form.AddFieldText(fieldKey, label);
        }

        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label)
        {
            return Form.AddFieldComboBox(fieldKey, label);
        }

        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, List<KeyValue> data)
        {
            return Form.AddFieldComboBox(fieldKey, label, data);
        }

        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, DataTable dt, string valueColumnName, string displayColumnName)
        {
            return Form.AddFieldComboBox(fieldKey, label, dt, valueColumnName, displayColumnName);
        }

        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, DataTable dt)
        {
            return Form.AddFieldComboBox(fieldKey, label, dt);
        }

        public PrFieldButton AddFieldButton(string fieldKey, string buttonText)
        {
            return Form.AddFieldButton(fieldKey, buttonText);
        }

        public PrFieldTextArea AddFieldTextArea(string fieldKey, string label)
        {
            return Form.AddFieldTextArea(fieldKey, label);
        }

        public PrFieldRadios AddFieldRadios(string fieldKey, string label)
        {
            return Form.AddFieldRadios(fieldKey, label);
        }

        public PrFieldRadios AddFieldRadios(string fieldKey, string label, List<KeyValue> data)
        {
            return Form.AddFieldRadios(fieldKey, label, data);
        }

        public PrFieldRadios AddFieldRadios(string fieldKey, string label, DataTable dt, string valueColumnName, string displayColumnName)
        {
            return Form.AddFieldRadios(fieldKey, label, dt, valueColumnName, displayColumnName);
        }

        public PrFieldRadios AddFieldRadios(string fieldKey, string label, DataTable dt)
        {
            return Form.AddFieldRadios(fieldKey, label, dt);
        }

        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label)
        {
            return Form.AddFieldCheckBoxes(fieldKey, label);
        }

        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, List<KeyValue> data)
        {
            return Form.AddFieldCheckBoxes(fieldKey, label, data);
        }

        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, DataTable dt, string valueColumnName, string displayColumnName)
        {
            return Form.AddFieldCheckBoxes(fieldKey, label, dt, valueColumnName, displayColumnName);
        }

        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, DataTable dt)
        {
            return Form.AddFieldCheckBoxes(fieldKey, label, dt);
        }

        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label)
        {
            return Form.AddFieldMultiSelectBox(fieldKey, label);
        }

        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, List<KeyValue> data)
        {
            return Form.AddFieldMultiSelectBox(fieldKey, label, data);
        }

        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, DataTable dt, string valueColumnName, string displayColumnName)
        {
            return Form.AddFieldMultiSelectBox(fieldKey, label, dt, valueColumnName, displayColumnName);
        }

        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, DataTable dt)
        {
            return Form.AddFieldMultiSelectBox(fieldKey, label, dt);
        }

        public PrFieldGridView AddFieldGridView(string fieldKey, string actionKey, FormService formService, bool clientSide, string tableLabel)
        {
            return Form.AddFieldGridView(fieldKey, actionKey, formService, clientSide, tableLabel);
        }

        public PrFieldGridView AddFieldGridView(string fieldKey, DataTable dt, string tableLabel)
        {
            return Form.AddFieldGridView(fieldKey, dt, tableLabel);
        }

        public PrFieldLabel AddFieldLabel(string fieldKey, string label, string defaultValue)
        {
            return Form.AddFieldLabel(fieldKey, label, defaultValue);
        }

        public PrFieldDate AddFieldDate(string fieldKey, string label)
        {
            return Form.AddFieldDate(fieldKey, label);
        }

        public PrFieldUpload AddFieldUpload(string fieldKey, string label)
        {
            return Form.AddFieldUpload(fieldKey, label);
        }

        public PrFieldFree AddFieldFree(string fieldKey, string label, string source)
        {
            return Form.AddFieldFree(fieldKey, label, source);
        }

        public PrFieldFree AddFieldFree(string fieldKey, string label, Control source)
        {
            return Form.AddFieldFree(fieldKey, label, source);
        }

        public PrFieldPlaceHolder AddFieldPlaceHolder(string render)
        {
            return Form.AddFieldPlaceHolder(render);
        }

        public PrForm AddTab(string label)
        {
            return Form.AddTab(label);
        }

        public PrForm AddTabArea()
        {
            return Form.AddTabArea();
        }

        public PrForm EndTab()
        {
            return Form.EndTab();
        }

        public PrForm EndTabArea()
        {
            return Form.EndTabArea();
        }

        #endregion

        public string RenderForm()
        {
            return Form.RenderForm();
        }

    }
}
