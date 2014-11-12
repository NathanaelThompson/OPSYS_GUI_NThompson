using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{

    public class RAMObject
    {
        //RAMObject members
        int size;
        public static List<Instruction> instructionsInRAM;
        
        
        //constructor with size parameter
        public RAMObject(int sz)
        {
            size = sz;
            instructionsInRAM = new List<Instruction>(size);
        }
        
        //default constructor
        public RAMObject()
        {
            size = 100;
            instructionsInRAM = new List<Instruction>(size);
            
        }

        //untested
        public void CompactRAM()
        {
            int address = 0;
            
            foreach (Instruction inst in instructionsInRAM)
            {
                if (inst.Equals(null))
                {
                    ProcessControlBlock tempPCB = new ProcessControlBlock();
                    tempPCB = inst.GetPCB(inst.GetJobID());
                    
                    MoveJobInRAM(tempPCB, address);
                    address += tempPCB.GetPCBJobLength();
                }
                else
                {
                    address++;
                }
            }

            //If there are jobs waiting on the hard drive, attempt to add them to RAM
            Queue<ProcessControlBlock> hdWaitQ = StartForm.hdd.jobsWaitingHD;
            if (hdWaitQ.Count <= 0)
            {
                return;
            }
            else
            {
                ProcessControlBlock waitingPCB = hdWaitQ.Dequeue();
                AddJobToRAM(waitingPCB);
            }
            
        }

        //untested
        public int GetRAMGap(int startAddress)
        {
            int totalGap = 0;
            for (int i = startAddress; i < size; i++)
            {
                if (instructionsInRAM[i].Equals(null))
                {
                    totalGap++;
                }
                else
                {
                    return totalGap;
                }
            }
            return totalGap;
        }

        //Gets total available slots in RAM
        public int GetTotalRAMSpace()
        {
            int totalGap = 0;
            for (int i = 0; i < size; i++)
            {
                if (instructionsInRAM[i].Equals(null))
                {
                    totalGap++;
                }
            }
            return totalGap;
        }
        //untested
        public void MoveJobInRAM(ProcessControlBlock pcb, int baseAddress)
        {
            List<Instruction> instructionsToMove = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction inst in instructionsToMove)
            {
                instructionsInRAM.Insert(baseAddress, inst);
            }
        }

        //adds instructions to RAM
        public void AddInstructionsToRAM(List<Instruction> instructs)
        {
            foreach (Instruction inst in instructs)
            {
                instructionsInRAM.Add(inst);
            }
        }

        //Add single set of instructions to RAM, untested
        public void AddJobToRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instructsToAdd = pcb.GetInstructions(pcb.GetPCBID());
            int address = 0;
            foreach (Instruction instruction in instructionsInRAM)
            {
                if (instructionsInRAM[address].Equals(null))
                {
                    int ramGap = GetRAMGap(address);
                    if (pcb.GetPCBJobLength() <= ramGap)
                    {
                        StartForm.ram.AddJobToRAM(pcb);
                    }
                    else
                    {
                        StartForm.hdd.ReturnJobToHD(pcb);
                    }
                }
                address++;
            }
        }

        //Gets instructions in RAM
        public List<Instruction> GetInstructionsInRAM()
        {
            return instructionsInRAM;
        }

        //Gets one job's instructions in RAM
        public List<Instruction> GetJobInRAM(ProcessControlBlock pcb)
        {
            List<Instruction> oneJobsInstructions = pcb.GetInstructions(pcb.GetPCBID());
            return oneJobsInstructions;
        }

        //Gets the size of RAM
        public int GetRAMSize()
        {
            return size;
        }
        
        //counts instructions in RAM
        public int InstructionsInRAMCount()
        {
            return instructionsInRAM.Count;
        }

        //untested
        public void RemoveJobFromRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instToRemove = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction inst in instToRemove)
            {
                instructionsInRAM.Remove(inst);
            }
        }

        //untested
        public void ClearRAM()
        {
            instructionsInRAM.Clear();
        }
    }
}
