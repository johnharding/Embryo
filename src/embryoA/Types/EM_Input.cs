using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel.Types;
using Embryo.Generic;

namespace Embryo.Types
{
    public class EM_Input : GH_Goo<EM_Default>
    {
        public EM_Input()
        {
          this.Value = new EM_Default();
        }

        // Constructor with initial value
        public EM_Input(EM_Default theValue)
        {
          this.Value = theValue;
        }

        // Copy Constructor
        public EM_Input(EM_Input localGoo)
        {
          this.Value = localGoo.Value;
        }

        public override IGH_Goo Duplicate()
        {
            return new EM_Input(this);
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override string TypeName
        {
            get { return "EM_Input"; }
        }
        
        public override string TypeDescription
        {
            get { return "Embryo Default"; }
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
            return "EM_Default";
        }

    }
}
