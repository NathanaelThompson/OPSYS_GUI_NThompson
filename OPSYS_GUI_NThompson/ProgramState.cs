using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class ProgramState
    {
        
        public ProgramState()
        {
            lineOfExecution = 0;
            instructionType = "";
            register1 = 1;
            register2 = 3;
            register3 = 5;
            register4 = 7;
            accumulator = 9;
        }
        public int lineOfExecution
        {
            get;
            set;
        }
        public string instructionType
        {
            get;
            set;
        }
        public int register1
        {
            get;
            set;
        }
        public int register2
        {
            get;
            set;
        }
        public int register3
        {
            get;
            set;
        }
        public int register4
        {
            get;
            set;
        }
        public int accumulator
        {
            get;
            set;
        }
        public int instructionValue
        {
            get;
            set;
        }
        public int jobID
        {
            get;
            set;
        }
    }
}
