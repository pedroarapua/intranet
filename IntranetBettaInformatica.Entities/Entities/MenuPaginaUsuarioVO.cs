using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class MenuPaginaUsuarioVO : EntidadeBaseVO
    {

        #region propriedades

        /// <summary>
        /// Pagina
        /// </summary>
        public virtual MenuPaginaVO MenuPagina { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public virtual UsuarioVO Usuario { get; set; }

        #endregion

        #region propriedades nao mapeadas

        /// <summary>
        /// Quantidade de acessos do usuário a pagina
        /// </summary>
        public virtual Int32 QtdAcessos { get; set; }

        #endregion

    }
}
