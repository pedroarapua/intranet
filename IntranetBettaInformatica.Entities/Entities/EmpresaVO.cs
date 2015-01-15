using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class EmpresaVO : EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public EmpresaVO()
        {
            this.Telefones = new List<EmpresaTelefoneVO>();
			this.Setores = new List<EmpresaSetorVO>();
		}

        #endregion

        #region propriedades

        /// <summary>
        /// Nome da Empresa
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Endereço da Empresa
        /// </summary>
        public virtual String Endereco { get; set; }

        /// <summary>
        /// Cidade da empresa
        /// </summary>
        public virtual String Cidade { get; set; }

        /// <summary>
        /// Estado da empresa
        /// </summary>
        public virtual EstadoVO Estado { get; set; }

        /// <summary>
        /// Email da Empresa
        /// </summary>
        public virtual String Email { get; set; }

        /// <summary>
        /// Site da Empresa
        /// </summary>
        public virtual String Site { get; set; }

        /// <summary>
        /// Tipo de Empresa
        /// </summary>
        public virtual TipoEmpresaVO TipoEmpresa { get; set; }

        /// <summary>
        /// Telefones da Empresa
        /// </summary>
        public virtual IList<EmpresaTelefoneVO> Telefones { get; set; }

        /// <summary>
        /// Setores da Empresa
        /// </summary>
        public virtual IList<EmpresaSetorVO> Setores { get; set; }

        /// <summary>
        /// Removido dos contatos
        /// </summary>
        public virtual Boolean RemovidoContato { get; set; }

        #endregion

        #region metodos

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

    }
}
