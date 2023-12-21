using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.UI
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

            try
            {
                SQLiteConnection conexion_sqlite;
                SQLiteCommand cmd_sqlite;
                SQLiteDataReader dataReader_Sqlite;

                conexion_sqlite = new SQLiteConnection("Data Source=Logins2.db;Version=3;New=False;");

                conexion_sqlite.Open();

                cmd_sqlite = conexion_sqlite.CreateCommand();
                //cmd_sqlite.CommandText = "INSERT INTO validation(user, password) VALUES ('" + txtRuser.Text + "', '" + txtRPassword.Text + "');";
                cmd_sqlite.ExecuteNonQuery();
                MessageBox.Show("registro exitoso.");
                cmd_sqlite.CommandText = "";
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
