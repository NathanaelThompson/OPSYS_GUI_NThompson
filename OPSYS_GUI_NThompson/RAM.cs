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
        public List<Instruction> instructionsInRAM;
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
        
        public void CompactRAM()
        {
            foreach (ProcessControlBlock pcb in pcbsInRAM)
            {
                if (pcb.programState.lineOfExecution >= pcb.GetPCBJobLength())
                {
                    pcb.destination = "Term";
                    pcb.location = "RAM";
                }

            }
            for (int i = 0; i < pcbsInRAM.Count; i++)
            {
                if (pcbsInRAM[i].destination == "Term")
                {
                    StartForm.dispatch.AddToTermQ(pcbsInRAM[i], 0);
                }
            }
            int gapInRAM = instructionsInRAM.Capacity - instructionsInRAM.Count;
            LongTermScheduler lts = new LongTermScheduler();
            Queue<ProcessControlBlock> hdInputQueue = HardDrive.jobsWaitingHD;
            int nextJobLength;
            try
            {
                nextJobLength = hdInputQueue.Peek().GetPCBJobLength();
            }
            catch (Exception ex)
            {
                nextJobLength = gapInRAM + 1;
            }
            if (gapInRAM > nextJobLength)
            {
                ProcessControlBlock newPCB = hdInputQueue.Dequeue();
                lts.SortedAdd(newPCB);
                StartForm.sortedPCBsInRAM.Add(newPCB);
                List<Instruction> instructsToAdd = new List<Instruction>(newPCB.GetInstructions(newPCB.GetPCBID()));
                foreach (Instruction inst in instructsToAdd)
                {
                    instructionsInRAM.Add(inst);
                }
            }

            ProcessControlBlock dynInstPCB = new ProcessControlBlock();
            foreach (Instruction inst in instructionsInRAM)
            {
                dynInstPCB = inst.GetPCB(inst.GetJobID());
                inst.instructionAddress = dynInstPCB.baseAddress + inst.GetInstructionLine();
            }
        }

        //If RAM gets corrupted, this will be the function to derail it
        //This could be reworked since I'm messing with the addressing stuff
        //public void MoveJobInRAM(ProcessControlBlock pcb, int baseAddress)
        //{
        //    List<Instruction> instructionsToMove = pcb.GetInstructions(pcb.GetPCBID());
        //    foreach (Instruction inst in instructionsToMove)
        //    {
        //        if (instructionsToMove.Count + instructionsInRAM.Count > size)
        //        {
        //            return;
        //        }
        //        else if (instructionsInRAM.Contains(inst))
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            Instruction instToSwap = new Instruction();
        //            instToSwap = inst;
        //            instructionsInRAM.Add(inst);
        //            instructionsInRAM.RemoveAt(instToSwap.instructionAddress);
        //        }
        //    }
        //}

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
                pcb.limitAddress = pcb.baseAddress + pcb.GetPCBJobLength() - 1;
            }
        }
        
        //Gets one instruction in RAM
        public Instruction GetInstructionInRAM(int address)
        {
            //int instIndex = 0;
            //Instruction tempInst = new Instruction();
            //tempInst.instIsNull = true;
            //foreach (Instruction inst in instructionsInRAM)
            //{
            //    if (address == inst.instructionAddress)
            //    {
            //        return inst;
            //    }
                
            //}
            //return tempInst;
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
            foreach (Instruction inst in instructionsInRAM)
            {
                int instIndex = instructionsInRAM.IndexOf(inst);
                inst.instructionAddress = instIndex;
            }
        }
    }
}
