namespace M3PlusMicrocontroller
{
    public class InstructionCompiler
    {
        public int Address;
        public Instruction Instruction;

        public InstructionCompiler(Instruction instruction, int address = 0)
        {
            Instruction = instruction;
            Address = address;
        }
    }
}