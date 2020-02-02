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
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window, IDisposable
    {
        private MainWindow window;
        private BooksModelContainer booksModel;
        public AddBookWindow(MainWindow window)
        {
            this.window = window;
            booksModel = new BooksModelContainer();
            InitializeComponent();
        }

        public void Dispose()
        {
            if(booksModel != null)
            {
                booksModel.Dispose();
                booksModel = null;
            }
            window = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = new Book
                {
                    Name = NameBook.Text,
                    Description = Description.Text,
                    Data = (DateTime)DateTime.SelectedDate,
                    Count = Convert.ToInt32(Count.Text)
                };
                booksModel.BookSet.Add(book);
                booksModel.SaveChanges();
                this.Close();
                window.Loading();
                Dispose();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Введены неверны данные", "Error", MessageBoxButton.OK);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Введены неверны данные", "Error", MessageBoxButton.OK);
            }
        }
    }
}
