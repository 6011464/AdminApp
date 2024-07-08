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

namespace Admin1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = "server=DESKTOP-8ELEIVF\\SQLEXPRESS01;database=Empresa; integrated security=true";


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string usuario = textBox1.Text.Trim();
            string contraseña = textBox2.Text; // No es necesario trim() aquí

            // Consulta SQL para validar el usuario y contraseña
            string consulta = "SELECT * FROM Usuario WHERE usuario = @usuario";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Crear comando SQL con parámetros
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@usuario", usuario);

                        // Ejecutar consulta y obtener resultado
                        SqlDataReader lector = comando.ExecuteReader();

                        if (lector.Read())
                        {
                            string contraseñaBaseDatos = lector["contraseña"].ToString();

                            // Verificar la contraseña (deberías usar una técnica de hash segura aquí)
                            if (contraseña == contraseñaBaseDatos)
                            {
                                MessageBox.Show("Bienvenido");

                                // Si las credenciales son válidas, abrir el siguiente formulario
                                Form2 frmbienvenido = new Form2();
                                this.Hide();
                                frmbienvenido.Show();
                            }
                            else
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos");
                        }

                        lector.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}