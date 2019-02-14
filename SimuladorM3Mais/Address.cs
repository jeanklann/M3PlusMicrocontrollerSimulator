namespace M3PlusMicrocontroller
{
    public class Address : Direction
    {
        public override byte Value { get; set; }
        public int ValueAddress { get; set; }
        public string Label { get;  }
        public override string Description => $"o endereço onde está o label '{Label}'";
        public override string Instruction => Label;

        public Address(string label)
        {
            Label = label;
        }
    }
}