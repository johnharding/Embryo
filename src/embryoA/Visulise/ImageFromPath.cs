﻿using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;

// TODO: Custom attributes

namespace Embryo.Visulise
{
    public class ImageFromPath : GH_Component
    {

        public ImageFromPath()
            : base("ImageFromPath", "Image", "Displays an image from a path string", "Embryo", "Visulise")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("P", "P", "File Path", GH_ParamAccess.item, "Path");
            pManager.AddNumberParameter("S", "S", "Scale Factor", GH_ParamAccess.item, 1.0);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("b827f677-1542-4360-b66c-5e7fa9885181"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new ImageFromPathAttrib(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.ImageFromPath01;
            }
        }
        
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }


    }
}

