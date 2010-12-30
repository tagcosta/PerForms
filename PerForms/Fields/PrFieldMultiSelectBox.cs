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
using PerForms.Util;

namespace PerForms.Fields
{
    public class PrFieldMultiSelectBox : PrField
    {
        private bool Required;
        private string Label { get; set; }
        private List<KeyValue> Data { get; set; }
        private bool Hidden;
        private List<string> DefaultValues;
        private bool MergeWithNext;

        public PrFieldMultiSelectBox(PrForm form, string fieldKey, string label, List<KeyValue> data)
            : base(form, fieldKey)
        {
            Label = new Escaper().EscapeJavaScript(label);
            Data = data;
            if (Data == null)
            {
                Data = new List<KeyValue>();
            }
            Required = false;
            Hidden = false;
            DefaultValues = new List<string>();
            MergeWithNext = false;
        }

        public PrFieldMultiSelectBox SetDefaultValues(string values)
        {
            return SetDefaultValues(values.Split(',').ToList<string>());
        }

        public PrFieldMultiSelectBox SetDefaultValues(List<string> values)
        {
            DefaultValues = values;
            return this;
        }

        public PrFieldMultiSelectBox SetRequired()
        {
            Required = true;
            return this;
        }

        public PrFieldMultiSelectBox SetHidden()
        {
            Hidden = true;
            return this;
        }

        public PrFieldMultiSelectBox SetMergeWithNext()
        {
            MergeWithNext = true;
            return this;
        }

        public PrFieldMultiSelectBox AddGetAJAXActionsAll(string actionKey)
        {
            return AddGetAJAXActions(actionKey, "ALL");
        }

        public PrFieldMultiSelectBox AddGetAJAXActions(string actionKey)
        {
            return AddGetAJAXActions(actionKey, FieldKey);
        }

        public PrFieldMultiSelectBox AddGetAJAXActions(string actionKey, string fieldGroupKey)
        {
            Form.OnPreFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').addGetAJAXActions('" + actionKey + "', '" + fieldGroupKey + "');";
            return this;
        }

        public override string Render()
        {
            string s_Required = Required ? "true" : "false";
            string s_MergeWithNext = MergeWithNext ? "true" : "false";

            string o = "{";
            o += "required: " + s_Required + ",";
            o += "mergeWithNext: " + s_MergeWithNext;
            o += "}";

            var items = "[";
            foreach (KeyValue kv in Data)
            {
                items += "{ Key: '" + kv.Key + "', Value: '" + kv.Value + "' },";
            }
            items = items.TrimEnd(',') + "]";
            string js = "form_" + Form.FormKey + ".addFieldMultiSelectBox('" + FieldKey + "', '" + Label + "', " + items + "," + o + ");";

            if (Hidden) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').hide();";
            if (DefaultValues.Count > 0)
            {
                string DefaultValue = "[";
                foreach (string value in DefaultValues)
                {
                    DefaultValue += "'" + value + "',";
                }
                DefaultValue = DefaultValue.TrimEnd(',') + "]";
                Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').setValues(" + DefaultValue + ");";
            }
            return js;
        }
    }
}