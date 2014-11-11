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

        //The following functions CANNOT STAY LIKE THEY ARE, THEY MUST BE CHANGED
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
                return;
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
                return;
            }
            else
            {
                ioQ.Enqueue(pcb);
            }
        }

        public void AddToTermQ(ProcessControlBlock pcb)
        {
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
            }
        }


    }
}
