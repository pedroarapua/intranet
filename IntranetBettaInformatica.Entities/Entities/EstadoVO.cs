using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class EstadoVO : EntidadeBaseVO
	{
		#region propriedades

		/// <summary>
        /// Nome do Estado
        /// </summary>
        public virtual String Nome { get; set; }

        /// <summary>
        /// Sigla do Estado
        /// </summary>
        public virtual String Sigla { get; set; }

		#endregion

	}
}
