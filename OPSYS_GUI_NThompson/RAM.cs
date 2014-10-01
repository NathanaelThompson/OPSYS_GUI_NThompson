using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{

    class RAMObject
    {
        int size;
        List<ProcessControlBlock> PCBList;
        static int instructionCount = 0;
        //constructor with a list of PCB objects, and size
        public RAMObject(int sz, List<ProcessControlBlock> listPCB)
        {
            size = sz;
            PCBList = listPCB;
        }

        //constructor with size parameter
        public RAMObject(int sz)
        {
            size = sz;
        }
        
        //default constructor
        public RAMObject()
        {
            size = 100;
        }

        //adds a PCB object to RAM
        public void AddPCBToRAM(ProcessControlBlock pcb)
        {
            if((pcb.GetPCBJobLength() + instructionCount) > size)
            {
                return;
            }
            else
            {
                instructionCount += pcb.GetPCBJobLength();
                PCBList.Add(pcb);
            }
        }

        //Gets the size of RAM
        public int GetRAMSize()
        {
            return size;
        }

        //Gets PCB objects in RAM
        public List<ProcessControlBlock> GetPCBsInRAM()
        {
            return PCBList;
        }
        
    }
}
