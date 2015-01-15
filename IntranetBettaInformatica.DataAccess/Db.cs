using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using NHibernate.Cfg;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg;
using System.Configuration;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Criterion;
using System.IO;

namespace IntranetBettaInformatica.DataAccess
{
    public class Db
    {
        private static String connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        private static String assemblyName = ConfigurationSettings.AppSettings["AssemblyName"];
        private static ISessionFactory sessionFactory;
        private static ISession session;
        private static ITransaction transaction;

        public static String ConnectionString { get { return connectionString; } }
        public static String AssemblyName { get { return assemblyName; } }

        public static ISessionFactory SessionFactory
        {
            get {
                if (sessionFactory == null || sessionFactory.IsClosed)
                    sessionFactory = CreateSessionFactory();
                return sessionFactory;
            }
            set { sessionFactory = value;}
        }

        public static ISession Session
        {
            get 
            {
                if (session == null || !session.IsOpen)
                    session = SessionFactory.OpenSession();
                return session;            
            }
            set { session = value;}
        }

        public static ITransaction Transaction
        {
            get
            {
                return transaction;
            }
            set { transaction = value;}
        }
        
        public static Boolean ShowSql { 
            get {
                String showSql = ConfigurationSettings.AppSettings["ShowSql"];
                if(String.IsNullOrEmpty(showSql))
                    return false;
                return Convert.ToBoolean(showSql);
            } 
        }
        
        public static Boolean ExportSchema
        {
            get
            {
                String exportSchema = ConfigurationSettings.AppSettings["ExportSchema"];
                if (String.IsNullOrEmpty(exportSchema))
                    return false;
                return Convert.ToBoolean(exportSchema);
            }
        }

        #region contrutores

        public Db()
        {
            
        }

        #endregion

        #region metodos staticos

        private static ISessionFactory CreateSessionFactory()
        {
            try
            {
                //PostgreSQLConfiguration conf1 = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(Db.ConnectionString);
                MySQLConfiguration conf1 = MySQLConfiguration.Standard.ConnectionString(Db.ConnectionString);
                if(ShowSql)
                {
                    conf1 = conf1.ShowSql();
                    log4net.Config.XmlConfigurator.Configure();
                }
					return Fluently
						.Configure()
						.Database(conf1)
						.Mappings(m =>
							m.FluentMappings.AddFromAssembly(Assembly.Load(AssemblyName))
						)
						.ExposeConfiguration(BuildSchema)
						//.ExposeConfiguration(c =>
						//{
						//    c.SetProperty("cache.provider_class", "NHibernate.Cache.HashtableCacheProvider");
						//    c.SetProperty("dialect", "NHibernate.Dialect.MySQLDialect");
						//    c.SetProperty("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
						//    c.SetProperty("cache.use_second_level_cache", "false");
						//    c.SetProperty("cache.use_query_cache", "false");
						//})
						.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static ISession OpenSession(ISessionFactory sessionFactory)
        {
            return SessionFactory.OpenSession();
        }

        public void IniciaTransacao()
        {
            Session.FlushMode = FlushMode.Commit;
            Transaction = Session.BeginTransaction();
        }

        public void FinalizaTransacao(Boolean commit)
        {
            if(commit)
                Transaction.Commit();
            else
                Transaction.Rollback();
            Transaction.Dispose();
        }

        private static void BuildSchema(Configuration config)
        {
            if (Db.ExportSchema)
                new SchemaExport(config)
					.SetOutputFile(String.Format("{0}{1}.sql", Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName) + "/Arquitetura/2.0/Scripts/", "script_criacao"))
                    .Create(true, false);
        }

        #endregion

        #region metodos publicos

        public void Salvar(object obj)
        {
            try
            {
                Session.Clear();
                Session.Merge(obj);
                
                if (Transaction == null || !Transaction.IsActive)
                {
                    Session.Flush();
                }
                
            }
            catch (Exception)
            {
                session.Close();
                throw;
            }
        }

        public object Inserir(object obj)
        {
            try
            {
                Session.Clear();
                obj = Session.Merge(obj);
               
                if (Transaction == null || !Transaction.IsActive)
                {
                    Session.Flush();
                }
            }
            catch (Exception)
            {
                session.Close();
                throw;
            }
            return obj;
        }

        public void Delete(object obj)
        {
            try
            {
                Session.Clear();
                Session.Delete(obj);
                if (Transaction == null || !Transaction.IsActive)
                {
                    Session.Flush();
                }
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public t SelectById<t>(object id)
        {
            try
            {
                return Session.Get<t>(Convert.ToInt32(id));
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public List<t> Select<t>(List<ICriterion> restricoes, params String[] properties)
        {
            try
            {
                ICriteria q = Session.CreateCriteria(typeof(t));
                
                foreach (ICriterion c in restricoes)
                    q.Add(c);

                if (properties.Count() > 0)
                {
                    foreach (String prop in properties)
                    {
                        q.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty(prop), prop));
                    }
                    return q.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(t))).List<t>().ToList<t>();
                }
                return q.List<t>().ToList<t>();                
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public List<t> Select<t>(params String[] properties)
        {
            try
            {
                ICriteria q = Session.CreateCriteria(typeof(t));
                if (properties.Count() > 0)
                {
                    foreach (String prop in properties)
                    {
                        q.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty(prop), prop));
                    }
                    return q.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(t))).List<t>().ToList<t>();
                }
                return q.List<t>().ToList<t>();
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public List<t> Select<t>(int pagina, int tamanhoPagina, params String[] properties)
        {
            try
            {
                ICriteria q = Session.CreateCriteria(typeof(t)).SetFirstResult(pagina * tamanhoPagina).SetMaxResults(tamanhoPagina);
                int total = Session.CreateCriteria(typeof(t)).SetProjection(Projections.RowCount()).UniqueResult<int>();
                if (properties.Count() > 0)
                {
                    foreach (String prop in properties)
                    {
                        q.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty(prop), prop));
                    }
                    return q.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(t))).List<t>().ToList<t>();
                }
                return q.List<t>().ToList<t>();
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public List<t> Select<t>(IQuery query)
        {
            try
            {
                return query.List<t>().ToList<t>();
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }
        public List<t> Select<t>(IQueryable<t> query)
        {
            try
            {
                return query.ToList<t>();
            }
            catch (Exception)
            {
                Session.Close();
                throw;
            }
        }

        public static Boolean ExportDataBase()
        {
            CreateSessionFactory();
            return true;
        }

        #endregion

        

    }
}
