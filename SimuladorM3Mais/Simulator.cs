using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public class Simulator {
        public int Frequency;
        public int NextInstruction;
        public Instruction[] Program;
        public bool Flag_C;
        public bool Flag_Z;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN1, 1: IN2, 2: IN3, 3: IN4
        public byte[] Out; //0: OUT1, 1: OUT2, 2: OUT3, 3: OUT4



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
