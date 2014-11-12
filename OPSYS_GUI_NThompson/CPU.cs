using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPSYS_GUI_NThompson
{
    public class CPUObject
    {
        private static int register1, register2, register3, register4, accumulator, programCounter;
        //static ProcessControlBlock currentPCB = StartForm.readyQueueSF.Dequeue();
        //List<Instruction> currentInst = currentPCB.GetInstructions(currentPCB.GetPCBID());
        bool endOfInstructionsFlag = false;
        public static int cpuCycles
        {
            get;
            set;
        }
        public int register1Value
        {
            get
            {
                return register1;
            }
            set
            {
                register1 = register1Value;
            }
        }
        public int register2Value
        {
            get
            {
                return register2;
            }
            set
            {
                register2 = register2Value;
            }
        }
        public int register3Value
        {
            get
            {
                return register3;
            }
            set
            {
                register3 = register3Value;
            }
        }
        public int register4Value
        {
            get
            {
                return register4;
            }
            set
            {
                register4 = register4Value;
            }
        }
        public int accumulatorValue
        {
            get
            {
                return accumulator;
            }
            set
            {
                accumulator = accumulatorValue;
            }
        }
        public bool isAvailable
        {
            get;
            set;
        }
        public CPUObject()
        {
            register1 = 1;
            register2 = 3;
            register3 = 5;
            register4 = 7;
            accumulator = 9;
            programCounter = 0;
        }
        public CPUObject(int reg1, int reg2, int reg3, int reg4, int acc, int pc)
        {
            register1 = reg1;
            register2 = reg2;
            register3 = reg3;
            register4 = reg4;
            accumulator = acc;
            programCounter = pc;
        }

        
        public void FetchDecodeAndExecute()
        {
            
            ProcessControlBlock currentPCB = StartForm.dispatch.GetJobFromReadyQ();
            //Dat one line fetch
            List<Instruction> currentInstructions = currentPCB.GetInstructions(currentPCB.GetPCBID());
            ProgramState currentProgramState = currentPCB.programState;
            bool restartFlag = false;

            int instIndex = currentProgramState.lineOfExecution;
            if (instIndex != 0)
            {
                register1Value = currentProgramState.register1;
                register2Value = currentProgramState.register2;
                register3Value = currentProgramState.register3;
                register4Value = currentProgramState.register4;
                accumulatorValue = currentProgramState.accumulator;
            }

            while (!(restartFlag))
            {
                int inst_currentLine = instIndex;
                string instType = currentInstructions[instIndex].GetInstructionType();
                int instructionValue = currentInstructions[instIndex].GetInstructionValue();
                int instID = currentInstructions[instIndex].GetJobID();
                
                //temporary lookup table
                switch (instType)
                {
                    case "add"://add two registers, done
                        MathDecisionFunction(currentInstructions[instIndex]);
                        instIndex++;
                        break;
                    case "sub"://subtract two registers, done
                        MathDecisionFunction(currentInstructions[instIndex]);
                        instIndex++;
                        break;
                    case "mul"://multiply, done
                        MathDecisionFunction(currentInstructions[instIndex]);
                        instIndex++;
                        break;
                    case "div"://divide, done
                        MathDecisionFunction(currentInstructions[instIndex]);
                        instIndex++;
                        break;
                    case "_rd"://read, send job to io queue for X cycles
                        //this needs to be reworked to include cycle values
                        StartForm.dispatch.AddToIOQ(currentPCB, instructionValue);
                        restartFlag = true;
                        break;
                    case "_wr"://write, send job to io queue for X cycles
                        //this needs to be reworked
                        StartForm.dispatch.AddToIOQ(currentPCB, instructionValue);
                        restartFlag = true;
                        break;
                    case "_wt"://wait, send job to wait queue
                        //this needs to be reworked
                        StartForm.dispatch.AddToWaitQ(currentPCB, instructionValue);
                        restartFlag = true;
                        break;
                    case "sto"://store value in acc, done
                        accumulatorValue = instructionValue;
                        instIndex++;
                        break;
                    case "rcl"://take acc value and assign to register, done
                        Recall(currentInstructions[instIndex]);
                        instIndex++;
                        break;
                    case "nul"://reset registers to default value, done
                        register1 = 1;
                        register2 = 3;
                        register3 = 5;
                        register4 = 7;
                        accumulator = 9;
                        instIndex++;
                        break;
                    case "stp"://halt execution, save state, return job to RQ
                        instIndex = 0;
                        currentProgramState.lineOfExecution = inst_currentLine;
                        currentProgramState.instructionType = instType;
                        currentProgramState.instructionValue = instructionValue;
                        currentProgramState.jobID = instID;
                        currentProgramState.register1 = register1Value;
                        currentProgramState.register2 = register2Value;
                        currentProgramState.register3 = register3Value;
                        currentProgramState.register4 = register4Value;
                        currentPCB.programState = currentProgramState;

                        //this needs to be reworked
                        //StartForm.readyQueueSF.Enqueue(pcb);
                        break;
                    case "err"://error condition, save state to PCB and terminate program
                        instIndex = 0;    
                        currentProgramState.lineOfExecution = inst_currentLine;
                        currentProgramState.instructionType = instType;
                        currentProgramState.instructionValue = instructionValue;
                        currentProgramState.jobID = instID;
                        currentProgramState.register1 = register1Value;
                        currentProgramState.register2 = register2Value;
                        currentProgramState.register3 = register3Value;
                        currentProgramState.register4 = register4Value;
                        currentPCB.programState = currentProgramState;

                        //this needs to be reworked
                        //StartForm.dispatch.termQueueD.Enqueue(pcb);
                        break;
                    default: //if SOMEHOW this case is called, it needs to be handled immediately
                        MessageBox.Show("BLUE SCREEN OF DEATH."+"\nProcess ID: " + 
                            instID + "\nInstruction Type: "+
                            instType + "\nLine of Execution: " +
                            inst_currentLine+" Restart the OS to continue.",
                            "What? No, it's totally blue. Shut up, you broke my thing. You are a thing breaker. Jerk.",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                        break;
                }
                cpuCycles++;
            }
        }

        public void Recall(Instruction inst)
        {
            string register1 = inst.GetRegister1();
            switch (register1)
            {
                case "A":
                    register1Value = accumulatorValue;
                    break;
                case "B":
                    register2Value = accumulatorValue;
                    break;
                case "C":
                    register3Value = accumulatorValue;
                    break;
                case "D":
                    register4Value = accumulatorValue;
                    break;
                default:
                    break;
            }
        }
        public void MathDecisionFunction(Instruction inst)
        {
            string register1 = inst.GetRegister1();
            string register2 = inst.GetRegister2();
            string instType = inst.GetInstructionType();
            int instValue = inst.GetInstructionValue();

            switch (instType)
            {
                case "add":
                    Add(register1, register2, instValue);
                    break;
                case "sub":
                    Subtract(register1, register2, instValue);
                    break;
                case "mul":
                    Multiply(register1, register2, instValue);
                    break;
                case "div":
                    Divide(register1, register2, instValue);
                    break;
                default:
                    break;
            }
        }

        //For anyone about to read this code, I am so sorry...
        public void Add(string reg1,string reg2, int instValue)
        {
            switch (reg1)
            {
                case "A":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register1Value + register1Value + instValue;
                            break;
                        case "B":
                            accumulatorValue += register1Value + register2Value + instValue;
                            break;
                        case "C":
                            accumulatorValue += register1Value + register3Value + instValue;
                            break;
                        case "D":
                            accumulatorValue += register1Value + register4Value + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register2Value + register1Value + instValue;
                            break;
                        case "B":
                            accumulatorValue += register2Value + register2Value + instValue;
                            break;
                        case "C":
                            accumulatorValue += register2Value + register3Value + instValue;
                            break;
                        case "D":
                            accumulatorValue += register2Value + register4Value + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register3Value + register1Value + instValue;
                            break;
                        case "B":
                            accumulatorValue += register3Value + register2Value + instValue;
                            break;
                        case "C":
                            accumulatorValue += register3Value + register3Value + instValue;
                            break;
                        case "D":
                            accumulatorValue += register3Value + register4Value + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register4Value + register1Value + instValue;
                            break;
                        case "B":
                            accumulatorValue += register4Value + register2Value + instValue;
                            break;
                        case "C":
                            accumulatorValue += register4Value + register3Value + instValue;
                            break;
                        case "D":
                            accumulatorValue += register4Value + register4Value + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Multiply(string reg1,string reg2, int instValue)
        {

            switch (reg1)
            {
                case "A":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register1Value * register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register1Value * register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register1Value * register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register1Value * register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value * register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register2Value * register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register2Value * register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register2Value * register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value * register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register3Value * register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register3Value * register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register3Value * register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register4Value * register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register4Value * register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register4Value * register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register4Value * register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Subtract(string reg1, string reg2, int instValue)
        {
            switch (reg1)
            {
                case "A":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register1Value - register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register1Value - register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register1Value - register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register1Value - register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value - register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register2Value - register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register2Value - register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register2Value - register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value - register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register3Value - register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register3Value - register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register3Value - register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register4Value - register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register4Value - register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register4Value - register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register4Value - register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Divide(string reg1, string reg2, int instValue)
        {
            switch (reg1)
            {
                case "A":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register1Value / register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register1Value / register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register1Value / register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register1Value / register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value / register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register2Value / register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register2Value / register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register2Value / register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value / register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register2Value / register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register2Value / register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register2Value / register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value / register1Value) + instValue;
                            break;
                        case "B":
                            accumulatorValue += (register3Value / register2Value) + instValue;
                            break;
                        case "C":
                            accumulatorValue += (register3Value / register3Value) + instValue;
                            break;
                        case "D":
                            accumulatorValue += (register3Value / register4Value) + instValue;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Run()
        {
            while (!(endOfInstructionsFlag))
            {
                //FetchDecodeAndExecute(currentPCB);
            }
        }
        #region Properties
        public int reg1
        {
            get
            {
                return register1;
            }
            set
            {
                reg1 = register1;
            }
        }
        public int reg2
        {
            get
            {
                return register2;
            }
            set
            {
                reg2 = register2;
            }
        }
        public int reg3
        {
            get
            {
                return register3;
            }
            set
            {
                reg3 = register3;
            }
        }
        public int reg4
        {
            get
            {
                return register4;
            }
            set
            {
                reg4 = register4;
            }
        }
        public int acc
        {
            get
            {
                return accumulator;
            }
            set
            {
                acc = accumulator;
            }
        }
        public int progCount
        {
            get
            {
                return programCounter;
            }
            set
            {
                progCount = programCounter;
            }
        }
        #endregion
    }
}
