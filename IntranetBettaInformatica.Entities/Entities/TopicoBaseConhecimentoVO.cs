using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class TopicoBaseConhecimentoVO : EntidadeBaseVO
	{
		#region construtores

		public TopicoBaseConhecimentoVO()
		{
			this.Conhecimentos = new List<BaseConhecimentoVO>();
		}

		#endregion

		#region atributos

		private String titulo;

		#endregion

		#region propriedades

		/// <summary>
        /// Titulo do Tópico
        /// </summary>
		public virtual String Titulo { get { return String.Format("{0} {1}", this.titulo, base.Removido.ToDescricaoRemovido(true)); } set { this.titulo = value; } }

		/// <summary>
		/// Lista de itens do topico do manual
		/// </summary>
		public virtual IList<BaseConhecimentoVO> Conhecimentos { get; set; }

		#endregion

	}
}
