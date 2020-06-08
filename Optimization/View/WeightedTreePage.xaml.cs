using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WeightedTree;

namespace Optimization.View
{
    /// <summary>
    /// Логика взаимодействия для WeightedTreePage.xaml
    /// </summary>
    public partial class WeightedTreePage : Page
    {
        public WeightedTreePage()
        {
            InitializeComponent();
        }

        public class TableRow
        {
            public string symbol { get; set; }
            public int frequency { get; set; }
            public string code { get; set; }
        }

        private TreePoints[][] drawingPoints;
        private double x1, x2, y1, y2, wP, hP;
        private Tree Tree;
        private List<string> str1 = new List<string>();
        private List<int> str2 = new List<int>();
        private int countLevel;
        private static string currDir = Environment.CurrentDirectory.ToString();
        private string FilePath;
        private bool emptyData = true;
        private Window enterDataWindow = null;

        private void Form_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!emptyData)
            {
                TablePrint();
                TreeDrawing();
            }
        }

        private void DownloadFromFile_Click(object sender, RoutedEventArgs e)
        {
            str1 = new List<string>();
            str2 = new List<int>();
            Tree = new Tree();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                FilePath = openFileDialog.FileName;
            else FilePath = currDir + @"\test1.txt";

            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string lineCurrent = sr.ReadLine();
                        string[] words = lineCurrent.Split(' ');
                        str1.Add(words[0]);
                        str2.Add(Convert.ToInt32(words[1]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка считывания. Будет загружен документ \"test1.txt\" ");
                DownloadTest();
            }
            emptyData = false;
            if (CheckData(str1))
                WorkWithTree();
            else
            {
                MessageBox.Show("Среди дабавляемых символов содержались 2 одинаковых символа. Построение такого дерева невозможно. Будет загружен документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    FilePath = saveFileDialog.FileName;
                else FilePath = currDir + @"\Savedtest.txt";

                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    for (int i = 0; i < str1.Count; i++)
                        sw.WriteLine(str1[i] + " " + str2[i]);
                }
            }
            else MessageBox.Show("Дерево - пусто. Сохранение не будет произведено.");
        }

        private TextBox tBData;
        private void EnterData_Click(object sender, RoutedEventArgs e)
        {
            enterDataWindow = new Window();
            enterDataWindow.Title = "Взвешенные деревья";
            enterDataWindow.Height = 200;
            enterDataWindow.Width = 500;

            tBData = new TextBox();
            tBData.Height = 80;
            tBData.Width = 300;
            tBData.HorizontalAlignment = HorizontalAlignment.Left;
            tBData.VerticalAlignment = VerticalAlignment.Bottom;
            tBData.Margin = new Thickness(20, 0, 0, 20);
            tBData.TextWrapping = TextWrapping.Wrap;

            Button b = new Button();
            b.Content = "Ok";
            b.Click += B_Click;
            b.Height = 50;
            b.Width = 150;
            b.VerticalAlignment = VerticalAlignment.Bottom;
            b.HorizontalAlignment = HorizontalAlignment.Right;
            b.Margin = new Thickness(0, 0, 10, 20);
            b.Background = Brushes.SkyBlue;

            TextBlock tB = new TextBlock() { Text = "   Ввод производится через пробел, каждая пара значений состоит из символа и его частоты. После ввода нажмите на кнопку \"Ok\" " };
            tB.TextWrapping = TextWrapping.Wrap;
            tB.VerticalAlignment = VerticalAlignment.Top;
            tB.HorizontalAlignment = HorizontalAlignment.Center;
            tB.Width = 400;
            tB.Margin = new Thickness(0, 20, 0, 0);
            tB.Foreground = Brushes.DarkSlateBlue;

            Grid q = new Grid();
            q.Children.Add(tBData);
            q.Background = Brushes.LightBlue;
            q.Children.Add(b);
            q.Children.Add(tB);
            enterDataWindow.Content = q;
            enterDataWindow.Show();

        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            str1 = new List<string>();
            str2 = new List<int>();

            string data = tBData.Text;
            try
            {
                string[] words = data.Split(' ');
                for (int i = 0; i < words.Length / 2; i++)
                {
                    str1.Add(words[2 * i]);
                    str2.Add(Convert.ToInt32(words[2 * i + 1]));
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка считывания. Будет загружен документ \"test1.txt\" ");

                DownloadTest();
            }
            enterDataWindow.Close();

            emptyData = false;
            Tree = new Tree();
            if (CheckData(str1))
                WorkWithTree();
            else
            {
                MessageBox.Show("Среди дабавляемых символов содержались 2 одинаковых символа. Построение такого дерева невозможно. Будет загружен документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                for (int i = 0; i < str1.Count; i++)
                    if (tBNewEl.Text == str1[i])
                    {
                        MessageBox.Show(" Элемент уже содержится в дереве. \n\n Примечание: в дереве невозможно сoдержание одного елемента с разным весом!");
                        return;
                    }

                str1.Add(tBNewEl.Text);
                str2.Add(Convert.ToInt32(tBNewW.Text));
                Tree = new Tree();
                WorkWithTree();
            }
            else MessageBox.Show("  Дерево пусто! Для начала работы с деревом, заполните его, для этого нажмите \"Загрузить данные\" или \"Ввести данные\".");
        }

        private void DeleteElement_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                bool deleted = false;
                if (!emptyData && Tree.count > 2)
                {
                    for (int i = 0; i < str1.Count; i++)
                        if (tBNewEl.Text == str1[i] && Convert.ToInt32(tBNewW.Text) == str2[i])
                        {
                            str1.RemoveAt(i);
                            str2.RemoveAt(i);
                            deleted = true;
                        }

                    if (deleted)
                    {
                        Tree = new Tree();
                        WorkWithTree();
                    }
                    else MessageBox.Show(" Введенный элемент не содержится в дереве. \n\n Примечание: для удаления элемента также необходимо верно указать его вес!");
                }
                else MessageBox.Show(" Дерево содержит меньше двух элементов. Удаление не будет произведено!");
            }
            else MessageBox.Show("  Дерево пусто! Для начала работы с деревом, заполните его, для этого нажмите \"Загрузить данные\" или \"Ввести данные\".");
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            if (!emptyData)
            {
                textBox2.Text = "";
                string co = textBox1.Text;
                string[] reply = co.Split(' ');
                for (int i = 0; i < reply.Length; i++)
                {
                    reply[i] = Tree.FindNode(reply[i]);
                    textBox2.AppendText(reply[i] + ' ');
                }
            }
            else MessageBox.Show("  Дерево пусто! Для начала работы с деревом, заполните его, для этого нажмите \"Загрузить данные\" или \"Ввести данные\".");
        }

        public void DownloadTest()
        {
            str1 = new List<string>();
            str2 = new List<int>();
            using (StreamReader sr = new StreamReader(currDir + @"\test1.txt"))
            {
                while (sr.Peek() >= 0)
                {
                    string lineCurrent = sr.ReadLine();
                    string[] words = lineCurrent.Split(' ');
                    str1.Add(words[0]);
                    str2.Add(Convert.ToInt32(words[1]));
                }
            }
        }

        private List<TableRow> table;
        public void TablePrint()
        {
            table = new List<TableRow>();
            for (int i = 0; i < Tree.table.Count; i++)
                table.Add(new TableRow { symbol = str1[i], frequency = str2[i], code = Tree.table[str1[i]] });

            dataGrid1.ItemsSource = table;
            dataGrid1.Columns[0].Width = (dataGrid1.ActualWidth - 10.0) / 4.0;
            dataGrid1.Columns[1].Width = (dataGrid1.ActualWidth - 10.0) / 4.0;
            dataGrid1.Columns[2].Width = (dataGrid1.ActualWidth - 10.0) / 2.0;
            dataGrid1.Columns[0].Header = "Символ";
            dataGrid1.Columns[1].Header = "Частота";
            dataGrid1.Columns[2].Header = "Код";
        }

        public void TreeDrawing()
        {
            pictureCanvas.Children.Clear();

            x1 = 10;
            x2 = pictureCanvas.ActualWidth - 30;
            y1 = 5;
            y2 = pictureCanvas.ActualHeight - 30;
            wP = x2 - x1;
            hP = y2 - y1;

            countLevel = Tree.levelCount;
            drawingPoints = Tree.Print(drawingPoints, x1, x2, y1, y2, wP, hP);
            string[][] key = Tree.k;

            for (int i = 0; i < countLevel; i++)
            {
                for (int j = 0; j < drawingPoints[i].Length; j++)
                {
                    if (drawingPoints[i][j].X != 0)
                    {
                        if (i != countLevel - 1 && drawingPoints[i + 1][2 * j].X != 0)
                        {
                            Line a = new Line();
                            a.X1 = drawingPoints[i][j].X;
                            a.X2 = drawingPoints[i + 1][2 * j].X;
                            a.Y1 = drawingPoints[i][j].Y;
                            a.Y2 = drawingPoints[i + 1][2 * j].Y;
                            a.Stroke = Brushes.DarkBlue;
                            pictureCanvas.Children.Add(a);
                        }

                        if (i != countLevel - 1 && drawingPoints[i + 1][2 * j + 1].X != 0)
                        {
                            Line a = new Line();
                            a.X1 = drawingPoints[i][j].X;
                            a.X2 = drawingPoints[i + 1][2 * j + 1].X;
                            a.Y1 = drawingPoints[i][j].Y;
                            a.Y2 = drawingPoints[i + 1][2 * j + 1].Y;
                            a.Stroke = Brushes.DarkBlue;
                            pictureCanvas.Children.Add(a);
                        }

                        Canvas myCanvers = new Canvas();
                        Canvas.SetLeft(myCanvers, drawingPoints[i][j].X - 5);
                        Canvas.SetTop(myCanvers, drawingPoints[i][j].Y - 5);
                        myCanvers.Width = 20;
                        myCanvers.Height = 20;

                        Ellipse el = new Ellipse();
                        el.Width = 20;
                        el.Height = 20;
                        el.Stroke = Brushes.Black;
                        el.Fill = Brushes.Aqua;

                        TextBlock tx = new TextBlock() { Text = key[i][j] };
                        tx.HorizontalAlignment = HorizontalAlignment.Center;
                        tx.Foreground = Brushes.BlueViolet;
                        //tx.FontSize = 3;
                        Canvas.SetLeft(tx, 5);

                        myCanvers.Children.Add(el);
                        myCanvers.Children.Add(tx);
                        pictureCanvas.Children.Add(myCanvers);

                    }
                }
            }
        }

        public void WorkWithTree()
        {
            if (str1.Count > 1)
            {
                for (int i = 0; i < str1.Count; i++)
                    Tree.CreateQue(str1[i], str2[i]);

                Tree.CreateTree();
                TablePrint();
                Tree.levelCount = DetermeCountLevel();
                TreeDrawing();
            }
            else
            {
                MessageBox.Show("Дерево содержит меньше двух элементов. Будет загружен документ \"test1.txt\" ");
                DownloadTest();
                WorkWithTree();
            }
        }

        public int DetermeCountLevel()
        {
            int length = table[0].code.Length;
            for (int i = 0; i < table.Count; i++)
                if (length < table[i].code.Length)
                    length = table[i].code.Length;

            return length + 1;
        }

        public bool CheckData(List<string> s)
        {
            bool clean = true;
            string str;
            for (int i = 0; i < s.Count; i++)
            {
                str = s[i];
                for (int j = 0; j < s.Count; j++)
                {
                    if (str == s[j] && i != j)
                        clean = false;
                }
            }
            return clean;
        }
    }
}
