using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade ExtensaoArquivoVO
    /// </summary>
	public static class ExtensaoArquivoCarga
    {
        #region atributos

        private static List<ExtensaoArquivoVO> lista = new List<ExtensaoArquivoVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ExtensaoArquivo
        /// </summary>
		public static List<ExtensaoArquivoVO> Lista
        {
            get
            {
                return lista;
            }
            set
            {
                lista = value;
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// Carrega a lista
        /// </summary>
        private static void PreencherLista()
        {
			#region extensão de fotos

            ExtensaoArquivoVO extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Fotos,
				Extensao = ".gif"
            };
            Lista.Add(extensao);

            extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Fotos,
				Extensao = ".jpg"
            };
            Lista.Add(extensao);

            extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Fotos,
				Extensao = ".jpeg"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Fotos,
				Extensao = ".png"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Fotos,
				Extensao = ".bmp"
            };
            Lista.Add(extensao);

			#endregion

			#region extensao de videos

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".mpeg"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".avi"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".rmvb"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".flv"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".wmv"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".mp4"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".mov"
            };
            Lista.Add(extensao);

			extensao = new ExtensaoArquivoVO()
            {
                TipoExtensao = ETipoExtensao.Videos,
				Extensao = ".swf"
            };
            Lista.Add(extensao);

			#endregion
		
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new ExtensaoArquivoBO(obj).Salvar());
        }


        #endregion
    }
}
