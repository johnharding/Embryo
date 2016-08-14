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
            : base("ScatterPlot", "Scatter", "Draws an n-legged spider of (normalised) data", "Embryo", "Visualise")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "X Data", GH_ParamAccess.list);
            pManager.AddNumberParameter("Y", "Y", "Y Data", GH_ParamAccess.list);
            pManager.AddIntegerParameter("R", "R", "Display point radius", GH_ParamAccess.item, 3);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("122125f7-e599-473b-8d4a-6a47262ca888"); }
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

