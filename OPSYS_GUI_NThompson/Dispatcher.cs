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
        //to add the PCB to the appropriate queue.

        //This function is also a real piece of shit and needs to be reworked...
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
                        AddToWaitQ(pcb, pcb.waitCycles);
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
                else if(pcb.destination == "Term")
                {
                    AddToTermQ(pcb, 0);
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
                    if (pcb.location == "Wait")
                    {
                        RemoveJobFromQ("Wait", pcb);
                    }
                    if(pcb.location == "IO")
                    {
                        RemoveJobFromQ("IO", pcb);
                    }
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
            if (pcb.waitCycles <= 0)
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
                    List<ProcessControlBlock> readyQPCBs = new List<ProcessControlBlock>(readyQ);
                    readyQ.Clear();
                    foreach (ProcessControlBlock readyQPCB in readyQPCBs)
                    {
                        if (pcb.GetPCBID() == readyQPCB.GetPCBID())
                        {
                            //readyQPCBs.Remove(readyQPCB);

                        }
                        else
                        {
                            readyQ.Enqueue(readyQPCB);
                        }
                    }
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
                    List<ProcessControlBlock> readyQPCBs = new List<ProcessControlBlock>(readyQ);
                    readyQ.Clear();
                    foreach (ProcessControlBlock readyQPCB in readyQPCBs)
                    {
                        if (pcb.GetPCBID() == readyQPCB.GetPCBID())
                        {
                            //readyQPCBs.RemoveAt(readyQPCB);
                        }
                        else
                        {
                            readyQ.Enqueue(readyQPCB);
                        }
                    }

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
                if (!StartForm.finishedJobs.Contains(pcb))
                {
                    StartForm.finishedJobs.Add(pcb);
                }
               
                RemoveJobEverywhere(pcb);
                termQ.Dequeue();
            }
        }

        public void RemoveJobEverywhere(ProcessControlBlock pcb)
        {
            StartForm.sortedPCBsInRAM.Remove(pcb);
            List<ProcessControlBlock> ramJobs = new List<ProcessControlBlock>(StartForm.ram.pcbsInRAM);
            List<ProcessControlBlock> readyList = new List<ProcessControlBlock>(readyQ);
            int readyIndex = -1;
            int ramIndex = -1;
            foreach (ProcessControlBlock readyPCB in readyList)
            {
                if (pcb.GetPCBID() == readyPCB.GetPCBID())
                {
                    readyIndex = readyList.IndexOf(readyPCB);
                }
            }

            foreach (ProcessControlBlock ramPCB in ramJobs)
            {
                if (pcb.GetPCBID() == ramPCB.GetPCBID())
                {
                    ramIndex = ramJobs.IndexOf(ramPCB);
                }
            }
            if (readyIndex != -1)
            {
                readyQ.Clear();
                readyList.Remove(pcb);
                foreach (ProcessControlBlock readyPCB in readyList)
                {
                    readyQ.Enqueue(readyPCB);
                }
            }
            if (ramIndex != -1)
            {
                StartForm.ram.RemoveJobFromRAM(pcb);
                StartForm.ram.RemoveInstructionSetFromRAM(pcb);
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
                    if ((pcb.location == "IO" || pcb.location == "Wait") && pcb.waitCycles <= 0)
                    {
                        if (pcb.location == "IO")
                        {
                            RemoveJobFromQ("IO", pcb);
                        }
                        if (pcb.location == "Wait")
                        {
                            RemoveJobFromQ("Wait", pcb);
                        }
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
                        return readyQ.Peek();
                    }
                    else
                    {
                        //if no job is available to be added to the readyQ,
                        //even after decrementing wait times,
                        //then we must wait until a job is ready
                        DecrementToZero();
                        return readyQ.Peek();
                    }
                }
                //if after all that, there are still no jobs in the readyQ
                //return a failed PCB
                DecrementToZero();
                return tempPCB;
            }
            else
            {
                return readyQ.Peek();
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
                    RemoveJobFromQ("Wait", pcb);
                    return;
                }
                else
                {
                    DecrementQueueTimes();
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
                    RemoveJobFromQ("IO", pcb);
                    return;
                }
                else
                {
                    DecrementQueueTimes();
                }
            }

            //if no job was caught in the previous foreach loop
            //decrememnt queue times

            if (allQsFail && waitQ.Count > 0 && ioQ.Count > 0 && readyQ.Count <= 0)
            {
                DecrementToZero();
            }
            else
            {
                return;
            }
            //int lowestWaitTime = -999;
            //int lowestWaitID;
            
            //int lowestIOTime = -999;
            //int lowestIOID;

            //foreach (ProcessControlBlock pcb in waitQ)
            //{
            //    if (pcb.waitCycles > lowestWaitTime)
            //    {
            //        lowestWaitID = pcb.GetPCBID();
            //        lowestWaitTime = pcb.waitCycles;
            //    }
            //}
            //foreach (ProcessControlBlock pcb in waitQ)
            //{
            //    pcb.waitCycles -= lowestWaitTime;
            //}
            //foreach (ProcessControlBlock pcb in ioQ)
            //{
            //    if (pcb.waitCycles > lowestIOTime)
            //    {
            //        lowestIOID = pcb.GetPCBID();
            //        lowestIOTime = pcb.waitCycles;
            //    }
            //}
            //foreach (ProcessControlBlock pcb in ioQ)
            //{
            //    pcb.waitCycles -= lowestIOTime;
            //}
        }
        public void RemoveJobFromQ(string queueToEdit, ProcessControlBlock pcbToRemove)
        {
            switch (queueToEdit)
            {
                case "Ready":
                    List<ProcessControlBlock> readyQList = new List<ProcessControlBlock>(readyQ);
                    for (int i = 0; i < readyQ.Count; i++)
                    {
                        if (readyQList[i].GetPCBID() == pcbToRemove.GetPCBID())
                        {
                            readyQList.Remove(pcbToRemove);
                            break;
                        }
                    }
                    readyQ.Clear();
                    for (int i = 0; i < readyQList.Count; i++)
                    {
                        readyQ.Enqueue(readyQList[i]);
                    }
                    break;
                case "IO":
                    List<ProcessControlBlock> ioQList = new List<ProcessControlBlock>(ioQ);
                    for (int i = 0; i < ioQ.Count; i++)
                    {
                        if (ioQList[i].GetPCBID() == pcbToRemove.GetPCBID())
                        {
                            ioQList.Remove(pcbToRemove);
                            break;
                        }
                    }
                    ioQ.Clear();
                    for (int i = 0; i < ioQList.Count; i++)
                    {
                        ioQ.Enqueue(ioQList[i]);
                    }
                    break;
                case "Wait":
                    List<ProcessControlBlock> waitQList = new List<ProcessControlBlock>(waitQ);
                    for (int i = 0; i < waitQ.Count; i++)
                    {
                        if (waitQList[i].GetPCBID() == pcbToRemove.GetPCBID())
                        {
                            waitQList.Remove(pcbToRemove);
                            break;
                        }
                    }
                    waitQ.Clear();
                    for (int i = 0; i < waitQList.Count; i++)
                    {
                        waitQ.Enqueue(waitQList[i]);
                    }
                    break;
                default:
                    break;
            }
        }
        public int CheckQueueStates()
        {
            foreach (ProcessControlBlock pcb in ioQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                    RemoveJobFromQ("IO", pcb);
                    return 0;
                }
            }
            foreach (ProcessControlBlock pcb in waitQ)
            {
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                    RemoveJobFromQ("Wait", pcb);
                    return 1;
                }
            }
            return -999;
        }
        
        public void DecrementQueueTimes()
        {
            List<ProcessControlBlock> waitingPCBs = new List<ProcessControlBlock>();
            if (waitQ.Count <= 0 && ioQ.Count <= 0 && readyQ.Count > 0)
            {
                return;
            }
            foreach (ProcessControlBlock pcb in waitQ)
            {
                waitingPCBs.Add(pcb);
            }
            foreach(ProcessControlBlock pcb in ioQ){
                waitingPCBs.Add(pcb);
            }
            foreach(ProcessControlBlock pcb in waitingPCBs){
                if (pcb.waitCycles <= 0)
                {
                    AddToReadyQ(pcb);
                    RemoveJobFromQ(pcb.location, pcb);
                }
            }
            if (waitQ.Count != 0)
            {
                foreach (ProcessControlBlock pcb in waitQ)
                {
                    pcb.waitCycles-=3;
                }
            }

            if (ioQ.Count != 0)
            {
                foreach (ProcessControlBlock pcb in ioQ)
                {
                    pcb.waitCycles-=3;
                }
                
            }
        }

        public void UpdateFrontOfRQ(ProcessControlBlock pcb)
        {
            if (readyQ.Count <= 0)
            {
                readyQ.Enqueue(pcb);
            }
            else
            {
                readyQ.Peek().baseAddress = pcb.baseAddress;
                readyQ.Peek().destination = pcb.destination;
                readyQ.Peek().limitAddress = pcb.limitAddress;
                readyQ.Peek().location = pcb.location;
                readyQ.Peek().programState = pcb.programState;
                readyQ.Peek().totalCycles = pcb.totalCycles;
                readyQ.Peek().waitCycles = pcb.waitCycles;
                readyQ.Peek().programState.accumulator = pcb.programState.accumulator;
                readyQ.Peek().programState.instructionType = pcb.programState.instructionType;
                readyQ.Peek().programState.instructionValue = pcb.programState.instructionValue;
                readyQ.Peek().programState.jobID = pcb.programState.jobID;
                readyQ.Peek().programState.lineOfExecution = pcb.programState.lineOfExecution;
                readyQ.Peek().programState.register1 = pcb.programState.register1;
                readyQ.Peek().programState.register2 = pcb.programState.register2;
                readyQ.Peek().programState.register3 = pcb.programState.register3;
                readyQ.Peek().programState.register4 = pcb.programState.register4;
            }
        }
    }
}
