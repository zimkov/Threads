using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class SimpleRecordClass
    {
        double[] arrayNumbers;
        double[] baseNumbers;
        public ConcurrentDictionary<double, double> simpleNumbers;
        public SimpleRecordClass() { }

        public SimpleRecordClass(double[] arrayNumbers, double[] baseNumbers, ConcurrentDictionary<double, double> simpleNumbers)
        {
            this.arrayNumbers = arrayNumbers;
            this.baseNumbers = baseNumbers;
            this.simpleNumbers = simpleNumbers;
        }

        public double[] GetArray() { return arrayNumbers; }
        public double[] GetBaseNumbers() { return baseNumbers; }

    }
}
