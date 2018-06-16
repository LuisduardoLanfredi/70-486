using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam70486
{
    public class DependencyInjectionTester : IDependencyInjectionTester
    {
        private int i = 0;

        public DependencyInjectionTester()
        {
            i++;
        }

        public int Testing()
        {
            return i;
        }
    }
}
