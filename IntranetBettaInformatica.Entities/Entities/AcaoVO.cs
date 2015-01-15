using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Entities.Entities
{
	[Serializable]
	public class AcaoVO:EntidadeBaseVO
	{
		#region propriedades mapeadas

		/// <summary>
		/// Descrição da Ação
		/// </summary>
		public virtual String Descricao { get; set; }

		/// <summary>
		/// Enumerator de tipo da ação
		/// </summary>
		public virtual ETipoAcao TipoAcao { get { return (ETipoAcao)this.Id; } }

		/// <summary>
		/// Pagina da ação
		/// </summary>
		public virtual MenuPaginaVO Pagina { get; set; }

		#endregion

		#region propriedades não mapeadas

		public virtual Boolean Checked { get; set; }

		#endregion
	}
}
