using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de EstadoVO
    /// </summary>
    public class EstadoBO:BaseBO<EstadoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro EstadoVO
        /// </summary>
        /// <param name="user"></param>
        public EstadoBO(EstadoVO estado)
        {
            base.Object = estado;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public EstadoBO() { }

        #endregion
    }
}
