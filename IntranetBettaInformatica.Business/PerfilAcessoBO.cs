using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de PerfilAcessoVO
    /// </summary>
    public class PerfilAcessoBO:BaseBO<PerfilAcessoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro PerfilAcessoVO
        /// </summary>
        /// <param name="user"></param>
        public PerfilAcessoBO(PerfilAcessoVO perfil)
        {
            base.Object = perfil;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public PerfilAcessoBO() { }

        #endregion

	}
}
