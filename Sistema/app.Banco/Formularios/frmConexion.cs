using app.Banco.Utilidades;
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
    public partial class frmConexion : Form
    {
        public frmConexion()
        {
            InitializeComponent();
        }

        #region Botones de comando

        private void iconCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void iconAceptar_Click(object sender, EventArgs e)
        {
            errorIcono.Clear();
            bool datosValidos = true;

            foreach(Control control in tableLayoutPanel1.Controls)
            {
                if(control is Guna.UI2.WinForms.Guna2TextBox gunaTextBox)
                {
                    if(string.IsNullOrWhiteSpace(gunaTextBox.Text))
                    {
                        errorIcono.SetError(gunaTextBox, "Este campo es obligatorio.");
                        datosValidos = false;
                    }
                }
            }

            if(!datosValidos)
            {
                MessageBox.Show("Información incompleta, serán remarcados lo datos que faltan.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Crear objeto de configuración
            var parametros = new ParametrosConexion
            {
                servidor = txtServidor.Text.Trim(),
                baseDatos = txtBaseDatos.Text.Trim()
            };

            if(!AdminstrarConexion.ProbarConexion(parametros, out string error))
            {
                MessageBox.Show($"No se pudo establecer la conexión con la base de datos.\n\nDetalles: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            try
            {
                AdminstrarConexion.Guardar(parametros);
                MessageBox.Show("Datos de conexión registrados con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);
            }
        }

        #endregion
    }
}
