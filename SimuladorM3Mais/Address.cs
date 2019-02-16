namespace M3PlusMicrocontroller
{
    public class Address : Direction
    {
        public Address(string label)
        {
            Label = label;
        }

        public override byte Value { get; set; }
        public int ValueAddress { get; set; }
        public string Label { get; }
        public override string Description => $"o endereço onde está o label '{Label}'";
        public override string Instruction => Label;
    }
}