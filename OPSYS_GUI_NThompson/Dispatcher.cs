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
            else if (pcb.location == "Wait" || pcb.location == "IO")
            {
                //do nothing, leave in IO or Wait Queue
                return;
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
                    pcb.location = "Wait";
                    waitQ.Enqueue(pcb);
                }
            }
        }

        public void AddToIOQ(ProcessControlBlock pcb, int wtCycles)
        {
            if (pcb.waitCycles <= 0)
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
                    pcb.location = "IO";
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
                StartForm.ram.RemoveJobFromRAM(pcb);
                termQ.Dequeue();
                int pcbID = pcb.GetPCBID();
                int pcbAcc = pcb.programState.accumulator;
                int pcbCycles = pcb.totalCycles;
                int priority = pcb.GetPCBJobPriority();
                string message = "Process " + pcbID + " has been removed." + 
                    "\nAccumulator Value: " + pcbAcc +
                    "\nCycles: " + pcbCycles +
                    "\nPriority: " + priority;
                
                string message2 = "Job Name: " + pcb.GetPCBJobName();
                MessageBox.Show(message, message2, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //The GetJobFromXQ functions remain largely unused. Which is fine.
        //I think I'm just calling Dequeue() all over the place
        public ProcessControlBlock GetJobFromReadyQ(List<ProcessControlBlock> pcbsInRAM)
        {
            //If there's nothing in the ready queue, we have a big problem
            if (readyQ.Count <= 0)
            {
                List<ProcessControlBlock> tempJobsList = pcbsInRAM;
                ProcessControlBlock tempPCB = new ProcessControlBlock();
                tempPCB.destination = "Fail";
                foreach (ProcessControlBlock pcb in tempJobsList)
                {
                    if ((pcb.location == "IO" || pcb.location == "Wait") && pcb.waitCycles == 0)
                    {
                        pcb.location = "Ready";
                        pcb.destination = "Ready";
                        AddToReadyQ(pcb);
                        tempPCB = pcb;
                        tempPCB.location = "Ready";
                        tempPCB.destination = "Ready";
                        break;
                    }
                }
                if (tempPCB.waitCycles != -999)
                {
                    DecrementQueueTimes();
                    int queueState = CheckQueueStates();
                    if (queueState == 0 || queueState == 1)
                    {
                        return readyQ.Dequeue();
                    }
                    else
                    {
                        return GetJobFromReadyQ(pcbsInRAM);
                    }
                }
                return tempPCB;
            }
            else
            {
                return readyQ.Dequeue();
            }
        }
        public int CheckQueueStates()
        {
            foreach (ProcessControlBlock pcb in ioQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                    return 0;
                }
            }
            foreach (ProcessControlBlock pcb in waitQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                    return 1;
                }
            }
            return -999;
        }
        //public ProcessControlBlock GetJobFromWaitQ()
        //{
        //    if (waitQ.Count <= 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return waitQ.Dequeue();
        //    }
        //}
        //public ProcessControlBlock GetJobFromIOQ()
        //{
        //    if (ioQ.Count <= 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return ioQ.Dequeue();
        //    }
        //}
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
