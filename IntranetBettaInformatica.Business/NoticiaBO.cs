using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de NoticiaVO
    /// </summary>
    public class NoticiaBO:BaseBO<NoticiaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro NoticiaVO
        /// </summary>
        /// <param name="user"></param>
        public NoticiaBO(NoticiaVO noticia)
        {
            base.Object = noticia;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public NoticiaBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// Busca as noticias pelo usuario logado
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="ativo"></param>
        /// <param name="iniciada"></param>
        /// <param name="finalizada"></param>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public List<NoticiaVO> Buscar(UsuarioVO usuario, Boolean ativo, Boolean iniciada, Boolean finalizada, DateTime? dataIni, DateTime? dataFim)
        {
            IQueryable<NoticiaVO> query = base.GetQueryLinq;
            if (usuario != null)
            {
                query = query.Where(x => x.Usuarios.Any(x1 => x1.Id == usuario.Id));
            }

            if (dataFim.HasValue)
                dataFim = dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (dataIni.HasValue && dataFim.HasValue)
            {
                query = query.Where(x => (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal));
            }
            else if (dataIni.HasValue)
            {
                query = query.Where(x => x.DataInicial >= dataIni);
            }
            else if (dataFim.HasValue)
            {
                query = query.Where(x => x.DataFinal <= dataFim);
            }

            if (!dataIni.HasValue)
                dataIni = DateTime.Now;
            if (!dataFim.HasValue)
                dataFim = DateTime.Now;
            if (iniciada && ativo && finalizada)
            {
                query = query.Where(
                    x => (
                            (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                        )
                        ||
                        (x.DataInicial > dataIni)
                        ||
                        (x.DataFinal < dataFim)
                    );
            }
            else if (iniciada)
            {
                if (ativo)
                {
                    query = query.Where(
                       x => (
                           (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                           )
                           ||
                           (x.DataInicial > dataIni)
                   );
                }
                else if (finalizada)
                {
                    query = query.Where(
                        x => (
                            (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                            )
                            ||
                            (x.DataFinal < dataFim)
                    );
                }
                else
                {
                    query = query.Where(
                        x => (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                    );
                }
            }
            else if (ativo)
            {
                if (finalizada)
                {
                    query = query.Where(
                        x =>
                            x.DataInicial > dataIni
                            ||
                            x.DataFinal < dataFim
                    );
                }
                else
                {
                    query = query.Where(
                        x =>
                            x.DataInicial > dataIni
                    );
                }
            }
            else if (finalizada)
            {
                query = query.Where(
                    x =>
                        x.DataFinal < dataFim
                );
            }

            return base.Select(query);
        }

        public List<NoticiaVO> GetNoticias(List<NoticiaVO> lst, Boolean ativo, Boolean iniciada, Boolean finalizada)
        {
            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;
                
            if (iniciada && ativo && finalizada)
            {
                lst = lst.FindAll(
                    x => (
                            (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                        )
                        ||
                        (x.DataInicial > dataIni)
                        ||
                        (x.DataFinal < dataFim)
                    );
            }
            else if (iniciada)
            {
                if (ativo)
                {
                    lst = lst.FindAll(
                       x => (
                           (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                           )
                           ||
                           (x.DataInicial > dataIni)
                   );
                }
                else if (finalizada)
                {
                    lst = lst.FindAll(
                        x => (
                            (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                            )
                            ||
                            (x.DataFinal < dataFim)
                    );
                }
                else
                {
                    lst = lst.FindAll(
                        x => (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim) || (dataIni >= x.DataInicial && dataIni <= x.DataFinal) || (dataFim >= x.DataInicial && dataFim <= x.DataFinal)
                    );
                }
            }
            else if (ativo)
            {
                if (finalizada)
                {
                    lst = lst.FindAll(
                        x =>
                            x.DataInicial > dataIni
                            ||
                            x.DataFinal < dataFim
                    );
                }
                else
                {
                    lst = lst.FindAll(
                        x =>
                            x.DataInicial > dataIni
                    );
                }
            }
            else if (finalizada)
            {
                lst = lst.FindAll(
                    x =>
                        x.DataFinal < dataFim
                );
            }
            return lst;
        }

        /// <summary>
        /// Busca as noticias existentes
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="iniciada"></param>
        /// <param name="finalizada"></param>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public List<NoticiaVO> Buscar(Boolean ativo, Boolean iniciada, Boolean finalizada, DateTime? dataIni, DateTime? dataFim)
        {
            return this.Buscar(null, ativo, iniciada, finalizada, dataIni, dataFim);
        }

        #endregion
    }
}
