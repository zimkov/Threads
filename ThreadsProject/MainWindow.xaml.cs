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
        SimpleNumbersClass algorithm;
        List<double> listNumbers;
        public MainWindow()
        {
            InitializeComponent();

            //Лабораторная 1
            NComboBox.ItemsSource = new int[] { 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000};
            NComboBox.SelectedIndex = 1;
            MComboBox.ItemsSource = new int[] { 1, 2, 3, 4, 5, 10, 20, 30, 100 };
            MComboBox.SelectedIndex = 1;
            MathComboBox.ItemsSource = new MathFunctionClass[]
            {
                new MathFunctionClass("Умножение", new Multiply()),
                new MathFunctionClass("Возведение в степень", new Power()),
                new MathFunctionClass("Факториал", new Factorial()),
                new MathFunctionClass("Фибоначи", new Fibonacci())
            };
            MathComboBox.SelectedIndex = 0;


            //Лабораторная 2
            NComboBox2.ItemsSource = new int[] { 100, 10000, 1000000 };
            NComboBox2.SelectedIndex = 0;
            MComboBox2.ItemsSource = new int[] { 1, 2, 3, 4, 5, 10, 20, 30, 100 };
            MComboBox2.SelectedIndex = 1;
            AlgorithmComboBox.ItemsSource = new SimpleNumbersClass[]
            {
                new SimpleNumbersClass("Алгоритм1", new Algorithm1()),
                new SimpleNumbersClass("Алгоритм2", new Algorithm2())
            };
            AlgorithmComboBox.SelectedIndex = 0;

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            path = PathTextBox.Text;
            N = (int)NComboBox.SelectedItem;
            M = (int)MComboBox.SelectedItem;
            int cof;
            bool intParse = int.TryParse(KTextBox.Text, out cof);
            if (!intParse)
            {
                //Введено неправильное K
                ResultLabel.Text = "Значение коэффициента K должно быть целочисленное";
                return;
            }
            K = cof;
            mathFunc = (MathFunctionClass)MathComboBox.SelectedItem;
            ResultLabel.Text = ThreadClass.CreateThreadPool(this.listNumbers.ToArray(), M, K, mathFunc);
        }

        private void GenerateFile_Click(object sender, RoutedEventArgs e)
        {
            path = String.Format(@PathTextBox.Text);
            File.WriteAllText(path, "");
            string buffer = "";
            for (int i = 1; i < N; i += 100)
            {
                for (int j = i; j < i+100; j++)
                {
                    buffer += j + " ";
                }
                File.AppendAllText(path, buffer);
                buffer = "";
            }
            File.AppendAllText(path, N + "");
            ResultLabel.Text = "Файл заполнен числами от 1 до " + N;
            ResultLabel2.Text = "Файл заполнен числами от 1 до " + N;
        }

        private void NComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            N = (int)NComboBox.SelectedItem;
        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Text = "Идет чтение файла...";
            path = String.Format(@PathTextBox.Text);
            List<double> listNumbers = new List<double>();
            foreach (var line in File.ReadLines(path))
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
                        ResultLabel.Text = $"Не удалось преобразовать строку: {part}";
                        ResultLabel2.Text = $"Не удалось преобразовать строку: {part}";
                        return;
                    }
                }
            }
            ResultLabel.Text = "Файл прочитан";
            ResultLabel2.Text = "Файл прочитан";
            this.listNumbers = listNumbers;
        }

        private void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                PathTextBox.Text = filename;
                PathTextBox2.Text = filename;
            }
        }

        private void StartButton2_Click(object sender, RoutedEventArgs e)
        {
            path = PathTextBox2.Text;
            N = (int)NComboBox2.SelectedItem;
            M = (int)MComboBox2.SelectedItem;
            algorithm = (SimpleNumbersClass)AlgorithmComboBox.SelectedItem;
            ResultLabel2.Text = SimpleThread.GetSimpleNumbers(this.listNumbers.ToArray(), algorithm);
        }

        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmComboBox.SelectedItem == null)
            {
                //Обработка ошибки
            }
            if (AlgorithmComboBox.SelectedItem is SimpleNumbersClass simple)
            {
                algorithm = simple;
            }
        }

        private void NComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            N = (int)NComboBox2.SelectedItem;
        }

        private void MComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            M = (int)MComboBox2.SelectedItem;
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
