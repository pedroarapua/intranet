using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.DataAccess;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de UsuarioBaseConhecimentoBO
    /// </summary>
    public class UsuarioBaseConhecimentoBO:BaseBO<UsuarioBaseConhecimentoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro UsuarioBaseConhecimentoBO
        /// </summary>
        /// <param name="user"></param>
        public UsuarioBaseConhecimentoBO(UsuarioBaseConhecimentoVO usuarioConhecimento)
        {
            base.Object = usuarioConhecimento;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public UsuarioBaseConhecimentoBO() { }

		/// <summary>
		/// Método que busca as bases de conhecimento do usuário
		/// </summary>
		/// <param name="usuario"></param>
		/// <returns></returns>
		public List<UsuarioBaseConhecimentoVO> BuscarPorUsuario(Int32 idUsuario)
		{
			IQueryable<UsuarioBaseConhecimentoVO> query = base.GetQueryLinq.Where(x => x.Usuario.Id == idUsuario);
			return base.Select(query);
		}

		/// <summary>
		/// Metodo que busca os conhecimentos cadastrados
		/// </summary>
		/// <param name="lstConhecimentosId"></param>
		/// <returns></returns>
		public IList<UsuarioBaseConhecimentoVO> BuscarPorConhecimentos(List<Int32> lstConhecimentosId)
		{
			IList<UsuarioBaseConhecimentoVO> lstRetorno = new List<UsuarioBaseConhecimentoVO>();
			if(lstConhecimentosId.Count > 0) 
				lstRetorno = Db.Session.CreateQuery(String.Format("from UsuarioBaseConhecimentoVO where Conhecimento.Id in ({0})", String.Join(",", lstConhecimentosId))).List<UsuarioBaseConhecimentoVO>();
			return lstRetorno;
		}

		
        #endregion
    }
}
