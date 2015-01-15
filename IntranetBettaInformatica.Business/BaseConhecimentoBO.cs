using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de BaseConhecimentoVO
    /// </summary>
    public class BaseConhecimentoBO:BaseBO<BaseConhecimentoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro BaseConhecimentoVO
        /// </summary>
        /// <param name="user"></param>
        public BaseConhecimentoBO(BaseConhecimentoVO conhecimento)
        {
            base.Object = conhecimento;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public BaseConhecimentoBO() { }

        #endregion
    }
}
