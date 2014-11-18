using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPSYS_GUI_NThompson
{
    public partial class DisplayForm : Form
    {
        public DisplayForm()
        {
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            InstructionGridCreator();   
        }
        public void InstructionGridCreator()
        {
            DataGridView grid = new DataGridView();
            instructionGrid.ColumnCount = 9;
            instructionGrid.Columns[0].Name = "Job Name";
            instructionGrid.Columns[1].Name = "Job Length";
            instructionGrid.Columns[2].Name = "Job Priority";
            instructionGrid.Columns[3].Name = "Register 1";
            instructionGrid.Columns[4].Name = "Register 2";
            instructionGrid.Columns[5].Name = "Register 3";
            instructionGrid.Columns[6].Name = "Register 4";
            instructionGrid.Columns[7].Name = "Accumulator";
            instructionGrid.Columns[8].Name = "Total CPU Cycles";

            List<ProcessControlBlock> finishedJobs = StartForm.finishedJobs;
            for (int i = 0; i < finishedJobs.Count-1; i++)
            {
                string[] row = new string[] 
                {   
                    finishedJobs[i].GetPCBJobName(),
                    finishedJobs[i].GetPCBJobLength().ToString(),
                    finishedJobs[i].GetPCBJobPriority().ToString(),
                    finishedJobs[i].programState.register1.ToString(),
                    finishedJobs[i].programState.register2.ToString(),
                    finishedJobs[i].programState.register3.ToString(),
                    finishedJobs[i].programState.register4.ToString(),
                    finishedJobs[i].programState.accumulator.ToString(),
                    finishedJobs[i].totalCycles.ToString()
                };
                
                grid = instructionGrid;
                grid.Rows.Add(row);
            }
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
