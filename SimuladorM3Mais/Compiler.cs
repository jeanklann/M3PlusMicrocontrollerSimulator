using System.Collections.Generic;

namespace M3PlusMicrocontroller {
    public class Compiler {
        public const int MemoryMaxSize = 65536;

        public List<InstructionCompiler> Instructions;
        public List<Label> Labels;

        private TokenAnalyzer _tokenAnalyzer;
        private int _nextAddress;
        private bool _nextTokenHasBreakpoint;
        

        private Token NeedSeparator(string program) {
            var token = _tokenAnalyzer.NextToken();
            if (token.Type != TokenType.Separator)
                throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) + ". Falta o separador ','.");
            token = _tokenAnalyzer.NextToken();
            return token;
        }
        private static void ThrowInvalidLabel(Token token, string program) {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) + ". Label inválido.");
        }

        private void NewInstruction(Instruction instruction) {
            instruction.HasBreakpoint = _nextTokenHasBreakpoint;
            _nextTokenHasBreakpoint = false;
            Instructions.Add(new InstructionCompiler(instruction, _nextAddress));
            if(instruction.Label != null) {
                foreach (Label item in Labels) {
                    if(instruction.Label == item.Name) {
                        instruction.Address = item.Address;
                        instruction.Label = null;
                    }
                }
            }
            _nextAddress += instruction.Size;
        }
        private void NewLabel(string name, Token token, string program) {
            foreach (var item in Labels) {
                if(name == item.Name) {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) + ". O label já existe.");
                }
            }
            foreach (InstructionCompiler item in Instructions) {
                if (item.Instruction.Label != null) {
                    if (item.Instruction.Label == name) {
                        item.Instruction.Address = _nextAddress;
                        item.Instruction.Label = null;
                    }
                }
            }

            Label label = new Label(name, _nextAddress);
            Labels.Add(label);
        }

        public Instruction[] Compile(string program, bool[] breakpoints) {
            Instructions = new List<InstructionCompiler>();
            Labels = new List<Label>();
            _tokenAnalyzer = new TokenAnalyzer();

            _tokenAnalyzer.Analyze(program);
            Token token;
            do {
                token = _tokenAnalyzer.NextToken();
                int lineToken = Helpers.CountLines(program, token.Index)-1;
                if (breakpoints[lineToken]) {
                    _nextTokenHasBreakpoint = true;
                }
                if (token.Type == TokenType.CpuInstruction)
                {
                    token = OperationInstructions(program, token);
                    token = JmpInstructions(program, token);
                    token = PushInstructions(token);
                    token = CallInstructions(token);
                    
                } else if(token.Type == TokenType.Identificator)
                {
                    token = Identificator(program, token);
                } else if (token.Type == TokenType.EoF) {
                    break;
                } else {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) + ". Deve-se iniciar com uma instrução ou com um label.");
                }
            } while (token.Type != TokenType.EoF);
            Instruction[] instructionCompiled = new Instruction[MemoryMaxSize];
            foreach (InstructionCompiler item in Instructions) {
                if(item.Instruction.Label != null) throw new CompilerError("Erro na instrução "+item.Instruction.Text+". Não foi encontrado o label "+ item.Instruction .Label+ ".");
                instructionCompiled[item.Address] = item.Instruction;
            }
            return instructionCompiled;
        }

        private Token CallInstructions(Token token)
        {
            Function function;
            switch (token.Value)
            {
                case "CALL":
                    token = _tokenAnalyzer.NextToken();
                    function = (simulator, from, to, i) =>
                    {
                        var next = simulator.NextInstruction;

                        --simulator.PointerStack;
                        simulator.Stack[simulator.PointerStack] = (byte)(next / 256);
                        --simulator.PointerStack;
                        simulator.Stack[simulator.PointerStack] = (byte)(next % 256);

                        --simulator.PointerStack;
                        simulator.Stack[simulator.PointerStack] = (byte)(i.Address / 256);
                        --simulator.PointerStack;
                        simulator.Stack[simulator.PointerStack] = (byte)(i.Address % 256);

                        simulator.NextInstruction = i.Address;

                        ++simulator.PointerStack;
                        ++simulator.PointerStack;
                        
                        return 0;
                    };
                    NewInstruction(new Instruction("CALL", function, $"Chama o procedimento no label {token.Value}.")
                        {Label = token.Value});
                    break;
                case "RET":
                    token = _tokenAnalyzer.NextToken();
                    function = (simulator, from, to, i) =>
                    {
                        int next;

                        next = simulator.Stack[simulator.PointerStack] % 256;
                        ++simulator.PointerStack;
                        next += simulator.Stack[simulator.PointerStack] * 256;
                        ++simulator.PointerStack;

                        simulator.NextInstruction = next;
                        
                        return 0;
                    };
                    NewInstruction(new Instruction("RET", function, "Retorno do procedimento.")
                        {Label = token.Value});
                    break;
            }
            return token;
        }

        private Token PushInstructions(Token token)
        {
            Function function = null;
            var instructionDescription = string.Empty;
            switch (token.Value)
            {
                case "PUSHA":
                    function = Functions.Push;
                    instructionDescription = "Coloca o registrador acumulador na pilha.";
                    break;
                case "POPA":
                    function = Functions.Pop;
                    instructionDescription = "Tira um valor da pilha e coloca no acumulador.";
                    break;
                case "PUSH":
                    function = Functions.Push;
                    instructionDescription = "Coloca o registrador na pilha.";
                    break;
                case "POP":
                    function = Functions.Pop;
                    instructionDescription = "Tira um valor da pilha e coloca no registrador.";
                    break;
            }

            if (function != null)
            {
                Direction to = null;
                var instruction = token.Value;
                token = _tokenAnalyzer.NextToken();
                switch (instruction)
                {
                    case "PUSHA":
                    case "POPA":
                        to = DirectionFactory.Create(token);
                        break;
                }

                NewInstruction(to != null
                    ? new Instruction(instruction, function, to, instructionDescription)
                    : new Instruction(instruction, function, instructionDescription));
            }
            return token;
        }

        private Token Identificator(string program, Token token)
        {
            var labelToken = token;
            token = _tokenAnalyzer.NextToken();
            if (token.Type == TokenType.IdentificatorSeparator)
            {
                if (_tokenAnalyzer.PeekNextToken().Type == TokenType.EoF)
                {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                            ". Um label não pode ser o último código do programa. Caso deseje encessar o programa aqui insira isso na última linha: \"JMP " +
                                            labelToken.Value + "\"");
                }
                else
                {
                    NewLabel(labelToken.Value, labelToken, program);
                }
            }
            else
            {
                throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                        ". Falta o dois pontos (:) após o label.");
            }

            return token;
        }

        private Token JmpInstructions(string program, Token token)
        {
            var instructionText = token.Value;
            switch (token.Value)
            {
                case "JMP":
                case "JMPC":
                case "JMPZ":
                    var name = token.Value;
                    token = _tokenAnalyzer.NextToken();
                    if (token.Type == TokenType.Identificator)
                    {
                        switch (name)
                        {
                            case "JMP":
                                NewInstruction(new Instruction(instructionText, Functions.Jmp,
                                    $"Pula para o label {token.Value}") {Label = token.Value});
                                break;
                            case "JMPC":
                                NewInstruction(new Instruction(instructionText, Functions.Jmpc,
                                        $"Pula para o label {token.Value} caso a flag carry esteja ligada.")
                                    {Label = token.Value});
                                break;
                            case "JMPZ":
                                NewInstruction(new Instruction(instructionText, Functions.Jmpz,
                                        $"Pula para o label {token.Value} caso a flag zero esteja ligada.")
                                    {Label = token.Value});
                                break;
                        }
                    }
                    else
                    {
                        ThrowInvalidLabel(token, program);
                    }

                    break;
            }

            return token;
        }

        private Token OperationInstructions(string program, Token token)
        {
            Function function = null;
            var instructionDescription = string.Empty;
            var descriptionStyle = 0;
            switch (token.Value)
            {
                case "ADD":
                    function = Functions.Add;
                    instructionDescription = "Adiciona ";
                    break;
                case "SUB":
                    function = Functions.Sub;
                    instructionDescription = "Subtrai ";
                    break;
                case "MOV":
                    function = Functions.Mov;
                    instructionDescription = "Copia ";
                    descriptionStyle = 1;
                    break;
                case "INC":
                    function = Functions.Inc;
                    instructionDescription = "Incrementa ";
                    break;
                case "AND":
                    function = Functions.And;
                    instructionDescription = "Faz a operação E n";
                    descriptionStyle = 1;
                    break;
                case "OR":
                    function = Functions.Or;
                    instructionDescription = "Faz a operação OU n";
                    descriptionStyle = 1;
                    break;
                case "XOR":
                    function = Functions.Xor;
                    instructionDescription = "Faz a operação XOU n";
                    descriptionStyle = 1;
                    break;
                case "NOT":
                    function = Functions.Not;
                    instructionDescription = "Faz a operação NÃO n";
                    descriptionStyle = 1;
                    break;
            }

            if (function == null) return token;
            var instructionText = token.Value;
            token = _tokenAnalyzer.NextToken();
            var directionFrom = DirectionFactory.Create(token);
            token = NeedSeparator(program);
            var directionTo = DirectionFactory.Create(token);
            try
            {
                NewInstruction(new Instruction(instructionText, function, directionFrom, directionTo,
                    instructionDescription, descriptionStyle));
            }
            catch (CompilerError e)
            {
                throw new CompilerError($"Erro na linha {Helpers.CountLines(program, token.Index)}. {e.Message}");
            }

            return token;
        }
    }
}
