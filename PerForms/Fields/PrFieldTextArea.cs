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
    public class PrFieldTextArea : PrField
    {
        private bool Required;
        protected virtual string Label { get; set; }
        private bool Hidden;
        private string DefaultValue;
        private bool MergeWithNext;

        public PrFieldTextArea(PrForm form, string fieldKey, string label)
            : base(form, fieldKey)
        {
            Label = new Escaper().EscapeJavaScript(label);
            Required = false;
            Hidden = false;
            DefaultValue = string.Empty;
            MergeWithNext = false;
        }

        public PrFieldTextArea SetDefaultValue(string value)
        {
            DefaultValue = new Escaper().EscapeJavaScript(value);
            return this;
        }

        public PrFieldTextArea SetRequired()
        {
            Required = true;
            return this;
        }

        public PrFieldTextArea SetHidden()
        {
            Hidden = true;
            return this;
        }

        public PrFieldTextArea SetMergeWithNext()
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

            string js = "form_" + Form.FormKey + ".addFieldTextArea('" + FieldKey + "', '" + Label + "'," + o + ");";
            if (Hidden) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').hide();";
            if (!string.IsNullOrEmpty(DefaultValue)) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').setValues(['" + DefaultValue + "']);";
            return js;
        }
    }
}
