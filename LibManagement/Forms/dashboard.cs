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
        public dashboard(User user)
        {
            InitializeComponent();
            CheckUSer(user);
            
        }

        public void CheckUSer(User user)
        {
            if (!user.AdminOrUser)
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
            bookForm bookForm = new bookForm();
            bookForm.ShowDialog();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.ShowDialog();
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
