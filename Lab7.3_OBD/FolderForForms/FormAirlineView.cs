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
    public partial class FormAirlineView : Form
    {
        public FormAirlineView()
        {
            InitializeComponent();
        }

        private void FormAirlineView_Load(object sender, EventArgs e)
        {
            string comandView = "SELECT * FROM Airline_view";

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(comandView, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            float price;
            while (reader.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
   
                price = float.Parse(reader[3].ToString());
                data[data.Count - 1][3] = price.ToString("0.00");
                data[data.Count - 1][4] = reader[4].ToString();
            }
            reader.Close();

            foreach (string[] s in data)
            {
                dataGridViewAirline.Rows.Add(s);
            }
            connection.Close();
        }
    }
}
