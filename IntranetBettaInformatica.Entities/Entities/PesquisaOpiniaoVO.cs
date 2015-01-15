using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class PesquisaOpiniaoVO:EntidadeBaseVO
    {

        public PesquisaOpiniaoVO()
        {
            this.Usuarios = new List<UsuarioVO>();
            this.Respostas = new List<RespostaVO>();
        }

        /// <summary>
        /// Pergunta
        /// </summary>
        public virtual String Pergunta { get; set; }

        /// <summary>
        /// Data Inicial
        /// </summary>
        public virtual DateTime DataInicial { get; set; }

        /// <summary>
        /// Data Final
        /// </summary>
        public virtual DateTime DataFinal { get; set; }

        /// <summary>
        /// Mostrar resultado
        /// </summary>
        public virtual Boolean MostrarResultado { get; set; }

        /// <summary>
        /// Usuários da Pesquisa
        /// </summary>
        public virtual IList<UsuarioVO> Usuarios { get; set; }

        /// <summary>
        /// Respostas da Pesquisa
        /// </summary>
        public virtual IList<RespostaVO> Respostas { get; set; }

        #region propriedades não mapeadas

        /// <summary>
        /// propriedade que retorna de a pesquisa está no período vigente
        /// </summary>
        public virtual StatusPesquisa Status { 
            get {
                if ((DataInicial >= DateTime.Now && DataFinal <= DateTime.Now) || (DataInicial < DateTime.Now && DataFinal > DateTime.Now))
                    return StatusPesquisa.Iniciada;
                else if (DataInicial > DateTime.Now)
                    return StatusPesquisa.Aguardando;
                else
                    return StatusPesquisa.Finalizada;                
            }
        }

        #endregion

    }

    public enum StatusPesquisa
    {
        Aguardando = 0,
        Iniciada = 1,
        Finalizada = 2
    }
}
