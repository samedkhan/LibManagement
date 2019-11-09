using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibManagement.Helpers;
using LibManagement.Models;
using LibManagement.Services;


namespace LibManagement.Forms
{
    public partial class StatisticForm : Form
    {

        private OrderService _orderService;
        private UserService _userService;
        private string _status;
        private User _enteredUser;
        public StatisticForm(User user)
        {
            this._enteredUser = user;
            _orderService = new OrderService();
            _userService = new UserService();

         
            InitializeComponent();
            CalculatePayments();
            CalculateFinePayments();
            FillUserCombobox();
            
            //FillDgv();
        }

        #region Fill

        public void FillUserCombobox()
        {
            cmbUsers.Items.Add(new ComboItem(0, "Hamısı"));
            cmbUsers.SelectedIndex = 0;
            foreach (var item in _userService.GetAllUsers())
            {
                cmbUsers.Items.Add(new ComboItem(item.UserId, item.FullName));
            }
        }

        public void FillDgv()
        {
            foreach (var item in _orderService.GetAllOrders())
            {
                if (item.Status == true)
                {
                    _status = "AÇIQ";
                }
                else
                {
                    _status = "TAMAMLANIB";
                }
                dgvOrders.Rows.Add(item.OrderId,
                                     item.customer.FullName,
                                     item.book.Name,
                                     item.CreatedAt.ToString("dd/MM/yyyy"),
                                     item.Deadline.ToString("dd/MM/yyyy"),
                                     item.TotalRentPrice.ToString("0.0") + " AZN",
                                     item.FineForLate.ToString("0.0") + " AZN",
                                     item.TotalPrice.ToString("0.0") + " AZN",
                                     _status,
                                     item.user.FullName
                                     );
            }
        }

        #endregion

        #region Go-Back-Dashboard
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
        }

        #endregion

        #region SEARCH by Customer Name
        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            dgvOrders.Rows.Clear();
            cmbUsers.SelectedIndex = 0;
            foreach (var item in _orderService.GetAllOrders())
            {
                if (item.customer.FullName.ToLower().Contains(txtCustomer.Text.ToLower()))
                {
                    if (item.Status == true)
                    {
                        _status = "AÇIQ";
                    }
                    else
                    {
                        _status = "TAMAMLANIB";
                    }
                    dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd/MM/yyyy"),
                                    item.Deadline.ToString("dd/MM/yyyy"),
                                    item.TotalRentPrice.ToString("0.0") + " AZN",
                                    item.FineForLate.ToString("0.0") + " AZN",
                                    item.TotalPrice.ToString("0.0") + " AZN",
                                    _status,
                                    item.user.FullName
                                    );
                }

            }
        }

        #endregion

        #region SEARCH by user name

        private void CmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvOrders.Rows.Clear();
            txtCustomer.Clear();
            if (cmbUsers.SelectedIndex != 0)
            {

                foreach (var item in _orderService.GetAllOrders())
                {
                    if (item.UserId == (cmbUsers.SelectedItem as ComboItem).Id)
                    {
                        if (item.Status == true)
                        {
                            _status = "AÇIQ";
                        }
                        else
                        {
                            _status = "TAMAMLANIB";
                        }
                        dgvOrders.Rows.Add(item.OrderId,
                                        item.customer.FullName,
                                        item.book.Name,
                                        item.CreatedAt.ToString("dd/MM/yyyy"),
                                        item.Deadline.ToString("dd/MM/yyyy"),
                                        item.TotalRentPrice.ToString("0.0") + " AZN",
                                        item.FineForLate.ToString("0.0") + " AZN",
                                        item.TotalPrice.ToString("0.0") + " AZN",
                                        _status,
                                        item.user.FullName
                                        );
                    }
                }
                return;
            }
            else
            {
                FillDgv();
            }
        }

        #endregion

        #region Show Orders by STATUS

        private void ChkClosed_CheckedChanged(object sender, EventArgs e) // SHOW by CLOSED STATUS
        {

            if (chkClosed.Checked)
            {
                chkOpened.Checked = false;
                dgvOrders.Rows.Clear();
                txtCustomer.Clear();
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status == false)
                    {

                        dgvOrders.Rows.Add(item.OrderId,
                                     item.customer.FullName,
                                     item.book.Name,
                                     item.CreatedAt.ToString("dd/MM/yyyy"),
                                     item.Deadline.ToString("dd/MM/yyyy"),
                                     item.TotalRentPrice.ToString("0.0") + " AZN",
                                     item.FineForLate.ToString("0.0") + " AZN",
                                     item.TotalPrice.ToString("0.0") + " AZN",
                                     "TAMAMLANIB",
                                     item.user.FullName
                                     );
                    }
                }
            }
            else
            {
                dgvOrders.Rows.Clear();
                FillDgv();
            }
        }

        private void ChkOpened_CheckedChanged(object sender, EventArgs e) // SHOW by OPEN STATUS
        {
            if (chkOpened.Checked)
            {
                chkClosed.Checked = false;
                dgvOrders.Rows.Clear();
                txtCustomer.Clear();
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status == true)
                    {
                        dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd/MM/yyyy"),
                                    item.Deadline.ToString("dd/MM/yyyy"),
                                    item.TotalRentPrice.ToString("0.0") + " AZN",
                                    item.FineForLate.ToString("0.0") + " AZN",
                                    item.TotalPrice.ToString("0.0") + " AZN",
                                    "AÇIQ",
                                    item.user.FullName
                                    );
                    }
                }
            }
            else
            {
                dgvOrders.Rows.Clear();
                FillDgv();
            }
        }



        #endregion

        #region Show Orders by Date
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if(dtpStart.Value > dtpEnd.Value)
            {
                MessageBox.Show("Tarixləri düzgün daxil edin");
                return;
            }
            dgvOrders.Rows.Clear();
            chkClosed.Checked = false;
            chkOpened.Checked = false;
           
            //Fill datagridview by Date
            foreach (var item in _orderService.GetAllOrders())
            {
                if(item.CreatedAt >= dtpStart.Value && item.CreatedAt <= dtpEnd.Value)
                {
                    if (item.Status == true)
                    {
                        _status = "AÇIQ";

                    }
                    else
                    {
                        _status = "TAMAMLANIB";
                    }
                    dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd/MM/yyyy"),
                                    item.Deadline.ToString("dd/MM/yyyy"),
                                    item.TotalRentPrice.ToString("0.0") + " AZN",
                                    item.FineForLate.ToString("0.0") + " AZN",
                                    item.TotalPrice.ToString("0.0") + " AZN",
                                    _status,
                                    item.user.FullName
                                    );
                }
            }
            // Calculate TOTAL and CASH Payments by Date ---- 
            
                if (dgvOrders.Rows.Count > 1)
                {
                    lblKassa.Text = _orderService.CalculateCashPaymentByDate(dtpStart.Value, dtpEnd.Value).ToString("0.00") + "AZN"; // Calculate Cash Payment
                    lblTotal.Text = _orderService.CalculateTotalPayByDate(dtpStart.Value, dtpEnd.Value).ToString("0.00") + "AZN"; // Calculate Total
                }
                else
                {
                    lblKassa.Text = "0.00 AZN";
                lblTotal.Text = "0.00 AZN";
                }

        }
        #endregion

        #region Calculate

        // Calculate TOTAL and CASH PAYMENTs when opened this FORM
        public void CalculatePayments()
        {
            if (_orderService.GetAllOrders().Count != 0)
            {
                if (_orderService.CheckOrderStatus())
                {
                    lblKassa.Text = _orderService.CalculateCashPayment().ToString("0.00") + "AZN"; // Calculate Cash Payment
                }
                lblTotal.Text = _orderService.CalculateTotalPay().ToString("0.00") + "AZN"; // Calculate Total Price
            }
        }

        public void CalculateFinePayments()
        {
            foreach (var item in _orderService.GetAllOrders())
            {

                int daysDiff = ((TimeSpan)(DateTime.Today - item.Deadline)).Days;
                if (daysDiff >= 1 && item.Status == true)
                {
                    item.FineForLate = ((decimal)item.book.SalePrice * (decimal)0.5) / daysDiff; // Calculate Fine Pay for Late days

                }
                item.TotalPrice = item.TotalRentPrice + item.FineForLate; // Calculate Total Price

                _orderService.Update(item);

            }
        }


        #endregion

        private void BtnExcell_Click(object sender, EventArgs e)
        {
            SfdSaveAs.ShowDialog();
            SfdSaveAs.FileName = "report"; 
        }
    }
}
