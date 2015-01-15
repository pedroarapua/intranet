using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de MenuPaginaVO
    /// </summary>
    public class MenuPaginaBO:BaseBO<MenuPaginaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TemaVO
        /// </summary>
        /// <param name="user"></param>
        public MenuPaginaBO(MenuPaginaVO menuPagina)
        {
            base.Object = menuPagina;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public MenuPaginaBO() { }

        #endregion

        #region metodos

        public List<MenuPaginaVO> BuscarPaginasPai()
        {
            return base.Select(base.GetQueryLinq.Where(x => x.MenuPaginaPai == null && !x.Removido));
        }

		
        #endregion
    }
}
