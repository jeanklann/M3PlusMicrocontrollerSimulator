namespace M3PlusMicrocontroller
{
    public abstract class Direction
    {
        public static Simulator Simulador { get; set; }
        public abstract byte Value { get; set; }
        public abstract string Description { get; }
        public abstract string Instruction { get; }
    }

    public class Address : Direction
    {
        public override byte Value { get; set; }
        public string Label { get; set; }
        public override string Description { get; }
        public override string Instruction { get; }
    }
}