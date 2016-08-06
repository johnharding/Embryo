using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Embryo.Graph
{
    public class EmbryoMainAttrib : GH_ComponentAttributes
    {

        public EmbryoMainAttrib(EmbryoMain owner)
            : base(owner)
        {
        }
        
       

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
            }

            if (channel == GH_CanvasChannel.Objects)
            {
                // 1. Component Render
                base.Render(canvas, graphics, channel);

                // 2. Canvas Segmentation
                graphics.DrawLine(Pens.Black, 0, 0, 0, -100000);
                graphics.DrawLine(Pens.Black, 0, 0, -100000, 0);
                graphics.FillEllipse(Brushes.Black, -2, -2, 4, 4);

                Font ubuntuFont = new Font("ubuntu", 8);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                graphics.DrawString("Child", ubuntuFont, Brushes.Black, 10,-20, format);
                graphics.DrawString("Parent", ubuntuFont, Brushes.Black, 10, 20, format);
                graphics.DrawString("Ingredients", ubuntuFont, Brushes.Black, -80, 20, format);

                /*
                GH_Palette palette = GH_Palette.Pink;

                Color myColor = Color.LightGray;

                
                switch (Owner.RuntimeMessageLevel)
                {
                    case GH_RuntimeMessageLevel.Warning:
                        myColor = Color.Orange;
                        break;

                    case GH_RuntimeMessageLevel.Error:
                        myColor = Color.Red;
                        break;
                }
                 

                if (Owner.Hidden) myColor = Color.Gray;
                if (Owner.Locked) myColor = Color.DarkGray;

                RectangleF myRect = new RectangleF(Bounds.Location, Bounds.Size);
                GH_Capsule capsule = GH_Capsule.CreateCapsule(myRect, palette, 10, 0);

                capsule.Render(graphics, myColor);
                capsule.Dispose();
                capsule = null;

                base.RenderComponentCapsule(canvas, graphics, false, false, false, true, true, false);

                

                PointF iconLocation = new PointF(ContentBox.X-4, ContentBox.Y+70);
                graphics.DrawImage(Owner.Icon_24x24, iconLocation);
                */

                format.Dispose();

            }
        }

    }
}