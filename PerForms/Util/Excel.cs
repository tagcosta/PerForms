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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace PerForms.Util
{
    public class Excel
    {
        public byte[] GetExcelBufferFromDataTable(DataTable dt)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            Sheet sheet = hssfworkbook.CreateSheet("Sheet1");

            sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                sheet.CreateRow(i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sheet.GetRow(i).CreateCell(j).SetCellValue(dt.Rows[i - 1][dt.Columns[j].ColumnName].ToString());
                }
            }

            MemoryStream memstream = new MemoryStream();
            try
            {
                hssfworkbook.Write(memstream);

                byte[] buffer = memstream.GetBuffer();

                return buffer;
            }
            finally
            {
                memstream.Close();
            }
        }
    }
}