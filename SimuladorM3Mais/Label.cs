namespace M3PlusMicrocontroller
{
    public class Label {
        public readonly string Name = "";
        public readonly int Address;

        public Label(string name, int address = 0) {
            Name = name;
            Address = address;
        }
    }
}