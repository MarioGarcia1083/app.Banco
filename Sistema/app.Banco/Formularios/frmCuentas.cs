using app.Banco.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.Banco.Formularios
{
    public partial class frmCuentas : Form
    {
        public frmCuentas()
        {
            InitializeComponent();
        }

        #region Métodos

        private void ListarRegistros()
        {
            try
            {
                string connetionString = ConexionDB.ObtenerConexion();

                using (SqlConnection conexion = new SqlConnection(connetionString))
                {
                    string consulta = @"
                        SELECT
                            idCuenta AS id,
                            idCliente AS Cliente,
                            idUsuario AS Usuario,
                            saldoInicial AS [Saldo Inicial],
                            saldoActual AS [Saldo Actual],
                            estadoCuenta AS Estado
                        FROM cuenta";

                    SqlDataAdapter adapter = new SqlDataAdapter(consulta, conexion);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvListado.DataSource = dt;
                    dgvListado.Columns[0].Visible = false;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        #endregion

        #region Eventos del Formulario

        private void frmCuentas_Load(object sender, EventArgs e)
        {
            ListarRegistros();
        }


        #endregion

        #region Botones de Comando

        private void iconCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
