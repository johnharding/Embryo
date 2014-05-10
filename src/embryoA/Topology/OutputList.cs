using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;

namespace Embryo.Topology
{
    class OutputList
    {
        int componentRef;
        List<int>outputs;
        
        public OutputList(int compRef)
        {
            componentRef = compRef;
            outputs = new List<int>();
        }

        public void addcompIndex(int outputIndex)
        {    
            outputs.Add(outputIndex);
        }

        public void clearList()
        {
            outputs.Clear();
        }


    }
}
