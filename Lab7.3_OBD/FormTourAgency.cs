using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7._3_OBD
{
    public partial class FormTourAgency : Form
    {

        public void WriteUserID(string userID)
        {
            string UserID = userID;
            if (UserID == "TourAgencyManager") ShowButtonsTAM();
            if (UserID == "ReservationManager") ShowButtonsRM();
        }

        public FormTourAgency()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            customizeDesign();
        }

        private void ShowButtonsTAM()
        {
            buttonReservation.Visible = false;
            buttonClient.Visible = false;
            buttonViewReservation.Visible = false;
        }

        private void ShowButtonsRM()
        {
            buttonAirline.Visible = false;
            buttonTour.Visible = false;
            buttonHotel.Visible = false;
        }

        private void customizeDesign()
        {
            panelSubMenuHotel.Visible = false;
            panelSubMenuAirline.Visible = false;
            panelSubMenuReservation.Visible = false;
            panelSubMenuTour.Visible = false;
            panelSubMenuClient.Visible = false;
            panelSubMenuViews.Visible = false;
        }

        private void hideSubMenu()
        {
            if (panelSubMenuClient.Visible == true)
                panelSubMenuClient.Visible = false;
            if (panelSubMenuHotel.Visible == true)
                panelSubMenuHotel.Visible = false;
            if (panelSubMenuAirline.Visible == true)
                panelSubMenuAirline.Visible = false;
            if (panelSubMenuReservation.Visible == true)
                panelSubMenuReservation.Visible = false;
            if (panelSubMenuTour.Visible == true)
                panelSubMenuTour.Visible = false;
            if (panelSubMenuViews.Visible == true)
                panelSubMenuViews.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void buttonTour_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuTour);
        }

        private void buttonAirline_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuAirline);
        }

        private void buttonHotel_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuHotel);
        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuClient);
        }

        private void buttonReservation_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuReservation);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            Hide();
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonTourNew_Click(object sender, EventArgs e)
        {
            openChildForm(new FormTourNew());

            hideSubMenu();
        }

        private void buttonTourEdit_Click(object sender, EventArgs e)
        {
            openChildForm(new FormTourEdit());

            hideSubMenu();
        }

        private void buttonTourView_Click(object sender, EventArgs e)
        {
            openChildForm(new FormTourView());

            hideSubMenu();
        }

        private void buttonAirlineNew_Click(object sender, EventArgs e)
        {
            openChildForm(new FormAirlineNew());

            hideSubMenu();
        }

        private void buttonAirlineEdit_Click(object sender, EventArgs e)
        {
            openChildForm(new FormAirlineEdit());

            hideSubMenu();
        }

        private void buttonAirlineView_Click(object sender, EventArgs e)
        {
            openChildForm(new FormAirlineView());

            hideSubMenu();
        }

        private void buttonHotelNew_Click(object sender, EventArgs e)
        {
            openChildForm(new FormHotelNew());

            hideSubMenu();
        }

        private void buttonHotelEdit_Click(object sender, EventArgs e)
        {
            openChildForm(new FormHotelEdit());

            hideSubMenu();
        }

        private void buttonHotelView_Click(object sender, EventArgs e)
        {
            openChildForm(new FormHotelView());

            hideSubMenu();
        }

        private void buttonClientNew_Click(object sender, EventArgs e)
        {
            openChildForm(new FormClientNew());

            hideSubMenu();
        }

        private void buttonClientEdit_Click(object sender, EventArgs e)
        {
            openChildForm(new FormClientEdit());

            hideSubMenu();
        }

        private void buttonClientView_Click(object sender, EventArgs e)
        {
            openChildForm(new FormClientView());

            hideSubMenu();
        }

        private void buttonReservationNew_Click(object sender, EventArgs e)
        {
            openChildForm(new FormReservationNew());

            hideSubMenu();
        }

        private void buttonResservationView_Click(object sender, EventArgs e)
        {
            openChildForm(new FormReservationView());

            hideSubMenu();
        }

        private void buttonViews_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuViews);
        }

        private void buttonViewTour_Click(object sender, EventArgs e)
        {
            openChildForm(new FormTourReport());

            hideSubMenu();
        }

        private void buttonViewHotel_Click(object sender, EventArgs e)
        {
            openChildForm(new FormHotelReport());

            hideSubMenu();
        }

        private void buttonViewAirline_Click(object sender, EventArgs e)
        {
            openChildForm(new FormAirlineReport());

            hideSubMenu();
        }

        private void buttonViewReservation_Click(object sender, EventArgs e)
        {
            openChildForm(new FormReservationReport());

            hideSubMenu();
        }
    }
}
