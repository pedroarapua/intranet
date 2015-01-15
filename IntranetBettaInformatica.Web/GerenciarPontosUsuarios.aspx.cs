using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Ext.Net;
using IntranetBettaInformatica.Business;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Web.Common;
using System.Collections.Generic;
using ExcelLibrary.SpreadSheet;
using System.IO;
using System.Reflection;

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarPontosUsuarios : BasePage
    {

        #region propriedades

        private PontoUsuarioVO PontoSelecionado
        {
            get
            {
                if (this.ViewState["PontoSelecionado"] == null)
                    return null;
                return (PontoUsuarioVO)this.ViewState["PontoSelecionado"];
            }
            set { this.ViewState["PontoSelecionado"] = value; }
        }

        #endregion

        #region eventos

        /// <summary>
        /// Evento disparado para carregar a página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPagina();
                CarregarUsuariosBusca();
                base.SetTituloIconePagina(frmTitulo);
            }
			btnNovo.Disabled = !hdfAdicionarPontosUsuarios.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverPontoUsuario(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarPontoUsuario(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winPontosUsuario.Title = "Cadastrando Ponto de Usuário";
            CarregarUsuarios(null);
            LimparCampos();
            txtData.MaxDate = DateTime.Today;
            winPontosUsuario.Show((Control)sender);
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            txtData.MaxDate = DateTime.Today;
            PreencherCampos(e);
            winPontosUsuario.Title = "Alterando Ponto de Usuário";
            winPontosUsuario.Show((Control)sender);
        }

        protected void btnBuscar_Click(object sender, DirectEventArgs e)
        {
            LoadPagina();
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportaExcel(BuscarPontos());
        }

        protected void btnGrafico_Click(object sender, DirectEventArgs e)
        {
            VisualizarGrafico();
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strPontosUsuarios.DataSource = BuscarPontos();
            strPontosUsuarios.DataBind();
        }

        private List<PontoUsuarioVO> BuscarPontos()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            UsuarioVO usuario = null;
            if (!txtDataInicial.Text.IsNullOrEmpty() && txtDataInicial.Text != txtDataInicial.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicial.Text);
            }
            if (!txtDataFinal.Text.IsNullOrEmpty() && txtDataFinal.Text != txtDataFinal.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinal.Text);
            }
            if (cboUsuariosBusca.SelectedIndex > 0)
                usuario = new UsuarioVO() { Id = cboUsuariosBusca.Value.ToInt32() };

            return new PontoUsuarioBO().BuscarPontos(dataIni, dataFim, usuario);
        }

        private void RemoverPontoUsuario(DirectEventArgs e)
        {
            try
            {
                PontoUsuarioVO pontoU = new PontoUsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
                new PontoUsuarioBO(pontoU).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover ponto de usuário.", "");
            }
        }

        private void SalvarPontoUsuario(DirectEventArgs e)
        {
            try
            {
                PontoUsuarioVO pontoU = new PontoUsuarioVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    pontoU = PontoSelecionado;

                pontoU.Data = txtData.SelectedDate;
                pontoU.HoraInicio = pontoU.Data.AddHours(txtHoraInicial.SelectedTime.Hours).AddMinutes(txtHoraInicial.SelectedTime.Minutes);
                pontoU.HoraTermino = pontoU.Data.AddHours(txtHoraFinal.SelectedTime.Hours).AddMinutes(txtHoraFinal.SelectedTime.Minutes);
                pontoU.Usuario = new UsuarioVO() { Id = cboUsuario.Value.ToInt32() };

                if (pontoU.HoraInicio > pontoU.HoraTermino)
                {
                    base.MostrarMensagem("Erro", "horário inicial não pode ser superior ao final.", String.Empty);
                    return;
                }

                pontoU.Usuario.PontosUsuario = new PontoUsuarioBO().BuscarPontosDoDia(pontoU.Data, pontoU.Usuario);
                PontoUsuarioVO pontoAux = pontoU.Usuario.PontosUsuario.FirstOrDefault(x => pontoU.Id != x.Id && ((pontoU.HoraInicio >= x.HoraInicio && pontoU.HoraInicio <= x.HoraTermino) || (pontoU.HoraTermino >= x.HoraInicio && pontoU.HoraTermino <= x.HoraTermino) || (pontoU.HoraInicio <= x.HoraInicio && pontoU.HoraTermino >= x.HoraTermino)));
                if (pontoAux != null)
                {
                    base.MostrarMensagem("Erro", String.Format("Ponto de usuário conflitante Data => {0}. <br/>Horário inicial => {1}. <br/>Horário de término => {2}", pontoAux.Data.ToString("dd/MM/yyyy"), pontoAux.HoraInicio.ToString("HH:mm"), pontoAux.HoraTermino.Value.ToString("HH:mm")), String.Empty);
                    return;
                }

                DateTime dataMatu = pontoU.Data.AddHours(12);
                DateTime dataVesp = pontoU.Data.AddHours(18);
                PeriodoVO p = new PeriodoVO(){ Id = pontoU.HoraInicio <= dataMatu ? 1 : pontoU.HoraInicio <= dataVesp ? 2 : 3 };
                pontoU.Periodo = p;
                pontoU.Removido = false;
                pontoU.Justificativa = txtJustificativa.Text;

                new PontoUsuarioBO(pontoU).Salvar();

                base.MostrarMensagem("Sucesso", "Ponto de usuário gravado com sucesso.", String.Empty);
                
                LoadPagina();
                winPontosUsuario.Hide();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar ponto de usuário.", String.Empty);
            }            
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            PontoSelecionado = new PontoUsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtData.Text = PontoSelecionado.Data.ToString("dd/MM/yyyy");
            txtHoraInicial.Text = PontoSelecionado.HoraInicio.ToString("HH:mm");
			if (PontoSelecionado.HoraTermino.HasValue)
				txtHoraFinal.Text = PontoSelecionado.HoraTermino.Value.ToString("HH:mm");
			else
				txtHoraFinal.Clear();
            CarregarUsuarios(PontoSelecionado.Usuario);
            cboUsuario.SetValue(PontoSelecionado.Usuario.Id);
            txtJustificativa.Text = PontoSelecionado.Justificativa;
        }

        private void LimparCampos()
        {
            txtData.Clear();
            txtHoraInicial.Clear();
            txtHoraFinal.Clear();
            cboUsuario.Clear();
            txtJustificativa.Clear();
        }

        private void CarregarUsuarios(UsuarioVO usuario)
        {
            strUsuarios.DataSource = new UsuarioBO().Select().Where(x => (x.UsuarioSistema == true || (usuario != null && x.Id == usuario.Id)) ||  (x.Removido == false || (usuario != null && x.Id == usuario.Id))).ToList();
            strUsuarios.DataBind();
        }

        private void CarregarUsuariosBusca()
        {
            strUsuariosBusca.DataSource = new UsuarioBO().BuscarUsuariosSistema(false, true);
            strUsuariosBusca.DataBind();

            cboUsuariosBusca.Items.Insert(0, new Ext.Net.ListItem("[Todos]", String.Empty));
            cboUsuariosBusca.SelectedIndex = 0;
        }

        private void ExportaExcel(List<PontoUsuarioVO> list)
        {
            if (list.Count == 0)
            {
                this.MostrarMensagem("Alerta", "Não existem informações para serem exportadas.", String.Empty);
                return;
            }

            base.RemoverArquivosExistentes();

            Workbook workbook = new Workbook();
            FileStream stream = null;

            foreach (var group in list.GroupBy(x => x.Usuario))
            {
                Worksheet worksheet = new Worksheet("Relatório de Horas - "+ group.Key.Nome);
                String path = Path.Combine(Server.MapPath("~/ConfiguracoesSistema"), "1.jpg");

                if (File.Exists(path))
                {
                    stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    Picture pic = new Picture();
                    System.Drawing.Image imgLogo = new System.Drawing.Bitmap(stream);
                    pic.Image = new ExcelLibrary.SpreadSheet.Image(imageToByteArray(imgLogo), 0xF01D);

                    //Tamanho da imagem.
                    pic.TopLeftCorner = new CellAnchor(0, 0, 10, 10);
                    pic.BottomRightCorner = new CellAnchor(4, 2, 10, 10);

                    // Adiciona a imagem(logo).
                    worksheet.AddPicture(pic);
                }

                worksheet.Cells[2, 3] = new ExcelLibrary.SpreadSheet.Cell("");
                worksheet.Cells[2, 4] = new ExcelLibrary.SpreadSheet.Cell(DateTime.Now.ToString("dd/MM/yyyy"));

                int index = 0;
                String[] header = new String[] { "Data", "Início", "Término", "Justificativa", "Período", "Tempo" };

                index = 0;
                foreach (string strHeader in header)
                {
                    worksheet.Cells[6, index] = new ExcelLibrary.SpreadSheet.Cell(strHeader);
                    index++;
                }

                index = 0;
                int indexLinha = 7;

                foreach(var obj1 in group)
                {
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(obj1.Data.ToString("dd/MM/yyyy"));
                    index++;
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(obj1.HoraInicio.ToString("HH:mm"));
                    index++;
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(obj1.HoraTermino.HasValue ? obj1.HoraTermino.Value.ToString("HH:mm") : "");
                    index++;
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(obj1.Justificativa);
                    index++;
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(obj1.Periodo.Nome);
                    index++;
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(GetTempo(obj1.Tempo.ToInt32()));
                    index = 0;
                    indexLinha++;
                }

                Int32 min = group.Sum(x => x.Tempo).ToInt32();

                String tempo = GetTempo(min);

                worksheet.Cells[indexLinha, 4] = new ExcelLibrary.SpreadSheet.Cell("Total de Horas");
                worksheet.Cells[indexLinha, 5] = new ExcelLibrary.SpreadSheet.Cell(tempo);
                /// Adiciona o worksheet no workbook
                workbook.Worksheets.Add(worksheet);
            }

            // Nome do arquivo a ser gerado            
            String nomeAux = Guid.NewGuid().ToString() + "_" + UsuarioLogado.Id.ToString();
            String caminho = Path.Combine(Server.MapPath("~/temp"), nomeAux + ".xls");
            workbook.Save(caminho);

            if (stream != null)
                stream.Close();

            FileInfo arquivo = new FileInfo(caminho);
            if (arquivo.Exists)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=RelatorioHoras.xls");
                Response.AddHeader("Content-Length", arquivo.Length.ToString());
                Response.Flush();
                Response.WriteFile(caminho);
            }
        }

        private String GetTempo(Int32 min)
        {
            Int32 horas = 0;
            Int32 minutos = min.ToInt32();
            if (minutos > 60)
            {
                minutos = min % 60;
                horas = Convert.ToInt32(min / 60);
            }

            String strHora = horas.ToString().Length > 1 ? horas.ToString() : ("0" + horas);
            String strMinutos = minutos.ToString().Length > 1 ? minutos.ToString() : ("0" + minutos);
            return strHora + ":" + strMinutos;
        }

        private void VisualizarGrafico()
        {
            List<PontoUsuarioVO> pontos = BuscarPontos();
        }

        #endregion
    }
}
