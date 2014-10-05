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

        //adds instructions to RAM
        public void AddInstructionsToRAM(List<Instruction> instructs)
        {
            foreach (Instruction inst in instructs)
            {
                instructionsInRAM.Add(inst);
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
    }
}
