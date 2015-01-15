using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using IntranetBettaInformatica.DataAccess;
using System.Security.Cryptography;

namespace IntranetBettaInformatica.Business
{
    public class BaseBO<t>
    {
        #region propriedades

        public virtual Object Object { get; set; }
        
        public IQueryable<t> GetQueryLinq
        {
            get
            {
                return Db.Session.Query<t>();
            }
        }


        #endregion

        #region metodos de chamadas ao banco

        public virtual object Salvar()
        {
            try
            {
                if (Object.GetType().GetProperty("Id").GetValue(Object, null).ToInt32() > 0)
                {
                    new Db().Salvar(Object);
                    return Object;
                }
                else
                {
                    return new Db().Inserir(Object);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IniciaTransacao()
        {
            new Db().IniciaTransacao();
        }

        public void FinalizaTransacao(Boolean commit)
        {
            new Db().FinalizaTransacao(commit);
        }
        public virtual Boolean Delete()
        {
            new Db().Delete(Object);
            return true;
        }
        public virtual Boolean DeleteUpdate()
        {
            Object.GetType().GetProperty("Removido").SetValue(Object , true, null);
            new Db().Salvar(Object);
            return true;
        }
        public virtual t SelectById(Object id)
        {
            return new Db().SelectById<t>(id);
        }
        public virtual List<t> Select(List<ICriterion> restricoes, params String[] properties)
        {
            return new Db().Select<t>(restricoes, properties);
        }
        public virtual List<t> Select(params String[] properties)
        {
            return new Db().Select<t>(properties);
        }
        public virtual List<t> Select(IQuery query)
        {
            return new Db().Select<t>(query);
        }
        public virtual List<t> Select(IQueryable<t> query)
        {
            return new Db().Select<t>(query);
        }
        public virtual List<t> Select(int pagina, int tamanhoPagina, params String[] properties)
        {
            return new Db().Select<t>(pagina, tamanhoPagina, properties);
        }

        #endregion

    }
}
