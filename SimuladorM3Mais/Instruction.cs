﻿using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public delegate void Function(Simulator simulator);

    public class Instruction {



        public string Text = "";
        public string Description = "";
        public Function Function = delegate(Simulator sim) { };
        public int Bytes = 1;
        public int[] Frames = null;

        public int Address = 0; //JMP somewhere -> JMP #42
        public string Label;

        public Instruction(string text, string description = "", int bytes = 1, int[] frames = null) {
            Text = text;
            Description = description;
            Bytes = bytes;
            Frames = frames;
        }

        public override string ToString() {
            return Text;
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
        public static Instruction ADD_A_RAM(byte address) {
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
        public static Instruction ADD_A_OUT0() {
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
        public static Instruction ADD_RAM_A(byte address) {
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
        public static Instruction ADD_IN0_A() {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_A(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_B(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_C(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_D(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_E(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction ADD_ROM_RAM(byte data, byte address) {
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
        public static Instruction SUB_A_RAM(byte address) {
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
        public static Instruction SUB_A_OUT0() {
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
        public static Instruction SUB_RAM_A(byte address) {
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
        public static Instruction SUB_IN0_A() {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_A(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_B(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_C(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_D(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_E(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction SUB_ROM_RAM(byte data, byte address) {
            throw new NotImplementedException();
        }
        public static Instruction AND_A_A() {
            Instruction instruction = new Instruction("AND A, A", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_B() {
            Instruction instruction = new Instruction("AND A, B", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador B.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_C() {
            Instruction instruction = new Instruction("AND A, C", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador C.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_D() {
            Instruction instruction = new Instruction("AND A, D", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador D.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_E() {
            Instruction instruction = new Instruction("AND A, E", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado no registrador E.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_RAM(byte address) {
            Instruction instruction = new Instruction("AND A, #" + address, "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço "+address+".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT0() {
            Instruction instruction = new Instruction("AND A, OUT0", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT1() {
            Instruction instruction = new Instruction("AND A, OUT1", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT2() {
            Instruction instruction = new Instruction("AND A, OUT2", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_A_OUT3() {
            Instruction instruction = new Instruction("AND A, OUT3", "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_B_A() {
            Instruction instruction = new Instruction("AND B, A", "Faz a operação AND do registrador B com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_C_A() {
            Instruction instruction = new Instruction("AND C, A", "Faz a operação AND do registrador C com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_D_A() {
            Instruction instruction = new Instruction("AND E, A", "Faz a operação AND do registrador D com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_E_A() {
            Instruction instruction = new Instruction("AND E, A", "Faz a operação AND do registrador E com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_RAM_A(byte address) {
            Instruction instruction = new Instruction("AND #" + address + ", A", "Faz a operação AND do valor na memória RAM no endereço " + address + " com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] & simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN0_A() {
            Instruction instruction = new Instruction("AND IN0, A", "Faz a operação AND da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN1_A() {
            Instruction instruction = new Instruction("AND IN1, A", "Faz a operação AND da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN2_A() {
            Instruction instruction = new Instruction("AND IN2, A", "Faz a operação AND da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_IN3_A() {
            Instruction instruction = new Instruction("AND IN3, A", "Faz a operação AND da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_A(byte data) {
            Instruction instruction = new Instruction("AND " + data + ", A", "Faz a operação AND do valor " + data + " com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_B(byte data) {
            Instruction instruction = new Instruction("AND " + data + ", B", "Faz a operação AND do valor " + data + " com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_C(byte data) {
            Instruction instruction = new Instruction("AND " + data + ", C", "Faz a operação AND do valor " + data + " com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_D(byte data) {
            Instruction instruction = new Instruction("AND " + data + ", D", "Faz a operação AND do valor " + data + " com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_E(byte data) {
            Instruction instruction = new Instruction("AND " + data + ", E", "Faz a operação AND do valor " + data + " com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("AND " + data + ", #"+address, "Faz a operação AND do valor " + data + " com o acumulador e o resultado é colocado na memória ram no endereço "+address+".", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_A() {
            Instruction instruction = new Instruction("OR A, A", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_B() {
            Instruction instruction = new Instruction("OR A, B", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador B.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_C() {
            Instruction instruction = new Instruction("OR A, C", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador C.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_D() {
            Instruction instruction = new Instruction("OR A, D", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador D.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_E() {
            Instruction instruction = new Instruction("OR A, E", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado no registrador E.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_RAM(byte address) {
            Instruction instruction = new Instruction("OR A, #" + address, "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + address + ".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT0() {
            Instruction instruction = new Instruction("OR A, OUT0", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT1() {
            Instruction instruction = new Instruction("OR A, OUT1", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT2() {
            Instruction instruction = new Instruction("OR A, OUT2", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_A_OUT3() {
            Instruction instruction = new Instruction("OR A, OUT3", "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_B_A() {
            Instruction instruction = new Instruction("OR B, A", "Faz a operação OR do registrador B com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_C_A() {
            Instruction instruction = new Instruction("OR C, A", "Faz a operação OR do registrador C com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_D_A() {
            Instruction instruction = new Instruction("OR E, A", "Faz a operação OR do registrador D com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_E_A() {
            Instruction instruction = new Instruction("OR E, A", "Faz a operação OR do registrador E com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_RAM_A(byte address) {
            Instruction instruction = new Instruction("OR #" + address + ", A", "Faz a operação OR do valor na memória RAM no endereço " + address + " com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] | simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN0_A() {
            Instruction instruction = new Instruction("OR IN0, A", "Faz a operação OR da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN1_A() {
            Instruction instruction = new Instruction("OR IN1, A", "Faz a operação OR da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN2_A() {
            Instruction instruction = new Instruction("OR IN2, A", "Faz a operação OR da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_IN3_A() {
            Instruction instruction = new Instruction("OR IN3, A", "Faz a operação OR da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_A(byte data) {
            Instruction instruction = new Instruction("OR " + data + ", A", "Faz a operação OR do valor " + data + " com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_B(byte data) {
            Instruction instruction = new Instruction("OR " + data + ", B", "Faz a operação OR do valor " + data + " com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_C(byte data) {
            Instruction instruction = new Instruction("OR " + data + ", C", "Faz a operação OR do valor " + data + " com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_D(byte data) {
            Instruction instruction = new Instruction("OR " + data + ", D", "Faz a operação OR do valor " + data + " com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_E(byte data) {
            Instruction instruction = new Instruction("OR " + data + ", E", "Faz a operação OR do valor " + data + " com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("OR " + data + ", #" + address, "Faz a operação OR do valor " + data + " com o acumulador e o resultado é colocado na memória ram no endereço " + address + ".", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_A() {
            Instruction instruction = new Instruction("XOR A, A", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_B() {
            Instruction instruction = new Instruction("XOR A, B", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador B.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_C() {
            Instruction instruction = new Instruction("XOR A, C", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador C.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_D() {
            Instruction instruction = new Instruction("XOR A, D", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador D.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_E() {
            Instruction instruction = new Instruction("XOR A, E", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado no registrador E.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_RAM(byte address) {
            Instruction instruction = new Instruction("XOR A, #" + address, "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + address + ".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT0() {
            Instruction instruction = new Instruction("XOR A, OUT0", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT1() {
            Instruction instruction = new Instruction("XOR A, OUT1", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT2() {
            Instruction instruction = new Instruction("XOR A, OUT2", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_A_OUT3() {
            Instruction instruction = new Instruction("XOR A, OUT3", "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(simulator.Reg[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_B_A() {
            Instruction instruction = new Instruction("XOR B, A", "Faz a operação XOR do registrador B com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[1] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_C_A() {
            Instruction instruction = new Instruction("XOR C, A", "Faz a operação XOR do registrador C com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[2] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_D_A() {
            Instruction instruction = new Instruction("XOR E, A", "Faz a operação XOR do registrador D com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[3] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_E_A() {
            Instruction instruction = new Instruction("XOR E, A", "Faz a operação XOR do registrador E com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[4] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_RAM_A(byte address) {
            Instruction instruction = new Instruction("XOR #" + address + ", A", "Faz a operação XOR do valor na memória RAM no endereço " + address + " com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.Reg[0] ^ simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN0_A() {
            Instruction instruction = new Instruction("XOR IN0, A", "Faz a operação XOR da entrada IN0 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[0] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN1_A() {
            Instruction instruction = new Instruction("XOR IN1, A", "Faz a operação XOR da entrada IN1 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[1] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN2_A() {
            Instruction instruction = new Instruction("XOR IN2, A", "Faz a operação XOR da entrada IN2 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[2] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_IN3_A() {
            Instruction instruction = new Instruction("XOR IN3, A", "Faz a operação XOR da entrada IN3 com o acumulador e o resultado se mantém no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(simulator.In[3] ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_A(byte data) {
            Instruction instruction = new Instruction("XOR " + data + ", A", "Faz a operação XOR do valor " + data + " com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_B(byte data) {
            Instruction instruction = new Instruction("XOR " + data + ", B", "Faz a operação XOR do valor " + data + " com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_C(byte data) {
            Instruction instruction = new Instruction("XOR " + data + ", C", "Faz a operação XOR do valor " + data + " com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_D(byte data) {
            Instruction instruction = new Instruction("XOR " + data + ", D", "Faz a operação XOR do valor " + data + " com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_E(byte data) {
            Instruction instruction = new Instruction("XOR " + data + ", E", "Faz a operação XOR do valor " + data + " com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("XOR " + data + ", #" + address, "Faz a operação XOR do valor " + data + " com o acumulador e o resultado é colocado na memória ram no endereço " + address + ".", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
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
        public static Instruction NOT_A_RAM(byte address) {
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
        public static Instruction NOT_A_OUT0() {
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
        public static Instruction NOT_RAM_A(byte address) {
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
        public static Instruction NOT_IN0_A() {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_A(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_B(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_C(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_D(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_E(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction NOT_ROM_RAM(byte data, byte address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_A() {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_B() {
            Instruction instruction = new Instruction("MOV A, B", "Move o conteúdo do acumulador para o registrador B", 1, new int[]{ 1 });
            instruction.Function = delegate (Simulator simulator) {
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
        public static Instruction MOV_A_RAM(byte address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_A_OUT0() {
            Instruction instruction = new Instruction("MOV A, OUT0", "Move o conteúdo do acumulador para a saída OUT0", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT1() {
            Instruction instruction = new Instruction("MOV A, OUT1", "Move o conteúdo do acumulador para a saída OUT1", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT2() {
            Instruction instruction = new Instruction("MOV A, OUT2", "Move o conteúdo do acumulador para a saída OUT2", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT3() {
            Instruction instruction = new Instruction("MOV A, OUT3", "Move o conteúdo do acumulador para a saída OUT3", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = simulator.Reg[0];
            };
            return instruction;
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
        public static Instruction MOV_RAM_A(byte address) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_IN0_A() {
            Instruction instruction = new Instruction("MOV IN0, A", "Move o valor da entrada IN0 para o acumulador", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[0];
            };
            return instruction;
        }
        public static Instruction MOV_IN1_A() {
            Instruction instruction = new Instruction("MOV IN1, A", "Move o valor da entrada IN1 para o acumulador", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[1];
            };
            return instruction;
        }
        public static Instruction MOV_IN2_A() {
            Instruction instruction = new Instruction("MOV IN2, A", "Move o valor da entrada IN2 para o acumulador", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[2];
            };
            return instruction;
        }
        public static Instruction MOV_IN3_A() {
            Instruction instruction = new Instruction("MOV IN3, A", "Move o valor da entrada IN3 para o acumulador", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[3];
            };
            return instruction;
        }
        public static Instruction MOV_ROM_A(byte data) {
            Instruction instruction = new Instruction("MOV "+data+", A", "Move o valor "+ data +" para o acumulador", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_B(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_C(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_D(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_E(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction MOV_ROM_RAM(byte data, byte address) {
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
        public static Instruction INC_A_RAM(byte address) {
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
        public static Instruction INC_A_OUT0() {
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
        public static Instruction INC_RAM_A(byte address) {
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
        public static Instruction INC_IN0_A() {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_A(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_B(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_C(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_D(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_E(byte data) {
            throw new NotImplementedException();
        }
        public static Instruction INC_ROM_RAM(byte data, byte address) {
            throw new NotImplementedException();
        }
        public static Instruction JMP(string label) {
            Instruction instruction = new Instruction("JMP " + label, "Pula para o label " + label + ".", 4, new int[] { 2, 4 });
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                simulator.NextInstruction = instruction.Address;
            };
            return instruction;
        }
        public static Instruction JMPC(string label) {
            Instruction instruction = new Instruction("JMPC " + label, "Pula para o label " + label + " caso a flag carry esteja ligada.", 4, new int[] { 1, 1, 1, 3 });
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                if (simulator.Flag_C) {
                    simulator.NextInstruction = instruction.Address;
                }
            };
            return instruction;
        }
        public static Instruction JMPZ(string label) {
            Instruction instruction = new Instruction("JMPZ " + label, "Pula para o label " + label + " caso a flag zero esteja ligada.", 4, new int[] { 1, 1, 1, 3 });
            instruction.Label = label;
            instruction.Function = delegate (Simulator simulator) {
                if (simulator.Flag_Z) {
                    simulator.NextInstruction = instruction.Address;
                }
            };
            return instruction;
        }
        public static Instruction CALL(string label) {
            throw new NotImplementedException();
        }
        public static Instruction RET() {
            throw new NotImplementedException();
        }


    }
}