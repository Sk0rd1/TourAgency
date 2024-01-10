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
using System.Windows.Input;

namespace Lab7._3_OBD
{
    public partial class FormReservationNew : Form
    {
        public FormReservationNew()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        Dictionary<string, string> dictType = new Dictionary<string, string>();

        Dictionary<string, string> dictTour = new Dictionary<string, string>();
        Dictionary<string, string> dictHotel = new Dictionary<string, string>();
        Dictionary<string, string> dictAirline = new Dictionary<string, string>();

        private void FormReservationNew_Load(object sender, EventArgs e)
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
            }

            readerType.Close();
            connectionType.Close();

            foreach (var item in dictType)
            {
                comboBoxType.Items.Add(item.Key);
            }
        }

        private string DateToString(DateTime dateTime)
        {
            string result;

            result =  dateTime.Year.ToString();

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

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            string startDate = DateToString(dateTimePickerStart.Value);
            string endDate = DateToString(dateTimePickerEnd.Value);
            string comandTour;
            try
            {
                comandTour = "SELECT * FROM Tour WHERE ID_Type_Of_Tour = '" + dictType[comboBoxType.Text] + "'" + " AND ID_Location = '" + dictLocation[comboBoxLocation.Text] + "'" + " AND Start_Date < '" + startDate + "' AND End_Date > '" + endDate + "'";
            }
            catch
            {
                MessageBox.Show("Заповніть поля!");
                return;
            }

            SqlCommand commandTour = new SqlCommand(comandTour, connection);
            SqlDataReader readerTour = commandTour.ExecuteReader();

            while (readerTour.Read()) 
            {
                dictTour.Add(readerTour[2].ToString(), readerTour[0].ToString());
                comboBoxTour.Items.Add(readerTour[2].ToString());
            }
            readerTour.Close();

            string comandHotel = "SELECT * FROM Hotel WHERE ID_Location = '" + dictLocation[comboBoxLocation.Text] + "'";

            SqlCommand commandHotel = new SqlCommand(comandHotel, connection);
            SqlDataReader readerHotel = commandHotel.ExecuteReader();

            while (readerHotel.Read())
            {
                dictHotel.Add(readerHotel[1].ToString(), readerHotel[0].ToString());
                comboBoxHotel.Items.Add(readerHotel[1].ToString());
            }
            readerHotel.Close();

            string comandAirline = "SELECT * FROM Airline WHERE ID_Location = '" + dictLocation[comboBoxLocation.Text] + "'";

            SqlCommand commandAirline = new SqlCommand(comandAirline, connection);
            SqlDataReader readerAirline = commandAirline.ExecuteReader();

            while (readerAirline.Read())
            {
                dictAirline.Add(readerAirline[1].ToString(), readerAirline[0].ToString());
                comboBoxAirline.Items.Add(readerAirline[1].ToString());
            }
            readerAirline.Close();

            connection.Close();
        }

        private float TotalCost()
        {
            float result = 0;

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand commandTour = new SqlCommand("SELECT * FROM Tour_Payment WHERE ID_Tour = '" + dictTour[comboBoxTour.Text] + "'", connection);
            SqlDataReader readerTour = commandTour.ExecuteReader();
            readerTour.Read();
            result += float.Parse(readerTour[1].ToString());
            readerTour.Close();

            SqlCommand commandHotel = new SqlCommand("SELECT * FROM Hotel_Payment WHERE ID_Hotel = '" + dictHotel[comboBoxHotel.Text] + "'", connection);
            SqlDataReader readerHotel = commandHotel.ExecuteReader();
            readerHotel.Read();
            var days = dateTimePickerEnd.Value.DayOfYear - dateTimePickerStart.Value.DayOfYear;
            result += (float.Parse(readerHotel[1].ToString())) * days;
            readerHotel.Close();

            SqlCommand commandAirline = new SqlCommand("SELECT * FROM Airline_Payment WHERE ID_IATA = '" + dictAirline[comboBoxAirline.Text] + "'", connection);
            SqlDataReader readerAirline = commandAirline.ExecuteReader();
            readerAirline.Read();
            result += float.Parse(readerAirline[1].ToString());
            readerAirline.Close();

            return result;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string valueType, valueLocation;

            string valueTour = dictTour[comboBoxTour.Text];
            string valueHotel = dictHotel[comboBoxHotel.Text];
            string valueAirline = dictAirline[comboBoxAirline.Text];

            string idReservation = DataBase.ValueIDReservation();

            SqlConnection connection = DataBase.ConnectionForDB();
            connection.Open();
            SqlCommand command = new SqlCommand("AddReservation", connection);

            SqlCommand commandClient = new SqlCommand("SELECT * FROM Client WHERE Phone_Number LIKE '%" + textBoxClient.Text + "%'", connection);
            SqlDataReader readerClient = commandClient.ExecuteReader();
            readerClient.Read();
            string idClient = readerClient[0].ToString();
            readerClient.Close();

            SqlCommand commandPayment = new SqlCommand("AddPayment", connection);

            commandPayment.CommandType = CommandType.StoredProcedure;

            commandPayment.Parameters.Add("@ID_Payment", SqlDbType.VarChar).Value = idReservation;
            commandPayment.Parameters.Add("@Total_Cost", SqlDbType.Money).Value = TotalCost();
            commandPayment.Parameters.Add("@Check_Number", SqlDbType.Int).Value = idReservation;
            commandPayment.Parameters.Add("@Payment_Method", SqlDbType.VarChar).Value = "1";
            commandPayment.Parameters.Add("@ID_Tour", SqlDbType.VarChar).Value = valueTour;
            commandPayment.Parameters.Add("@ID_IATA", SqlDbType.VarChar).Value = valueAirline;
            commandPayment.Parameters.Add("@ID_Hotel", SqlDbType.VarChar).Value = valueHotel;

            int isAdded = 0;

            try
            {
                int rowsAffected = commandPayment.ExecuteNonQuery();
                isAdded++;
            }
            catch
            {
                MessageBox.Show("Помилка!");
                connection.Close();
               return;
            }

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@ID_Reservation", SqlDbType.VarChar).Value = idReservation;
            command.Parameters.Add("@ID_Client", SqlDbType.VarChar).Value = idClient;
            command.Parameters.Add("@ID_Payment", SqlDbType.VarChar).Value = idReservation;
            command.Parameters.Add("@ID_Tour", SqlDbType.VarChar).Value = valueTour;
            command.Parameters.Add("@ID_IATA", SqlDbType.VarChar).Value = valueAirline;
            command.Parameters.Add("@ID_Hotel", SqlDbType.VarChar).Value = valueHotel;
            command.Parameters.Add("@Start_Tour_Date", SqlDbType.Date).Value = DateToString(dateTimePickerStart.Value);
            command.Parameters.Add("@End_Tour_Date", SqlDbType.Date).Value = DateToString(dateTimePickerEnd.Value);
            

            if(connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                int rowAffected = command.ExecuteNonQuery();
                isAdded++;
            }
            catch
            {
                MessageBox.Show("Помилка!");
                connection.Close();
                return;
            }

            if (isAdded == 2) MessageBox.Show("Запис успішно додано!");
            connection.Close();
        }
    }
}
