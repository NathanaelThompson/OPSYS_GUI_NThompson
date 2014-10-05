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
            instructionGrid.ColumnCount = 6;
            instructionGrid.Columns[0].Name = "Line";
            instructionGrid.Columns[1].Name = "Type";
            instructionGrid.Columns[2].Name = "Register 1";
            instructionGrid.Columns[3].Name = "Register 2";
            instructionGrid.Columns[4].Name = "Value";
            instructionGrid.Columns[5].Name = "Process ID";

            int instructionCount = StartForm.ram.InstructionsInRAMCount();
            List<Instruction> instructionsInRam = StartForm.ram.GetInstructionsInRAM();
            for (int i = 0; i < instructionCount; i++)
            {
                string[] row = new string[] 
                {   
                    instructionsInRam[i].GetInstructionLine().ToString(),
                    instructionsInRam[i].GetInstructionType(),
                    instructionsInRam[i].GetRegister1(),
                    instructionsInRam[i].GetRegister2(),
                    instructionsInRam[i].GetInstructionValue().ToString(),
                    instructionsInRam[i].GetJobID().ToString() 
                };
                instructionGrid.Rows.Add(row);
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
