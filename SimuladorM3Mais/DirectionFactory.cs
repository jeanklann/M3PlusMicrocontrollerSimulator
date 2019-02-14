namespace M3PlusMicrocontroller
{
    public static class DirectionFactory
    {
        public static Direction Create(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Registrer:
                    return new Register(token.Value[0]);
                case TokenType.Input:
                    return new Input(token.Value);
                case TokenType.Output:
                    return new Output(token.Value);
                case TokenType.Dram:
                    return new AddressRam(new Register(token.Value[0]));
                case TokenType.Number:
                    return new Rom(byte.Parse(token.Value));
                case TokenType.RamAddress:
                    return new Ram(byte.Parse(token.Value));
                default:
                    throw new CompilerError($"O tipo {token.Type.ToString()} não pode ser identificado.");
            }
        }
    }
}