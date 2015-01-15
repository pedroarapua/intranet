using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ReuniaoVO
    /// </summary>
    public class ReuniaoBO:BaseBO<ReuniaoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ReuniaoVO
        /// </summary>
        /// <param name="user"></param>
        public ReuniaoBO(ReuniaoVO reuniao)
        {
            base.Object = reuniao;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ReuniaoBO() { }

        #endregion

		#region metodos

		/// <summary>
		/// Metodo que busca as reuniões do usuário no dia
		/// </summary>
		/// <param name="usuario"></param>
		/// <returns></returns>
		public List<ReuniaoVO> BuscarReunioesParaDia(UsuarioVO usuario)
		{
			IQueryable<ReuniaoVO> query = base.GetQueryLinq.Where(x => x.Participantes.Any(x1 => x1.Id == usuario.Id) && x.ECancelada == false && x.DataInicial.Date == DateTime.Today.Date);
			return base.Select(query);
		}

		/// <summary>
		/// Metodo que busca reuniões na gerência de reuniões
		/// </summary>
		/// <param name="usuario"></param>
		/// <param name="visualizarTodas"></param>
		/// <returns></returns>
		public List<ReuniaoVO> BuscarReunioesUsuario(UsuarioVO usuario, Boolean visualizarTodas)
		{
			IQueryable<ReuniaoVO> query = base.GetQueryLinq.Where(x => x.Participantes.Any(x1 => x1.Id == usuario.Id) || visualizarTodas == true);
			return base.Select(query);
		}

		#endregion
	}
}
