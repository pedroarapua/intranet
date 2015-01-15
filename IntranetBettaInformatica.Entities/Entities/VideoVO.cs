using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class VideoVO : EntidadeBaseVO
    {

        public VideoVO()
        {
            Comentarios = new List<ComentarioVideoVO>();
        }

        /// <summary>
        /// Extensão do Video
        /// </summary>
        public virtual String Extensao { get; set; }

        /// <summary>
        /// Título do Video
        /// </summary>
        public virtual String Titulo { get; set; }

        /// <summary>
        /// Nome do Video Original
        /// </summary>
        public virtual String NomeOriginal { get; set; }

        /// <summary>
        /// Galeria que o video pertence
        /// </summary>
        public virtual GaleriaVO Galeria { get; set; }

        /// <summary>
        /// Comentarios do video
        /// </summary>
        public virtual IList<ComentarioVideoVO> Comentarios { get; set; }

        #region propriedades nao mapeadas

        /// <summary>
        /// Caminho completo do video original
        /// </summary>
        public virtual String CaminhoVideoOriginal { get { return Extensao.IsNullOrEmpty() ? String.Empty : "GaleriaVideos/" + Id + Extensao; } }

        /// <summary>
        /// Título texto curto
        /// </summary>
        public virtual String TituloTruncado { 
            get {
                String tituloTrunc = Titulo.Length < 30 ? Titulo : Titulo.Substring(0,30);
                tituloTrunc += Titulo.Length > 30 ? "..." : "";
                return tituloTrunc;
            }
        }

        #endregion

    }
}
