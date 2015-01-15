using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class BaseConhecimentoVO : EntidadeBaseVO
	{

		#region propriedades

		/// <summary>
		/// Descrição 
		/// </summary>
		public virtual String Titulo { get; set; }

		/// <summary>
		/// Topico da Base de Conhecimento
		/// </summary>
		public virtual TopicoBaseConhecimentoVO Topico { get; set; }

		/// <summary>
		/// Usuários que possuam este conhecimento
		/// </summary>
		public virtual IList<UsuarioBaseConhecimentoVO> UsuariosConhecimentos { get; set; }

		#endregion

	}
}
