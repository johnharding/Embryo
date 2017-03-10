using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Embryo.Graph
{
    class EmbryoInfo : Grasshopper.Kernel.GH_AssemblyInfo
    {
        public override string Description
        {
            get { return "Embryo builds graphs"; }
        }
        public override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.EmbryoMain02; }
        }
        public override string Name
        {
            get { return "Embryo"; }
        }
        public override string Version
        {
            get { return "0.5.0"; }
        }
        public override Guid Id
        {
            get { return new Guid("{19D0565A-50F4-4139-92ED-DA0A35459274}"); }
        }

        public override string AuthorName
        {
            get { return "John Harding"; }
        }
        public override string AuthorContact
        {
            get { return "johnharding@fastmail.fm"; }
        }
    }
}