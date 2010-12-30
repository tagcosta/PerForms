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
using PerForms.JQGrid;
using PerForms.Util;

namespace PerForms.Fields
{
    public class PrFieldGridView : PrField
    {
        private bool ClientSide { get; set; }
        
        private string FieldGroupKey { get; set; }
        private string ActionKey { get; set; }
        private DataTable Data { get; set; }
        private bool MergeWithNext;
        private string FieldLabel { get; set; }
        private string TableLabel { get; set; }
        private bool MultipleSelection { get; set; }
        private bool Required;

        public PrFieldGridView(PrForm form, string fieldKey, string actionKey, FormService formService, bool clientSide, string tableLabel)
            :base(form, fieldKey)
        {
            ActionKey = actionKey;
            ClientSide = clientSide;

            if (ClientSide)
            {
                Data = formService.GetDataTableAll(actionKey, null);
            }
            else
            {
                Data = formService.GetDataTableZero(actionKey, null);
            }
            MergeWithNext = false;
            FieldLabel = "";
            TableLabel = new Escaper().EscapeJavaScript(tableLabel);
            MultipleSelection = false;
            Required = false;
        }

        public PrFieldGridView(PrForm form, string fieldKey, DataTable dt, string tableLabel)
            : base(form, fieldKey)
        {
            ClientSide = true;
            Data = dt;
            MergeWithNext = false;
            FieldLabel = "";
            TableLabel = new Escaper().EscapeJavaScript(tableLabel);
            MultipleSelection = false;
            Required = false;
        }

        public PrFieldGridView SetFieldLabel(string fieldLabel)
        {
            FieldLabel = new Escaper().EscapeJavaScript(fieldLabel);
            return this;
        }

        public PrFieldGridView SetMultipleSelection()
        {
            MultipleSelection = true;
            return this;
        }

        public PrFieldGridView SetMergeWithNext()
        {
            MergeWithNext = true;
            return this;
        }

        public PrFieldGridView SetRequired()
        {
            Required = true;
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

            string sMultipleSelection = MultipleSelection ? "1" : "0";
            string js = "";
            if (ClientSide)
            {
                DataTableJQGridConv dttgvd = new DataTableJQGridConv(Data);

                js = "form_" + Form.FormKey + ".addFieldGridView('" + FieldKey + "', '" + FieldLabel + "', " + dttgvd.Columns + ", " + dttgvd.Rows + ", '" + ActionKey + "', true, '" + TableLabel + "', " + sMultipleSelection + "," + o + ");";
            }
            else
            {
                DataTableJQGridConv dttgvd = new DataTableJQGridConv(Data);

                js = "form_" + Form.FormKey + ".addFieldGridView('" + FieldKey + "', '" + FieldLabel + "', " + dttgvd.Columns + ", null, '" + ActionKey + "', false, '" + TableLabel + "', " + sMultipleSelection + "," + o + ");";
            }

            return js;
        }
    }
}