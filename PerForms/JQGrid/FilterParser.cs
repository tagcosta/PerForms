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
using PerForms.Filters;

namespace PerForms.JQGrid
{
    public class FilterParser
    {
        public Filter Parse(QueryStringInfo queryStringInfo)
        {
            Filter filter = new Filter
            {
                Page = queryStringInfo.page,
                RowsPerPage = queryStringInfo.rows,
                SortColumnName = queryStringInfo.sidx,
                SortAscending = queryStringInfo.sord.ToUpper().Equals("ASC")
            };
            filter.FilterOperator = new FilterOperatorFactory().Build(queryStringInfo.filters.groupOp);
            foreach (QueryStringFilterRule queryStringFilterRule in queryStringInfo.filters.rules)
            {
                filter.FilterRows.Add(new FilterRow
                {
                    FieldName = queryStringFilterRule.field,
                    FieldValue = queryStringFilterRule.data,
                    FieldOperator = new FieldOperatorFactory().Build(queryStringFilterRule.op)
                });
            }
            return filter;
        }
    }
}
