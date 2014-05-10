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

    public class EmbryoAbout : GH_Component
    {
        public EmbryoAbout()
            : base(" About", " About", "Information about the Embryo project", "Embryo", " Parent")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ab", "Ab", "Information about Embryo", GH_ParamAccess.item);
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, "http://www.grasshopper3d.com/group/embryo/forum/topics/background-to-embryo");
        }

        public override Guid ComponentGuid
        {
            // Generated at http://www.newguid.com/
            get { return new Guid("177b9c6a-cc2b-4ac3-ab0c-5778875b39af"); }
        }

        //public override void CreateAttributes()
        //{
            //m_attributes = new GetGeometryAttrib(this);
        //}

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.About;
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }
    }
}

