using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Embryo.Generic
{
    public static class TypeCheck
    {
            /// <summary>
            /// Checks the type
            /// </summary>     
            public static bool isValid(string myType, string yourType)
            {
                bool myBool = false;

                if (myType.Equals(yourType))
                {
                    return true;
                }
                else
                {
                    // EXTRA Casting Checks
                    switch (myType)
                    {
                        // 1.Arc

                        // 2.Boolean
                        
                        // 3.Box

                        // 4.Brep
                        case "Grasshopper.Kernel.Parameters.Param_Brep":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Box"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_Surface"
                             || yourType == "SquishyXMorphs.GH_TwistedBox")
                            {
                                myBool = true;
                            }
                            break;

                        // 5.Circle
 
                        // 6.Color

                        // 7.Complex

                        // 8.Curve
                        case "Grasshopper.Kernel.Parameters.Param_Curve":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Line"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_Circle"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_Arc")
                            {
                                myBool = true;
                            }
                            break;

                        // 9.FilePath

                        // 10.GenericObject
                        case "Grasshopper.Kernel.Parameters.Param_GenericObject":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Integer"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_Number")
                            {
                                myBool = true;
                            }
                            break;

                        // 11.Geometry
                        case "Grasshopper.Kernel.Parameters.Param_Geometry":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Box"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_Geometry")
                            {
                                myBool = true;
                            }
                            break;

                        // 12.Group

                        // 13.GUID

                        // 14.Integer
                        case "Grasshopper.Kernel.Parameters.Param_Integer":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Number"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                            {
                                myBool = true;
                            }
                            break;

                        // 15.Interval

                        // 16.Interval2D

                        // 17.Interval2D_OBSOLETE

                        // 18.Line
                        case "Grasshopper.Kernel.Parameters.Param_Line":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Curve")
                            {
                                myBool = true;
                            }
                            break;

                        // 19. Mesh

                        // 20.MeshFace

                        // 21.MeshParameters

                        // 22.Number
                        case "Grasshopper.Kernel.Parameters.Param_Number":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Integer"
                             || yourType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                            {
                                myBool = true;
                            }
                            break;

                        // 23.OGLShader
                        
                        // 24.Path

                        // 25.Plane

                        // 26.Point
                        case "Grasshopper.Kernel.Parameters.Param_Point":
                            if (yourType == "Grasshopper.Kernel.Parameters.Param_Plane")
                            {
                                myBool = true;
                            }
                            break;
                        
                        // 27.Rectangle
                        
                        // 28.ScriptVariable

                        // 29.String

                        // 30.StructurePath

                        // 31.Surface

                        // 32.Time

                        // 33.Transform

                        // 34.Vector

                        // 35.SquishyXMorphs.GH_TwistedBox
                        
                        // 36.Field
                        
                        // 37.Matrix

                        // Default
                        default:
                            break;

                    } // End of switch
                }
                return myBool;
            }

            public static bool sliderValid(string myType, string yourType)
            {
                if (yourType == "Grasshopper.Kernel.Special.GH_NumberSlider")
                {
                    if (myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                    || myType == "Grasshopper.Kernel.Parameters.Param_Number"
                    || myType == "Grasshopper.Kernel.Parameters.Param_Integer"
                    || myType == "Grasshopper.Kernel.Parameters.Param_GenericObject")
                    {
                        return true;
                    }
                }

                return false;

            }


            
    }
}
