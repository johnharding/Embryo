using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Embryo.Params;
using Embryo.Generic;
using Embryo.Types;

// TODO: Custom attributes

namespace Embryo.Utilities
{
    public class ParentInput : GH_Component
    {
        public string paramType;
        private EM_InputParam myParam;
        private EM_Default localDefault;

        public ParentInput()
            : base("Embryo Input", "Embryo Input", "Tag parameters on the parent cavas that can be used by Embryo as inputs", "Embryo", "Parent")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.Register_GenericParam("I", "I", "Attach to the input parameter to be used by Embryo");
            myParam = new EM_InputParam();
            localDefault = new EM_Default();
            myParam.PersistentData.Append(new EM_Input(localDefault));
            pManager.AddParameter(myParam, "I", "I", "Connect to any parameters on the parent canvas that can be used by Embryo as inputs", GH_ParamAccess.item);

            pManager[0].WireDisplay = GH_ParamWireDisplay.faint;
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("68b3b695-40ba-4f50-a270-ffec7738129e"); }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.ChildInput02;
            }
        }

        public override void CreateAttributes()
        {
            m_attributes = new ParentInputAttrib(this);
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

