using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsProject
{
    public interface ISimple
    {
        ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers);
    }

    //Параллельный алгоритм #1: декомпозиция по данным
    class Algorithm1 : ISimple
    {
        ConcurrentBag<double> simpleNumbers = new ConcurrentBag<double>();
        private static void RunThreadFunc(object array)
        {
            if (array is SimpleRecordClass threadClass)
            {
                double[] baseNumbers = threadClass.GetBaseNumbers();
                double[] arrayNumbers = threadClass.GetArray();
                int n = (int)Math.Sqrt(arrayNumbers.Length);

                List<double> simple = arrayNumbers.ToList();

                foreach(double i in baseNumbers)
                {
                    for (int j = simple.Count - 1; j >= 0; j--)
                    {
                        if (simple[j] % i == 0)
                        {
                            simple.Remove(simple[j]);
                        }
                    }
                }
                foreach(double number in simple)
                {
                    threadClass.simpleNumbers.Add(number);
                }

            }
        }

        private static List<List<double>> CircularDecomposition(double[] array, int k)
        {
            int N = array.Length;
            List<List<double>> subarrays = new List<List<double>>();

            // Инициализация подмассивов
            for (int i = 0; i < k; i++)
            {
                subarrays.Add(new List<double>());
            }

            // Распределение элементов по подмассивам
            for (int i = 0; i < N; i++)
            {
                subarrays[i % k].Add(array[i]);
            }

            return subarrays;
        }

        public ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(arrayNumbers, basenumbers.Length);
            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Создаем массив потоков
            Thread[] threads = new Thread[basenumbers.Length];
            SimpleRecordClass[] threadClass = new SimpleRecordClass[basenumbers.Length];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(RunThreadFunc);
                threads[i].Name = $"Поток {i}";
            }

            // Запускаем все потоки
            for (int i = 0; i < threads.Length; i++)
            {
                threadClass[i] = new SimpleRecordClass(decompositionNumbers[i].ToArray(), basenumbers, simpleNumbers);
                threads[i].Start(threadClass[i]);
            }

            // Ожидаем завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Останавливаем таймер и выводим время выполнения всех потоков
            timer.Stop();
            return simpleNumbers;
        }
    }


    //Параллельный алгоритм #2: декомпозиция набора простых чисел
    class Algorithm2 : ISimple
    {
        ConcurrentBag<double> simpleNumbers = new ConcurrentBag<double>();
        private static void RunThreadFunc(object array)
        {
            if (array is SimpleRecordClass threadClass)
            {
                double[] baseNumbers = threadClass.GetBaseNumbers();
                double[] arrayNumbers = threadClass.GetArray();
                int n = (int)Math.Sqrt(arrayNumbers.Length);

                List<double> simple = arrayNumbers.ToList();

                foreach (double i in baseNumbers)
                {
                    for (int j = simple.Count - 1; j >= 0; j--)
                    {
                        if (simple[j] % i == 0)
                        {
                            simple.Remove(simple[j]);
                        }
                    }
                }
                foreach (double number in simple)
                {
                    threadClass.simpleNumbers.Add(number);
                }

            }
        }

        private static List<List<double>> CircularDecomposition(double[] array, int k)
        {
            int N = array.Length;
            List<List<double>> subarrays = new List<List<double>>();

            // Инициализация подмассивов
            for (int i = 0; i < k; i++)
            {
                subarrays.Add(new List<double>());
            }

            // Распределение элементов по подмассивам
            for (int i = 0; i < N; i++)
            {
                subarrays[i % k].Add(array[i]);
            }

            return subarrays;
        }

        public ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(arrayNumbers, basenumbers.Length);
            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Создаем массив потоков
            Thread[] threads = new Thread[basenumbers.Length];
            SimpleRecordClass[] threadClass = new SimpleRecordClass[basenumbers.Length];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(RunThreadFunc);
                threads[i].Name = $"Поток {i}";
            }

            // Запускаем все потоки
            for (int i = 0; i < threads.Length; i++)
            {
                threadClass[i] = new SimpleRecordClass(decompositionNumbers[i].ToArray(), basenumbers, simpleNumbers);
                threads[i].Start(threadClass[i]);
            }

            // Ожидаем завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Останавливаем таймер и выводим время выполнения всех потоков
            timer.Stop();
            return simpleNumbers;
        }
    }




    public class SimpleNumbersClass
    {
        public string nameFunc;
        public ISimple Algorithm { private get; set; }
        public SimpleNumbersClass(string nameFunc, ISimple algorithm)
        {
            this.nameFunc = nameFunc;
            Algorithm = algorithm;
        }

        public ConcurrentBag<double> FindSimple(double[] array, double[] arraySimple, ConcurrentBag<double> simpleNumbers)
        {
            return Algorithm.FindSimple(array, arraySimple, simpleNumbers);
        }
        public override string ToString() => $"{nameFunc}";
    }
}
