using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace lab2
{
    internal class Program
    {
        
        static double[] SieveEratosthenes(double[] numbers)
        {
            List<double> baseNumbers = numbers.ToList();
            for (int i = 0; i < baseNumbers.Count(); i++)
            {
                for (int j = 0; j < baseNumbers.Count(); j++)
                {
                    if (baseNumbers[j] == 1)
                    {
                        baseNumbers.Remove(baseNumbers[j]);
                    }
                    if (baseNumbers[j] % baseNumbers[i] == 0 && baseNumbers[i] != baseNumbers[j])
                    {
                        baseNumbers.Remove(baseNumbers[j]);
                    }
                }
            }
            return baseNumbers.ToArray();
        }

        public static string GetSimpleNumbers(double[] array, SimpleNumbersClass algorithm, int countThreads)
        {
            int countBaseNumbers = (int)Math.Sqrt(array.Length);
            double[] basenumbers = new double[countBaseNumbers];

            for (int i = 0; i < countBaseNumbers; i++)
            {
                basenumbers[i] = array[i];
            }

            int countN = array.Length - countBaseNumbers;
            double[] arrayNumbers = new double[countN];

            for (int i = 0; i < arrayNumbers.Length; i++)
            {
                arrayNumbers[i] = array[i + countBaseNumbers];
            }
            basenumbers = SieveEratosthenes(basenumbers);

            ConcurrentDictionary<double, double> simpleNumbers = new ConcurrentDictionary<double, double>();

            simpleNumbers = algorithm.FindSimple(arrayNumbers, basenumbers, simpleNumbers, countThreads);

            double[] resultArray = new double[simpleNumbers.Count];

            int countValue = 0;
            foreach (double value in simpleNumbers.Values)
            {
                resultArray[countValue] = value;
                countValue++;
            }

            Array.Sort(resultArray);

            string result = "";

            foreach (double number in basenumbers)
            {
                result += number + " ";
            }

            foreach (double number in resultArray)
            {
                result += number + " ";
            }

            return result;

        }


        static void Main(string[] args)
        {

            string path = @"C:\Users\Admin\Desktop\Alexei\numbers.txt";
            int N = 100;
            int M = 4;
            SimpleNumbersClass algorithm = new SimpleNumbersClass("Алгоритм3", new Algorithm3());

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
                        Console.WriteLine($"Не удалось преобразовать строку: {part}");
                        return;
                    }
                }
            }

            Console.WriteLine(GetSimpleNumbers(listNumbers.ToArray(), algorithm, M));
        }
    }
}
