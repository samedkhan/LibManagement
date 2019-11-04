using LibManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibManagement.Forms
{
    public partial class dashboard : Form
    {
        private User _enteredUser;
        public dashboard(User user)
        {
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
              
            }
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

        private void Button1_Click(object sender, EventArgs e)
        {
            bookForm bookForm = new bookForm(_enteredUser);
            bookForm.Text = bookForm.Text + " --- " + this._enteredUser.FullName;
            bookForm.ShowDialog();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm(_enteredUser);
            customerForm.Text = customerForm.Text + " --- " + this._enteredUser.FullName;
            customerForm.ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm(this._enteredUser);
            userForm.Text = userForm.Text + " --- " + this._enteredUser.FullName;
            userForm.ShowDialog();
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm(this._enteredUser);
            orderForm.Text = orderForm.Text +  " --- "  +  this._enteredUser.FullName;
            orderForm.ShowDialog();
        }
    }
}
