using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Embryo.Generic;

// TODO: Custom attributes

namespace Embryo.Visulise
{
    public class SpiderGraph : GH_Component
    {
        
        public SpiderGraph()
            : base("Spider Chart", "Spider", "Draws an n-legged spider of (normalised) data", "Embryo", "Visulise")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("D", "D", "Input number list", GH_ParamAccess.item);
            pManager.AddTextParameter("L", "L", "Optional legend (string list)", GH_ParamAccess.item);
            pManager.AddNumberParameter("W", "W", "Optional importance weightings (number list)", GH_ParamAccess.list);

            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //double myFloat = 0.0;
            //string myString = "";
            //List<double> myFloat2 = new List<double>();

            //if (!DA.GetData(0, ref myFloat)) { return; }
            //if (!DA.GetData(1, ref myString)) { return; }
            //DA.GetDataList(2, myFloat2);

        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("20dc2f27-e90d-4071-83ed-dd918de48621"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new SpiderGraphAttrib(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Spider_Chart_01;
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

