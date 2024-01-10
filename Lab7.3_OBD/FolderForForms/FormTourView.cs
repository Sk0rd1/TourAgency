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
using System.Diagnostics;

namespace Lab7._3_OBD
{
    public partial class FormTourView : Form
    {
        public FormTourView()
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
        private void FormTourView_Load(object sender, EventArgs e)
        {
            string comandView = "SELECT * FROM Type_of_the_tour_view";

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(comandView, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            float price;

            while (reader.Read()) 
            {
                data.Add(new string[7]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][6] = reader[1].ToString();
                data[data.Count - 1][1] = reader[2].ToString();
                data[data.Count - 1][2] = reader[3].ToString();
                data[data.Count - 1][4] = DateToString(DateTime.Parse(reader[4].ToString()));
                data[data.Count - 1][5] = DateToString(DateTime.Parse(reader[5].ToString()));

                price = float.Parse(reader[6].ToString());
                data[data.Count - 1][3] = price.ToString("0.00");
            }
            reader.Close();

            foreach (string[] s in data) 
            {
                dataGridViewTour.Rows.Add(s);
            }
            connection.Close();
        }
    }
}
