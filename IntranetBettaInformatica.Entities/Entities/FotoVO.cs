using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class FotoVO:EntidadeBaseVO
    {

        public FotoVO()
        {
            Comentarios = new List<ComentarioFotoVO>();
        }

        /// <summary>
        /// Extensão da Foto
        /// </summary>
        public virtual String Extensao { get; set; }

        /// <summary>
        /// Título da Foto
        /// </summary>
        public virtual String Titulo { get; set; }

        /// <summary>
        /// Nome da Foto Original
        /// </summary>
        public virtual String NomeOriginal { get; set; }

        /// <summary>
        /// Capa do Album
        /// </summary>
        public virtual Boolean CapaAlbum { get; set; }

        /// <summary>
        /// Galeria que a foto pertence
        /// </summary>
        public virtual GaleriaVO Galeria { get; set; }

        /// <summary>
        /// Comentarios da foto
        /// </summary>
        public virtual IList<ComentarioFotoVO> Comentarios { get; set; }

        #region propriedades nao mapeadas

        /// <summary>
        /// Caminho completo da imagem thumb
        /// </summary>
        public virtual String CaminhoImagemThumb { get { return Extensao.IsNullOrEmpty() ? String.Empty : "GaleriaImagens/FotosThumbs/" + Id + Extensao; } }

        /// <summary>
        /// Caminho completo da imagem original
        /// </summary>
        public virtual String CaminhoImagemOriginal { get { return Extensao.IsNullOrEmpty() ? String.Empty : "GaleriaImagens/FotosOriginais/" + Id + Extensao; } }

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
