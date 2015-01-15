using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class NoticiaVO:EntidadeBaseVO
    {

        public NoticiaVO()
        {
            this.Usuarios = new List<UsuarioVO>();
        }

        /// <summary>
        /// Titulo da Noticia
        /// </summary>
        public virtual String Titulo { get; set; }

        /// <summary>
        /// HTML da Noticia
        /// </summary>
        public virtual String HTML { get; set; }

        /// <summary>
        /// Data Inicial
        /// </summary>
        public virtual DateTime DataInicial { get; set; }

        /// <summary>
        /// Data Final
        /// </summary>
        public virtual DateTime DataFinal { get; set; }

        /// <summary>
        /// Usuários que visualizam a Noticia
        /// </summary>
        public virtual IList<UsuarioVO> Usuarios { get; set; }

        #region propriedades não mapeadas

        /// <summary>
        /// propriedade que retorna se a notícia está no período vigente
        /// </summary>
        public virtual StatusNoticia Status { 
            get {
                if ((DataInicial >= DateTime.Now && DataFinal <= DateTime.Now) || (DataInicial < DateTime.Now && DataFinal > DateTime.Now))
                    return StatusNoticia.Iniciada;
                else if (DataInicial > DateTime.Now)
                    return StatusNoticia.Aguardando;
                else
                    return StatusNoticia.Finalizada;                
            }
        }

        #endregion

    }

    public enum StatusNoticia
    {
        Aguardando = 0,
        Iniciada = 1,
        Finalizada = 2
    }
}
