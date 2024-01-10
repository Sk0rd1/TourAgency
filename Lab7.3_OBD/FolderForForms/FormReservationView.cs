using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab7._3_OBD
{
    public partial class FormReservationView : Form
    {
        public FormReservationView()
        {
            InitializeComponent();
        }

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
        private void FormReservationView_Load(object sender, EventArgs e)
        {
            string comandView = "SELECT * FROM Reservation_view";

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(comandView, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            float price;

            while (reader.Read())
            {
                data.Add(new string[8]);

                data[data.Count - 1][2] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][0] = reader[2].ToString();
                price = float.Parse(reader[3].ToString());
                data[data.Count - 1][3] = price.ToString("0.00");
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = DateToString(DateTime.Parse(reader[6].ToString()));
                data[data.Count - 1][7] = DateToString(DateTime.Parse(reader[7].ToString()));
            }
            reader.Close();

            foreach (string[] s in data)
            {
                dataGridViewReservation.Rows.Add(s);
            }
        }
    }
}
