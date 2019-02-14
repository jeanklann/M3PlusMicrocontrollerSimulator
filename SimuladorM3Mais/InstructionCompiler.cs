namespace M3PlusMicrocontroller
{
    public class InstructionCompiler {
        public Instruction Instruction;
        public int Address;
        public InstructionCompiler(Instruction instruction, int address = 0) {
            Instruction = instruction;
            Address = address;
        }
    }
}