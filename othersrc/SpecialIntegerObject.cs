using System;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Embryo.Utilities
{
    public class SpecialIntegerObject : GH_Param<GH_Integer>
    {
        
        public SpecialIntegerObject() :
            base(new GH_InstanceDescription("Special Integer", "SpInt", "Provides special integers", "Embryo", "Utilities"))
        { }

        public override void CreateAttributes()
        {
            m_attributes = new SpecialIntegerAttributes(this);
        }

        //protected override Bitmap Icon
        //{
        //    get
        //    {
        //        //TODO: return a proper icon here.
        //        return Properties.Resources.E4;
        //    }
        //}
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary | GH_Exposure.hidden;
            }
        }
        public override System.Guid ComponentGuid
        {
            get { return new Guid("{06de8b82-6bf4-4e35-bd39-6bfb421f5970}"); }
        }

        private int m_value = 6;
        public int Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Since we're doing something special, we need to override CollectVolatileData_Custom
        /// to put our local integer into the volatile data fiels.
        /// </summary>
        protected override void CollectVolatileData_Custom()
        {
            VolatileData.Clear();
            AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, new GH_Integer(Value));
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetInt32("SpecialInteger", m_value);
            return base.Write(writer);
        }
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            m_value = 0;
            reader.TryGetInt32("SpecialInteger", ref m_value);
            return base.Read(reader);
        }
    }

    
}