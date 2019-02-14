namespace M3PlusMicrocontroller
{
        public class Rom : Direction
        {
            public override string Description => $"o valor {Helpers.ToHex(Value)}";
            public override string Instruction => $"{Helpers.ToHex(Value)}";
            public sealed override byte Value { get; set; }

            public Rom(byte value)
            {
                Value = value;
            }
        }
}