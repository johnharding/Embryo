using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel.Types;
using Embryo.Generic;

namespace Embryo.Types
{
    public class EM_Goo : GH_Goo<EM_Settings>
    {
        public EM_Goo()
        {
          this.Value = new EM_Settings();
        }

        // Constructor with initial value
        public EM_Goo(EM_Settings theValue)
        {
          this.Value = theValue;
        }

        // Copy Constructor
        public EM_Goo(EM_Goo localGoo)
        {
          this.Value = localGoo.Value;
        }

        public override IGH_Goo Duplicate()
        {
            return new EM_Goo(this);
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override string TypeName
        {
            get { return "EM_Goo"; }
        }
        
        public override string TypeDescription
        {
            get { return "Embryo Settings"; }
        }

        public override object ScriptVariable()
        {
            return this.Value;
        }

        // Return a string representation of the state (value) of this instance.
        public override string ToString()
        {
            //if (this.Value == 0) { return "False"; }
            //if (this.Value > 0) { return "True"; }
            return "EM_Settings";
        }


    }
}
