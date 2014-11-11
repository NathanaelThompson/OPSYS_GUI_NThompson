﻿using System;
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

                if ((lengthOfJob + totalLOI) < sizeOfRam)
                {
                    StartForm.ram.AddInstructionsToRAM(tempInstructions);
                }
                else
                {
                    return;
                }

                totalLOI += lengthOfJob;
            }
        }

        public List<ProcessControlBlock> ShortestJobFirst(List<ProcessControlBlock> unsortedPCB)
        {
            int[,] lengths_ids = new int[unsortedPCB.Count,2];
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
                    ProcessControlBlock tempPCB = new ProcessControlBlock();
                    if (pcb.GetPCBID() == lengths_ids[listIndex, 1] && pcb.GetPCBJobLength() == lengths_ids[listIndex, 0])
                    {
                        sortedPCBList.Insert(listIndex, pcb);
                        listIndex++;
                        isSorted = true;
                    }
                    else
                    {
                        isSorted = false;
                    }
                }
            }
            
            int sizeOfRAM = StartForm.ram.GetRAMSize();
            int totalLOI = 0;
            //foreach PCB object, if they will fit, add instructions to RAM
            foreach (ProcessControlBlock pcb in sortedPCBList)
            {
                int lengthOfJob = pcb.GetPCBJobLength();
                int pcbID = pcb.GetPCBID();
                List<Instruction> tempInstructions = pcb.GetInstructions(pcbID);

                if ((lengthOfJob + totalLOI) < sizeOfRAM)
                {
                    StartForm.ram.AddInstructionsToRAM(tempInstructions);
                }
                else 
                {
                    int lastIndex = sortedPCBList.IndexOf(pcb);
                    for (int i = lastIndex; i < sortedPCBList.Count; i++)
                    {
                        //StartForm.waitQueueSF.Enqueue(sortedPCBList[i]);
                    }
                    sortedPCBList.RemoveRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    return sortedPCBList;
                }
                totalLOI += lengthOfJob;
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

            //Gets the instructions from a PCB object's ID
            //Checks to see if they fit, then adds them
            int sizeOfRAM = StartForm.ram.GetRAMSize();
            int totalLOI = 0;
            foreach (ProcessControlBlock pcb in sortedPCBList)
            {
                int lengthOfJob = pcb.GetPCBJobLength();
                int pcbID = pcb.GetPCBID();
                List<Instruction> tempInstructions = pcb.GetInstructions(pcbID);

                if ((lengthOfJob + totalLOI) < sizeOfRAM)
                {
                    StartForm.ram.AddInstructionsToRAM(tempInstructions);
                }
                else
                {
                    int lastIndex = sortedPCBList.IndexOf(pcb);
                    
                    for (int i = lastIndex; i < sortedPCBList.Count; i++)
                    {
                        //StartForm.waitQueueSF.Enqueue(sortedPCBList[i]);
                    }

                    sortedPCBList.RemoveRange(lastIndex, (sortedPCBList.Count - lastIndex));
                    return sortedPCBList;
                }
                totalLOI += lengthOfJob;
            }
            return sortedPCBList;
        }
        
    }
}
