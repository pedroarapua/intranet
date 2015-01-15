using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class UsuarioBaseConhecimentoVO : EntidadeBaseVO
	{
		#region propriedades

		/// <summary>
        /// Conhecimento do Usuário
        /// </summary>
        public virtual BaseConhecimentoVO Conhecimento { get; set; }

		/// <summary>
		/// Usuário
		/// </summary>
		public virtual UsuarioVO Usuario { get; set; }

        /// <summary>
        /// Comprovável
        /// </summary>
        public virtual Boolean Comprovavel { get; set; }

		/// <summary>
		/// Nivel de conhecimento
		/// </summary>
		public virtual ENivelConhecimento NivelConhecimento { get; set; }

		#endregion

	}

	/// <summary>
	/// Enumerator com o nivel de conhecimento do usuario
	/// </summary>
	public enum ENivelConhecimento
	{
		Nenhum = 1,
		Basico = 2,
		Intermediario = 3,
		Avancado = 4
	}
}
