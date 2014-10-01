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
 *          Move the instructions to the long term scheduler. (NOT STARTED)
 *          Something about process controls blocks. (NOT STARTED)
 *          Sort instructions based on three algorithms: (NOT STARTED)
 *              1.First come first serve
 *              2.Priority
 *              3.
 *          Move each sorted instruction to RAM. (NOT STARTED)
 *          Display information about instructions to user. (NOT STARTED)
 * 
 * Phase 2: Moves jobs from RAM to ready-queue via the short term scheduler, and whole bunch of other stuff.
 * Phase 3:
 *
 */

 /*
  * Notes
  * --------
  * 8/26/14: Simulated operating system.  Note that as of right now we are not manipulating the physical hardware. Quite frankly, I'm glad.
  *          I may feel comfortable in C#, but there is no reason for me to try to manipulate the hardware until I actually know how the abstract
  *          pieces interact.
  *          
  * 8/28/14: Orignially this was going to be a console application.  However, multiple times in class Prof. Franklin stated that he preferred ease-of-use
  *          and style over bare-bones functionality.  As such, I intend to make this program run with one button click.
  *          Adding notes and a to-do list might seem like overkill when it comes to documentation, but I'd rather have too much than not enough.  Who knows,
  *          maybe someone will read this one day.  
  *          
  *          There is some potential for inheritance/polymorphism between the items which are hardware and can hold a certain amount of items.
  *          For example, the hard drive and RAM are both containers where little to no processing will take place.  There is also overlap
  *          in the long term scheduler, the short term scheduler, and a small amount in the bootstrap.  I could just be making extra work for myself,
  *          but it might clear up some things in the long run.  Hard to say for sure.  As of this day, it is not likely I will implement much inheritance 
  *          or polymorphism.
  *          
  *          On this day I created, but not completed, most classes required for Phase 1.  Though I'm sure more will unfold as I progress.  
  *          
  * 9/17/14  I've neglected this project too long.  I've been way too busy with Applications Programming.  
  *          It is literally what I spend 90% of my time on these days, the other 10% being relaxing from working so much on AppProg.
  *          With that in mind, I have much work to do in this program.  None of it is code I haven't written before, but there
  *          will be a LARGE amount of inter-class dependence; which brings its own headaches.
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
        public StartForm()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            HardDrive hdd = new HardDrive();
            LongTermScheduler lts = new LongTermScheduler();
            
            RAMObject ram;
            
            if (ramBox.Text == "")
            {
                ram = new RAMObject(100);
            }
            else
            {
                try
                {
                    int ramSize = int.Parse(ramBox.Text);
                    if (ramSize > 1000000)
                    {
                        MessageBox.Show("Size of RAM too large",
                            "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        ram = new RAMObject(ramSize);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ram size formatted incorrectly. Please try again using only integers.", 
                        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto EndOfBigLoop;
                }
            }

            if(scheduleCB.Text == "" || scheduleCB.Text == "First Come First Serve")
            {
                lts.FirstComeFirstServe(Program.instructionsAndJobHeaders);
                //add sorted instructions to pcbs
                //move pcb objects to RAM if they fit
                //display a timer
            }

            if (scheduleCB.Text == "Shortest Job First")
            {
                lts.ShortestJobFirst(Program.instructionsAndJobHeaders);
                //add sorted instructions to pcb
            }

            if (scheduleCB.Text == "Priority")
            {
                lts.PrioritySort(Program.instructionsAndJobHeaders);
                //add sorted instructions to pcb
            }

        EndOfBigLoop:
            ;
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            string helpMessage = "If you press the 'Go' button, this program will use default values for processing. " +
                "The default values are as follows: Number of Processors = 1, Number of Cores per Processor = 1, Size of RAM = 100, Scheduling Algorithm = FCFS. " +
                "If you have any more questions about this project or how it works, contact me at: nathanael.thompson90@gmail.com.";
            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
