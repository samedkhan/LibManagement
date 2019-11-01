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
using LibManagement.Helpers;

namespace LibManagement.Forms
{
    public partial class bookForm : Form
    {
        private readonly BookService _bookService;
        private readonly JanreService _janreService;
        private int _selectedIndex;
        private Book _selectedBook;
        private int rentPercent = 1;
        public bookForm()
        {
            InitializeComponent();

            _bookService = new BookService();
            _janreService = new JanreService();
            
            FillComboJanre();
            FillComboJAnreForChanged();
        }

        #region Fill
      
        public void FillDataView()
        {
            
            foreach(var item in _bookService.AllBook())
            {
                dataGridView1.Rows.Add(item.BookId, item.Name, item.janre.Name, item.TotalPiece);
            }
        }

        public void FillComboJanre()
        {
            cmbJanres.Items.Add(new ComboItem(0, "Hamısı"));
            cmbJanres.SelectedIndex = 0;
            foreach(var item in _janreService.AllJanre())
            {
                cmbJanres.Items.Add(new ComboItem(item.JanreId, item.Name));
            }
        }

        public void FillComboJAnreForChanged()
        {
            cmbJanresForChanging.Items.Add(new ComboItem(0, "seçilməyib"));
            cmbJanresForChanging.SelectedIndex = 0;
            foreach (var item in _janreService.AllJanre())
            {
                cmbJanresForChanging.Items.Add(new ComboItem(item.JanreId, item.Name));
            }
        }

        

        #endregion;

        #region Reset

        public void Reset()
        {
            btnAdd.Hide();
            btnDelete.Hide();
            btnUpdate.Hide();
            cmbJanresForChanging.SelectedIndex = -1;
            txtAuthor.Clear();
            txtName.Clear();
            numPiece.Value = 0;
            NumPrice.Value = 0;
            lblPrice.Text = "0.0";
            txtBookSearch.Clear();

        }

        #endregion

        #region ChangingElements

        public void ChangingJanres()
        {
            if (cmbJanres.SelectedIndex != 0)
            {
                foreach (var item in _bookService.AllBookById((cmbJanres.SelectedItem as ComboItem).Id))
                {
                    dataGridView1.Rows.Add(item.BookId, item.Name, item.janre.Name, item.TotalPiece);
                }
            }
            else
            {
                FillDataView();
            }
        }
        #endregion

        private void CmbJanres_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reset();
            dataGridView1.Rows.Clear();
            ChangingJanres();
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                btnAdd.Show();
            }
            else
            {
                btnAdd.Hide();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cmbJanresForChanging.SelectedIndex == 0)
                
            {
                MessageBox.Show("Kitabın Janrın Seçin !!!");
                return;
            }

            if(string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtAuthor.Text))
            {
                MessageBox.Show("Boş Xanaları Doldurun");
                return;
            }
            Book book = new Book
            {
                JanreId = (cmbJanresForChanging.SelectedItem as ComboItem).Id,
                Name = txtName.Text,
                AutorName = txtAuthor.Text,
                TotalPiece = Convert.ToInt32(numPiece.Value),
                SalePrice = Convert.ToDecimal(NumPrice.Value),
                RentPrice = Convert.ToDecimal(lblPrice.Text)

            };
            if (_bookService.Contain(book.Name))
            {
                MessageBox.Show("Bu kitab bazada mövcuddur!!!");
                return;
            }
            _bookService.Add(book);
            MessageBox.Show(book.Name + " adlı kitab bazaya daxil edildi!!!");
            dataGridView1.Rows.Add(book.BookId, book.Name, book.janre.Name, book.TotalPiece);
            Reset();
            

        }

        private void DataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _selectedIndex = e.RowIndex;
            _selectedBook = _bookService.Find(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value));

            cmbJanresForChanging.SelectedIndex = _selectedBook.JanreId;
            txtName.Text = _selectedBook.Name;
            txtAuthor.Text = _selectedBook.AutorName;
            numPiece.Value = _selectedBook.TotalPiece;
            NumPrice.Value = _selectedBook.SalePrice;
            lblPrice.Text = _selectedBook.RentPrice.ToString("0.0");
            btnAdd.Hide();
            btnDelete.Show();
            btnUpdate.Show();

        }

        private void TxtBookSearch_TextChanged(object sender, EventArgs e)
        {
            Book book;
            if (!string.IsNullOrEmpty(txtBookSearch.Text))
            {
                cmbJanres.SelectedIndex = 0;
                
               book = _bookService.FindByName(txtBookSearch.Text);
                if (book == null)
                {
                    dataGridView1.Rows.Clear();
                   
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(book.BookId, book.Name, book.janre.Name, book.TotalPiece);
                }

            }
            else
            {
                ChangingJanres();
            }

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Əminsinizmi?", _selectedBook.Name, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _bookService.Delete(_selectedBook);
                Reset();
                dataGridView1.Rows.RemoveAt(_selectedIndex);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text) || cmbJanresForChanging.SelectedIndex == 0)
            {
                MessageBox.Show("Xanaları düzgün doldurun!!!");
                return;
            }
            _selectedBook.Name = txtName.Text;
            _selectedBook.AutorName = txtAuthor.Text;
            _selectedBook.JanreId = (cmbJanresForChanging.SelectedItem as ComboItem).Id;
            _selectedBook.SalePrice = Convert.ToDecimal(NumPrice.Value);
            _selectedBook.RentPrice = Convert.ToDecimal(lblPrice.Text);
            _selectedBook.TotalPiece = Convert.ToInt32(numPiece.Value);

            _bookService.Update(_selectedBook);
            MessageBox.Show("Məlumat yeniləndi !!!");
            dataGridView1.Rows[_selectedIndex].Cells[1].Value = txtName.Text;
            dataGridView1.Rows[_selectedIndex].Cells[2].Value = (cmbJanresForChanging.SelectedItem as ComboItem).Value;
            dataGridView1.Rows[_selectedIndex].Cells[3].Value = numPiece.Value;
            Reset();

        }

        private void NumPrice_ValueChanged(object sender, EventArgs e)
        {
            decimal RentPrice = (Convert.ToDecimal(NumPrice.Value) * rentPercent) / 100;
            lblPrice.Text = RentPrice.ToString();
        }
    }
}
