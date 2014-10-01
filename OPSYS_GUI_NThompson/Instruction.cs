using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class Instruction
    {
        string jobName, instructionType, register1, register2;
        int jobNumber, instructionLine, instructionValue, jobPriority, jobLength;

        //Constructor which describes the job's name, length, and priority
        public Instruction(string name, int length, int priority)
        {
            jobName = name;
            jobLength = length;
            jobPriority = priority;
        }
        
        //Constructor describing individual instructions
        public Instruction(int line, string type, string reg1, string reg2, int value)
        {
            instructionLine = line;
            instructionType = type;
            register1 = reg1;
            register2 = reg2;
            instructionValue = value;
        }

        #region Get Methods
        public string GetJobName()
        {
            return jobName;
        }
        public int GetJobLength()
        {
            return jobLength;
        }
        public int GetJobPriority()
        {
            return jobPriority;
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
        public void SetJobName(string name)
        {
            jobName = name;
        }
        public void SetJobLength(int length)
        {
            jobLength = length;
        }
        public void SetJobPriority(int priority)
        {
            jobPriority = priority;
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
