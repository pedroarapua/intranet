using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de VagaEmpregoCurriculoVO
    /// </summary>
    public class VagaEmpregoCurriculoBO:BaseBO<VagaEmpregoCurriculoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro curriculo
        /// </summary>
        /// <param name="user"></param>
        public VagaEmpregoCurriculoBO(VagaEmpregoCurriculoVO curriculo)
        {
            base.Object = curriculo;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public VagaEmpregoCurriculoBO() { }

        #endregion
    }
}
