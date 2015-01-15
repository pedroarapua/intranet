using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Enumertators
{
	public enum ETipoAcao
	{
		// Perfis de Acesso //
		VisualizarPerfisAcesso = 1,
		AdicionarPerfisAcesso = 2,
		EditarPerfisAcesso = 3,
		RemoverPerfisAcesso = 4,

		// Páginas //
		VisualizarPaginas = 5,
		AdicionarPaginas = 6,
		EditarPaginas = 7,
		RemoverPaginas = 8,

		// Usuarios //
		VisualizarUsuarios = 9,
		AdicionarUsuarios = 10,
		EditarUsuarios = 11,
		RemoverUsuarios = 12,
		AdicionarTelefonesUsuarios = 13,

		// Temas //
		VisualizarTemas = 14, 
		AdicionarTemas = 15,
		EditarTemas = 16,
		RemoverTemas = 17,

		// Sistemas //
		VisualizarSistemas = 18,
		AdicionarSistemas = 19,
		EditarSistemas = 20,
		RemoverSistemas = 21,

		// Configurações //
		VisualizarConfiguracoes = 22,
		SalvarVisualizarConfiguracoes = 23,

		// Fotos //
		VisualizarFotos = 24,
		AdicionarGaleriaFotos = 25,
		EditarGaleriaFotos = 26,
		RemoverGaleriaFotos = 27,
		AdicionarFotos = 28,
		EditarFotos = 29,
		RemoverFotos = 30,

		// Videos //
		VisualizarVideos = 31,
		AdicionarGaleriaVideos =32,
		EditarGaleriaVideos = 33,
		RemoverGaleriaVideos = 34,
		AdicionarVideos = 35,
		EditarVideos = 36,
		RemoverVideos = 37,

		// Pesquisas //
		VisualizarPesquisas = 38,
		AdicionarPesquisas = 39,
		EditarPesquisas = 40,
		RemoverPesquisas = 41,
		VisualizarResultadosPesquisas = 42,

		// Pontos de Usuários //
		VisualizarPontosUsuarios = 43,
		AdicionarPontosUsuarios = 44,
		EditarPontosUsuarios = 45,
		RemoverPontosUsuarios = 46,
		ExportarExcelPontosUsuarios = 47,
		GraficoHorasPontosUsuarios = 48,

		// Outras Permissões //
		RegistrarPontosUsuarios = 49,

		// Contatos //
		VisualizarContatos = 50,
		AdicionarContatos = 51,
		EditarContatos = 52,
		RemoverContatos = 53,

		// Mensagens //
		VisualizarMensagens = 61,
		AdicionarMensagens = 62,
		EditarMensagens = 63,
		RemoverMensagens = 64,
		MarcarComoLidoMensagens = 65,

		// Responder Pesquisas //
		VisualizarResponderPesquisas = 66,
		SalvarResponderPesquisas = 67,
		VisualizarGraficoResponderPesquisas = 68,

		// Banco de Arquivos //
		VisualizarBancoArquivos = 69,
		AdicionarPastaBancoArquivos = 70,
		EditarPastaBancoArquivos = 71,
		RemoverPastaBancoArquivos = 72,
		AdicionarArquivoBancoArquivos = 73,
		EditarArquivoBancoArquivos = 74,
		RemoverArquivoBancoArquivos = 75,
		DownloadArquivoBancoArquivos = 76,

		// Visualizar Notícias //
		VisualizarNoticiasColaborador = 77,

		// Aniversariantes //
		VisualizarAniversariantes = 78,
		EnviarMensagemAniversariantes = 79,

		// Meu Perfil //
		VisualizarMeuPerfil = 80,
		SalvarMeuPerfil = 81,

		// Tipos Empresa //
		VisualizarTiposEmpresa = 82,
		AdicionarTiposEmpresa = 83,
		EditarTiposEmpresa = 84,
		RemoverTiposEmpresa = 85,

		// Ensalamento //
		VisualizarEnsalamento = 95,
		AdicionarEnsalamento = 96,
		EditarEnsalamento = 97,
		RemoverEnsalamento = 98,

		// VisualizarGalerias //
		VisualizarGalerias = 58,
		VisualizarFotosGalerias = 59,
		VisualizarVideosGalerias = 60,
		AdicionarComentarioFoto = 104,
		AdicionarComentarioVideo = 105,
		EditarComentarioFoto = 106,
		EditarComentarioVideo = 107,
		RemoverComentarioFoto = 108,
		RemoverComentarioVideo = 109,

		// Funções //
		VisualizarFuncoes = 110,
		AdicionarFuncoes = 111,
		EditarFuncoes = 112,
		RemoverFuncoes = 113,

		// Home //
		VisualizarHome = 103,
		VisualizarTwitter = 114,

		// Setores //
		VisualizarSetores = 91,
		AdicionarSetores = 92,
		EditarSetores = 93,
		RemoverSetores = 94,
		VisualizarChartSetores = 115,

		// Visualizar Organização //
		VisualizarOrganizacao = 116,

		// Empresas //
		VisualizarEmpresas = 86,
		AdicionarEmpresas = 87,
		EditarEmpresas = 88,
		RemoverEmpresas = 89,
		AdicionarTelefonesEmpresas = 90,
		VisualizarChartEmpresas = 117,

		// Colaboradores //
		VisualizarColaboradores = 119,
		VisualizarPerfilColaborador = 120,

		// Noticias //
		VisualizarNoticias = 54,
		AdicionarNoticias = 55,
		EditarNoticias = 56,
		RemoverNoticias = 57,
		VisualizarHtmlNoticias = 121,

		// Extensões Arquivos //
		VisualizarExtensoesArquivo = 122,
		AdicionarExtensoesArquivo = 123,
		RemoverExtensoesArquivo = 124,

		// Reuniões //
		VisualizarReunioes = 99,
		EditarReunioes = 100,
		RemoverReunioes = 101,
		CancelarReunioes = 102,
		AdicionarReunioes = 118,
		VisualizarTodasReunioes = 125,

		// Manual dos Colaboradores //
		VisualizarItemManualColaborador = 126, 
		AdicionarTopicoManualColaborador = 127,
		EditarTopicoManualColaborador = 128, 
		RemoverTopicoManualColaborador = 129, 
		AdicionarItemManualColaborador = 130, 
		EditarItemManualColaborador = 131, 
		RemoverItemManualColaborador = 132,

		// Base de Conhecimento //
		VisualizarBaseConhecimento = 133,
		AdicionarTopicoBaseConhecimento = 134,
		EditarTopicoBaseConhecimento = 135,
		RemoverTopicoBaseConhecimento = 136,
		AdicionarBaseConhecimento = 137,
		EditarBaseConhecimento = 138,
		RemoverBaseConhecimento = 139,

		// Vagas de Emprego //
		VisualizarVagaEmprego = 140,
		AdicionarVagaEmprego = 141,
		EditarVagaEmprego = 142,
		RemoverVagaEmprego = 143,
		FecharVagaEmprego = 144,
		VisualizarCurriculosVagaEmprego = 145,
		DownloadCurriculoVagaEmprego = 146,
		IndicarVagaEmprego = 147,

		// Buscar Perfil de Conhecimento //
		VisualizarBuscaPerfil = 148,
		VisualizarConhecimentoUsuarioBuscaPerfil = 149
	}
}
