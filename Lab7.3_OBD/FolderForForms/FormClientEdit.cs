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
    public partial class FormClientEdit : Form
    {
        public FormClientEdit()
        {
            InitializeComponent();
        }

        private void FormClientEdit_Load(object sender, EventArgs e)
        {

        }

        string ID_Client;
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "SELECT * FROM Client WHERE Phone_Number LIKE '%" + textBoxName.Text + "%'";

            SqlConnection connectionSearch = DataBase.ConnectionForDB();
            if (connectionSearch.State == ConnectionState.Closed)
                connectionSearch.Open();

            SqlCommand commandSeach = new SqlCommand(commandSearchStr, connectionSearch);
            SqlDataReader readerSearch = commandSeach.ExecuteReader();

            string HotelID;
            readerSearch.Read();
            try
            {
                HotelID = readerSearch[0].ToString();
            }
            catch
            {
                readerSearch.Close();
                MessageBox.Show("Не знайдено!");
                return;
            }

            ID_Client = readerSearch[0].ToString();
            textBoxFirstName.Text = readerSearch[1].ToString();
            textBoxSecondName.Text = readerSearch[2].ToString();
            textBoxPassport.Text = readerSearch[4].ToString();
            textBoxName.Text = readerSearch[3].ToString();
            //comboBoxLocation.SelectedItem = dictLocationID[readerSearch[2].ToString()];


            readerSearch.Close();

            connectionSearch.Close();
        }

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "UPDATE Client ";
            commandSearchStr += "SET First_Name = @First_Name, Second_Name = @Second_Name, Phone_Number = @Phone_Number, Passport_Information = @Passport_Information ";
            commandSearchStr += "WHERE ID_Client = @ID_Client";

            SqlConnection connectionSearch = DataBase.ConnectionForDB();
            if (connectionSearch.State == ConnectionState.Closed)
                connectionSearch.Open();

            SqlCommand command = new SqlCommand(commandSearchStr, connectionSearch);
            command.Parameters.AddWithValue("@First_Name", textBoxFirstName.Text);
            command.Parameters.AddWithValue("@Second_Name", textBoxSecondName.Text);
            command.Parameters.AddWithValue("@Phone_Number", textBoxName.Text);
            command.Parameters.AddWithValue("@Passport_Information", textBoxPassport.Text);
            command.Parameters.AddWithValue("@ID_Client", ID_Client);

            try
            {
                command.ExecuteNonQuery();

                MessageBox.Show("Запис оновлено!");
            }
            catch
            {
                MessageBox.Show("Помилка!");
            }
            connectionSearch.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            string deleteStr = "DELETE FROM Client WHERE Phone_Number = '" + textBoxName.Text + "'";

            SqlCommand command = new SqlCommand(deleteStr, connection);

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Запис видалено!");
                textBoxName.Text = string.Empty;
                textBoxFirstName.Text = string.Empty;
                textBoxSecondName.Text = string.Empty;
                textBoxPassport.Text = string.Empty;
            }
            catch
            {
                MessageBox.Show("Помилка!");
            }

            connection.Close();
        }
    }
}
