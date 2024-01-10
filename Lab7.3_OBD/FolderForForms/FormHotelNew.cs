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
    public partial class FormHotelNew : Form
    {
        public FormHotelNew()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            string valueLocation;

            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("AddHotel", connection);

            command.CommandType = CommandType.StoredProcedure;
            if (dictLocation.TryGetValue(comboBoxLocation.Text, out valueLocation))
            {
                command.Parameters.Add("@ID_Hotel", SqlDbType.VarChar).Value = DataBase.ValueIDHotel();
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = textBoxName.Text;
                command.Parameters.Add("@ID_Location", SqlDbType.VarChar).Value = valueLocation;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = textBoxDescription.Text;
                command.Parameters.Add("@Hotel_Cost", SqlDbType.Money).Value = textBoxCost.Text;
            }

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

        private void FormHotelNew_Load(object sender, EventArgs e)
        {
            string comandViewLocation = "SELECT * FROM [Location]";

            SqlConnection connectionLocation = DataBase.ConnectionForDB();
            if (connectionLocation.State == ConnectionState.Closed)
                connectionLocation.Open();

            SqlCommand commandLocation = new SqlCommand(comandViewLocation, connectionLocation);
            SqlDataReader readerLocation = commandLocation.ExecuteReader();

            while (readerLocation.Read())
            {
                dictLocation.Add(readerLocation[1].ToString(), readerLocation[0].ToString());
            }
            readerLocation.Close();
            connectionLocation.Close();

            foreach (var item in dictLocation)
            {
                comboBoxLocation.Items.Add(item.Key);
            }
        }
    }
}
