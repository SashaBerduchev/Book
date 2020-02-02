using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void ExpExcel_Click(object sender, RoutedEventArgs e)
        {
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                Microsoft.Office.Interop.Excel.Range xlSheetRange;
                xlApp.Workbooks.Add(Type.Missing);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;

                //выбираем лист на котором будем работать (Лист 1)
               var xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlApp.Sheets[1];
                //Название листа
                xlSheet.Name = "Book";


                int collInd = 0;
                string data = "";
                int rowInd = 0;
                //называем колонки
                for (int i = 1; i < booklist.Items.Count; i++)
                {
                    data = booklist.Items[i].ToString();
                    xlSheet.Cells[1, i + 1] = data;

                    //выделяем первую строку
                    xlSheetRange = xlSheet.get_Range("A3:Z3", Type.Missing);

                    //делаем полужирный текст и перенос слов
                    xlSheetRange.WrapText = true;
                    xlSheetRange.Font.Bold = true;


                }

                //заполняем строки
                for (collInd = 0; collInd < booklist.Items.Count; collInd++)
                {
                    data = booklist.Items[collInd].ToString();
                    xlSheet.Cells[collInd + 1, rowInd+1] = data;

                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot; // внутренние вертикальные
                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot; // внутренние горизонтальные            
                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble; // верхняя внешняя
                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble; // правая внешняя
                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble; // левая внешняя
                    xlSheet.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble; // нижняя внешняя

                }
                

                //выбираем всю область данных
                xlSheetRange = xlSheet.UsedRange;

                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();

            }
            catch(Exception exp)
            {
                Trace.WriteLine(exp.StackTrace);
                MessageBox.Show(exp.ToString(), "Error", MessageBoxButton.OK);
            }
            finally
            {
                //Показываем ексель
                xlApp.Visible = true;

                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;

            }
        }
    }
}
