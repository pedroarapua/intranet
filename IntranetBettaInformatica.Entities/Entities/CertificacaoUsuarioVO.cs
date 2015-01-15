using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class CertificacaoUsuarioVO : EntidadeBaseVO
	{
		#region propriedades

		/// <summary>
        /// Certificação
        /// </summary>
        public virtual String Certificacao { get; set; }

        /// <summary>
        /// Orgão da certificação
        /// </summary>
        public virtual String Orgao { get; set; }

		/// <summary>
		/// Usuário
		/// </summary>
		public virtual UsuarioVO Usuario { get; set; }

		#endregion

	}
}
