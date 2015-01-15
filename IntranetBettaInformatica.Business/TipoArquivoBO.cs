using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Expressions;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de TipoArquivoVO
    /// </summary>
    public class TipoArquivoBO:BaseBO<TipoArquivoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TipoArquivoVO
        /// </summary>
        /// <param name="user"></param>
        public TipoArquivoBO(TipoArquivoVO tipoArquivo)
        {
            base.Object = tipoArquivo;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public TipoArquivoBO() { }

        #endregion

    }
}
