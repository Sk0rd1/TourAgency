using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7._3_OBD
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ForeColor = Color.Red;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            FormTourAgency formTourAgency = new FormTourAgency();
            bool isConnected = DataBase.Connection(textBoxLogin.Text, textBoxPassword.Text);

            if (isConnected)
            {
                formTourAgency.WriteUserID(textBoxLogin.Text);
                formTourAgency.Show();
                Hide();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();

            foreach (Form form in openForms)
            {
                if (form != null) form.Close();
            }

            Application.Exit();
        }
    }
}