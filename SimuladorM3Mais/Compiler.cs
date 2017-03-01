using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public class Compiler {
        public const int MEMORY_MAX_SIZE = 65536;

        public List<InstructionCompiler> Instructions;
        public List<Label> Labels;
        

        private TokenAnalyzer tokenAnalyzer;
        private int NextAddress = 0;

        

        private Token NeedSeparator(TokenAnalyzer analyzer, string Program) {
            Token token = tokenAnalyzer.NextToken();
            if (token.Type != TokenType.Separator)
                throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Falta o separador ','.");
            token = tokenAnalyzer.NextToken();
            return token;
        }
        private void ThrowInvalidOperation(Token token, string Program) {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Operação inválida.");
        }
        private void ThrowInvalidLabel(Token token, string Program) {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Label inválido.");
        }
        private void ThrowAwaitedRegistrerAddress(Token token, string Program) {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Operação inválida.");
        }

        private void NewInstruction(Instruction instruction) {
            Instructions.Add(new InstructionCompiler(instruction, NextAddress));
            if(instruction.Label != null) {
                foreach (Label item in Labels) {
                    if(instruction.Label == item.Name) {
                        instruction.Address = item.Address;
                        instruction.Label = null;
                    }
                }
            }
            NextAddress += instruction.Bytes;
        }
        private void NewLabel(string name) {
            foreach (InstructionCompiler item in Instructions) {
                if (item.Instruction.Label != null) {
                    if (item.Instruction.Label == name) {
                        item.Instruction.Address = NextAddress;
                        item.Instruction.Label = null;
                    }
                }
            }

            Label label = new Label(name, NextAddress);
            Labels.Add(label);
        }

        public Instruction[] Compile(string Program) {
            Instructions = new List<InstructionCompiler>();
            Labels = new List<Label>();
            tokenAnalyzer = new TokenAnalyzer();

            tokenAnalyzer.Analyze(Program);
            Token token = null;
            do {
                token = tokenAnalyzer.NextToken();
                if(token.Type == TokenType.CPUInstruction) {
                    switch (token.Value) {
                        case "ADD": //////////////////////////////////////// ADD
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.ADD_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.ADD_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.ADD_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.ADD_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.ADD_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.ADD_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.ADD_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.ADD_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.ADD_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.ADD_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.ADD_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.ADD_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.ADD_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.ADD_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.ADD_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.ADD_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.ADD_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.ADD_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.ADD_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.ADD_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.ADD_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if(token.Type == TokenType.Registrer) {
                                    if(token.Value == "A") {
                                        if(value == "IN4") {
                                            NewInstruction(Instruction.ADD_IN4_A());
                                        } else if(value == "IN1") {
                                            NewInstruction(Instruction.ADD_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.ADD_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.ADD_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "SUB": //////////////////////////////////////// SUB
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.SUB_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.SUB_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.SUB_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.SUB_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.SUB_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.SUB_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.SUB_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.SUB_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.SUB_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.SUB_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.SUB_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.SUB_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.SUB_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.SUB_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.SUB_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.SUB_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.SUB_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.SUB_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.SUB_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.SUB_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.SUB_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.SUB_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.SUB_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.SUB_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.SUB_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "AND": //////////////////////////////////////// AND
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.AND_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.AND_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.AND_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.AND_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.AND_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.AND_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.AND_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.AND_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.AND_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.AND_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.AND_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.AND_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.AND_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.AND_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.AND_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.AND_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.AND_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.AND_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.AND_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.AND_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.AND_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.AND_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.AND_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.AND_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.AND_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "OR": //////////////////////////////////////// OR
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.OR_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.OR_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.OR_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.OR_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.OR_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.OR_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.OR_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.OR_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.OR_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.OR_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.OR_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.OR_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.OR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.OR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.OR_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.OR_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.OR_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.OR_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.OR_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.OR_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.OR_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.OR_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.OR_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.OR_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.OR_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "XOR": //////////////////////////////////////// XOR
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.XOR_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.XOR_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.XOR_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.XOR_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.XOR_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.XOR_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.XOR_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.XOR_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.XOR_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.XOR_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.XOR_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.XOR_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.XOR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.XOR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.XOR_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.XOR_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.XOR_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.XOR_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.XOR_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.XOR_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.XOR_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.XOR_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.XOR_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.XOR_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.XOR_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "NOT": //////////////////////////////////////// NOT
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.NOT_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.NOT_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.NOT_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.NOT_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.NOT_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.NOT_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.NOT_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.NOT_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.NOT_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.NOT_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.NOT_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.NOT_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.NOT_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.NOT_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.NOT_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.NOT_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.NOT_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.NOT_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.NOT_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.NOT_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.NOT_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.NOT_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.NOT_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.NOT_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.NOT_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "MOV": //////////////////////////////////////// MOV
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.MOV_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.MOV_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.MOV_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.MOV_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.MOV_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.MOV_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.MOV_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.MOV_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.MOV_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.MOV_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.MOV_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.MOV_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.MOV_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.MOV_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.MOV_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.MOV_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.MOV_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.MOV_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.MOV_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.MOV_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.MOV_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.MOV_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.MOV_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.MOV_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.MOV_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "INC": //////////////////////////////////////// INC
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Registrer) {
                                if (token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.INC_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(Instruction.INC_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(Instruction.INC_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(Instruction.INC_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(Instruction.INC_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(Instruction.INC_A_RAM(byte.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT4") {
                                            NewInstruction(Instruction.INC_A_OUT4());
                                        } else if (token.Value == "OUT1") {
                                            NewInstruction(Instruction.INC_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(Instruction.INC_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(Instruction.INC_A_OUT3());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.INC_B_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "C") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.INC_C_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "D") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.INC_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Value == "E") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(Instruction.INC_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                byte address = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.INC_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                byte value = byte.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(Instruction.INC_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(Instruction.INC_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(Instruction.INC_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(Instruction.INC_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(Instruction.INC_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    byte address = byte.Parse(token.Value);
                                    NewInstruction(Instruction.INC_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Input) {
                                string value = token.Value;
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        if (value == "IN4") {
                                            NewInstruction(Instruction.INC_IN4_A());
                                        } else if (value == "IN1") {
                                            NewInstruction(Instruction.INC_IN1_A());
                                        } else if (value == "IN2") {
                                            NewInstruction(Instruction.INC_IN2_A());
                                        } else if (value == "IN3") {
                                            NewInstruction(Instruction.INC_IN3_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "JMP": //////////////////////////////////////// JMP
                            token = tokenAnalyzer.NextToken();
                            if(token.Type == TokenType.Identificator) {
                                string name = token.Value;
                                NewInstruction(Instruction.JMP(name));
                            } else {
                                ThrowInvalidLabel(token, Program);
                            }
                            break;
                        case "JMPC": //////////////////////////////////////// JMPC
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Identificator) {
                                string name = token.Value;
                                NewInstruction(Instruction.JMPC(name));
                            } else {
                                ThrowInvalidLabel(token, Program);
                            }
                            break;
                        case "JMPZ": //////////////////////////////////////// JMPZ
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Identificator) {
                                string name = token.Value;
                                NewInstruction(Instruction.JMPZ(name));
                            } else {
                                ThrowInvalidLabel(token, Program);
                            }
                            break;
                        case "CALL": //////////////////////////////////////// CALL
                            token = tokenAnalyzer.NextToken();
                            if (token.Type == TokenType.Identificator) {
                                string name = token.Value;
                                NewInstruction(Instruction.CALL(name));
                            } else {
                                ThrowInvalidLabel(token, Program);
                            }
                            break;
                        case "RET": //////////////////////////////////////// RET
                            NewInstruction(Instruction.RET());
                            break;
                        default:
                            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Instrução " + token.Value + " não implementada.");


                    }
                } else if(token.Type == TokenType.Identificator) {
                    string text = token.Value;
                    token = tokenAnalyzer.NextToken();
                    if(token.Type == TokenType.IdentificatorSeparator) {
                        NewLabel(text);
                    } else {
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Falta o dois pontos (:) após o label.");
                    }
                } else if (token.Type == TokenType.EoF) {
                    break;
                } else {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Deve-se iniciar com uma instrução ou com um label.");
                }
            } while (token.Type != TokenType.EoF);
            Instruction[] instructionCompiled = new Instruction[MEMORY_MAX_SIZE];
            foreach (InstructionCompiler item in Instructions) {
                instructionCompiled[item.Address] = item.Instruction;
            }
            return instructionCompiled;
        }
    }

    public class Label {
        public string Name = "";
        public int Address = 0;

        public Label(string name, int address = 0) {
            Name = name;
            Address = address;
        }
    }

    public class InstructionCompiler {
        public Instruction Instruction;
        public int Address = 0;
        public InstructionCompiler(Instruction instruction, int address = 0) {
            Instruction = instruction;
            Address = address;
        }
    }

    public class Token {

        public static readonly string[] INPUTS = { "IN4", "IN1", "IN2", "IN3" };
        public static readonly string[] OUTPUTS = { "OUT4", "OUT1", "OUT2", "OUT3" };
        public static readonly string[] REGISTRERS = { "A", "B", "C", "D", "E" };
        public static readonly string[] CPUINSTRUCTIONS = { "ADD", "SUB", "AND", "OR", "XOR", "NOT", "MOV", "INC", "JMP", "JMPC", "JMPZ", "CALL", "RET" };
        public static readonly char[] LINESEPARATOR = { '\n', '\r' };
        public static readonly char INSTRUCTIONSEPARATOR = ',';
        public static readonly char[] SPACE = { ' ', '\t' };
        public static readonly char IDENTIFICATORSEPARATOR = ':';
        public static readonly char COMMENT = ';';
        public static readonly char RAM = '#';
        
        public TokenType Type = TokenType.Error;
        public string Value = "";
        public int Index;

        public Token(TokenType Type = TokenType.Error, string Value = "", int Index = 0) {
            this.Type = Type;
            this.Value = Value;
            this.Index = Index;
        }


    }
    public class TokenAnalyzer {
        public List<Token> Tokens = new List<Token>();
        private string Program;
        private int Index = 0;
        private int NextTokenindex = 0;
        public void Analyze(string Program) {
            Token token = null;
            this.Program = Program.ToUpper();
            do {
                token = NextToken_internal();
                Tokens.Add(token);
            } while (token.Type != TokenType.EoF);
        }
        public Token NextToken() {
            if(Tokens.Count == 0) {
                throw new Exception("Not analyzed. Need to call function Analyze() first.");
            }
            Token token = Tokens[NextTokenindex];
            ++NextTokenindex;
            if (NextTokenindex >= Tokens.Count) {
                NextTokenindex = Tokens.Count - 1;
            }
            return token;
        }
        public Token PeekNextToken() {
            if (Tokens.Count == 0) {
                throw new Exception("Not analyzed. Need to call function Analyze() first.");
            }
            return Tokens[NextTokenindex];
        }
        private Token NextToken_internal() {
            int BeginIndex = Index;
            string Value = "";
            bool Analyzed = false;
            do {
                if(Program.Length <= Index) {
                    break;
                }
                Value += Program[Index];
                ++Index;

                if (Value == "") break;

                foreach (char item in Token.SPACE) {  //It's only space
                    if (item+"" == Value) {
                        Value = "";
                        break;
                    }
                }
                foreach (char item in Token.LINESEPARATOR) {  //It's only a blank line
                    if (item+"" == Value) {
                        Value = "";
                        break;
                    }
                }
                if (Token.COMMENT+"" == Value) {    //Comment
                    Value = "";
                    bool c = true;
                    do {
                        foreach (char item in Token.LINESEPARATOR) {  //End of comment
                            if (item == Program[Index]) {
                                c = false;
                                break;
                            }
                        }
                        ++Index;
                        if (Program.Length <= Index) {
                            c = false;
                            break;
                        }
                    } while (c);
                    Value = "";
                    BeginIndex = Index;
                    continue;
                }
                if (Value == "") {
                    BeginIndex = Index;
                    continue;
                }
                if (Value == Token.INSTRUCTIONSEPARATOR+"") { //If it's an instruction separator (,)
                    Analyzed = true;
                    break;
                }
                if (Value == Token.IDENTIFICATORSEPARATOR+"") {    //If it's an identificator separator (:)
                    Analyzed = true;
                    break;
                }

                foreach (char item in Token.LINESEPARATOR) {  //End of the line at the final of the string
                    if (item == Value[Value.Length-1]) {
                        Value = Value.Substring(0,Value.Length-1);
                        Analyzed = true;
                        break;
                    }
                }
                foreach (char item in Token.SPACE) {  //Space character at the final of the string
                    if (item == Value[Value.Length - 1]) {
                        Value = Value.Substring(0, Value.Length - 1);
                        Analyzed = true;
                        break;
                    }
                }
                if (Token.IDENTIFICATORSEPARATOR == Value[Value.Length - 1]) {    //If it's an identificator
                    Value = Value.Substring(0, Value.Length - 1);
                    --Index;
                    Analyzed = true;
                    break;
                }
                if (Token.INSTRUCTIONSEPARATOR == Value[Value.Length - 1]) {    //Separator at the final of the string
                    Value = Value.Substring(0, Value.Length - 1);
                    --Index;
                    Analyzed = true;
                    break;
                }
                if (Token.COMMENT == Value[Value.Length - 1]) {    //Comment at the final of the string
                    Value = Value.Substring(0, Value.Length - 1);
                    --Index;
                    Analyzed = true;
                    break;
                }

            } while (!Analyzed);


            // Verifies what type of token is
            if (Value == "") {
                return new Token(TokenType.EoF, "", BeginIndex);
            }

            foreach (string item in Token.REGISTRERS) {
                if (item == Value) {
                    return new Token(TokenType.Registrer, Value, BeginIndex);
                }
            }
            foreach (string item in Token.INPUTS) {
                if (item == Value) {
                    return new Token(TokenType.Input, Value, BeginIndex);
                }
            }
            foreach (string item in Token.CPUINSTRUCTIONS) {
                if (item == Value) {
                    return new Token(TokenType.CPUInstruction, Value, BeginIndex);
                }
            }
            foreach (string item in Token.OUTPUTS) {
                if (item == Value) {
                    return new Token(TokenType.Output, Value, BeginIndex);
                }
            }

            if(Value == Token.IDENTIFICATORSEPARATOR+"") {
                return new Token(TokenType.IdentificatorSeparator, Value, BeginIndex);
            }
            if (Value == Token.INSTRUCTIONSEPARATOR + "") {
                return new Token(TokenType.Separator, Value, BeginIndex);
            }


            int number = -1;
            if(int.TryParse(Value, out number)) {
                if(number < 0 || number > 255) {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, BeginIndex) + ". Valor inválido, os números para este processador devem estar entre 0 e 255.");
                }
                return new Token(TokenType.Number, number.ToString(), BeginIndex);
            }
            
            if (Value[0] == Token.RAM) {
                if (int.TryParse(Value.Substring(1,Value.Length-1), out number)) {
                    if (number < 0 || number > 255) {
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, BeginIndex) + ". Valor inválido, os números para este processador devem estar entre 0 e 255.");
                    }
                    return new Token(TokenType.RamAddress, number.ToString(), BeginIndex);
                } else {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, BeginIndex) + ". Valor de endereçamento inválido.");
                }
            }

            for (int i = 0; i < Value.Length; i++) {
                if(i == 0) {
                    if (!(char.IsLetter(Value[i]) || Value[i] == '_')) {
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, BeginIndex) + ". Os labels devem começar com uma letra ou com underline (_).");
                    }
                }
                if(!(char.IsLetterOrDigit(Value[i]) || Value[i] == '_')) {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, BeginIndex) + ". Os labels devem ter somente letras, números e undelines (_).");
                }
            }
            return new Token(TokenType.Identificator, Value, BeginIndex);
        }

    }
    public enum TokenType {
        Error, Separator, RamAddress, Number, Input, Output, Registrer, CPUInstruction, Identificator, IdentificatorSeparator, RomAddress, EoF
    }

    public class CompilerError : Exception {
        public CompilerError(string msg) : base(msg) { }
    }


}
