using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsProject
{
    public interface IMathFunction
    {
        void DoMath(object array, double k, int threadId);
    }

    class Multiply : IMathFunction
    {
        public void DoMath(object array, double k, int threadId)
        {
            if (array != null)
            {
                if (array is List<double> doubleList)
                {
                    for (int i = 0; i < doubleList.Count; i++)
                    {
                        doubleList[i] = doubleList[i] * k;
                        //Console.Write(doubleList[i] + " ");
                    }
                    Console.WriteLine($"Поток #{threadId}: Выполнено умножение всех чисел на {k}");
                }
                else
                {
                    Console.WriteLine("Ошибка IMathFunction: Ожидается тип double");
                }
            }
        }
    }

    class Power : IMathFunction
    {
        public void DoMath(object array, double k, int threadId)
        {
            if (array != null)
            {
                if (array is List<double> doubleList)
                {
                    for (int i = 0; i < doubleList.Count; i++)
                    {
                        doubleList[i] = Math.Pow(doubleList[i], k);
                        //Console.Write(doubleList[i] + " ");
                    }
                    Console.WriteLine($"Поток #{threadId}: Выполнено возведение всех чисел в степень {k}");
                }
                else
                {
                    Console.WriteLine("Ошибка IMathFunction: Ожидается тип double");
                }
            }
        }
    }

    class Factorial : IMathFunction
    {
        public void DoMath(object array, double k, int threadId)
        {
            if (array != null)
            {
                if (array is List<double> doubleList)
                {
                    for (int i = 0; i < doubleList.Count; i++)
                    {
                        double N = doubleList[i];

                        for (int j = 1; j < N; j++)
                        {
                            doubleList[i] = doubleList[i] * j;
                        }
                        //Console.Write(doubleList[i] + " ");
                    }
                    Console.WriteLine($"Поток #{threadId}: Вычислен факториал для всех чисел");
                }
                else
                {
                    Console.WriteLine("Ошибка IMathFunction: Ожидается тип double");
                }
            }
        }
    }

    class Fibonacci : IMathFunction 
    {
        public void DoMath(object array, double k, int threadId)
        {
            if (array != null)
            {
                if (array is List<double> doubleList)
                {
                    for (int i = 0; i < doubleList.Count; i++)
                    {
                        double a = 1;
                        double b = 1;
                        double c = 0;
                        double j = 3;

                        double N = doubleList[i];
                        if (N < j)
                        {
                            doubleList[i] = 1;
                            //Console.Write(doubleList[i] + " ");
                            continue;
                        }
                        while (j <= N)
                        {
                            c = b;
                            b = a + b;
                            a = c;
                            j++;
                        }

                        doubleList[i] = b;
                        //Console.Write(doubleList[i] +  " ");
                    }
                    Console.WriteLine($"Поток #{threadId}: Вычислены числа Фибоначи для каждого числа");
                }
                else
                {
                    Console.WriteLine("Ошибка IMathFunction: Ожидается тип double");
                }
            }
        }
    }

    public class MathFunctionClass
    {
        public string nameFunc;
        public IMathFunction mathFunction {  private get; set; }
        public MathFunctionClass(string nameFunc, IMathFunction math) 
        {
            this.nameFunc = nameFunc;
            mathFunction = math;
        }

        public void DoMath(object array, double k, int threadId)
        {
            mathFunction.DoMath(array, k, threadId);
        }
        public override string ToString() => $"{nameFunc}";
    }
}
