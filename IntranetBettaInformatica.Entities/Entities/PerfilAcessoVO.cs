using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class PerfilAcessoVO : EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region construtores

		public PerfilAcessoVO() {
			this.Acoes = new List<AcaoVO>();
        }

        #endregion

        #region propriedades

        /// <summary>
        /// Nome do Perfil de Acesso
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Descricao do Perfil de Acesso
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Perfil do moderador?
        /// </summary>
        public virtual Boolean EModerador { get; set; }

        /// <summary>
		/// Ações nas páginas do perfil de acesso
		/// </summary>
		public virtual IList<AcaoVO> Acoes { get; set; }

        #endregion

		#region

		/// <summary>
		/// Menu paginas do perfil de acesso
		/// </summary>
		public virtual IList<MenuPaginaVO> MenuPaginas { get { return this.Acoes.Where(x=> x.Pagina != null).Select(x=> x.Pagina).Distinct(new KeyEqualityComparer<MenuPaginaVO>(x => x.Id)).ToList(); } }

		#endregion
	}
}
