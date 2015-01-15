using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
	public class VagaEmpregoVO : EntidadeBaseVO
	{
		#region construtores

		public VagaEmpregoVO()
		{
			this.Curriculos = new List<VagaEmpregoCurriculoVO>();
		}

		#endregion

		#region propriedades

		/// <summary>
        /// Título da vaga
        /// </summary>
        public virtual String Titulo { get; set; }

        /// <summary>
        /// Descrição da vaga
        /// </summary>
        public virtual String Descricao { get; set; }

		/// <summary>
		/// Currículos associados a vaga de emprego
		/// </summary>
		public virtual IList<VagaEmpregoCurriculoVO> Curriculos { get; set; }

		/// <summary>
		/// Status da vaga de emprego
		/// </summary>
		public virtual EStatusVagaEmprego Status { get; set; }

		/// <summary>
		/// Data de Atualizacao da vaga
		/// </summary>
		public virtual DateTime DataAtualizacao { get; set; }

		#endregion

		#region propriedades nao mapeadas

		/// <summary>
		/// Codigo identificador da reunião
		/// </summary>
		public virtual String Codigo
		{
			get { return String.Format("VG{0}", Id.ToString().PadLeft(4, '0')); }
		}

		/// <summary>
		/// Quantidade de indicações da vaga
		/// </summary>
		public virtual Int32 QtdIndicacoes
		{
			get { return this.Curriculos.Count; }
		}

		#endregion

	}

	public enum EStatusVagaEmprego
	{
		Ativa = 1,
		Fechada = 2
	}
}
