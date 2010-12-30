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
using System.Xml.Serialization;
using PerForms.Util;

namespace PerForms.Actions
{
    /// <summary>
    /// <para>This class represents the object that is returned on each AJAX interaction.</para>
    /// <para>The purpose is to have a standard way of showing forms, messages, redirecting, etc.</para>
    /// <para>Note that you can execute more than one action in the same interaction.</para>
    /// </summary>
    [Serializable]
    public class AJAXActions
    {
        public List<AJAXAction> Actions { get; set; }

        public AJAXActions()
        {
            Actions = new List<AJAXAction>();
        }

        public AJAXActions AddShowMessageAction(string title, string content)
        {
            Actions.Add(new ShowMessageAction(title, content));
            return this;
        }

        public AJAXActions AddShowMessageAction(ShowMessageAction.EMessageType type, string title, string subtitle, string content)
        {
            Actions.Add(new ShowMessageAction(type, title, subtitle, content));
            return this;
        }

        public AJAXActions AddShowFormAction(string form)
        {
            Actions.Add(new ShowFormAction(form));
            return this;
        }

        public AJAXActions AddShowFormAction(string form, ShowFormAction.EPreviousFormActionType previousFormActionType)
        {
            Actions.Add(new ShowFormAction(form, previousFormActionType));
            return this;
        }

        public AJAXActions AddUpdateItemsAction(string fieldKey, List<KeyValue> keyValues)
        {
            Actions.Add(new UpdateItemsAction(fieldKey, keyValues));
            return this;
        }

        public AJAXActions AddRedirectToURLAction(string url)
        {
            Actions.Add(new RedirectToURLAction(url));
            return this;
        }

        public AJAXActions AddSetValueAction(string fieldKey, string value)
        {
            Actions.Add(new SetValuesAction(fieldKey, value));
            return this;
        }

        public AJAXActions AddSetValuesAction(string fieldKey, List<string> values)
        {
            Actions.Add(new SetValuesAction(fieldKey, values));
            return this;
        }
    }
}