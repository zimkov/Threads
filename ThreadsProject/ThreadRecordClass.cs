using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsProject
{
    public class ThreadRecordClass
    {
        private List<double> decompositionNumbers;
        public MathFunctionClass mathFunc;
        private double K = 1;
        private int threadId;

        public ThreadRecordClass()
        {

        }
        public ThreadRecordClass(List<double> decompositionNumbers, double k, int threadId, MathFunctionClass math)
        {
            this.decompositionNumbers = decompositionNumbers;
            this.K = k;
            this.threadId = threadId;
            this.mathFunc = math;
        }

        public List<double> GetNumbers()
        {
            if (decompositionNumbers == null)
            {
                Console.WriteLine("Ошибка ThreadClass: Не присвоенно значение decompositionNumbers");
                return null;
            }
            else
            {
                return decompositionNumbers;
            }
        }

        public double GetK()
        {
            return K;
        }

        public int GetThreadId()
        {
            return threadId;
        }
    }
}
