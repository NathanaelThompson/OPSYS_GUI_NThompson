using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace OPSYS_GUI_NThompson
{
    public class HardDrive
    {
        //this creates a static List with a capacity of 5000 instructions
        //and 500 PCBs
        //there may be more instructions than that in the final project,
        //but this is a quick and dirty workaround until I have a better
        //idea of what's going on
        public static List<Instruction> instructionList = new List<Instruction>(5000);
        public static List<ProcessControlBlock> pcbList = new List<ProcessControlBlock>(500);
        string rawData;
        string path;

        public HardDrive()
        {

        }
        public HardDrive(string path)
        {
            this.path = path;
            BootStrapper(path);   
        }

        //everytime a hard drive object is created 
        //(which should only be once per application startup) this function 
        //is called to sanitize the input and put it into the instructions List, unsorted
        public void BootStrapper(string instructionPath)
        {
            //places to split the incoming text file
            char[] delimiters = {' ', ',', '.',':','\t','\r','\n'};
            
            //trys to read all data from instruction text file
            try
            {
                if (instructionPath != "")
                {
                    rawData = File.ReadAllText(instructionPath);
                }
                else
                {
                    rawData = File.ReadAllText("ugradPart1.txt");
                }
            }
            catch (Exception ex)
            {
                //if this fails, the user is prompted to select a file
                MessageBox.Show("File error, please select a file.",
                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HardDrive tempDrive = new HardDrive();
                rawData = File.ReadAllText(tempDrive.InstructionSelector());
            }

            string[] sanitizedInput = rawData.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
            
            //Reads in the instructions from the input form
            try
            {
                //if this loop comes across an array element that 
                //is simply a string with a value of "Job"
                //this decisicion block assigns the various
                //Job data to a list item, 
                //otherwise, it fills in the instructions
                int tempID = 0;
                for (int i = 0; i < sanitizedInput.Length; i++)
                {
                    if (sanitizedInput[i].Equals("Job"))
                    {
                        ProcessControlBlock tempPCB = new ProcessControlBlock();
                        tempPCB.SetPCBJobName(String.Concat(sanitizedInput[i] + " " + sanitizedInput[i + 1]));
                        tempPCB.SetPCBJobLength(int.Parse(sanitizedInput[i + 2]));
                        tempPCB.SetPCBJobPriority(int.Parse(sanitizedInput[i + 3]));
                        tempPCB.SetPCBID(i);
                        tempID = i;
                        pcbList.Add(tempPCB);
                        i += 3;
                    }
                    else
                    {
                        Instruction tempInstruction = new Instruction();
                        tempInstruction.SetInstructionLine(int.Parse(sanitizedInput[i]));
                        tempInstruction.SetInstructionType(sanitizedInput[i + 1]);
                        tempInstruction.SetRegister1(sanitizedInput[i + 2]);
                        tempInstruction.SetRegister2(sanitizedInput[i + 3]);
                        tempInstruction.SetInstructionValue(int.Parse(sanitizedInput[i + 4]));
                        tempInstruction.SetJobID(tempID);
                        instructionList.Add(tempInstruction);
                        i += 4;
                    }
                }
            }
            catch (Exception ex)
            {
                //If there are more than 5000 instructions, this catch will grab it
                //and display the error to the user
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Gets the list of instructions
        public List<Instruction> GetInstructions()
        {
            return instructionList;
        }

        //Gets the list of PCB objects
        public List<ProcessControlBlock> GetPCBList()
        {
            return pcbList;
        }
        public string InstructionSelector()
        {
            //This function grabs the selected file from an OpenFileDialog
            //If no file is selected, filePath is left at the default, "ugradPart1.txt"
            //This function remains largely untested due to a lack of instruction files
            //and my unwillingness to create new instructions.
            string filePath;
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Text Files|*.txt";
            opf.Title = "Please select an instruction file...";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                filePath = opf.FileName;

            }
            else
            {
                filePath = "ugradPart1.txt";
            }
            return filePath;
        }
    }
}
