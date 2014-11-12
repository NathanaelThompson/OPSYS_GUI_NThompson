/*
 * Nathanael Thompson
 * Operating Systems
 * SPSU Fall 2014
 * Prof. Michael Franklin
 * 
 */
/*
 * TO DO
 * --------
 * Phase 1: Load instructions from text file into bootstrap program. (COMPLETED)
 *          Format instructions. (COMPLETED)
 *          Move instructions to the hard drive. (COMPLETED)
 *          Move the instructions to the long term scheduler. (COMPLETED)
 *          Sort jobs and move into PCBs (COMPLETED)
 *          Sort instructions based on three algorithms: (COMPLETED)
 *              1.First come first serve (good)
 *              2.Priority (may be missing last job)
 *              3.Shortest job first (may be missing last job)
 *          Move each sorted instruction to RAM. (COMPLETED)
 *          Display information about instructions to user. (COMPLETED)
 * 
 * Phase 2:  Jobs                                                                                  Instructions
 *         -------------------------------------------------------------------------------------------------------------------------------------------------
 *         | Moves jobs from LTS to Ready/Wait queues (COMPLETE)                                 | Moves instructions from RAM to CPU (IN PROGRESS)        |
 *         | Create Dispatcher class (IN PROGRESS)                                               | Create CPU class (IN PROGRESS)                          |
 *         | Display Job Information to user (process ID, values in register upon termination)   | Process information in the CPU (fetch, decode, execute) |
 *         |                                                                                     |   -Move data to and from registers and accumulator      |
 *         |                                                                                     |                                                         |
 *         -------------------------------------------------------------------------------------------------------------------------------------------------
 * Phase 3: Implement multi-threaded environment
 *          Gather statistics about the various multi-CPU environments
 *          Create graphs and report
 *
 */

/*
 * Notes
 * --------
 * 8/26/14: Simulated operating system.  Note that as of right now we are not manipulating the physical hardware. Quite frankly, I'm glad.
 *          I may feel comfortable in C#, but there is no reason for me to try to manipulate the hardware until I actually know how the abstract
 *          pieces interact.
 *          
 * 8/28/14: Orignially this was going to be a console application.  
 *          However, multiple times in class Prof. Franklin stated that he preferred ease-of-use
 *          and style over bare-bones functionality.  As such, I intend to make this 
 *          program run with one button click. 
 *          
 * 9/17/14: I've neglected this project too long.  I've been way too busy with Applications Programming.  
 *          
 * 10/1/14: Once again, I've neglected this project for far too long.  This is due in 5 days, but it is pretty far along 
 *          after only being worked on for about 5 hours.  Luckily, I have this weekend, and the rest of this week
 *          to really get my hands dirty with this project.  
 *          
 * 10/2/14: I can't believe how easy this project is.  This isn't even the hardest programming assignment I've had this week.
 *          This is pretty much done.  The only thing left is the Priority sort and the SJF sort.
 *          
 * 10/3/14: Issues sorting PCBs.
 * 10/4/14: This project is fully functional.  I haven't tested ram size for extreme values, but that's it.  I am going to add an option for
 *          the user to select the file from an open file dialog.
 *          
 * 11/4/14: Hard to believe it has been a full month since I last looked at this.
 *          I have a few questions for Franklin, but today I started working
 *          on managing a program's/job's state, and the dispatcher.  
 *          
 *          I'm starting to see a pattern emerge.  There are two sets of data we have to keep track of: the job headers (pcbs), and
 *          the actual instructions.  Both are created at the hard drive, but the instructions are sent to RAM, and the pcbs are sent to the lts.
 *          The trick is to manage this flow of information for phase 2.  What gets passed to the CPU, and what gets passed to the dispatcher?
 *          Obviously instructions are going to the CPU, and pcbs are heading to the dispatcher.  But what order?  Well at the start it will
 *          be as the sorted list from the LTS. But more jobs have to read in later.  The challenge lies in the fact that you have to save
 *          the states of the job AND the state of the PCBs.  So the intelligent thing would be to focus on one or the other.
 *          For any given day, I should focus my effort on either the dispatcher or the CPU.
 *          
 * 11/5/14: The phrase of the day is: Von Neumann CPU architecture.  My plan for today is to have a sort of map for the CPU.
 *          When an instruction is headed to the CPU, it has 1 of 12 instruction types. I need to have a lookup table, or something mimicking one.
 *          
 *          After working for about 30 minutes, I think I might need to start with the dispatcher.
 *          
 *          Jobs added to Ready/Wait queue on first pass.
 *    
 * 11/8/14: Ton of CPU work today.  I hope to have this project done by Friday 11/14.  My brother is getting married so I'll be pretty
 *          involved with that for the weekend before phase 2 of this project is due.
 *          
 *          As of right now, 9 of the 12 instruction types are complete and work on the other 3 has already begun.
 *          
 * 11/11/14:I have cleared my schedule as much as I can.  This is now a full 6 day code marathon.  I'm only taking breaks to eat, to sleep,
 *          to go to class, and to go to my brother's wedding.  Today's task is compacting RAM.
 *          
 *          UGH. Found a soul crushing error in the Math portion of my CPU. Gonna be a long day...
 *          Ok, problem fixed.  Everything that depends on the Queues needs to be rewritten.
 *          
 *          PCBs/RAM need/s an addressing system.  There has to be so much swapping and compacting that I really need to try to simplify
 *          the process.
 *          
 *          Functions for re-arranging RAM are in place.  I haven't tested anything in over 700 lines of code.  I'm just rushing
 *          to get the idea on paper (so to speak).
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OPSYS_GUI_NThompson
{
    public partial class StartForm : Form
    {
        #region Fields and Properties For Hardware and Software
        //string where the file path is stored
        public static string instructionPath;
        
        //declaring important objects/hardware
        public static RAMObject ram;
        public static List<Instruction> instructions;
        public static List<ProcessControlBlock> pcbList;
        public static HardDrive hdd;
        public static Dispatcher dispatch;
        public static CPUObject cpu;

        //For testing purposes only
        public static List<ProcessControlBlock> sortedPCBListAll;
        
        #endregion
        public StartForm()
        {
            InitializeComponent();
        }

        //Function used on the first pass to set up some details
        private void goButton_Click(object sender, EventArgs e)
        {
            //new hard drive, which passes the instuction file path as a parameter
            hdd = new HardDrive(instructionPath);

            //List holding all instructions from hard drive
            instructions = new List<Instruction>(hdd.GetInstructions());

            //List holding all PCB Objects created at the hard drive
            pcbList = new List<ProcessControlBlock>(hdd.GetPCBList());
            
            #region RAM work
            //if ramBox is empty, initialize RAM with size 100
            if (ramBox.Text == "")
            {
                ram = new RAMObject(100);
            }
            else
            {
                //This try/catch block attempts to grab the size of RAM from the form
                //If the size is inappropriate or something other than an integer
                //is grabbed from ramBox, this will generate an Exception and messageBox
                try
                {
                    int ramSize = int.Parse(ramBox.Text);
                    if (ramSize > 1000000)
                    {
                        MessageBox.Show("Size of RAM too large. Please try again.",
                            "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ramAst.Visible = true;
                        return;
                    }
                    else if (ramSize < 10)
                    {
                        MessageBox.Show("Size of RAM too small. Please try again.",
                            "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ramAst.Visible = true;
                        return;
                    }
                    else //RAM size successfully passed size limitations and formatting
                    {
                        ram = new RAMObject(ramSize);
                        ramAst.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ram size formatted incorrectly. Please try again using only integers.", 
                        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            #endregion

            #region LTS work
            //New long term scheduler
            LongTermScheduler lts = new LongTermScheduler();

            
            //Selecting and applying the scheduling algorithm
            if (scheduleCB.SelectedItem == null || scheduleCB.SelectedItem.ToString() == "First Come First Serve")
            {   //First Come First Serve
                scheduleAst.Visible = false;
                lts.FirstComeFirstServe(pcbList);
                sortedPCBListAll = pcbList;
            }
            else if (scheduleCB.SelectedItem.ToString() == "Shortest Job First")
            {   //Shortest Job First
                scheduleAst.Visible = false;
                sortedPCBListAll = lts.ShortestJobFirst(pcbList);
            }
            else if (scheduleCB.SelectedItem.ToString() == "Priority")
            {   //Priority
                scheduleAst.Visible = false;
                sortedPCBListAll = lts.PrioritySort(pcbList);
            }
            else
            {
                MessageBox.Show("Scheduling Algorithm Error. Please try again with a different algorithm.",
                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                scheduleAst.Visible = true;
                return;
            }
            #endregion

            BigLoop();

        }//end "go" button

        public int bigLoopCycles = 0;
        public void BigLoop()
        {
            while (true)
            {
                ram.CompactRAM();
                List<Instruction> nextJobInRAM = ram.GetJobInRAM(sortedPCBListAll[bigLoopCycles]);
                ProcessControlBlock pcb = nextJobInRAM[0].GetPCB(nextJobInRAM[0].GetJobID());
                dispatch = new Dispatcher();
                cpu = new CPUObject();
                dispatch.Dispatch(pcb);
                bigLoopCycles++;
            }
        }
        private void helpButton_Click(object sender, EventArgs e)
        {
            string helpMessage = "If you press the 'Go' button, this program will use default values for processing.\n\n" +
                "The default values are as follows: Number of Processors = 1, Number of Cores per Processor = 1, Size of RAM = 100, Scheduling Algorithm = FCFS. \n\n" +
                "You can import instructions by clicking the 'Import Instructions...' menu item.  The default instruction file location is OPSYS_GUI_NThompson/Debug/bin. \n\n" +
                "If you have any more questions about this project or how it works, contact me at: nathanael.thompson90@gmail.com.";
            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //selecting a new instruction file from the Hard Drive
            HardDrive tempDrive = new HardDrive();
            instructionPath = tempDrive.InstructionSelector();
        }
    }
}
