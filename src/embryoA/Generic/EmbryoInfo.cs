using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Embryo.Generic
{
    public class EmbryoInfo : Grasshopper.Kernel.GH_AssemblyInfo
    {
        public override string Description
        {
            get { return "Generates new graphs automatically"; }
        }
        public override System.Drawing.Bitmap Icon
        {
            get { return Embryo.Properties.Resources.EmbryoMain02; }
        }
        public override string Name
        {
            get { return "Embryo"; }
        }
        public override string Version
        {
            //post-embryoviz release
            get { return "0.2.1"; }
        }
        public override Guid Id
        {
            get { return new Guid("{2d93b4d4-c51c-43e9-b3bc-c6823838ea47}"); }
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
