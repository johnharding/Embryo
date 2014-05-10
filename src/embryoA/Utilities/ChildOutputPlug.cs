using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;

// TODO: Custom attributes

namespace Embryo.Utilities
{
    public class ChildOutputPlug : GH_Component
    {
        public string paramType;

        public ChildOutputPlug()
            : base("ChildOutputPlug", "ChildOutputPlug", "Stops the output being used for existing (not generated) child components", "Embryo", "Child")
        {

        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("I", "I", "Connect the parameter here", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            return;
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("004e5436-b7ce-471b-9e45-392cb09e0fc4"); }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Dummy01;
            }
        }
        public override void CreateAttributes()
        {
            m_attributes = new ChildOutputPlugAttrib(this);
        }
    }
}

