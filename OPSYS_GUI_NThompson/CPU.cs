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
        private int register1, register2, register3, register4, accumulator, pc;
        
        bool endOfInstructionsFlag = false;

        public int programCounter
        {
            get
            {
                return pc;
            }
            set
            {
                pc = value;
            }
        }
        public int register1Value
        {
            get
            {
                return register1;
            }
            set
            {
                register1 = value;
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
                register2 = value;
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
                register3 = value;
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
                register4 = value;
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
                accumulator = value;
            }
        }
        public bool isAvailable
        {
            get;
            set;
        }
        public bool contextSwitchFlag
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

        
        ProgramState currentProgramState = new ProgramState();
        public void Fetch(ProcessControlBlock pcb)
        {
            //sets the program state, program counter, gets the instruction from RAM, then decodes and Executes it
            ProgramState pgst = new ProgramState();
            pgst = pcb.programState;
            if (pgst.lineOfExecution > pcb.GetPCBJobLength()-1)
            {
                dspatcher.AddToTermQ(pcb, 0);
            }
            programCounter = pcb.baseAddress + pgst.lineOfExecution;
            Instruction currentInstruction = StartForm.ram.GetInstructionInRAM(programCounter);
            DecodeAndExecute(currentInstruction, pcb);
        }
        Dispatcher dspatcher = new Dispatcher();
        
        public void DecodeAndExecute(Instruction inst, ProcessControlBlock currentPCB)
        {
            
            currentPCB.location = "CPU";
            currentPCB.destination = "Ready";
            dspatcher = StartForm.dispatch;
            currentProgramState = currentPCB.programState;
            
            //have a small problem when checking for lastInstruction
            bool lastInstruction = false;

            //If a job has been here before, get the program state
            if ((inst.GetInstructionLine() != 0) && (inst.GetInstructionLine() + 1 < currentPCB.GetPCBJobLength()))
            {
                register1Value = currentProgramState.register1;
                register2Value = currentProgramState.register2;
                register3Value = currentProgramState.register3;
                register4Value = currentProgramState.register4;
                accumulatorValue = currentProgramState.accumulator;
            }
            
            int inst_currentLine = inst.GetInstructionLine();
            string instType = inst.GetInstructionType();
            int instructionValue = inst.GetInstructionValue();
            int instID = inst.GetJobID();

            if (inst_currentLine + 1 > currentPCB.GetPCBJobLength())
            {
                lastInstruction = true;
            }

            //psuedo lookup table
            switch (instType)
            {
                case "add"://add two registers, done
                    MathDecisionFunction(inst);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "sub"://subtract two registers, done
                    MathDecisionFunction(inst);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "mul"://multiply, done
                    MathDecisionFunction(inst);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "div"://divide, done
                    MathDecisionFunction(inst);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "_rd"://read, send job to io queue for X cycles, done
                    dspatcher.readyQ.Dequeue();
                    currentPCB.destination = "IO";
                    dspatcher.AddToIOQ(currentPCB, instructionValue);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "_wr"://write, send job to io queue for X cycles, done
                    dspatcher.readyQ.Dequeue();
                    currentPCB.destination = "IO";
                    dspatcher.AddToIOQ(currentPCB, instructionValue);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "_wt"://wait, send job to wait queue, done
                    dspatcher.readyQ.Dequeue();
                    currentPCB.destination = "Wait";
                    dspatcher.AddToWaitQ(currentPCB, instructionValue);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "sto"://store value in acc, done
                    accumulatorValue = instructionValue;
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "rcl"://take acc value and assign to register, done
                    Recall(inst);
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "nul"://reset registers to default value, done
                    register1 = 1;
                    register2 = 3;
                    register3 = 5;
                    register4 = 7;
                    accumulator = 9;
                    currentProgramState.lineOfExecution++;
                    currentPCB.totalCycles++;
                    dspatcher.DecrementQueueTimes();
                    break;
                case "stp"://halt execution, save state, return job to RQ, done
                    currentProgramState.lineOfExecution = inst_currentLine + 1;
                    currentProgramState.instructionType = instType;
                    currentProgramState.instructionValue = instructionValue;
                    currentProgramState.jobID = instID;
                    currentProgramState.register1 = register1Value;
                    currentProgramState.register2 = register2Value;
                    currentProgramState.register3 = register3Value;
                    currentProgramState.register4 = register4Value;
                    currentPCB.programState = currentProgramState;

                    currentPCB.totalCycles++;
                    dspatcher.AddToReadyQ(currentPCB);
                    dspatcher.DecrementQueueTimes();

                    break;
                case "err"://error condition, save state to PCB and terminate program, done
                    dspatcher.readyQ.Dequeue();
                    currentProgramState.lineOfExecution = inst_currentLine;
                    currentProgramState.instructionType = instType;
                    currentProgramState.instructionValue = instructionValue;
                    currentProgramState.jobID = instID;
                    currentProgramState.register1 = register1Value;
                    currentProgramState.register2 = register2Value;
                    currentProgramState.register3 = register3Value;
                    currentProgramState.register4 = register4Value;
                    currentPCB.programState = currentProgramState;

                    currentPCB.totalCycles++;
                    dspatcher.AddToTermQ(currentPCB, 0);
                    dspatcher.DecrementQueueTimes();

                    break;
                default: //if SOMEHOW this case is called, it needs to be handled immediately
                    MessageBox.Show("BLUE SCREEN OF DEATH." + "\nProcess ID: " +
                        instID + "\nInstruction Type: " +
                        instType + "\nLine of Execution: " +
                        inst_currentLine + " Restart the OS to continue.",
                        "What? No, it's totally blue. Shut up, you broke my thing. You are a thing breaker. Jerk.",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                    break;
                
            }
            if (lastInstruction)
            {
                dspatcher.AddToTermQ(currentPCB, 0);
            }
            else
            {
                //dspatcher.AddToReadyQ(currentPCB);
                dspatcher.UpdateFrontOfRQ(currentPCB);
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
                            accumulatorValue += register1Value + register1Value;
                            break;
                        case "B":
                            accumulatorValue += register1Value + register2Value;
                            break;
                        case "C":
                            accumulatorValue += register1Value + register3Value;
                            break;
                        case "D":
                            accumulatorValue += register1Value + register4Value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register2Value + register1Value;
                            break;
                        case "B":
                            accumulatorValue += register2Value + register2Value;
                            break;
                        case "C":
                            accumulatorValue += register2Value + register3Value;
                            break;
                        case "D":
                            accumulatorValue += register2Value + register4Value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register3Value + register1Value;
                            break;
                        case "B":
                            accumulatorValue += register3Value + register2Value;
                            break;
                        case "C":
                            accumulatorValue += register3Value + register3Value;
                            break;
                        case "D":
                            accumulatorValue += register3Value + register4Value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += register4Value + register1Value;
                            break;
                        case "B":
                            accumulatorValue += register4Value + register2Value;
                            break;
                        case "C":
                            accumulatorValue += register4Value + register3Value;
                            break;
                        case "D":
                            accumulatorValue += register4Value + register4Value;
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
                            accumulatorValue += (register1Value * register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register1Value * register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register1Value * register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register1Value * register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value * register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register2Value * register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register2Value * register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register2Value * register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value * register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register3Value * register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register3Value * register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register3Value * register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register4Value * register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register4Value * register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register4Value * register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register4Value * register4Value);
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
                            accumulatorValue += (register1Value - register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register1Value - register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register1Value - register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register1Value - register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value - register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register2Value - register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register2Value - register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register2Value - register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value - register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register3Value - register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register3Value - register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register3Value - register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register4Value - register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register4Value - register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register4Value - register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register4Value - register4Value);
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
                            accumulatorValue += (register1Value / register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register1Value / register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register1Value / register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register1Value / register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "B":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value / register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register2Value / register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register2Value / register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register2Value / register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "C":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register2Value / register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register2Value / register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register2Value / register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register2Value / register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case "D":
                    switch (reg2)
                    {
                        case "A":
                            accumulatorValue += (register3Value / register1Value);
                            break;
                        case "B":
                            accumulatorValue += (register3Value / register2Value);
                            break;
                        case "C":
                            accumulatorValue += (register3Value / register3Value);
                            break;
                        case "D":
                            accumulatorValue += (register3Value / register4Value);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
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
