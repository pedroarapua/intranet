using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class FuncaoVO:EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public FuncaoVO()
        {
			this.Usuarios = new List<UsuarioVO>();
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome da função
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Descrição da função
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Ordem que deve estar na hierarquia
        /// </summary>
        public virtual Int32 Ordem { get; set; }

		/// <summary>
		/// Usuários que possuem a função
		/// </summary>
		public virtual IList<UsuarioVO> Usuarios { get; set; }

		#endregion

	}
}
