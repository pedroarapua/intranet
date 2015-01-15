using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Expressions;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ArquivoVO
    /// </summary>
    public class ArquivoBO:BaseBO<ArquivoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ArquivoVO
        /// </summary>
        /// <param name="user"></param>
        public ArquivoBO(ArquivoVO arquivo)
        {
            base.Object = arquivo;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ArquivoBO() { }

        #endregion

    }
}
