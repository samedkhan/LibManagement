using LibManagement.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using LibManagement.Services;

namespace LibManagement.Forms
{
    public partial class dashboard : Form
    {
        private User _enteredUser;
        private BookService _bookService;
        private CustomerService _customerService;
        private OrderService _orderService;
        private UserService _userService;
        public dashboard(User user)
        {
            _bookService = new BookService();
            _customerService = new CustomerService();
            _orderService = new OrderService();
            _userService = new UserService();
            this._enteredUser = user;
            InitializeComponent();
            CheckUSer();
        }

        public void CheckUSer()
        {
            if (!this._enteredUser.IsAdmin)
            {
                btnUsers.Enabled = false;
                btnStatistic.Enabled = false;
                btnUsers.BackColor = Color.Gray;
                btnStatistic.BackColor = Color.Gray;
            }
            btnBooks.Text += "  - " + _bookService.Sum();
            btnCustomers.Text += "  - " + _customerService.Sum();
            btnOrders.Text += "  - " + _orderService.Sum();
            btnUsers.Text += "  - " + _userService.Sum();
        }
       
        private void Button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Əminsinizmi?", "", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                foreach (Form form in Application.OpenForms)
                {
                    if(form.GetType() != loginForm.GetType())
                    {
                        form.Hide();
                    }
                }
                loginForm.Show();
                
            }
        }

        

       

        

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        

        private void BtnUsers_Click(object sender, EventArgs e) //Go To User Form
        {
            this.Hide();
            UserForm userForm = new UserForm(this._enteredUser);
            userForm.Text = userForm.Text + " --- " + this._enteredUser.FullName;
            userForm.ShowDialog();
           
        }

        private void BtnBooks_Click(object sender, EventArgs e)  //Go To Book Form
        {
            this.Hide();
            bookForm bookForm = new bookForm(_enteredUser);
            bookForm.Text = bookForm.Text + " --- " + this._enteredUser.FullName;
            bookForm.ShowDialog();
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customerForm = new CustomerForm(_enteredUser);
            customerForm.Text = customerForm.Text + " --- " + this._enteredUser.FullName;
            customerForm.ShowDialog();
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            this.Hide();
            OrdersForm orderForm = new OrdersForm(this._enteredUser);
            orderForm.Text = orderForm.Text + " --- " + this._enteredUser.FullName;
            orderForm.Show();
            
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            loginForm.FormClosed += (s, args) => this.Close();
        }

        private void BtnStatistic_Click(object sender, EventArgs e)
        {
            this.Hide();
            StatisticForm statisticForm = new StatisticForm(_enteredUser);
            statisticForm.ShowDialog();
        }
    }
}
