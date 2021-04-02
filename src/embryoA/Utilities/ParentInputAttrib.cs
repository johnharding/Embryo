
using System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Attributes;
using Embryo.Generic;
using System.Drawing.Drawing2D;

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
            // Lock this object to the pixel grid. 
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
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }

            if (channel == GH_CanvasChannel.Objects)
            {

                Pen myPen = new Pen(Color.FromArgb(255, 50, 50, 50));
                myPen.Width = 2;
                SolidBrush myBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
                SolidBrush myBrush2 = new SolidBrush(Color.FromArgb(150, 150, 150));

                graphics.FillEllipse(myBrush2, Bounds.Location.X + 40 - 4, Bounds.Location.Y + 16 - 4, 8, 8);

                Rectangle myRect = new Rectangle((int)Bounds.Location.X, (int)Bounds.Location.Y, (int)Bounds.Width, (int)Bounds.Height);
                GraphicsPath p = RoundedRectangle.Create(myRect, 3);

                graphics.DrawPath(myPen, p);
                graphics.FillPath(myBrush, p);

                PointF iconLocation = new PointF(Pivot.X+8, Pivot.Y+4);
                graphics.DrawImage(Owner.Icon_24x24, iconLocation);

            }
        }

    }
}

