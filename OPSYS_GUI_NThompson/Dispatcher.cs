using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    /*
     * Two goals of Dispatcher class
     * -----------------------------
     * 1.Context switching
     * 2.Speed****
     */
    public class Dispatcher
    {
        private Queue<ProcessControlBlock> readyQD, waitQD, ioQD, termQD;

        //Whenever the dispatcher is created, it grabs the state
        //of the PCBs as they were arranged by the LTS
        public Dispatcher()
        {
            readyQD = StartForm.readyQueueSF;
            waitQD = StartForm.waitQueueSF;
            ioQD = StartForm.ioQueueSF;
            termQD = StartForm.termQueueSF;
        }
        public Queue<ProcessControlBlock> readyQueueD
        {
            get
            {
                return readyQD;
            }
            set
            {
                readyQueueD = readyQD;
            }
        }
        public Queue<ProcessControlBlock> waitQueueD
        {
            get
            {
                return waitQD;
            }
            set
            {
                waitQueueD = waitQD;
            }
        }
        public Queue<ProcessControlBlock> ioQueueD
        {
            get
            {
                return ioQD;
            }
            set
            {
                ioQueueD = ioQD;
            }
        }
        public Queue<ProcessControlBlock> termQueueD
        {
            get
            {
                return termQD;
            }
            set
            {
                termQueueD = termQD;
            }
        }
    }
}
