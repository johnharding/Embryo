using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper;
using System.Collections.Generic;
using Rhino.Geometry;

namespace Embryo.Utilities
{
    public class UnRoll : GH_Component
    {
        public UnRoll()
            : base("Unroll", "Unroll", "Unrolls a Brep to the World XY plane", "Embryo", "Utilities")
        {
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Input Brep", "Br", "Brep that you wish to unroll", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_BRepParam("Output Brep", "Sr", "Surface that has been to unrolled", GH_ParamAccess.list);
            pManager.Register_BooleanParam("Same area?", "AI", "Returns true if the area is identical for I/O");
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep mySurface = null;
            if (!DA.GetData(0, ref mySurface)){ return; }
            
            Unroller me = new Unroller(mySurface);
            
            Curve[] crv;
            Point3d[] pt;
            TextDot[] txt;

            Brep[] newSurfaces = me.PerformUnroll(out crv, out pt, out txt);

            bool sameArea = false;

            // Initial Area
            double iArea = Math.Round(mySurface.GetArea(), 3);

            // New Area
            double nArea = 0;
            for (int i = 0; i < newSurfaces.Length; i++)
            {
                nArea += newSurfaces[i].GetArea();
            }
            nArea = Math.Round(nArea, 3);

            // Comparison
            if (iArea.Equals(nArea)) sameArea = true;
            
            DA.SetDataList(0, newSurfaces);
            DA.SetData(1, sameArea);
 
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("604bb3fa-6bb1-4883-a308-ce603c8be6b3"); }
        }


        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.UnRoll02;
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

