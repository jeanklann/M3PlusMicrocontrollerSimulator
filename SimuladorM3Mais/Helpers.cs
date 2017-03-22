using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public static class Helpers {
        public delegate void CommandFunction(Simulator simulator);

        public static Instruction[] GenerateFunctions(int[] program) {
            Instruction[] commands = new Instruction[program.Length];
            
            int i = 0;
            while(i < program.Length) {
                /*
                ULAOperation = program[i];
                Registrer = program[i];
                Controller = program[i];

                ULAOperation = 0xE0 & ULAOperation;
                ULAOperation = ULAOperation >> 5;

                Registrer = 0x18 & Registrer;
                Registrer = Registrer >> 3;

                Controller = 0x07 & Controller;
                
                if(!IsHighDecoder) {
                    IsHighDecoder = Controller == 7;
                    if(IsHighDecoder) {
                        i++;
                        continue;
                    }
                }
                */
                CreateCommand(program, i, commands);
                //IsHighDecoder = false;
                ++i;
            }

            

            return commands;
        }
        private static int CreateCommand(int[] Program, int PosRead, Instruction[] Commands) {
            int NextPos = PosRead;

            int ULAOperation = Program[NextPos];
            int Registrer = Program[NextPos];
            int Controller = Program[NextPos];
            Controller = 0x07 & Controller;
            
            bool IsHighDecoder = false;
            if(Controller == 7) {
                NextPos++;
                IsHighDecoder = true;
                Controller = Program[NextPos];
                Controller = 0x07 & Controller;

                ULAOperation = Program[NextPos];
                Registrer = Program[NextPos];
            }

            ULAOperation = 0xE0 & ULAOperation;
            ULAOperation = ULAOperation >> 5;
            Registrer = 0x18 & Registrer;
            Registrer = Registrer >> 3;

            if(IsHighDecoder) {
                switch(Controller) {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    default:
                        throw new Exception("Impossível");
                }
            } else {
                switch(Controller) {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    default:
                        throw new Exception("Impossível");
                }
            }

            return NextPos;
        }
        private static string PrintOperation(int ULAOperation) {
            switch(ULAOperation) {
                case 0:
                    return "ADD ";
                case 1:
                    return "SUB ";
                case 2:
                    return "AND ";
                case 3:
                    return "OR ";
                case 4:
                    return "XOR ";
                case 5:
                    return "NOT ";
                case 6:
                    return "MOV ";
                case 7:
                    return "INC ";
                default:
                    throw new Exception("Erro interno na operação");
            }
        }
        private static string PrintRegister(int Registrer) {
            switch(Registrer) {
                case 0:
                    return "B";
                case 1:
                    return "C";
                case 2:
                    return "D";
                case 3:
                    return "E";
                default:
                    throw new Exception("Erro interno no registrador");
            }
        }
        private static string PrintInput(int Registrer) {
            switch(Registrer) {
                case 0:
                    return "IN1";
                case 1:
                    return "IN2";
                case 2:
                    return "IN3";
                case 3:
                    return "IN4";
                default:
                    throw new Exception("Erro interno no input");
            }
        }
        private static string PrintOutput(int Registrer) {
            switch(Registrer) {
                case 0:
                    return "OUT1";
                case 1:
                    return "OUT2";
                case 2:
                    return "OUT3";
                case 3:
                    return "OUT4";
                default:
                    throw new Exception("Erro interno no input");
            }
        }
        private static string PrintController(int Controller, bool IsHighDecoder, int[] program, ref int i, int Registrer, int ULAOperation) {
            int data;
            string res = "";
            res += i + ": ";
            if(IsHighDecoder) {
                switch(Controller) {
                    case 0:
                        res += PrintOperation(ULAOperation);
                        res += program[++i] + ", A";
                        break;
                    case 1:
                        res += PrintOperation(ULAOperation);
                        res += program[++i] + ", "+PrintRegister(Registrer);
                        break;
                    case 2:
                        res += PrintOperation(ULAOperation);
                        res += program[++i]+", #" + "AC * dado -> RAM ";
                        break;
                    case 3:
                        data = program[++i];
                        data = data << 8;
                        data += program[++i];
                        res += "JMP #" + data;
                        break;
                    case 4:
                        data = program[++i];
                        data = data << 8;
                        data += program[++i];
                        res += "JMPC " +data;
                        break;
                    case 5:
                        
                        data = program[++i];
                        data = data << 8;
                        data += program[++i];
                        res += "JMPZ  " +data;
                        break;
                    case 6:
                        data = program[++i];
                        data = data << 8;
                        ++i;
                        data += program[i];
                        res += "CALL " + data;
                        break;
                    case 7:
                        res += "RET ";
                        break;
                    default:
                        throw new Exception("Erro interno no controle");
                }
            } else {
                switch(Controller) {
                    case 0:
                        res += PrintOperation(ULAOperation);
                        res += "A, A";
                        break;
                    case 1:
                        res += PrintOperation(ULAOperation);
                        res += "A, ";
                        res += PrintRegister(Registrer);
                        break;
                    case 2:
                        res += PrintOperation(ULAOperation);
                        data = program[++i];
                        res += "A, ";
                        res += data +"";
                        break;
                    case 3:
                        res += PrintOperation(ULAOperation);
                        res += "A, ";
                        res += PrintOutput(Registrer);
                        break;
                    case 4:
                        res += PrintOperation(ULAOperation);
                        res += PrintOutput(Registrer);
                        res += ", A";
                        break;
                    case 5:
                        res += PrintOperation(ULAOperation);
                        data = program[++i];
                        data = data << 8;
                        data += program[++i];
                        res += "#"+data +", A";
                        break;
                    case 6:
                        res += PrintOperation(ULAOperation);
                        res += PrintInput(Registrer);
                        res += ", A";
                        break;
                    default:
                        throw new Exception("Erro interno no controle");
                }
            }
            return res;
        }

        public static int CountLines(string program, int index) {
            int line = 1;
            for (int i = 0; i < program.Length; i++) {
                if (program[i] == '\n') line++;
                if (i >= index) break;
            }
            return line;
        }

        public static string ToHex(byte value) {
            string res = value.ToString("X");
            if (res.Length == 1)
                return "0" + res;
            return res;
        }
    }
}
