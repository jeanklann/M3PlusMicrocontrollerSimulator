namespace M3PlusMicrocontroller {
    public delegate int Function(Simulator simulator, Direction from, Direction to, Instruction instruction);
    
    public class Instruction {
        public string Text { get; }
        public readonly string Description;
        private readonly Function _function;
        private readonly Direction _from;
        public readonly Direction To;
        public void Execute(Simulator sim)
        {
            var res = _function(sim, _from, To, this);
            To.Value = (byte) res;
            sim.FlagC = res > byte.MaxValue;
            sim.FlagZ = To.Value == 0;
        }
        public int Size = 1;
        public bool HasBreakpoint = false;

        public int Address = 0; //JMP somewhere -> JMP #42
        public string Label { get; set; }

        private static readonly string[] descricao = {
            "{0}o registrador A com {1} e envia para {2}",
            "{0}{1} e envia para {2}"
        };
        
        public Instruction(string text, Function function, Direction from, Direction to, string description, int descriptionStyle = 0) {
            Text = $"{text} {from.Instruction}, {to.Instruction}";
            _function = function;
            _from = from;
            To = to;
            Description = string.Format(descricao[descriptionStyle], description, _from.Description, To.Description);
            ValidaFluxosPadroes();
        }
        public Instruction(string text, Function function, Direction to, string description)
        {
            Description = description;
            Text = $"{text} {to.Instruction}";
            _function = function;
            To = to;
        }
        
        public Instruction(string text, Function function, string description)
        {
            Description = description;
            Text = $"{text}";
            _function = function;
        }

        private void ValidaFluxosPadroes()
        {
            if (ValidaFluxoAcumulador<Register, Register>(true)) return; //A*A → A ; A*reg → A
            if (ValidaFluxoAcumulador<Register, Register>(false)) return; //A*A → A ; A*A → reg
            if (ValidaFluxoAcumulador<Register, Ram>(false)) return; //A*A → RAM
            if (ValidaFluxoAcumulador<Register, Output>(false)) return; //A*A → OUT
            if (ValidaFluxoAcumulador<Ram, Register>(true)) return; //A*RAM → A
            if (ValidaFluxoAcumulador<Input, Register>(true)) return; //A*IN → A
            if (ValidaFluxo<Rom, Register>()) return; //A*ROM → A ; A*ROM → reg
            if (ValidaFluxo<Rom, Ram>()) return; //A*ROM → RAM
            if (ValidaFluxoAcumulador<AddressRam, Register>(true)) return; //A*DRAM → A
            if (ValidaFluxoAcumulador<Register, AddressRam>(false)) return; //A*A → DRAM
            throw new CompilerError(FluxoInvalido);
        }

        private const string FluxoInvalido = "Fluxo de controle inválido";
        private bool ValidaFluxo<TE, TD>(bool comparacaoExtra = true) 
            where TE : Direction
            where TD : Direction
        {
            if (!comparacaoExtra)
                return false;
            if (_from.GetType() == typeof(TE) && To.GetType() == typeof(TD))
                return true;
            return false;
        }
        private bool ValidaFluxoAcumulador<TE, TD>(bool direita) 
            where TE : Direction
            where TD : Direction
        {
            if (direita)
            {
                if ((To as Register)?.WitchOne != 0)
                    return false;
            }
            else
            {
                if ((_from as Register)?.WitchOne != 0)
                    return false;
            }
            if (_from.GetType() == typeof(TE) && To.GetType() == typeof(TD))
                return true;
            return false;
        }

        public byte[] Convert()
        {
            return new byte[] {0};
        }
    }
}
