using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class MenuPaginaVO : EntidadeBaseVO
    {

        #region contrutores

        public MenuPaginaVO()
        {
            MenuPaginas = new List<MenuPaginaVO>();
            this.Acoes = new List<AcaoVO>();
        }

        #endregion

        #region propriedades

        /// <summary>
        /// Descricao da Página
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Url do Menu da Pagina
        /// </summary>
        public virtual String Url { get; set; }

        /// <summary>
        /// Icone a ser mostrado no menu
        /// </summary>
        public virtual String Icone { get; set; }

        /// <summary>
        /// Pertence ao Menu
        /// </summary>
        public virtual Boolean EmMenu { get; set; }

        /// <summary>
        /// Ordem do Menu da Pagina
        /// </summary>
        public virtual Int32 Ordem { get; set; }

        /// <summary>
        /// Menu Pagina Pai
        /// </summary>
        public virtual MenuPaginaVO MenuPaginaPai { get; set; }

        /// <summary>
        /// Menus Paginas do Menu Pagina
        /// </summary>
        public virtual IList<MenuPaginaVO> MenuPaginas { get; set; }

        /// <summary>
		/// Lista de ações da página
		/// </summary>
		public virtual IList<AcaoVO> Acoes { get; set; }

        #endregion

        #region propriedades nao mapeadas

        /// <summary>
        /// Icone a ser mostrado no menu
        /// </summary>
        public virtual String IconeCls { get; set; }

        #endregion

        #region metodos

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
