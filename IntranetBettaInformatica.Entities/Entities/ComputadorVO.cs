﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class ComputadorVO:EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region construtores

		public ComputadorVO()
        {
        }

		#endregion

		#region propriedades

		/// <summary>
        /// Nome da maquina
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// IP da maquina
        /// </summary>
        public virtual String Ip { get; set; }

        /// <summary>
        /// Usuario que utiliza a maquina
        /// </summary>
        public virtual UsuarioVO Usuario { get; set; }

		#endregion

	}
}
