using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// TODO: Custom attributes

namespace Embryo.Utilities
{
    public class ParentOutput : GH_Component
    {
        public string paramType;
        public bool selected;

        public ParentOutput()
            : base("Parent Output", "", "This component tags output parameters that can be used by Embryo", "Embryo", " Parent")
        {
            selected = false;
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("O", "O", "Attach to the output parameter to be used by Embryo", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare a variable for the input String
            object obj = null;
            DA.GetData(0, ref obj);

            if (obj == null) { return; }
            
            // TODO: Use Volatile Data
            //string outType = this.Params.Output[0].VolatileData.GetType().ToString();
            //IGH_Structure volData = this.Params.Input[0].VolatileData as IGH_Structure;
            //string outType = volData.get_Branch(0).GetType().ToString();
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("cccb5126-3224-41ec-8841-9e0d1603af8e"); }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.ChildOutput02;
            }
        }

        public override void CreateAttributes()
        {
            m_attributes = new ParentOutputAttrib(this);
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

