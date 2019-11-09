using System;
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
        private User _enteredUser;
        private string _isPassiv;
        public bookForm(User user)
        {
            InitializeComponent();
            this._enteredUser = user;
            _bookService = new BookService();
            _janreService = new JanreService();
            
            FillComboJanre();
            FillComboJAnreForChanged();
        }

        #region Fill
      
        public void FillDataView()  // Fill Datagridview
        {
            
            foreach(var item in _bookService.AllBook())
            {
                if(item.isPassiv == true)
                {
                    _isPassiv = "PASSiV";
                }
                else
                {
                    _isPassiv = "AKTiV";
                }
                dataGridView1.Rows.Add(item.BookId,
                                        item.Name,
                                        item.janre.Name,
                                        _isPassiv,
                                        item.TotalPiece, 
                                        item.InLibrary,
                                        item.InOrder
                                        );
            }
        }


        public void FillComboJanre() // Fill Combobox "Janres"
        {
            cmbJanres.Items.Add(new ComboItem(0, "Hamısı"));
            cmbJanres.SelectedIndex = 0;
            foreach(var item in _janreService.AllJanre())
            {
                cmbJanres.Items.Add(new ComboItem(item.JanreId, item.Name));
            }
        }

        public void FillComboJAnreForChanged() // Fill Combobox "Janres" for Changing item property
        {
            cmbJanresForChanging.Items.Add(new ComboItem(0, "seçilməyib"));
            cmbJanresForChanging.SelectedIndex = 0;
            foreach (var item in _janreService.AllJanre())
            {
                cmbJanresForChanging.Items.Add(new ComboItem(item.JanreId, item.Name));
            }
        }



        #endregion;

        #region Show Books by Janre

        public void ChangingJanres()  // METHOD View items when changing combobox JANRES
        {
            if (cmbJanres.SelectedIndex != 0)
            {
                foreach (var item in _bookService.AllBook())
                {
                    if (item.isPassiv == true)
                    {
                        _isPassiv = "PASSiV";
                    }
                    else
                    {
                        _isPassiv = "AKTiV";
                    }
                    if(item.JanreId == (cmbJanres.SelectedItem as ComboItem).Id)
                    {
                        dataGridView1.Rows.Add(item.BookId,
                                                item.Name,
                                                item.janre.Name,
                                                _isPassiv,
                                                item.TotalPiece,
                                                item.InLibrary,
                                                item.InOrder
                                                );
                    }
                    
                }
            }
            else
            {
                FillDataView();
            }
        }

        private void CmbJanres_SelectedIndexChanged(object sender, EventArgs e) // View items when changing combobox JANRES
        {
            Reset();
            dataGridView1.Rows.Clear();
            ChangingJanres();
        }
        #endregion

        #region Show Book by Name
        private void TxtBookSearch_TextChanged(object sender, EventArgs e) // SEARCH BOOK by NAME of BOOK
        {
            dataGridView1.Rows.Clear();
            if (!string.IsNullOrEmpty(txtBookSearch.Text))
            {
                cmbJanres.SelectedIndex = 0;

                foreach (Book book in _bookService.AllBook())
                {
                    if (book.Name.ToLower().Contains(txtBookSearch.Text.ToLower()))
                    {
                        if (book.isPassiv == true)
                        {
                            _isPassiv = "PASSiV";
                        }
                        else
                        {
                            _isPassiv = "AKTiV";
                        }
                        dataGridView1.Rows.Add(book.BookId,
                                                book.Name,
                                                book.janre.Name,
                                                _isPassiv,
                                                book.TotalPiece,
                                                book.InLibrary,
                                                book.InOrder
                                                );
                    }
                }
            }
            else
            {
                ChangingJanres();
            }

        }
        #endregion

        #region Book ADD
        private void BtnAdd_Click(object sender, EventArgs e) // ADD NEW item (book)
        {
            if (cmbJanresForChanging.SelectedIndex == 0)

            {
                MessageBox.Show("Kitabın Janrın Seçin !!!");
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtAuthor.Text))
            {
                MessageBox.Show("Boş Xanaları Doldurun");
                return;
            }
            foreach (var item in _bookService.AllBook())
            {
                if (item.Name.Contains(txtName.Text))
                {
                    MessageBox.Show("Bu kitab bazada mövcuddur!!!");
                    return;
                }
            }
            Book book = new Book
            {
                JanreId = (cmbJanresForChanging.SelectedItem as ComboItem).Id,
                Name = txtName.Text,
                AutorName = txtAuthor.Text,
                TotalPiece = Convert.ToInt32(numPiece.Value),
                SalePrice = Convert.ToDecimal(NumPrice.Value),
                RentPrice = Convert.ToDecimal(lblPrice.Text),
                InLibrary = Convert.ToInt32(numPiece.Value),

            };
            if (book.TotalPiece > 0)
            {
                book.isPassiv = false;
            }
            else
            {
                book.isPassiv = true;
            }

            _bookService.Add(book);

            MessageBox.Show(book.Name + " adlı kitab bazaya daxil edildi!!!");
            if (book.TotalPiece > 0)
            {
                dataGridView1.Rows.Add(book.BookId, book.Name, book.janre.Name, "AKTiV", book.TotalPiece, book.InLibrary, book.InOrder);
            }
            else
            {
                dataGridView1.Rows.Add(book.BookId, book.Name, book.janre.Name, "PASSiV", book.TotalPiece, book.InLibrary, book.InOrder);
            }
            Reset();


        }
        #endregion

        #region Update Book
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || cmbJanresForChanging.SelectedIndex == 0)
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
            _selectedBook.InLibrary = Convert.ToInt32(dataGridView1.Rows[_selectedIndex].Cells[4].Value) + Convert.ToInt32(numPiece.Value);

            _bookService.Update(_selectedBook);
            MessageBox.Show("Məlumat yeniləndi !!!");
            dataGridView1.Rows[_selectedIndex].Cells[1].Value = txtName.Text;
            dataGridView1.Rows[_selectedIndex].Cells[2].Value = (cmbJanresForChanging.SelectedItem as ComboItem).Value;

            dataGridView1.Rows[_selectedIndex].Cells[4].Value = numPiece.Value;
            dataGridView1.Rows[_selectedIndex].Cells[5].Value = _selectedBook.InLibrary.ToString();
            Reset();

        }

        #endregion

        #region Lock Book
        private void BtnPassiv_Click(object sender, EventArgs e)
        {
            if (_selectedBook.InOrder > 0)
            {
                MessageBox.Show("Bu kitab hal hazırda sifarişdədir, PASSİV edilə bilməz!!!");
                return;
            }
            else
            {
                DialogResult r = MessageBox.Show("Passiv edilsin?", _selectedBook.Name, MessageBoxButtons.YesNo);

                if (r == DialogResult.Yes)
                {
                    _selectedBook.TotalPiece = 0;
                    _selectedBook.InLibrary = 0;
                    _selectedBook.InOrder = 0;
                    _selectedBook.isPassiv = true;
                    _bookService.Update(_selectedBook);
                    Reset();
                    dataGridView1.Rows[_selectedIndex].Cells[3].Value = "PASSIV";
                    dataGridView1.Rows[_selectedIndex].Cells[4].Value = "0";
                    dataGridView1.Rows[_selectedIndex].Cells[5].Value = "0";
                    dataGridView1.Rows[_selectedIndex].Cells[6].Value = "0";
                }
            }

        }

        #endregion

        #region Unlock Book

        private void BtnUnlock_Click(object sender, EventArgs e) //Unlock book
        {
            DialogResult r = MessageBox.Show("Aktivləşdirilsin?", _selectedBook.Name, MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                _selectedBook.isPassiv = false;
                _bookService.Update(_selectedBook);
                MessageBox.Show(_selectedBook.Name + " aktivləşdirildi!!!");
                dataGridView1.Rows[_selectedIndex].Cells[3].Value = "AKTIV";
                Reset();
            }
        }

        #endregion

        #region Select Book from DataGridView
        private void DataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) // SELECT BOOK by Double-Click to row header
        {
            Reset();
            _selectedIndex = e.RowIndex;
            _selectedBook = _bookService.Find(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value));

            cmbJanresForChanging.SelectedIndex = _selectedBook.JanreId;
            txtName.Text = _selectedBook.Name;
            txtAuthor.Text = _selectedBook.AutorName;
            numPiece.Value = _selectedBook.TotalPiece;
            NumPrice.Value = _selectedBook.SalePrice;
            lblPrice.Text = _selectedBook.RentPrice.ToString("0.0");
            btnAdd.Hide();
            if (_selectedBook.isPassiv == true)
            {
                btnUnlock.Show();
            }
            btnLock.Show();
            btnUpdate.Show();

        }
        #endregion

        #region Setting for visible Add Button

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

        #endregion

        #region Calculate RentPrice
        private void NumPrice_ValueChanged(object sender, EventArgs e) //Calculate and Show Rent Price by percent (1%)
        {
            decimal RentPrice = (Convert.ToDecimal(NumPrice.Value) * rentPercent) / 100;
            lblPrice.Text = RentPrice.ToString();
        }
        #endregion

        #region Resetting All Selected items
        private void DataGridView1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAuthor.Text) || !string.IsNullOrEmpty(txtName.Text))
            {              
            
                Reset();
                
            }
           
        }
        #endregion

        #region Reset

        public void Reset()
        {
            btnAdd.Hide();
            btnUnlock.Hide();
            btnUpdate.Hide();
            btnLock.Hide();
            cmbJanresForChanging.SelectedIndex = -1;
            txtAuthor.Clear();
            txtName.Clear();
            numPiece.Value = 0;
            NumPrice.Value = 0;
            lblPrice.Text = "0.0";
            txtBookSearch.Clear();

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
    }
}
