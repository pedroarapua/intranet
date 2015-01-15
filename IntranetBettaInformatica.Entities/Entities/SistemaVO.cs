using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class SistemaVO : EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region propriedades

		/// <summary>
        /// Nome do Sistema
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Url do Sistema
        /// </summary>
        public virtual String Url { get; set; }

        /// <summary>
        /// Extensao da imagem do logo do Sistema
        /// </summary>
        public virtual String ExtensaoImagem { get; set; }

        /// <summary>
        /// Caminho completo da imagem do sistema
        /// </summary>
        public virtual String CaminhoImagem { get { return ExtensaoImagem.IsNullOrEmpty() ? String.Empty : "Sistemas/" + Id + ExtensaoImagem; } }

        #endregion
    }
}
