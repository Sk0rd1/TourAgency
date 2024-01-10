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
    public partial class FormReservationReport : Form
    {
        public FormReservationReport()
        {
            InitializeComponent();
        }

        private void FormReservationReport_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = new DateTime(2023, 1, 1);
            dateTimePickerEnd.Value = new DateTime(2024, 12, 31);
            Refresh("2010-01-01", "2050-12-31");
        }

        private void Refresh(string startDate, string EndDate)
        {
            string queryString;

            if (startDate == "2010-01-01" && EndDate == "2050-12-31")
            {
                queryString = "SELECT * FROM Reservation_view WHERE TotalCost BETWEEN " + numericUpDownMin.Value + " AND " + numericUpDownMax.Value;
            }
            else
            {
                queryString = "SELECT * FROM Reservation_view WHERE StartDate > '" + startDate + "' AND EndDate < '" + EndDate + "' AND TotalCost BETWEEN " + numericUpDownMin.Value + " AND " + numericUpDownMax.Value;
            }
            SqlConnection connection = DataBase.ConnectionForDB();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet view = new DataSet();

            adapter.Fill(view);
            adapter.Dispose();

            dataSetReservationBindingSource.DataSource = view.Tables[0];

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

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Refresh(DataBase.DateToString(dateTimePickerStart.Value), DataBase.DateToString(dateTimePickerEnd.Value));
        }

        private void buttonSearchAll_Click(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = new DateTime(2023, 1, 1);
            dateTimePickerEnd.Value = new DateTime(2024, 12, 31);

            numericUpDownMax.Value = 9999;
            numericUpDownMin.Value = 0;

            Refresh(DataBase.DateToString(dateTimePickerStart.Value), DataBase.DateToString(dateTimePickerEnd.Value));
        }
    }
}
