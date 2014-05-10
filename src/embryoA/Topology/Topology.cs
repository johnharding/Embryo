using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;

namespace Embryo.Topology
{
    class Topology
    {
        List<OutputList>links;

        public Topology()
        {
            links = new List<OutputList>();
        }

        public void addcompIndex(int componentIndex, int outputIndex)
        {

            links.Add(new OutputList(componentIndex));
        }


    }
}
