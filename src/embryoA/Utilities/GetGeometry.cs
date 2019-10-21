using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Grasshopper;
using Embryo.Generic;
using Rhino.Geometry;

namespace Embryo.Utilities
{

    public class GetGeometry : GH_Component
    {
        public GetGeometry()
            : base("Get Geometry", "", "Gets the geometry from the child graph", "Embryo", "Parent")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("G", "G", "Collects geometry (Breps, Meshes, Surfaces, etc..) from the child graph", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("I", "I", "Hands over geometry (Breps, meshes, lines, etc..) from the child graph", GH_ParamAccess.list);
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Now get and set the inputs
            object obj = null;
            DA.GetData(0, ref obj);
            if (obj == null) { return; }
            DA.SetData(0, obj); 

        }

        public override Guid ComponentGuid
        {
            // Generated at http://www.newguid.com/
            get { return new Guid("56212076-52dd-44cc-aa82-aa5b31504302"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new GetGeometryAttrib(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.GetGeometry08;
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

    }
}

