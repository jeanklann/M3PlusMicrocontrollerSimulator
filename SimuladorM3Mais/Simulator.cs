using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace M3PlusMicrocontroller {
    public class Simulator {
        public int Frequency = 1;
        public bool FrequencyLimit = true;
        public int NextInstruction;
        public Instruction[] Program;
        public byte[] CompiledProgram;
        public bool FlagC;
        public bool FlagZ;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN4, 1: IN1, 2: IN2, 3: IN3
        public byte[] Out; //0: OUT4, 1: OUT1, 2: OUT2, 3: OUT3
        public byte[] Ram;
        public byte[] Stack;
        private byte _pointerStack;
        public byte PointerStack{
            get => _pointerStack;
            set {
                if (_pointerStack == 1 && value == 0) {
                    MessageBox.Show("Ocorreu um estouro na memória de pilha (stack overflow).\r\nVerifique o seu código.", "Stack overflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Stop();
                    throw new SimulationException();
                }

                if(_pointerStack == 0 && value == 1) {
                    MessageBox.Show("Foi tentado retirar um valor da pilha mas não havia nenhum valor (stack underflow).\r\nVerifique o seu código.", "Stack underflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Stop();
                    throw new SimulationException();
                }
                _pointerStack = value;
            }
        }
        public int LowFrequencyIteraction;
        
        private int _instructionsCountFrequency;
        private int _instructionsCount;
        private bool _frequencyReaded;
        public int CurrentFrequency { get; private set; }
        private bool _internalSimulation;
        public bool InternalSimulation { get => _internalSimulation;
            set {
                if (value) {
                    if (Frequency > 20) {
                        Frequency = 20;
                    }
                }
                _internalSimulation = value;
            } }
        private Thread _thread;
        public bool Running;
        public bool Stopped = true;

        private Instruction _stepOutInstruction;
        private Instruction _lastBreakpointInstruction;
        public Simulator() {
            Reset();
            Direction.Simulador = this;
        }

        public void Reset() {
            NextInstruction = 0;
            FlagC = false;
            FlagZ = false;
            Reg = new byte[5];
            In = new byte[4];
            Out = new byte[4];
            Ram = new byte[256];
            Stack = new byte[256];
            _pointerStack = 0;
            Stopped = true;
            _instructionsCountFrequency = 0;
            _stepOutInstruction = null;
            _lastBreakpointInstruction = null;
        }
        
        public void Run() {
            if (!Running) {
                _thread = new Thread(Run_thread);
                if (Stopped) {
                    Reset();
                }
                Running = true;
                Stopped = false;
                _thread.Start();

                Thread currentFrequency = new Thread(Read_frequency);
                currentFrequency.Start();
            }
        }

        private void Read_frequency() {
            while (Running) {
                Thread.Sleep(1000);
                if (!Running) return;
                CurrentFrequency = _instructionsCount;
                _frequencyReaded = true;
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
                    Stopped = true;
                    break;
                }
                if (instruction.HasBreakpoint) {
                    if(instruction == _stepOutInstruction) {
                        _stepOutInstruction.HasBreakpoint = false;
                    }
                    if (_lastBreakpointInstruction != instruction) {
                        _lastBreakpointInstruction = instruction;
                        Running = false;
                        return;
                    } else {
                        _lastBreakpointInstruction = null;
                    }
                }
                if (!FrequencyLimit || !_internalSimulation) {
                    NextInstruction += instruction.Size;
                    try {
                        instruction.Execute(this);
                    } catch (SimulationException) {
                        Running = false;
                        Stopped = true;
                        break;
                    }
                } else {
                    ++LowFrequencyIteraction;
                }
                ++_instructionsCountFrequency;
                ++_instructionsCount;
                if (_frequencyReaded) {
                    _frequencyReaded = false;
                    _instructionsCount = 0;
                }
                if (FrequencyLimit) {
                    if (Frequency > 100) {
                        if (_instructionsCountFrequency > Frequency / 20) {
                            Sleep(50, stopwatch);
                            _instructionsCountFrequency = 0;
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
            if (_stepOutInstruction != null && _stepOutInstruction.HasBreakpoint) {
                _stepOutInstruction.HasBreakpoint = false;
                _stepOutInstruction = null;
            }
            Running = false;
        }

        public void Debug_StepInto() {
            Stopped = false;
            if (!Running) {
                if (!_internalSimulation) {
                    Instruction instruction = Program[NextInstruction];
                    NextInstruction += instruction.Size;
                    instruction.Execute(this);
                } else {
                    ++LowFrequencyIteraction;
                }
            }
        }

        public void Debug_StepOut() {
            Stopped = false;
            if (!Running) {
                Instruction instruction = Program[NextInstruction];
                NextInstruction += instruction.Size;
                Instruction newInstruction = Program[NextInstruction];
                instruction.Execute(this);
                if (newInstruction != null) {
                    newInstruction.HasBreakpoint = true;
                    Run();
                    _stepOutInstruction = newInstruction;
                }
            }
        }

        public void Stop() {
            if (_stepOutInstruction != null && _stepOutInstruction.HasBreakpoint) {
                _stepOutInstruction.HasBreakpoint = false;
                _stepOutInstruction = null;
            }
            Running = false;
            Stopped = true;
        }
    }
    public class SimulationException:Exception {
    }
}
