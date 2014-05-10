using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Embryo.Generic;

namespace Embryo.Visulise
{   //GH_Attributes<SpiderGraph>
    public class ScatterPlotAttrib : GH_ComponentAttributes
    {
        List<Point> myPoints;

        public ScatterPlotAttrib(ScatterPlot owner)
            : base(owner)
        {
            myPoints = new List<Point>();
        }
        
        // Out of my depth here...
        protected override void Layout()
        {
        //    //Lock this object to the pixel grid. 
        //    //I.e., do not allow it to be position in between pixels.
        //    Pivot = GH_Convert.ToPoint(Pivot);
            
            Bounds = new RectangleF(Pivot, new SizeF(540, 500));
            RectangleF myRect = new RectangleF(Bounds.Location, new SizeF(100f, 60f));
            myRect.X += 16;
            myRect.Y += 4;
            LayoutInputParams(Owner, myRect);

        }


        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
            {
                base.Render(canvas, graphics, channel);
                Layout();
            }

            if (channel == GH_CanvasChannel.Objects)
            {

                GH_Structure<GH_Number> dupData1 = (GH_Structure<GH_Number>)Owner.Params.Input[0].VolatileData;
                GH_Structure<GH_Number> dupData2 = (GH_Structure<GH_Number>)Owner.Params.Input[1].VolatileData;
                GH_Structure<GH_Integer> dupData3 = (GH_Structure<GH_Integer>)Owner.Params.Input[2].VolatileData;

                Pen pen;
                SolidBrush myBrush;

                if (Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Blank)
                {
                    pen = new Pen(Friends.EM_Colour(), 3);
                    myBrush = new SolidBrush(Friends.EM_Colour());
                }
                else
                {
                    pen = new Pen(Friends.EM_Colour(), 3);
                    myBrush = new SolidBrush(Friends.EM_Colour());
                }

                graphics.FillRectangle(myBrush, Rectangle.Round(Bounds));
                graphics.FillEllipse(myBrush, Bounds.Location.X - 4 - 1, Bounds.Location.Y + 14 - 4, 8, 8);
                graphics.FillEllipse(myBrush, Bounds.Location.X - 4 - 1, Bounds.Location.Y + 34 - 4, 8, 8);
                graphics.FillEllipse(myBrush, Bounds.Location.X - 4 - 1, Bounds.Location.Y + 54 - 4, 8, 8);

                graphics.DrawRectangle(pen, Rectangle.Round(Bounds));

                

                // Setup default font
                Font ubuntuFont = new Font("ubuntu", 8);

                // String Formatting
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                StringFormat format3 = new StringFormat();

                StringFormat format4 = new StringFormat();
                format4.Alignment = StringAlignment.Far;
                format4.Trimming = StringTrimming.EllipsisCharacter;

                // Draw stuff
                graphics.DrawString("X", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 14-6), format3);
                graphics.DrawString("Y", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 34-6), format3);
                graphics.DrawString("R", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 54-6), format3);


                Pen myPen = new Pen(Brushes.Black, 1);
                SolidBrush myWhiteBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

                if (!dupData1.IsEmpty && !dupData2.IsEmpty && !dupData3.IsEmpty)
                {
                    if (dupData1.Branches[0][0] != null && dupData2.Branches[0][0] != null && dupData3.Branches[0][0] != null && (dupData1.DataCount == dupData2.DataCount))
                    {
                        int pRadius = dupData3.get_DataItem(0).Value;


                        // Get the max X value
                        double maxX = 0;
                        double mulX = 1;
                        for (int i = 0; i < dupData1.DataCount; i++)
                        {
                            if (dupData1.get_DataItem(i).Value > maxX) maxX = dupData1.get_DataItem(i).Value;
                        }
                        if(maxX>0) mulX = 400 / maxX;

                        // Get the max Y value
                        double maxY = 0;
                        double mulY = 0;
                        for (int i = 0; i < dupData2.DataCount; i++)
                        {
                            if (dupData2.get_DataItem(i).Value > maxY) maxY = dupData2.get_DataItem(i).Value;
                        }
                        if(maxY>0) mulY = 400 / maxY;

                        // Now draw the points
                        for (int i = 0; i < dupData1.DataCount; i++)
                        {
                            int X =  (int)(dupData1.get_DataItem(i).Value*mulX) + (int)Bounds.Location.X + 60 + 20;
                            int Y = -(int)(dupData2.get_DataItem(i).Value * mulY) + (int)Bounds.Location.Y + 470 - 10;

                            myPoints.Add(new Point(X, Y));
                            graphics.FillEllipse(Brushes.White, myPoints[i].X - (pRadius / 2), myPoints[i].Y - (pRadius / 2), pRadius, pRadius);
                        }

                        // Draw the graph
                        double roundX = Math.Round(maxX, 2);
                        double roundY = Math.Round(maxY, 2);

                        graphics.DrawLine(myPen, Bounds.Location.X + 60 + 20, Bounds.Location.Y + 470 - 10, Bounds.Location.X + 460 + 20, Bounds.Location.Y + 470 - 10);
                        graphics.DrawLine(myPen, Bounds.Location.X + 60 + 20, Bounds.Location.Y + 470 - 10, Bounds.Location.X + 60 + 20, Bounds.Location.Y + 70 - 10);
                        graphics.DrawString(roundX.ToString(), ubuntuFont, Brushes.Black, Bounds.Location.X + 460 + 20, Bounds.Location.Y + 470 - 10, format3);
                        graphics.DrawString(roundY.ToString(), ubuntuFont, Brushes.Black, Bounds.Location.X + 58 + 20, Bounds.Location.Y + 64 - 10, format4);
                        graphics.DrawString("0", ubuntuFont, Brushes.Black, Bounds.Location.X + 60 + 20, Bounds.Location.Y + 470 - 10, format3);
                        graphics.DrawString("0", ubuntuFont, Brushes.Black, Bounds.Location.X + 58 + 20, Bounds.Location.Y + 458 - 10, format4);
                    }
                    else
                    {
                        Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Lists have to be matching in length and numeric in value");
                    }
                }
                else
                {
                    graphics.DrawString("johnharding@fastmail.fm", ubuntuFont, Brushes.White, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 480 - 6 - 10), format3);
                    graphics.DrawImage(Owner.Icon_24x24, Bounds.Location.X + 12, Bounds.Location.Y + 450 - 10);      
                }

                myPoints.Clear();
                format.Dispose();
                format3.Dispose();
                format4.Dispose();
                //Layout();
            }
        }

    }
}

