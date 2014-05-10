using System;
using System.Drawing;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Rhino;
using Grasshopper;

namespace Embryo.Graph
{
    public class EmbryoFix2 : GH_Component
    {
        private List<GH_Component> myComponents;
        Grasshopper.GUI.Canvas.GH_Canvas canvas;
        private int counter;

        public EmbryoFix2()
            : base("That", "Component", "Does", "Not", "Work!")
        {
            canvas = Instances.ActiveCanvas;
            counter = 0;
            myComponents = new List<GH_Component>();
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
            if (token) AddPointComponent();
            //else RemovePointComponent();

            return;
        }

        private void AddPointComponent()
        {
            canvas.Document.SolutionStart += new GH_Document.SolutionStartEventHandler(ModifyComponents);
        }

        private void RemovePointComponent()
        {
            // Remove the ModifyComponents method so not called next time around
            canvas.Document.SolutionStart -= new GH_Document.SolutionStartEventHandler(ModifyComponents);
        }

        public void ModifyComponents(object sender, GH_SolutionEventArgs e)
        {
            if (counter > 0) e.Document.RemoveObjects(myComponents, false);
            myComponents.Clear();

            // Emit point from componentserver and instantiate
            for (int i = 0; i < 10; i++)
            {
                GH_Component tC = (GH_Component)Instances.ComponentServer.EmitObject(new Guid("3581f42a-9592-4549-bd6b-1c0fc39d067b"));
                canvas.InstantiateNewObject(tC, null, new PointF(50*i, 50*i), false);
                myComponents.Add(tC);
            }
            //e.Document.AddObject(myComponent, false);

            for (int i = 1; i < myComponents.Count; i++)
            {
                myComponents[i].Params.Input[1].AddSource(myComponents[i - 1].Params.Output[0]);

                myComponents[i].ClearData();
                myComponents[i].CollectData();
                myComponents[i].ComputeData();
            }

            counter++;

            canvas.Document.SolutionStart -= new GH_Document.SolutionStartEventHandler(ModifyComponents);

        }

        public override Guid ComponentGuid
        {
            // Generated at http://www.newguid.com/
            get { return new Guid("998db140-c783-49eb-bd0e-ec02921b2cbf"); }
        }
    }
}
