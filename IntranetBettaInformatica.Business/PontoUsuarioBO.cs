using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de PontoUsuarioVO
    /// </summary>
    public class PontoUsuarioBO:BaseBO<PontoUsuarioVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro PontoUsuarioVO
        /// </summary>
        /// <param name="user"></param>
        public PontoUsuarioBO(PontoUsuarioVO ponto)
        {
            base.Object = ponto;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public PontoUsuarioBO() { }

        #endregion

        #region metodos

        public List<PontoUsuarioVO> BuscarPontosDoDia(UsuarioVO usuario)
        {
            return BuscarPontosDoDia(DateTime.Today, usuario);
        }

        public List<PontoUsuarioVO> BuscarPontosDoDia(DateTime data, UsuarioVO usuario)
        {
            return base.Select(base.GetQueryLinq.Where(x => x.Removido == false && x.Usuario.Id == usuario.Id && x.Data == data));
        }

        /// <summary>
        /// Busca de pontos passando como parametro data inicial, final e usuario
        /// </summary>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<PontoUsuarioVO> BuscarPontos(DateTime? dataIni, DateTime? dataFim, UsuarioVO usuario)
        {
            IQueryable<PontoUsuarioVO> query = base.GetQueryLinq.Where(x=> x.Removido == false);
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

            if (usuario != null)
                query = query.Where(x => x.Usuario.Id == usuario.Id);

            return base.Select(query);
        }

        

        #endregion
    }
}
