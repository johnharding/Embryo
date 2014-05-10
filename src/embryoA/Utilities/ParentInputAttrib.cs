using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using Embryo.Generic;

namespace Embryo.Utilities
{
    public class ParentInputAttrib : GH_ComponentAttributes
    {
        public ParentInputAttrib(ParentInput owner)
            : base(owner)
        {
        }

        protected override void Layout()
        {
            //    //Lock this object to the pixel grid. 
            //    //I.e., do not allow it to be position in between pixels.
            //    Pivot = GH_Convert.ToPoint(Pivot);
            Bounds = new RectangleF(Pivot, new SizeF(40, 32));
            RectangleF myRect1 = new RectangleF(Bounds.Location, Bounds.Size);
            RectangleF myRect2 = new RectangleF(Bounds.Location, Bounds.Size);
            myRect1.X -= 12;
            LayoutOutputParams(Owner, myRect1);
            LayoutInputParams(Owner, myRect2);

        }




        public override void ExpireLayout()
        {    
            base.ExpireLayout();

          // Destroy any data you have that becomes 
          // invalid when the layout expires.
        }


        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }

            if (channel == GH_CanvasChannel.Objects)
            {

                //base.Render(canvas, graphics, channel);

                Pen myPen;
                SolidBrush myBrush;

                if (Owner.Params.Output[0].Recipients.Count != 0)
                {
                    myPen = new Pen(Color.Black, 3);
                    myBrush = new SolidBrush(Color.Black);
                }
                else
                {
                    myPen = new Pen(Friends.EM_Colour(), 3);
                    myBrush = new SolidBrush(Friends.EM_Colour());
                }

                graphics.FillEllipse(myBrush, Bounds.Location.X + 41 - 4, Bounds.Location.Y + 16 - 4, 8, 8);
                graphics.FillRectangle(myBrush, Rectangle.Round(Bounds));
                graphics.DrawRectangle(myPen, Rectangle.Round(Bounds));

                //RenderComponentParameters(canvas, graphics, Owner, new GH_PaletteStyle(Color.Azure));

                PointF iconLocation = new PointF(Pivot.X+8, Pivot.Y+4);
                graphics.DrawImage(Owner.Icon_24x24, iconLocation);

            }
        }



    }
}

