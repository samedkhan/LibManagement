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
        private string Status;
        public CustomerForm(User user)
        {
            this._enteredUser = user;
            _customerService = new CustomerService();
            InitializeComponent();
            FillDgv();
        }

        #region Fill DataGridView

        public void FillDgv()
        {
            dgvCustomers.Rows.Clear();

            foreach(var item in _customerService.AllCustomers())
            {
                if(item.IsPassiv == true)
                {
                    Status = "PASSiV";
                }
                else
                {
                    Status = "AKTiV";
                }
                dgvCustomers.Rows.Add(item.CustomerId, item.FullName,  item.PhoneNumber, item.CreatedAt.ToString("dd.MM.yyyy"), Status);

            }
        }

        #endregion

        #region Reset Buttons and Textbox

        public void REset()
        {
            txtFullName.Clear();
            txtPhone.Clear();
            btnAdd.Show();
            btnUpdate.Hide();
            btnLock.Hide();
            btnUnlock.Hide();
        }

        #endregion

        #region Show Customer by his fullname
        private void TxtFullName_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.Rows.Clear();
            foreach(Customer customer in _customerService.AllCustomers())
            {
                if (customer.FullName.ToLower().Contains(txtNameSearch.Text.ToLower()))
                {
                   
                    if (customer.IsPassiv == true)
                    {
                        Status = "PASSiV";
                    }
                    else
                    {
                        Status = "AKTiV";
                    }
                    dgvCustomers.Rows.Add(customer.CustomerId, customer.FullName, customer.PhoneNumber, customer.CreatedAt.ToString("dd.MM.yyyy"), Status);
                }
            }
        }

        #endregion

        #region Add Customer

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
                PhoneNumber = txtPhone.Text,
                CreatedAt = DateTime.Today.Date,
                IsPassiv = false,

            };
            _customerService.Add(customer);
            MessageBox.Show(customer.FullName + " adlı müştəri bazaya daxil edildi!!!");

            dgvCustomers.Rows.Add(customer.CustomerId, customer.FullName, customer.PhoneNumber, customer.CreatedAt.ToString("dd.MM.yyyy"), "AKTiV");
            REset();

        }
        #endregion

        #region Update Customer button
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
        #endregion

        #region Lock Customer
        private void BtnLock_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Abunəlik dayandırılsın?", _selectedCustomer.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedCustomer.IsPassiv = true;
                _customerService.Update(_selectedCustomer);
                MessageBox.Show(_selectedCustomer.FullName + " abunəliyi dayandırıldı!!!");
                dgvCustomers.Rows[_selectedIndex].Cells[4].Value = "PASSiV";
                REset();
            }

        }
        #endregion

        #region Unlock Customer
        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Abunəlik aktivləşdirilsin?", _selectedCustomer.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedCustomer.IsPassiv = false;
                _customerService.Update(_selectedCustomer);
                MessageBox.Show(_selectedCustomer.FullName + " abunəliyi dayandırıldı!!!");
                dgvCustomers.Rows[_selectedIndex].Cells[4].Value = "AKTiV";
                REset();
            }
        }
        #endregion

        #region Select Book in Datagridview
        private void DgvCustomers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            REset();
            _selectedCustomer = _customerService.FindById(Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;
            txtFullName.Text = _selectedCustomer.FullName;
            txtPhone.Text = _selectedCustomer.PhoneNumber;
            btnAdd.Hide();

            if (_selectedCustomer.IsPassiv)
            {
                btnUnlock.Show();
            }
            btnLock.Show();
            btnUpdate.Show();
        }

        #endregion

     

        #region Go-Back-Dashboard
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
            
        }


        #endregion

        #region Show only ACTIVE customers
        private void ChkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActive.Checked)
            {
                chkPassiv.Checked = false;
                dgvCustomers.Rows.Clear();
                foreach (var customer in _customerService.AllCustomers())
                {
                    if (customer.IsPassiv == false)
                    {
                        dgvCustomers.Rows.Add(customer.CustomerId, customer.FullName, customer.PhoneNumber, customer.CreatedAt.ToString("dd.MM.yyyy"), "AKTiV");
                    }
                }
            }
            else
            {
                dgvCustomers.Rows.Clear();
                FillDgv();
            }
        }
        #endregion

        #region Show only PASSIVE customers
        private void ChkPassiv_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassiv.Checked)
            {
                chkActive.Checked = false;
                dgvCustomers.Rows.Clear();
                foreach (var customer in _customerService.AllCustomers())
                {
                    if (customer.IsPassiv == true)
                    {
                        dgvCustomers.Rows.Add(customer.CustomerId, customer.FullName, customer.PhoneNumber, customer.CreatedAt.ToString("dd.MM.yyyy"), "PASSiV");
                    }
                }
            }
            else
            {
                dgvCustomers.Rows.Clear();
                FillDgv();
            }
        }


        #endregion

       
    }
}
