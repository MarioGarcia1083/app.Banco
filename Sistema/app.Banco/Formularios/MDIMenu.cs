using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.Banco.Formularios
{
    public partial class MDIMenu : Form
    {
        private Form formularioActivo = null;

        public MDIMenu()
        {
            InitializeComponent();
        }

        #region Metodos

        private void AbrirFormulario(Form formularioHijo, bool esHijoDelPanelContenedor = true)
        {
            try
            {
                if (esHijoDelPanelContenedor)
                {
                    if (formularioActivo != null)
                    {
                        formularioActivo.Close();
                        formularioActivo.Dispose();
                    }

                    formularioActivo = formularioHijo;

                    formularioHijo.TopLevel = false;
                    formularioHijo.FormBorderStyle = FormBorderStyle.None;
                    formularioHijo.Dock = DockStyle.Fill;

                    panelContenedor.Controls.Clear();
                    panelContenedor.Controls.Add(formularioHijo);
                    panelContenedor.Tag = formularioHijo;

                    formularioHijo.Show();
                    formularioHijo.BringToFront();
                }
                else
                {
                    formularioHijo.TopLevel = true;
                    formularioHijo.FormBorderStyle = FormBorderStyle.Sizable;

                    formularioHijo.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Se ha generado un error inesperado al cargar el formulario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Opciones del menú

        private void listadoDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmClientes(), true);
        }

        private void listadoDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmusuarios(), true);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmCuentas(), true);
        }

        #endregion
    }
}
