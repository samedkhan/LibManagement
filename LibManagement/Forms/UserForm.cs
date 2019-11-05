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
        private string Status;
        private User _selectedUser;
        private int _selectedIndex;
        public User _enteredUser;
        public UserForm(User user)
        {
            
            InitializeComponent();
            this._enteredUser = user;
            _userService = new UserService();
            FillDgv();
        }

        #region Fill Datagridview

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
                    Admin = "Menecer";
                }
                if (item.IsPassiv == true)
                {
                    Status = "PASSİV";

                }
                else
                {
                    Status = "AKTİV";
                }

                dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, Admin, Status);
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
            btnUnlock.Hide();
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

        #region Selectable-Search 
        private void ChkUser_CheckedChanged(object sender, EventArgs e) // Show only USER Managers
        {
            if (chkUser.Checked)
            {
                chkAdmin.Checked = false;
                dgvUsers.Rows.Clear();
                foreach (var item in _userService.GetAllUsers())
                {
                    if (item.IsPassiv == true)
                    {
                        Status = "PASSİV";

                    }
                    else
                    {
                        Status = "AKTİV";
                    }

                    if (item.IsAdmin != true)
                    {
                        dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, "Menecer", Status);
                    }
                }
            }
            else
            {
                dgvUsers.Rows.Clear();
                FillDgv();
            }
        }

        private void ChkAdmin_CheckedChanged(object sender, EventArgs e) //Show only ADMIN Managers
        {
            if (chkAdmin.Checked)
            {
                chkUser.Checked = false;
                dgvUsers.Rows.Clear();
                foreach (var item in _userService.GetAllUsers())
                {
                    if (item.IsPassiv == true)
                    {
                        Status = "PASSİV";

                    }
                    else
                    {
                        Status = "AKTİV";
                    }
                    if (item.IsAdmin == true)
                    {
                        dgvUsers.Rows.Add(item.UserId, item.FullName, item.Username, item.Password, "Administrator", Status);
                    }
                }
            }
            else
            {
                dgvUsers.Rows.Clear();
                FillDgv();
            }
        }

        #endregion

        #region Add-New-User

        private void BtnAdd_Click(object sender, EventArgs e) // ADD NEW USER
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

        #endregion

        #region Update-User

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtPassword.Text))
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

        #endregion

        #region Lock-User

        private void BtnPassiv_Click(object sender, EventArgs e) // Lock User
        {

            DialogResult r = MessageBox.Show("Passiv edilsin?", _selectedUser.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedUser.IsPassiv = true;
                _userService.Update(_selectedUser);
                MessageBox.Show(_selectedUser.FullName + " passiv edildi!!!");
                dgvUsers.Rows[_selectedIndex].Cells[5].Value = "PASSİV";
                Reset();
            }

        }

        #endregion

        #region Select-User-In-DatagridView

        private void DgvUsers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Reset();
            _selectedUser = _userService.Find(Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells[0].Value));
            _selectedIndex = e.RowIndex;
            txtFullName.Text = _selectedUser.FullName;
            txtLogin.Text = _selectedUser.Username;
            txtPassword.Text = _selectedUser.Password;

            if (_selectedUser.IsPassiv == true)
            {
                btnUnlock.Show();
            }
            btnPassiv.Show();
            btnUpdate.Show();
            btnAdd.Hide();
        }


        #endregion

        #region Unlock User
        private void BtnUnlock_Click(object sender, EventArgs e) //Unlock user
        {
            DialogResult r = MessageBox.Show("Aktivləşdirilsin?", _selectedUser.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedUser.IsPassiv = false;
                _userService.Update(_selectedUser);
                MessageBox.Show(_selectedUser.FullName + " aktivləşdirildi!!!");
                dgvUsers.Rows[_selectedIndex].Cells[5].Value = "AKTIV";
                Reset();
            }
        }

        #endregion

        #region Go-Back-Dashboard
        private void BtnBack_Click(object sender, EventArgs e)
        {
            dashboard dashboard = new dashboard(_enteredUser);
            dashboard.Show();
            this.Hide();
        }

        #endregion
    }
}

