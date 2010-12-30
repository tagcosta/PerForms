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
using System.Runtime.Remoting.Messaging;

namespace PerForms.Util
{
    public class Context
    {
        public object Retrieve(string key)
        {
            if (IsInWebContext())
            {
                return HttpContext.Current.Items[key];
            }
            else
            {
                return CallContext.GetData(key);
            }
        }

        public void Remove(string key)
        {
            if (IsInWebContext())
            {
                HttpContext.Current.Items.Remove(key);
            }
            else
            {
                CallContext.SetData(key, null);
            }
        }

        public void Store(string key, object item)
        {
            if (IsInWebContext())
            {
                if (ContainsKey(key))
                {
                    HttpContext.Current.Items[key] = item;
                }
                else
                {
                    HttpContext.Current.Items.Add(key, item);
                }
            }
            else
            {
                CallContext.SetData(key, item);
            }
        }

        public bool ContainsKey(string key)
        {
            if (IsInWebContext())
            {
                return HttpContext.Current.Items.Contains(key);
            }
            else
            {
                return CallContext.GetData(key) != null;
            }
        }

        public T Retrieve<T>(string key)
        {
            return (T)Retrieve(key);
        }

        public void Store<T>(string key, T item)
        {
            Store(key, (object)item);
        }

        private bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }
    }
}
