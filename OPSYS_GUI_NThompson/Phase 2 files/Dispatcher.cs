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

        //This is what I call the ignorant dispatcher
        //It doesn't know anything about the PCB, but runs a series of tests
        //to add the PCB to the appropriate queue
        public void DispatchInitialQueue(ProcessControlBlock pcb)
        {
            if (!(pcb.destination == ""))
            {
                if (pcb.destination == "IO")
                {
                    if (ioQ.Contains(pcb))
                    {
                        return;
                    }
                    else
                    {
                        AddToIOQ(pcb, pcb.waitCycles);
                    }
                }
                else if (pcb.destination == "Wait")
                {
                    if (waitQ.Contains(pcb))
                    {
                        return;
                    }
                    else
                    {
                        AddToWaitQ(pcb, 0);
                    }
                }
                else if (pcb.destination == "Ready")
                {
                    if (readyQ.Contains(pcb))
                    {
                        return;
                    }
                    else
                    {
                        AddToReadyQ(pcb);
                    }
                }
                else
                {
                    //AddToWaitQ(pcb, pcb.waitCycles);
                    if (pcb.destination == "RAM")
                    {
                        StartForm.ram.AddJobToRAM(pcb);
                    }
                }
            }
            else
            {
                AddToReadyQ(pcb);
            }
            
        }
        

        //An attempt to add a job to the ready queue
        public void AddToReadyQ(ProcessControlBlock pcb)
        {
            if (readyQ.Contains(pcb))
            {
                return;
            }

            //the default waitCycles value is -999,
            //so this asks if the PCB has been assigned wait cycles at all
            if (pcb.waitCycles == -999)
            {
                pcb.waitCycles = 0;
            }
            
            //if readyQ is full
            if (readyQ.Count >= 10)
            {
                //add to waitQ
                AddToWaitQ(pcb, pcb.waitCycles);
            }
            else if ((pcb.location == "Wait" || pcb.location == "IO") || pcb.destination == "Ready")
            {//if there is a job in the wait or IO queue that wish to eventually enter the ready queue
                if (pcb.waitCycles <= 0)
                {//check their wait cycles
                    pcb.location = "Ready";
                    pcb.destination = "CPU";
                    AddToReadyQ(pcb);
                }
                return;
            }
            else
            {
                pcb.location = "Ready";
                pcb.destination = "CPU";
                readyQ.Enqueue(pcb);
            }
        }

        //This function and the AddToIOQ function follow almost identical logic as the
        //GetJobFromReadyQ function
        public void AddToWaitQ(ProcessControlBlock pcb, int wtCycles)
        {
            if (waitQ.Contains(pcb))
            {
                return;
            }
            if (pcb.waitCycles == -999)
            {
                pcb.waitCycles = wtCycles;
            }
            if (waitQ.Count >= 10)
            {
                pcb.destination = "Wait";
                pcb.location = "RAM";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                if (wtCycles <= 0)
                {
                    pcb.location = "Wait";
                    pcb.destination = "Ready";
                    //AddToReadyQ(pcb);
                    //was originally here, but it would create a Stack Overflow error if the readyQ was full
                }
                else
                {
                    pcb.location = "Wait";
                    pcb.destination = "Ready";
                    waitQ.Enqueue(pcb);
                }
            }
        }

        public void AddToIOQ(ProcessControlBlock pcb, int wtCycles)
        {
            if (ioQ.Contains(pcb))
            {
                return;
            }
            if (pcb.waitCycles <= 0)
            {
                pcb.waitCycles = wtCycles;
            }
            if (ioQ.Count >= 10)
            {
                pcb.destination = "IO";
                pcb.location = "RAM";
                StartForm.ram.AddJobToRAM(pcb);
            }
            else
            {
                if (pcb.waitCycles <= 0)
                {
                    pcb.location = "IO";
                    pcb.destination = "Ready";
                    //AddToReadyQ(pcb);
                }
                else
                {
                    pcb.location = "IO";
                    pcb.destination = "Ready";
                    ioQ.Enqueue(pcb);
                }
            }
        }

        //If a job is added to the termQ, we have to make sure that we keep relevant data about the job to display to the user
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
                StartForm.ram.RemoveInstructionSetFromRAM(pcb);
                termQ.Dequeue();
                //int pcbID = pcb.GetPCBID();
                //int pcbAcc = pcb.programState.accumulator;
                //int pcbCycles = pcb.totalCycles;
                //int priority = pcb.GetPCBJobPriority();
                //string message = "Process " + pcbID + " has been removed." + 
                //    "\nAccumulator Value: " + pcbAcc +
                //    "\nCycles: " + pcbCycles +
                //    "\nPriority: " + priority;
                
                //string message2 = "Job Name: " + pcb.GetPCBJobName();
                //MessageBox.Show(message, message2, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //The GetJobFromXQ functions remain (mostly) unused
        //I'm just calling Dequeue() all over the place
        public ProcessControlBlock GetJobFromReadyQ(List<ProcessControlBlock> pcbsInRAM)
        {
            //If there's nothing in the ready queue, we have a big, complicated problem
            if (readyQ.Count <= 0)
            {
                //get everything in RAM
                List<ProcessControlBlock> tempJobsList = pcbsInRAM;
                ProcessControlBlock tempPCB = new ProcessControlBlock();

                //essentially setting a fail flag
                tempPCB.destination = "Fail";
                foreach (ProcessControlBlock pcb in tempJobsList)
                {
                    //if a job is located in one of the queues, and is done waiting,
                    //go ahead and add it to the readyQ next
                    //change the "fail flag"
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

                //if there are no jobs in the readyQ
                //and the fail flag foreach doesn't send something to the readyQ
                //this checks to see if the tempPCB has been assigned wait cycles at all
                if (tempPCB.waitCycles != -999 || tempPCB.waitCycles >= 0)
                {
                    //at this point, everything in RAM is waiting, so we should decrement waiting times
                    //check to see if anything can go into the readyQ
                    //then return the job we just added to the ready queue
                    DecrementQueueTimes();
                    int queueState = CheckQueueStates();
                    if (queueState == 0 || queueState == 1)
                    {
                        return readyQ.Dequeue();
                    }
                    else
                    {
                        //if no job is available to be added to the readyQ,
                        //even after decrementing wait times,
                        //then we must wait until a job is ready
                        DecrementToZero();
                        return readyQ.Dequeue();
                    }
                }
                else
                {
                    DecrementQueueTimes();
                }
                //if after all that, there are still no jobs in the readyQ
                //return a failed PCB
                return tempPCB;
            }
            else
            {
                return readyQ.Dequeue();
            }
        }

        //For readyQ handling,
        //Decrement the io and wait Qs until at least 1 job reaches a waiting time of zero
        public void DecrementToZero()
        {
            bool allQsFail = true;
            foreach (ProcessControlBlock pcb in waitQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    pcb.location = "Ready";
                    pcb.destination = "Ready";
                    allQsFail = false;
                    AddToReadyQ(pcb);
                    return;
                }
            }
            
            //if no job was caught in the previous foreach loop
            //decrememnt queue times
            DecrementQueueTimes();
            foreach (ProcessControlBlock pcb in ioQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    pcb.location = "Ready";
                    pcb.destination = "Ready";
                    allQsFail = false;
                    AddToReadyQ(pcb);
                    return;
                }
            }

            //if no job was caught in the previous foreach loop
            //decrememnt queue times
            DecrementQueueTimes();
            if (allQsFail)
            {
                DecrementToZero();
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
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    pcb.waitCycles--;
                }
            }
            foreach (ProcessControlBlock pcb in currentPCBsIO)
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                }
                else
                {
                    pcb.waitCycles--;
                }
                
            }
        }

    }
}
