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
        public static Queue<ProcessControlBlock> readyQ, waitQ, ioQ, termQ;

        public Dispatcher()
        {
            readyQ = new Queue<ProcessControlBlock>(10);
            waitQ = new Queue<ProcessControlBlock>(10);
            ioQ = new Queue<ProcessControlBlock>(10);
            termQ = new Queue<ProcessControlBlock>(200);
        }

        public void Dispatch(ProcessControlBlock pcb)
        {
            if (!(pcb.destination.Equals(null)))
            {
                if (pcb.destination == "IO")
                {
                    AddToIOQ(pcb);
                }
                else if (pcb.destination == "Wait")
                {
                    AddToWaitQ(pcb);
                }
                else if (pcb.destination == "Ready")
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    AddToWaitQ(pcb);
                }
            }
            StartForm.cpu.FetchDecodeAndExecute(readyQ.Dequeue());
        }
        
        public void AddToReadyQ(ProcessControlBlock pcb)
        {
            if (readyQ.Count >= 10)
            {
                AddToWaitQ(pcb);
            }
            else
            {
                readyQ.Enqueue(pcb);
            }
            
        }

        public void AddToWaitQ(ProcessControlBlock pcb)
        {
            if (waitQ.Count >= 10)
            {
                pcb.destination = "Wait";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                waitQ.Enqueue(pcb);
            }
        }

        public void AddToIOQ(ProcessControlBlock pcb)
        {
            if (ioQ.Count >= 10)
            {
                pcb.destination = "IO";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                ioQ.Enqueue(pcb);
            }
        }

        public void AddToTermQ(ProcessControlBlock pcb)
        {
            //Seriously, this should never EVER be true
            //I can't even imagine a scenario where the terminate queue reaches 
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
                termQ.Dequeue();
            }
        }


    }
}
