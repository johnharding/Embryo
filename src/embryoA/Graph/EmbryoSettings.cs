using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Generic;
using Embryo.Params;
using Grasshopper.Kernel.Types;
using Embryo.Types;

namespace Embryo.Graph
{
    public class EmbryoSettings : GH_Component
    {
        EM_Settings mySettings;

        public EmbryoSettings()
            : base("Embryo Settings", "Embryo", "Settings Component for Embryo", "Embryo", "Parent")
        {
            mySettings = new EM_Settings();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Preview", "Preview", "Preview the components in the generated graph",GH_ParamAccess.item, true);
            pm.AddIntervalParameter("SliderDomain", "SliderDomain", "Numeric domain for generated sliders", GH_ParamAccess.item, new Rhino.Geometry.Interval(-10.0, 10.0));
            pm.AddBooleanParameter("One-to-One", "One-to-One", "If true, each component output can only be used once", GH_ParamAccess.item, true);
            pm.AddIntegerParameter("X Spacing", "X Spacing", "Component spacing in the X direction (column spacing)", GH_ParamAccess.item, 120);
            pm.AddIntegerParameter("Y Spacing", "Y Spacing", "Component spacing in the Y direction (row spacing)", GH_ParamAccess.item, 120);
            pm.AddBooleanParameter("RemoveDead", "RemoveDead", "Removes components that have warnings or errors", GH_ParamAccess.item, true);
            pm.AddBooleanParameter("TerminalsOnly", "TerminalsOnly", "Previews only terminal components (i.e. with no children)", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            pm.RegisterParam(new EM_SettingsParam(), "Settings", "Settings", "User settings for Embryo");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Get data
            bool preview = false;
            DA.GetData(0, ref preview);

            Rhino.Geometry.Interval myInterval = new Rhino.Geometry.Interval(-10.0, 10.0);
            DA.GetData(1, ref myInterval);
            GH_Interval interval = new GH_Interval();
            GH_Convert.ToGHInterval(myInterval,  GH_Conversion.Primary, ref interval);

            bool oneToOne = false;
            DA.GetData(2, ref oneToOne);

            int gridX = 120;
            DA.GetData(3, ref gridX);

            int gridY = 100;
            DA.GetData(4, ref gridY);

            bool removeDead = false;
            DA.GetData(5, ref removeDead);

            bool onlyTerms = false;
            DA.GetData(6, ref onlyTerms);

            // Set data
            mySettings.Preview = preview;
            mySettings.Interval = interval;
            mySettings.OneToOne = oneToOne;
            mySettings.GridX = gridX;
            mySettings.GridY = gridY;
            mySettings.RemoveDead = removeDead;
            mySettings.OnlyTerms = onlyTerms;

            EM_Goo mywrap = new EM_Goo(mySettings);

            // Finally, output the local settings file
            DA.SetData(0, mywrap);

        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("1990a9c5-d815-4d82-808e-112b91834659"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Settings01;
            }
        }
    }
}
