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

namespace PerForms.Filters
{
    public class FilterFormatter
    {
        public string Format(Filter filter, string originalQuery, out string totalRecordsQuery)
        {
            //{0}: sort column
            //{1}: sort column order
            //{2}: number of rows per page
            //{3}: original query without order by
            //{4}: where clause
            //{5}: page number
            string template = @"
            SELECT * FROM
            (	
	            SELECT
	            (
		            (
			            (ROW_NUMBER() OVER (ORDER BY [OriginalTable].[{0}] {1}) - 1) -- sort column and sort column order
			            / {2} -- number of rows per page
		            )
	            ) + 1 as [PageNumber], * FROM (
            -- Start Original Query
            -- Restriction: Original Query cannot have an ORDER BY clause
            {3}
            -- End Original Query
            ) [OriginalTable] WHERE ({4}) -- where clause
            ) [PagedTable] WHERE [PagedTable].[PageNumber] IN ({5}) -- page number
            ";

            string sortColumnOrder = filter.SortAscending ? "asc" : "desc";
            string whereClause = GetWhereClause(filter, "OriginalTable");

            totalRecordsQuery = "SELECT COUNT([OriginalTable].[" + filter.SortColumnName + "]) FROM (" + originalQuery + ") [OriginalTable] WHERE (" + whereClause + ")";
            return string.Format(template, filter.SortColumnName, sortColumnOrder, filter.RowsPerPage, originalQuery, whereClause, filter.Page);
        }

        private string GetWhereClause(Filter filter, string tableName)
        {
            if (filter.FilterRows.Count == 0) return "0 = 0";

            string whereClause = "(" + filter.FilterRows[0].FieldOperator.GetFilter(GetFullyQualifiedName(tableName, filter.FilterRows[0].FieldName), filter.FilterRows[0].FieldValue) + ")";
            for (int i = 1; i < filter.FilterRows.Count; i++)
            {
                string filterTemplate = filter.FilterOperator.GetTemplate();
                whereClause += string.Format(filterTemplate, filter.FilterRows[i].FieldOperator.GetFilter(GetFullyQualifiedName(tableName, filter.FilterRows[i].FieldName), filter.FilterRows[i].FieldValue));
            }
            return whereClause;
        }

        private string GetFullyQualifiedName(string tableName, string fieldName)
        {
            return "[" + tableName + "].[" + fieldName + "]";
        }
    }
}