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

        //UNFINISHED, DO NOT CALL
        public void CompactRAM()
        {
            int[] emptyAddressArr = new int[GetRAMSize()];
            int address = 0;
            foreach (Instruction inst in instructionsInRAM)
            {
                if (inst.Equals(null))
                {
                    MoveJobInRAM(inst.GetPCB(inst.GetJobID()), address);
                    address++;
                }
                else
                {
                    address++;
                }
            }
        }
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

        //Add single set of instructions to RAM
        public void AddJobToRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instructsToAdd = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction instruction in instructsToAdd)
            {
                instructionsInRAM.Add(instruction);
            }
        }
        //Gets instructions in RAM
        public List<Instruction> GetInstructionsInRAM()
        {
            return instructionsInRAM;
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

        public void RemoveJobFromRAM(ProcessControlBlock pcb)
        {
            List<Instruction> instToRemove = pcb.GetInstructions(pcb.GetPCBID());
            foreach (Instruction inst in instToRemove)
            {
                instructionsInRAM.Remove(inst);
            }
        }
        public void ClearRAM(List<Instruction> instructions)
        {
            instructions.Clear();
        }
    }
}
