using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace M3PlusMicrocontroller {
    public class Simulator {
        public int Frequency = 200000;
        public int NextInstruction;
        public Instruction[] Program;
        public bool Flag_C = false;
        public bool Flag_Z = false;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN4, 1: IN1, 2: IN2, 3: IN3
        public byte[] Out; //0: OUT4, 1: OUT1, 2: OUT2, 3: OUT3
        public byte[] RAM;
        private int count = 0;
        public int Count { get { return count; } }

        private Thread thread;
        public bool Running = false;
        public bool Stopped = true;

        public Simulator() {
            Reset();
        }

        public void Reset() {
            NextInstruction = 0;
            Flag_C = false;
            Flag_Z = false;
            Reg = new byte[5];
            In = new byte[4];
            Out = new byte[4];
            RAM = new byte[255];
            Stopped = true;
            count = 0;
        }
        
        public void Run() {
            if (!Running) {
                if (Stopped) {
                    Reset();
                }
                thread = new Thread(Run_thread);
                Running = true;
                Stopped = false;
                thread.Start();
            }
        }
        private void Run_thread() {
            while (Running) {
                Instruction instruction = Program[NextInstruction];
                if(instruction == null) {
                    Running = false;
                    MessageBox.Show("O simulador tentou executar uma instrução no endereço " + NextInstruction + ", porém neste endereço não há nenhuma instrução. " +
                        "Verifique o seu código, e, caso o programa se comportou corretamente, coloque o seguinte código ao final do programa: \"FIM_DO_PROGRAMA: JMP FIM_DO_PROGRAMA\".", "Erro na simulação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                NextInstruction += instruction.Bytes;
                instruction.Function(this);
                ++count;
                if (instruction.HasBreakpoint) {
                    Running = false;
                    return;
                }
                if (1000 / Frequency != 0) {
                    Thread.Sleep(1000 / Frequency);
                }
            }
            if (Stopped) {
                Reset();
            }
        }

        public void Pause() {
            Running = false;
        }

        public void Debug_StepInto() {
            Instruction instruction = Program[NextInstruction];
            NextInstruction += instruction.Bytes;
            instruction.Function(this);
        }

        public void Debug_StepOut() {
            throw new System.NotImplementedException();
        }
        
        public void AddBreakpoint(uint line) {
            throw new System.NotImplementedException();
        }

        public void Stop() {
            Running = false;
            Stopped = true;
        }
    }
    public class SimulationException:Exception {
        public SimulationException() : base() { }
        public SimulationException(string message) : base(message) { }
    }
}
