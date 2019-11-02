﻿using System;
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
        public dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //DialogResult result = MessageBox.Show("Əminsinizmi?", MessageBoxButtons.YesNo);
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
    }
}
