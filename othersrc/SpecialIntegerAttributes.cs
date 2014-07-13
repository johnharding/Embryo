using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Embryo.Utilities
{
    public class SpecialIntegerAttributes : GH_Attributes<SpecialIntegerObject>
    {

        public SpecialIntegerAttributes(SpecialIntegerObject owner)
            : base(owner)
        {

        }

        public override bool HasInputGrip { get { return true; } }
        public override bool HasOutputGrip { get { return true; } }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                //Render output grip.
                GH_CapsuleRenderEngine.RenderOutputGrip(graphics, canvas.Viewport.Zoom, OutputGrip, true);

                //Render capsules.
                for (int col = 0; col < 4; col++)
                {
                    for (int row = 0; row < 4; row++)
                    {
                        int value = Value(col, row);
                        Rectangle button = Button(col, row);

                        GH_Palette palette = GH_Palette.Pink;
                        if (value == Owner.Value)
                            palette = GH_Palette.Black;
                       
                        GH_Capsule capsule = GH_Capsule.CreateTextCapsule(button, button, palette, value.ToString(), 0, 0);
                        capsule.Render(graphics, Selected, Owner.Locked, false);
                        capsule.Dispose();
                    }
                }
            }
        }
    }
}
