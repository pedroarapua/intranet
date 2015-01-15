using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using IntranetBettaInformatica.Entities.Entities;
using System.Security.Cryptography;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de UsuarioVO
    /// </summary>
    public class UsuarioBO:BaseBO<UsuarioVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro UsuarioVO
        /// </summary>
        /// <param name="user"></param>
        public UsuarioBO(UsuarioVO user)
        {
            base.Object = user;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public UsuarioBO() { }

        #endregion

        #region metodos

        public UsuarioVO Login(String usuario, String senha)
        {
            List<UsuarioVO> lst = base.Select(base.GetQueryLinq.Where(obj => obj.Login == usuario && obj.Senha == EncriptyPassword(senha) && obj.UsuarioSistema == true));
            return lst.FirstOrDefault();
        }

        public Boolean ValidarLogin(UsuarioVO usuario, Boolean emEdicao)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq.Where(x=> x.Login == usuario.Login);
            if (emEdicao)
            {
                query = query.Where(x => x.Id != usuario.Id);
            }

            return query.FirstOrDefault() == null;
        }

        public List<UsuarioVO> BuscarUsuarios(Boolean removido)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq.Where(x=> x.Removido == removido);
            return base.Select(query);
        }

        public List<UsuarioVO> BuscarAniversariantes(UsuarioVO usuarioLogado)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq.Where(x => x.Removido == false && x.DataNascimento.HasValue && x.DataNascimento.Value.Day == DateTime.Today.Day && x.DataNascimento.Value.Month == DateTime.Today.Month && x.Id != usuarioLogado.Id);
            //query = query.Fetch(x => x.Empresa).Fetch(x => x.Estado).FetchMany(x => x.MensagensEnviadas).FetchMany(x => x.MensagensRecebidas).FetchMany(x => x.Noticias).Fetch(x => x.PaginaInicial).FetchMany(x => x.Paginas).Fetch(x => x.PerfilAcesso);
            //query = query.FetchMany(x => x.Sistemas);
            return base.Select(query);
        }

        public List<UsuarioVO> BuscarPorDataNascimento(DateTime? dataIni, DateTime? dataFim)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq.Where(x => x.Removido == false && x.DataNascimento.HasValue);
            if(dataIni.HasValue && dataFim.HasValue)
                query = query.Where(x => ((x.DataNascimento.Value.Day >= dataIni.Value.Day && x.DataNascimento.Value.Month == dataIni.Value.Month) || (x.DataNascimento.Value.Month > dataIni.Value.Month)) && ((x.DataNascimento.Value.Day <= dataFim.Value.Day && x.DataNascimento.Value.Month == dataFim.Value.Month) || (x.DataNascimento.Value.Month < dataFim.Value.Month)));
            else if(dataIni.HasValue)
                query = query.Where(x => (x.DataNascimento.Value.Day >= dataIni.Value.Day && x.DataNascimento.Value.Month == dataIni.Value.Month) || (x.DataNascimento.Value.Month > dataIni.Value.Month));
            else if (dataFim.HasValue)
                query = query.Where(x => (x.DataNascimento.Value.Day <= dataFim.Value.Day && x.DataNascimento.Value.Month == dataFim.Value.Month) || (x.DataNascimento.Value.Month < dataFim.Value.Month));
            return base.Select(query).OrderByDescending(x=> x.DataNascimento).ToList();
        }

        public List<UsuarioVO> BuscarContatos(String nome, EmpresaVO empresa, Boolean? removido)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq;
            if (removido.HasValue)
            {
                query = query.Where(x => x.RemovidoContato == removido);
            }
            if (!nome.IsNullOrEmpty())
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper()));
            }
            if(empresa != null)
            {
                query = query.Where(x => x.Empresa.Id == empresa.Id);
            }
            return base.Select(query);
        }

        public List<UsuarioVO> BuscarUsuariosSistema(Boolean? removido, Boolean? usuarioSistema)
        {
            IQueryable<UsuarioVO> query = base.GetQueryLinq;
            if(removido.HasValue)
                query = query.Where(x => x.Removido == removido.GetValueOrDefault());
            if(usuarioSistema.HasValue)
                query = query.Where(x => x.UsuarioSistema == usuarioSistema.GetValueOrDefault());
            return base.Select(query);
        }

        /// <summary>
        /// metodo que remove o usuario da gerencia de contatos
        /// </summary>
        /// <returns></returns>
        public virtual Boolean DeleteContato(UsuarioVO usuario)
        {
            usuario.RemovidoContato = true;
            base.Object = usuario;
            try
            {
                base.Salvar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

		public UsuarioVO BuscarPorLogin(String login)
		{
			return base.GetQueryLinq.Where(x => x.Login.ToUpper() == login.ToUpper()).ToList().FirstOrDefault();
		}

        public static String EncriptyPassword(string password)
        {
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
            byte[] input = Encoding.Default.GetBytes(password);
            byte[] output = md5Hash.ComputeHash(input);
            return Encoding.Default.GetString(output);
        }

        #endregion
    }
}
