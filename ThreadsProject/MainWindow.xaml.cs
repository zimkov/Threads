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

namespace ThreadsProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path;
        int N, M, K;
        MathFunctionClass mathFunc;
        List<double> listNumbers;
        public MainWindow()
        {
            InitializeComponent();
            NComboBox.ItemsSource = new int[] { 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000};
            NComboBox.SelectedIndex = 1;
            MComboBox.ItemsSource = new int[] { 1, 2, 3, 4, 5, 10, 20, 30, 40, 50, 100 };
            MComboBox.SelectedIndex = 1;
            MathComboBox.ItemsSource = new MathFunctionClass[]
            {
                new MathFunctionClass("Умножение", new Multiply()),
                new MathFunctionClass("Возведение в степень", new Power()),
                new MathFunctionClass("Факториал", new Factorial()),
                new MathFunctionClass("Фибоначи", new Fibonacci())
            };
            MathComboBox.SelectedIndex = 0;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            path = PathTextBox.Text;
            N = (int)NComboBox.SelectedItem;
            M = (int)MComboBox.SelectedItem;
            K = int.Parse(KTextBox.Text);
            mathFunc = (MathFunctionClass)MathComboBox.SelectedItem;
            TimeResultLabel.Content = ThreadClass.CreateThreadPool(this.listNumbers.ToArray(), M, K, mathFunc);
        }

        private void GenerateFile_Click(object sender, RoutedEventArgs e)
        {
            path = String.Format(@PathTextBox.Text);
            File.WriteAllText(path, "");
            for (int i = 1; i <= N; i++)
            {
                File.AppendAllText(path, i + " ");
            }
            File.AppendAllText(path, N + "");
            Console.WriteLine("Файл заполнен числами от 1 до " + N);
        }

        private void NComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            N = (int)NComboBox.SelectedItem;
        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            path = String.Format(@PathTextBox.Text);
            List<double> listNumbers = new List<double>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    foreach (var part in parts)
                    {
                        if (double.TryParse(part, out double number))
                        {
                            listNumbers.Add(number);
                            //Console.Write(number + " ");
                        }
                        else
                        {
                            Console.WriteLine($"Не удалось преобразовать строку: {part}");
                        }
                    }
                }
            }
            Console.WriteLine("Файл прочитан\n\n");
            this.listNumbers = listNumbers;
        }

        private void MComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            M = (int)MComboBox.SelectedItem;
        }

        private void MathComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MathComboBox.SelectedItem == null)
            {
                //Обработка ошибки
            }
            if (MathComboBox.SelectedItem is MathFunctionClass mathFunction)
            {
                mathFunc = mathFunction;
            }
        }
    }
}
