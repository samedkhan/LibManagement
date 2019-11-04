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
    public partial class OrderForm : Form
    {
        private readonly BookService _bookService;
        private readonly CustomerService _customerService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        private string Status;
        private User _enteredUser;
        private Order _selectedOrder;
        private int _selectedIndex;
        public OrderForm(User user)
        {
            
            InitializeComponent();
            this._enteredUser = user;
            _bookService = new BookService();
            _customerService = new CustomerService();
            _userService = new UserService();
            _orderService = new OrderService();

            Reset();

            CalculatePayments();

            CalculateFinePayments();

            FillDgv();

            FillCmbCustomer();

            FillCmbBook();
        }

        #region Fill

        public void FillDgv()
        {
           
            foreach(var item in _orderService.GetAllOrders())
            {
                
                if (item.Status == true)
                {
                    Status = "AÇIQ";
                }
                else
                {
                    Status = "TAMAMLANIB";
                }
               

                dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd.MM.yyyy"),
                                    item.Deadline.ToString("dd.MM.yyyy"),
                                    item.TotalRentPrice.ToString("0.0" + " AZN"),
                                    item.FineForLate.ToString("0.0" + " AZN"),
                                    item.user.FullName,
                                    Status);
            }
        }

        public void FillCmbCustomer()
        {
            foreach(var item in _customerService.AllCustomers())
            {
                cmbCustomers.Items.Add(new ComboItem(item.CustomerId, item.FullName));
            }
        }

        public void FillCmbBook()
        {
            foreach(var item in _bookService.AllBook())
            {
                if(item.InLibrary > 0)
                {
                    cmbBook.Items.Add(new ComboItem(item.BookId, item.Name));
                }
            }
        }

        #endregion

        #region Reset

        public void Reset()
        {
            cmbBook.SelectedIndex = -1;
            cmbCustomers.SelectedIndex = -1;
            dtpDeadline.Value = DateTime.Today;
            btnAdd.Show();
            btnClose.Hide();
            btnDelete.Hide();
            btnUpdate.Hide();


        }

        #endregion


        #region Calculate

        public void CalculatePayments()
        {
            if (_orderService.GetAllOrders().Count != 0)
            {
                if (_orderService.CheckOrderStatus())
                {
                    lblKassa.Text = _orderService.CalculateCashPayment().ToString("0.00" + " AZN"); // Calculate Cash Payment
                }

                lblTotal.Text = _orderService.CalculateTotalPay().ToString("0.00" + " AZN"); // Calculate Total Price

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


        private void ChkClosed_CheckedChanged(object sender, EventArgs e) // SHOW by CLOSED STATUS
        {

            if (chkClosed.Checked)
            {
                chkOpened.Checked = false;
                dgvOrders.Rows.Clear();
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status==false)
                    {
                        dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd.MM.yyyy"),
                                    item.Deadline.ToString("dd.MM.yyyy"),
                                    item.TotalRentPrice.ToString("0.0" + " AZN"),
                                    item.FineForLate.ToString("0.0" + " AZN"),
                                    item.user.FullName,
                                    "TAMAMLANIB");
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
                foreach (Order item in _orderService.GetAllOrders())
                {

                    if (item.Status == true)
                    {
                        dgvOrders.Rows.Add(item.OrderId,
                                    item.customer.FullName,
                                    item.book.Name,
                                    item.CreatedAt.ToString("dd.MM.yyyy"),
                                    item.Deadline.ToString("dd.MM.yyyy"),
                                    item.TotalRentPrice.ToString("0.0" + " AZN"),
                                    item.FineForLate.ToString("0.0" + " AZN"),
                                    item.user.FullName,
                                    "AÇIQ");
                    }
                }
            }
            else
            {
                dgvOrders.Rows.Clear();
                FillDgv();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
           
            if (cmbBook.SelectedIndex == -1 || cmbCustomers.SelectedIndex == -1 || dtpDeadline.Value <= DateTime.Now)
            {
                MessageBox.Show("Məlumatları düzgün daxil edin!!!");
                return;
            }

            foreach(Order item in _orderService.GetAllOrders())
            {
                if(item.customer.FullName.Contains((cmbCustomers.SelectedItem as ComboItem).Value)
                    && item.book.Name.Contains((cmbBook.SelectedItem as ComboItem).Value))
                {
                    MessageBox.Show("Bu sifariş bazada mövcuddur!!!");
                    return;
                }
            }

            int daysDiff = ((TimeSpan)(dtpDeadline.Value - DateTime.Today)).Days;


            Order order = new Order
            {
                CreatedAt = DateTime.Today.Date,
                Deadline = dtpDeadline.Value.Date,
                CustomerId = (cmbCustomers.SelectedItem as ComboItem).Id,
                BookId = (cmbBook.SelectedItem as ComboItem).Id,
                Status = true,
                FineForLate = 0,
                TotalRentPrice = daysDiff * _bookService.Find((cmbBook.SelectedItem as ComboItem).Id).RentPrice,
                UserId = _enteredUser.UserId,
            };
            _orderService.Add(order);
            _bookService.Find((cmbBook.SelectedItem as ComboItem).Id).InLibrary--; // Book update - bunu mevbur bele etdim yoxsa ishlemir ozun test ele goreceksen

            _bookService.Find((cmbBook.SelectedItem as ComboItem).Id).InOrder++;
            _bookService.Update(_bookService.Find((cmbBook.SelectedItem as ComboItem).Id)); //burani sen gunorta yazdin ishleyirdi sonra bazani temizledim sifirdan bashladim yazmaga onda problem yaratdi bele deyishdim bu da gah ishleyir gah yox

            // Calculate Total Price

            MessageBox.Show("Sifariş təsdiq edildi Sifarişin məbləği:  " + order.TotalRentPrice.ToString("0.0") + " AZN");

            //MessageBox.Show(order.customer.FullName);

            //dgvOrders.Rows.Add(order.book.Name, //bura da problem verir ne meseledir anlamiram
            //                       order.customer.FullName,
            //                       order.book.Name,
            //                       order.CreatedAt.ToString("dd.MM.yyyy"),
            //                       order.Deadline.ToString("dd.MM.yyyy"),
            //                       order.TotalRentPrice.ToString("0.0" + " AZN"),
            //                       order.FineForLate.ToString("0.0" + " AZN"),
            //                       order.user.FullName,
            //                       "AÇIQ");

            dgvOrders.Rows.Clear();
            CalculatePayments();
            FillDgv();
            Reset();

           
        }

        private void DgvOrders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedOrder = _orderService.Find(Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;

            cmbBook.SelectedItem = new ComboItem(_selectedOrder.BookId, _selectedOrder.book.Name);
            cmbBook.Text = _selectedOrder.book.Name;

            cmbCustomers.SelectedItem = new ComboItem(_selectedOrder.customer.CustomerId, _selectedOrder.customer.FullName);

            cmbCustomers.Text = _selectedOrder.customer.FullName;

            dtpDeadline.Value = _selectedOrder.Deadline;

            

            if(_selectedOrder.Status == true) // Change or Delete order
            {
                int daysDiff = ((TimeSpan)(DateTime.Today - _selectedOrder.CreatedAt)).Days;
                if (daysDiff < 1)
                {
                    btnDelete.Show();
                    btnUpdate.Show();
                    btnAdd.Hide();
                    btnClose.Show();
                }
                else
                {
                    btnDelete.Hide();
                    btnUpdate.Show();
                    btnAdd.Hide();
                    btnClose.Show();
                }
               
            }
            



        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
           DialogResult r =  MessageBox.Show("Əminsinizmi?", _selectedOrder.book.Name, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                

                _selectedOrder.book.InLibrary++;
                _selectedOrder.book.InOrder--;

                _orderService.Update(_selectedOrder);
                _orderService.Delete(_selectedOrder);

                MessageBox.Show("Sifariş silindi!!!");
                dgvOrders.Rows.RemoveAt(_selectedIndex);

                lblTotal.Text = _orderService.CalculateTotalPay().ToString("0.00" + " AZN");
                //if (_orderService.GetAllOrders().Count != 0)
                //{
                //    lblTotal.Text = _orderService.CalculateTotalPrice().ToString("0.00" + " AZN");
                //}
                //else
                //{
                //    lblTotal.Text = "0.00 AZN";
                //}    
                Reset();

            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Sifarişi Tamamlansın?", " ", MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedOrder.Status = false;
                _selectedOrder.book.InLibrary++;
                _selectedOrder.book.InOrder--;
                _orderService.Update(_selectedOrder);
                lblTotal.Text = _orderService.CalculateTotalPay().ToString("0.00" + " AZN");
                dgvOrders.Rows[_selectedIndex].Cells[8].Value = "TAMAMLANIB";
            }
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
            this.Hide();
        }

        private void TxtCustomer_TextChanged(object sender, EventArgs e)
        {
            dgvOrders.Rows.Clear();
            foreach (Order item in _orderService.GetAllOrders())
            {
                if (item.customer.FullName.ToLower().Contains(txtCustomer.Text.ToLower()) && item.Status == true)
                {
             
                    dgvOrders.Rows.Add(item.OrderId,
                                        item.customer.FullName,
                                        item.book.Name,
                                        item.CreatedAt.ToString("dd.MM.yyyy"),
                                        item.Deadline.ToString("dd.MM.yyyy"),
                                        item.TotalRentPrice.ToString("0.0" + " AZN"),
                                        item.FineForLate.ToString("0.0" + " AZN"),
                                        item.user.FullName,
                                        "AÇIQ");
                }

               
            }
        }
    }
}
