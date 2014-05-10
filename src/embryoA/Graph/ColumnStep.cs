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
    public class ColumnStep : GH_Component
    {

        List<Guid> guidList;

        public ColumnStep()
            : base("ColumnStep", "ColStep", "Steps through the graph previewing each column step-by-step", "Embryo", "Cognise")
        {
            guidList = new List<Guid>();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddGenericParameter("Components", "Co", "List of Embryo Components", GH_ParamAccess.list);
            pm.AddIntegerParameter("Last column", "Lc", "Previews the graph up to and including this column", GH_ParamAccess.item, 0); 
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

            // Get the last column index
            int maxCol = Friends.GetMaxCol(myComponents);

            // Check to see if we're over
            if (myEndPoint > maxCol)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Value higher than component count!");
                myEndPoint = maxCol;
            }

            // Set the previews
            for(int i=0; i< myComponents.Count; i++)
            {
                if (myComponents[i].GetColumn() > myEndPoint) myComponents[i].Component.Hidden = true;
                else myComponents[i].Component.Hidden = false;
            }

        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("e4f0194f-09f6-41d9-ad9c-41225b93c98a"); }
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
                return Properties.Resources.ColumnStep;
            }
        }
    }
}
