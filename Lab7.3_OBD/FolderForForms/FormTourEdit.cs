using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7._3_OBD
{
    public partial class FormTourEdit : Form
    {
        public FormTourEdit()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        Dictionary<string, string> dictType = new Dictionary<string, string>();
        Dictionary<string, string> dictTourID = new Dictionary<string, string>();
        Dictionary<string, string> dictLocationID = new Dictionary<string, string>();

        private void FormTourEdit_Load(object sender, EventArgs e)
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
                dictLocation.Add(readerLocation[0].ToString(), readerLocation[1].ToString());
            }
            readerLocation.Close();

            foreach (var item in dictLocation)
            {
                comboBoxLocation.Items.Add(item.Key);
            }

            string comandViewType = "SELECT * FROM Type_Of_Tour";

            SqlConnection connectionType = DataBase.ConnectionForDB();
            if (connectionType.State == ConnectionState.Closed)
                connectionType.Open();

            SqlCommand commandType = new SqlCommand(comandViewType, connectionType);
            SqlDataReader readerType = commandType.ExecuteReader();

            while (readerType.Read())
            {
                dictType.Add(readerType[1].ToString(), readerType[0].ToString());
                dictTourID.Add(readerType[0].ToString(), readerType[1].ToString());
            }

            readerType.Close();
            connectionType.Close();

            foreach (var item in dictType)
            {
                comboBoxType.Items.Add(item.Key);
            }
        }

        string ID_Tour;
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "SELECT * FROM Tour WHERE Name LIKE '%" + textBoxName.Text + "%'";

            SqlConnection connectionSearch = DataBase.ConnectionForDB();
            if (connectionSearch.State == ConnectionState.Closed)
                connectionSearch.Open();

            SqlCommand commandSeach = new SqlCommand(commandSearchStr, connectionSearch);
            SqlDataReader readerSearch = commandSeach.ExecuteReader();

            readerSearch.Read();

            string TourID;

            try 
            {
                TourID = readerSearch[0].ToString();
            }
            catch
            {
                readerSearch.Close();
                MessageBox.Show("Не знайдено!");
                return;
            }
            
            ID_Tour = readerSearch[0].ToString();
            textBoxName.Text = readerSearch[2].ToString();
            textBoxStartDate.Text = DataBase.DateToString(DateTime.Parse(readerSearch[5].ToString()));
            textBoxEndDate.Text = DataBase.DateToString(DateTime.Parse(readerSearch[6].ToString()));
            textBoxDescription.Text = readerSearch[4].ToString();
            comboBoxType.SelectedItem = dictTourID[readerSearch[1].ToString()];
            comboBoxLocation.SelectedItem = dictLocation[readerSearch[3].ToString()];


            readerSearch.Close();

            string commandCostStr = "SELECT * FROM Tour_Payment WHERE ID_Tour = '" + TourID + "'";
            SqlCommand commandCost = new SqlCommand(commandCostStr, connectionSearch);
            SqlDataReader readerCost = commandCost.ExecuteReader();

            readerCost.Read();

            textBoxCost.Text = float.Parse(readerCost[1].ToString()).ToString("0.00");

            readerCost.Close();

            connectionSearch.Close();
        }

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            string commandSearchStr = "UPDATE Tour ";
            commandSearchStr += "SET Name = @Name, ID_Type_Of_Tour = @Type, ID_Location = @ID_Location, Description = @Description, Start_Date = @Start_Date, End_Date = @End_Date ";
            commandSearchStr += "WHERE ID_Tour = @ID_Tour";

            string commandCostStr = "UPDATE Tour_Payment ";
            commandCostStr += "SET Tour_Cost = @Tour_Cost ";
            commandCostStr += "WHERE ID_Tour = @ID_Tour";

            SqlConnection connectionSearch = DataBase.ConnectionForDB();
            if (connectionSearch.State == ConnectionState.Closed)
                connectionSearch.Open();

            SqlCommand command = new SqlCommand(commandSearchStr, connectionSearch);
            command.Parameters.AddWithValue("@Name", textBoxName.Text);
            command.Parameters.AddWithValue("@ID_Location", dictLocation[comboBoxLocation.Text]);
            command.Parameters.AddWithValue("@Type", dictType[comboBoxType.Text]);
            command.Parameters.AddWithValue("@Description", textBoxDescription.Text);
            command.Parameters.AddWithValue("@Start_Date", textBoxStartDate.Text);
            command.Parameters.AddWithValue("@End_Date", textBoxEndDate.Text);
            command.Parameters.AddWithValue("@ID_Tour", ID_Tour);

            SqlCommand commandCost = new SqlCommand(commandCostStr, connectionSearch);
            commandCost.Parameters.AddWithValue("@Tour_Cost", float.Parse(textBoxCost.Text));
            commandCost.Parameters.AddWithValue("@ID_Tour", ID_Tour);

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
