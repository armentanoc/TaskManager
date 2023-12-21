using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Windows.Forms;
using System.Data.SQLite;
namespace TaskManager.UI
{
    public partial class StartScreen : Form
    {
        int exit = 3;
        public StartScreen()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            txtUser.Text = "";
            txtPassword.Text = "";
        }

        private void btnRegister_Click(object sender, System.EventArgs e)
        {
            Register register = new Register();
            register.Display();
        }

        private void btnEnter_Click(object sender, System.EventArgs e)
        {
            try
            {
                SQLiteConnection conexion_sqlite;
                SQLiteCommand cmd_sqlite;
                SQLiteDataReader datareader_sqlite;

                conexion_sqlite = new SQLiteConnection("Data Source=Logins2.db; Version=3;New=False;");
                conexion_sqlite.Open();

                cmd_sqlite = conexion_sqlite.CreateCommand();
                cmd_sqlite.CommandText = "SELECT * FROM validacion WHERE user ='" + txtUser.Text + "'AND password ='" + txtPassword.Text + "'";
                SQLiteDataReader Milector = cmd_sqlite.ExecuteReader();
                int cont = 0;
                string user;
                while (Milector.Read())
                {
                    cont++;
                }
                if (cont == 1)
                {
                    user = txtUser.Text;
                    exit = 3;
                    MessageBox.Show("Acceso Concedido.");
                    MainPage ventana3 = new MainPage(user);
                    ventana3.Show();
                    Hide();

                }
                else
                {
                    exit--;
                    MessageBox.Show("Usuario y/o Contraseña incorrectos,le quedan" + " " + exit + " " + "intentos");
                }
                if (exit == 0)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
