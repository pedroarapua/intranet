using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de VagaEmpregoVO
    /// </summary>
    public class VagaEmpregoBO:BaseBO<VagaEmpregoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro VagaEmpregoVO
        /// </summary>
        /// <param name="user"></param>
        public VagaEmpregoBO(VagaEmpregoVO vaga)
        {
            base.Object = vaga;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public VagaEmpregoBO() { }

        #endregion
    }
}
