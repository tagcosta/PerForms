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
    public class FieldValueEscaper
    {
        public string Escape(string input)
        {
            return Escape(input, true);
        }

        public string Escape(string input, bool includeQuotes)
        {
            bool? inputBool = GetAsBool(input);
            if (inputBool.HasValue)
            {
                if (inputBool.Value) return "1";
                else return "0";
            }

            int? inputInt = GetAsInt(input);
            if (inputInt.HasValue)
            {
                return inputInt.Value.ToString();
            }

            double? inputDouble = GetAsDouble(input);
            if (inputDouble.HasValue)
            {
                return inputDouble.Value.ToString();
            }

            DateTime? inputDateTime = GetAsDateTime(input);
            if (inputDateTime.HasValue)
            {
                if (inputDateTime.Value.Hour == 0 && inputDateTime.Value.Minute == 0 && inputDateTime.Value.Second == 0)
                {
                    return "CONVERT(DATETIME, '" + inputDateTime.Value.ToString("dd-MM-yyyy") + "', 105)";
                }
                else
                {
                    return "CONVERT(DATETIME, '" + inputDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', 20)";
                }
            }

            string inputEscaped = input.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]");

            if (includeQuotes) return "'" + inputEscaped + "'";
            else return inputEscaped;
        }

        private bool? GetAsBool(string input)
        {
            try
            {
                return Convert.ToBoolean(input);
            }
            catch { return null; }
        }

        private int? GetAsInt(string input)
        {
            try
            {
                return Convert.ToInt32(input);
            }
            catch { return null; }
        }

        private double? GetAsDouble(string input)
        {
            try
            {
                return Convert.ToDouble(input.Replace(',', '.'));
            }
            catch { return null; }
        }

        private DateTime? GetAsDateTime(string input)
        {
            try
            {
                return Convert.ToDateTime(input);
            }
            catch { return null; }
        }
    }
}
