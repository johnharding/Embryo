using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;
using Embryo.Generic;
using Embryo.Types;

namespace Embryo.Params
{
    public class EM_SettingsParam : GH_PersistentParam<EM_Goo>
    {
        public EM_SettingsParam():
            base(new GH_InstanceDescription("Settings", "Settings", "Represents a collection of Embryo Settings", "Embryo", "Params"))
        {
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{bbb1f34e-2fe3-4c9c-b49e-983e7457c6b5}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary | GH_Exposure.primary;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.SettingsParam01;
            }
        }

        protected override GH_GetterResult Prompt_Singular(ref EM_Goo value)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<EM_Goo> values)
        {
            return GH_GetterResult.success;
        }

    }
}







