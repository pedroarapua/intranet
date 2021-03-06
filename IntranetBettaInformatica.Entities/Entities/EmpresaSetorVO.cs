﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class EmpresaSetorVO : EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public EmpresaSetorVO()
        {
            SetoresFilhos = new List<EmpresaSetorVO>();
        }

        #endregion

        #region propriedades

        /// <summary>
        /// Empresa do Setor
        /// </summary>
        public virtual EmpresaVO Empresa { get; set; }

        /// <summary>
        /// SetorPai do Setor
        /// </summary>
        public virtual EmpresaSetorVO SetorPai { get; set; }

        /// <summary>
        /// Nome do Setor
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Setores da EmpresaSetor
        /// </summary>
        public virtual IList<EmpresaSetorVO> SetoresFilhos { get; set; }

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
