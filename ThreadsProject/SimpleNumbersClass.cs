using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ThreadsProject
{
    public interface ISimple
    {
        ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers, int countThreads);
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

        public ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers, int countThreads)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(arrayNumbers, countThreads);
            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Создаем массив потоков
            Thread[] threads = new Thread[countThreads];
            SimpleRecordClass[] threadClass = new SimpleRecordClass[countThreads];

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

        public ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers, int countThreads)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(basenumbers, countThreads);
            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Создаем массив потоков
            Thread[] threads = new Thread[countThreads];
            SimpleRecordClass[] threadClass = new SimpleRecordClass[countThreads];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(RunThreadFunc);
                threads[i].Name = $"Поток {i}";
            }

            // Запускаем все потоки
            for (int i = 0; i < threads.Length; i++)
            {
                threadClass[i] = new SimpleRecordClass(arrayNumbers, decompositionNumbers[i].ToArray(), simpleNumbers);
                threads[i].Start(threadClass[i]);
            }

            // Ожидаем завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Останавливаем таймер и выводим время выполнения всех потоков
            timer.Stop();

            // Отсеивание повторов
            var count = simpleNumbers.GroupBy(x => x)
                             .ToDictionary(g => g.Key, g => g.Count());

            // Оставляем только те элементы, которые встречаются ровно countThreads раз
            var filtered = count.Where(kvp => kvp.Value == countThreads)
                                .Select(kvp => kvp.Key)
                                .ToList();
            ConcurrentBag<double> result = new ConcurrentBag<double>(filtered);

            return result;
        }
    }



    //Параллельный алгоритм #3: применение пула потоков
    class Algorithm3 : ISimple
    {
        ConcurrentBag<double> simpleNumbers = new ConcurrentBag<double>();
        private static void RunThreadFunc(object array)
        {
            if (array is object[] obj)
            {
                double baseNumbers = (double)obj[0];
                ConcurrentBag<double> arrayNumbers = (ConcurrentBag<double>)obj[2];
                //int n = (int)Math.Sqrt(arrayNumbers.Length);

                //List<double> simple = arrayNumbers.ToList();

                foreach (double number in arrayNumbers) 
                { 
                    if (number % baseNumbers == 0)
                    {

                    }
                }
                //for (int j = simple.Count - 1; j >= 0; j--)
                //{
                //    if (simple[j] % i == 0)
                //    {
                //        simple.Remove(simple[j]);
                //    }
                //}
                
                foreach (double number in simple)
                {
                    threadClass.simpleNumbers.Add(number);
                }

            }
        }

        public ConcurrentBag<double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentBag<double> simpleNumbers, int countThreads)
        {
            foreach (double number in arrayNumbers)
            {
                simpleNumbers.Add(number);
            }
            // Объявляем массив сигнальных сообщений
            ManualResetEvent[] events = new ManualResetEvent[basenumbers.Length];

            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Добавляем в пул рабочие элементы с параметрами
            for (int i = 0; i < basenumbers.Length; i++)
            {
                events[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(RunThreadFunc, new object[] { basenumbers[i], events[i], simpleNumbers });
            }
            // Дожидаемся завершения
            WaitHandle.WaitAll(events);

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

        public ConcurrentBag<double> FindSimple(double[] array, double[] arraySimple, ConcurrentBag<double> simpleNumbers, int countThreads)
        {
            return Algorithm.FindSimple(array, arraySimple, simpleNumbers, countThreads);
        }
        public override string ToString() => $"{nameFunc}";
    }
}
