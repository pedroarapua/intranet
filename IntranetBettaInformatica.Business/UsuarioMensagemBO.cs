using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de UsuarioMensagemVO
    /// </summary>
    public class UsuarioMensagemBO:BaseBO<UsuarioMensagemVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro MensagemVO
        /// </summary>
        /// <param name="user"></param>
        public UsuarioMensagemBO(UsuarioMensagemVO usuarioM)
        {
            base.Object = usuarioM;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public UsuarioMensagemBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// busca as mensagem recebidas do usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<UsuarioMensagemVO> BuscarMensagensRecebidas(UsuarioVO usuario, Boolean? msgLidas, DateTime? dataIni, DateTime? dataFim)
        {
            IQueryable<UsuarioMensagemVO> query = base.GetQueryLinq.Where(x => x.Removido == false && x.Mensagem.Removido == false && x.UsuarioRecMens.Id == usuario.Id);
            if(dataFim.HasValue)
                dataFim = dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (dataIni.HasValue && dataFim.HasValue)
            {
                query = query.Where(x => x.Mensagem.Data >= dataIni && x.Mensagem.Data <= dataFim);
            }
            else if (dataIni.HasValue)
            {
                query = query.Where(x => x.Mensagem.Data >= dataIni);
            }
            else if (dataFim.HasValue)
            {
                query = query.Where(x => x.Mensagem.Data <= dataFim);
            }

            if (msgLidas.HasValue)
            {
                query = query.Where(x => x.LidoMensagem == msgLidas.GetValueOrDefault());
            }

            return base.Select(query).OrderByDescending(x => x.Mensagem.Data).ToList();
        }

        /// <summary>
        /// busca uma mensagem do usuario recebida
        /// </summary>
        /// <param name="codigoMensagem"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public UsuarioMensagemVO BuscarPorMensagemUsuario(Int32 codigoMensagem, UsuarioVO usuario)
        {
            return base.Select(base.GetQueryLinq.Where(x => x.UsuarioRecMens.Id == usuario.Id && x.Removido == false && x.Mensagem.Id == codigoMensagem))[0];
        }

        /// <summary>
        /// busca mensagens recebidas pelo usuario passando como parametnro uma lista de mensagens e o usuario logado
        /// </summary>
        /// <param name="mensagens"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<UsuarioMensagemVO> BuscarPorMensagensUsuario(List<MensagemVO> mensagens, UsuarioVO usuario)
        {
            IQueryable<UsuarioMensagemVO> query = base.GetQueryLinq.Where(x => x.UsuarioRecMens.Id == usuario.Id && x.Removido == false);
            query = query.AsEnumerable().Where(x => mensagens.Any(x1 => x1.Id == x.Mensagem.Id)).AsQueryable();
            return base.Select(query);
        }

        /// <summary>
        /// Atualiza as mensagens recebidas para lidas.
        /// </summary>
        /// <param name="usuarioM"></param>
        public void Salvar(List<UsuarioMensagemVO> usuarioM)
        {
            try
            {
                base.IniciaTransacao();
                foreach(UsuarioMensagemVO uM in usuarioM)
                {
                    uM.LidoMensagem = true;
                    new UsuarioMensagemBO(uM).Salvar();
                }
                base.FinalizaTransacao(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
