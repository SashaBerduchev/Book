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
                booklist.ItemsSource = booksModel.BookSet.Select(x => x.Name + " " + x.Data + " " + x.Description + " " + x.Count).ToList();
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
    }
}
