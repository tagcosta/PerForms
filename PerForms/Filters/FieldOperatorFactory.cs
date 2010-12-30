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
    public class FieldOperatorFactory
    {
        public IFieldOperator Build(string fieldOperator)
        {
            fieldOperator = fieldOperator.ToUpper();
            switch (fieldOperator)
            {
                case "EQ": return new FieldOperatorEqual();
                case "NE": return new FieldOperatorNotEqual();
                case "LT": return new FieldOperatorLess();
                case "LE": return new FieldOperatorLessOrEqual();
                case "GT": return new FieldOperatorGreater();
                case "GE": return new FieldOperatorGreaterOrEqual();
                case "BW": return new FieldOperatorBeginsWith();
                case "BN": return new FieldOperatorNotBeginsWith();
                case "IN": return new FieldOperatorIn();
                case "NI": return new FieldOperatorNotIn();
                case "EW": return new FieldOperatorEndsWith();
                case "EN": return new FieldOperatorNotEndsWith();
                case "CN": return new FieldOperatorContains();
                case "NC": return new FieldOperatorNotContains();
                default: return new FieldOperatorEqual();
            }
        }
    }
}
