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

namespace PerForms.JQGrid
{
    [Serializable]
    public class JQGrid
    {
        public class Row
        {
            public int id { get; set; }
            public List<string> cell { get; set; }

            public Row()
            {
                cell = new List<string>();
            }
        }

        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }
        public List<Row> rows { get; set; }

        public JQGrid()
        {
            rows = new List<Row>();
        }

        public void AddRows(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Row row = new Row();
                row.id = rows.Count + 1;
                row.cell = GetValues(dr);
                rows.Add(row);
            }
        }

        private List<string> GetValues(DataRow dr)
        {
            List<string> list = new List<string>();
            object[] values = dr.ItemArray;
            foreach (object value in values)
            {
                list.Add(value.ToString());
            }
            return list;
        }
    }
}