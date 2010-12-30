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
using NHibernate;
using NHibernate.Cfg;
using System.Web;
using System.Runtime.Remoting.Messaging;
using PerForms.Config;
using PerForms.Util;

namespace PerForms.Logging
{
    public class NH_PERFORMS
    {
        public enum EDataBaseDialect { MSSQL2000 = 0 }

        private Context Context { get; set; }

        private const string SessionKey = "NH_PERFORMSSessionKey";
        private const string TransactionKey = "NH_PERFORMSTransactionKey";

        private ISessionFactory SessionFactory { get; set; }

        private static NH_PERFORMS instance;
        public static NH_PERFORMS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NH_PERFORMS();
                }
                return instance;
            }
        }

        private NH_PERFORMS()
        {
            SessionFactory = GetSessionFactory();
            Context = new Context();
        }

        private ISessionFactory GetSessionFactory()
        {
            if (PerFormsConfig.DBConfig.LogsActive)
            {
                Configuration cfg = new Configuration();
                cfg.Properties["adonet.batch_size"] = "10";
                cfg.Properties["show_sql"] = "false";
                cfg.Properties["use_outer_join"] = "true";
                cfg.Properties["command_timeout"] = "60";
                cfg.Properties["query.substitutions"] = "true 1, false 0, yes 'Y', no 'N'";
                cfg.Properties["proxyfactory.factory_class"] = "NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu";
                cfg.Properties["hbm2ddl.keywords"] = "auto-quote";

                switch (PerFormsConfig.DBConfig.LogsDataBaseDialect)
                {
                    case EDataBaseDialect.MSSQL2000:
                        cfg.Properties["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver";
                        cfg.Properties["dialect"] = "NHibernate.Dialect.MsSql2000Dialect";
                        break;
                }
                
                cfg.Properties["connection.connection_string"] = PerFormsConfig.DBConfig.LogsConnectionString;
                cfg.AddAssembly("PerForms");
                return cfg.BuildSessionFactory();
            }
            else return null;
        }

        public ISession GetSession()
        {
            if (!Context.ContainsKey(SessionKey))
            {
                Context.Store<ISession>(SessionKey, SessionFactory.OpenSession());   
            }
            return Context.Retrieve<ISession>(SessionKey);
        }

        public void Disconnect()
        {
            //If a transaction is running, rollback it to avoid leaving transactions open
            RollbackTransaction();
            if (Context.ContainsKey(SessionKey))
            {
                Context.Retrieve<ISession>(SessionKey).Disconnect();
                Context.Remove(SessionKey);
            }
        }

        public void StartTransaction()
        {
            if (!Context.ContainsKey(TransactionKey))
            {
                Context.Store<ITransaction>(TransactionKey, GetSession().BeginTransaction());
            }
        }

        public void CommitTransaction()
        {
            if (Context.ContainsKey(TransactionKey))
            {
                Context.Retrieve<ITransaction>(TransactionKey).Commit();
                Context.Remove(TransactionKey);
            }
        }

        public void RollbackTransaction()
        {
            if (Context.ContainsKey(TransactionKey))
            {
                Context.Retrieve<ITransaction>(TransactionKey).Rollback();
                Context.Remove(TransactionKey);
            }
        }

        public void Flush()
        {
            try
            {
                StartTransaction();
                GetSession().Flush();
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }
    }
}
