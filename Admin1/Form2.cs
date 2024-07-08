using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Admin1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();


        }
        string connectionString = "server=DESKTOP-8ELEIVF\\SQLEXPRESS01;database=BD_PIZZERIA_TURBO; integrated security=true";
        SqlConnection conexion;

        private void Form2_Load(object sender, EventArgs e)
        {
            conexion = new SqlConnection(connectionString);

            CargarClientesYOrdenes();
            ConfigurarDataGridView();
            CargarClientesParaEliminar();
        }
        #region
        private void button1_Click(object sender, EventArgs e)
        {
        }
        #endregion
        private void CargarClientesYOrdenes()
        {
            try
            {
                // Abrir la conexión
                conexion.Open();

                // Consulta SQL para obtener clientes y sus pedidos
                string consulta = @"
                    SELECT 
                        Clientes.Cliente AS NombreCliente, 
                        Orden.Numero_Orden AS NumeroOrden,
                        Orden.Pizza_Detalle AS PizzaDetalle,
                        Orden.CantidadPizza AS CantidadPizza,
                        Orden.Bebida_Detalle AS BebidaDetalle,
                        Orden.CantidadBebida AS CantidadBebida,
                        Orden.Postre_Detalle AS PostreDetalle,
                        Orden.CantidadPostre AS CantidadPostre
                    FROM Clientes
                    LEFT JOIN Orden ON Clientes.IdCliente = Orden.xClienteId";

                // Crear un SqlDataAdapter para ejecutar la consulta y llenar un DataTable
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                // Asignar el DataTable como origen de datos para el DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes y sus pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cerrar la conexión en el bloque finally para asegurar que se cierre correctamente
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        #region
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        #endregion
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                LimpiarTextBoxTabPage2();
                LimpiarDataGridView();


            }
        }

        private void LimpiarDataGridView()
        {
            dataGridView2.Rows.Clear();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            BuscarClienteYMostrarDetalles();
        }


        private void BuscarClienteYMostrarDetalles()
        {
            int idCliente;
            if (!int.TryParse(textBox1.Text, out idCliente))
            {
                MessageBox.Show("Ingrese un ID de cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();

                string consulta = @"
                    SELECT 
                        Clientes.Cliente AS NombreCliente,
                        Clientes.Teléfono AS Telefono,
                        Clientes.Dirección AS Direccion,
                        Orden.Pizza_Detalle AS PizzaDetalle,
                        Orden.Bebida_Detalle AS BebidaDetalle,
                        Orden.Postre_Detalle AS PostreDetalle
                    FROM Clientes
                    LEFT JOIN Orden ON Clientes.IdCliente = Orden.xClienteId
                    WHERE Clientes.IdCliente = @IdCliente";

                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Mostrar los datos en los TextBox correspondientes
                    textBox2.Text = reader["NombreCliente"].ToString();
                    textBox3.Text = reader["Telefono"].ToString();
                    textBox4.Text = reader["Direccion"].ToString();
                    textBox11.Text = reader["PizzaDetalle"].ToString();
                    textBox12.Text = reader["BebidaDetalle"].ToString();
                    textBox13.Text = reader["PostreDetalle"].ToString();

                    // Llenar el DataGridView dataGridView2 con los datos del cliente encontrado
                    dataGridView2.Rows.Clear();
                    dataGridView2.Rows.Add(
                        reader["NombreCliente"].ToString(),
                        reader["Telefono"].ToString(),
                        reader["Direccion"].ToString(),
                        reader["PizzaDetalle"].ToString(),
                        reader["BebidaDetalle"].ToString(),
                        reader["PostreDetalle"].ToString()
                    );
                }
                else
                {
                    MessageBox.Show("No se encontró información para el cliente seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarTextBoxTabPage2();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar y cargar los datos del cliente y su orden: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        private void LimpiarTextBoxTabPage2()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         

        }

        private void ConfigurarDataGridView()
        {
            dataGridView2.Columns.Clear();

            // Definir las columnas necesarias para dataGridView2
            dataGridView2.Columns.Add("NombreCliente", "Nombre Cliente");
            dataGridView2.Columns.Add("Telefono", "Teléfono");
            dataGridView2.Columns.Add("Direccion", "Dirección");
            dataGridView2.Columns.Add("PizzaDetalle", "Detalle Pizza");
            dataGridView2.Columns.Add("BebidaDetalle", "Detalle Bebida");
            dataGridView2.Columns.Add("PostreDetalle", "Detalle Postre");

            dataGridView2.Columns["NombreCliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.AutoGenerateColumns = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Validar que haya una fila seleccionada en dataGridView2
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idCliente;
            if (!int.TryParse(textBox1.Text, out idCliente))
            {
                MessageBox.Show("Ingrese un ID de cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();

                string updateCliente = @"
                    UPDATE Clientes
                    SET Cliente = @NombreCliente,
                        Teléfono = @Telefono,
                        Dirección = @Direccion
                    WHERE IdCliente = @IdCliente";

                SqlCommand cmdUpdate = new SqlCommand(updateCliente, conexion);
                cmdUpdate.Parameters.AddWithValue("@NombreCliente", textBox2.Text);
                cmdUpdate.Parameters.AddWithValue("@Telefono", textBox3.Text);
                cmdUpdate.Parameters.AddWithValue("@Direccion", textBox4.Text);
                cmdUpdate.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdUpdate.ExecuteNonQuery();

                string updateOrden = @"
                    UPDATE Orden
                    SET Pizza_Detalle = @PizzaDetalle,
                        Bebida_Detalle = @BebidaDetalle,
                        Postre_Detalle = @PostreDetalle
                    WHERE xClienteId = @IdCliente";

                SqlCommand cmdUpdateOrden = new SqlCommand(updateOrden, conexion);
                cmdUpdateOrden.Parameters.AddWithValue("@PizzaDetalle", textBox11.Text);
                cmdUpdateOrden.Parameters.AddWithValue("@BebidaDetalle", textBox12.Text);
                cmdUpdateOrden.Parameters.AddWithValue("@PostreDetalle", textBox13.Text);
                cmdUpdateOrden.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdUpdateOrden.ExecuteNonQuery();

                MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar los datos del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();

                // Recargar los datos actualizados
                CargarClientesYOrdenes();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BuscarClienteYMostrarDetalles();
        }


        private void CargarClientesParaEliminar()
        {
            try
            {
                // Verificar si la conexión ya está abierta antes de abrir de nuevo
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
            SELECT 
                Clientes.IdCliente,
                Clientes.Cliente AS NombreCliente,
                Users.Usuario AS NombreUsuario,
                Orden.Numero_Orden AS NumeroOrden
            FROM Clientes
            LEFT JOIN Users ON Clientes.IdCliente = Users.XCli
            LEFT JOIN Orden ON Clientes.IdCliente = Orden.xClienteId";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                dataGridView3.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes y sus usuarios para eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Asegurarse de cerrar la conexión en el bloque finally
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EliminarClienteUsuarioOrden();
        }


        private void EliminarClienteUsuarioOrden()
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Ingrese el ID del cliente a eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idCliente;
            if (!int.TryParse(textBox6.Text, out idCliente))
            {
                MessageBox.Show("Ingrese un ID de cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();

                // Eliminar usuario asociado al cliente
                SqlCommand cmdEliminarUsuario = new SqlCommand("DELETE FROM Users WHERE XCli = @IdCliente", conexion);
                cmdEliminarUsuario.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdEliminarUsuario.ExecuteNonQuery();

                // Eliminar orden asociada al cliente
                SqlCommand cmdEliminarOrden = new SqlCommand("DELETE FROM Orden WHERE xClienteId = @IdCliente", conexion);
                cmdEliminarOrden.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdEliminarOrden.ExecuteNonQuery();

                // Eliminar cliente
                SqlCommand cmdEliminarCliente = new SqlCommand("DELETE FROM Clientes WHERE IdCliente = @IdCliente", conexion);
                cmdEliminarCliente.Parameters.AddWithValue("@IdCliente", idCliente);
                cmdEliminarCliente.ExecuteNonQuery();

                MessageBox.Show("Cliente y sus registros asociados han sido eliminados correctamente.", "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar los datos en dataGridView3 después de la eliminación
                CargarClientesParaEliminar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente y sus registros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BuscarClientePorId();
        }

        private void BuscarClientePorId()
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Ingrese el ID del cliente a buscar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idCliente;
            if (!int.TryParse(textBox6.Text, out idCliente))
            {
                MessageBox.Show("Ingrese un ID de cliente válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Abrir conexión
                conexion.Open();

                string consulta = @"
            SELECT 
                Clientes.IdCliente,
                Clientes.Cliente AS NombreCliente,
                Users.Usuario AS NombreUsuario,
                Orden.Numero_Orden AS NumeroOrden
            FROM Clientes
            LEFT JOIN Users ON Clientes.IdCliente = Users.XCli
            LEFT JOIN Orden ON Clientes.IdCliente = Orden.xClienteId
            WHERE Clientes.IdCliente = @IdCliente";

                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                // Asignar resultado al dataGridView3
                dataGridView3.DataSource = dt;

                // Mostrar datos en los TextBox correspondientes si se encontró el cliente
                if (dt.Rows.Count > 0)
                {
                    textBox7.Text = dt.Rows[0]["NombreCliente"].ToString();
                    textBox8.Text = dt.Rows[0]["NombreUsuario"].ToString();
                    textBox9.Text = dt.Rows[0]["NumeroOrden"].ToString();
                }
                else
                {
                    // Limpiar TextBox si no se encontró información
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    MessageBox.Show("No se encontró información para el cliente seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cerrar conexión en el bloque finally
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LimpiarCampos();    
        }


        private void LimpiarCampos()
        {
            textBox6.Clear();   // Limpiar el campo de búsqueda por ID
            textBox7.Clear();   // Limpiar NombreCliente
            textBox8.Clear();   // Limpiar NombreUsuario
            textBox9.Clear();   // Limpiar NumeroOrden
            textBox10.Clear();  // Puedes limpiar otros campos si es necesario

            dataGridView3.DataSource = null;  // Limpiar el DataGridView
        }
    }
}



