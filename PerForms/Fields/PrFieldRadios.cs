﻿#region License
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
    public class PrFieldRadios : PrField
    {
        private bool Required;
        private string Label { get; set; }
        private List<KeyValue> Data { get; set; }
        private bool Hidden;
        private string DefaultValue;
        private bool MergeWithNext;

        public PrFieldRadios(PrForm form, string fieldKey, string label, List<KeyValue> data)
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
            DefaultValue = string.Empty;
            MergeWithNext = false;
        }

        public PrFieldRadios SetDefaultValue(string value)
        {
            DefaultValue = new Escaper().EscapeJavaScript(value);
            return this;
        }

        public PrFieldRadios SetRequired()
        {
            Required = true;
            return this;
        }

        public PrFieldRadios SetHidden()
        {
            Hidden = true;
            return this;
        }

        public PrFieldRadios SetMergeWithNext()
        {
            MergeWithNext = true;
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
            string js = "form_" + Form.FormKey + ".addFieldRadios('" + FieldKey + "', '" + Label + "', " + items + "," + o + ");";
            if (Hidden) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').hide();";
            if (!string.IsNullOrEmpty(DefaultValue)) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').setValues(['" + DefaultValue + "']);";
            return js;
        }

        public PrFieldRadios AddConditionalVisibility(Enum greenValue, string targetGroupKey)
        {
            return AddConditionalVisibility((Convert.ToInt32(greenValue)), targetGroupKey);
        }

        public PrFieldRadios AddConditionalVisibility(int greenValue, string targetGroupKey)
        {
            return AddConditionalVisibility(greenValue.ToString(), targetGroupKey);
        }

        public PrFieldRadios AddConditionalVisibility(string greenValues, string targetGroupKey)
        {
            return AddConditionalVisibility(greenValues.Split(',').ToList<string>(), targetGroupKey);
        }

        public PrFieldRadios AddConditionalVisibility(List<string> greenValues, string targetGroupKey)
        {
            string items = "[";
            foreach (string greenValue in greenValues)
            {
                items += greenValue;
            }
            items = items.TrimEnd(',') + "]";
            Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').addConditionalVisibility(" + items + ", '" + targetGroupKey + "');";
            return this;
        }
    }
}
