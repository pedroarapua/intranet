using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de AcaoVO
    /// </summary>
    public class AcaoBO:BaseBO<AcaoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro AcaoVO
        /// </summary>
        /// <param name="user"></param>
        public AcaoBO(AcaoVO acao)
        {
            base.Object = acao;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public AcaoBO() { }

        #endregion
    }
}
