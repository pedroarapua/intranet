using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class PeriodoVO:EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public PeriodoVO()
        {
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome do Período
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

		#endregion
	}
}
