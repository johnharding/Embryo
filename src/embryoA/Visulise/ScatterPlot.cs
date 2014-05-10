using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;

// TODO: Custom attributes

namespace Embryo.Visulise
{
    public class ScatterPlot : GH_Component
    {

        public ScatterPlot()
            : base("ScatterPlot", "Scatter", "Draws an n-legged spider of (normalised) data", "Embryo", "Visulise")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "X Data", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Y Data", GH_ParamAccess.item);
            pManager.AddIntegerParameter("R", "R", "Display point radius", GH_ParamAccess.item, 3);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double myFloat = 0.0;
            double myFloat2 = 0.0;
            int myInt = 1;
            if (!DA.GetData(0, ref myFloat)) { return; }
            if (!DA.GetData(1, ref myFloat2)) { return; }
            DA.GetData(2, ref myInt);
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("6b290ec5-9299-40cf-b692-10fb06bef00f"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new ScatterPlotAttrib(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Scatter04;
            }
        }
        
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }


    }
}

