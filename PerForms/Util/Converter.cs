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

namespace PerForms.Util
{
    public class Converter
    {
        public string ToStringDelimitedString(List<char> chars, string delimiter)
        {
            string output = "";
            foreach (Char c in chars)
            {
                output += c + delimiter;
            }

            if (output == "") return "";
            return output.Substring(0, output.Length - delimiter.Length);
        }

        public char[] ToCharArray(string str, string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) return str.ToCharArray();

            string[] strs = str.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            str = "";
            foreach (string s in strs)
            {
                str += s;
            }
            return str.ToCharArray();
        }

        public DataTable ToDataTable(List<KeyValue> keyValueList, string keyColumnName, string valueColumnName)
        {
            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(keyColumnName);
            dataTable.Columns.Add(valueColumnName);
            foreach (KeyValue keyValue in keyValueList)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[keyColumnName] = keyValue.Key;
                dataRow[valueColumnName] = keyValue.Value;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public List<KeyValue> ToKeyValueList(DataTable dataTable, string keyColumnName, string valueColumnName)
        {
            List<KeyValue> keyValueList = new List<KeyValue>();
            foreach (DataRow dr in dataTable.Rows)
            {
                keyValueList.Add(new KeyValue(dr[keyColumnName].ToString(), dr[valueColumnName].ToString()));
            }
            return keyValueList;
        }
    }
}
