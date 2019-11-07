using System;
using System.Security.Cryptography;
using System.Text;
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

        #region Password Creating

        public string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            string result = BitConverter.ToString(bytes).Replace("-", string.Empty);
            return result;
        }

        #endregion

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text)) // Checking EMPTIES
            {
                MessageBox.Show("Boş xanaları doldurun!!!");
                return;
            }

            User user = _userService.FindUsername(txtUsername.Text, MD5Hash(txtPassword.Text));

            if (user == null || user.IsPassiv == true) // CHEKING USER is PASSIV or NOT
            {
                MessageBox.Show("İstifadəçi adı və ya Şifrə yalnışdır!!!");
                return;
            }

            dashboard dashboard = new dashboard(user);
            dashboard.Text = user.FullName;
            dashboard.Show();

            this.Hide();

        }


    }
}
