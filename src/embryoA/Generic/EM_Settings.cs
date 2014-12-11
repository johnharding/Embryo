using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Embryo.Generic
{
    public class EM_Settings
    {
        public bool Preview {get; set;}
        public Grasshopper.Kernel.Types.GH_Interval Interval { get; set; }
        public bool OneToOne { get; set; }
        public int MaxRowNo { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public bool RemoveDead { get; set; }
        public bool OnlyTerms { get; set; }
        public bool ExpireSolution { get; set; }

        public EM_Settings()
        {
            Preview = true;

            Grasshopper.Kernel.Types.GH_Interval tempInterval = new Grasshopper.Kernel.Types.GH_Interval();
            GH_Convert.ToGHInterval(new Rhino.Geometry.Interval(-10.0, 10.0), GH_Conversion.Primary, ref tempInterval);
            Interval = tempInterval;

            OneToOne = true;
            MaxRowNo = 16;
            GridX = 120;
            GridY = 100;

            RemoveDead = true;
            OnlyTerms = false;

            ExpireSolution = false;

        }


    }
}
