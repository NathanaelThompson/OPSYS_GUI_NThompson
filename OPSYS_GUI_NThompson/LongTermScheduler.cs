using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class LongTermScheduler
    {
        
        public LongTermScheduler()
        {

        }

        public void FirstComeFirstServe(List<ProcessControlBlock> unsortedPCB)
        {
            int sizeOfRam = StartForm.ram.GetRAMSize();
            
            //total lines of instructions
            int totalLOI = 0;

            //Gets the instructions from a PCB object's ID
            //Checks to see if they fit, then adds them
            foreach (ProcessControlBlock pcb in unsortedPCB)
            {
                int lengthOfJob = pcb.GetPCBJobLength();
                int pcbID = pcb.GetPCBID();
                List<Instruction> tempInstructions = pcb.GetInstructions(pcbID);

                if ((lengthOfJob + totalLOI + 1) < sizeOfRam)
                {
                    //sets some details about the job getting added to RAM
                    pcb.destination = "Ready";
                    pcb.baseAddress = totalLOI;
                    if (pcb.baseAddress == 0)
                    {
                        pcb.limitAddress = lengthOfJob - 1;
                    }
                    else
                    {
                        pcb.limitAddress = pcb.baseAddress + lengthOfJob - 1;
                    }
                    StartForm.ram.AddJobToRAM(pcb);
                }
                else
                {
                    pcb.destination = "Hard Drive";
                    int lastIndex = unsortedPCB.IndexOf(pcb);
                    List<ProcessControlBlock> ramFullPCBList = unsortedPCB.GetRange(lastIndex, (unsortedPCB.Count - lastIndex));
                    StartForm.hdd.ReturnJobsToHD(ramFullPCBList);
                    break;
                }

                totalLOI += lengthOfJob;
            }
        }

        public List<ProcessControlBlock> ShortestJobFirst(List<ProcessControlBlock> unsortedPCB)
        {
            int[,] lengths_ids = new int[unsortedPCB.Count, 2];
            for (int i = 0; i < unsortedPCB.Count; i++)
            {
                lengths_ids[i, 0] = unsortedPCB[i].GetPCBJobLength();
                lengths_ids[i, 1] = unsortedPCB[i].GetPCBID();
            }

            //Hybrid selection/swap sort, there's probably a name for it, but I'm unsure what it is
            int minValue = 1000000;
            int tempValue = 0;
            int tempID = 0;
            int minID = 0;
            bool passFlag = false;

            //loops until passFlag = true/until all elements are sorted
            while (!(passFlag))
            {
                for (int i = 0; i < unsortedPCB.Count - 1; i++)
                {
                    //if the length at index i is less than minValue (which on the first pass should always be true)
                    if (lengths_ids[i, 0] < minValue)
                    {
                        //if index i = 0
                        //the minValue will be the first element of the the lengths_ids array
                        if (i == 0)
                        {
                            minValue = lengths_ids[i, 0];
                            tempID = lengths_ids[i, 1];
                        }
                        else
                        {
                            minValue = lengths_ids[i + 1, 0];
                            tempValue = lengths_ids[i, 0];
                            tempID = lengths_ids[i, 1];
                            minID = lengths_ids[i + 1, 1];
                            lengths_ids[i + 1, 0] = tempValue;
                            lengths_ids[i, 0] = minValue;
                            lengths_ids[i, 1] = minID;
                            lengths_ids[i + 1, 1] = tempID;

                            //there was a swap, so the elements are still not completely sorted
                            passFlag = false;
                        }

                    }//else if value at index i > value at index i+1, swap them
                    else if (lengths_ids[i, 0] > lengths_ids[i + 1, 0])
                    {
                        minValue = lengths_ids[i + 1, 0];
                        tempValue = lengths_ids[i, 0];
                        tempID = lengths_ids[i, 1];
                        minID = lengths_ids[i + 1, 1];
                        lengths_ids[i + 1, 0] = tempValue;
                        lengths_ids[i, 0] = minValue;
                        lengths_ids[i, 1] = minID;
                        lengths_ids[i + 1, 1] = tempID;

                        //if there was a swap, reset the loop
                        i = -1;

                        //there was a swap, so the elements are still not completely sorted
                        passFlag = false;
                    }
                    else
                    {
                        passFlag = true;
                    }
                }
            }

            //Attempt to join sorted lengths with their IDs
            List<ProcessControlBlock> sortedPCBList = new List<ProcessControlBlock>(unsortedPCB.Count);
            int listIndex = 0;
            bool isSorted = false;
            while (!isSorted)
            {
                foreach (ProcessControlBlock pcb in unsortedPCB)
                {
                    //*
                    if (pcb.GetPCBID() == lengths_ids[listIndex, 1] && pcb.GetPCBJobLength() == lengths_ids[listIndex, 0])
                    {
                        sortedPCBList.Insert(listIndex, pcb);
                        listIndex++;

                        if (listIndex > (unsortedPCB.Count - 1))
                        {
                            isSorted = true;
                            break;
                        }

                    }
                    else
                    {
                        isSorted = false;
                    }
                }
            }
            StartForm.ram.SetJobsInRAM(sortedPCBList);//this is only called on the first pass
            int sizeOfRAM = StartForm.ram.GetRAMSize();
            int totalLOI = 0;
            //foreach PCB object, if they will fit, add instructions to RAM
            foreach (ProcessControlBlock pcb in sortedPCBList)
            {
                int lengthOfJob = pcb.GetPCBJobLength();
                int pcbID = pcb.GetPCBID();
                List<Instruction> tempInstructions = pcb.GetInstructions(pcbID);

                if ((lengthOfJob + totalLOI + 1) < sizeOfRAM)
                {
                    pcb.destination = "RAM";
                    pcb.baseAddress = totalLOI;
                    if (pcb.baseAddress == 0)
                    {
                        pcb.limitAddress = lengthOfJob;
                    }
                    else
                    {
                        pcb.limitAddress = pcb.baseAddress + lengthOfJob;
                    }
                    StartForm.ram.AddJobToRAM(pcb);
                }
                else
                {
                    int lastIndex = sortedPCBList.IndexOf(pcb);
                    List<ProcessControlBlock> ramFullPCBList = sortedPCBList.GetRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    if (StartForm.bigLoopCycles == 0)
                    {
                        StartForm.hdd.ReturnJobsToHD(ramFullPCBList);
                    }
                    sortedPCBList.RemoveRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    return sortedPCBList;
                }
                totalLOI += lengthOfJob + 1;
            }
            return sortedPCBList;
        }

        public List<ProcessControlBlock> PrioritySort(List<ProcessControlBlock> unsortedPCB)
        {
            //adding priorites and IDs of unsortedPCB to an array
            int[,] priorities_ids = new int[unsortedPCB.Count, 2];
            for (int i = 0; i < unsortedPCB.Count; i++)
            {
                priorities_ids[i, 0] = unsortedPCB[i].GetPCBJobPriority();
                priorities_ids[i, 1] = unsortedPCB[i].GetPCBID();
            }

            //Hybrid selection/swap sort, there's probably a name for it, but I'm unsure what it is
            int maxPriority = -1;
            int tempValue = 0;
            int tempID = 0;
            int maxID = 0;
            bool passFlag = false;

            //loops until passFlag = true/until all elements are sorted
            while (!(passFlag))
            {
                for (int i = 0; i < unsortedPCB.Count - 1; i++)
                {
                    //if the length at index i is less than minValue (which on the first pass should always be true)
                    if (priorities_ids[i, 0] > maxPriority)
                    {
                        //if index i = 0
                        //the minValue will be the first element of the the lengths_ids array
                        if (i == 0)
                        {
                            maxPriority = priorities_ids[i, 0];
                            tempID = priorities_ids[i, 1];
                        }
                        else
                        {
                            //swapping IDs and Priorities
                            maxPriority = priorities_ids[i + 1, 0];
                            tempValue = priorities_ids[i, 0];
                            tempID = priorities_ids[i, 1];
                            maxID = priorities_ids[i + 1, 1];
                            priorities_ids[i + 1, 0] = tempValue;
                            priorities_ids[i, 0] = maxPriority;
                            priorities_ids[i, 1] = maxID;
                            priorities_ids[i + 1, 1] = tempID;

                            //there was a swap, so the elements are still not completely sorted
                            passFlag = false;
                        }

                    }//else if value at index i > value at index i+1, swap them
                    else if (priorities_ids[i, 0] < priorities_ids[i + 1, 0])
                    {
                        //swapping IDs and priorities
                        maxPriority = priorities_ids[i + 1, 0];
                        tempValue = priorities_ids[i, 0];
                        tempID = priorities_ids[i, 1];
                        maxID = priorities_ids[i + 1, 1];
                        priorities_ids[i + 1, 0] = tempValue;
                        priorities_ids[i, 0] = maxPriority;
                        priorities_ids[i, 1] = maxID;
                        priorities_ids[i + 1, 1] = tempID;

                        //if there was a swap, reset the loop
                        i = -1;

                        //there was a swap, so the elements are still not completely sorted
                        passFlag = false;
                    }
                    else
                    {
                        passFlag = true;
                    }
                }
            }

            //Attempt to join sorted lengths with their IDs
            List<ProcessControlBlock> sortedPCBList = new List<ProcessControlBlock>(unsortedPCB.Count);
            
            int listIndex = 0;
            bool filledFlag = false;
            while (!filledFlag)
            {
                foreach (ProcessControlBlock pcb in unsortedPCB)
                {
                    //if there is a match between the pcb's ID and Priority, insert pcb into the sorted list
                    if (pcb.GetPCBID() == priorities_ids[listIndex, 1] && pcb.GetPCBJobPriority() == priorities_ids[listIndex, 0])
                    {
                        sortedPCBList.Insert(listIndex, pcb);
                        listIndex++;

                        //if listIndex is greated than the max index of the priorities_ids array
                        if (listIndex > (unsortedPCB.Count-1))
                        {
                            filledFlag = true;
                            break;
                        }
                        else
                        {
                            filledFlag = false;
                        }
                    }
                }
            }
            StartForm.ram.SetJobsInRAM(sortedPCBList);
            //Gets the instructions from a PCB object's ID
            //Checks to see if they fit, then adds them
            int sizeOfRAM = StartForm.ram.GetRAMSize();
            int totalLOI = 0;
            foreach (ProcessControlBlock pcb in sortedPCBList)
            {
                int lengthOfJob = pcb.GetPCBJobLength();
                int pcbID = pcb.GetPCBID();
                List<Instruction> tempInstructions = pcb.GetInstructions(pcbID);

                //if it fits, throw it in RAM
                if ((lengthOfJob + totalLOI + 1) < sizeOfRAM)
                {
                    pcb.baseAddress = totalLOI;
                    pcb.destination = "Ready";
                    if (pcb.baseAddress == 0)
                    {
                        pcb.limitAddress = lengthOfJob;
                    }
                    else
                    {
                        pcb.limitAddress = pcb.baseAddress + lengthOfJob;
                    }
                    StartForm.ram.AddJobToRAM(pcb);
                }
                else//Otherwise, return the job to the hard drive, and remove it from the sortedPCBList
                {
                    int lastIndex = sortedPCBList.IndexOf(pcb);
                    List<ProcessControlBlock> ramFullPCBList = sortedPCBList.GetRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    if (StartForm.bigLoopCycles == 0)
                    {
                        StartForm.hdd.ReturnJobsToHD(ramFullPCBList);
                    }
                    sortedPCBList.RemoveRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    return sortedPCBList;
                }
                totalLOI += lengthOfJob + 1;
            }
            return sortedPCBList;
        }

        //If the jobs are already sorted, this function gets called
        public void SortedAdd(List<ProcessControlBlock> sortedPCBsInHD)
        {
            int numOfRAMInst = StartForm.ram.GetInstructionsInRAM().Count;
            if ((sortedPCBsInHD[0].GetPCBJobLength() + numOfRAMInst) < StartForm.ram.GetRAMSize())
            {
                sortedPCBsInHD[0].destination = "RAM";
                StartForm.ram.AddJobToRAM(sortedPCBsInHD[0]);
                StartForm.ram.AddInstructionsToRAM(sortedPCBsInHD[0].GetInstructions(sortedPCBsInHD[0].GetPCBID()));
            }
        }
    }
}
