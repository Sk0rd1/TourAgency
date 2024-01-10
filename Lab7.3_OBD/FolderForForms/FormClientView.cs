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
    public partial class FormClientView : Form
    {
        public FormClientView()
        {
            InitializeComponent();
        }

        private void FormClientView_Load(object sender, EventArgs e)
        {
            string comandView = "SELECT * FROM Client";

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

                data[data.Count - 1][0] = reader[1].ToString();
                data[data.Count - 1][1] = reader[2].ToString();
                data[data.Count - 1][2] = reader[3].ToString();
                data[data.Count - 1][3] = reader[4].ToString();
            }
            reader.Close();

            foreach (string[] s in data)
            {
                dataGridViewClient.Rows.Add(s);
            }
            connection.Close();
        }
    }
}
