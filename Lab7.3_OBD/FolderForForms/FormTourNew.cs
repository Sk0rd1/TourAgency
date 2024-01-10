using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7._3_OBD
{
    public partial class FormTourNew : Form
    {
        public FormTourNew()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        Dictionary<string, string> dictType = new Dictionary<string, string>();

        private string DateToString(DateTime dateTime)
        {
            string result;

            result = dateTime.Year.ToString();

            if (dateTime.Month < 10)
                result += "-0" + dateTime.Month.ToString();
            else
                result += "-" + dateTime.Month.ToString();

            if (dateTime.Day < 10)
                result += "-0" + dateTime.Day.ToString();
            else
                result += "-" + dateTime.Day.ToString();
            return result;
        }

        private void FormTourNew_Load(object sender, EventArgs e)
        {
            string comandViewLocation = "SELECT * FROM [Location]";

            SqlConnection connectionLocation = DataBase.ConnectionForDB();
            if (connectionLocation.State == ConnectionState.Closed)
                connectionLocation.Open();

            SqlCommand commandLocation = new SqlCommand(comandViewLocation, connectionLocation);
            SqlDataReader readerLocation = commandLocation.ExecuteReader();

            while(readerLocation.Read())
            {
                dictLocation.Add(readerLocation[1].ToString(), readerLocation[0].ToString());
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

            while(readerType.Read())
            {
                dictType.Add(readerType[1].ToString(), readerType[0].ToString());
            }

            readerType.Close();
            connectionType.Close();

            foreach (var item in dictType)
            {
                comboBoxType.Items.Add(item.Key);
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonTourAdd_Click(object sender, EventArgs e)
        {
            string valueType, valueLocation;

            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("AddTour", connection);

            command.CommandType = CommandType.StoredProcedure;
            if (dictType.TryGetValue(comboBoxType.Text, out valueType) && dictLocation.TryGetValue(comboBoxLocation.Text, out valueLocation))
            {
                command.Parameters.Add("@ID_Tour", SqlDbType.VarChar).Value = DataBase.ValueIDTour();
                command.Parameters.Add("@ID_Type_Of_Tour", SqlDbType.VarChar).Value = valueType;
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = textBoxName.Text;
                command.Parameters.Add("@ID_Location", SqlDbType.VarChar).Value = valueLocation;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = textBoxDescription.Text;
                command.Parameters.Add("@Start_Date", SqlDbType.Date).Value = DateToString(dateTimePickerStart.Value);
                command.Parameters.Add("@End_Date", SqlDbType.Date).Value = DateToString(dateTimePickerEnd.Value);
                command.Parameters.Add("@Tour_Cost", SqlDbType.Money).Value = textBoxCost.Text;
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
    }
}
