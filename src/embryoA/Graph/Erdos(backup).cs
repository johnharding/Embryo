﻿using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;
using Rhino;
using Grasshopper;
using Embryo.Generic;

/* Embryo
 * 1D version as recommended by Millar (2003) for least constrained system
 * Can be changed to 2d cartesian system at a later date
 * Do we need a way to untangle the spaghetti or do we just embrace it?
 */

namespace Embryo.Graph
{
    public class Erdos : GH_Component
    {
        public Erdos()
            : base("Random Graph 1D (Erdős–Rényi)", "Erdős 1D", "Generates a new 1D graph automatically based on the components present on the parent canvas", "Embryo", "Graph")
        {
        }
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.Register_BooleanParam("Reset Toggle", "G", "Generates a new graph when set to true");   
            pm.Register_IntegerParam("Slider Number", "S", "The number of sliders in the new graph (seeds)",16);
            pm.Register_IntegerParam("Component Number", "C", "The number of components in the new graph",32);
            pm.Register_IntegerParam("Random Seed", "S", "Random Seed can be included. Default zero negates a seed",0);
            pm.Register_IntegerParam("Iterations", "N", "Number of iterations",1);
            // pManager.Register_GenericParam("cIn", "cIn", "Component types to use in the new graph", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            // Grasshopper.Kernel.GH_Param<float> jim = new Grasshopper.Kernel.GH_Param<float>();
            // pm.Register_IntegerParam("token integer", "T", "Token output parameter");
            pm.Register_GenericParam("Geometry", "G", "Gives the current enabled child graph geometry");
            //pm.Register_IntegerParam("Integer", "I", "Gives the current enabled child graph geometry", GH_ParamAccess.list);
            // pm.Register_StringParam("token string", "S", "Token output parameter");
            // pm.RegisterParam(jim);
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            // Parallel array of slider used tags TODO: Make custom slider class
            List<bool> sliderUsed = new List<bool>();
           
            bool myData = false;
            if (!DA.GetData(0, ref myData)) { return; }
            if (myData == false) { return; }
            
            Grasshopper.GUI.Canvas.GH_Canvas canvas = Instances.ActiveCanvas;
  
            // Get a list of parent components
            List<IGH_ActiveObject> parentObject = canvas.Document.ActiveObjects();
            
            // Remove these next two lines to generate components on the current canvas
            // Warning: You must stop the calculation at the end of the code to prevent
            // infinite grasshopper solving loop.
            Grasshopper.Kernel.GH_Document doc = Instances.DocumentServer.NextAvailableDocument(); //Instances.DocumentServer.AddNewDocument();
            canvas.Document = doc;

            // Get a list of current child components
            List<IGH_ActiveObject> childObject = canvas.Document.ActiveObjects();
            //160,80,120,80
            doc.PreviewColour = Color.FromArgb(160,240,240,240);
            canvas.Viewport.Zoom = 0.1f;


            // TODO: TEST THIS
            // System.Windows.Forms.ToolStripDropDown myBox = new System.Windows.Forms.ToolStripDropDown();
            // this.AppendAdditionalComponentMenuItems(myBox);

            #region component GUIDs

            Dictionary<int, string> ghComponents = new Dictionary<int, string>();

            int myCounter = 0;

            // Cycle through all paraent objects in order to get the component types
            for (int i = 0; i < parentObject.Count; i++)
            {
                string newGuid = parentObject[i].ComponentGuid.ToString();
                string newType = StringTool.Truncate(parentObject[i].GetType().ToString(), 26);
                // Make sure that we do not add ourselves or special components (i.e. buttons & sliders)
                if (newGuid != this.ComponentGuid.ToString() && newType != "Grasshopper.Kernel.Special")
                {
                    if (parentObject[i].Locked != true)
                    {
                        ghComponents.Add(myCounter, newGuid);
                        myCounter++;
                    }
                }
            }

            // Create a list of willing objects from the child canvas
            List<GH_Component> willingObject = new List<GH_Component>();
            int maxPos = 0;

            for (int i = 0; i < childObject.Count; i++)
            {
                string george = childObject[i].ComponentGuid.ToString();
                if(george == "cccb5126-3224-41ec-8841-9e0d1603af8e") // If george is a willing output
                {
                    // Search through all again
                    for (int j = 0; j < childObject.Count; j++)
                    {
                        if (childObject[i].DependsOn(childObject[j])){
                            willingObject.Add((GH_Component)childObject[i]); // i or j? You decide
                            
                        }
                    }
                    // Record the start position
                    if (childObject[i].Attributes.Pivot.X > maxPos) maxPos = (int)childObject[i].Attributes.Pivot.X;
                }
            }
            
            

            #endregion

            // Record which datatypes have been made explicit so far in the graph
            // This is used to check whether a component can be instantiated yet.
            // Add the slider data types as a default
            List<string> typeList = new List<string>();
            typeList.Add("Grasshopper.Kernel.Parameters.Param_Integer");
            typeList.Add("Grasshopper.Kernel.Parameters.Param_Number");
            typeList.Add("Grasshopper.Kernel.Parameters.Param_Boolean");

            // This list stores reference to an index (from Cartesian GP)
            List<EmCompMonitor> oPointer = new List<EmCompMonitor>();

            // Declare the slider and component lists
            List<GH_Component> myComponents = new List<GH_Component>();
            List<Grasshopper.Kernel.Special.GH_NumberSlider> mySliders = new List<Grasshopper.Kernel.Special.GH_NumberSlider>();

            // Get graph sizing from input data
            int sCount = 0; DA.GetData(1, ref sCount);
            int cCount = 0; DA.GetData(2, ref cCount);
            int mySeed = 0; DA.GetData(3, ref mySeed);
            int iterations = 1; DA.GetData(4, ref iterations);
            
            // Random number generator with or without seed
            Random rand;
            if (mySeed == 0) rand = new Random();
            else rand = new Random(mySeed);

            // Generate the parameters
            for (int j = 0; j < sCount; j++)
            {
                mySliders.Add(new Grasshopper.Kernel.Special.GH_NumberSlider());
                mySliders[j].Slider.Maximum = 10m;
                mySliders[j].SetSliderValue((decimal)rand.Next(1, 10), true);
                mySliders[j].Slider.Type = Grasshopper.GUI.Base.GH_SliderAccuracy.Float;
                canvas.InstantiateNewObject(mySliders[j], null, new PointF(100+maxPos+100, j * 50 + 50), false);

                // So that unused sliders can be removed later. TODO: Put sliders in a class!
                sliderUsed.Add(false);
            }

            
            // Loop 10 times.
            for (int n = 0; n < iterations; n++)
            {
                
                // Get rid of everything
                for (int i = 0; i < myComponents.Count; i++)
                {
                    doc.RemoveObject(myComponents[i], false);
                }
                myComponents.Clear();
                oPointer.Clear();

                // Find some inputs from somewhere in the graph, including the sliders
                for (int i = 0; i < cCount; i++)
                {
                    // Declare our new component
                    GH_Component tempObject;
                    // Select a component at random

                    int compRef = rand.Next(0, ghComponents.Count);

                    if (i == 0) tempObject = (GH_Component)Instances.ComponentServer.EmitObject(new Guid("8a5aae11-8775-4ee5-b4fc-db3a1bd89c2f"));
                    else tempObject = (GH_Component)Instances.ComponentServer.EmitObject(new Guid(ghComponents[compRef]));

                    // JimmyActive tracks whether a component is to be removed from the canvas later
                    bool jimmyActive = true;

                    // Add the component to the list and instantiate on the canvas
                    myComponents.Add(tempObject);
                    int xPos = i * 100 + 300 + maxPos + 100;
                    int yPos = compRef * 100 + 100; // rand.Next(100, 500);
                    //int index = myComponents.Count - 1;

                    canvas.InstantiateNewObject(myComponents[i], null, new PointF(xPos, yPos), false);



                    
                    int I;
                    int spokenFor;

                    // Wire up each inputs
                    for (int k = 0; k < myComponents[i].Params.Input.Count; k++)
                    {
                        // Don't cycle ourselves! future proof for 2d (don't select from current column)
                        // 'I' represents the component we are connecting to
                        I = i;
                        
                        while (I == i)
                        {
                              // Hitting a negative value means pick from the sliders
                              // If we are the first component, then make 'I' definitely negative.
                              if (i != 0) I = rand.Next(-mySliders.Count-willingObject.Count, i);
                              else I = -1;
                        }

                        

                        // One for each input
                        bool sliderFlag = false;

                        // Pick from the sliders
                        if (I < 0 && I > -mySliders.Count)
                        {
                            int S = rand.Next(0, mySliders.Count);
                            string myType = myComponents[i].Params.Input[k].GetType().ToString();

                            // YourType is known to be slider types
                            // Only add a wire if myType is a number, integer or boolean (0 or 1 for our sliders)
                            if (myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                             || myType == "Grasshopper.Kernel.Parameters.Param_Number"
                             || myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                             || myType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                            {
                                myComponents[i].Params.Input[k].AddSource(mySliders[S]);
                                sliderFlag = true;
                                sliderUsed[S] = true;
                            }
                        }

                        // Cover the willing object case
                        if (I < -mySliders.Count)
                        {
                            int W = rand.Next(0, willingObject.Count);
                            string myType = myComponents[i].Params.Input[k].GetType().ToString();
                            string yourType = willingObject[W].Params.Output[0].GetType().ToString();

                           // if (myType == "Grasshopper.Kernel.Parameters.Param_Point")
                            //{
                               // if (yourType == "Grasshopper.Kernel.Parameters.Param_Point")
                                //{
                                    myComponents[i].Params.Input[k].AddSource(willingObject[W].Params.Output[0]);
                                    sliderFlag = true;
                                    //myComponents[yourI].Hidden = true;
                                //}
                          //  }
                           // if (myType == "Grasshopper.Kernel.Parameters.Param_Plane")
                           // {
                           //     if (yourType == "Grasshopper.Kernel.Parameters.Param_Plane")
                              //  {
                                   // myComponents[i].Params.Input[k].AddSource(willingObject[W].Params.Output[0]);
                                   // sliderFlag = true;
                                    //myComponents[yourI].Hidden = true;
                               // }
                            //}
                           
                        }

                        // Select from the existing components, if sliderflag is false (it will be unless slider selection was a success
                        // NOTE: AVOID THE FIRST COMPONENT, AS NOTHING IS IN oPOINTER YET
                        if (i > 0 && !sliderFlag)
                        {
                            bool flag = false;
                            int counter = 0;



                            while (!flag)
                            {
                                // First check whether the component selected is active
                                if (oPointer[counter].isActive)
                                {
                                    int yourI = oPointer[counter].i;
                                    int yourK = oPointer[counter].k;

                                    // int K = rand.Next(0, myComponents[I].Params.Output.Count);

                                    // This needs to change but I'm running out of time
                                    string myType = myComponents[i].Params.Input[k].GetType().ToString();
                                    string yourType = myComponents[yourI].Params.Output[yourK].GetType().ToString();

                                    switch (myType)
                                    {
                                        case "Grasshopper.Kernel.Parameters.Param_Integer":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Integer" || yourType == "Grasshopper.Kernel.Parameters.Param_Number")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;

                                        case "Grasshopper.Kernel.Parameters.Param_Number":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Integer" 
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Number"
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;
                                            
                                            
                                        case "Grasshopper.Kernel.Parameters.Param_Curve":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Line" 
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Curve"
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Circle"
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Arc")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;

                                        case "Grasshopper.Kernel.Parameters.Param_Line":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Line" || yourType == "Grasshopper.Kernel.Parameters.Param_Curve")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;

                                        case "Grasshopper.Kernel.Parameters.Param_Point":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Point")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;

                                        case "Grasshopper.Kernel.Parameters.Param_GenericObject":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Integer" 
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Number"
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;
                                            
                                        case "Grasshopper.Kernel.Parameters.Param_Geometry":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Box" || yourType == "Grasshopper.Kernel.Parameters.Param_Geometry")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                //myComponents[yourI].Hidden = true;
                                            }
                                            break;
                                         
                                        case "Grasshopper.Kernel.Parameters.Param_Brep":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Box" 
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Brep"
                                             || yourType == "Grasshopper.Kernel.Parameters.Param_Surface")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                myComponents[yourI].Hidden = true;
                                            }
                                            break;

                                        //planes are usually already persistent data (global XY)
                                        case "Grasshopper.Kernel.Parameters.Param_Plane":
                                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Plane")
                                            {
                                                myComponents[i].Params.Input[k].AddSource(myComponents[yourI].Params.Output[yourK]);
                                                flag = true;
                                                myComponents[yourI].Hidden = true;
                                            }
                                            else
                                            {
                                                flag = true; //everything is fine, world XY is set.
                                                break;
                                            }
                                            break;

                                        case "Grasshopper.Kernel.Parameters.Param_Boolean":
                                            flag = true; // Let's leave this at the moment.
                                            break;

                                        default:
                                            break;
                                    }
                                } // End of pointer is active check

                                counter++;

                                // If we've gone through everything then forget it, 
                                // we have failed to attach ALL the wires
                                // & must set jimmyActive to false for the whole 'i' component

                                if (counter >= oPointer.Count)
                                {
                                    // TODO: Put into method ************
                                    // Try the sliders one more time again
                                    int S = rand.Next(0, mySliders.Count);
                                    string myType = myComponents[i].Params.Input[k].GetType().ToString();

                                    // YourType is known to be slider types
                                    // Only add a wire if myType is a number, integer or boolean (0 or 1 for our sliders)
                                    if (myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                                     || myType == "Grasshopper.Kernel.Parameters.Param_Number"
                                     || myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                                     || myType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                                    {
                                        myComponents[i].Params.Input[k].AddSource(mySliders[S]);
                                        sliderUsed[S] = true;
                                    }
                                    else
                                    {
                                        jimmyActive = false;
                                    }
                                    //// TODO: Put into method***********

                                    flag = true;
                                }

                            }// End of while
                        }// End of if

                        // Shuffle before moving onto the next input
                        Shuffle(oPointer, mySeed);
                        spokenFor = I;
                        

                    } // End of trying to connect a particular input to something

                    // Once we have done everything we want to, we need to add ourselves to the type stack
                    // So that future components know what they can connect to. 
                    for (int k = 0; k < tempObject.Params.Output.Count; k++)
                    {
                        string thisString = tempObject.Params.Input[k].GetType().ToString();
                        if (!typeList.Contains(thisString)) typeList.Add(thisString);

                        // Now add to the pointer stack
                        oPointer.Add(new EmCompMonitor(i, 0, k, jimmyActive));
                    }

                    // Get rid of the component if it failed. Note: we still need the oPointer though
                    if (!jimmyActive) doc.RemoveObject(myComponents[i], false);

                    // Shuffle after updating the oPointer with new things
                    Shuffle(oPointer, mySeed);

                } // End of i. Move onto the next component

                // Get rid of any unused sliders
                //for (int j = 0; j < sCount; j++)
                //{
                //    if (!sliderUsed[j])
                //    {
                //        doc.RemoveObject(mySliders[j], false);
                //    }
                //}

                //canvas.Refresh();
                canvas.Document.NewSolution(true);

                //GH_RuntimeMessageLevel.Warning = 
                
            } //loop 10 times

            DA.SetData(0, willingObject.Count);
            
        }

        public static void Shuffle(List<EmCompMonitor> list, int seed) // Shuffle a list of integers, if this is an array on zero based increasing integers then this is a good way to get random but unique items from a list
        {
            Random rng;
            if (seed == 0) rng = new Random();
            else rng = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);

                //temporary new thing that uses constructor to ensure byVal
                EmCompMonitor valueN = new EmCompMonitor(list[n].i, list[n].j, list[n].k, list[n].isActive);
                EmCompMonitor valueK = new EmCompMonitor(list[k].i, list[k].j, list[k].k, list[k].isActive);

                list[k] = valueN;
                list[n] = valueK;

            }
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("00aa6ae9-a615-40b4-a68b-f73b67e43040"); } 
            //old: f0f75d15-8ad4-4a4a-91d9-c0e0e689a80b
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.E7;
            }
        }

        public override void CreateAttributes()
        {
            m_attributes = new ErdosAttributes(this);
        }

    }
}
