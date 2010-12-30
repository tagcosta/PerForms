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
using System.Collections.Specialized;
using PerForms.Util;

namespace PerForms.JQGrid
{
    [Serializable]
    public class QueryStringInfo
    {
        public int rows { get; set; }
        public int page { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }

        public QueryStringFilter filters { get; set; }

        public JQGrid GetJQGrid(DataTable dataTable, int totalRecords)
        {
            if (dataTable.Columns.Contains("PageNumber")) dataTable.Columns.Remove("PageNumber");

            JQGrid jqg = new JQGrid
            {
                page = page,
                records = totalRecords,
                total = Convert.ToInt32(Math.Ceiling((double)totalRecords / rows))
            };
            jqg.AddRows(dataTable);
            return jqg;
        }

        public QueryStringInfo Fill(NameValueCollection nvc)
        {
            if (!nvc.AllKeys.Contains("rows")
                || !nvc.AllKeys.Contains("page")
                || !nvc.AllKeys.Contains("sidx")
                || !nvc.AllKeys.Contains("sord"))
            { return null; }
            
            try
            {
                rows = Convert.ToInt32(nvc["rows"]);
                page = Convert.ToInt32(nvc["page"]);
                sidx = nvc["sidx"];
                sord = nvc["sord"];
                try
                {
                    filters = new JSON().Deserialize<QueryStringFilter>(nvc["filters"]);
                }
                catch (ArgumentNullException) { filters = new QueryStringFilter { groupOp = "ALL", rules = new QueryStringFilterRule[0] }; }
                return this;
            }
            catch { return null; }
        }
    }
}
