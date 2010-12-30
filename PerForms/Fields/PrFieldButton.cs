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
    public class PrFieldButton : PrField
    {
        protected virtual string Label { get; set; }
        protected virtual string ButtonText { get; set; }
        private bool Hidden;
        private bool MergeWithNext;

        public PrFieldButton(PrForm form, string fieldKey, string buttonText)
            : base(form, fieldKey)
        {
            Label = " ";
            ButtonText = new Escaper().EscapeJavaScript(buttonText);
            Hidden = false;
            MergeWithNext = false;
        }

        public PrFieldButton SetFieldLabel(string fieldLabel)
        {
            Label = new Escaper().EscapeJavaScript(fieldLabel);
            return this;
        }

        public PrFieldButton SetHidden()
        {
            Hidden = true;
            return this;
        }

        public PrFieldButton SetMergeWithNext()
        {
            MergeWithNext = true;
            return this;
        }

        public PrFieldButton AddGetAJAXActionsAll(string actionKey)
        {
            return AddGetAJAXActions(actionKey, "ALL");
        }

        public PrFieldButton AddGetAJAXActions(string actionKey)
        {
            return AddGetAJAXActions(actionKey, FieldKey);
        }

        public PrFieldButton AddGetAJAXActions(string actionKey, string fieldGroupKey)
        {
            Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').addGetAJAXActions('" + actionKey + "', '" + fieldGroupKey + "');";
            return this;
        }

        public override string Render()
        {
            string s_MergeWithNext = MergeWithNext ? "true" : "false";
            
            string o = "{";
            o += "mergeWithNext: " + s_MergeWithNext;
            o += "}";

            string js = "form_" + Form.FormKey + ".addFieldButton('" + FieldKey + "', '" + ButtonText + "','" + Label + "'," + o + ");";
            if (Hidden) Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').hide();";
            return js;
        }

        public PrFieldButton AddCancel()
        {
            Form.OnFormReady += "form_" + Form.FormKey + ".getFieldByKey('" + FieldKey + "').addCancel();";
            return this;
        }
    }
}
