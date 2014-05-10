using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper.GUI.Canvas;

namespace Embryo.Generic
{
    public class EM_Plug
    {
        public GH_Component Component;
        public List<IGH_Param> sourceClone;

        // Take a record of the input parameters
        public EM_Plug(GH_Component localComponent)
        {
            Component = localComponent;
            sourceClone = new List<IGH_Param>(localComponent.Params.Input[0].Sources);
        }

        // Reconnect any disconnected wires caused by Embryo processes
        public void Reconnect()
        {
            for (int i = 0; i < sourceClone.Count; i++)
            {
                Component.Params.Input[0].AddSource(sourceClone[i]);
            }
        }
    }
}
