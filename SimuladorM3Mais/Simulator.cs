using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace M3PlusMicrocontroller {
    public class Simulator {
        
        public int Frequency = 1;
        public int NextInstruction;
        public Instruction[] Program;
        public bool Flag_C = false;
        public bool Flag_Z = false;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN0, 1: IN1, 2: IN2, 3: IN3
        public byte[] Out; //0: OUT0, 1: OUT1, 2: OUT2, 3: OUT3
        public byte[] RAM;
        private int count = 0;

        public bool Running = false;
        
        public Simulator() {
            Flag_C = false;
            Flag_Z = false;
            Reg = new byte[5];
            In = new byte[4];
            Out = new byte[4];
            RAM = new byte[255];
            count = 0;
        }

        public void Reset() {
            NextInstruction = 0;
            Flag_C = false;
            Flag_Z = false;
            Reg = new byte[5];
            In = new byte[4];
            Out = new byte[4];
            RAM = new byte[255];
            count = 0;
        }

        public void Run_v1() {
            Running = true;
            Thread thread = new Thread(Run_v1_thread);
            Console.ReadLine();
            thread.Start();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do {
                Instruction instruction = Program[NextInstruction];
                ++count;
                
                Console.Write("PC: ");
                Console.Write(NextInstruction);
                Console.Write("\tInstrução: ");
                Console.WriteLine(instruction);
                

                NextInstruction += instruction.Bytes;
                instruction.Function(this);
                Thread.Sleep(16);
            } while (Running);
        }
        private void Run_v1_thread() {
            Console.ReadLine();
            In[0] = 255;
            Console.WriteLine("Alterado");
            Console.ReadLine();
            Running = false;
        }

        public void Run() {
            throw new System.NotImplementedException();
        }

        public void Pause() {
            throw new System.NotImplementedException();
        }

        public void Debug_StepInto() {
            throw new System.NotImplementedException();
        }

        public void Debug_StepOut() {
            throw new System.NotImplementedException();
        }

        public void Debug_Continue() {
            throw new System.NotImplementedException();
        }

        public void Debug_Pause() {
            throw new System.NotImplementedException();
        }

        public void AddBreakpoint(uint line) {
            throw new System.NotImplementedException();
        }

        public void Stop() {
            throw new System.NotImplementedException();
        }
    }
}
