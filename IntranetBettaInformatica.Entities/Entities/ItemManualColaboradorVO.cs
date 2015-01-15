using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class ItemManualColaboradorVO : EntidadeBaseVO
	{

		#region propriedades

		/// <summary>
		/// Descrição 
		/// </summary>
		public virtual String Descricao { get; set; }

		/// <summary>
		/// Topico do Manual
		/// </summary>
		public virtual TopicoManualColaboradorVO Topico { get; set; }

		#endregion

	}
}
