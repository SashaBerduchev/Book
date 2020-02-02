using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Books
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private Book book;
        private BooksModelContainer booksModel;
        private MainWindow window;
        public UpdateWindow(Book book, BooksModelContainer booksModel, MainWindow window)
        {

            this.book = book;
            this.booksModel = booksModel;
            this.window = window;
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            NameBook.Text = book.Name;
            DateTime.SelectedDate = book.Data;
            Description.Text = book.Description;
            Count.Text = Convert.ToString(book.Count);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            book.Name = NameBook.Text;
            book.Data = (DateTime)DateTime.SelectedDate;
            book.Description = Description.Text;
            book.Count = Convert.ToInt32(Count.Text);
            booksModel.SaveChanges();
            window.Loading();
            this.Close();
        }
    }
}
