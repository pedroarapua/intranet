using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de MensagemVO
    /// </summary>
    public class MensagemBO:BaseBO<MensagemVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro MensagemVO
        /// </summary>
        /// <param name="user"></param>
        public MensagemBO(MensagemVO mensagem)
        {
            base.Object = mensagem;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public MensagemBO() { }

        #endregion

        #region metodos

        public List<MensagemVO> BuscarMensagensEnviadas(UsuarioVO usuario, DateTime? dataIni, DateTime? dataFim)
        {
            IQueryable<MensagemVO> query = base.GetQueryLinq.Where(x => x.Removido == false && x.UsuarioEnvio.Id == usuario.Id);

            if (dataFim.HasValue)
                dataFim = dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (dataIni.HasValue && dataFim.HasValue)
            {
                query = query.Where(x => x.Data >= dataIni && x.Data <= dataFim);
            }
            else if (dataIni.HasValue)
            {
                query = query.Where(x => x.Data >= dataIni);
            }
            else if (dataFim.HasValue)
            {
                query = query.Where(x => x.Data <= dataFim);
            }
            return base.Select(query).OrderByDescending(x => x.Data).ToList();
        }

        #endregion
    }
}
