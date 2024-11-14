using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsProject
{
    internal class ThreadClass
    {
        private static void RunThreadFunc(object array)
        {
            if (array is ThreadRecordClass threadClass)
            {
                threadClass.mathFunc.DoMath(threadClass.GetNumbers(), threadClass.GetK(), threadClass.GetThreadId());
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



        public static string CreateThreadPool(double[] array, int countThreads, double k, MathFunctionClass mathFunction)
        {
            // Производим круговую декомпозицию для равного распределения данных между потоками
            List<List<double>> decompositionNumbers = CircularDecomposition(array, countThreads);

            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Создаем массив потоков
            Thread[] threads = new Thread[countThreads];
            ThreadRecordClass[] threadClass = new ThreadRecordClass[countThreads];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(RunThreadFunc);
                threads[i].Name = $"Поток {i}";
            }

            // Запускаем все потоки
            for (int i = 0; i < threads.Length; i++)
            {
                threadClass[i] = new ThreadRecordClass(decompositionNumbers[i], k, i, mathFunction);
                threads[i].Start(threadClass[i]);
            }

            // Ожидаем завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Останавливаем таймер и выводим время выполнения всех потоков
            timer.Stop();
            return "Время выполнения всех потоков: " + timer.Elapsed.ToString();

        }
    }
}
