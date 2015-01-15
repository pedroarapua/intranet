using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FluentNHibernate.MappingModel;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class UsuarioVO : EntidadeBaseVO
	{

		#region atributos

		private String nome;

		#endregion

		#region contrutores

		public UsuarioVO()
        {
            this.Sistemas = new List<SistemaVO>();
            this.Telefones = new List<UsuarioTelefoneVO>();
            this.MensagensEnviadas = new List<MensagemVO>();
            this.MensagensRecebidas = new List<UsuarioMensagemVO>();
            this.MensagensRecebidasNaoLidas = new List<UsuarioMensagemVO>();
            this.PontosUsuario = new List<PontoUsuarioVO>();
            this.Noticias = new List<NoticiaVO>();
            this.Pesquisas = new List<PesquisaOpiniaoVO>();
            this.Paginas = new List<MenuPaginaUsuarioVO>();
			this.Reunioes = new List<ReuniaoVO>();
		}

        #endregion

        #region propriedades mapeadas

        /// <summary>
        /// Nome do usuario
        /// </summary>
		public virtual String Nome { get { return String.Format("{0} {1}", this.nome, base.Removido.ToDescricaoRemovido(true)); } set { this.nome = value; } }

        /// <summary>
        /// Login do usuario
        /// </summary>
        public virtual String Login { get; set; }

        /// <summary>
        /// Senha do usuario
        /// </summary>
        public virtual String Senha { get; set; }

        /// <summary>
        /// E usuario do sistema
        /// </summary>
        public virtual Boolean UsuarioSistema { get; set; }

        /// <summary>
        /// Data de Nascimento do usuario
        /// </summary>
        public virtual DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Cidade do usuario
        /// </summary>
        public virtual String Cidade { get; set; }

        /// <summary>
        /// Extensao da Foto do usuario
        /// </summary>
        public virtual String ExtensaoFoto { get; set; }

        /// <summary>
        /// Endereco do usuario
        /// </summary>
        public virtual String Endereco { get; set; }

        /// <summary>
        /// Email do usuario
        /// </summary>
        public virtual String Email { get; set; }

        /// <summary>
        /// Twitter do usuário
        /// </summary>
        public virtual String Twitter { get; set; }

        /// <summary>
        /// Palavra chave da senha do usuario
        /// </summary>
        public virtual String PalavraChave { get; set; }

        /// <summary>
        /// Estado do usuario
        /// </summary>
        public virtual EstadoVO Estado { get; set; }

        /// <summary>
        /// Empresa do usuario
        /// </summary>
        public virtual EmpresaVO Empresa { get; set; }

        /// <summary>
        /// Setor do usuario
        /// </summary>
        public virtual EmpresaSetorVO Setor { get; set; }

        /// <summary>
        /// Tema a ser utilizado pelo usuario
        /// </summary>
        public virtual TemaVO Tema { get; set; }

		/// <summary>
		/// Função do usuário na empresa
		/// </summary>
		public virtual FuncaoVO Funcao { get; set; }

        /// <summary>
        /// Sistemas que o usuario esta cadastrado
        /// </summary>
        public virtual IList<SistemaVO> Sistemas { get; set; }

        /// <summary>
        /// Perfi de Acesso do usuario
        /// </summary>
        public virtual PerfilAcessoVO PerfilAcesso { get; set; }

        /// <summary>
        /// Pagina inicial do usuario
        /// </summary>
        public virtual MenuPaginaVO PaginaInicial { get; set; }

        /// <summary>
        /// Telefones do Usuario
        /// </summary>
        public virtual IList<UsuarioTelefoneVO> Telefones { get; set; }

        /// <summary>
        /// Mensagens enviadas pelo usuário
        /// </summary>
        public virtual IList<MensagemVO> MensagensEnviadas { get; set; }

        /// <summary>
        /// Mensagens recebidas pelo usuário
        /// </summary>
        public virtual IList<UsuarioMensagemVO> MensagensRecebidas { get; set; }

        /// <summary>
        /// Mensagens recebidas pelo não lidas
        /// </summary>
        public virtual IList<UsuarioMensagemVO> MensagensRecebidasNaoLidas { get; set; }

        /// <summary>
        /// Pontos registrados do usuario do dia
        /// </summary>
        public virtual IList<PontoUsuarioVO> PontosUsuario { get; set; }

        /// <summary>
        /// Notícias do usuario
        /// </summary>
        public virtual IList<NoticiaVO> Noticias { get; set; }

        /// <summary>
        /// Pesquisas do usuario
        /// </summary>
        public virtual IList<PesquisaOpiniaoVO> Pesquisas { get; set; }

        /// <summary>
        /// Paginas dos favoritos do usuario
        /// </summary>
        public virtual IList<MenuPaginaUsuarioVO> Paginas { get; set; }

		/// <summary>
        /// Removido dos contatos
        /// </summary>
        public virtual Boolean RemovidoContato { get; set; }

        #endregion

        #region propriedades não mapeadas

        /// <summary>
        /// Caminho completo da imagem original do usuario
        /// </summary>
        public virtual String CaminhoImagemOriginal { get { return ExtensaoFoto.IsNullOrEmpty() ? "FotosUsuarios/FotosOriginais/user.jpg" : "FotosUsuarios/FotosOriginais/" + Id + ExtensaoFoto; } }

        /// <summary>
        /// Caminho completo da imagem thumbs do usuario
        /// </summary>
        public virtual String CaminhoImagemThumbs { get { return ExtensaoFoto.IsNullOrEmpty() ? "FotosUsuarios/FotosThumbs/user.jpg" : "FotosUsuarios/FotosThumbs/" + Id + ExtensaoFoto; } }

        #endregion

		#region propriedades nao mapeadas

		/// <summary>
		/// Reuniões do usuario no dia
		/// </summary>
		public virtual List<ReuniaoVO> Reunioes { get; set; }

		#endregion


	}
}
