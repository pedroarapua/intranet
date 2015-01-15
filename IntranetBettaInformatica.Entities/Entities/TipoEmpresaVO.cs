using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class TipoEmpresaVO:EntidadeBaseVO
	{

		#region atributos

		private String descricao;

		#endregion

		#region propriedades

		/// <summary>
        /// Descricao do Tema
        /// </summary>
		public virtual String Descricao { get { return String.Format("{0} {1}", this.descricao, base.Removido.ToDescricaoRemovido(true)); } set { this.descricao = value; } }

		#endregion

	}
}
