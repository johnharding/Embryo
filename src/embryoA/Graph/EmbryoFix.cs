using System;
using System.Drawing;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Rhino;
using Grasshopper;

namespace Embryo.Graph
{
    public class EmbryoFix : GH_Component
    {
        private GH_Component myComponent;
        private Grasshopper.GUI.Canvas.GH_Canvas canvas;

        public EmbryoFix()
            : base("This", "Component", "Does", "Not", "Work!")
        {
            canvas = Instances.ActiveCanvas;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Delete Component", "Delete?", "Generates a new graph when set to true", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get data
            bool token = false;
            DA.GetData(0, ref token);

            // Add or remove component
            if (!token) AddPointComponent();
            else RemovePointComponent();

            return;
        }

        private void AddPointComponent()
        {
            // Emit point from componentserver and instantiate
            myComponent = (GH_Component)Instances.ComponentServer.EmitObject(new Guid("3581f42a-9592-4549-bd6b-1c0fc39d067b"));
            canvas.InstantiateNewObject(myComponent, null, new PointF(50, 50), false);
            
            // Locate Component
            myComponent.Attributes.Pivot = new PointF(200, 200);

            // Remove it upon next solution
            canvas.Document.SolutionStart += new GH_Document.SolutionStartEventHandler(RemoveComponent);
        }

        private void RemovePointComponent()
        {
            // Remove the removeObject method so not called next time around
            canvas.Document.SolutionStart -= new GH_Document.SolutionStartEventHandler(RemoveComponent);
        }

        public void RemoveComponent(object sender, GH_SolutionEventArgs e)
        {   
            e.Document.RemoveObject(myComponent, false);
        }

        public override Guid ComponentGuid
        {
            // Generated at http://www.newguid.com/
            get { return new Guid("29890c2f-2506-4a92-b4bf-8eeefb57cd17"); }
        }
    }
}
