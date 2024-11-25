using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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


        public static string GetSimpleNumbers(double[] array, SimpleNumbersClass algorithm)
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

            ConcurrentBag<double> simpleNumbers = new ConcurrentBag<double>();

            simpleNumbers = algorithm.FindSimple(arrayNumbers, basenumbers, simpleNumbers);

            double[] resultArray = simpleNumbers.ToArray();
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

            return result;

        }

    }
}
