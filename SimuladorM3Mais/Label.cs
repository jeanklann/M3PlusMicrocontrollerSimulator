namespace M3PlusMicrocontroller
{
    public class Label
    {
        public readonly int Address;
        public readonly string Name;

        public Label(string name, int address = 0)
        {
            Name = name;
            Address = address;
        }
    }
}