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
using System.Web;

namespace PerForms.Util
{
    public class BrowserDetector
    {
        public enum EBrowser
        {
            Unknown = 0,
            IE6 = 1,
            IE7 = 2,
            IE8 = 3,
            Firefox = 4,
            Chrome = 5,
            Opera = 6,
            Safari = 7
        }

        public EBrowser DetectBrowser()
        {
            string browser = HttpContext.Current.Request.Browser.Browser;
            string type = HttpContext.Current.Request.Browser.Type;
            string userAgent = HttpContext.Current.Request.UserAgent;

            switch (browser)
            {
                case "IE":
                    switch (type)
                    {
                        case "IE6": return EBrowser.IE6;
                        case "IE7": return EBrowser.IE7;
                        case "IE8": return EBrowser.IE8;
                        default: return EBrowser.Unknown;
                    }
                case "AppleMAC-Safari":
                    if (userAgent.Contains("Chrome")) return EBrowser.Chrome;
                    else return EBrowser.Safari;
                case "Firefox": return EBrowser.Firefox;
                case "Opera": return EBrowser.Opera;
                default: return EBrowser.Unknown;
            }
        }
    }
}
