using System;

namespace IDE
{
    [Serializable]
    public enum ComponentType {
        None, Input, Output, Disable, Not, And, Nand, Or, Nor, Xor, Xnor, Keyboard, Display7Seg, BinTo7Seg, Circuit,
        Microcontroller, Osciloscope, BlackTerminal, JkFlipFlop, RsFlipFlop, DFlipFlop, FlipFlop,
        HalfAdder, FullAdder, Ula, ControlModule, Registrer8Bit, Registrers, Tristate, Stack, RamMemory,
        RomMemory, PortBank, Registrer8BitSg, LedMatrix, Disable8Bit, Counter8Bit, RomAddresser, Registrer8BitCBuffer, Clock
    }
}