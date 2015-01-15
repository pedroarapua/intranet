using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de TopicoBaseConhecimentoVO
    /// </summary>
    public class TopicoBaseConhecimentoBO:BaseBO<TopicoBaseConhecimentoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TopicoBaseConhecimentoVO
        /// </summary>
        /// <param name="user"></param>
        public TopicoBaseConhecimentoBO(TopicoBaseConhecimentoVO topico)
        {
            base.Object = topico;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public TopicoBaseConhecimentoBO() { }

        #endregion
    }
}
