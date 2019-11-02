using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibManagement.Models;
using LibManagement.Services;

namespace LibManagement.Forms
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService;
        public LoginForm()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Boş xanaları doldurun!!!");
                return;
            }

            User user = _userService.Contain(txtUsername.Text, txtPassword.Text);

            if (user == null)
            {
                MessageBox.Show("İstifadəçi adı və ya Şifrə yalnışdır!!!");
                return;
            }

            dashboard dashboard = new dashboard(user);
            dashboard.Show();

            this.Hide();

        }


    }
}
