using CircuitSimulator.Components.Digital.MMaisMaisMais;

namespace IDE
{
    public struct InternalComponents
    {
        public ControlModule ControlModule;
        public RomMemory RomMemory;
        public Ula Ula;
        public Registrers Registrers;
        public Registrer8BitCBuffer Accumulator;
        public RamMemory RamMemory;
        public RamMemory StackMemory;
        public Counter8Bit StackCounter;
        public PortBank PortBank;
        public RomAddresser RomAddresser;
        public Microcontroller Microcontroller;
    }
}