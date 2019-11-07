using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibManagement.Services;
using LibManagement.Models;
using LibManagement.Helpers;

namespace LibManagement.Forms
{
    public partial class OrdersForm : Form
    {
        private readonly BookService _bookService;
        private readonly CustomerService _customerService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        private string Status;
        private User _enteredUser;
        private Order _selectedOrder;
        private int _selectedIndex;
        public OrdersForm(User user)
        {
            InitializeComponent();
            _bookService = new BookService();
            _customerService = new CustomerService();
            _userService = new UserService();
            _orderService = new OrderService();
            this._enteredUser = user;
            CheckUser();
            FillAllOrder();
            FillTodayRefunds();
            FillTomorrowRefunds();
            FillLateOrders();
            FillClosedOrders();

        }

        #region Check User

        public void CheckUser()
        {
            if(_enteredUser.IsAdmin == true)
            {
                tcOrders.TabPages.Add(new TabPage("tabStatistic").Text = "STATiSTiKA");

            }
        }

        #endregion

        #region Fill

        public void FillAllOrder()
        {
            foreach(Order order in _orderService.GetAllOrders())
            {
                if (order.Status == true)
                {
                    Status = "AÇIQ";
                }
                else
                {
                    Status = "TAMAMLANIB";
                }
                dgvAllOrders.Rows.Add(order.OrderId,
                                        order.customer.FullName,
                                        order.book.Name, order.CreatedAt.ToString("dd.MM.yyyy"),
                                        order.Deadline.ToString("dd.MM.yyyy"),
                                        order.TotalRentPrice.ToString("0.00") + " AZN",
                                        order.FineForLate.ToString("0.00") + " AZN",
                                        Status);
            }
        }

        public void FillTodayRefunds()
        {
           
            foreach (Order order in _orderService.GetAllOrders())
            {
                int value = DateTime.Compare(order.Deadline, DateTime.Now.Date);

                if (value == 0 && order.Status == true)
                {
                    dgvTodayRefunds.Rows.Add(order.OrderId,
                                         order.customer.FullName,
                                         order.book.Name,
                                         order.CreatedAt.ToString("dd.MM.yyyy"),
                                         "BUGÜN",
                                         order.TotalRentPrice.ToString("0.00") + " AZN"
                                         );
                }
               
                
            }
        }

        public void FillTomorrowRefunds()
        {

            foreach (Order order in _orderService.GetAllOrders())
            {
                int value = DateTime.Compare(order.Deadline, DateTime.Now.Date.AddDays(1));

                if (value == 0 && order.Status == true)
                {
                    dgvTomorrowRefunds.Rows.Add(order.OrderId,
                                         order.customer.FullName,
                                         order.book.Name,
                                         order.CreatedAt.ToString("dd.MM.yyyy"),
                                         "SABAH",
                                         order.TotalRentPrice.ToString("0.00") + " AZN"
                                         );
                }


            }
        }


        public void FillLateOrders()
        {

            foreach (Order order in _orderService.GetAllOrders())
            {
                int value = DateTime.Compare(order.Deadline, DateTime.Now.Date);

                if (value == -1 && order.Status == true)
                {
                    dgvLateOrders.Rows.Add(order.OrderId,
                                         order.customer.FullName,
                                         order.book.Name,
                                         order.CreatedAt.ToString("dd.MM.yyyy"),
                                         order.Deadline.ToString("dd.MM.yyyy"),
                                         order.TotalRentPrice.ToString("0.00") + " AZN",
                                         order.FineForLate.ToString("0.00") + " AZN"
                                         );
                }


            }
        }

        public void FillClosedOrders()
        {
            dgvClosedOrders.Rows.Clear();
            foreach (Order order in _orderService.GetAllOrders())
            {
                

                if (order.Status == false)
                {
                    dgvClosedOrders.Rows.Add(order.OrderId,
                                         order.customer.FullName,
                                         order.book.Name,
                                         order.CreatedAt.ToString("dd.MM.yyyy"),
                                         order.Deadline.ToString("dd.MM.yyyy"),
                                         order.TotalRentPrice.ToString("0.00") + " AZN",
                                         order.FineForLate.ToString("0.00") + " AZN"
                                         );
                }


            }
        }
        #endregion


        private void TcOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tcOrders.SelectedTab == tabFull)
            {
                txtNameSearch.Visible = true;
                txtBookNameSearch.Visible = true;
                lblCustomer.Visible = true;
                LblBook.Visible = true;
                chkClosed.Visible = true;
                chkOpened.Visible = true;
                pctBook.Visible = true;
                pctUser.Visible = true;
                btnAdd.Visible = true;

            }
            else
            {
                txtNameSearch.Visible = false;
                txtBookNameSearch.Visible = false;
                lblCustomer.Visible = false;
                LblBook.Visible = false;
                chkClosed.Visible = false;
                chkOpened.Visible = false;
                pctBook.Visible = false;
                pctUser.Visible = false;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDone.Visible = false;
                btnDelete.Visible = false;
            }
        }


        #region Show Order which closed or open

        private void ChkClosed_CheckedChanged(object sender, EventArgs e) // SHOW by CLOSED STATUS
        {

            if (chkClosed.Checked)
            {
                chkOpened.Checked = false;
                dgvAllOrders.Rows.Clear();
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status == false)
                    {
                  
                        dgvAllOrders.Rows.Add(item.OrderId,
                                        item.customer.FullName,
                                        item.book.Name,
                                        item.CreatedAt.ToString("dd.MM.yyyy"),
                                        item.Deadline.ToString("dd.MM.yyyy"),
                                        item.TotalRentPrice.ToString("0.00") + " AZN",
                                        item.FineForLate.ToString("0.00") + " AZN",
                                        "TAMAMLANIB");
                    }
                }
            }
            else
            {
                dgvAllOrders.Rows.Clear();
                FillAllOrder();
            }
        }

        private void ChkOpened_CheckedChanged(object sender, EventArgs e) // SHOW by OPEN STATUS
        {
            if (chkOpened.Checked)
            {
                chkClosed.Checked = false;
                dgvAllOrders.Rows.Clear();
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status == true)
                    {
                        dgvAllOrders.Rows.Add(item.OrderId,
                                       item.customer.FullName,
                                       item.book.Name,
                                       item.CreatedAt.ToString("dd.MM.yyyy"),
                                       item.Deadline.ToString("dd.MM.yyyy"),
                                       item.TotalRentPrice.ToString("0.00") + " AZN",
                                       item.FineForLate.ToString("0.00") + " AZN",
                                       "AÇIQ");
                    }
                }
            }
            else
            {
                dgvAllOrders.Rows.Clear();
                FillAllOrder();
            }
        }


        #endregion

        #region Go-Back-Dashboard  ---- ?????
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();

        }

        #endregion

        #region Reset

        public void Reset()
        {
            btnAdd.Show();
            btnDone.Hide();
            btnDelete.Hide();
            btnUpdate.Hide();

        }

        private void DgvAllOrders_Click(object sender, EventArgs e)
        {
            Reset();
        }

        #endregion

        #region SEARCH
        private void TxtNameSearch_TextChanged(object sender, EventArgs e)
        {
            dgvAllOrders.Rows.Clear();
            Reset();
            txtBookNameSearch.Clear();
            foreach(Order order in _orderService.GetAllOrders())
            {
                if (order.customer.FullName.ToLower().Contains(txtNameSearch.Text.ToLower()))
                {
                    if (order.Status == true)
                    {
                        Status = "AÇIQ";
                    }
                    else
                    {
                        Status = "TAMAMLANIB";
                    }
                    dgvAllOrders.Rows.Add(order.OrderId,
                                            order.customer.FullName,
                                            order.book.Name, order.CreatedAt.ToString("dd.MM.yyyy"),
                                            order.Deadline.ToString("dd.MM.yyyy"),
                                            order.TotalRentPrice.ToString("0.00") + " AZN",
                                            order.FineForLate.ToString("0.00") + " AZN",
                                            Status);
                }

            }
        }

        private void TxtBookNameSearch_TextChanged(object sender, EventArgs e)
        {
            dgvAllOrders.Rows.Clear();
            Reset();
            txtNameSearch.Clear();
            foreach (Order order in _orderService.GetAllOrders())
            {
                if (order.book.Name.ToLower().Contains(txtBookNameSearch.Text.ToLower()))
                {
                    if (order.Status == true)
                    {
                        Status = "AÇIQ";
                    }
                    else
                    {
                        Status = "TAMAMLANIB";
                    }
                    dgvAllOrders.Rows.Add(order.OrderId,
                                            order.customer.FullName,
                                            order.book.Name, order.CreatedAt.ToString("dd.MM.yyyy"),
                                            order.Deadline.ToString("dd.MM.yyyy"),
                                            order.TotalRentPrice.ToString("0.00") + " AZN",
                                            order.FineForLate.ToString("0.00") + " AZN",
                                            Status);
                }

            }
        }




        #endregion



        #region SELECT ROW in DgvAllOrdes
        private void DgvAllOrders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Reset();
            _selectedOrder = _orderService.Find(Convert.ToInt32(dgvAllOrders.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;

           
            if (_selectedOrder.Status == true) // Change or Delete order
            {
                int daysDiff = ((TimeSpan)(DateTime.Today - _selectedOrder.CreatedAt)).Days;
                if (daysDiff < 1)
                {
                    btnDelete.Show();
                    btnUpdate.Show();
                    btnDone.Show();
                    btnAdd.Show();
                }
                else
                {
                    btnDelete.Hide();
                    btnUpdate.Show();
                    btnAdd.Hide();
                    btnDone.Show();
                    btnAdd.Show();
                }

            }
        }


        #endregion;

        #region SELECT Row in DgvTodayRefunds

        private void DgvTodayRefunds_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedOrder = _orderService.Find(Convert.ToInt32(dgvTodayRefunds.Rows[e.RowIndex].Cells[0].Value));
           

            MessageBox.Show(_selectedOrder.customer.FullName + "\n" +
                            _selectedOrder.book.Name + "\n" +
                            _selectedOrder.customer.PhoneNumber);
        }

        #endregion

        #region SELECT Row in DgvTomorrowRefunds

        private void DgvTomorrowRefunds_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedOrder = _orderService.Find(Convert.ToInt32(dgvTomorrowRefunds.Rows[e.RowIndex].Cells[0].Value));


            MessageBox.Show(_selectedOrder.customer.FullName + "\n" +
                            _selectedOrder.book.Name + "\n" +
                            _selectedOrder.customer.PhoneNumber);
        }

        #endregion
        
        #region SELECT Row in DgvLateOrders

        private void DgvLateOrders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedOrder = _orderService.Find(Convert.ToInt32(dgvLateOrders.Rows[e.RowIndex].Cells[0].Value));


            MessageBox.Show(_selectedOrder.customer.FullName + "\n" +
                            _selectedOrder.book.Name + "\n" +
                            _selectedOrder.customer.PhoneNumber);
        }

        #endregion



        #region Order Delete

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Silinsin?", _selectedOrder.customer.FullName + " - " + _selectedOrder.book.Name, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _orderService.Delete(_selectedOrder);
                MessageBox.Show("Sifariş silindi");
                dgvAllOrders.Rows.RemoveAt(_selectedIndex);
                Reset();
            }
        }

        #endregion

        #region Order Done
        private void BtnDone_Click(object sender, EventArgs e)
        {
            _selectedOrder.Status = false;
            _orderService.Update(_selectedOrder);
            MessageBox.Show(_selectedOrder.customer.FullName + " " +
                                _selectedOrder.book.Name + " " +
                                " TAMAMLANDI");
            dgvAllOrders.Rows[_selectedIndex].Cells[7].Value = "TAMAMLANMIŞ";
            Reset();
            FillClosedOrders();
        }







        #endregion

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            NewOrderForm newOrderForm = new NewOrderForm();
            newOrderForm.ShowDialog();
        }
    }
}
