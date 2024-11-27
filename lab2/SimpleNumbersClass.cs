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

namespace lab2
{
    public interface ISimple
    {
        ConcurrentDictionary<double, double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentDictionary<double, double> simpleNumbers, int countThreads);
    }

    //Параллельный алгоритм #1: декомпозиция по данным
    class Algorithm1 : ISimple
    {
        ConcurrentDictionary<double, double> simpleNumbers = new ConcurrentDictionary<double, double>();
        private static void RunThreadFunc(object array)
        {
            if (array is SimpleRecordClass threadClass)
            {
                double[] baseNumbers = threadClass.GetBaseNumbers();
                ConcurrentDictionary<double, double> arrayNumbers = threadClass.simpleNumbers;


                foreach (double baseN in baseNumbers)
                {
                    foreach (var number in arrayNumbers.Values)
                    {
                        if (number % baseN == 0)
                        {
                            arrayNumbers.TryRemove(number, out _);
                        }
                    }
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

        public ConcurrentDictionary<double, double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentDictionary<double, double> simpleNumbers, int countThreads)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(arrayNumbers, countThreads);

            ConcurrentDictionary<double, double> result = simpleNumbers;

            foreach (double number in arrayNumbers)
            {
                result.TryAdd(number, number);
            }

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
        ConcurrentDictionary<double, double> simpleNumbers = new ConcurrentDictionary<double, double>();
        private static void RunThreadFunc(object array)
        {
            if (array is SimpleRecordClass threadClass)
            {
                double[] baseNumbers = threadClass.GetBaseNumbers();
                ConcurrentDictionary<double, double> arrayNumbers = threadClass.simpleNumbers;
                

                foreach (double baseN in baseNumbers)
                {
                    foreach (var number in arrayNumbers.Values)
                    {
                        if (number % baseN == 0)
                        {
                            arrayNumbers.TryRemove(number, out _);
                        }
                    }
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

        public ConcurrentDictionary<double, double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentDictionary<double, double> simpleNumbers, int countThreads)
        {
            List<List<double>> decompositionNumbers = CircularDecomposition(basenumbers, countThreads);

            ConcurrentDictionary<double, double> result = simpleNumbers;

            foreach (double number in arrayNumbers)
            {
                result.TryAdd(number, number);
            }

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

            return result;
        }
    }



    //Параллельный алгоритм #3: применение пула потоков
    class Algorithm3 : ISimple
    {
        ConcurrentDictionary<double, double> simpleNumbers = new ConcurrentDictionary<double, double>();
        private static void RunThreadFunc(object array)
        {
            if (array is object[] obj)
            {
                double baseNumbers = (double)obj[0];
                ManualResetEvent ev = ((object[])obj)[1] as ManualResetEvent;
                ConcurrentDictionary<double, double> arrayNumbers = (ConcurrentDictionary<double, double>)obj[2];

                foreach(var number in arrayNumbers.Values)
                {
                    if (number % baseNumbers == 0)
                    {
                        arrayNumbers.TryRemove(number, out _);
                    }
                }

                ev.Set();
            }
        }

        public ConcurrentDictionary<double, double> FindSimple(double[] arrayNumbers, double[] basenumbers, ConcurrentDictionary<double, double> simpleNumbers, int countThreads)
        {
            foreach (double number in arrayNumbers)
            {
                simpleNumbers.TryAdd(number, number);
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

        public ConcurrentDictionary<double, double> FindSimple(double[] array, double[] arraySimple, ConcurrentDictionary<double, double> simpleNumbers, int countThreads)
        {
            return Algorithm.FindSimple(array, arraySimple, simpleNumbers, countThreads);
        }
        public override string ToString() => $"{nameFunc}";
    }
}
