using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class ProcessControlBlock
    {
        string jobName;
        int jobLength, jobPriority;
        public Instruction[] instructions;
        
        public ProcessControlBlock(Instruction[] instructs, string name, int length, int priority)
        {
            instructions = instructs;
            jobName = name;
            jobLength = length;
            jobPriority = priority;
        }

        public ProcessControlBlock()
        {

        }
        public Instruction[] GetInstructionsInPCB()
        {
            return instructions;
        }
        public string GetPCBJobName()
        {
            return jobName;
        }
        public int GetPCBJobLength()
        {
            return jobLength;
        }
    }
}
