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
using System.Security.Cryptography;

namespace LibManagement.Forms
{

    public partial class UserForm : Form
    {
        private readonly UserService _userService;
        private string Admin;
        private User _selectedUser;
        private int _selectedIndex;
        public User _enteredUser;
        
        
        public UserForm(User user)
        {
            this._enteredUser = user;
            _userService = new UserService();
            InitializeComponent();
            FillDgv();
        }

        #region Fill

        public void FillDgv()
        {
            foreach (var item in _userService.GetAllUsers())
            {
                if (item.IsAdmin == true)
                {
                    Admin = "Administrator";
                }
                else
                {
                    Admin = "İstifadəçi";
                }
                dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, Admin);
            }
        }

        #endregion

        #region Reset

        public void Reset()
        {
            txtFullName.Clear();
            txtLogin.Clear();
            txtPassword.Clear();
            chkAdmin.Checked = false;
            chkUser.Checked = false;
            btnPassiv.Hide();
            btnUpdate.Hide();
            btnAdd.Show();
        }

        #endregion

        #region Password Creating

        public string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            string result = BitConverter.ToString(bytes).Replace("-", string.Empty);
            return result;
        }

        #endregion

        private void ChkUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUser.Checked)
            {
                chkAdmin.Checked = false;
                dgvUsers.Rows.Clear();
                foreach (var item in _userService.GetAllUsers())
                {

                    if (item.IsAdmin != true)
                    {
                        dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, "İstifadəçi");
                    }
                }
            }
            else
            {
                dgvUsers.Rows.Clear();
                FillDgv();
            }
        }

        private void ChkAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdmin.Checked)
            {
                chkUser.Checked = false;
                dgvUsers.Rows.Clear();
                foreach (var item in _userService.GetAllUsers())
                {

                    if (item.IsAdmin == true)
                    {
                        dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, "Administrator");
                    }
                }
            }
            else
            {
                dgvUsers.Rows.Clear();
                FillDgv();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text)) || (chkUser.Checked != true && chkAdmin.Checked != true))
            {
                MessageBox.Show("Xanaları Düzgün Doldurun!!!");
                return;
            }
            foreach (var item in _userService.GetAllUsers())
            {
                if (item.Username.Contains(txtLogin.Text))
                {
                    MessageBox.Show("Bu İstifadəçi adı artıq bazada mövcuddur!!!");
                    return;
                }
            }
            User user = new User
            {
                FullName = txtFullName.Text,
                Username = txtLogin.Text,
                Password = MD5Hash(txtPassword.Text),
                CreaterId = _enteredUser.UserId,
                
            };
            if (chkAdmin.Checked)
            {
                user.IsAdmin = true;
            }
            else
            {
                user.IsAdmin = false;
            }

            _userService.Add(user);
            MessageBox.Show(user.FullName + " bazaya daxil edildi!!!");
            
            Reset();

        }

        private void BtnPassiv_Click(object sender, EventArgs e)
        {

            DialogResult r = MessageBox.Show("Əminsinizmi?", _selectedUser.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedUser.IsPassiv = true;
                _userService.Update(_selectedUser);
                Reset();
            }

        }

        private void DgvUsers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedUser = _userService.Find(Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;
            txtFullName.Text = _selectedUser.FullName;
            txtLogin.Text = _selectedUser.Username;
            txtPassword.Text = _selectedUser.Password;

            if (_selectedUser.IsAdmin == true)
            {
                chkAdmin.Checked = true;
            }
            else
            {
                chkUser.Checked = true;
            }

            btnPassiv.Show();
            btnUpdate.Show();
            btnAdd.Hide();
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
            this.Hide();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Boşluqları doldurun!!!");
                return;
            }

            _selectedUser.IsPassiv = false;

            if (chkAdmin.Checked == true)
            {
                _selectedUser.IsAdmin = true;
            }
            else
            {
                _selectedUser.IsAdmin = false;
            }

            _selectedUser.Password = txtPassword.Text;
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Username = txtLogin.Text;
            _selectedUser.CreaterId = _enteredUser.UserId;

            _userService.Update(_selectedUser);
            MessageBox.Show("Məlumatlar yeniləndi!!!");
            Reset();
        }
    }
}
