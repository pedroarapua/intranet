using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class SalaVO:EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region propriedades

		/// <summary>
        /// nome da sala
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// setor em que a sala se encontra
        /// </summary>
        public virtual EmpresaSetorVO Setor { get; set; }

        #endregion
    }
}
