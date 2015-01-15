using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IntranetBettaInformatica.DataAccess;

namespace IntranetBettaInformatica.PopulateTables
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Db.ExportDataBase();
            //Insere configurações do sistema
            ConfiguracoesSistemaCarga.DispararCargaBanco();

            // Insere estados
            EstadoCarga.DispararCargaBanco();

            // Insere temas
            TemaCarga.DispararCargaBanco();

            // Insere sistemas
            SistemaCarga.DispararCargaBanco();

			// Insere perfis
            MenuPaginaCarga.DispararCargaBanco();

			// Insere ações
			AcaoCarga.DispararCargaBanco();

            // Insere perfis
            PerfilAcessoCarga.DispararCargaBanco();

            // Insere tipos de empresa
            TipoEmpresaCarga.DispararCargaBanco();

            // Insere empresa
            EmpresaCarga.DispararCargaBanco();

            // Insere usuarios
            UsuarioCarga.DispararCargaBanco();

            // Inserer períodos
            PeriodoCarga.DispararCargaBanco();
        }
    }
}
