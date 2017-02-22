using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public delegate void Function(Simulator simulator, Instruction instruction);

    public class Instruction {



        public string Text = "";
        public string Description = "";
        public Function Function;
        public int Bytes = 0;
        public int Frames = 0;

        public byte Value = 0; //MOV 42,A
        public int Address = 0; //JMP 42



        public Instruction(string text, string description = "", int bytes = 1, int frames = 1) {
            Text = text;
            Description = description;
            Bytes = bytes;
            Frames = frames;
        }

        public static Instruction ADD_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction AND_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction AND_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction AND_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction OR_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction OR_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction OR_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction OR_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction XOR_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_B() {
            Instruction instruction = new Instruction("MOV A, B", "Move o conteúdo do acumulador para o registrador B", 1, 3);
            instruction.Function = delegate (Simulator simulator, Instruction thisInstruction) {
                simulator.Reg[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_A(int data) {
            Instruction instruction = new Instruction("MOV "+data+", A", "Move o valor "+ data +" para o acumulador", 3, 3);
            instruction.Function = delegate (Simulator simulator, Instruction thisInstruction) {
                simulator.Reg[0] = thisInstruction.Value;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_B() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_C() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_D() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_E() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_RAM(int address) {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_OUT1() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_OUT2() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_OUT3() {
            throw new NotImplementedException();
        }
        public static Instruction INC_A_OUT4() {
            throw new NotImplementedException();
        }
        public static Instruction INC_B_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_C_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_D_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_E_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_RAM_A(int address) {
            throw new NotImplementedException();
        }
        public static Instruction INC_IN1_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_IN2_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_IN3_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_IN4_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_A(int data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_B(int data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_C(int data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_D(int data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_E(int data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_RAM(int data, int address) {
            throw new NotImplementedException();
        }

    }
}
