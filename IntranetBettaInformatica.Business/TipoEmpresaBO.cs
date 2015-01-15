using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de TipoEmpresaVO
    /// </summary>
    public class TipoEmpresaBO:BaseBO<TipoEmpresaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TipoEmpresaVO
        /// </summary>
        /// <param name="user"></param>
        public TipoEmpresaBO(TipoEmpresaVO tipoEmpresa)
        {
            base.Object = tipoEmpresa;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public TipoEmpresaBO() { }

        #endregion
    }
}
