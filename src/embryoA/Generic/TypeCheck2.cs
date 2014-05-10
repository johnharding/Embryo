using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Embryo.Generic
{
    public static class TypeCheck2
    {
            /// <summary>
            /// Checks the type with simplified param types (experimental)
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
                        case "Brep":
                            if (yourType == "Box"
                             || yourType == "Surface"
                             || yourType == "Twisted Box")
                            {
                                myBool = true;
                            }
                            break;

                        // 5.Circle
 
                        // 6.Color

                        // 7.Complex

                        // 8.Curve
                        case "Curve":
                            if (yourType == "Line"
                             || yourType == "Circle"
                             || yourType == "Arc")
                            {
                                myBool = true;
                            }
                            break;

                        // 9.FilePath

                        // 10.GenericObject
                        case "Generic Object":
                            if (yourType == "Integer"
                             || yourType == "Number")
                            {
                                myBool = true;
                            }
                            break;

                        // 11.Geometry
                        case "Geometry":
                            if (yourType == "Box")
                            {
                                myBool = true;
                            }
                            break;

                        // 12.Group

                        // 13.GUID

                        // 14.Integer
                        case "Integer":
                            if (yourType == "Number"
                             || yourType == "Generic Object")
                            {
                                myBool = true;
                            }
                            break;

                        // 15.Interval

                        // 16.Interval2D

                        // 17.Interval2D_OBSOLETE

                        // 18.Line
                        case "Line":
                            if (yourType == "Curve")
                            {
                                myBool = true;
                            }
                            break;

                        // 19. Mesh

                        // 20.MeshFace

                        // 21.MeshParameters

                        // 22.Number
                        case "Number":
                            if (yourType == "Integer"
                             || yourType == "Generic Object")
                            {
                                myBool = true;
                            }
                            break;

                        // 23.OGLShader
                        
                        // 24.Path

                        // 25.Plane

                        // 26.Point
                        
                        // 27.Rectangle
                        
                        // 28.ScriptVariable

                        // 29.String

                        // 30.StructurePath

                        // 31.Surface

                        // 32.Time

                        // 33.Transform

                        // 34.Vector
                        case "Vector":
                            if (yourType == "Point")
                            {
                                myBool = true;
                            }
                            break;

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
                    if (myType == "Integer"
                    || myType == "Number"
                    || myType == "Integer"
                    || myType == "Generic Object")
                    {
                        return true;
                    }
                }

                return false;

            }


            
    }
}
