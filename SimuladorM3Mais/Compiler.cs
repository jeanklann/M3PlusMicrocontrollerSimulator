using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public class Compiler {
        private TokenAnalyzer tokenAnalyzer;

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
        private void ThrowAwaitedRegistrerAddress(Token token, string Program) {
            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Operação inválida.");
        }

        private void NewInstruction(List<Instruction> instructions, Instruction instruction) {
            throw new NotImplementedException();
        }
        public Instruction[] Compile(string Program) {
            List<Instruction> instructions = new List<Instruction>();
            tokenAnalyzer = new TokenAnalyzer();
            tokenAnalyzer.Analyze(Program);
            Token token = null;
            do {
                token = tokenAnalyzer.NextToken();
                if(token.Type == TokenType.CPUInstruction) {
                    switch (token.Value) {
                        case "ADD": //////////////////////////////////////// ADD
                            token = tokenAnalyzer.NextToken();
                            if(token.Type == TokenType.Registrer) {
                                if(token.Value == "A") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if(token.Value == "A") {
                                            NewInstruction(instructions, Instruction.ADD_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.ADD_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.ADD_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.ADD_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.ADD_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.ADD_A_RAM(int.Parse(token.Value)));
                                    } else if(token.Type == TokenType.Output) {
                                        if(token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.ADD_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.ADD_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.ADD_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.ADD_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if(token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.ADD_B_A());
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
                                            NewInstruction(instructions, Instruction.ADD_C_A());
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
                                            NewInstruction(instructions, Instruction.ADD_D_A());
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
                                            NewInstruction(instructions, Instruction.ADD_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if(token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if(token.Type == TokenType.Registrer) {
                                    if(token.Value == "A") {
                                        NewInstruction(instructions, Instruction.ADD_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if(token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if(token.Type == TokenType.Registrer) {
                                    if(token.Value == "A") {
                                        NewInstruction(instructions, Instruction.ADD_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.ADD_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.ADD_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.ADD_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.ADD_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if(token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.ADD_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.SUB_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.SUB_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.SUB_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.SUB_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.SUB_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.SUB_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.SUB_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.SUB_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.SUB_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.SUB_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.SUB_B_A());
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
                                            NewInstruction(instructions, Instruction.SUB_C_A());
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
                                            NewInstruction(instructions, Instruction.SUB_D_A());
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
                                            NewInstruction(instructions, Instruction.SUB_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.SUB_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.SUB_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.SUB_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.SUB_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.SUB_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.SUB_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.SUB_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.AND_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.AND_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.AND_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.AND_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.AND_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.AND_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.AND_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.AND_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.AND_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.AND_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.AND_B_A());
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
                                            NewInstruction(instructions, Instruction.AND_C_A());
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
                                            NewInstruction(instructions, Instruction.AND_D_A());
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
                                            NewInstruction(instructions, Instruction.AND_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.AND_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.AND_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.AND_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.AND_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.AND_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.AND_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.AND_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.OR_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.OR_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.OR_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.OR_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.OR_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.OR_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.OR_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.OR_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.OR_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.OR_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.OR_B_A());
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
                                            NewInstruction(instructions, Instruction.OR_C_A());
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
                                            NewInstruction(instructions, Instruction.OR_D_A());
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
                                            NewInstruction(instructions, Instruction.OR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.OR_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.OR_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.OR_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.OR_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.OR_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.OR_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.OR_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.XOR_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.XOR_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.XOR_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.XOR_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.XOR_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.XOR_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.XOR_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.XOR_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.XOR_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.XOR_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.XOR_B_A());
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
                                            NewInstruction(instructions, Instruction.XOR_C_A());
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
                                            NewInstruction(instructions, Instruction.XOR_D_A());
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
                                            NewInstruction(instructions, Instruction.XOR_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.XOR_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.XOR_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.XOR_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.XOR_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.XOR_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.XOR_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.XOR_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.NOT_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.NOT_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.NOT_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.NOT_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.NOT_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.NOT_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.NOT_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.NOT_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.NOT_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.NOT_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.NOT_B_A());
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
                                            NewInstruction(instructions, Instruction.NOT_C_A());
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
                                            NewInstruction(instructions, Instruction.NOT_D_A());
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
                                            NewInstruction(instructions, Instruction.NOT_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.NOT_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.NOT_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.NOT_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.NOT_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.NOT_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.NOT_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.NOT_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.MOV_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.MOV_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.MOV_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.MOV_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.MOV_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.MOV_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.MOV_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.MOV_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.MOV_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.MOV_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.MOV_B_A());
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
                                            NewInstruction(instructions, Instruction.MOV_C_A());
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
                                            NewInstruction(instructions, Instruction.MOV_D_A());
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
                                            NewInstruction(instructions, Instruction.MOV_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.MOV_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.MOV_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.MOV_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.MOV_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.MOV_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.MOV_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.MOV_ROM_RAM(value, address));
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
                                            NewInstruction(instructions, Instruction.INC_A_A());
                                        } else if (token.Value == "B") {
                                            NewInstruction(instructions, Instruction.INC_A_B());
                                        } else if (token.Value == "C") {
                                            NewInstruction(instructions, Instruction.INC_A_C());
                                        } else if (token.Value == "D") {
                                            NewInstruction(instructions, Instruction.INC_A_D());
                                        } else if (token.Value == "E") {
                                            NewInstruction(instructions, Instruction.INC_A_E());
                                        }
                                    } else if (token.Type == TokenType.RamAddress) {
                                        NewInstruction(instructions, Instruction.INC_A_RAM(int.Parse(token.Value)));
                                    } else if (token.Type == TokenType.Output) {
                                        if (token.Value == "OUT1") {
                                            NewInstruction(instructions, Instruction.INC_A_OUT1());
                                        } else if (token.Value == "OUT2") {
                                            NewInstruction(instructions, Instruction.INC_A_OUT2());
                                        } else if (token.Value == "OUT3") {
                                            NewInstruction(instructions, Instruction.INC_A_OUT3());
                                        } else if (token.Value == "OUT4") {
                                            NewInstruction(instructions, Instruction.INC_A_OUT4());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    }
                                } else if (token.Value == "B") {
                                    token = NeedSeparator(tokenAnalyzer, Program);
                                    if (token.Type == TokenType.Registrer) {
                                        if (token.Value == "A") {
                                            NewInstruction(instructions, Instruction.INC_B_A());
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
                                            NewInstruction(instructions, Instruction.INC_C_A());
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
                                            NewInstruction(instructions, Instruction.INC_D_A());
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
                                            NewInstruction(instructions, Instruction.INC_D_A());
                                        } else {
                                            ThrowInvalidOperation(token, Program);
                                        }
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                }
                            } else if (token.Type == TokenType.RamAddress) {
                                int address = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.INC_RAM_A(address));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else if (token.Type == TokenType.Number) {
                                int value = int.Parse(token.Value);
                                token = NeedSeparator(tokenAnalyzer, Program);
                                if (token.Type == TokenType.Registrer) {
                                    if (token.Value == "A") {
                                        NewInstruction(instructions, Instruction.INC_ROM_A(value));
                                    } else if (token.Value == "B") {
                                        NewInstruction(instructions, Instruction.INC_ROM_B(value));
                                    } else if (token.Value == "C") {
                                        NewInstruction(instructions, Instruction.INC_ROM_C(value));
                                    } else if (token.Value == "D") {
                                        NewInstruction(instructions, Instruction.INC_ROM_D(value));
                                    } else if (token.Value == "E") {
                                        NewInstruction(instructions, Instruction.INC_ROM_E(value));
                                    } else {
                                        ThrowInvalidOperation(token, Program);
                                    }
                                } else if (token.Type == TokenType.RamAddress) {
                                    int address = int.Parse(token.Value);
                                    NewInstruction(instructions, Instruction.INC_ROM_RAM(value, address));
                                } else {
                                    ThrowInvalidOperation(token, Program);
                                }
                            } else {
                                ThrowInvalidOperation(token, Program);
                            }
                            break;
                        case "JMP":
                            break;
                        case "JMPC":
                            break;
                        case "JMPZ":
                            break;
                        case "CALL":
                            break;
                        case "RET":
                            break;
                        default:
                            throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Instrução " + token.Value + " não implementada.");


                    }
                } else if(token.Type == TokenType.Identificator) {
                    string text = token.Value;
                    token = tokenAnalyzer.NextToken();
                    if(token.Type == TokenType.IdentificatorSeparator) {
                        text += ":";
                        ////////////////// LABEL
                    } else {
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Falta : após o label.");
                    }
                } else {
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(Program, token.Index) + ". Deve-se iniciar com uma instrução ou com um label.");
                }
            } while (token.Type == TokenType.EoF);
            return null;
        }
    }

    public class Token {

        public static readonly string[] INPUTS = { "IN1", "IN2", "IN3", "IN4" };
        public static readonly string[] OUTPUTS = { "OUT1", "OUT2", "OUT3", "OUT4" };
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
                return new Token(TokenType.Number, number.ToString(), BeginIndex);
            }
            
            if (Value[0] == Token.RAM) {
                if (int.TryParse(Value.Substring(1,Value.Length-1), out number)) {
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
