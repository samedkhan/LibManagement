using System;
using System.Windows.Forms;
using LibManagement.Services;
using LibManagement.Models;
using LibManagement.Helpers;

namespace LibManagement.Forms
{
    public partial class NewOrderForm : Form
    {
        private readonly BookService _bookService;
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private User _enteredUser;

        public event EventHandler DataAdded;

        public NewOrderForm(User user)
        {
            _bookService = new BookService();
            _customerService = new CustomerService();
            _orderService = new OrderService();
            this._enteredUser = user;
            InitializeComponent();
            FillCmbCustomer();
            FillCmbBook();

        }

        #region Fill Combobox's

        public void FillCmbCustomer()
        {
            foreach (var item in _customerService.AllCustomers())
            {
               if(item.IsPassiv != true)
                {
                    cmbCustomers.Items.Add(new ComboItem(item.CustomerId, item.FullName));
                }
            }
        }

        public void FillCmbBook()
        {
            foreach (var item in _bookService.AllBook())
            {
                if (item.InLibrary > 0)
                {
                    cmbBook.Items.Add(new ComboItem(item.BookId, item.Name));
                }
            }
        }

        #endregion

        #region ADD ORDER
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int daysDiff = ((TimeSpan)(dtpDeadline.Value - DateTime.Today)).Days;
            int id;
            if (cmbBook.SelectedIndex == -1 || cmbCustomers.SelectedIndex == -1 || dtpDeadline.Value <= DateTime.Now)
            {
                MessageBox.Show("Məlumatları düzgün daxil edin!!!");
                return;
            }

            foreach (Order item in _orderService.GetAllOrders())
            {
                if (item.customer.FullName.Contains((cmbCustomers.SelectedItem as ComboItem).Value)
                    && item.book.Name.Contains((cmbBook.SelectedItem as ComboItem).Value) && item.Status==true)
                {
                    DialogResult r = MessageBox.Show("Sifarişçi bu kitabı artıq kirayə götürüb, yenidən kirayə verilsinmi?", "", MessageBoxButtons.YesNo);

                    if (r == DialogResult.Yes)
                    {
                        Order orderAgain = new Order
                        {
                            CreatedAt = DateTime.Today.Date,
                            Deadline = dtpDeadline.Value.Date,
                            CustomerId = (cmbCustomers.SelectedItem as ComboItem).Id,
                            BookId = (cmbBook.SelectedItem as ComboItem).Id,
                            Status = true,
                            FineForLate = 0,
                            TotalRentPrice = daysDiff * _bookService.Find((cmbBook.SelectedItem as ComboItem).Id).RentPrice,
                            UserId = _enteredUser.UserId,
                            DigitForSum = 1
                        };
                        id = _orderService.Add(orderAgain);
                        _bookService.Find(id).InLibrary--;
                        _bookService.Find(id).InOrder++;
                        _bookService.Update(_bookService.Find(id));

                        MessageBox.Show("Sifariş təsdiq edildi Sifarişin məbləği:  " + orderAgain.TotalRentPrice.ToString("0.0") + " AZN");
                        Reset();
                        this.DataAdded?.Invoke(this, new EventArgs());
                        
                        return;
                    }
                }
            }

            


            Order NewOrder = new Order
            {
                CreatedAt = DateTime.Today.Date,
                Deadline = dtpDeadline.Value.Date,
                CustomerId = (cmbCustomers.SelectedItem as ComboItem).Id,
                BookId = (cmbBook.SelectedItem as ComboItem).Id,
                Status = true,
                FineForLate = 0,
                TotalRentPrice = daysDiff * _bookService.Find((cmbBook.SelectedItem as ComboItem).Id).RentPrice,
                UserId = _enteredUser.UserId,
                DigitForSum = 1
            };
            id = _orderService.Add(NewOrder);
            _bookService.Find(id).InLibrary--; 
            _bookService.Find(id).InOrder++;
            _bookService.Update(_bookService.Find(id));

            MessageBox.Show("Sifariş təsdiq edildi Sifarişin məbləği:  " + NewOrder.TotalRentPrice.ToString("0.0") + " AZN");
            Reset();
            this.DataAdded?.Invoke(this, new EventArgs());
            this.Close();

           
        }
        #endregion

        #region Reset

        public void Reset()
        {
            cmbBook.SelectedIndex = -1;
            cmbCustomers.SelectedIndex = -1;
            dtpDeadline.Value = DateTime.Today;
        }

        #endregion
    }
}
