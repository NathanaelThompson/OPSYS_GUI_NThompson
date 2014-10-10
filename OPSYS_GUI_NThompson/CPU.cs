using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPSYS_GUI_NThompson
{
    public class CPU
    {
        int register1, register2, register3, register4, accumulator, programCounter;

        public CPU()
        {
            register1 = 1;
            register2 = 3;
            register3 = 5;
            register4 = 7;
            accumulator = 9;
            programCounter = 0;
        }
        public CPU(int reg1, int reg2, int reg3, int reg4, int acc, int pc)
        {
            register1 = reg1;
            register2 = reg2;
            register3 = reg3;
            register4 = reg4;
            accumulator = acc;
            programCounter = pc;
        }

        public void SetRegister1(int value)
        {
            register1 = value;
        }
        public void SetRegister2(int value)
        {
            register2 = value;
        }
        public void SetRegister3(int value)
        {
            register3 = value;
        }
        public void SetRegister4(int value)
        {
            register4 = value;
        }
        public void SetAccumulator(int value)
        {
            accumulator = value;
        }
        public int GetRegister1()
        {
            return register1;
        }
        public int GetRegister2()
        {
            return register2;
        }
        public int GetRegister3()
        {
            return register3;
        }
        public int GetRegister4()
        {
            return register4;
        }
        public int GetAccumulator()
        {
            return accumulator;
        }
    }
}
