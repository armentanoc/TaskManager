using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace TaskManager.UI
{
    public partial class MainPage : Form
    {
        string usuario;
        public MainPage(string user)
        {
            InitializeComponent();
            usuario = user;
            lblBienvenido.Text = "Bienvenido" + " " + usuario;

        }

        private void btnExitC(object sender, EventArgs e)
        {
            Close();
            StartScreen startscreen = new StartScreen();
            startscreen.Show();
        }
    }
}
