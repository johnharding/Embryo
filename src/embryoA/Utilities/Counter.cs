using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper;
using System.Collections.Generic;
using Rhino.Geometry;

namespace Embryo.Utilities
{
    public class Counter : GH_Component
    {
        private int myCount;

        public Counter()
            : base("Counter", "Counter", "Increments by 1 when solved (connect to a timer). Useful with cognise tools to step through definitions", "Embryo", "Cognise")
        {
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Reset", "Re", "Reset the value to zero", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Count", "Ti", "Counter value", GH_ParamAccess.item);
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool reset = false;
            if (!DA.GetData(0, ref reset)){ return; }
            if (reset)
            {
                myCount = 0;
                DA.SetData(0, myCount);
                return;
            }
            else
            {
                myCount++;
                DA.SetData(0, myCount);
            }
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("207756ea-4a67-4b0c-9600-8d938044e0a0"); }
        }


        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Timer03;
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

    }
}

