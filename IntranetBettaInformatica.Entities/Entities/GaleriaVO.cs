using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class GaleriaVO:EntidadeBaseVO
    {

        public GaleriaVO()
        {
            Fotos = new List<FotoVO>();
            Videos = new List<VideoVO>();
        }

        /// <summary>
        /// Nome da Galeria
        /// </summary>
        public virtual String Nome { get; set; }

        /// <summary>
        /// Descricao da Galeria
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Fotos da Galeria
        /// </summary>
        public virtual IList<FotoVO> Fotos { get; set; }

        /// <summary>
        /// Videos da Galeria
        /// </summary>
        public virtual IList<VideoVO> Videos { get; set; }

        /// <summary>
        /// Caminho da foto de capa do album da galeria
        /// </summary>
        public virtual String CapaAlbumThumb { 
            get {
                FotoVO fotoCapa = Fotos.FirstOrDefault(x => x.CapaAlbum = true);
                return fotoCapa != null ? fotoCapa.CaminhoImagemThumb : "";
            } 
        }

        /// <summary>
        /// Propriedade que retorna a quantidade de fotos no album
        /// </summary>
        public virtual Int32 QuantidadeFotos
        {
            get
            {
                return Fotos.Count(x=> x.Removido == false);
            }
        }

        /// <summary>
        /// Propriedade que retorna a quantidade de videos na galeria
        /// </summary>
        public virtual Int32 QuantidadeVideos
        {
            get
            {
                return Videos.Count(x => x.Removido == false);
            }
        }
        
    }
}
