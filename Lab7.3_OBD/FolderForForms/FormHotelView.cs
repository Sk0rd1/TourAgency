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
    public partial class FormHotelView : Form
    {
        public FormHotelView()
        {
            InitializeComponent();
        }

        private void FormHotelView_Load(object sender, EventArgs e)
        {
            string comandView = "SELECT * FROM Hotel_view";

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(comandView, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            float price;
            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();

                price = float.Parse(reader[1].ToString());
                data[data.Count - 1][2] = price.ToString("0.00");

                data[data.Count - 1][3] = reader[2].ToString();
                data[data.Count - 1][1] = reader[3].ToString();
            }
            reader.Close();

            foreach (string[] s in data)
            {
                dataGridViewHotel.Rows.Add(s);
            }
            connection.Close();
        }
    }
}
