using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public delegate void Function(Simulator simulator);
    
    public class Instruction {
        public string Text = "";
        public string Description = "";
        public Function Function = delegate(Simulator sim) { };
        public int Size;
        private Registrer_enum RegistrerEnum = Registrer_enum.B;
        private Instruction_enum InstructionEnum = Instruction_enum.ADD;
        private Operation_enum OperationEnum = Operation_enum.A_A;
        public bool HasBreakpoint = false;
        private byte RamAddress;
        private byte RomValue;

        public int Address = 0; //JMP somewhere -> JMP #42
        public string Label;
        
        public Instruction(string text, string description = "", int size = 1) {
            Text = text;
            Description = description;
            Size = size;
        }

        public override string ToString() {
            return Text;
        }

        private enum Registrer_enum {
            B, C, D, E
        }
        private enum Instruction_enum {
            ADD, SUB, AND, OR, XOR, NOT, MOV, INC
        }
        private enum Operation_enum {
            A_A, A_REG, A_RAM, A_OUT, REG_A, RAM_A, IN_A,
            ROM_A, ROM_REG, ROM_RAM, JMP, JMPC, JMPZ, CALL,
            RET, DRAM_A, A_DRAM, PUSH, POP, PUSHA, POPA
        }
        public byte[] Convert() {
            byte[] res = null;
            int hd = 0;
            switch (OperationEnum) {
                case Operation_enum.A_A:
                    res = new byte[]{ A_A_BYTE };
                    break;
                case Operation_enum.A_REG:
                    res = new byte[] { A_REG_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[0] = (byte)(res[0] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[0] = (byte)(res[0] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[0] = (byte)(res[0] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[0] = (byte)(res[0] | E_BYTE);
                    break;
                case Operation_enum.A_RAM:
                    res = new byte[] { A_RAM_BYTE, 0x00 };
                    res[1] = RamAddress;
                    break;
                case Operation_enum.A_OUT:
                    res = new byte[] { A_OUT_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[0] = (byte)(res[0] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[0] = (byte)(res[0] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[0] = (byte)(res[0] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[0] = (byte)(res[0] | E_BYTE);
                    break;
                case Operation_enum.REG_A:
                    res = new byte[] { REG_A_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[0] = (byte)(res[0] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[0] = (byte)(res[0] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[0] = (byte)(res[0] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[0] = (byte)(res[0] | E_BYTE);
                    break;
                case Operation_enum.RAM_A:
                    res = new byte[] { RAM_A_BYTE, 0x00 };
                    res[1] = RamAddress;
                    break;
                case Operation_enum.IN_A:
                    res = new byte[] { IN_A_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[0] = (byte)(res[0] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[0] = (byte)(res[0] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[0] = (byte)(res[0] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[0] = (byte)(res[0] | E_BYTE);
                    break;
                case Operation_enum.ROM_A:
                    hd = 1;
                    res = new byte[] { HD_BYTE, ROM_A_BYTE, 0x00 };
                    res[2] = RomValue;
                    break;
                case Operation_enum.ROM_REG:
                    hd = 1;
                    res = new byte[] { HD_BYTE, ROM_REG_BYTE, 0x00 };
                    if (RegistrerEnum == Registrer_enum.B) res[1] = (byte)(res[1] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[1] = (byte)(res[1] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[1] = (byte)(res[1] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[1] = (byte)(res[1] | E_BYTE);
                    res[2] = RomValue;
                    break;
                case Operation_enum.ROM_RAM:
                    hd = 1;
                    res = new byte[] { HD_BYTE, ROM_RAM_BYTE, 0x00, 0x00 };
                    res[2] = RomValue;
                    res[3] = RamAddress;
                    break;
                case Operation_enum.JMP:
                    res = new byte[] { HD_BYTE, JMP_BYTE, 0x00, 0x00 };
                    res[2] = (byte)(Address/256);
                    res[3] = (byte)(Address%256);
                    return res;
                case Operation_enum.JMPC:
                    res = new byte[] { HD_BYTE, JMPC_BYTE, 0x00, 0x00 };
                    res[2] = (byte)(Address / 256);
                    res[3] = (byte)(Address % 256);
                    return res;
                case Operation_enum.JMPZ:
                    res = new byte[] { HD_BYTE, JMPZ_BYTE, 0x00, 0x00 };
                    res[2] = (byte)(Address / 256);
                    res[3] = (byte)(Address % 256);
                    return res;
                case Operation_enum.CALL:
                    res = new byte[] { HD_BYTE, CALL_BYTE, 0x00, 0x00 };
                    res[2] = (byte)(Address / 256);
                    res[3] = (byte)(Address % 256);
                    return res;
                case Operation_enum.RET:
                    res = new byte[] { HD_BYTE, HD_BYTE, CALL_BYTE };
                    return res;
                case Operation_enum.DRAM_A:
                    res = new byte[] { HD_BYTE, HD_BYTE, DRAM_A_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[2] = (byte)(res[2] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[2] = (byte)(res[2] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[2] = (byte)(res[2] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[2] = (byte)(res[2] | E_BYTE);
                    hd = 2;
                    break;
                case Operation_enum.A_DRAM:
                    res = new byte[] { HD_BYTE, HD_BYTE, A_DRAM_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[2] = (byte)(res[2] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[2] = (byte)(res[2] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[2] = (byte)(res[2] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[2] = (byte)(res[2] | E_BYTE);
                    hd = 2;
                    break;
                case Operation_enum.PUSH:
                    res = new byte[] { HD_BYTE, HD_BYTE, PUSH_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[2] = (byte)(res[2] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[2] = (byte)(res[2] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[2] = (byte)(res[2] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[2] = (byte)(res[2] | E_BYTE);
                    return res;
                case Operation_enum.POP:
                    res = new byte[] { HD_BYTE, HD_BYTE, POP_BYTE };
                    if (RegistrerEnum == Registrer_enum.B) res[2] = (byte)(res[2] | B_BYTE);
                    if (RegistrerEnum == Registrer_enum.C) res[2] = (byte)(res[2] | C_BYTE);
                    if (RegistrerEnum == Registrer_enum.D) res[2] = (byte)(res[2] | D_BYTE);
                    if (RegistrerEnum == Registrer_enum.E) res[2] = (byte)(res[2] | E_BYTE);
                    return res;
                case Operation_enum.PUSHA:
                    res = new byte[] { HD_BYTE, HD_BYTE, PUSHA_BYTE };
                    return res;
                case Operation_enum.POPA:
                    res = new byte[] { HD_BYTE, HD_BYTE, POPA_BYTE };
                    return res;
                default:
                    return null;
            }
            switch (InstructionEnum) {
                case Instruction_enum.ADD:
                    res[hd] = (byte)(res[hd] | ADD_BYTE);
                    return res;
                case Instruction_enum.AND:
                    res[hd] = (byte)(res[hd] | AND_BYTE);
                    return res;
                case Instruction_enum.INC:
                    res[hd] = (byte)(res[hd] | INC_BYTE);
                    return res;
                case Instruction_enum.MOV:
                    res[hd] = (byte)(res[hd] | MOV_BYTE);
                    return res;
                case Instruction_enum.NOT:
                    res[hd] = (byte)(res[hd] | NOT_BYTE);
                    return res;
                case Instruction_enum.OR:
                    res[hd] = (byte)(res[hd] | OR_BYTE);
                    return res;
                case Instruction_enum.SUB:
                    res[hd] = (byte)(res[hd] | SUB_BYTE);
                    return res;
                case Instruction_enum.XOR:
                    res[hd] = (byte)(res[hd] | XOR_BYTE);
                    return res;
                default:
                    return null;
            }
        }

        public static void Convert(byte[] byteArray, int inputIndex, out int outputIndex, out Instruction instruction) {
            instruction = null;
            int index = inputIndex;
            byte byte_fluxo = (byte)(byteArray[index] & HD_BYTE);
            byte byte_reg = (byte)(byteArray[index] & D_BYTE);
            byte byte_instruction = (byte)(byteArray[index] & INC_BYTE);
            if (byte_fluxo == HD_BYTE) {
                ++index;
                byte_fluxo = (byte)(byteArray[index] & HD_BYTE);
                byte_reg = (byte)(byteArray[index] & D_BYTE);
                byte_instruction = (byte)(byteArray[index] & INC_BYTE);
                if (byte_fluxo == HD_BYTE) {
                    ++index;
                    byte_fluxo = (byte)(byteArray[index] & HD_BYTE);
                    byte_reg = (byte)(byteArray[index] & D_BYTE);
                    byte_instruction = (byte)(byteArray[index] & INC_BYTE);
                    switch (byte_fluxo) {
                        case RET_BYTE:
                            instruction = RET();
                            break;
                        case DRAM_A_BYTE: {
                                switch (byte_instruction) {
                                    case ADD_BYTE:
                                        if (byte_reg == B_BYTE) instruction = ADD_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = ADD_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = ADD_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = ADD_DRAME_A();
                                        break;
                                    case SUB_BYTE:
                                        if (byte_reg == B_BYTE) instruction = SUB_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = SUB_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = SUB_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = SUB_DRAME_A();
                                        break;
                                    case AND_BYTE:
                                        if (byte_reg == B_BYTE) instruction = AND_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = AND_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = AND_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = AND_DRAME_A();
                                        break;
                                    case OR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = OR_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = OR_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = OR_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = OR_DRAME_A();
                                        break;
                                    case XOR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = XOR_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = XOR_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = XOR_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = XOR_DRAME_A();
                                        break;
                                    case NOT_BYTE:
                                        if (byte_reg == B_BYTE) instruction = NOT_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = NOT_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = NOT_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = NOT_DRAME_A();
                                        break;
                                    case MOV_BYTE:
                                        if (byte_reg == B_BYTE) instruction = MOV_DRAMB_A();
                                        if (byte_reg == B_BYTE) instruction = MOV_DRAMC_A();
                                        if (byte_reg == B_BYTE) instruction = MOV_DRAMD_A();
                                        if (byte_reg == B_BYTE) instruction = MOV_DRAME_A();
                                        break;
                                    case INC_BYTE:
                                        if (byte_reg == B_BYTE) instruction = INC_DRAMB_A();
                                        if (byte_reg == C_BYTE) instruction = INC_DRAMC_A();
                                        if (byte_reg == D_BYTE) instruction = INC_DRAMD_A();
                                        if (byte_reg == E_BYTE) instruction = INC_DRAME_A();
                                        break;
                                    default:
                                        ThrowConvertError();
                                        break;
                                }
                            }
                            break;
                        case A_DRAM_BYTE: {
                                switch (byte_instruction) {
                                    case ADD_BYTE:
                                        if (byte_reg == B_BYTE) instruction = ADD_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = ADD_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = ADD_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = ADD_A_DRAME();
                                        break;
                                    case SUB_BYTE:
                                        if (byte_reg == B_BYTE) instruction = SUB_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = SUB_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = SUB_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = SUB_A_DRAME();
                                        break;
                                    case AND_BYTE:
                                        if (byte_reg == B_BYTE) instruction = AND_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = AND_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = AND_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = AND_A_DRAME();
                                        break;
                                    case OR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = OR_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = OR_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = OR_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = OR_A_DRAME();
                                        break;
                                    case XOR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = XOR_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = XOR_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = XOR_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = XOR_A_DRAME();
                                        break;
                                    case NOT_BYTE:
                                        if (byte_reg == B_BYTE) instruction = NOT_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = NOT_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = NOT_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = NOT_A_DRAME();
                                        break;
                                    case MOV_BYTE:
                                        if (byte_reg == B_BYTE) instruction = MOV_A_DRAMB();
                                        if (byte_reg == B_BYTE) instruction = MOV_A_DRAMC();
                                        if (byte_reg == B_BYTE) instruction = MOV_A_DRAMD();
                                        if (byte_reg == B_BYTE) instruction = MOV_A_DRAME();
                                        break;
                                    case INC_BYTE:
                                        if (byte_reg == B_BYTE) instruction = INC_A_DRAMB();
                                        if (byte_reg == C_BYTE) instruction = INC_A_DRAMC();
                                        if (byte_reg == D_BYTE) instruction = INC_A_DRAMD();
                                        if (byte_reg == E_BYTE) instruction = INC_A_DRAME();
                                        break;
                                    default:
                                        ThrowConvertError();
                                        break;
                                }
                            }
                            break;
                        case PUSH_BYTE: {
                                if (byte_reg == B_BYTE) instruction = PUSH_B();
                                if (byte_reg == C_BYTE) instruction = PUSH_C();
                                if (byte_reg == D_BYTE) instruction = PUSH_D();
                                if (byte_reg == E_BYTE) instruction = PUSH_E();
                            }
                            break;
                        case POP_BYTE: {
                                if (byte_reg == B_BYTE) instruction = POP_B();
                                if (byte_reg == C_BYTE) instruction = POP_C();
                                if (byte_reg == D_BYTE) instruction = POP_D();
                                if (byte_reg == E_BYTE) instruction = POP_E();
                            }
                            break;
                        case PUSHA_BYTE:
                            instruction = PUSHA();
                            break;
                        case POPA_BYTE:
                            instruction = POPA();
                            break;
                        default:
                            ThrowConvertError();
                            break;
                    }
                } else {
                    switch (byte_fluxo) {
                        case ROM_A_BYTE: {
                                ++index;
                                byte val = byteArray[index];
                                switch (byte_instruction) {
                                    case ADD_BYTE:
                                        instruction = ADD_ROM_A(val);
                                        break;
                                    case SUB_BYTE:
                                        instruction = SUB_ROM_A(val);
                                        break;
                                    case AND_BYTE:
                                        instruction = AND_ROM_A(val);
                                        break;
                                    case OR_BYTE:
                                        instruction = OR_ROM_A(val);
                                        break;
                                    case XOR_BYTE:
                                        instruction = XOR_ROM_A(val);
                                        break;
                                    case NOT_BYTE:
                                        instruction = NOT_ROM_A(val);
                                        break;
                                    case MOV_BYTE:
                                        instruction = MOV_ROM_A(val);
                                        break;
                                    case INC_BYTE:
                                        instruction = INC_ROM_A(val);
                                        break;
                                    default:
                                        ThrowConvertError();
                                        break;
                                }
                            }
                            break;
                        case ROM_REG_BYTE: {
                                ++index;
                                byte val = byteArray[index];
                                switch (byte_instruction) {
                                    case ADD_BYTE:
                                        if (byte_reg == B_BYTE) instruction = ADD_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = ADD_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = ADD_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = ADD_ROM_E(val);
                                        break;
                                    case SUB_BYTE:
                                        if (byte_reg == B_BYTE) instruction = SUB_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = SUB_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = SUB_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = SUB_ROM_E(val);
                                        break;
                                    case AND_BYTE:
                                        if (byte_reg == B_BYTE) instruction = AND_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = AND_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = AND_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = AND_ROM_E(val);
                                        break;
                                    case OR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = OR_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = OR_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = OR_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = OR_ROM_E(val);
                                        break;
                                    case XOR_BYTE:
                                        if (byte_reg == B_BYTE) instruction = XOR_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = XOR_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = XOR_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = XOR_ROM_E(val);
                                        break;
                                    case NOT_BYTE:
                                        if (byte_reg == B_BYTE) instruction = NOT_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = NOT_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = NOT_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = NOT_ROM_E(val);
                                        break;
                                    case MOV_BYTE:
                                        if (byte_reg == B_BYTE) instruction = MOV_ROM_B(val);
                                        if (byte_reg == B_BYTE) instruction = MOV_ROM_C(val);
                                        if (byte_reg == B_BYTE) instruction = MOV_ROM_D(val);
                                        if (byte_reg == B_BYTE) instruction = MOV_ROM_E(val);
                                        break;
                                    case INC_BYTE:
                                        if (byte_reg == B_BYTE) instruction = INC_ROM_B(val);
                                        if (byte_reg == C_BYTE) instruction = INC_ROM_C(val);
                                        if (byte_reg == D_BYTE) instruction = INC_ROM_D(val);
                                        if (byte_reg == E_BYTE) instruction = INC_ROM_E(val);
                                        break;
                                    default:
                                        ThrowConvertError();
                                        break;
                                }
                            }
                            break;
                        case ROM_RAM_BYTE: {
                                ++index;
                                byte val = byteArray[index];
                                ++index;
                                byte end = byteArray[index];
                                switch (byte_instruction) {
                                    case ADD_BYTE:
                                        instruction = ADD_ROM_RAM(val, end);
                                        break;
                                    case SUB_BYTE:
                                        instruction = SUB_ROM_RAM(val, end);
                                        break;
                                    case AND_BYTE:
                                        instruction = AND_ROM_RAM(val, end);
                                        break;
                                    case OR_BYTE:
                                        instruction = OR_ROM_RAM(val, end);
                                        break;
                                    case XOR_BYTE:
                                        instruction = XOR_ROM_RAM(val, end);
                                        break;
                                    case NOT_BYTE:
                                        instruction = NOT_ROM_RAM(val, end);
                                        break;
                                    case MOV_BYTE:
                                        instruction = MOV_ROM_RAM(val, end);
                                        break;
                                    case INC_BYTE:
                                        instruction = INC_ROM_RAM(val, end);
                                        break;
                                    default:
                                        ThrowConvertError();
                                        break;
                                }
                            }
                            break;
                        case JMP_BYTE: {
                                ++index;
                                int end = byteArray[index] * 256;
                                ++index;
                                end += byteArray[index];
                                instruction = JMP("Byte_" + end);
                                instruction.Address = end;
                            }
                            break;
                        case JMPC_BYTE: {
                                ++index;
                                int end = byteArray[index] * 256;
                                ++index;
                                end += byteArray[index];
                                instruction = JMPC("Byte_" + end);
                                instruction.Address = end;
                            }
                            break;
                        case JMPZ_BYTE: {
                                ++index;
                                int end = byteArray[index] * 256;
                                ++index;
                                end += byteArray[index];
                                instruction = JMPZ("Byte_" + end);
                                instruction.Address = end;
                            }
                            break;
                        case CALL_BYTE: {
                                ++index;
                                int end = byteArray[index] * 256;
                                ++index;
                                end += byteArray[index];
                                instruction = CALL("Byte_" + end);
                                instruction.Address = end;
                            }
                            break;
                        default:
                            ThrowConvertError();
                            break;
                    }
                }
            } else {
                switch (byte_fluxo) {
                    case A_A_BYTE: {
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    instruction = ADD_A_A();
                                    break;
                                case SUB_BYTE:
                                    instruction = SUB_A_A();
                                    break;
                                case AND_BYTE:
                                    instruction = AND_A_A();
                                    break;
                                case OR_BYTE:
                                    instruction = OR_A_A();
                                    break;
                                case XOR_BYTE:
                                    instruction = XOR_A_A();
                                    break;
                                case NOT_BYTE:
                                    instruction = NOT_A_A();
                                    break;
                                case MOV_BYTE:
                                    instruction = MOV_A_A();
                                    break;
                                case INC_BYTE:
                                    instruction = INC_A_A();
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }            
                        }                
                        break;           
                    case A_REG_BYTE: {
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    if (byte_reg == B_BYTE) instruction = ADD_A_B();
                                    if (byte_reg == C_BYTE) instruction = ADD_A_C();
                                    if (byte_reg == D_BYTE) instruction = ADD_A_D();
                                    if (byte_reg == E_BYTE) instruction = ADD_A_E();
                                    break;
                                case SUB_BYTE:
                                    if (byte_reg == B_BYTE) instruction = SUB_A_B();
                                    if (byte_reg == C_BYTE) instruction = SUB_A_C();
                                    if (byte_reg == D_BYTE) instruction = SUB_A_D();
                                    if (byte_reg == E_BYTE) instruction = SUB_A_E();
                                    break;
                                case AND_BYTE:
                                    if (byte_reg == B_BYTE) instruction = AND_A_B();
                                    if (byte_reg == C_BYTE) instruction = AND_A_C();
                                    if (byte_reg == D_BYTE) instruction = AND_A_D();
                                    if (byte_reg == E_BYTE) instruction = AND_A_E();
                                    break;
                                case OR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = OR_A_B();
                                    if (byte_reg == C_BYTE) instruction = OR_A_C();
                                    if (byte_reg == D_BYTE) instruction = OR_A_D();
                                    if (byte_reg == E_BYTE) instruction = OR_A_E();
                                    break;
                                case XOR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = XOR_A_B();
                                    if (byte_reg == C_BYTE) instruction = XOR_A_C();
                                    if (byte_reg == D_BYTE) instruction = XOR_A_D();
                                    if (byte_reg == E_BYTE) instruction = XOR_A_E();
                                    break;
                                case NOT_BYTE:
                                    if (byte_reg == B_BYTE) instruction = NOT_A_B();
                                    if (byte_reg == C_BYTE) instruction = NOT_A_C();
                                    if (byte_reg == D_BYTE) instruction = NOT_A_D();
                                    if (byte_reg == E_BYTE) instruction = NOT_A_E();
                                    break;
                                case MOV_BYTE:
                                    if (byte_reg == B_BYTE) instruction = MOV_A_B();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_C();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_D();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_E();
                                    break;
                                case INC_BYTE:
                                    if (byte_reg == B_BYTE) instruction = INC_A_B();
                                    if (byte_reg == C_BYTE) instruction = INC_A_C();
                                    if (byte_reg == D_BYTE) instruction = INC_A_D();
                                    if (byte_reg == E_BYTE) instruction = INC_A_E();
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    case A_RAM_BYTE: {
                            ++index;
                            byte value = byteArray[index];
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    instruction = ADD_A_RAM(value);
                                    break;
                                case SUB_BYTE:
                                    instruction = SUB_A_RAM(value);
                                    break;
                                case AND_BYTE:
                                    instruction = AND_A_RAM(value);
                                    break;
                                case OR_BYTE:
                                    instruction = OR_A_RAM(value);
                                    break;
                                case XOR_BYTE:
                                    instruction = XOR_A_RAM(value);
                                    break;
                                case NOT_BYTE:
                                    instruction = NOT_A_RAM(value);
                                    break;
                                case MOV_BYTE:
                                    instruction = MOV_A_RAM(value);
                                    break;
                                case INC_BYTE:
                                    instruction = INC_A_RAM(value);
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    case A_OUT_BYTE: {
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    if (byte_reg == B_BYTE) instruction = ADD_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = ADD_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = ADD_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = ADD_A_OUT3();
                                    break;
                                case SUB_BYTE:
                                    if (byte_reg == B_BYTE) instruction = SUB_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = SUB_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = SUB_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = SUB_A_OUT3();
                                    break;
                                case AND_BYTE:
                                    if (byte_reg == B_BYTE) instruction = AND_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = AND_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = AND_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = AND_A_OUT3();
                                    break;
                                case OR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = OR_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = OR_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = OR_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = OR_A_OUT3();
                                    break;
                                case XOR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = XOR_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = XOR_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = XOR_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = XOR_A_OUT3();
                                    break;
                                case NOT_BYTE:
                                    if (byte_reg == B_BYTE) instruction = NOT_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = NOT_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = NOT_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = NOT_A_OUT3();
                                    break;
                                case MOV_BYTE:
                                    if (byte_reg == B_BYTE) instruction = MOV_A_OUT0();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_OUT1();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_OUT2();
                                    if (byte_reg == B_BYTE) instruction = MOV_A_OUT3();
                                    break;
                                case INC_BYTE:
                                    if (byte_reg == B_BYTE) instruction = INC_A_OUT0();
                                    if (byte_reg == C_BYTE) instruction = INC_A_OUT1();
                                    if (byte_reg == D_BYTE) instruction = INC_A_OUT2();
                                    if (byte_reg == E_BYTE) instruction = INC_A_OUT3();
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    case REG_A_BYTE: {
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    if (byte_reg == B_BYTE) instruction = ADD_B_A();
                                    if (byte_reg == C_BYTE) instruction = ADD_C_A();
                                    if (byte_reg == D_BYTE) instruction = ADD_D_A();
                                    if (byte_reg == E_BYTE) instruction = ADD_E_A();
                                    break;
                                case SUB_BYTE:
                                    if (byte_reg == B_BYTE) instruction = SUB_B_A();
                                    if (byte_reg == C_BYTE) instruction = SUB_C_A();
                                    if (byte_reg == D_BYTE) instruction = SUB_D_A();
                                    if (byte_reg == E_BYTE) instruction = SUB_E_A();
                                    break;
                                case AND_BYTE:
                                    if (byte_reg == B_BYTE) instruction = AND_B_A();
                                    if (byte_reg == C_BYTE) instruction = AND_C_A();
                                    if (byte_reg == D_BYTE) instruction = AND_D_A();
                                    if (byte_reg == E_BYTE) instruction = AND_E_A();
                                    break;
                                case OR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = OR_B_A();
                                    if (byte_reg == C_BYTE) instruction = OR_C_A();
                                    if (byte_reg == D_BYTE) instruction = OR_D_A();
                                    if (byte_reg == E_BYTE) instruction = OR_E_A();
                                    break;
                                case XOR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = XOR_B_A();
                                    if (byte_reg == C_BYTE) instruction = XOR_C_A();
                                    if (byte_reg == D_BYTE) instruction = XOR_D_A();
                                    if (byte_reg == E_BYTE) instruction = XOR_E_A();
                                    break;
                                case NOT_BYTE:
                                    if (byte_reg == B_BYTE) instruction = NOT_B_A();
                                    if (byte_reg == C_BYTE) instruction = NOT_C_A();
                                    if (byte_reg == D_BYTE) instruction = NOT_D_A();
                                    if (byte_reg == E_BYTE) instruction = NOT_E_A();
                                    break;
                                case MOV_BYTE:
                                    if (byte_reg == B_BYTE) instruction = MOV_B_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_C_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_D_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_E_A();
                                    break;
                                case INC_BYTE:
                                    if (byte_reg == B_BYTE) instruction = INC_B_A();
                                    if (byte_reg == C_BYTE) instruction = INC_C_A();
                                    if (byte_reg == D_BYTE) instruction = INC_D_A();
                                    if (byte_reg == E_BYTE) instruction = INC_E_A();
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    case RAM_A_BYTE: { 
                        ++index;
                        byte value = byteArray[index]; 
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    instruction = ADD_RAM_A(value);
                                    break;
                                case SUB_BYTE:
                                    instruction = SUB_RAM_A(value);
                                    break;
                                case AND_BYTE:
                                    instruction = AND_RAM_A(value);
                                    break;
                                case OR_BYTE:
                                    instruction = OR_RAM_A(value);
                                    break;
                                case XOR_BYTE:
                                    instruction = XOR_RAM_A(value);
                                    break;
                                case NOT_BYTE:
                                    instruction = NOT_RAM_A(value);
                                    break;
                                case MOV_BYTE:
                                    instruction = MOV_RAM_A(value);
                                    break;
                                case INC_BYTE:
                                    instruction = INC_RAM_A(value);
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    case IN_A_BYTE: {
                            switch (byte_instruction) {
                                case ADD_BYTE:
                                    if (byte_reg == B_BYTE) instruction = ADD_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = ADD_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = ADD_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = ADD_IN3_A();
                                    break;
                                case SUB_BYTE:
                                    if (byte_reg == B_BYTE) instruction = SUB_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = SUB_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = SUB_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = SUB_IN3_A();
                                    break;
                                case AND_BYTE:
                                    if (byte_reg == B_BYTE) instruction = AND_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = AND_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = AND_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = AND_IN3_A();
                                    break;
                                case OR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = OR_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = OR_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = OR_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = OR_IN3_A();
                                    break;
                                case XOR_BYTE:
                                    if (byte_reg == B_BYTE) instruction = XOR_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = XOR_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = XOR_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = XOR_IN3_A();
                                    break;
                                case NOT_BYTE:
                                    if (byte_reg == B_BYTE) instruction = NOT_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = NOT_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = NOT_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = NOT_IN3_A();
                                    break;
                                case MOV_BYTE:
                                    if (byte_reg == B_BYTE) instruction = MOV_IN0_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_IN1_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_IN2_A();
                                    if (byte_reg == B_BYTE) instruction = MOV_IN3_A();
                                    break;
                                case INC_BYTE:
                                    if (byte_reg == B_BYTE) instruction = INC_IN0_A();
                                    if (byte_reg == C_BYTE) instruction = INC_IN1_A();
                                    if (byte_reg == D_BYTE) instruction = INC_IN2_A();
                                    if (byte_reg == E_BYTE) instruction = INC_IN3_A();
                                    break;
                                default:
                                    ThrowConvertError();
                                    break;
                            }
                        }
                        break;
                    default:
                        ThrowConvertError();
                        break;
                }
            }
            outputIndex = index+1;
        }

        private static void ThrowConvertError() {
            throw new Exception("Erro na conversão. Verifique se está utilizando um binário válido.");
        }

        //Instrução
        private const byte ADD_BYTE = 0x00;
        private const byte SUB_BYTE = 0x20;
        private const byte AND_BYTE = 0x40;
        private const byte OR_BYTE = 0x60;
        private const byte XOR_BYTE = 0x80;
        private const byte NOT_BYTE = 0xA0;
        private const byte MOV_BYTE = 0xC0;
        private const byte INC_BYTE = 0xE0;

        //Registrador
        private const byte B_BYTE = 0x00;
        private const byte C_BYTE = 0x08;
        private const byte D_BYTE = 0x10;
        private const byte E_BYTE = 0x18;

        //Fluxo de controle
        private const byte A_A_BYTE = 0x00;
        private const byte A_REG_BYTE = 0x01;
        private const byte A_RAM_BYTE = 0x02;
        private const byte A_OUT_BYTE = 0x03;
        private const byte REG_A_BYTE = 0x04;
        private const byte RAM_A_BYTE = 0x05;
        private const byte IN_A_BYTE = 0x06;

        private const byte HD_BYTE = 0x07;

        private const byte ROM_A_BYTE = 0x00;
        private const byte ROM_REG_BYTE = 0x01;
        private const byte ROM_RAM_BYTE = 0x02;
        private const byte JMP_BYTE = 0x03;
        private const byte JMPC_BYTE = 0x04;
        private const byte JMPZ_BYTE = 0x05;
        private const byte CALL_BYTE = 0x06;

        private const byte RET_BYTE = 0x00;
        private const byte DRAM_A_BYTE = 0x01;
        private const byte A_DRAM_BYTE = 0x02;
        private const byte PUSH_BYTE = 0x03;
        private const byte POP_BYTE = 0x04;
        private const byte PUSHA_BYTE = 0x05;
        private const byte POPA_BYTE = 0x06;
        
        public static Instruction ADD_A_A() {
            Instruction instruction = new Instruction("ADD A, A", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.RegistrerEnum = Registrer_enum.B;

            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_B() {
            Instruction instruction = new Instruction("ADD A, B", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_C() {
            Instruction instruction = new Instruction("ADD A, C", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_D() {
            Instruction instruction = new Instruction("ADD A, D", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_E() {
            Instruction instruction = new Instruction("ADD A, E", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_RAM(byte address) {
            Instruction instruction = new Instruction("ADD A, #"+Helpers.ToHex(address), "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço "+Helpers.ToHex(address)+".", 2);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT1() {
            Instruction instruction = new Instruction("ADD A, OUT1", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT2() {
            Instruction instruction = new Instruction("ADD A, OUT2", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT3() {
            Instruction instruction = new Instruction("ADD A, OUT3", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT0() {
            Instruction instruction = new Instruction("ADD A, OUT0", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_B_A() {
            Instruction instruction = new Instruction("ADD B, A", "Adiciona o valor do acumulador com o registrador B e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_C_A() {
            Instruction instruction = new Instruction("ADD C, A", "Adiciona o valor do acumulador com o registrador C e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_D_A() {
            Instruction instruction = new Instruction("ADD D, A", "Adiciona o valor do acumulador com o registrador D e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_E_A() {
            Instruction instruction = new Instruction("ADD E, A", "Adiciona o valor do acumulador com o registrador E e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[4];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_RAM_A(byte address) {
            Instruction instruction = new Instruction("ADD #" + Helpers.ToHex(address) + ", A", "Adiciona o valor do acumulador com o valor da memória no endreço "+Helpers.ToHex(address)+"e o resultado é colocado na memória no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[address];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN1_A() {
            Instruction instruction = new Instruction("ADD IN1, A", "Adiciona o valor do acumulador com o valor da entrada IN1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN2_A() {
            Instruction instruction = new Instruction("ADD IN2, A", "Adiciona o valor do acumulador com o valor da entrada IN2 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN3_A() {
            Instruction instruction = new Instruction("ADD IN3, A", "Adiciona o valor do acumulador com o valor da entrada IN3 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN0_A() {
            Instruction instruction = new Instruction("ADD IN0, A", "Adiciona o valor do acumulador com o valor da entrada IN0 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_A(byte data) {
            Instruction instruction = new Instruction("ADD "+ Helpers.ToHex(data) + ", A", "Adiciona o valor do acumulador com o valor "+Helpers.ToHex(data)+" e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_B(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", B", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_C(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", C", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_D(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", D", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_E(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", E", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", #"+Helpers.ToHex(address), "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado na memória no endereço "+Helpers.ToHex(address)+".", 4);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_DRAMB_A() {
            Instruction instruction = new Instruction("ADD #B, A", "Adiciona o valor do acumulador com o valor da memória no endreço que está no registrador B e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[simulator.Reg[1]];
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_DRAMC_A() {
            Instruction instruction = new Instruction("ADD #C, A", "Adiciona o valor do acumulador com o valor da memória no endreço que está no registrador C e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[simulator.Reg[2]];
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_DRAMD_A() {
            Instruction instruction = new Instruction("ADD #D, A", "Adiciona o valor do acumulador com o valor da memória no endreço que está no registrador D e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[simulator.Reg[3]];
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_DRAME_A() {
            Instruction instruction = new Instruction("ADD #E, A", "Adiciona o valor do acumulador com o valor da memória no endreço que está no registrador E e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[simulator.Reg[4]];
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_DRAMB() {
            Instruction instruction = new Instruction("ADD A, #B", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço Que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_DRAMC() {
            Instruction instruction = new Instruction("ADD A, #C", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço Que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_DRAMD() {
            Instruction instruction = new Instruction("ADD A, #D", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço Que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_DRAME() {
            Instruction instruction = new Instruction("ADD A, #E", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço Que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction SUB_A_A() {
            Instruction instruction = new Instruction("SUB A, A", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_B() {
            Instruction instruction = new Instruction("SUB A, B", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_C() {
            Instruction instruction = new Instruction("SUB A, C", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_D() {
            Instruction instruction = new Instruction("SUB A, D", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_E() {
            Instruction instruction = new Instruction("SUB A, E", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_RAM(byte address) {
            Instruction instruction = new Instruction("SUB A, #" + Helpers.ToHex(address), "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço " + Helpers.ToHex(address)+".", 2);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT1() {
            Instruction instruction = new Instruction("SUB A, OUT1", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT2() {
            Instruction instruction = new Instruction("SUB A, OUT2", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT3() {
            Instruction instruction = new Instruction("SUB A, OUT3", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT0() {
            Instruction instruction = new Instruction("SUB A, OUT0", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.ADD;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }

        public static Instruction SUB_B_A() {
            Instruction instruction = new Instruction("SUB B, A", "Subtrai o valor do acumulador com o registrador B e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_C_A() {
            Instruction instruction = new Instruction("SUB C, A", "Subtrai o valor do acumulador com o registrador C e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_D_A() {
            Instruction instruction = new Instruction("SUB D, A", "Subtrai o valor do acumulador com o registrador D e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_E_A() {
            Instruction instruction = new Instruction("SUB E, A", "Subtrai o valor do acumulador com o registrador E e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[4];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_RAM_A(byte address) {
            Instruction instruction = new Instruction("SUB #" + Helpers.ToHex(address) + ", A", "Subtrai o valor do acumulador com o valor da memória no endreço " + Helpers.ToHex(address) + "e o resultado é colocado na memória no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[address];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN1_A() {
            Instruction instruction = new Instruction("SUB IN1, A", "Subtrai o valor do acumulador com o valor da entrada IN1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN2_A() {
            Instruction instruction = new Instruction("SUB IN2, A", "Subtrai o valor do acumulador com o valor da entrada IN2 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN3_A() {
            Instruction instruction = new Instruction("SUB IN3, A", "Subtrai o valor do acumulador com o valor da entrada IN3 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN0_A() {
            Instruction instruction = new Instruction("SUB IN0, A", "Subtrai o valor do acumulador com o valor da entrada IN0 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_A(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", A", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_B(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", B", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_C(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", C", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_D(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", D", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_E(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", E", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data) + " e o resultado é colocado na memória no endereço " + Helpers.ToHex(address) + ".", 4);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_DRAMB_A() {
            Instruction instruction = new Instruction("SUB #B, A", "Subtrai o valor do acumulador com o valor da memória no endreço que está no registrador B e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[simulator.Reg[1]];
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_DRAMC_A() {
            Instruction instruction = new Instruction("SUB #C, A", "Subtrai o valor do acumulador com o valor da memória no endreço que está no registrador C e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[simulator.Reg[2]];
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_DRAMD_A() {
            Instruction instruction = new Instruction("SUB #D, A", "Subtrai o valor do acumulador com o valor da memória no endreço que está no registrador D e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[simulator.Reg[3]];
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_DRAME_A() {
            Instruction instruction = new Instruction("SUB #E, A", "Subtrai o valor do acumulador com o valor da memória no endreço que está no registrador E e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[simulator.Reg[4]];
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_DRAMB() {
            Instruction instruction = new Instruction("SUB A, #B", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_DRAMC() {
            Instruction instruction = new Instruction("SUB A, #C", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_DRAMD() {
            Instruction instruction = new Instruction("SUB A, #D", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_DRAME() {
            Instruction instruction = new Instruction("SUB A, #E", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.SUB;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction AND_A_A() {
            Instruction instruction = new Instruction("AND A, A", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_B() {
            Instruction instruction = new Instruction("AND A, B", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_C() {
            Instruction instruction = new Instruction("AND A, C", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_D() {
            Instruction instruction = new Instruction("AND A, D", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_E() {
            Instruction instruction = new Instruction("AND A, E", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_RAM(byte address) {
            Instruction instruction = new Instruction("AND A, #" + Helpers.ToHex(address), "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT0() {
            Instruction instruction = new Instruction("AND A, OUT0", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT1() {
            Instruction instruction = new Instruction("AND A, OUT1", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT2() {
            Instruction instruction = new Instruction("AND A, OUT2", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT3() {
            Instruction instruction = new Instruction("AND A, OUT3", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_B_A() {
            Instruction instruction = new Instruction("AND B, A", "Faz a operação AND do registrador B com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_C_A() {
            Instruction instruction = new Instruction("AND C, A", "Faz a operação AND do registrador C com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_D_A() {
            Instruction instruction = new Instruction("AND E, A", "Faz a operação AND do registrador D com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_E_A() {
            Instruction instruction = new Instruction("AND E, A", "Faz a operação AND do registrador E com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_RAM_A(byte address) {
            Instruction instruction = new Instruction("AND #" + Helpers.ToHex(address) + ", A", "Faz a operação AND do valor na memória RAM no endereço " + Helpers.ToHex(address) + " com o acumulador e o resultado se mantém no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN0_A() {
            Instruction instruction = new Instruction("AND IN0, A", "Faz a operação AND da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN1_A() {
            Instruction instruction = new Instruction("AND IN1, A", "Faz a operação AND da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN2_A() {
            Instruction instruction = new Instruction("AND IN2, A", "Faz a operação AND da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN3_A() {
            Instruction instruction = new Instruction("AND IN3, A", "Faz a operação AND da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_A(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", A", "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_B(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", B", "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_C(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", C", "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_D(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", D", "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_E(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", E", "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Faz a operação AND do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado na memória ram no endereço " + Helpers.ToHex(address) + ".", 4);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_DRAMB_A() {
            Instruction instruction = new Instruction("AND #B, A", "Faz a operação AND do valor na memória RAM no endereço que está no registrador B com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[simulator.Reg[1]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_DRAMC_A() {
            Instruction instruction = new Instruction("AND #C, A", "Faz a operação AND do valor na memória RAM no endereço que está no registrador C com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[simulator.Reg[2]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_DRAMD_A() {
            Instruction instruction = new Instruction("AND #D, A", "Faz a operação AND do valor na memória RAM no endereço que está no registrador D com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[simulator.Reg[3]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_DRAME_A() {
            Instruction instruction = new Instruction("AND #E, A", "Faz a operação AND do valor na memória RAM no endereço que está no registrador E com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[simulator.Reg[4]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_DRAMB() {
            Instruction instruction = new Instruction("AND A, #B", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[1]] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_DRAMC() {
            Instruction instruction = new Instruction("AND A, #C", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[2]] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_DRAMD() {
            Instruction instruction = new Instruction("AND A, #D", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[3]] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_DRAME() {
            Instruction instruction = new Instruction("AND A, #E", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.AND;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[4]] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_A() {
            Instruction instruction = new Instruction("OR A, A", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_B() {
            Instruction instruction = new Instruction("OR A, B", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_C() {
            Instruction instruction = new Instruction("OR A, C", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_D() {
            Instruction instruction = new Instruction("OR A, D", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_E() {
            Instruction instruction = new Instruction("OR A, E", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_RAM(byte address) {
            Instruction instruction = new Instruction("OR A, #" + Helpers.ToHex(address), "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT0() {
            Instruction instruction = new Instruction("OR A, OUT0", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT1() {
            Instruction instruction = new Instruction("OR A, OUT1", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT2() {
            Instruction instruction = new Instruction("OR A, OUT2", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT3() {
            Instruction instruction = new Instruction("OR A, OUT3", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_B_A() {
            Instruction instruction = new Instruction("OR B, A", "Faz a operação OR do registrador B com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_C_A() {
            Instruction instruction = new Instruction("OR C, A", "Faz a operação OR do registrador C com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_D_A() {
            Instruction instruction = new Instruction("OR D, A", "Faz a operação OR do registrador D com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_E_A() {
            Instruction instruction = new Instruction("OR E, A", "Faz a operação OR do registrador E com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_RAM_A(byte address) {
            Instruction instruction = new Instruction("OR #" + Helpers.ToHex(address) + ", A", "Faz a operação OR do valor na memória RAM no endereço " + Helpers.ToHex(address) + " com o acumulador e o resultado se mantém no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN0_A() {
            Instruction instruction = new Instruction("OR IN0, A", "Faz a operação OR da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN1_A() {
            Instruction instruction = new Instruction("OR IN1, A", "Faz a operação OR da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN2_A() {
            Instruction instruction = new Instruction("OR IN2, A", "Faz a operação OR da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN3_A() {
            Instruction instruction = new Instruction("OR IN3, A", "Faz a operação OR da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_A(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", A", "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_B(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", B", "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_C(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", C", "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_D(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", D", "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_E(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", E", "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Faz a operação OR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado na memória ram no endereço " + Helpers.ToHex(address) + ".", 4);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_DRAMB_A() {
            Instruction instruction = new Instruction("OR #B, A", "Faz a operação OR do valor na memória RAM no endereço que está no registrador B com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[simulator.Reg[1]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_DRAMC_A() {
            Instruction instruction = new Instruction("OR #C, A", "Faz a operação OR do valor na memória RAM no endereço que está no registrador C com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[simulator.Reg[2]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_DRAMD_A() {
            Instruction instruction = new Instruction("OR #D, A", "Faz a operação OR do valor na memória RAM no endereço que está no registrador D com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[simulator.Reg[3]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_DRAME_A() {
            Instruction instruction = new Instruction("OR #E, A", "Faz a operação OR do valor na memória RAM no endereço que está no registrador E com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[simulator.Reg[4]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_DRAMB() {
            Instruction instruction = new Instruction("OR A, #B", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[1]] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }

        public static Instruction OR_A_DRAMC() {
            Instruction instruction = new Instruction("OR A, #C", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[2]] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_DRAMD() {
            Instruction instruction = new Instruction("OR A, #D", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.OR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[3]] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_DRAME() {
            Instruction instruction = new Instruction("OR A, #E", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[4]] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_A() {
            Instruction instruction = new Instruction("XOR A, A", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_B() {
            Instruction instruction = new Instruction("XOR A, B", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_C() {
            Instruction instruction = new Instruction("XOR A, C", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_D() {
            Instruction instruction = new Instruction("XOR A, D", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_E() {
            Instruction instruction = new Instruction("XOR A, E", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_RAM(byte address) {
            Instruction instruction = new Instruction("XOR A, #" + Helpers.ToHex(address), "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT0() {
            Instruction instruction = new Instruction("XOR A, OUT0", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT1() {
            Instruction instruction = new Instruction("XOR A, OUT1", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT2() {
            Instruction instruction = new Instruction("XOR A, OUT2", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT3() {
            Instruction instruction = new Instruction("XOR A, OUT3", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_B_A() {
            Instruction instruction = new Instruction("XOR B, A", "Faz a operação XOR do registrador B com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_C_A() {
            Instruction instruction = new Instruction("XOR C, A", "Faz a operação XOR do registrador C com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_D_A() {
            Instruction instruction = new Instruction("XOR D, A", "Faz a operação XOR do registrador D com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_E_A() {
            Instruction instruction = new Instruction("XOR E, A", "Faz a operação XOR do registrador E com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_RAM_A(byte address) {
            Instruction instruction = new Instruction("XOR #" + Helpers.ToHex(address) + ", A", "Faz a operação XOR do valor na memória RAM no endereço " + Helpers.ToHex(address) + " com o acumulador e o resultado se mantém no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN0_A() {
            Instruction instruction = new Instruction("XOR IN0, A", "Faz a operação XOR da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN1_A() {
            Instruction instruction = new Instruction("XOR IN1, A", "Faz a operação XOR da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN2_A() {
            Instruction instruction = new Instruction("XOR IN2, A", "Faz a operação XOR da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN3_A() {
            Instruction instruction = new Instruction("XOR IN3, A", "Faz a operação XOR da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_A(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", A", "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_B(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", B", "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_C(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", C", "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_D(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", D", "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_E(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", E", "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Faz a operação XOR do valor " + Helpers.ToHex(data) + " com o acumulador e o resultado é colocado na memória ram no endereço " + Helpers.ToHex(address) + ".", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_DRAMB_A() {
            Instruction instruction = new Instruction("XOR #B, A", "Faz a operação XOR do valor na memória RAM no endereço que está no registrador B com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[simulator.Reg[1]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_DRAMC_A() {
            Instruction instruction = new Instruction("XOR #C, A", "Faz a operação XOR do valor na memória RAM no endereço que está no registrador C com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[simulator.Reg[2]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_DRAMD_A() {
            Instruction instruction = new Instruction("XOR #D, A", "Faz a operação XOR do valor na memória RAM no endereço que está no registrador D com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[simulator.Reg[3]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_DRAME_A() {
            Instruction instruction = new Instruction("XOR #E, A", "Faz a operação XOR do valor na memória RAM no endereço que está no registrador E com o acumulador e o resultado se mantém no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[simulator.Reg[4]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_DRAMB() {
            Instruction instruction = new Instruction("XOR A, #B", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[1]] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_DRAMC() {
            Instruction instruction = new Instruction("XOR A, #C", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[2]] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_DRAMD() {
            Instruction instruction = new Instruction("XOR A, #D", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[3]] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_DRAME() {
            Instruction instruction = new Instruction("XOR A, #E", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.XOR;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[4]] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_A() {
            Instruction instruction = new Instruction("NOT A, A", "Faz a operação NOT no acumulador e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_B() {
            Instruction instruction = new Instruction("NOT A, B", "Faz a operação NOT no acumulador e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_C() {
            Instruction instruction = new Instruction("NOT A, C", "Faz a operação NOT no acumulador e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_D() {
            Instruction instruction = new Instruction("NOT A, D", "Faz a operação NOT no acumulador e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_E() {
            Instruction instruction = new Instruction("NOT A, E", "Faz a operação NOT no acumulador e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_RAM(byte address) {
            Instruction instruction = new Instruction("NOT A, #" + Helpers.ToHex(address), "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT1() {
            Instruction instruction = new Instruction("NOT A, OUT1", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT2() {
            Instruction instruction = new Instruction("NOT A, OUT2", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT3() {
            Instruction instruction = new Instruction("NOT A, OUT3", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT0() {
            Instruction instruction = new Instruction("NOT A, OUT0", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_B_A() {
            Instruction instruction = new Instruction("NOT B, A", "Faz a operação NOT no registrador B e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[1]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_C_A() {
            Instruction instruction = new Instruction("NOT C, A", "Faz a operação NOT no registrador C e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[2]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_D_A() {
            Instruction instruction = new Instruction("NOT D, A", "Faz a operação NOT no registrador D e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[3]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_E_A() {
            Instruction instruction = new Instruction("NOT E, A", "Faz a operação NOT no registrador E e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[4]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_RAM_A(byte address) {
            Instruction instruction = new Instruction("NOT #" + Helpers.ToHex(address) + ", A", "Faz a operação NOT na memória no endereço " + Helpers.ToHex(address) + " e o resultado é colocado no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN1_A() {
            Instruction instruction = new Instruction("NOT IN1, A", "Faz a operação NOT na entrada IN1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[1]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN2_A() {
            Instruction instruction = new Instruction("NOT IN2, A", "Faz a operação NOT na entrada IN2 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[2]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN3_A() {
            Instruction instruction = new Instruction("NOT IN3, A", "Faz a operação NOT na entrada IN3 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[3]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN0_A() {
            Instruction instruction = new Instruction("NOT IN0, A", "Faz a operação NOT na entrada IN0 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_A(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", A", "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_B(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", B", "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_C(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", C", "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_D(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", D", "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_E(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", E", "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Faz a operação NOT do valor " + Helpers.ToHex(data) + " e o resultado é colocado na memória no endereço " + Helpers.ToHex(address) + ".", 4);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_DRAMB_A() {
            Instruction instruction = new Instruction("NOT #B, A", "Faz a operação NOT na memória no endereço que está no registrador B e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[simulator.Reg[1]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_DRAMC_A() {
            Instruction instruction = new Instruction("NOT #C, A", "Faz a operação NOT na memória no endereço que está no registrador C e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[simulator.Reg[2]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_DRAMD_A() {
            Instruction instruction = new Instruction("NOT #D, A", "Faz a operação NOT na memória no endereço que está no registrador D e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[simulator.Reg[3]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_DRAME_A() {
            Instruction instruction = new Instruction("NOT #E, A", "Faz a operação NOT na memória no endereço que está no registrador E e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[simulator.Reg[4]]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_DRAMB() {
            Instruction instruction = new Instruction("NOT A, #B", "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[1]] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_DRAMC() {
            Instruction instruction = new Instruction("NOT A, #C", "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[2]] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_DRAMD() {
            Instruction instruction = new Instruction("NOT A, #D", "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[3]] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_DRAME() {
            Instruction instruction = new Instruction("NOT A, #E", "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.NOT;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[4]] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction MOV_A_A() {
            Instruction instruction = new Instruction("MOV A, B", "Copia o conteúdo do acumulador para o acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_B() {
            Instruction instruction = new Instruction("MOV A, B", "Copia o conteúdo do acumulador para o registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_C() {
            Instruction instruction = new Instruction("MOV A, C", "Copia o conteúdo do acumulador para o registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_D() {
            Instruction instruction = new Instruction("MOV A, D", "Copia o conteúdo do acumulador para o registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_E() {
            Instruction instruction = new Instruction("MOV A, E", "Copia o conteúdo do acumulador para o registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_RAM(byte address) {
            Instruction instruction = new Instruction("MOV A, #" + Helpers.ToHex(address), "Copia o conteúdo do acumulador para a memória no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT1() {
            Instruction instruction = new Instruction("MOV A, OUT1", "Copia o conteúdo do acumulador para a saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT2() {
            Instruction instruction = new Instruction("MOV A, OUT2", "Copia o conteúdo do acumulador para a saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT3() {
            Instruction instruction = new Instruction("MOV A, OUT3", "Copia o conteúdo do acumulador para a saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT0() {
            Instruction instruction = new Instruction("MOV A, OUT0", "Copia o conteúdo do acumulador para a saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_B_A() {
            Instruction instruction = new Instruction("MOV B, A", "Copia o conteúdo do registrador B para o acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[1];
            };
            return instruction;
        }
        public static Instruction MOV_C_A() {
            Instruction instruction = new Instruction("MOV C, A", "Copia o conteúdo do registrador C para o acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[2];
            };
            return instruction;
        }
        public static Instruction MOV_D_A() {
            Instruction instruction = new Instruction("MOV D, A", "Copia o conteúdo do registrador D para o acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[3];
            };
            return instruction;
        }
        public static Instruction MOV_E_A() {
            Instruction instruction = new Instruction("MOV E, A", "Copia o conteúdo do registrador E para o acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[4];
            };
            return instruction;
        }
        public static Instruction MOV_RAM_A(byte address) {
            Instruction instruction = new Instruction("MOV #" + Helpers.ToHex(address) + ", A", "Copia o conteúdo do valor da memória no endereço " + Helpers.ToHex(address) + " para o acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[address];
            };
            return instruction;
        }
        public static Instruction MOV_IN1_A() {
            Instruction instruction = new Instruction("MOV IN1, A", "Copia o valor da entrada IN1 para o acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[1];
            };
            return instruction;
        }
        public static Instruction MOV_IN2_A() {
            Instruction instruction = new Instruction("MOV IN2, A", "Copia o valor da entrada IN2 para o acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[2];
            };
            return instruction;
        }
        public static Instruction MOV_IN3_A() {
            Instruction instruction = new Instruction("MOV IN3, A", "Copia o valor da entrada IN3 para o acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[3];
            };
            return instruction;
        }
        public static Instruction MOV_IN0_A() {
            Instruction instruction = new Instruction("MOV IN0, A", "Copia o valor da entrada IN0 para o acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[0];
            };
            return instruction;
        }
        public static Instruction MOV_ROM_A(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", A", "Copia o valor " + data + " para o acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_B(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", B", "Copia o valor " + Helpers.ToHex(data) + " para o registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_C(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", C", "Copia o valor " + Helpers.ToHex(data) + " para o registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_D(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", D", "Copia o valor " + Helpers.ToHex(data) + " para o registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_E(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", E", "Copia o valor " + Helpers.ToHex(data) + " para o registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Copia o valor " + Helpers.ToHex(data) + " para a memória no endereço " + Helpers.ToHex(address) + ".", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = data;
            };
            return instruction;
        }
        public static Instruction MOV_DRAMB_A() {
            Instruction instruction = new Instruction("MOV #B, A", "Copia o conteúdo do valor da memória no endereço que está no registrador B para o acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[simulator.Reg[1]];
            };
            return instruction;
        }
        public static Instruction MOV_DRAMC_A() {
            Instruction instruction = new Instruction("MOV #C, A", "Copia o conteúdo do valor da memória no endereço que está no registrador C para o acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[simulator.Reg[2]];
            };
            return instruction;
        }
        public static Instruction MOV_DRAMD_A() {
            Instruction instruction = new Instruction("MOV #D, A", "Copia o conteúdo do valor da memória no endereço que está no registrador D para o acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[simulator.Reg[3]];
            };
            return instruction;
        }
        public static Instruction MOV_DRAME_A() {
            Instruction instruction = new Instruction("MOV #E, A", "Copia o conteúdo do valor da memória no endereço que está no registrador E para o acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[simulator.Reg[4]];
            };
            return instruction;
        }
        public static Instruction MOV_A_DRAMB() {
            Instruction instruction = new Instruction("MOV A, #B", "Copia o conteúdo do acumulador para a memória no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[1]] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_DRAMC() {
            Instruction instruction = new Instruction("MOV A, #C", "Copia o conteúdo do acumulador para a memória no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[2]] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_DRAMD() {
            Instruction instruction = new Instruction("MOV A, #D", "Copia o conteúdo do acumulador para a memória no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[3]] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_DRAME() {
            Instruction instruction = new Instruction("MOV A, #E", "Copia o conteúdo do acumulador para a memória no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.MOV;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[simulator.Reg[4]] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction INC_A_A() {
            Instruction instruction = new Instruction("INC A, A", "Incrementa o valor do acumulador em 1 e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.A_A;
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_B() {
            Instruction instruction = new Instruction("INC A, B", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador B.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_C() {
            Instruction instruction = new Instruction("INC A, C", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador C.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_D() {
            Instruction instruction = new Instruction("INC A, D", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador D.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_E() {
            Instruction instruction = new Instruction("INC A, E", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador E.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_RAM(byte address) {
            Instruction instruction = new Instruction("INC A, #" + Helpers.ToHex(address), "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço " + Helpers.ToHex(address) + ".", 2);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_RAM;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT1() {
            Instruction instruction = new Instruction("INC A, OUT1", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT1.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT2() {
            Instruction instruction = new Instruction("INC A, OUT2", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT2.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT3() {
            Instruction instruction = new Instruction("INC A, OUT3", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT3.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT0() {
            Instruction instruction = new Instruction("INC A, OUT0", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT0.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_OUT;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_B_A() {
            Instruction instruction = new Instruction("INC B, A", "Incrementa o valor do registrador B em 1 e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[1] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_C_A() {
            Instruction instruction = new Instruction("INC C, A", "Incrementa o valor do registrador C em 1 e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[2] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_D_A() {
            Instruction instruction = new Instruction("INC D, A", "Incrementa o valor do registrador D em 1 e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[3] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_E_A() {
            Instruction instruction = new Instruction("INC E, A", "Incrementa o valor do registrador E em 1 e o resultado é colocado no acumulador.", 1);
            instruction.OperationEnum = Operation_enum.REG_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[4] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_RAM_A(byte address) {
            Instruction instruction = new Instruction("INC #" + Helpers.ToHex(address) + ", A", "Incrementa o valor da memória no endreço " + Helpers.ToHex(address) + " em 1 e o resultado é colocado na memória no acumulador.", 2);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.RAM_A;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[address] + 1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN1_A() {
            Instruction instruction = new Instruction("INC IN1, A", "Incrementa da entrada IN1 em 1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[1] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN2_A() {
            Instruction instruction = new Instruction("INC IN2, A", "Incrementa da entrada IN2 em 1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[2] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN3_A() {
            Instruction instruction = new Instruction("INC IN3, A", "Incrementa da entrada IN3 em 1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[3] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN0_A() {
            Instruction instruction = new Instruction("INC IN0, A", "Incrementa da entrada IN3 em 1 e o resultado é colocado no acumulador.", 1);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.IN_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[0] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_A(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", A", "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_A;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_B(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", B", "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_C(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", C", "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_D(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", D", "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_E(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", E", "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_REG;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.RomValue = data;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Incrementa o valor " + Helpers.ToHex(data) + " em 1 e o resultado é colocado na memória no endereço " + Helpers.ToHex(address) + ".", 4);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.ROM_RAM;
            instruction.RomValue = data;
            instruction.RamAddress = address;
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_DRAMB_A() {
            Instruction instruction = new Instruction("INC #B, A", "Incrementa o valor da memória no endreço que está no registrador B em 1 e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[simulator.Reg[1]] + 1;
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_DRAMC_A() {
            Instruction instruction = new Instruction("INC #C, A", "Incrementa o valor da memória no endreço que está no registrador C em 1 e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[simulator.Reg[2]] + 1;
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_DRAMD_A() {
            Instruction instruction = new Instruction("INC #D, A", "Incrementa o valor da memória no endreço que está no registrador D em 1 e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[simulator.Reg[3]] + 1;
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_DRAME_A() {
            Instruction instruction = new Instruction("INC #E, A", "Incrementa o valor da memória no endreço que está no registrador E em 1 e o resultado é colocado na memória no acumulador.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.DRAM_A;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[simulator.Reg[4]] + 1;
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_DRAMB() {
            Instruction instruction = new Instruction("INC A, #B", "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço que está no registrador B.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.B;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.RAM[simulator.Reg[1]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[1]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_DRAMC() {
            Instruction instruction = new Instruction("INC A, #C", "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço que está no registrador C.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.C;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.RAM[simulator.Reg[2]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[2]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_DRAMD() {
            Instruction instruction = new Instruction("INC A, #D", "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço que está no registrador D.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.D;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.RAM[simulator.Reg[3]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[3]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_DRAME() {
            Instruction instruction = new Instruction("INC A, #E", "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço que está no registrador E.", 3);
            instruction.InstructionEnum = Instruction_enum.INC;
            instruction.OperationEnum = Operation_enum.A_DRAM;
            instruction.RegistrerEnum = Registrer_enum.E;
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + 1;
                simulator.RAM[simulator.Reg[4]] = (byte)val;
                simulator.Flag_Z = simulator.RAM[simulator.Reg[4]] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction JMP(string label) {
            Instruction instruction = new Instruction("JMP " + label, "Pula para o label " + label + ".", 4);
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address / 256);
                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address);
                simulator.NextInstruction = instruction.Address;
                ++simulator.PointerStack;
                ++simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction JMPC(string label) {
            Instruction instruction = new Instruction("JMPC " + label, "Pula para o label " + label + " caso a flag carry esteja ligada.", 4);
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                if (simulator.Flag_C) {
                    --simulator.PointerStack;
                    simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address / 256);
                    --simulator.PointerStack;
                    simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address);
                    simulator.NextInstruction = instruction.Address;
                    ++simulator.PointerStack;
                    ++simulator.PointerStack;
                }
            };
            return instruction;
        }
        public static Instruction JMPZ(string label) {
            Instruction instruction = new Instruction("JMPZ " + label, "Pula para o label " + label + " caso a flag zero esteja ligada.", 4);
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                if (simulator.Flag_Z) {
                    --simulator.PointerStack;
                    simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address / 256);
                    --simulator.PointerStack;
                    simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address);
                    simulator.NextInstruction = instruction.Address;
                    ++simulator.PointerStack;
                    ++simulator.PointerStack;
                }
            };
            return instruction;
        }
        public static Instruction CALL(string label) {
            Instruction instruction = new Instruction("CALL " + label, "Chama o procedimento no label " + label + ".", 4);
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                int next = simulator.NextInstruction + instruction.Size;

                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(next / 256);
                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(next);

                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address / 256);
                --simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = (byte)(instruction.Address);

                simulator.NextInstruction = instruction.Address;

                ++simulator.PointerStack;
                ++simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction RET() {
            Instruction instruction = new Instruction("RET", "Retorno do procedimento.", 3);
            instruction.Function = delegate (Simulator simulator) {
                int next = 0;

                next = simulator.Stack[simulator.PointerStack];
                ++simulator.PointerStack;
                next = simulator.Stack[simulator.PointerStack] * 256;
                ++simulator.PointerStack;

                simulator.NextInstruction = next;
            };
            return instruction;
        }
        public static Instruction PUSHA() {
            Instruction instruction = new Instruction("PUSHA", "Coloca o registrador acumulador na pilha.", 3);
            instruction.Function = delegate (Simulator simulator) {
                ++simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction POPA() {
            Instruction instruction = new Instruction("POPA", "Tira um valor da pilha e coloca no acumulador.", 3);
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Stack[simulator.PointerStack];
                --simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction PUSH_B() {
            Instruction instruction = new Instruction("PUSH B", "Coloca o registrador B na pilha.", 3);
            instruction.Function = delegate (Simulator simulator) {
                ++simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = simulator.Reg[1];
            };
            return instruction;
        }
        public static Instruction PUSH_C() {
            Instruction instruction = new Instruction("PUSH C", "Coloca o registrador C na pilha.", 3);
            instruction.Function = delegate (Simulator simulator) {
                ++simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = simulator.Reg[2];
            };
            return instruction;
        }
        public static Instruction PUSH_D() {
            Instruction instruction = new Instruction("PUSH D", "Coloca o registrador D na pilha.", 3);
            instruction.Function = delegate (Simulator simulator) {
                ++simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = simulator.Reg[3];
            };
            return instruction;
        }
        public static Instruction PUSH_E() {
            Instruction instruction = new Instruction("PUSH E", "Coloca o registrador E na pilha.", 3);
            instruction.Function = delegate (Simulator simulator) {
                ++simulator.PointerStack;
                simulator.Stack[simulator.PointerStack] = simulator.Reg[4];
            };
            return instruction;
        }

        public static Instruction POP_B() {
            Instruction instruction = new Instruction("POP B", "Tira um valor da pilha e coloca no registrador B.", 3);
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = simulator.Stack[simulator.PointerStack];
                --simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction POP_C() {
            Instruction instruction = new Instruction("POP C", "Tira um valor da pilha e coloca no registrador C.", 3);
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = simulator.Stack[simulator.PointerStack];
                --simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction POP_D() {
            Instruction instruction = new Instruction("POP D", "Tira um valor da pilha e coloca no registrador D.", 3);
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = simulator.Stack[simulator.PointerStack];
                --simulator.PointerStack;
            };
            return instruction;
        }
        public static Instruction POP_E() {
            Instruction instruction = new Instruction("POP E", "Tira um valor da pilha e coloca no registrador E.", 3);
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = simulator.Stack[simulator.PointerStack];
                --simulator.PointerStack;
            };
            return instruction;
        }



    }
}
