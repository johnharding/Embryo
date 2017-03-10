using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper;

namespace Embryo.Generic
{
    public class Friends
    {
        // Shuffles Embryo Components
        public static void ShuffleComponents(List<EM_Component> list, int seed)
        {
            Random rng;
            if (seed == 0) rng = new Random();
            else rng = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                EM_Component value = list[k];
                list[k] = list[n];
                list[n] = value;

            }
        }

        // Shuffles things of object type
        public static void ShuffleObjects(List<object> list, int seed)
        {
            Random rng;
            if (seed == 0) rng = new Random();
            else rng = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                object value = list[k];
                list[k] = list[n];
                list[n] = value;

            }
        }

        // Shuffles things of object type
        public static void ShuffleDoubles(List<double> list, int seed)
        {
            Random rng;
            if (seed == 0) rng = new Random();
            else rng = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                double value = list[k];
                list[k] = list[n];
                list[n] = value;

            }
        }

        // Shuffles things of object type
        public static void ShuffleIntegers(List<int> list, int seed)
        {
            Random rng;
            if (seed == 0) rng = new Random();
            else rng = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;

            }
        }

        // Returns the largest column out of a list of components
        public static int GetMaxCol(List<EM_Component> myComponents)
        {
            int maxCol = 0;
            for (int i = 0; i < myComponents.Count; i++)
            {
                // Only if it's alive
                //if (myComponents[i].Alive)
                //{
                int thisCol = myComponents[i].GetColumn();
                if (thisCol > maxCol) maxCol = thisCol;
                //}
            }
            return maxCol;
        }

        // Returns the largest double value in a list
        public static double MaxDouble(List<double> myList)
        {
            double maxDouble = 0.0;
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i] > maxDouble) maxDouble = myList[i];
            }
            return maxDouble;
        }

        // Default colour for Embryo components
        public static Color EM_Colour(){
            return Color.SlateGray;
        }

        //public void RemoveObject(object sender, EventArgs e)
        //{
        //    Grasshopper.GUI.Canvas.GH_Canvas canvas = Instances.ActiveCanvas;
        //    canvas.Document.RemoveObject(myComponent, false);
        //}

    }
}
