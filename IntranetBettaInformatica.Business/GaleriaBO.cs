using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de GaleriaVO
    /// </summary>
    public class GaleriaBO:BaseBO<GaleriaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro GaleriaVO
        /// </summary>
        /// <param name="user"></param>
        public GaleriaBO(GaleriaVO galeria)
        {
            base.Object = galeria;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public GaleriaBO() { }

        #endregion
    }
}
