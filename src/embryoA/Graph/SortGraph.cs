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
    public class SortGraph : GH_Component
    {

        List<Guid> guidList;

        public SortGraph()
            : base("Topological sort", "TopoSort", "Topologically sorts an existing graph for better understanding", "Embryo", "Cognise")
        {
            guidList = new List<Guid>();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            //pm.Register_GenericParam("Instances", "G", "Instance GUID list", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            pm.AddGenericParameter("Components", "Co", "A list of the generated components", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            // Grab the canvas
            // As there is no rewiring, using event handlers should not be needed.
            Grasshopper.GUI.Canvas.GH_Canvas canvas = Instances.ActiveCanvas;
            
        }
        


        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("85e917ed-7467-4e07-b14c-c6b95f9bc63c"); }
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
                return Properties.Resources.RemixDAG01;
            }
        }
    }
}
