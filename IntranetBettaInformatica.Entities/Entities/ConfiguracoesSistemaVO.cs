using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class ConfiguracoesSistemaVO : EntidadeBaseVO
    {
        #region propriedade mapeadas

        /// <summary>
        /// Extensao da Imagem
        /// </summary>
        public virtual String ExtensaoImagem { get; set; }

        /// <summary>
        /// Descricao do Logo
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Servidor smtp para envio de emails
        /// </summary>
        public virtual String ServidoSmtp { get; set; }

        /// <summary>
        /// Login do Servidor Smtp
        /// </summary>
        public virtual String Login { get; set; }

        /// <summary>
        /// Senha do Servidor Smtp
        /// </summary>
        public virtual String Senha { get; set; }

        #endregion

        #region propriedades não mapeadas

        /// <summary>
        /// Caminho completo da imagem do sistema
        /// </summary>
        public virtual String CaminhoImagem { get { return ExtensaoImagem.IsNullOrEmpty() ? String.Empty : "ConfiguracoesSistema/" + Id + ExtensaoImagem; } }

        #endregion
    }
}
