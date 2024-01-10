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

namespace Lab7._3_OBD
{
    public partial class FormClientNew : Form
    {
        public FormClientNew()
        {
            InitializeComponent();
        }

        private void FormClientNew_Load(object sender, EventArgs e)
        {

        }

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("AddClient", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@ID_Client", SqlDbType.VarChar).Value = DataBase.ValueIDClient();
            command.Parameters.Add("@First_Name", SqlDbType.VarChar).Value = textBoxFirstName.Text;
            command.Parameters.Add("@Second_Name", SqlDbType.VarChar).Value = textBoxSecondName.Text;
            command.Parameters.Add("@Phone_Number", SqlDbType.VarChar).Value = textBoxPhoneNumber.Text;
            command.Parameters.Add("@Passport_Information", SqlDbType.VarChar).Value = textBoxPassport.Text;

            connection.Open();

            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                MessageBox.Show("Запис успішно додано!");
            }
            catch
            {
                MessageBox.Show("Направильний запис!");
            }
         
            connection.Close();
        }
    }
}
