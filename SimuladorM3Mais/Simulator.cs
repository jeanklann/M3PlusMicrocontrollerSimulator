using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusSimulator
{
    public class Simulator {
        public int Frequency;
        public int NextInstruction;
        public Command[] Program;
        public bool Flag_C;
        public bool Flag_Z;


        public void Run() {
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
