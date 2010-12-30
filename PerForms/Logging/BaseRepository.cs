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

namespace PerForms.Logging
{
    public class BaseRepository : IBaseRepository
    {
        public virtual void Insert<T>(T item)
        {
            NH_PERFORMS.Instance.GetSession().Save(item);
        }

        public virtual void Delete<T>(T item)
        {
            NH_PERFORMS.Instance.GetSession().Delete(item);
        }

        public virtual void Update<T>(T item)
        {
            NH_PERFORMS.Instance.GetSession().Update(item);
        }

        public virtual void InsertOrUpdate<T>(T item)
        {
            NH_PERFORMS.Instance.GetSession().SaveOrUpdate(item);
        }

        public virtual IList<T> GetAll<T>()
        {
            return NH_PERFORMS.Instance.GetSession().CreateQuery("from " + typeof(T).ToString()).List<T>();
        }

        public virtual T GetById<T>(int id)
        {
            return NH_PERFORMS.Instance.GetSession().Get<T>(id);
        }

        public virtual IList<T> GetById<T>(IList<int> ids)
        {
            return GetById<T>(ids.ToArray<int>());
        }

        public virtual IList<T> GetById<T>(int[] ids)
        {
            if (ids.Length == 0) return new List<T>();

            return NH_PERFORMS.Instance.GetSession()
                .CreateQuery("from " + typeof(T).ToString() + " t where t.id in (:ids)")
                .SetParameterList("ids", ids)
                .List<T>();
        }

        public virtual T LoadById<T>(int id)
        {
            return NH_PERFORMS.Instance.GetSession().Load<T>(id);
        }
    }
}
