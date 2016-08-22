using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusSimulator {
    public static class Helpers {
        public delegate void CommandFunction(Simulator simulator);

        public static Command[] GenerateFunctions(int[] program) {
            Command[] commands = new Command[program.Length];
            int ULAOperation;
            int Registrer;
            int Controller;
            bool IsHighDecoder = false;
            int i = 0;
            while(i < program.Length) {
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



                Console.Write(PrintController(Controller, IsHighDecoder, program, ref i, Registrer, ULAOperation));

                Console.WriteLine();
                IsHighDecoder = false;
                ++i;
            }

            

            return commands;
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
                    throw new Exception("Erro interno no ipput");
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
                    throw new Exception("Erro interno no ipput");
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
    }
}
