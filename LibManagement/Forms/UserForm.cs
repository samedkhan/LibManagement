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

    public partial class UserForm : Form
    {
        private readonly UserService _userService;
        private string Admin;
        private User _selectedUser;
        private int _selectedIndex;

        public UserForm()
        {
            _userService = new UserService();
            InitializeComponent();
            FillDgv();
        }

        #region Fill

        public void FillDgv()
        {
            foreach (var item in _userService.GetAllUsers())
            {
                if (item.AdminOrUser == true)
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
            btnDelete.Hide();
            btnUpdate.Hide();
            btnAdd.Show();
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

                    if (item.AdminOrUser != true)
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

                    if (item.AdminOrUser == true)
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
                Password = txtPassword.Text
            };
            if (chkAdmin.Checked)
            {
                user.AdminOrUser = true;
            }
            else
            {
                user.AdminOrUser = false;
            }

            _userService.Add(user);
            MessageBox.Show(user.FullName + " bazaya daxil edildi!!!");
            
            Reset();

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Əminsinizmi?", _selectedUser.FullName, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                MessageBox.Show(_selectedUser.FullName + " bazadan silindi!!!");
                _userService.Delete(_selectedUser);
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

            if (_selectedUser.AdminOrUser == true)
            {
                chkAdmin.Checked = true;
            }
            else
            {
                chkUser.Checked = true;
            }

            btnDelete.Show();
            btnUpdate.Show();
            btnAdd.Hide();
        }
    }
}
