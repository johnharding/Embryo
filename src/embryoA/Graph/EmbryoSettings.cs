using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Rhino;
using Grasshopper;
using Embryo.Generic;
using System.Windows.Forms;
using Embryo.Params;
using Grasshopper.Kernel.Types;
using Embryo.Types;

/**************************************************************************
 * Random graph main embryo component
 * 1D version as recommended by Millar (2003) for least constrained system
 * Can be changed to 2d cartesian system at a later date
 * Do we need a way to untangle the spaghetti or do we just embrace it?
 **************************************************************************/

namespace Embryo.Graph
{
    public class EmbryoSettings : GH_Component
    {
        EM_Settings mySettings;

        public EmbryoSettings()
            : base("Embryo Settings", "Embryo", "Settings Component for Embryo", "Embryo", " Parent")
        {
            mySettings = new EM_Settings();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Preview", "Pr", "Preview the components in the generated graph",GH_ParamAccess.item, true);
            pm.AddIntervalParameter("Slider Domain", "SD", "Numeric domain for generated sliders", GH_ParamAccess.item, new Rhino.Geometry.Interval(-10.0, 10.0));
            pm.AddBooleanParameter("One-to-One", "OO", "If true, each component output can only be used once", GH_ParamAccess.item, false);
            pm.AddIntegerParameter("GridX", "GX", "Component spacing in the X direction (column spacing)", GH_ParamAccess.item, 120);
            pm.AddIntegerParameter("GridY", "GY", "Component spacing in the Y direction (row spacing)", GH_ParamAccess.item, 120);
            pm.AddBooleanParameter("Remove dead", "RD", "Removes components that have warnings or errors", GH_ParamAccess.item, false);
            pm.AddBooleanParameter("Only Terminals", "OT", "Previews only terminal components (i.e. with no children)", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            //pm.AddIntegerParameter("", "", "", GH_ParamAccess.item);
            pm.RegisterParam(new EM_SettingsParam(), "Settings", "Se", "User settings for Embryo");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Get data
            bool preview = false;
            DA.GetData(0, ref preview);

            Rhino.Geometry.Interval myInterval = new Rhino.Geometry.Interval(-10.0, 10.0);
            DA.GetData(1, ref myInterval);
            Grasshopper.Kernel.Types.GH_Interval interval = new Grasshopper.Kernel.Types.GH_Interval();
            Grasshopper.Kernel.GH_Convert.ToGHInterval(myInterval,  GH_Conversion.Primary, ref interval);

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
            //DA.SetData(0, 2);
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
