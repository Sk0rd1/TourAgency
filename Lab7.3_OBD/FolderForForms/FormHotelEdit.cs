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
    public partial class FormHotelEdit : Form
    {
        public FormHotelEdit()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        Dictionary<string, string> dictLocationID = new Dictionary<string, string>();

        string ID_Hotel;
        private void FormHotelEdit_Load(object sender, EventArgs e)
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
                dictLocationID.Add(readerLocation[0].ToString(), readerLocation[1].ToString());
            }
            readerLocation.Close();

            foreach (var item in dictLocation)
            {
                comboBoxLocation.Items.Add(item.Key);
            }
            connectionLocation.Close();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "SELECT * FROM Hotel WHERE Name LIKE '%" + textBoxName.Text + "%'";

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

            ID_Hotel = readerSearch[0].ToString();
            textBoxName.Text = readerSearch[1].ToString();
            textBoxDescription.Text = readerSearch[3].ToString();
            comboBoxLocation.SelectedItem = dictLocationID[readerSearch[2].ToString()];


            readerSearch.Close();

            string commandCostStr = "SELECT * FROM Hotel_Payment WHERE ID_Hotel = '" + HotelID + "'";
            SqlCommand commandCost = new SqlCommand(commandCostStr, connectionSearch);
            SqlDataReader readerCost = commandCost.ExecuteReader();

            readerCost.Read();

            textBoxCost.Text = float.Parse(readerCost[1].ToString()).ToString("0.00");

            readerCost.Close();

            connectionSearch.Close();
        }

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "UPDATE Hotel ";
            commandSearchStr += "SET Name = @Name, ID_Location = @ID_Location, Description = @Description ";
            commandSearchStr += "WHERE ID_Hotel = @ID_Hotel";

            string commandCostStr = "UPDATE Hotel_Payment ";
            commandCostStr += "SET Hotel_Cost = @Hotel_Cost ";
            commandCostStr += "WHERE ID_Hotel = @ID_Hotel";

            SqlConnection connectionSearch = DataBase.ConnectionForDB();
            if (connectionSearch.State == ConnectionState.Closed)
                connectionSearch.Open();

            SqlCommand command = new SqlCommand(commandSearchStr, connectionSearch);
            command.Parameters.AddWithValue("@Name", textBoxName.Text);
            command.Parameters.AddWithValue("@ID_Location", dictLocation[comboBoxLocation.Text]);
            command.Parameters.AddWithValue("@Description", textBoxDescription.Text);
            command.Parameters.AddWithValue("@ID_Hotel", ID_Hotel);

            SqlCommand commandCost = new SqlCommand(commandCostStr, connectionSearch);
            commandCost.Parameters.AddWithValue("@Hotel_Cost", float.Parse(textBoxCost.Text));
            commandCost.Parameters.AddWithValue("@ID_Hotel", ID_Hotel);

            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                rowsAffected = commandCost.ExecuteNonQuery();

                MessageBox.Show("Запис оновлено!");
            }
            catch
            {
                MessageBox.Show("Помилка!");
            }
            connectionSearch.Close();
        }
    }
}
