﻿using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Rhino;
using Grasshopper;
using Embryo.Generic;
using System.Windows.Forms;

/**************************************************************************
 * Sort Graph Component
 * Takes the instance guidlist and sorts them numerically
 **************************************************************************/

namespace Embryo.Graph
{
    public class SortGraph : GH_Component
    {
        // Grasshopper canvas pointer
        Grasshopper.GUI.Canvas.GH_Canvas canvas;
        private List<EM_Component> existingComponents;

        List<Guid> guidList;

        public SortGraph()
            : base("Topological sort", "TopoSort", "Topologically sorts an existing graph for better understanding", "Embryo", "Cognise")
        {
            canvas = Instances.ActiveCanvas;
            guidList = new List<Guid>();
            existingComponents = new List<EM_Component>();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Reset", "Go", "Begins a new Embryo solution", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            pm.AddGenericParameter("Components", "Co", "A list of the generated components", GH_ParamAccess.list);
            pm.AddIntegerParameter("meh", "meh", "meh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            existingComponents.Clear();

            // Now the setup
            bool myData = false;
            if (!DA.GetData("Reset", ref myData)) { return; }

            // Set up an event so that everything happens after Grasshopper expires the whole solution
            if (myData == false)
            {
                try
                {
                    canvas.Document.SolutionEnd -= new GH_Document.SolutionEndEventHandler(TopSort);
                }
                catch
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something has gone seriously wrong, and to be honest I'm not sure what to do about it");
                }
            }

            else
            {
                try
                {
                    canvas.Document.SolutionEnd -= new GH_Document.SolutionEndEventHandler(TopSort);
                }
                catch
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something has gone seriously wrong, and to be honest I'm not sure what to do about it");
                }
                canvas.Document.SolutionEnd += new GH_Document.SolutionEndEventHandler(TopSort);
            }

            DA.SetDataList(0, existingComponents);
            DA.SetData(1, existingComponents.Count);


        }



        public void TopSort(object sender, GH_SolutionEventArgs e)
        {
            List<IGH_ActiveObject> canvasObject = e.Document.ActiveObjects();

            // Get things
            for (int i = 0; i < canvasObject.Count; i++)
            {
                string george = canvasObject[i].ComponentGuid.ToString();

                // If we're not the mother component
                if (!george.Equals("85e917ed-7467-4e07-b14c-c6b95f9bc63c"))
                {
                    try
                    {
                        EM_Component eComponent = new EM_Component((GH_Component)canvasObject[i], true);
                        existingComponents.Add(eComponent);
                    }
                    catch
                    {
                    }
                }
            }

            // Sort things




            for (int i = 0; i < existingComponents.Count; i++)
            {
                existingComponents[i].Component.Attributes.Pivot = new PointF(40*i, 40*i);

            }


            for (int i = 0; i < existingComponents.Count; i++)
            {
                existingComponents[i].Component.ExpireSolution(false);
            }


            this.ExpireSolution(false);

            // Compute all objects downstream of this component (such as cognise ones)
            List<IGH_ActiveObject> myList2 = OnPingDocument().FindAllDownstreamObjects(this);
            foreach (IGH_ActiveObject myObject in myList2)
            {
                myObject.ClearData();
                myObject.CollectData();
                myObject.ComputeData();
            }    

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("85e917ed-7467-4e07-b14c-c6b95f9bc63c"); }
        }

        //public override void CreateAttributes()
        //{
        //    m_attributes = new EmbryoMainAttrib(this);
        //}

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.RemixDAG01;
            }
        }
    }
}
