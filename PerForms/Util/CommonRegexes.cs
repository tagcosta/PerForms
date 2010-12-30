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

namespace PerForms.Util
{
    public class CommonRegexes
    {
        public string MaxChars(int maxChars)
        {
            return "^([\\s\\S]){0," + maxChars + "}$";
        }

        public string MinChars(int minChars)
        {
            return "^([\\s\\S]){" + minChars + ",}$";
        }

        public string MinAndMaxChars(int minChars, int maxChars)
        {
            return "^([\\s\\S]){" + minChars + "," + maxChars + "}$";
        }

        public string OnlyNumeric()
        {
            return "^([\\d])*$";
        }

        public string OnlyNumericAnd(params char[] allowedChars)
        {
            string str = "";
            List<char> mustEscapeChars = GetCharacteresThatNeedEscaping();
            foreach (char allowedChar in allowedChars)
            {
                str += mustEscapeChars.Contains(allowedChar) ? "\\" + allowedChar : allowedChar.ToString();
            }
            return "^([\\d" + str + "])*$";
        }

        public string OnlyNumericAnd(string allowedChars)
        {
            return OnlyNumericAnd(new Converter().ToCharArray(allowedChars, null));
        }

        public string OnlyAlphaNumeric()
        {
            return "^([\\da-zA-ZÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜáíóúñÑªºãõ'\\. ])*$";
        }

        public string OnlyAlphaNumericAnd(params char[] allowedChars)
        {
            string str = "";
            List<char> mustEscapeChars = GetCharacteresThatNeedEscaping();
            foreach (char allowedChar in allowedChars)
            {
                str += mustEscapeChars.Contains(allowedChar) ? "\\" + allowedChar : allowedChar.ToString();
            }
            return "^([\\da-zA-ZÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜáíóúñÑªºãõ'\\. ]" + str + "])*$";
        }

        public string OnlyAlphaNumericAnd(string allowedChars)
        {
            return OnlyAlphaNumericAnd(new Converter().ToCharArray(allowedChars, null));
        }

        private List<char> GetCharacteresThatNeedEscaping()
        {
            List<char> chars = new List<char>();
            chars.Add('.');
            chars.Add('*');
            chars.Add('+');
            chars.Add('?');
            chars.Add('|');
            chars.Add('(');
            chars.Add(')');
            chars.Add('[');
            chars.Add(']');
            chars.Add('{');
            chars.Add('}');
            chars.Add('\\');
            chars.Add('$');
            chars.Add('^');
            chars.Add('-');
            chars.Add(',');
            chars.Add('#');
            return chars;
        }
    }
}
