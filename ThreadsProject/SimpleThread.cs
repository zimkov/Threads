﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ThreadsProject
{
    public class SimpleThread
    {
        
        static double[] SieveEratosthenes(double[] numbers)
        {
            List<double> baseNumbers = numbers.ToList();
            for (int i = 0; i < baseNumbers.Count(); i++)
            {
                for (int j = 0; j < baseNumbers.Count(); j++)
                {
                    if(baseNumbers[j] == 1)
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

            for(int i = 0; i < countBaseNumbers; i++)
            {
                basenumbers[i] = array[i];
            }

            int countN = array.Length - countBaseNumbers;
            double[] arrayNumbers = new double[countN];

            for(int i = 0; i < arrayNumbers.Length; i++)
            {
                arrayNumbers[i] = array[i + countBaseNumbers];
            }
            basenumbers = SieveEratosthenes(basenumbers);

            ConcurrentDictionary<double, double> simpleNumbers = new ConcurrentDictionary<double, double>();

            string timeResult = "";
            // Запускаем таймер
            Stopwatch timer = new Stopwatch();
            timer.Start();

            simpleNumbers = algorithm.FindSimple(arrayNumbers, basenumbers, simpleNumbers, countThreads);

            // Останавливаем таймер и выводим время выполнения всех потоков
            timer.Stop();

            timeResult = "Время выполнения всех потоков: " + timer.Elapsed.ToString();

            double[] resultArray = new double[simpleNumbers.Count];

            int countValue = 0;
            foreach (double value in simpleNumbers.Values) 
            {
                resultArray[countValue] = value;
                countValue++;
            }

            Array.Sort(resultArray);

            string result = "";

            foreach(double number in basenumbers)
            {
                result += number + " ";
            }

            foreach (double number in resultArray)
            {
                result += number + " ";
            }

            result = timeResult + "\n\n" + result;

            return result;

        }

    }
}
