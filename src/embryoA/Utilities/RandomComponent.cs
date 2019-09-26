using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper;
using System.Collections.Generic;
using Rhino.Geometry;
using Embryo.Generic;

namespace Embryo.Utilities
{
    public class RandomComponent : GH_Component
    {

        int counter = 0;

        public RandomComponent()
            : base("RandomComponent", "RandomComponent", "Generates a random component from your tabs", "Embryo", "Parent")
        {
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Go", "Go", "Trigger random component generation", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsStandard", "St", "If true, selects from standard components only", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("ResetLocation", "Rl", "Resets the component pivot location counter", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool go = false;
            bool isStandard = true;
            bool resetLocation = false;
            if (!DA.GetData(0, ref go)){ return; }
            if (!DA.GetData(1, ref isStandard)){ return; }
            if (!DA.GetData(2, ref resetLocation)) { return; }

            Grasshopper.GUI.Ribbon.GH_Layout myRibbon = Instances.ComponentServer.CompleteRibbonLayout;

            if (resetLocation) counter = 0;

            if (go)
            {
                try
                {

                    Random rnd = new Random();
                    int t = rnd.Next(1, myRibbon.TabCount);
                    if (isStandard) t = t % 10;
                    int p = rnd.Next(0, myRibbon.Tabs[t].Panels.Count);
                    int i = rnd.Next(0, myRibbon.Tabs[t].Panels[p].Items.Count);
                    Guid myId = Instances.ComponentServer.CompleteRibbonLayout.Tabs[t].Panels[p].Items[i].Id;
                    GH_Component myComponent = (GH_Component)Instances.ComponentServer.EmitObject(myId);

                    myComponent.Attributes.Pivot = new System.Drawing.PointF(this.Attributes.Pivot.X + 200 + counter % 1000, this.Attributes.Pivot.Y + Math.Abs(counter / 1000) * 150);

                    
                    OnPingDocument().AddObject(myComponent, false);

                    
                    counter += 100;
                }
                catch
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Ooops. ID is might be a parameter or something funky.");
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("93020D78-CB78-447D-987B-2C149EF0306C"); }
        }


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

