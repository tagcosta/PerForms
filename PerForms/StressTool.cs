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
using System.Web.UI;

namespace PerForms
{
    /// <summary>
    /// <para>Class created to call an ajax method x number of times synchronously or asynchronously.</para>
    /// </summary>
    public class StressTool
    {
        private int Number;
        public delegate void Action();
        private event Action ActionEvent;

        private string[] Keys;
        private string[] Values;
        private string ActionKey;
        private string MethodName;

        public StressTool(int number, Action action)
        {
            Number = number;
            ActionEvent = action;
        }

        public StressTool(int number)
        {
            Number = number;
            ActionEvent = delegate { };
        }

        public StressTool Prepare(WebServiceInterface service, string actionKey, string keysString, string valuesString)
        {
            MethodName = "GetAJAXActions";
            ActionKey = actionKey;
            GetKeyValues(keysString, valuesString, out Keys, out Values);

            ActionEvent = delegate
            {
                service.GetAJAXActions(ActionKey, Keys, Values);
            };
            return this;
        }

        public void GetKeyValues(string keysString, string valuesString, out string[] keys, out string[] values)
        {
            keys = keysString.Split(';');
            values = valuesString.Split(';');
        }

        public string GetKeyValues(string[] keys, string[] values)
        {
            string json = "[ ";
            for (int i = 0; i < keys.Length; i++)
            {
                json += "{ Key: '" + keys[i] + "', Value: '" + values[i] + "' },";
            }
            json = json.TrimEnd(',') + " ]";
            return json;
        }

        public void RunSync()
        {
            for (int i = 0; i < Number; i++)
            {
                ActionEvent();
            }
        }

        public void RunAsync(Page page)
        {
            string keyValues = GetKeyValues(Keys, Values);

            string html = @"
            <script type='text/javascript'>
                $(function () {
                    for(var i = 0; i < " + Number + @"; i++) {
                        var ajax = new perForms_AJAX('" + MethodName + @"');
                        ajax.addKeyValues(" + keyValues + @");
                        ajax.add('actionKey', '" + ActionKey + @"');
                        ajax.request(function (x) { });
                    }
                });
            </script>
            ";
            page.Form.Controls.Add(new LiteralControl(html));
        }
    }
}