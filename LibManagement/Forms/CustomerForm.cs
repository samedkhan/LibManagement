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

namespace LibManagement.Forms
{
    public partial class CustomerForm : Form
    {
        private readonly CustomerService _customerService;
        private Customer _selectedCustomer;
        private int _selectedIndex;
        private User _enteredUser;
        public CustomerForm(User user)
        {
            this._enteredUser = user;
            _customerService = new CustomerService();
            InitializeComponent();
            FillDgv();
        }

        #region Fill

        public void FillDgv()
        {
            dgvCustomers.Rows.Clear();

            foreach(var item in _customerService.AllCustomers())
            {
                dgvCustomers.Rows.Add(item.CustomerId, item.FullName, item.PhoneNumber);

            }
        }

        #endregion

        #region Reset

        public void REset()
        {
            txtFullName.Clear();
            txtPhone.Clear();
            btnAdd.Show();
            btnDelete.Hide();
            btnUpdate.Hide();
        }

        #endregion
        private void TxtFullName_TextChanged(object sender, EventArgs e)
        {


            dgvCustomers.Rows.Clear();
            foreach (var item in _customerService.AllCustomers())
            {
                if((item.FullName.ToLower().Contains(txtFullName.Text.ToLower())|| string.IsNullOrEmpty(txtFullName.Text)) &&( item.PhoneNumber.ToLower().Contains(txtPhone.Text.ToLower())|| string.IsNullOrEmpty(txtPhone.Text)))
                {
                    
                    dgvCustomers.Rows.Add(item.CustomerId, item.FullName, item.PhoneNumber);
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBox.Show("Boşluqları doldurun!!!");
                return;
            }
            int phone;
            if (!int.TryParse(txtPhone.Text, out phone))
            {
                MessageBox.Show("Telefon nömrəsi düzgün deyil!!!");
                return;
            }

            if(_customerService.Contain(txtFullName.Text, txtPhone.Text))
            {
                MessageBox.Show("Bu müştəri bazada mövcuddur!!!");
                
                return;
            }

            Customer customer = new Customer
            {
                FullName = txtFullName.Text,
                PhoneNumber = txtPhone.Text
            };
            _customerService.Add(customer);
            MessageBox.Show(customer.FullName + " adlı müştəri bazaya daxil edildi!!!");

            dgvCustomers.Rows.Add(customer.CustomerId, customer.FullName, customer.PhoneNumber);
            REset();

        }

        private void DgvCustomers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedCustomer = _customerService.FindById(Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;
            txtFullName.Text = _selectedCustomer.FullName;
            txtPhone.Text = _selectedCustomer.PhoneNumber;
            btnAdd.Hide();
            btnDelete.Show();
            btnUpdate.Show();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Əminsinizmi?", _selectedCustomer.FullName, MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                _customerService.Delete(_selectedCustomer);
                MessageBox.Show("Müştəri silindi!!!");
                REset();
                FillDgv();

            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            int phone;

            if (string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtPhone.Text) || !int.TryParse(txtPhone.Text, out phone))
            {
                MessageBox.Show("Xanaları düzgün doldurun!!!");
                return;
            }

            _selectedCustomer.FullName = txtFullName.Text;
            _selectedCustomer.PhoneNumber = txtPhone.Text;
            _customerService.Update(_selectedCustomer);
            MessageBox.Show("Məlumat yeniləndi !!!");
            REset();
            FillDgv();

        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
            this.Hide();
        }
    }
}
