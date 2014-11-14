using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class Instruction
    {
        //Instruction members
        string instructionType, register1, register2;
        int instructionLine, instructionValue, jobID, address;

        public int instructionAddress
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public Instruction()
        {
            //default Instruction Construction
            //See what I did there?
        }
        
        //Constructor describing individual instructions
        public Instruction(int line, string type, string reg1, string reg2, int value, int jobID)
        {
            instructionLine = line;
            instructionType = type;
            register1 = reg1;
            register2 = reg2;
            instructionValue = value;
            this.jobID = jobID;
        }

        #region Get Methods
        public int GetJobID()
        {
            return jobID;
        }
        public ProcessControlBlock GetPCB(int id)
        {
            ProcessControlBlock pcbToReturn = new ProcessControlBlock();
            foreach (ProcessControlBlock pcb in StartForm.pcbList)
            {
                if (id == pcb.GetPCBID())
                {
                    pcbToReturn = pcb;
                    break;
                }
                
            }
            return pcbToReturn;
        }
        public int GetInstructionLine()
        {
            return instructionLine;
        }
        public string GetInstructionType()
        {
            return instructionType;
        }
        public string GetRegister1()
        {
            return register1;
        }
        public string GetRegister2()
        {
            return register2;
        }
        public int GetInstructionValue()
        {
            return instructionValue;
        }
        #endregion

        #region Set Methods
        public void SetJobID(int id)
        {
            jobID = id;
        }
       
        public void SetInstructionLine(int lineNumber)
        {
            instructionLine = lineNumber;
        }
        public void SetInstructionType(string type)
        {
            instructionType = type;
        }
        public void SetRegister1(string reg1)
        {
            register1 = reg1;
        }
        public void SetRegister2(string reg2)
        {
            register2 = reg2;
        }
        public void SetInstructionValue(int value)
        {
            instructionValue = value;
        }
        #endregion
    }
}
