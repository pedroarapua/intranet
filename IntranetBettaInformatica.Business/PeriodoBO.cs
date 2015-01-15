using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de PeriodoVO
    /// </summary>
    public class PeriodoBO:BaseBO<PeriodoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro PeriodoVO
        /// </summary>
        /// <param name="user"></param>
        public PeriodoBO(PeriodoVO periodo)
        {
            base.Object = periodo;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public PeriodoBO() { }

        #endregion
    }
}
