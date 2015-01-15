using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class VagaEmpregoCurriculoVO : EntidadeBaseVO
	{
		#region propriedades

		/// <summary>
        /// Nome do Indicado
        /// </summary>
        public virtual String Nome { get; set; }

        /// <summary>
        /// Extensão do Arquivo enviado
        /// </summary>
        public virtual String Extensao { get; set; }

		/// <summary>
		/// Nome original do arquivo
		/// </summary>
		public virtual String NomeOriginal { get; set; }

		/// <summary>
		/// Vaga de emprego do currículo
		/// </summary>
		public virtual VagaEmpregoVO VagaEmprego { get; set; }

		#endregion

	}
}
