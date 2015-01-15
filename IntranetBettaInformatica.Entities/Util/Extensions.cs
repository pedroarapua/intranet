using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Enumertators;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica
{
    public static class Extensions
    {
        public static Int32 ToInt32(this Object obj)
        {
            return Convert.ToInt32(obj);
        }
		public static String ToText(this ETipoAcao acao)
		{
			switch (acao)
			{
				case ETipoAcao.VisualizarPerfisAcesso: return "Visualizar";
				case ETipoAcao.AdicionarPerfisAcesso: return "Adicionar";
				case ETipoAcao.EditarPerfisAcesso: return "Editar";
				case ETipoAcao.RemoverPerfisAcesso: return "Remover";
				case ETipoAcao.VisualizarPaginas: return "Visualizar";
				case ETipoAcao.AdicionarPaginas: return "Adicionar";
				case ETipoAcao.EditarPaginas: return "Editar";
				case ETipoAcao.RemoverPaginas: return "Remover";
				case ETipoAcao.VisualizarUsuarios: return "Visualizar";
				case ETipoAcao.AdicionarUsuarios: return "Adicionar";
				case ETipoAcao.EditarUsuarios: return "Editar";
				case ETipoAcao.RemoverUsuarios: return "Remover";
				case ETipoAcao.AdicionarTelefonesUsuarios: return "Adicionar Telefones";
				case ETipoAcao.VisualizarTemas: return "Visualizar";
				case ETipoAcao.AdicionarTemas: return "Adicionar";
				case ETipoAcao.EditarTemas: return "Editar";
				case ETipoAcao.RemoverTemas: return "Remover";
				case ETipoAcao.VisualizarSistemas: return "Visualizar";
				case ETipoAcao.AdicionarSistemas: return "Adicionar";
				case ETipoAcao.EditarSistemas: return "Editar";
				case ETipoAcao.RemoverSistemas: return "Remover";
				case ETipoAcao.VisualizarConfiguracoes: return "Visualizar";
				case ETipoAcao.SalvarVisualizarConfiguracoes: return "Salvar Configurações";
				case ETipoAcao.VisualizarFotos: return "Visualizar";
				case ETipoAcao.AdicionarGaleriaFotos: return "Adicionar Galeria";
				case ETipoAcao.EditarGaleriaFotos: return "Editar Galeria";
				case ETipoAcao.RemoverGaleriaFotos: return "Remover Galeria";
				case ETipoAcao.AdicionarFotos: return "Adicionar";
				case ETipoAcao.EditarFotos: return "Editar";
				case ETipoAcao.RemoverFotos: return "Remover";
				case ETipoAcao.VisualizarVideos: return "Visualizar";
				case ETipoAcao.AdicionarGaleriaVideos: return "Adicionar Galeria";
				case ETipoAcao.EditarGaleriaVideos: return "Editar Galeria";
				case ETipoAcao.RemoverGaleriaVideos: return "Remover Galeria";
				case ETipoAcao.AdicionarVideos: return "Adicionar";
				case ETipoAcao.EditarVideos: return "Editar";
				case ETipoAcao.RemoverVideos: return "Remover";
				case ETipoAcao.VisualizarPesquisas: return "Visualizar";
				case ETipoAcao.AdicionarPesquisas: return "Adicionar";
				case ETipoAcao.EditarPesquisas: return "Editar";
				case ETipoAcao.RemoverPesquisas: return "Remover";
				case ETipoAcao.VisualizarResultadosPesquisas: return "Visualizar";
				case ETipoAcao.VisualizarPontosUsuarios: return "Visualizar";
				case ETipoAcao.AdicionarPontosUsuarios: return "Adicionar";
				case ETipoAcao.EditarPontosUsuarios: return "Editar";
				case ETipoAcao.RemoverPontosUsuarios: return "Remover";
				case ETipoAcao.ExportarExcelPontosUsuarios: return "Exportar p/ Excel";
				case ETipoAcao.GraficoHorasPontosUsuarios: return "Grafico de Horas p/ Usuario";
				case ETipoAcao.RegistrarPontosUsuarios: return "Registrar Ponto Eletrônico";
				case ETipoAcao.VisualizarContatos: return "Visualizar";
				case ETipoAcao.AdicionarContatos: return "Adicionar";
				case ETipoAcao.EditarContatos: return "Editar";
				case ETipoAcao.RemoverContatos: return "Remover";
				case ETipoAcao.VisualizarNoticias: return "Visualizar";
				case ETipoAcao.AdicionarNoticias: return "Adicionar";
				case ETipoAcao.EditarNoticias: return "Editar";
				case ETipoAcao.RemoverNoticias: return "Remover";
				case ETipoAcao.VisualizarGalerias: return "Visualizar";
				case ETipoAcao.VisualizarFotosGalerias : return "Visualizar Fotos";
				case ETipoAcao.VisualizarVideosGalerias: return "Visualizar Videos";
				case ETipoAcao.AdicionarComentarioFoto: return "Adicionar Comentario Foto";
				case ETipoAcao.AdicionarComentarioVideo: return "Adicionar Comentario Video";
				case ETipoAcao.EditarComentarioFoto: return "Editar Comentario Foto";
				case ETipoAcao.EditarComentarioVideo: return "Editar Comentario Video";
				case ETipoAcao.RemoverComentarioFoto: return "Remover Comentario Foto";
				case ETipoAcao.RemoverComentarioVideo: return "Remover Comentario Video";
				case ETipoAcao.VisualizarMensagens: return "Visualizar";
				case ETipoAcao.AdicionarMensagens: return "Adicionar";
				case ETipoAcao.EditarMensagens: return "Editar";
				case ETipoAcao.RemoverMensagens: return "Remover";
				case ETipoAcao.MarcarComoLidoMensagens: return "Marcar c/ Lido";
				case ETipoAcao.VisualizarResponderPesquisas: return "Visualizar";
				case ETipoAcao.SalvarResponderPesquisas: return "Salvar";
				case ETipoAcao.VisualizarGraficoResponderPesquisas: return "Visualizar Grafico de Questão";
				case ETipoAcao.VisualizarBancoArquivos: return "Visualizar";
				case ETipoAcao.AdicionarPastaBancoArquivos: return "Adicionar Pasta";
				case ETipoAcao.EditarPastaBancoArquivos: return "Editar Pasta";
				case ETipoAcao.RemoverPastaBancoArquivos: return "Remover Pasta";
				case ETipoAcao.AdicionarArquivoBancoArquivos: return "Adicionar Arquivo";
				case ETipoAcao.EditarArquivoBancoArquivos: return "Editar Arquivo";
				case ETipoAcao.RemoverArquivoBancoArquivos: return "Remover Arquivo";
				case ETipoAcao.DownloadArquivoBancoArquivos: return "Download de Arquivo";
				case ETipoAcao.VisualizarNoticiasColaborador: return "Visualizar";
				case ETipoAcao.VisualizarAniversariantes: return "Visualizar";
				case ETipoAcao.EnviarMensagemAniversariantes: return "Enviar Mensagem";
				case ETipoAcao.VisualizarMeuPerfil: return "Visualizar";
				case ETipoAcao.SalvarMeuPerfil: return "Salvar";
				case ETipoAcao.VisualizarTiposEmpresa: return "Visualizar";
				case ETipoAcao.AdicionarTiposEmpresa: return "Adicionar";
				case ETipoAcao.EditarTiposEmpresa: return "Editar";
				case ETipoAcao.RemoverTiposEmpresa: return "Remover";
				case ETipoAcao.VisualizarEmpresas: return "Visualizar";
				case ETipoAcao.AdicionarEmpresas: return "Adicionar";
				case ETipoAcao.EditarEmpresas: return "Editar";
				case ETipoAcao.RemoverEmpresas: return "Remover";
				case ETipoAcao.AdicionarTelefonesEmpresas: return "Adicionar Telefones";
				case ETipoAcao.VisualizarSetores: return "Visualizar";
				case ETipoAcao.AdicionarSetores: return "Adicionar";
				case ETipoAcao.EditarSetores: return "Editar";
				case ETipoAcao.RemoverSetores: return "Remover";
				case ETipoAcao.VisualizarEnsalamento: return "Visualizar";
				case ETipoAcao.AdicionarEnsalamento: return "Adicionar";
				case ETipoAcao.EditarEnsalamento: return "Editar";
				case ETipoAcao.RemoverEnsalamento: return "Remover";
				case ETipoAcao.VisualizarReunioes: return "Visualizar";
				case ETipoAcao.EditarReunioes: return "Editar";
				case ETipoAcao.RemoverReunioes: return "Remover";
				case ETipoAcao.CancelarReunioes: return "Cancelar";
				case ETipoAcao.VisualizarTodasReunioes: return "Visualizar Todas";
				case ETipoAcao.VisualizarHome: return "Visualizar";
				case ETipoAcao.VisualizarFuncoes: return "Visualizar";
				case ETipoAcao.AdicionarFuncoes: return "Adicionar";
				case ETipoAcao.EditarFuncoes: return "Editar";
				case ETipoAcao.RemoverFuncoes: return "Remover";
				case ETipoAcao.VisualizarTwitter: return "Visualizar Twitter";
				case ETipoAcao.VisualizarChartSetores: return "Gráfico da Estrutura Organizacional";
				case ETipoAcao.VisualizarOrganizacao: return "Visualizar";
				case ETipoAcao.VisualizarChartEmpresas: return "Gráfico da Estrutura Organizacional";
				case ETipoAcao.AdicionarReunioes: return "Adicionar";
				case ETipoAcao.VisualizarColaboradores: return "Visualizar";
				case ETipoAcao.VisualizarPerfilColaborador: return "Visualizar Perfil Colaborador";
				case ETipoAcao.VisualizarHtmlNoticias: return "Visualizar HTML";
				case ETipoAcao.VisualizarExtensoesArquivo: return "Visualizar";
				case ETipoAcao.AdicionarExtensoesArquivo: return "Adicionar";
				case ETipoAcao.RemoverExtensoesArquivo: return "Remover";
				case ETipoAcao.VisualizarItemManualColaborador: return "Visualizar";
				case ETipoAcao.AdicionarTopicoManualColaborador: return "Adicionar Tópico";
				case ETipoAcao.EditarTopicoManualColaborador: return "Editar Tópico";
				case ETipoAcao.RemoverTopicoManualColaborador: return "Remover Tópico";
				case ETipoAcao.AdicionarItemManualColaborador: return "Adicionar Item Manual";
				case ETipoAcao.EditarItemManualColaborador: return "Editar Item Manual";
				case ETipoAcao.RemoverItemManualColaborador: return "Remover Item Manual";
				case ETipoAcao.VisualizarBaseConhecimento: return "Visualizar";
				case ETipoAcao.AdicionarTopicoBaseConhecimento: return "Adicionar Tópico";
				case ETipoAcao.EditarTopicoBaseConhecimento: return "Editar Tópico";
				case ETipoAcao.RemoverTopicoBaseConhecimento: return "Remover Tópico";
				case ETipoAcao.AdicionarBaseConhecimento: return "Adicionar Conhecimento";
				case ETipoAcao.EditarBaseConhecimento: return "Editar Conhecimento";
				case ETipoAcao.RemoverBaseConhecimento: return "Remover Conhecimento";
				case ETipoAcao.VisualizarVagaEmprego: return "Visualizar";
				case ETipoAcao.AdicionarVagaEmprego: return "Adicionar";
				case ETipoAcao.EditarVagaEmprego: return "Editar";
				case ETipoAcao.RemoverVagaEmprego: return "Remover";
				case ETipoAcao.FecharVagaEmprego: return "Fechar Vaga";
				case ETipoAcao.VisualizarCurriculosVagaEmprego: return "Currículos";
				case ETipoAcao.DownloadCurriculoVagaEmprego: return "Download Currículo";
				case ETipoAcao.IndicarVagaEmprego: return "Indicar";
				case ETipoAcao.VisualizarBuscaPerfil: return "Visualizar";
				case ETipoAcao.VisualizarConhecimentoUsuarioBuscaPerfil: return "Visualizar Conhecimentos";
			}
			return String.Empty;
		}
		public static String ToText(this ETipoExtensao tipoExtensao)
		{
			String retorno = String.Empty;
			switch (tipoExtensao)
			{
				case ETipoExtensao.Fotos: retorno = "Fotos"; break;
				case ETipoExtensao.Videos: retorno = "Videos"; break;
				case ETipoExtensao.BancoArquivos: retorno = "Banco de Arquivos"; break;
				case ETipoExtensao.Curriculum: retorno = "Curriculum"; break;
			}
			return retorno;
		}
		public static String ToText(this EStatusVagaEmprego statusVagaEmprego)
		{
			String retorno = String.Empty;
			switch (statusVagaEmprego)
			{
				case EStatusVagaEmprego.Ativa: retorno = "Aberta"; break;
				case EStatusVagaEmprego.Fechada: retorno = "Fechada"; break;
			}
			return retorno;
		}
		public static String ToText(this ENivelConhecimento nivel)
		{
			String retorno = String.Empty;
			switch (nivel)
			{
				case ENivelConhecimento.Nenhum: retorno = "Nenhum"; break;
				case ENivelConhecimento.Basico: retorno = "Básico"; break;
				case ENivelConhecimento.Intermediario: retorno = "Intermediário"; break;
				case ENivelConhecimento.Avancado: retorno = "Avançado"; break;
			}
			return retorno;
		}
		
        public static Boolean IsNullOrEmpty(this String obj)
        {
            return String.IsNullOrEmpty(obj);
        }
        public static Boolean ToBoolean(this Int32 obj)
        {
            return Convert.ToBoolean(obj);
        }
		public static String ToDescricaoRemovido(this Boolean obj, Boolean masculino)
		{
			return obj ? String.Format("[Removid{0}]", masculino ? "o" : "a") : String.Empty;
		}
        public static object GetPropriedadeValor(this object obj, string property)
        {
            String[] props = property.Split('.');
            System.Reflection.PropertyInfo propertyInfo = null;
            foreach (String p in props)
            {
                if (obj == null)
                {
                    obj = "";
                    break;
                }
                Type type = obj.GetType();
                propertyInfo = obj.GetType().GetProperty(p);
                object value = propertyInfo.GetValue(obj, null);
                if (propertyInfo.PropertyType == typeof(Boolean) || propertyInfo.PropertyType == typeof(Boolean?))
                {
                    obj = value != null ? ((Boolean)value) ? "Sim" : "Não" : "";
                }
                else
                    obj = value;

            }
            return obj;
        }
        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string Truncate(this string source, int length)
        {
            if (source.TrimEnd().Length > length)
            {
				source = source.TrimEnd().Substring(0, length);
                source += "...";
            }
            return source;
        }
    }
}
