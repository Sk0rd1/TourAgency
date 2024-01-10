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
    public partial class FormTourReport : Form
    {
        public FormTourReport()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        Dictionary<string, string> dictType = new Dictionary<string, string>();

        private void FormTourReport_Load(object sender, EventArgs e)
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

            dateTimePickerStart.Value = new DateTime(2023, 1, 1);
            dateTimePickerEnd.Value = new DateTime(2024, 12, 31);

            Refresh(DataBase.DateToString(dateTimePickerStart.Value), DataBase.DateToString(dateTimePickerEnd.Value), null, null);

        }

        private void Refresh(string startDate, string endDate, string valueType, string valueLocation)
        {
            string queryString = "SELECT * FROM Type_of_the_tour_view";

            if(!(startDate == "2023-01-01" && endDate == "2024-12-31" && valueType == null && valueLocation == null))
            {
                if (startDate == "2023-01-01")
                {
                    if(valueType != null)
                    {
                        queryString += " WHERE TypeOfTour = '" + valueType + "' ";

                        if (valueLocation != null)
                            queryString += " AND Country = '" + valueLocation + "' ";
                    }
                    else
                    {
                        if (valueLocation != null)
                            queryString += " WHERE Country = '" + valueLocation + "' ";
                    }
                }
                else
                {
                    queryString += " WHERE StartDate <= '" + startDate + "' AND EndDate >= '" + endDate + "' ";

                    if (valueType != null)
                        queryString += " AND TypeOfTour = '" + valueType + "' ";

                    if (valueLocation != null)
                        queryString += " AND Country = '" + valueLocation + "' ";
                }
            }

            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet view = new DataSet();

            adapter.Fill(view);
            adapter.Dispose();

            dataSetTourBindingSource.DataSource = view.Tables[0];

            this.reportViewer1.RefreshReport();
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

        private void buttonSearchData_Click(object sender, EventArgs e)
        {
            string valueType, valueLocation;
            if (comboBoxType.SelectedItem == null)
                valueType = null;
            else
                valueType = comboBoxType.SelectedItem.ToString();

            if (comboBoxLocation.SelectedItem == null)
                valueLocation = null;
            else
                valueLocation = comboBoxLocation.SelectedItem.ToString();

            Refresh(DataBase.DateToString(dateTimePickerStart.Value), DataBase.DateToString(dateTimePickerEnd.Value), valueType, valueLocation);
            //comboBoxLocation.SelectedItem = null;
            //comboBoxType.SelectedItem = null;
            //dateTimePickerStart.Value = new DateTime(2023, 1, 1);
            //dateTimePickerEnd.Value = new DateTime(2024, 12, 31);
        }

        private void buttonSearchAll_Click(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = new DateTime(2023, 1, 1);
            dateTimePickerEnd.Value = new DateTime(2024, 12, 31);
            comboBoxLocation.SelectedItem = null;
            comboBoxType.SelectedItem = null;

            Refresh(DataBase.DateToString(dateTimePickerStart.Value), DataBase.DateToString(dateTimePickerEnd.Value), null, null);
        }
    }
}