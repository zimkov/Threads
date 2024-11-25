using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsProject
{
    public class SimpleRecordClass
    {
        double[] arrayNumbers;
        double[] baseNumbers;
        public ConcurrentBag<double> simpleNumbers;
        public SimpleRecordClass() { }

        public SimpleRecordClass(double[] arrayNumbers, double[] baseNumbers, ConcurrentBag<double> simpleNumbers)
        {
            this.arrayNumbers = arrayNumbers;
            this.baseNumbers = baseNumbers;
            this.simpleNumbers = simpleNumbers;
        }

        public double[] GetArray() { return arrayNumbers; }
        public double[] GetBaseNumbers() { return baseNumbers; }

    }
}
