﻿using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusSimulator {
    public class Command {
        public int Value;
        public int Size;
        public bool IsHighDecoder;
        public Operation Operation;
        public Registrer Registrer;
        public Controller Controller;
        public delegate void Function(Simulator simulator);
        public Function Execute;
        

        public void CreateFunction() {
            if(IsHighDecoder) {
                switch(Controller) {
                    case Controller.JMP:
                        Execute = delegate(Simulator simulator) {
                            simulator.NextInstruction = Value;
                        };
                        break;
                    case Controller.JMPC:
                        Execute = delegate (Simulator simulator) {
                            if(simulator.Flag_C)
                                simulator.NextInstruction = Value;
                        };
                        break;
                    case Controller.JMPZ:
                        Execute = delegate (Simulator simulator) {
                            if(simulator.Flag_Z)
                                simulator.NextInstruction = Value;
                        };
                        break;
                    case Controller.CALL:
                        throw new NotImplementedException();
                        break;
                    case Controller.RET:
                        throw new NotImplementedException();
                        break;
                    default:
                        break;
                }
            }
        }

        public string HexString {
            get {
                return null;
            }
        }
        public byte[] HexArray {
            get {
                return null;
            }
        }
        public string Description {
            get {
                string res = "";
                if(IsHighDecoder) {
                    switch(Controller) {
                        case Controller.JMP:
                            res += "Pula para o endereço " + Value + " da ROM.";
                            break;
                        case Controller.JMPC:
                            res += "Pula para o endereço " + Value + " da ROM se o flag C estiver ativo.";
                            break;
                        case Controller.JMPZ:
                            res += "Pula para o endereço " + Value + " da ROM se o flag Z estiver ativo.";
                            break;
                        case Controller.CALL:
                            res += "Chama o procedimento no endereço " + Value + " da ROM.";
                            break;
                        case Controller.RET:
                            res += "Retorna do procedimento";
                            break;
                        default:
                            break;
                    }
                } else {
                    switch(Operation) {
                        case Operation.ADD:
                            res += "Executa a operação ADD (adição) entre o acumulador e ";
                            break;
                        case Operation.SUB:
                            res += "Executa a operação SUB (subtração) entre o acumulador e ";
                            break;
                        case Operation.AND:
                            res += "Executa a operação AND (e) entre o acumulador e ";
                            break;
                        case Operation.OR:
                            res += "Executa a operação OR (ou) entre o acumulador e ";
                            break;
                        case Operation.XOR:
                            res += "Executa a operação XOR (ou especial) entre o acumulador e ";
                            break;
                        case Operation.NOT:
                            res += "Executa a operação NOT (não) entre o acumulador e ";
                            break;
                        case Operation.MOV:
                            res += "Executa a operação MOV (move) entre o acumulador e ";
                            break;
                        case Operation.INC:
                            res += "Executa a operação INC (incrementa) no acumulador e envia no";
                            break;
                        default:
                            break;
                    }
                }
                return null;
            }
        }
        
    }
    
    public enum Operation {
        ADD, SUB, AND, OR, XOR, NOT, MOV, INC
    }

    public enum Registrer {
        B_IN1_OUT1, C_IN2_OUT2, D_IN3_OUT3, E_IN4_OUT4
    }
    public enum Controller {
        ACC_ACC_ACC, ACC_ACC_REG, ACC_ACC_RAM, ACC_ACC_OUT, ACC_REG_ACC, ACC_RAM_ACC, ACC_IN_ACC,
        ACC_DATA_ACC, ACC_DATA_REG, ACC_DATA_RAM, JMP, JMPC, JMPZ, CALL, RET
    }
}
