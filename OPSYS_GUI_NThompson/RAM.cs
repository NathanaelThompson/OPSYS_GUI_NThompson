using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    //So, I rewrote many of these functions because I kept coming across dispatching error that I mistook for RAM errors.
    //As such, some of these functions might not make sense, but I will try to comment them, and will have them working for Phase 3
    public class RAMObject
    {
        //RAMObject members
        int size;
        public static List<Instruction> instructionsInRAM;
        public List<ProcessControlBlock> pcbsInRAM;
        public bool jobAdded;
        //constructor with size parameter
        public RAMObject(int sz)
        {
            size = sz;
            instructionsInRAM = new List<Instruction>(size);
            pcbsInRAM = new List<ProcessControlBlock>();
        }

        //default constructor
        public RAMObject()
        {
            size = 100;
            instructionsInRAM = new List<Instruction>(size);
            pcbsInRAM = new List<ProcessControlBlock>();
            
        }
        //This function could DEFINITELY use a re-write.
        public void CompactRAM()
        {
            //int address = 0;
            
            ////size = size of RAM
            ////This was orignially a foreach loop, but it wasn't moving to the empty slots in RAM
            //for (int i = 0; i < size; i++)
            //{
            //    //If this tries to access an out of bounds array element, breaks out of the for loop
            //    if((i + 1) > instructionsInRAM.Count || (address + 1) > instructionsInRAM.Count)
            //    {
            //        break;
            //    }

            //    //this SHOULD be getting an instruction from RAM
            //    Instruction inst = new Instruction();
            //    inst = instructionsInRAM[address];

            //    //ask that instruction if it is null
            //    //if it isn't null, attempt to squish it inside of RAM
            //    //if it is null, address++
            //    //if (!(inst.instIsNull))
            //    //{
            //    //    ProcessControlBlock pcb = new ProcessControlBlock();
            //    //    pcb = inst.GetPCB(inst.GetJobID());
                    
            //    //    inst.GetPCB(inst.GetJobID()).baseAddress = address;
            //    //    MoveJobInRAM(pcb, pcb.baseAddress);
            //    //    address += inst.GetPCB(inst.GetJobID()).GetPCBJobLength();
            //    //}
            //    //else
            //    //{
            //    //    address++;
            //    //}
            //}

            ////If there are jobs waiting on the hard drive, attempt to add them to RAM
            //Queue<ProcessControlBlock> hdWaitQ = HardDrive.jobsWaitingHD;
            //if (hdWaitQ.Count <= 0)
            //{
            //    return;
            //}
            //else
            //{
            //    ProcessControlBlock hdPCBToAdd = new ProcessControlBlock();
            //    hdPCBToAdd = hdWaitQ.Peek();
            //    AddJobToRAM(hdPCBToAdd);
            //    if (jobAdded)
            //    {
            //        hdWaitQ.Dequeue();
            //    }
            //}
            
            ////Once all the jobs and such have been added/compacted/whatever
            ////This should reassign all the Instruction addresses
            ////Originally a regular for loop, would throw an out of bounds error
            //int instIndex = 0;
            //foreach(Instruction inst in instructionsInRAM)
            //{
            //    instructionsInRAM[instIndex].instructionAddress = instIndex;
            //    instIndex++;
            //}
        }

        //If RAM gets corrupted, this will be the function to derail it
        //This could be reworked since I'm messing with the addressing stuff
        public void MoveJobInRAM(ProcessControlBlock pcb, int baseAddress)
        {
            List<Instruction> instructionsToMove = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction inst in instructionsToMove)
            {
                if (instructionsToMove.Count + instructionsInRAM.Count > size)
                {
                    return;
                }
                else if (instructionsInRAM.Contains(inst))
                {
                    return;
                }
                else
                {
                    Instruction instToSwap = new Instruction();
                    instToSwap = inst;
                    instructionsInRAM.Add(inst);
                    instructionsInRAM.RemoveAt(instToSwap.instructionAddress);
                }
            }
        }

        //adds instructions to RAM
        public void AddInstructionsToRAM(List<Instruction> instructs)
        {
            ProcessControlBlock tempPCB = instructs[0].GetPCB(instructs[0].GetJobID());
            int instIndex = tempPCB.baseAddress;
            bool instFit = false;
            foreach (Instruction inst in instructs)
            {
                //if the job won't fit, or if any of its instructions are in RAM, don't add
                //otherwise, add the instructions
                inst.instIsNull = false;
                if (instructs.Count + instructionsInRAM.Count > size)
                {
                    jobAdded = false;
                    break;
                }
                else if (instructionsInRAM.Contains(inst))
                {
                    jobAdded = false;
                    break;
                }
                else
                {
                    instFit = true;
                    instructionsInRAM.Add(inst);
                    int instAddress = instructionsInRAM.IndexOf(inst);
                    inst.instructionAddress = instIndex + inst.GetInstructionLine();
                }
            }
            if (!instFit)
            {
                jobAdded = false;
            }
            else
            {

                jobAdded = true;
                
            }
        }

        //Add single set of instructions to RAM, untested
        public void AddJobToRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instructsToAdd = pcb.GetInstructions(pcb.GetPCBID());
            pcbsInRAM.Add(pcb);
            AddInstructionsToRAM(instructsToAdd);
            
            if (jobAdded == true && (pcb.destination != "IO" || pcb.destination != "Wait"))
            {
                pcb.location = "RAM";
                pcb.destination = "Ready";
            }
        }

        //Gets one instruction in RAM
        public Instruction GetInstructionInRAM(int address)
        {
            return instructionsInRAM[address];
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

        //Get list of PCBs in RAM
        public List<ProcessControlBlock> GetJobsInRAM()
        {
            List<ProcessControlBlock> pcbListToReturn = new List<ProcessControlBlock>();
            foreach (Instruction inst in instructionsInRAM)
            {
                pcbListToReturn.Add(inst.GetPCB(inst.GetJobID()));
            }
            pcbsInRAM = (List<ProcessControlBlock>)pcbListToReturn.Distinct();
            return (List<ProcessControlBlock>)pcbListToReturn.Distinct();
            
        }

        //Get single PCB tied to RAM
        public ProcessControlBlock GetPCBTiedToRAM(int id)
        {
            ProcessControlBlock tempPCB = new ProcessControlBlock();
            tempPCB.destination = "Fail";
            foreach (Instruction inst in instructionsInRAM)
            {
                if (inst.GetJobID() == id)
                {
                    return inst.GetPCB(id);
                }
            }
            return tempPCB;
        }

        //Gets the size of RAM
        public int GetRAMSize()
        {
            return size;
        }
        
        public void SetJobsInRAM(List<ProcessControlBlock> pcbListToSet)
        {
            pcbsInRAM = pcbListToSet;
            foreach (ProcessControlBlock pcb in pcbsInRAM)
            {
                pcb.location = "RAM";
            }
        }
        
        public void RemoveJobFromRAM(ProcessControlBlock pcb)
        {   
            pcbsInRAM.Remove(pcb);
        }

        public void RemoveInstructionSetFromRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instToRemove = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction inst in instToRemove)
            {
                instructionsInRAM.Remove(inst);
            }
        }
    }
}
