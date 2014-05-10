using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper.GUI.Canvas;

namespace Embryo.Generic
{
    public class EM_Slider
    {
        public Grasshopper.Kernel.Special.GH_NumberSlider Slider;
        private int row;

        public EM_Slider(Grasshopper.Kernel.Special.GH_NumberSlider localSlider, bool isExist)
        {
            Slider = localSlider;
            row = 0;
            Alive = true;
            isExisting = isExist;
        }

        public bool Alive { get; set; }
        public bool isExisting { get; set; }

        public void addInput(int compRef, int outRef, List<EM_Component> componentList)
        {
            //int[] numbers = new int[2];
            //numbers[0] = compRef;
            //numbers[1] = outRef;
            //connections.Add(numbers);

        }

        public int getRow()
        {
            return row;
        }

        public void drawState(GH_Canvas canvas, Graphics graphics)
        {
            PointF origin = Slider.Attributes.Pivot;
            graphics.FillEllipse(Brushes.Black, origin.X, origin.Y, 4, 4);
        }

    }
}
