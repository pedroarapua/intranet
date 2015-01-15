using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.DataAccess;
using NHibernate;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de MenuPaginaUsuarioVO
    /// </summary>
    public class MenuPaginaUsuarioBO:BaseBO<MenuPaginaUsuarioVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro MenuPaginaUsuarioVO
        /// </summary>
        /// <param name="user"></param>
        public MenuPaginaUsuarioBO(MenuPaginaUsuarioVO menuPagina)
        {
            base.Object = menuPagina;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public MenuPaginaUsuarioBO() { }

        #endregion

        #region metodos

        public Boolean Inserir(MenuPaginaUsuarioVO m)
        {
            try
            {
                ISession session = Db.Session;
                session.Merge(m);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        #endregion
    }
}
