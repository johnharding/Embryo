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
    public class EM_InputParam : GH_PersistentParam<EM_Input>
    {
        public EM_InputParam():
            base(new GH_InstanceDescription("ParentInput", "ParentInput", "Represents a collection of Embryo Parent Inputs", "Embryo", "Params"))
        {
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{be60beef-8bad-4043-a3d7-0a4c181de9f6}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden | GH_Exposure.hidden;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                //TODO: return a proper icon here.
                return Properties.Resources.P2;
            }
        }

        protected override GH_GetterResult Prompt_Singular(ref EM_Input value)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<EM_Input> values)
        {
            return GH_GetterResult.success;
        }


    }
}







