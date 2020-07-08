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

                Font font = new Font("Arial", 8);
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                graphics.DrawString("Child", font, Brushes.Black, 10,-20, format);
                graphics.DrawString("Parent", font, Brushes.Black, 10, 20, format);
                graphics.DrawString("Ingredients", font, Brushes.Black, -70, 20, format);

                format.Dispose();

            }
        }
    }
}