using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de TemaVO
    /// </summary>
    public class TemaBO:BaseBO<TemaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TemaVO
        /// </summary>
        /// <param name="user"></param>
        public TemaBO(TemaVO tema)
        {
            base.Object = tema;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public TemaBO() { }

        #endregion
    }
}
