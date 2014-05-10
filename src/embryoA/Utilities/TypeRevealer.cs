using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using Grasshopper;
using System.Collections.Generic;

namespace Embryo.Utilities
{
    public class TypeRevealer : GH_Component
    {
        public TypeRevealer()
            : base("Type Revealer", "TR", "Shows the GH Parameter Type", "Embryo", "Utilities")
        {
        }

        //will this be here?
        //I presume that override overides the inherited methods?
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "The input component", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Component Class", "C", "the grasshopper component class name");
            pManager.Register_StringParam("Parameter Type Name", "P", "the data type name of the parameter<T>");
            pManager.Register_StringParam("Parameter Class", "P", "the grasshopper parameter class name");
            pManager.Register_StringParam("Volatile Data Type", "V", "the volatile data type");
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare a variable for the input String
            object obj = null;
            DA.GetData(0, ref obj);
            if (obj == null) { return; }

            Grasshopper.GUI.Canvas.GH_Canvas canvas = Instances.ActiveCanvas;
            List<IGH_ActiveObject> canvasObject = canvas.Document.ActiveObjects();

            for (int i = 0; i < canvasObject.Count; i++)
            {
                if (this.DependsOn(canvasObject[i]))
                {
                    // 0. Component Type
                    DA.SetData(0, canvasObject[i].GetType().ToString());

                    // 1. First output param of component 'TYPE'
                    GH_Component tempThing = (GH_Component)canvasObject[i];
                    DA.SetData(1, tempThing.Params.Output[0].TypeName);
                    DA.SetData(2, tempThing.Params.Output[0].GetType().ToString());

                }
            }
            
            DA.SetData(3, obj.GetType().ToString());

        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("59ca94d4-c842-434d-8311-b5db83fac582"); }
        }


        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.T2;
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

    }
}

