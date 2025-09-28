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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace app.Banco.Formularios
{
    public partial class frmusuarios : Form
    {
        public frmusuarios()
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
                            idUsuario AS id,
                            nombreUsuario AS Usuario,
                            telefonoUsuario AS Teléfono,
                            emailUsuario AS Email
                        FROM usuario";

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

        private void frmusuarios_Load(object sender, EventArgs e)
        {
            ListarRegistros();
        }

        #endregion

        #region Botones de Comando

        private void iconAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarUsuario frm = new frmAgregarUsuario();
            frm.registroAgregado += ListarRegistros;
            MostrarModal.MostrarConCapa(this, frm);
        }

        private void iconEliminar_Click(object sender, EventArgs e)
        {
            if(dgvListado.CurrentRow != null)
            {
                try
                {
                    if(MessageBox.Show("¿Seguro que desea eliminar el registro?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int.TryParse(dgvListado.CurrentRow.Cells["Id"].Value.ToString(), out int idUsuario);
                        string connetionString = ConexionDB.ObtenerConexion();

                        using (SqlConnection conexion = new SqlConnection(connetionString))
                        {
                            string consulta = "DELETE FROM usuario WHERE idUsuario = @IdUsuario";

                            SqlCommand command = new SqlCommand(consulta, conexion);
                            command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                            conexion.Open();

                            int resultado = command.ExecuteNonQuery();
                            if (resultado > 0)
                            {
                                MessageBox.Show("Registro eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el registro.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            ListarRegistros();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        #endregion

        #region Eventos del DataGridView

        private void dgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int id = Convert.ToInt32(dgvListado.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                    string usuario = dgvListado.Rows[e.RowIndex].Cells["Usuario"].Value.ToString();
                    string telefono = dgvListado.Rows[e.RowIndex].Cells["Teléfono"].Value.ToString();
                    string email = dgvListado.Rows[e.RowIndex].Cells["Email"].Value.ToString();

                    frmAgregarUsuario frm = new frmAgregarUsuario(id, usuario, telefono, email);
                    frm.registroAgregado += ListarRegistros;
                    MostrarModal.MostrarConCapa(this, frm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        #endregion

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text.Trim();

            try
            {
                string connetionString = ConexionDB.ObtenerConexion();

                using (SqlConnection conexion = new SqlConnection(connetionString))
                {
                    string consulta = $@"
                        SELECT
                            idUsuario AS id,
                            nombreUsuario AS Usuario,
                            telefonoUsuario AS Teléfono,
                            emailUsuario AS Email
                        FROM usuario
                        WHERE nombreUsuario LIKE '%{nombre}%'
                        OR telefonoUsuario LIKE '%{nombre}%'";

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
    }
}
