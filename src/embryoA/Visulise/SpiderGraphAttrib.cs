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
    public class SpiderGraphAttrib : GH_ComponentAttributes
    {
        int addY;

        public SpiderGraphAttrib(SpiderGraph owner)
            : base(owner)
        {
            addY = 0;
        }
        
        // I need to change to GH_Attributes to do this, but it doesn't seem to work!!!
        //public override bool HasInputGrip { get { return true; } }
        //public override bool HasOutputGrip { get { return false; } }

        // Out of my depth here...
        protected override void Layout()
        {
        //    //Lock this object to the pixel grid. 
        //    //I.e., do not allow it to be position in between pixels.
        //    Pivot = GH_Convert.ToPoint(Pivot);
            
            Bounds = new RectangleF(Pivot, new SizeF(500, 500 + addY));
            RectangleF myRect = new RectangleF(Bounds.Location, new SizeF(100f, 60f));
            myRect.X += 16;
            myRect.Y += 4;
            LayoutInputParams(Owner, myRect);
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
                Layout();
            }

            if (channel == GH_CanvasChannel.Objects)
            {
                bool isLegend = false;

                bool goObjectives = false;
                bool goWeightings = false;

                int centreX = (int)(Bounds.Location.X + 250);
                int centreY = (int)(Bounds.Location.Y + 250);

                GH_Structure<GH_Number> dupData1 = (GH_Structure<GH_Number>)Owner.Params.Input[0].VolatileData;
                GH_Structure<GH_String> dupData2 = (GH_Structure<GH_String>)Owner.Params.Input[1].VolatileData;
                GH_Structure<GH_Number> dupData3 = (GH_Structure<GH_Number>)Owner.Params.Input[2].VolatileData;

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


                // 1. Record the number of objectives in a new variable
                int objectives = dupData1.DataCount;
                if (objectives > 2)
                {
                    goObjectives = true;
                }
                else
                {
                    Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You must have 3 or more objectives");
                }

                // 2. Enlarge the capsule if a legend is included
                if (dupData1.DataCount == dupData2.DataCount) isLegend = true;
                if (isLegend) addY = objectives * 20 + 10;
                else { addY = 0; }

                // 3. Check that the weightings are within a boundary
                List<double> weightings = new List<double>();
                if (dupData1.DataCount == dupData3.DataCount){
                    for (int i = 0; i < objectives; i++)
                    {
                        weightings.Add(dupData3.get_DataItem(i).Value);
                    }
                }
                if (Friends.MaxDouble(weightings) < 1.0)
                {
                    goWeightings = true;
                }
                else
                {
                    Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Your weightings must not include a value over 1.0");
                }
               

                #region Draw the capsule

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

                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Near;
                format2.LineAlignment = StringAlignment.Near;
                format2.Trimming = StringTrimming.EllipsisCharacter;

                StringFormat format3 = new StringFormat();

                // Draw stuff
                graphics.DrawString("Objectives (>2)", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 14-6), format3);
                graphics.DrawString("Legend (op)", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 34 - 6), format3);
                graphics.DrawString("Weightings (op)", ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 54 - 6), format3);
                
                Pen myPen = new Pen(Brushes.Black,1);
                Pen myBluePen = new Pen(Brushes.White, 1);
                SolidBrush myWhiteBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));


                #endregion draw the capsule

                if (goObjectives && goWeightings)
                {
                    double angle = 2 * Math.PI / objectives;
                    Point[][] myPoints = new Point[6][];
                    Point[] myObjectives = new Point[objectives];

                    // 1. Calculate the list of points for the graph
                    for (int j = 0; j < objectives; j++)
                    {
                        if (isLegend) graphics.DrawString("" + (j + 1) + ". " + dupData2.get_DataItem(j).Value.ToString(), ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 20, (int)this.Bounds.Location.Y + 500 + (j * 20)), format2);

                        double myVal = dupData1.get_DataItem(j).Value;

                        //Stick within normalised values
                        if (myVal >= 0 && myVal <= 1)
                        {
                            // Add the weightings, if the data count is the same as the number of objectives
                            double multiplier = 1.0;
                            if (dupData3.DataCount == objectives) multiplier = dupData3.get_DataItem(j).Value;
                            int endX = (int)(centreX + Math.Cos(angle * j) * 200 * dupData1.get_DataItem(j).Value * multiplier);
                            int endY = (int)(centreY - Math.Sin(angle * j) * 200 * dupData1.get_DataItem(j).Value * multiplier);
                            myObjectives[j] = new Point(endX, endY);
                        }
                        else
                        {
                            Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data must be within a 0 to 1 range");
                            return;
                        }

                    }

                    // 2. Create Graph
                    for (int i = 0; i < 6; i++)
                    {
                        myPoints[i] = new Point[objectives];
                        for (int j = 0; j < objectives; j++)
                        {
                            double multiplier = 1.0;
                            if (dupData3.DataCount == objectives) multiplier = dupData3.get_DataItem(j).Value;

                            int endX = (int)(centreX + Math.Cos(angle * j) * 40 * i * multiplier);
                            int endY = (int)(centreY - Math.Sin(angle * j) * 40 * i * multiplier);
                            myPoints[i][j] = new Point(endX, endY);

                            // Put the reference in.
                            if (i == 5)
                            {
                                int index = j + 1;
                                int indexX = (int)(centreX + Math.Cos(angle * j) * 40 * i * multiplier) + (int)(Math.Cos(angle * j) * 2 * i);
                                int indexY = (int)(centreY - Math.Sin(angle * j) * 40 * i * multiplier) - (int)(Math.Sin(angle * j) * 2 * i);
                                graphics.DrawString(index.ToString(), ubuntuFont, Brushes.Black, new Point(indexX - 5, indexY - 7), format3);
                            }
                        }
                    }

                    // 3. Display Graph
                    graphics.FillEllipse(Brushes.Black, (float)(centreX) - 2.5f, (float)(centreY) - 2.5f, 5f, 5f);
                    for (int i = 0; i < 6; i++)
                    {
                        graphics.DrawPolygon(myBluePen, myPoints[i]);
                    }

                    for (int j = 0; j < objectives; j++)
                    {
                        graphics.DrawLine(myPen, myPoints[0][j], myPoints[5][j]);
                        graphics.FillEllipse(Brushes.Black, (float)myPoints[5][j].X - 1f, (float)myPoints[5][j].Y - 1f, 2f, 2f);
                    }

                    graphics.DrawString("Objectives = " + objectives.ToString(), ubuntuFont, Brushes.Black, new Point((int)this.Bounds.Location.X + 20, (int)this.Bounds.Location.Y + 460), format2);
                    graphics.FillPolygon(myWhiteBrush, myObjectives);
                    graphics.DrawPolygon(myPen, myObjectives);

                    for (int j = 0; j < objectives; j++)
                    {
                        graphics.FillEllipse(Brushes.Black, (float)myObjectives[j].X - 1.5f, (float)myObjectives[j].Y - 1.5f, 3f, 3f);
                    }
                }

                else
                {
                    graphics.DrawString("johnharding@fastmail.fm", ubuntuFont, Brushes.White, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 480 - 6), format3);
                    graphics.DrawImage(Owner.Icon_24x24, Bounds.Location.X + 12, Bounds.Location.Y + 450);
                }

                format.Dispose();
                format2.Dispose();
                format3.Dispose();
                // Layout();
            }
        }

    }
}

