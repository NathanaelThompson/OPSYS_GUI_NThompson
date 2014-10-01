using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OPSYS_GUI_NThompson
{
    class HardDrive
    {
        string rawData;
        public HardDrive()
        {
            BootstrapLoader();   
        }

        //everytime a hard drive object is created, this function is called
        //to sanitize the input and put it into the instructions array
        public void BootstrapLoader()
        {
            char[] delimiters = {' ', ',', '.',':','\t','\r','\n'};
            rawData = File.ReadAllText("ugradPart1.txt");
            string[] sanitizedInput = rawData.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
            
            //Reads in the instructions from the input form
            for (int i = 0; i < sanitizedInput.Length; i++)
            {
                if (sanitizedInput[i] == "Job")
                {
                    Program.instructionsAndJobHeaders[i].SetJobName(String.Concat(sanitizedInput[i] + " " + sanitizedInput[i + 1]));
                    Program.instructionsAndJobHeaders[i].SetJobLength(int.Parse(sanitizedInput[i + 2]));
                    Program.instructionsAndJobHeaders[i].SetJobPriority(int.Parse(sanitizedInput[i + 3]));
                    Program.instructionsAndJobHeaders[i + 1].SetInstructionLine(int.Parse(sanitizedInput[i + 4]));
                    Program.instructionsAndJobHeaders[i + 1].SetInstructionType(sanitizedInput[i + 5]);
                    Program.instructionsAndJobHeaders[i + 1].SetRegister1(sanitizedInput[i + 6]);
                    Program.instructionsAndJobHeaders[i + 1].SetRegister2(sanitizedInput[i + 7]);
                    Program.instructionsAndJobHeaders[i + 1].SetInstructionValue(int.Parse(sanitizedInput[i + 8]));
                }
                else
                {
                    i += 6;
                }
            }

        }
    }
}
