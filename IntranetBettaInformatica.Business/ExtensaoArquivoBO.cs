using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Expressions;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ExtensaoArquivoVO
    /// </summary>
    public class ExtensaoArquivoBO:BaseBO<ExtensaoArquivoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ExtensaoArquivoVO
        /// </summary>
        /// <param name="user"></param>
        public ExtensaoArquivoBO(ExtensaoArquivoVO extensaoArquivo)
        {
            base.Object = extensaoArquivo;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ExtensaoArquivoBO() { }

        #endregion

		#region metodos

		/// <summary>
		/// Valida existência de extensão para o tipo específico
		/// </summary>
		/// <param name="extensao"></param>
		/// <returns></returns>
		public Boolean ValidaExtensao(ExtensaoArquivoVO extensao)
		{
			return base.GetQueryLinq.Count(x => x.TipoExtensao == extensao.TipoExtensao && x.Extensao.ToUpper() == extensao.Extensao.ToUpper()) == 0;
		}

		#endregion

	}
}
