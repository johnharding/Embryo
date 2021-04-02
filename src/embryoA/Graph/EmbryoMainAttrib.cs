using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Embryo.Generic;

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

            Grasshopper.GUI.Canvas.GH_PaletteStyle styleStandard = null;

            if (channel == GH_CanvasChannel.Objects)
            {

                // 1. Cache the current styles
                styleStandard = GH_Skin.palette_normal_standard;
                GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.FromArgb(255 ,255, 255, 255), Color.FromArgb(255, 50, 50, 50), Color.Black);

                Pen myPen = new Pen(Color.FromArgb(255, 50, 50, 50), 1);

                GraphicsPath path = RoundedRectangle.Create((int)(Bounds.Location.X), (int)Bounds.Y - 16, (int)Bounds.Width, 16, 3);
                graphics.DrawPath(myPen, path);

                Font myFont = new Font(Grasshopper.Kernel.GH_FontServer.Standard.FontFamily, 5, FontStyle.Regular);
                StringFormat format = new StringFormat();

                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                graphics.DrawString("Version 0.6.0", myFont, Brushes.Black, (int)(Bounds.Location.X + (Bounds.Width / 2)), (int)Bounds.Location.Y-8, format);

                format.Dispose();


                // 2. Canvas Segmentation
                graphics.DrawLine(Pens.Black, 0, 0, 0, -100000);
                graphics.DrawLine(Pens.Black, 0, 0, -100000, 0);
                graphics.FillEllipse(Brushes.Black, -2, -2, 4, 4);

                Font font = new Font(Grasshopper.Kernel.GH_FontServer.Standard.FontFamily, 8);
                StringFormat format2 = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                graphics.DrawString("Child", font, Brushes.Black, 10, -20, format2);
                graphics.DrawString("Parent", font, Brushes.Black, 10, 20, format2);
                graphics.DrawString("Ingredients", font, Brushes.Black, -70, 20, format2);

            }

            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                // Restore the cached styles.
                GH_Skin.palette_normal_standard = styleStandard;

            }
        }
    }
}