using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper;
using System.Collections.Generic;
using Rhino.Geometry;

namespace Embryo.Utilities
{
    public class BrepVolume : GH_Component
    {
        public BrepVolume()
            : base("FastBrepVolume", "FastBrepVolume", "Finds the volume of a Brep without the centroid (for speed). Use at own risk!", "Embryo", "Utilities")
        {
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Input Brep", "Br", "Input Brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Volume", "Vo", "Brep volume", GH_ParamAccess.item);
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep mySurface = null;
            if (!DA.GetData(0, ref mySurface)){ return; }
            DA.SetData(0, mySurface.GetVolume());

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("B1C0B989-8139-47C8-9F58-FE8931DE1862"); }
        }


        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Area01;
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

