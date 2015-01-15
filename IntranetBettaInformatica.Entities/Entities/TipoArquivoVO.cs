using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class TipoArquivoVO : EntidadeBaseVO
	{
		
		#region atributos

		private String nome;

		#endregion

		#region construtores

		public TipoArquivoVO()
        {
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome do tipo de Arquivo
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

		#endregion

	}
}
