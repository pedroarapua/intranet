using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de TopicoManualColaboradorVO
    /// </summary>
    public class TopicoManualColaboradorBO:BaseBO<TopicoManualColaboradorVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TopicoManualColaboradorVO
        /// </summary>
        /// <param name="user"></param>
        public TopicoManualColaboradorBO(TopicoManualColaboradorVO topico)
        {
            base.Object = topico;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public TopicoManualColaboradorBO() { }

        #endregion
    }
}
