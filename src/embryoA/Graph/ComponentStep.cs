using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Rhino;
using Grasshopper;
using Embryo.Generic;
using System.Windows.Forms;

/**************************************************************************
 * Sort Graph Component
 * Takes the instance guidlist and sorts them numerically
 **************************************************************************/

namespace Embryo.Graph
{
    public class ComponentStep : GH_Component
    {

        public ComponentStep()
            : base("ComponentStep", "ComStep", "Steps through the graph previewing each component step", "Embryo", "Cognise")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddGenericParameter("Components", "Co", "List of Embryo Components", GH_ParamAccess.list);
            pm.AddIntegerParameter("Last component", "Lc", "Previews the graph up to and including this component", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            int myEndPoint = 0;
            if (!DA.GetData(1, ref myEndPoint)) { return; }
            if (myEndPoint < 0) { return; }
            
            List<EM_Component> myComponents = new List<EM_Component>();
            if (!DA.GetDataList<EM_Component>(0, myComponents)) { return; }

            if (myEndPoint > myComponents.Count-1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Index higher than current component count!");
                myEndPoint = myComponents.Count-1;
            }

            // Sort the components
            for (int i = 0; i < myComponents.Count; i++)
            {
                if (i > myEndPoint)
                {
                    myComponents[i].Component.Hidden = true;
                }
                else
                {
                    myComponents[i].Component.Hidden = false;
                }
            }
        }


        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("c44c002a-8d14-4349-8df5-28fb05e3465e"); }
        }

        //public override void CreateAttributes()
        //{
        //    m_attributes = new EmbryoMainAttrib(this);
        //}
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.ComponentStep;
            }
        }
    }
}
