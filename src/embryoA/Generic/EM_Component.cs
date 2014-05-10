using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper.GUI.Canvas;

namespace Embryo.Generic
{
    /// <summary> 
    /// The child component class
    /// </summary> 
    public class EM_Component
    {
        public GH_Component Component;
        public List<int[]> connections;
        private int column, row;

        public EM_Component(GH_Component localComponent, bool isExist)
        {
            connections = new List<int[]>();
            Component = localComponent;
            column = 0;
            row = 0;
            Alive = true;
            isExisting = isExist;
        }

        public bool Alive { get; set; }
        public bool isExisting { get; set; }

        /// <summary> 
        /// Adds an input to the component
        /// </summary> 
        public void AddInput(int compRef, int outRef, List<EM_Component> componentList)
        {
            //int[] numbers = new int[2];
            //numbers[0] = compRef;
            //numbers[1] = outRef;
            //connections.Add(numbers);

            // Adjust your rank based on the rank of the connection
            if (componentList[compRef].column >= column)
            {
                // Always stay one rank ahead of the largest connected component
                column = componentList[compRef].column + 1;

            }
        }

        public void ClearConnections()
        {
            connections.Clear();
        }

        public void ResetCartesian()
        {
            column = 0;
            row = 0;
        }

        public int GetColumn()
        {
            return column;
        }

        public int GetRow()
        {
            return row;
        }

        public void DrawState(GH_Canvas canvas, Graphics graphics)
        {
            PointF origin = Component.Attributes.Pivot;
            graphics.FillEllipse(Brushes.Black, origin.X, origin.Y, 4, 4);
        }

    }
}
