using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Queue<ProcessControlBlock> readyQ, waitQ, ioQ, termQ;

        public Dispatcher()
        {
            readyQ = new Queue<ProcessControlBlock>(10);
            waitQ = new Queue<ProcessControlBlock>(10);
            ioQ = new Queue<ProcessControlBlock>(10);
            termQ = new Queue<ProcessControlBlock>(200);
        }

        public void DispatchInitialQueue(ProcessControlBlock pcb)
        {
            if (!(pcb.destination == ""))
            {
                if (pcb.destination == "IO")
                {
                    AddToIOQ(pcb, pcb.waitCycles);
                }
                else if (pcb.destination == "Wait")
                {
                    AddToWaitQ(pcb, 0);
                }
                else if (pcb.destination == "Ready")
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    AddToWaitQ(pcb, pcb.waitCycles);
                }
            }
            else
            {
                AddToReadyQ(pcb);
            }
            
        }
        
        public void AddToReadyQ(ProcessControlBlock pcb)
        {
            if (pcb.waitCycles == -999)
            {
                pcb.waitCycles = 0;
            }
            if (readyQ.Count >= 10)
            {
                AddToWaitQ(pcb, pcb.waitCycles);
            }
            else
            {
                readyQ.Enqueue(pcb);
            }
            
        }

        public void AddToWaitQ(ProcessControlBlock pcb, int wtCycles)
        {
            if (pcb.waitCycles == -999)
            {
                pcb.waitCycles = wtCycles;
            }
            if (waitQ.Count >= 10)
            {
                pcb.destination = "Wait";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                if (wtCycles <= 0)
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    waitQ.Enqueue(pcb);
                }
            }
        }

        public void AddToIOQ(ProcessControlBlock pcb, int wtCycles)
        {
            if (pcb.waitCycles == -999)
            {
                pcb.waitCycles = wtCycles;
            }
            if (ioQ.Count >= 10)
            {
                pcb.destination = "IO";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    ioQ.Enqueue(pcb);
                }
            }
        }

        public void AddToTermQ(ProcessControlBlock pcb, int waitCycles)
        {
            //Seriously, this should never EVER be true
            //I can't even imagine a scenario where the terminate queue reaches 200
            if (termQ.Count >= 200)
            {
                MessageBox.Show("BLUE SCREEN OF DEATH." + "\nProcess ID: " +
                            pcb.GetPCBID() + 
                            "\nRestart the OS to continue.",
                            "What? No, it's totally blue. Shut up, you broke my thing. You are a thing breaker. Jerk.",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                termQ.Enqueue(pcb);
                StartForm.finishedJobs.Add(pcb);
                termQ.Dequeue();
            }
        }

        public ProcessControlBlock GetJobFromReadyQ()
        {
            if (readyQ.Count <= 0)
            {
                //signal that there are no more jobs
                return null;
            }
            else
            {
                return readyQ.Dequeue();
            }
        }
        public ProcessControlBlock GetJobFromWaitQ()
        {
            if (waitQ.Count <= 0)
            {
                return null;
            }
            else
            {
                return waitQ.Dequeue();
            }
        }
        public ProcessControlBlock GetJobFromIOQ()
        {
            if (ioQ.Count <= 0)
            {
                return null;
            }
            else
            {
                return ioQ.Dequeue();
            }
        }
        public void DecrementQueueTimes()
        {
            List<ProcessControlBlock> currentPCBsWait = new List<ProcessControlBlock>(waitQ);
            List<ProcessControlBlock> currentPCBsIO = new List<ProcessControlBlock>(ioQ);
            foreach (ProcessControlBlock pcb in currentPCBsWait)
            {
                pcb.waitCycles--;
            }
            foreach (ProcessControlBlock pcb in currentPCBsIO)
            {
                pcb.waitCycles--;
            }
        }

    }
}
