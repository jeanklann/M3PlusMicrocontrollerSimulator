using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace M3PlusMicrocontroller {
    public class Simulator {
        public int Frequency = 1;
        public bool FrequencyLimit = true;
        public int NextInstruction;
        public Instruction[] Program;
        public bool Flag_C = false;
        public bool Flag_Z = false;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN4, 1: IN1, 2: IN2, 3: IN3
        public byte[] Out; //0: OUT4, 1: OUT1, 2: OUT2, 3: OUT3
        public byte[] RAM;
        public byte[] Stack;
        public byte PointerStack;
        public int LowFrequencyIteraction = 0;

        private int instructionsCountFrequency = 0;
        private int instructionsCount = 0;
        private int currentFrequency = 0;
        private bool frequencyReaded = false;
        public int CurrentFrequency { get { return currentFrequency; } }

        private Thread thread;
        public bool Running = false;
        public bool Stopped = true;

        private Instruction stepOutInstruction;
        private Instruction lastBreakpointInstruction;
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
            RAM = new byte[256];
            Stack = new byte[256];
            PointerStack = 0;
            Stopped = true;
            instructionsCountFrequency = 0;
            stepOutInstruction = null;
            lastBreakpointInstruction = null;
        }
        
        public void Run() {
            if (!Running) {
                thread = new Thread(Run_thread);
                Running = true;
                Stopped = false;
                thread.Start();

                Thread currentFrequency = new Thread(Read_frequency);
                currentFrequency.Start();
            }
        }

        private void Read_frequency() {
            while (Running) {
                Thread.Sleep(1000);
                if (!Running) return;
                currentFrequency = instructionsCount;
                frequencyReaded = true;
            }

        }
        private void Run_thread() {
            Stopwatch stopwatch = new Stopwatch();
            while (Running) {
                Instruction instruction = Program[NextInstruction];
                if(instruction == null) {
                    Running = false;
                    MessageBox.Show("O simulador tentou executar uma instrução no endereço " + NextInstruction + ", porém neste endereço não há nenhuma instrução. " +
                        "Verifique o seu código, e, caso o programa se comportou corretamente, coloque o seguinte código ao final do programa: \"FIM_DO_PROGRAMA: JMP FIM_DO_PROGRAMA\".", "Erro na simulação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (instruction.HasBreakpoint) {
                    if(instruction == stepOutInstruction) {
                        stepOutInstruction.HasBreakpoint = false;
                    }
                    if (lastBreakpointInstruction != instruction) {
                        lastBreakpointInstruction = instruction;
                        Running = false;
                        return;
                    } else {
                        lastBreakpointInstruction = null;
                    }
                }
                if (!FrequencyLimit || Frequency > 10) {
                    NextInstruction += instruction.Size;
                    instruction.Function(this);
                } else {
                    ++LowFrequencyIteraction;
                }
                ++instructionsCountFrequency;
                ++instructionsCount;
                if (frequencyReaded) {
                    frequencyReaded = false;
                    instructionsCount = 0;
                }
                if (FrequencyLimit) {
                    if (Frequency > 100) {
                        if (instructionsCountFrequency > Frequency / 20) {
                            Sleep(50, stopwatch);
                            instructionsCountFrequency = 0;
                        }
                    } else {
                        Thread.Sleep(1000 / Frequency);
                        //Sleep(1000/Frequency, stopwatch);
                    }
                }

                
            }
            if (Stopped) {
                Reset();
            }
        }
        private void Sleep(int miliseconds, Stopwatch stopwatch) {
            if (!stopwatch.IsRunning) stopwatch.Start();
            while((stopwatch.ElapsedTicks * 1000) / Stopwatch.Frequency < miliseconds) {
                //Thread.Sleep()
                //Thread 100%
            }
            stopwatch.Reset();
            stopwatch.Start();
        }
        public void Pause() {
            if (stepOutInstruction != null && stepOutInstruction.HasBreakpoint) {
                stepOutInstruction.HasBreakpoint = false;
                stepOutInstruction = null;
            }
            Running = false;
        }

        public void Debug_StepInto() {
            if (!Running) {
                Instruction instruction = Program[NextInstruction];
                NextInstruction += instruction.Size;
                instruction.Function(this);
            }
        }

        public void Debug_StepOut() {
            if (!Running) {
                Instruction instruction = Program[NextInstruction];
                NextInstruction += instruction.Size;
                Instruction newInstruction = Program[NextInstruction];
                instruction.Function(this);
                if (newInstruction != null) {
                    newInstruction.HasBreakpoint = true;
                    Run();
                    stepOutInstruction = newInstruction;
                }
            }
        }

        public void Stop() {
            if (stepOutInstruction != null && stepOutInstruction.HasBreakpoint) {
                stepOutInstruction.HasBreakpoint = false;
                stepOutInstruction = null;
            }
            Running = false;
            Stopped = true;
        }
    }
    public class SimulationException:Exception {
        public SimulationException() : base() { }
        public SimulationException(string message) : base(message) { }
    }
}
