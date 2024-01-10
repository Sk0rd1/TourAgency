using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7._3_OBD
{
    public partial class FormHotelReport : Form
    {
        public FormHotelReport()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();

        private void FormHotelReport_Load(object sender, EventArgs e)
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

            string queryString = "SELECT * FROM Hotel_view;";
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet view = new DataSet();

            adapter.Fill(view);
            adapter.Dispose();

            dataSetHotelBindingSource.DataSource = view.Tables[0];

            this.reportViewer1.RefreshReport();
        }

        private void Refresh()
        {
            string valueLocation;
            dictLocation.TryGetValue(comboBoxLocation.Text, out valueLocation);
            string queryString = "SELECT * FROM Hotel_view WHERE Country = '"+ comboBoxLocation.Text +"';";
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet view = new DataSet();

            adapter.Fill(view);
            adapter.Dispose();

            dataSetHotelBindingSource.DataSource = view.Tables[0];

            this.reportViewer1.RefreshReport();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {

            Refresh();
        }

        private void buttonSearchAll_Click(object sender, EventArgs e)
        {
            comboBoxLocation.SelectedItem = null;
            string queryString = "SELECT * FROM Hotel_view";
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet view = new DataSet();

            adapter.Fill(view);
            adapter.Dispose();

            dataSetHotelBindingSource.DataSource = view.Tables[0];

            this.reportViewer1.RefreshReport();
        }
    }
}
