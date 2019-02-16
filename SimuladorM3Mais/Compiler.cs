using System.Collections.Generic;

namespace M3PlusMicrocontroller
{
    public class Compiler
    {
        public const int MemoryMaxSize = 65536;
        private int _nextAddress;
        private bool _nextTokenHasBreakpoint;

        private TokenAnalyzer _tokenAnalyzer;

        public List<InstructionCompiler> Instructions;
        public List<Label> Labels;


        private Token NeedSeparator(string program)
        {
            var token = _tokenAnalyzer.NextToken();
            if (token.Type != TokenType.Separator)
                throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                        ". Falta o separador ','.");
            token = _tokenAnalyzer.NextToken();
            return token;
        }

        private static void ThrowInvalidLabel(Token token, string program)
        {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) + ". Label inválido.");
        }

        private void NewInstruction(Instruction instruction)
        {
            instruction.HasBreakpoint = _nextTokenHasBreakpoint;
            _nextTokenHasBreakpoint = false;
            Instructions.Add(new InstructionCompiler(instruction, _nextAddress));
            if (instruction.To is Address label)
                foreach (var item in Labels)
                {
                    if (label.Label != item.Name) continue;
                    label.ValueAddress = item.Address;
                    label.Value = 1;
                }

            _nextAddress += instruction.Size;
        }

        private void NewLabel(string name, Token token, string program)
        {
            foreach (var item in Labels)
                if (name == item.Name)
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                            ". O label já existe.");
            foreach (var item in Instructions)
            {
                if (!(item.Instruction.To is Address labelInstruction)) continue;
                if (labelInstruction.Label != name) continue;
                labelInstruction.ValueAddress = _nextAddress;
                labelInstruction.Value = 1;
            }

            var label = new Label(name, _nextAddress);
            Labels.Add(label);
        }

        public Instruction[] Compile(string program, bool[] breakpoints)
        {
            Instructions = new List<InstructionCompiler>();
            Labels = new List<Label>();
            _tokenAnalyzer = new TokenAnalyzer();

            _tokenAnalyzer.Analyze(program);
            Token token;
            do
            {
                token = _tokenAnalyzer.NextToken();
                var lineToken = Helpers.CountLines(program, token.Index) - 1;
                if (breakpoints[lineToken]) _nextTokenHasBreakpoint = true;
                if (token.Type == TokenType.CpuInstruction)
                {
                    token = OperationInstructions(program, token);
                    token = JmpInstructions(program, token);
                    token = PushInstructions(token);
                    token = CallInstructions(token);
                }
                else if (token.Type == TokenType.Identificator)
                {
                    token = Identificator(program, token);
                }
                else if (token.Type == TokenType.EoF)
                {
                    break;
                }
                else
                {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                            ". Deve-se iniciar com uma instrução ou com um label.");
                }
            } while (token.Type != TokenType.EoF);

            var instructionCompiled = new Instruction[MemoryMaxSize];
            foreach (var item in Instructions)
            {
                if (item.Instruction.To is Address address && address.Value == 0)
                    throw new CompilerError("Erro na instrução " + item.Instruction.Text +
                                            ". Não foi encontrado o label " + item.Instruction.Label + ".");
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
                    function = Functions.Call;
                    NewInstruction(new Instruction("CALL", function, DirectionFactory.Create(token),
                        $"Chama o procedimento no label {token.Value}."));
                    break;
                case "RET":
                    token = _tokenAnalyzer.NextToken();
                    function = Functions.Ret;
                    NewInstruction(new Instruction("RET", function, "Retorno do procedimento."));
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
                    function = Functions.Pusha;
                    instructionDescription = "Coloca o registrador acumulador na pilha.";
                    break;
                case "POPA":
                    function = Functions.Popa;
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

            if (function == null) return token;
            var instruction = token.Value;
            switch (instruction)
            {
                case "PUSH":
                case "POP":
                    token = _tokenAnalyzer.NextToken();
                    var to = DirectionFactory.Create(token);
                    NewInstruction(new Instruction(instruction, function, to, instructionDescription));
                    break;
                case "PUSHA":
                case "POPA":
                    NewInstruction(new Instruction(instruction, function, new Register(0), instructionDescription));
                    break;
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
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(program, token.Index) +
                                            ". Um label não pode ser o último código do programa. Caso deseje encessar o programa aqui insira isso na última linha: \"JMP " +
                                            labelToken.Value + "\"");
                NewLabel(labelToken.Value, labelToken, program);
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
            switch (instructionText)
            {
                case "JMP":
                case "JMPC":
                case "JMPZ":
                    token = _tokenAnalyzer.NextToken();
                    if (token.Type == TokenType.Identificator)
                    {
                        var to = DirectionFactory.Create(token);
                        switch (instructionText)
                        {
                            case "JMP":
                                NewInstruction(new Instruction(instructionText, Functions.Jmp,
                                    to, $"Pula para o label {token.Value}") {Label = token.Value});
                                break;
                            case "JMPC":
                                NewInstruction(new Instruction(instructionText, Functions.Jmpc,
                                        to, $"Pula para o label {token.Value} caso a flag carry esteja ligada.")
                                    {Label = token.Value});
                                break;
                            case "JMPZ":
                                NewInstruction(new Instruction(instructionText, Functions.Jmpz,
                                        to, $"Pula para o label {token.Value} caso a flag zero esteja ligada.")
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