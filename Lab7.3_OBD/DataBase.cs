using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Lab7._3_OBD
{
    static internal class DataBase
    {
        static SqlConnection connection;

        static private string ConnectionString(string userID, string password)
        {
            string filePath = "DBConnectionString";
            string conectionString = File.ReadAllText(filePath);

            string[] components = conectionString.Split(';');
            
            return components[4] + ";" + components[3] + "; User ID=" + userID + ";" + " Password = " + password;

        }

        static public bool Connection(string UserID, string Password)
        {
            string connectionString = ConnectionString(UserID, Password);
            
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch
            {
                MessageBox.Show("Не підключено!");
                return false;
            }
        }


        static public SqlConnection ConnectionForDB()
        {
            return connection;
        }

        static public string ValueIDTour()
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("SELECT ID_Tour from Tour", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int currentValue, maxValue = -1;

            while (reader.Read())
            {
                currentValue = int.Parse(reader.GetString(0));
                if (currentValue > maxValue) maxValue = currentValue;
            }

            connection.Close();
            ++maxValue;
            string result;

            if(maxValue < 10) result = "00" + maxValue.ToString();
            else if (maxValue < 100) result = "0" + maxValue.ToString();
            else result = maxValue.ToString();

            return result;
        }

        static public string ValueIDHotel()
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("SELECT ID_Hotel from Hotel", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int currentValue, maxValue = -1;

            while (reader.Read())
            {
                currentValue = int.Parse(reader.GetString(0));
                if (currentValue > maxValue) maxValue = currentValue;
            }

            connection.Close();
            ++maxValue;
            string result;

            if (maxValue < 10) result = "00" + maxValue.ToString();
            else if (maxValue < 100) result = "0" + maxValue.ToString();
            else result = maxValue.ToString();

            return result;
        }

        static public string ValueIDClient()
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("SELECT ID_Client from Client", connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int currentValue, maxValue = -1;

            while (reader.Read())
            {
                currentValue = int.Parse(reader.GetString(0));
                if (currentValue > maxValue) maxValue = currentValue;
            }

            connection.Close();
            ++maxValue;
            string result;

            if (maxValue < 10) result = "00" + maxValue.ToString();
            else if (maxValue < 100) result = "0" + maxValue.ToString();
            else result = maxValue.ToString();

            return result;
        }

        static public string ValueIDReservation()
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("SELECT ID_Reservation from Reservation", connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int currentValue, maxValue = -1;

            while (reader.Read())
            {
                currentValue = int.Parse(reader.GetString(0));
                if (currentValue > maxValue) maxValue = currentValue;
            }

            connection.Close();
            return (++maxValue).ToString();
        }

        static public string ValueIDPayment()
        {
            SqlConnection connection = DataBase.ConnectionForDB();
            SqlCommand command = new SqlCommand("SELECT ID_Payment from Payment", connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int currentValue, maxValue = 1;

            while (reader.Read())
            {
                currentValue = int.Parse(reader.GetString(0));
                if (currentValue > maxValue) maxValue = currentValue;
            }

            connection.Close();
            return (++maxValue).ToString();
        }

        static public string DateToString(DateTime dateTime)
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
    }
}