using System;
using System.Collections.Generic;
using System.IO;
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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
                booklist.ItemsSource = booksModel.BookSet.ToList();
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

        private void booklist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new UpdateWindow((Book)booklist.SelectedItem, booksModel, this).Show();
        }

        private void ExpPDF_Click(object sender, RoutedEventArgs e)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, new FileStream("Books.pdf", FileMode.Create));
            document.Open();
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);


            //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
            PdfPTable table = new PdfPTable(booklist.Items.Count);
            //Добавим в таблицу общий заголовок
            PdfPCell cell = new PdfPCell(new Phrase("Books"));

            cell.Colspan = booklist.Items.Count;
            cell.HorizontalAlignment = 1;
            //Убираем границу первой ячейки, чтобы балы как заголовок
            cell.Border = 0;
            table.AddCell(cell);

            //Сначала добавляем заголовки таблицы
            for (int j = 0; j < booklist.Items.Count; j++)
            {
                cell = new PdfPCell(new Phrase(new Phrase(booklist.Items.Count)));
                //Фоновый цвет (необязательно, просто сделаем по красивее)
                cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            //Добавляем все остальные ячейки
            for (int j = 0; j < booklist.Items.Count; j++)
            {
                 table.AddCell(new Phrase(booklist.Items[j].ToString(), font));
            }
            //Добавляем таблицу в документ
            document.Add(table);
            
            //Закрываем документ
            document.Close();

            MessageBox.Show("Pdf-документ сохранен");
        }
    }
}
