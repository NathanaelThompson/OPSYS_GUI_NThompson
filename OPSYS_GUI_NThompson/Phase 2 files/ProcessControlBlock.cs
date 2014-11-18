using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class ProcessControlBlock
    {
        //PCB members
        private string destinationValue;
        string jobName;
        int jobLength, jobPriority, pcbID;
        private ProgramState progState;
        private int wtCycles = -999;
        public string destination
        {
            get
            {
                return destinationValue;
            }
            set 
            { 
                destinationValue = value; 
            }

        }
        public ProgramState programState
        {
            get
            {
                return progState;
            }
            set
            {
                progState = value;
            }
        }
        public string location
        {
            get;
            set;
        }
        public int baseAddress
        {
            get;
            set;
        }
        public int limitAddress
        {
            get
            {
                return baseAddress + jobLength;
            }
            set
            {
                limitAddress = value;
            }
        }
        public int waitCycles
        {
            get
            {
                return wtCycles;
            }
            set
            {
                wtCycles = value;
            }
        }
        public int totalCycles
        {
            get;
            set;
        }

        //overloadded Constructor
        public ProcessControlBlock(string name, int length, int priority, int id)
        {
            jobName = name;
            jobLength = length;
            jobPriority = priority;
            pcbID = id;
            ProgramState pgs = new ProgramState();
            progState = pgs;
            destinationValue = "";
        }

        //default constructor
        public ProcessControlBlock()
        {
            ProgramState pgs = new ProgramState();
            progState = pgs;
            destinationValue = "";
        }

        #region Get and Set Methods
        public string GetPCBJobName()
        {
            return jobName;
        }
        public int GetPCBJobLength()
        {
            return jobLength;
        }
        public int GetPCBJobPriority()
        {
            return jobPriority;
        }
        public int GetPCBID()
        {
            return pcbID;
        }
        public List<Instruction> GetInstructions(int pcbID)
        {
            List<Instruction> tempInstructions = new List<Instruction>();
            
            //compares the instructions' ID to the 
            //PCB's id passed as the parameter to this function
            foreach (Instruction inst in StartForm.instructions)
            {
                if (inst.GetJobID() == pcbID)
                {
                    tempInstructions.Add(inst);
                }
            }
            //return all instructions with matching ID
            return tempInstructions;
        }
        
        public void SetPCBJobName(string name)
        {
            jobName = name;
        }
        public void SetPCBJobLength(int length)
        {
            jobLength = length;
        }
        public void SetPCBJobPriority(int priority)
        {
            jobPriority = priority;
        }
        public void SetPCBID(int id)
        {
            pcbID = id;
        }
        
        #endregion
    }
}
