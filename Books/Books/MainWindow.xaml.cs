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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Books
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BooksModelContainer booksModel;
        private List<Book> books;
        public MainWindow()
        {
            InitializeComponent();
            booksModel = new BooksModelContainer();
            Loading();
        }

        public void Loading()
        {
            try
            {
                books = booksModel.BookSet.ToList();
                booklist.ItemsSource = booksModel.BookSet.Select(x =>x.Id + " "+ x.Name + " " + x.Data + " " + x.Description + " " + x.Count).ToList();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString(), "Error", MessageBoxButton.OK);
            }
            
        }

        private void AddBookBttn_Click(object sender, RoutedEventArgs e)
        {
            new AddBookWindow(this).Show();
        }

        private void DelBookBttn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                booksModel.BookSet.Remove(books[Convert.ToInt32(delelem.Text)]);
                booksModel.SaveChanges();
                Loading();
            }catch(IndexOutOfRangeException)
            {
                MessageBox.Show("Элемента не сущетвует", "Error", MessageBoxButton.OK);
            }
            catch (FormatException)
            {
                MessageBox.Show("Укажите элемент удаления", "Error", MessageBoxButton.OK);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Элемента не существует", "Error", MessageBoxButton.OK);
            }
        }
    }
}
