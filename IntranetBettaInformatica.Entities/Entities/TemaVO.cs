using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class TemaVO:EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public TemaVO()
        {
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome do Tema
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Descricao do Tema
        /// </summary>
        public virtual String Descricao { get; set; }

		#endregion


	}
}
