using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class ArquivoVO:EntidadeBaseVO
	{
		#region atributos

		private String nome;

		#endregion

		#region construtores

		public ArquivoVO()
        {
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome do arquivo
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Descrição do Arquivo
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Nome Original do Arquivo
        /// </summary>
        public virtual String NomeOriginal { get; set; }

        /// <summary>
        /// Extensão do Arquivo
        /// </summary>
        public virtual String Extensao { get; set; }

        /// <summary>
        /// Tipo de Arquivo
        /// </summary>
        public virtual TipoArquivoVO Tipo { get; set; }

		#endregion

	}
}
