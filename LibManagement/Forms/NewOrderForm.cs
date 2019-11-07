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
    public partial class NewOrderForm : Form
    {
        private readonly BookService _bookService;
        private readonly CustomerService _customerService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        private string Status;
        private User _enteredUser;
        private Order _selectedOrder;
        private int _selectedIndex;
        public NewOrderForm()
        {
            _bookService = new BookService();
            _customerService = new CustomerService();
            _userService = new UserService();
            _orderService = new OrderService();
            InitializeComponent();
            FillCmbCustomer();
            FillCmbBook();

        }

        #region Fill Combobox's

        public void FillCmbCustomer()
        {
            foreach (var item in _customerService.AllCustomers())
            {
                cmbCustomers.Items.Add(new ComboItem(item.CustomerId, item.FullName));
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
    }
}
