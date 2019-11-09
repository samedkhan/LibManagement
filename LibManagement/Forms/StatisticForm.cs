using System;
using System.Windows.Forms;
using LibManagement.Helpers;
using LibManagement.Models;
using LibManagement.Services;
using ClosedXML.Excel;


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

        #region SEARCH by User Name

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

        #region SHOW Orders by STATUS

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
                    btnExcell.Visible = true;
                    lblKassa.Text = _orderService.CalculateCashPaymentByDate(dtpStart.Value, dtpEnd.Value).ToString("0.00") + "AZN"; // Calculate Cash Payment
                    lblTotal.Text = _orderService.CalculateTotalPayByDate(dtpStart.Value, dtpEnd.Value).ToString("0.00") + "AZN"; // Calculate Total
                }
                else
                {
                    btnExcell.Visible = false;
                    lblKassa.Text = "0.00 AZN";
                    lblTotal.Text = "0.00 AZN";
                }

        }
        #endregion

        #region Calculate TOTAL and CASH payments

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

        #region Import & Save Excell file
        private void BtnExcell_Click(object sender, EventArgs e)
        {

            Import_Save();
        }

        public void Import_Save()
        {
            var wb = new XLWorkbook(@"C:\Users\SAMED\source\repos\LibManagement\LibManagement\export\library.xlsx");
            var ws = wb.Worksheet(1);
            ws.Cell(2, "C").Value = dtpStart.Value.ToString("dd/MM/yyyy") + " - " + dtpEnd.Value.ToString("dd/MM/yyyy");
            int rowStart = 5;
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
                    ws.Cell(rowStart, 1).Value = rowStart - 4;
                    ws.Cell(rowStart, 2).Value = item.customer.FullName;
                    ws.Cell(rowStart, 3).Value = item.book.Name;
                    ws.Cell(rowStart, 4).Value = item.CreatedAt.ToString("dd/MM/yyyy");
                    ws.Cell(rowStart, 5).Value = item.Deadline.ToString("dd/MM/yyyy");
                    ws.Cell(rowStart, 6).Value = item.TotalRentPrice.ToString("0.0") + " AZN";
                    ws.Cell(rowStart, 7).Value = item.FineForLate.ToString("0.0") + " AZN";
                    ws.Cell(rowStart, 8).Value = item.TotalPrice.ToString("0.0") + " AZN";
                    ws.Cell(rowStart, 9).Value = _status;
                    ws.Cell(rowStart, 10).Value = item.user.FullName;
                    rowStart++;
                }

            }
            ws.Cell(rowStart + 1, "B").Style.Fill.BackgroundColor = XLColor.Blue;
            ws.Cell(rowStart + 1, "B").Style.Font.FontSize = 14;
            ws.Cell(rowStart + 1, "B").Style.Font.FontColor = XLColor.White;
            ws.Cell(rowStart + 1, "B").Style.Font.Bold = true;
            ws.Cell(rowStart+1, "B").Value = "CƏMİ SATIŞ: " + lblTotal.Text;
            ws.Cell(rowStart + 2, "B").Style.Fill.BackgroundColor = XLColor.Blue;
            ws.Cell(rowStart + 2, "B").Style.Font.FontSize = 14;
            ws.Cell(rowStart + 2, "B").Style.Font.FontColor = XLColor.White;
            ws.Cell(rowStart + 2, "B").Style.Font.Bold = true;
            ws.Cell(rowStart+2, "B").Value = "KASSA: " + lblKassa.Text;
            SfdSaveAs.Title = "SATIŞLAR HAQQINDA MƏLUMAT";
            SfdSaveAs.FileName = "REPORT" + dtpStart.Value.ToString("d.M.yyyy") + "to" + dtpEnd.Value.ToString("d.M.yyyy");
            SfdSaveAs.DefaultExt = "xlsx";
            DialogResult r = SfdSaveAs.ShowDialog();
            if (r == DialogResult.OK)
            {
                wb.SaveAs(SfdSaveAs.FileName);
            }

            
        }

        #endregion;

        #region Go-Back-Dashboard
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
        }

        #endregion

    }
}
