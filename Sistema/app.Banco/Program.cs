using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Banco.Formularios;
using app.Banco.Utilidades;

namespace app.Banco
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var conexon = AdminstrarConexion.Cargar();
            if(string.IsNullOrWhiteSpace(conexon.servidor) || string.IsNullOrWhiteSpace(conexon.baseDatos))
            {
                using (var frm = new frmConexion())
                {
                    if(frm.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show("No se configuró la conexión. La aplicación se cerrará.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            Application.Run(new MDIMenu());
        }
    }
}
