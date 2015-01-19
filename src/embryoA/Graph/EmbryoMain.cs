using System;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Embryo.Properties;
using System.Collections.Generic;
using Rhino;
using Grasshopper;
using Embryo.Generic;
using System.Windows.Forms;
using Embryo.Params;
using Embryo.Types;

/**
 * 
 * Embryo component for Grasshopper
 * Copyright (c) 2013 John Harding
 * Cartesian Genetic Programming - 
 * 1D version as recommended by Millar (2003) for least constrained system
 * 
 * This software is released under
 * ...
 * 
 */


namespace Embryo.Graph
{
    /// <summary> 
    /// The main Embryo component inherited from the Grasshopper standard
    /// </summary> 
    public class EmbryoMain : GH_Component
    {
        // Grasshopper canvas pointer
        Grasshopper.GUI.Canvas.GH_Canvas canvas;

        // Lists of Embryo things
        private List<EM_Component> myComponents;
        private List<EM_Component> existingComponents;
        private List<EM_Slider> mySliders;
        private List<GH_Component> getGeometryComponents;
        List<GH_Component> ParentInputComponent;
        private List<IGH_Param> ParentInputParam;
        private List<EM_Plug> PlugObject;
        private EM_SettingsParam myParam;
        private EM_Settings localSettings;

        // List of all the outputs in the generated graph
        private List<object> outputStash;

        // List of outputs on the parent graph that are to be included
        List<object> willingOutput;

 
        
        // Create a dictionary that stores all of the component IDs to be used from the ingredient pool
        Dictionary<int, string> componentGUIDs;
        
        // The mystery component
        private EM_Component Helper;

        // Inputs from the Embryo Component
        private EM_Settings mySettings;
        private bool isCountdown;
        private Random randomMonkey;
        private int sCount;
        private int cCount;
        private int mySeed;
        private int inputCount;
        private int masterCounter;

        // Seeds that control graph generation. Metrics, Functions & Topology
        private List<double> metricSeed; 
        private List<int> functiSeed;
        private List<int> topoloSeed;

        /// <summary> 
        /// Grasshopper constructor
        /// </summary> 
        public EmbryoMain()
            : base("Embryo", "Embryo", "Generates new grasshopper networks automatically", "Embryo", " Parent")
        {
            masterCounter = 0;
            myComponents = new List<EM_Component>();
            existingComponents = new List<EM_Component>();
            mySliders = new List<EM_Slider>();
            outputStash = new List<object>();
            willingOutput = new List<object>();
            PlugObject = new List<EM_Plug>();
            ParentInputParam = new List<IGH_Param>();
            getGeometryComponents = new List<GH_Component>();
            componentGUIDs = new Dictionary<int, string>();
            ParentInputComponent = new List<GH_Component>();
            canvas = Instances.ActiveCanvas;

            // Slider and Component counts
            sCount = 0;
            cCount = 0;

            // This could be anything. At the moment it's the plug component. It's all a mystery to everyone why we need all of this.
            Helper = new EM_Component((GH_Component)Instances.ComponentServer.EmitObject(new Guid("004e5436-b7ce-471b-9e45-392cb09e0fc4")), false);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pm)
        {
            pm.AddBooleanParameter("Reset", "Go", "Begins a new Embryo solution", GH_ParamAccess.item);
            pm.AddIntegerParameter("Sliders", "Sl", "The number of generated sliders in the new graph", GH_ParamAccess.item, 8);
            pm.AddIntegerParameter("Components", "Co", "The number of generated components in the new graph", GH_ParamAccess.item, 16);
            pm.AddNumberParameter("Metric Genes", "RM", "A list of genes that generate metric parameters", GH_ParamAccess.list, 2);
            pm.AddIntegerParameter("Topology Genes", "RT", "A list of genes that generate the topological structure", GH_ParamAccess.list, 2);
            pm.AddIntegerParameter("Function Genes", "RF", "A list of genes that select components", GH_ParamAccess.list, 2);

            // Register the default settings for Embryo
            myParam = new EM_SettingsParam();
            localSettings = new EM_Settings();
            myParam.PersistentData.Append(new EM_Goo(localSettings));
            pm.AddParameter(myParam, "Settings", "Se", "Embryo Settings", GH_ParamAccess.item);

            // The random override negates the need for 3 seeds and explores the whole solution space
            pm.AddIntegerParameter("Random Overide", "RO", "0 is default Embryo mapping behaviour. Anything positive will overide the genes and produce a random graph using this input as a seed", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pm)
        {
            pm.AddIntegerParameter("Iterations", "It", "Number of iterations for this Embryo solution", GH_ParamAccess.item);
            pm.AddGenericParameter("Components", "Co", "A list of the generated components", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Random seed for topology. My seed is now set to this.
            mySeed = 2;
            randomMonkey = new Random(mySeed);

            // Get settings
            EM_Goo temp = new EM_Goo();
            if (!DA.GetData("Settings", ref temp)) { return; }
            mySettings = (EM_Settings)temp.Value;

            // One to One Settings
            isCountdown = mySettings.OneToOne;

            // Metric genes
            metricSeed = new List<double>();
            DA.GetDataList<double>("Metric Genes", metricSeed);
            
            // Topology genes
            topoloSeed = new List<int>();
            DA.GetDataList<int>("Topology Genes", topoloSeed);

            // Funcion genes
            functiSeed = new List<int>();
            DA.GetDataList<int>("Function Genes", functiSeed);

            // Sliders to be generated
            DA.GetData("Sliders", ref sCount);

            // Components to be generated
            DA.GetData("Components", ref cCount);

            // Now the setup
            bool myData = false;
            if (!DA.GetData("Reset", ref myData)) { return; }

            int crazySeed = -1;
            DA.GetData("Random Overide", ref crazySeed);
            
            // random override
            if (crazySeed > 0)
            {
                metricSeed = new List<double>();
                topoloSeed = new List<int>();
                functiSeed = new List<int>();
                Random rnd = new Random(crazySeed);
                for(int i=0; i<=200; i++)
                {
                    metricSeed.Add(rnd.NextDouble() * (mySettings.Interval.Value.Max - mySettings.Interval.Value.Min) + mySettings.Interval.Value.Min);
                    topoloSeed.Add(rnd.Next(0, 100000));
                    functiSeed.Add(rnd.Next(0, 100));
                }

                //Friends.ShuffleDoubles(metricSeed, crazySeed);
                //Friends.ShuffleIntegers(topoloSeed, crazySeed);
                //Friends.ShuffleIntegers(functiSeed, crazySeed);
            }

            // Set up an event so that everything happens after Grasshopper expires the whole solution
            if (myData == false)
            {
                try
                {
                    canvas.Document.SolutionEnd -= new GH_Document.SolutionEndEventHandler(ModifyComponents);
                }
                catch
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something has gone seriously wrong, and to be honest I'm not sure what to do about it");
                }
                masterCounter = 0;
            }

            else
            {
                try
                {
                    canvas.Document.SolutionEnd -= new GH_Document.SolutionEndEventHandler(ModifyComponents);
                }
                catch
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something has gone seriously wrong, and to be honest I'm not sure what to do about it");
                }
                canvas.Document.SolutionEnd += new GH_Document.SolutionEndEventHandler(ModifyComponents);
            }

            // This is the last thing that Embryo does.
            DA.SetData(0, masterCounter);
            DA.SetDataList(1, myComponents);

        }

        /// <summary> 
        /// This method is called at the end of the normal Grasshopper solution.
        /// </summary> 
        public void ModifyComponents(object sender, GH_SolutionEventArgs e)
        {

            //this.CollectData();
            //this.ComputeData();

            ClearSolution(e.Document);

            #region 2. MasterCounter = 0 things

            // If this is the first iteration then get stuff
            if (masterCounter == 0)
            {
                // Start from scratch
                ClearEverything();

                // A. Get all the stuff on the canvas
                List<IGH_ActiveObject> canvasObject = e.Document.ActiveObjects();

                // Check for Embryo Components on the canvas
                for (int i = 0; i < canvasObject.Count; i++)
                {              
                    string george = canvasObject[i].ComponentGuid.ToString();

                    // 1. Parent Outputs
                    if (george == "cccb5126-3224-41ec-8841-9e0d1603af8e")
                    {
                        // Search through all again
                        for (int j = 0; j < canvasObject.Count; j++)
                        {
                            if (canvasObject[i].DependsOn(canvasObject[j]))
                            {
                                string newType = canvasObject[j].GetType().ToString();
                                if (newType == "Grasshopper.Kernel.Special.GH_NumberSlider")
                                {
                                    willingOutput.Add((Grasshopper.Kernel.Special.GH_NumberSlider)canvasObject[j]);
                                }
                                else
                                {
                                    GH_Component willingThing = (GH_Component)canvasObject[i];
                                    for (int n = 0; n < willingThing.Params.Input[0].Sources.Count; n++)
                                    {
                                        willingOutput.Add((IGH_Param)willingThing.Params.Input[0].Sources[n]); // i or j? You decide
                                    }
                                }
                            }
                        }
                    }

                    // 2. Child OuputPlug
                    if (george == "004e5436-b7ce-471b-9e45-392cb09e0fc4")
                    {

                        // Add the new plug object. Sources are cloned in the constructor.
                        EM_Plug thisPlug = new EM_Plug((GH_Component)canvasObject[i]);
                        PlugObject.Add(thisPlug);

                    }

                    // 3. Parent Input (slightly tricky)
                    if (george == "68b3b695-40ba-4f50-a270-ffec7738129e")
                    {
                        GH_Component InputComponent = (GH_Component)canvasObject[i];
                        ParentInputComponent.Add(InputComponent);

                        for (int j = 0; j < canvasObject.Count; j++)
                        {
                            if (canvasObject[j].DependsOn(canvasObject[i]))
                            {
                                try
                                {
                                    // For Components
                                    GH_Component tempThing = (GH_Component)canvasObject[j];
                                    for (int k = 0; k < tempThing.Params.Input.Count; k++)
                                    {
                                        if (tempThing.Params.Input[k].Sources.Contains(InputComponent.Params.Output[0]))
                                        {
                                            ParentInputParam.Add((IGH_Param)tempThing.Params.Input[k]);
                                            tempThing.Params.Input[k].RemoveAllSources();

                                            // Now put back the Embryo Component... Bah!
                                            tempThing.Params.Input[k].AddSource(InputComponent.Params.Output[0]);
                                        }
                                    }
                                }
                                catch
                                {
                                    // For Paramters
                                    IGH_Param tempThing2 = (IGH_Param)canvasObject[j];
                                    if (tempThing2.Sources.Contains(InputComponent.Params.Output[0]))
                                    {
                                        ParentInputParam.Add((IGH_Param)tempThing2);
                                        tempThing2.RemoveAllSources();

                                        // Now put back the Embryo Component... Bah!
                                        tempThing2.AddSource(InputComponent.Params.Output[0]);
                                    }
                                }
                            }
                        }
                    }

                    // 4. Getgeometry
                    if (george == "56212076-52dd-44cc-aa82-aa5b31504302") getGeometryComponents.Add((GH_Component)canvasObject[i]);
                }

                // Get a list of (ingredient) components that are located at X<0
                for (int i = canvasObject.Count - 1; i >= 0; i--)
                {
                    // If in the bottom left quadrant...
                    if (canvasObject[i].Attributes.Pivot.X > 0 || canvasObject[i].Attributes.Pivot.Y < 0) canvasObject.RemoveAt(i);
                }

                int ingredientCount = canvasObject.Count;

                // Cycle through all the remaining canvas objects in order to get the component GUIDS
                int myCounter = 0;
                for (int i = 0; i < canvasObject.Count; i++)
                {
                    string newGuid = canvasObject[i].ComponentGuid.ToString();
                    string newType = StringTool.Truncate(canvasObject[i].GetType().ToString(), 26);

                    // Make sure that we do not add ourselves or special components (i.e. buttons & sliders)
                    if (newType != "Grasshopper.Kernel.Special")
                    {
                        if (canvasObject[i].Locked != true)
                        {
                            componentGUIDs.Add(myCounter, newGuid);
                            myCounter++;
                        }
                    }
                }

                canvasObject.Clear();

                // B. Get Existing child components (Setup a new list of canvas objects again).
                canvasObject = e.Document.ActiveObjects();

                // Remove anything not in the upper right quadrant
                for (int i = canvasObject.Count - 1; i >= 0; i--)
                {
                    if (canvasObject[i].Attributes.Pivot.Y > 0 || canvasObject[i].Attributes.Pivot.X < 0) canvasObject.RemoveAt(i);
                }

                // Also remove any child modifiers (for example plugs)
                for (int i = canvasObject.Count - 1; i >= 0; i--)
                {
                    if (canvasObject[i].ComponentGuid.ToString() == "004e5436-b7ce-471b-9e45-392cb09e0fc4") canvasObject.RemoveAt(i);
                }

                // Remove any previously generated components or sliders ALSO FROM THE DOCUMENT
                for (int i = canvasObject.Count - 1; i >= 0; i--)
                {
                    string genCheck = StringTool.Truncate(canvasObject[i].NickName.ToString(), 2);
                    if (genCheck.Equals("G_"))
                    {
                        OnPingDocument().RemoveObject(canvasObject[i], false);
                        canvasObject.RemoveAt(i);
                    }
                }

                // Cycle through all canvas (now child) objects in order to get the existing components
                for (int i = 0; i < canvasObject.Count; i++)
                {
                    string newGuid = canvasObject[i].ComponentGuid.ToString();
                    string newType = StringTool.Truncate(canvasObject[i].GetType().ToString(), 26);

                    // Make sure that we do not add ourselves or special components (i.e. buttons & sliders)
                    if (newType != "Grasshopper.Kernel.Special")
                    {
                        if (canvasObject[i].Locked != true)
                        {
                            EM_Component eComponent = new EM_Component((GH_Component)canvasObject[i], true);

                            // This seems to reset the objects so that it works
                            OnPingDocument().RemoveObject(eComponent.Component, false);
                            OnPingDocument().AddObject(eComponent.Component, false);
                            existingComponents.Add(eComponent);

                            // Note don't add to the stash yet. You may up with cyclic dependencies.
                        }
                    }
                }

                // Put back the plugs that have become detached
                for (int n = 0; n < PlugObject.Count; n++)
                {
                    PlugObject[n].Reconnect();
                }

                // Raise some errors if we haven't collected enough things
                if (ingredientCount == 0 && cCount > 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You must add some ingredient components or set the required number of components to zero!");
                    return;
                }
                if (cCount == 0 && existingComponents.Count == 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "If you are not generating any new components, you probably need to add some existing ones to the child canvas");
                    return;
                }

                
            }

            #endregion (End of masterCounter == 0)

            #region 3. Final Events before generation

            // Add the willingobjects to the outputStash
            for (int i = 0; i < willingOutput.Count; i++) outputStash.Add(willingOutput[i]);

            // Add the list of existing components in the initial order that they were.
            for (int i = 0; i < existingComponents.Count; i++)
            {
                // Clear all existing input connections (not output, we still need the plugs)
                for (int k = 0; k < existingComponents[i].Component.Params.Input.Count; k++)
                {
                    existingComponents[i].Component.Params.Input[k].RemoveAllSources();
                }

                existingComponents[i].Component.Message = "E";
                existingComponents[i].isExisting = true;

                myComponents.Add(existingComponents[i]);
            }

            #endregion

            #region 4. Make the sliders, add to canvas and add to the outputstash

            // Use the helper component if we are not generating anything
            // This triggers something that makes Galapagos work - I'm not entirely sure why
            if (cCount == 0 && sCount == 0)
            {
                e.Document.AddObject(Helper.Component, false);
                e.Document.RemoveObject(Helper.Component, false);
            }

            for (int i = 0; i < sCount; i++)
            {
                EM_Slider gSlider = new EM_Slider(new Grasshopper.Kernel.Special.GH_NumberSlider(), false);
                mySliders.Add(gSlider);
                mySliders[i].Slider.Slider.Minimum = (decimal)mySettings.Interval.Value.Min;
                mySliders[i].Slider.Slider.Maximum = (decimal)mySettings.Interval.Value.Max;
                mySliders[i].Slider.SetSliderValue((decimal)(metricSeed[i%metricSeed.Count]));
                mySliders[i].Slider.Slider.Type = Grasshopper.GUI.Base.GH_SliderAccuracy.Float;
                mySliders[i].Slider.NickName = "G_Slider" + i.ToString();

                e.Document.AddObject(mySliders[i].Slider, false);
                mySliders[i].Slider.Attributes.Pivot = new PointF(100f, -100 - (25 * i));

                outputStash.Add(mySliders[i].Slider);
            }
            #endregion

            #region 5. Make the NEW components and add them to the canvas

            // Start by adding a load of new components on top of the existing ones.
            for (int i = existingComponents.Count; i < (cCount + existingComponents.Count); i++)
            {
                // Select a component at random
                int functiIndex = i - existingComponents.Count;
                int compRef = functiSeed[functiIndex % functiSeed.Count] % componentGUIDs.Count;

                // Add the component to the list
                EM_Component eComponent = new EM_Component((GH_Component)Instances.ComponentServer.EmitObject(new Guid(componentGUIDs[compRef])), false);
                myComponents.Add(eComponent);

                // Add the object and move it (could be done later)
                e.Document.AddObject((GH_Component)myComponents[i].Component, false);
                //canvas.InstantiateNewObject(myComponents[i].Component, null, new PointF(100, 50), false);

                // Identify as a generated component
                myComponents[i].Component.Message = "G";
                myComponents[i].Component.NickName = "G_" + myComponents[i].Component.NickName;
                //myComponents[i].Component.MutableNickName = false;
            }

            // Sanity Check just before we run the thing...
            if (myComponents.Count == 0)
            {
                ClearSolution(e.Document);
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Not enough components");
                return;
            }

            #endregion

            #region 6. Hook up the components one by one
            
            // A local list to avoid going to the same output twice
            // This is different to the one-to-one list which is global
            List<object> stashRecord = new List<object>();

            // We will find some possible inputs from somewhere in the graph, including the sliders
            for (int i = 0; i < myComponents.Count; i++)
            {
                // Clear the list of stash members already visited
                stashRecord.Clear();

                // Add a message showing the component index.
                myComponents[i].Component.Message += i.ToString();

                // Wire up each inputs
                for (int k = 0; k < myComponents[i].Component.Params.Input.Count; k++)
                {
                    WireUp(myComponents[i], myComponents[i].Component.Params.Input[k], stashRecord, isCountdown, i, k);
                    inputCount++; // Keep a tally of the input number.
                }

                // Refresh the data. It is already clear.
                myComponents[i].Component.CollectData();
                myComponents[i].Component.ComputeData();

                // Get rid of the component if it failed. If not, finalise this component.
                if (!myComponents[i].Component.RuntimeMessageLevel.Equals(GH_RuntimeMessageLevel.Blank))
                {
                    myComponents[i].Alive = false;

                    // Remove the components from the canvas if this setting is true
                    if (mySettings.RemoveDead)
                    {
                        myComponents[i].ResetCartesian();
                        e.Document.RemoveObject(myComponents[i].Component, false);
                    }
                }
                else
                {
                    // Hide if preview is set to false
                    if (!mySettings.Preview)
                    {
                        myComponents[i].Component.Hidden = true;
                    }
                    else
                    {
                        myComponents[i].Component.Hidden = false;
                    }

                    // Only waste time if there are any get geometry components out there.
                    if (getGeometryComponents.Count != 0)
                    {
                        geometryOut(myComponents[i].Component);
                    }

                    // Add the outputs to the stash
                    for (int k = 0; k < myComponents[i].Component.Params.Output.Count; k++)
                    {
                        bool localFlag = false;
                        for (int q = 0; q < PlugObject.Count; q++)
                        {
                            if (PlugObject[q].Component.Params.Input[0].Sources.Contains((IGH_Param)myComponents[i].Component.Params.Output[k]))
                            {
                                localFlag = true;
                            }
                        }
                        // Only add the output to the outputStash if it is not plugged
                        if (!localFlag)
                        {
                            outputStash.Add(myComponents[i].Component.Params.Output[k]);
                        }
                    }
                }

            } // End of this component

            #endregion

            #region 7. Hook up the parent inputs and compute data

            stashRecord.Clear();
            int II = myComponents.Count-1;
            for (int k = 0; k < ParentInputParam.Count; k++)
            {
                // Ensure we're looking at the right part of the genome
                int KK = k % 4;
                if (KK == 0) II++;
                WireUp(null, ParentInputParam[k], stashRecord, isCountdown, II, KK);
            }

            // Having now been wired up, find downstream objects and calculate solution
            for (int i = 0; i < ParentInputComponent.Count; i++)
            {
                List<IGH_ActiveObject> myList = OnPingDocument().FindAllDownstreamObjects(ParentInputComponent[i]);
                foreach (IGH_ActiveObject myObject in myList)
                {
                    myObject.ClearData();
                    myObject.CollectData();
                    myObject.ComputeData();
                }
            }

            // Find downstream objects from get geometry and calculate solution
            // WHY ISN'T THIS WORKING ??
         
            for (int i = 0; i < getGeometryComponents.Count; i++)
            {
                List<IGH_ActiveObject> myList = OnPingDocument().FindAllDownstreamObjects(getGeometryComponents[i]);
                foreach (IGH_ActiveObject myObject in myList)
                {
                    myObject.ClearData();
                    myObject.CollectData();
                    myObject.ComputeData();
                }
            }


            #endregion

            #region 8. Topological Sort & Final events

            // Set cartesian2d
            int maxCol = Friends.GetMaxCol(myComponents);
            int[] cartesian2d = new int[maxCol + 1];
            for (int i = 0; i < cartesian2d.Length; i++) 
                cartesian2d[i] = 0;

            for (int i = 0; i < myComponents.Count; i++)
            {
                if (!mySettings.RemoveDead || myComponents[i].Alive)
                {
                    int activeCol = myComponents[i].GetColumn();
                    myComponents[i].Component.Attributes.Pivot = new PointF(activeCol * mySettings.GridX + 800, -mySettings.GridY * cartesian2d[activeCol] - 100);
                    cartesian2d[activeCol]++;
                }
                    // Set preview to false if it has no children.
                    // Easier to do it here
     
            }


            # endregion

            masterCounter++;

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

        /// <summary> 
        /// Gets all the geometry from the child canvas and hooks up to the GetGeometry input
        /// </summary> 
        public void geometryOut(GH_Component thisComponent)
        {
            for (int k = 0; k < thisComponent.Params.Output.Count; k++)
            {
                string outType = thisComponent.Params.Output[k].TypeName;
                if (outType.Equals("Surface") 
                    || outType.Equals("Box") 
                    || outType.Equals("Brep") 
                    || outType.Equals("Mesh") 
                    || outType.Equals("MeshFace") 
                    || outType.Equals("Twisted Box")
                    || outType.Equals("Geometry")
                    )
                {
                    for (int n = 0; n < getGeometryComponents.Count; n++)
                    {
                        getGeometryComponents[n].Params.Input[0].AddSource(thisComponent.Params.Output[k]);
                    }
                }
            }
        }

        /// <summary> 
        /// Clear the last child graph Embyro has made
        /// </summary> 
        public void ClearSolution(GH_Document myDoc)
        {
            // Just clear the output stash outright
            outputStash.Clear();

            // Remove ALL of the sliders
            for (int i = mySliders.Count - 1; i >= 0; i--)
            {
                //IGH_DocumentObject tempObject = canvas.Document.FindObject(guidList[i], true);
                myDoc.RemoveObject(mySliders[i].Slider, false);
                mySliders.RemoveAt(i);
            }

            // Remove any generated components
            for (int i = myComponents.Count - 1; i >= 0; i--)
            {
                if (myComponents[i].isExisting == false)
                {
                    myDoc.RemoveObject(myComponents[i].Component, false);
                    myComponents[i].ResetCartesian();
                    myComponents.RemoveAt(i);
                }
                else
                {
                    // Keep them but set the alive tag to true and reset column
                    myComponents[i].Alive = true;
                    myComponents[i].ResetCartesian();
                }
            }

            // De-wire any parent input params, unless they are parent inputs
            for (int i = 0; i < ParentInputParam.Count; i++)
            {
                for (int k = 0; k < ParentInputParam[i].Sources.Count; k++)
                {
                    IGH_Param thisSource = ParentInputParam[i].Sources[k];
                    if (!thisSource.TypeName.Equals("EM_Input"))
                    {
                        ParentInputParam[i].RemoveSource(thisSource);
                    }
                }
            }

            // De-wire any getgeometry components
            for (int i = 0; i < getGeometryComponents.Count; i++)
            {
                getGeometryComponents[i].Params.Input[0].RemoveAllSources();
            }

            // Now flush the myComponent list
            myComponents.Clear();
            mySliders.Clear();
        }

        /// <summary> 
        /// Clear absolutely everything and start from scratch
        /// </summary> 
        public void ClearEverything()
        {
            existingComponents.Clear();
            componentGUIDs.Clear();
            myComponents.Clear();
            mySliders.Clear();
            outputStash.Clear();
            willingOutput.Clear();
            PlugObject.Clear();
            ParentInputParam.Clear();
            getGeometryComponents.Clear();
        }

        /// <summary> 
        /// Method which hooks up an existing component to the graph
        /// </summary> 
        public void WireUp(EM_Component thisComponent, IGH_Param thisInput, List<object> stashRecord, bool isCountdown, int II, int KK)
        {

            string myType;
            string yourType;

            if (outputStash.Count > 0)
            {
                int index;
                
                // We need to form a sublist of valid connections
                // why?
                List<object> validStash = new List<object>();
                myType = thisInput.GetType().ToString();
                
                for(int s=0; s<outputStash.Count; s++)
                {
                    yourType = outputStash[s].GetType().ToString();
                    if (TypeCheck.sliderValid(myType, yourType) && !stashRecord.Contains(outputStash[s]))
                    {
                        validStash.Add(outputStash[s]);
                    }
                    else if (TypeCheck.isValid(myType, yourType) && !stashRecord.Contains(outputStash[s]))
                    {
                        validStash.Add(outputStash[s]);
                    }
                }

                // If there are no valid inputs, then flag and give up
                if(validStash.Count==0)
                {
                    if (thisComponent != null) thisComponent.Component.Message += "X";
                }

                else{
                    // 4 Inputs default
                    if (KK < 4) index = topoloSeed[(II * 4 + KK) % topoloSeed.Count] % validStash.Count;
                    else index = randomMonkey.Next(validStash.Count); // The only random part in Embryo now. NOT now used by parent inputs

                    yourType = validStash[index].GetType().ToString();

                    // Slider?
                    if (TypeCheck.sliderValid(myType, yourType) && !stashRecord.Contains(validStash[index]))
                    {
                        thisInput.AddSource((Grasshopper.Kernel.Special.GH_NumberSlider)validStash[index]);

                        
                        // Prevent two of the same sources for this component. This is not dependent on the one-to-one component
                        stashRecord.Add(validStash[index]);

                        // Remove from the outputStash list if one-to-one
                        if (isCountdown)
                        {
                            outputStash.Remove(validStash[index]);
                            validStash.RemoveAt(index);
                        }
                    }

                    // Component?
                    else if (TypeCheck.isValid(myType, yourType) && !stashRecord.Contains(validStash[index]))
                    {
                        thisInput.AddSource((IGH_Param)validStash[index]);


                        // There HAS GOT TO BE a better way of doing this than this...
                        // This is for layout purporses.
                        // Don't do it if we are a parent input component, as thisComponent will be null.
                        if (thisComponent != null)
                        {
                            for (int q = 0; q < myComponents.Count; q++)
                            {
                                for (int k = 0; k < myComponents[q].Component.Params.Output.Count; k++)
                                {
                                    if (myComponents[q].Component.Params.Output[k].Equals((IGH_Param)validStash[index]))
                                    {
                                        // Add a reference to the connected component
                                        thisComponent.AddInput(q, k, myComponents);

                                        // As we know the component IS a parent, then hide it according to the settings
                                        // We will be left with only terminal components all being well!!
                                        if (mySettings.OnlyTerms) myComponents[q].Component.Hidden = true;
                                    }
                                }
                            }
                        }

                        // Prevent two of the same sources for this component. This is not dependent on the one-to-one component
                        stashRecord.Add(validStash[index]);

                        // Remove from the outputStash list if one-to-one
                        if (isCountdown)
                        {
                            outputStash.Remove(validStash[index]);
                            validStash.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("965d8426-5a7d-4fe2-b886-583044f9884e"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new EmbryoMainAttrib(this);
        }

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
                return Properties.Resources.EmbryoMain02;
            }
        }

    }
}
