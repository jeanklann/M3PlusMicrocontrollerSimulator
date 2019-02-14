namespace M3PlusMicrocontroller
{
    public abstract class Direction
    {
        public static Simulator Simulador { get; set; }
        public abstract byte Value { get; set; }
        public abstract string Description { get; }
        public abstract string Instruction { get; }
    }
}