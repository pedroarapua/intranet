using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de PesquisaOpiniaoVO
    /// </summary>
    public class PesquisaOpiniaoBO:BaseBO<PesquisaOpiniaoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro PesquisaOpiniaoVO
        /// </summary>
        /// <param name="user"></param>
        public PesquisaOpiniaoBO(PesquisaOpiniaoVO pesquisa)
        {
            base.Object = pesquisa;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public PesquisaOpiniaoBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// Busca pesquisas pelo usuário logado
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<PesquisaOpiniaoVO> GetPesquisas(List<PesquisaOpiniaoVO> lst, Boolean ativo, Boolean iniciada, Boolean finalizada)
        {
            if (iniciada && ativo && finalizada)
            {
                lst = lst.FindAll(
                    x=> (
                            (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                        )
                        ||
                        (x.DataInicial > DateTime.Now)
                        ||
                        (x.DataFinal < DateTime.Now)
                    );
            }
            else if (iniciada)
            {
                if (ativo)
                {
                    lst = lst.FindAll(
                       x => (
                           (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                           )
                           ||
                           (x.DataInicial > DateTime.Now)
                   );
                }
                else if (finalizada)
                {
                    lst = lst.FindAll(
                        x => (
                            (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                            )
                            ||
                            (x.DataFinal < DateTime.Now)
                    );
                }
                else
                {
                    lst = lst.FindAll(
                        x => (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                    );
                }
            }
            else if (ativo)
            {
                if (finalizada)
                {
                    lst = lst.FindAll(
                        x =>
                            x.DataInicial > DateTime.Now
                            ||
                            x.DataFinal < DateTime.Now
                    );
                }
                else
                {
                    lst = lst.FindAll(
                        x =>
                            x.DataInicial > DateTime.Now
                    );
                }
            }
            else if (finalizada)
            {
                lst = lst.FindAll(
                    x =>
                        x.DataFinal < DateTime.Now
                );
            }

            return lst;
        }

        /// <summary>
        /// Busca pesquisas pelo usuário logado
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<PesquisaOpiniaoVO> BuscarPorUsuario(UsuarioVO usuario, Boolean ativo, Boolean iniciada, Boolean finalizada, DateTime? dataIni, DateTime? dataFim)
        {
            IQueryable<PesquisaOpiniaoVO> query = base.GetQueryLinq.Where(x => x.Usuarios.Any(x1 => x1.Id == usuario.Id));
            if (dataFim.HasValue)
                dataFim = dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (dataIni.HasValue && dataFim.HasValue)
            {
                query = query.Where(x => (x.DataInicial >= dataIni && x.DataFinal <= dataFim) || (x.DataInicial < dataIni && x.DataFinal > dataFim));
            }
            else if (dataIni.HasValue)
            {
                query = query.Where(x => x.DataInicial >= dataIni);
            }
            else if (dataFim.HasValue)
            {
                query = query.Where(x => x.DataFinal <= dataFim);
            }

            if (iniciada && ativo && finalizada)
            {
                query = query.Where(
                    x => (
                            (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                        )
                        ||
                        (x.DataInicial > DateTime.Now)
                        ||
                        (x.DataFinal < DateTime.Now)
                    );
            }
            else if (iniciada)
            {
                if (ativo)
                {
                    query = query.Where(
                       x => (
                           (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                           )
                           ||
                           (x.DataInicial > DateTime.Now)
                   );
                }
                else if (finalizada)
                {
                    query = query.Where(
                        x => (
                            (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                            )
                            ||
                            (x.DataFinal < DateTime.Now)
                    );
                }
                else
                {
                    query = query.Where(
                        x => (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                    );
                }
            }
            else if (ativo)
            {
                if (finalizada)
                {
                    query = query.Where(
                        x =>
                            x.DataInicial > DateTime.Now
                            ||
                            x.DataFinal < DateTime.Now
                    );
                }
                else
                {
                    query = query.Where(
                        x =>
                            x.DataInicial > DateTime.Now
                    );
                }
            }
            else if (finalizada)
            {
                //if (iniciada)
                //{
                //    query = query.Where(
                //        x => (
                //            (x.DataInicial >= DateTime.Now && x.DataFinal <= DateTime.Now) || (x.DataInicial < DateTime.Now && x.DataFinal > DateTime.Now)
                //            )
                //            ||
                //            (x.DataFinal < DateTime.Now)
                //    );
                //}
                //else
                //{
                query = query.Where(
                    x =>
                        x.DataFinal < DateTime.Now
                );
                //}
            }

            return base.Select(query);
        }

        #endregion
    }
}
