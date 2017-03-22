﻿using System;
using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller {
    public delegate void Function(Simulator simulator);
    
    public class Instruction {
        public string Text = "";
        public string Description = "";
        public Function Function = delegate(Simulator sim) { };
        public int Size = 1;
        public int[] Frames = null;
        public bool HasBreakpoint = false;

        public int Address = 0; //JMP somewhere -> JMP #42
        public string Label;

        public Instruction(string text, string description = "", int size = 1, int[] frames = null) {
            Text = text;
            Description = description;
            Size = size;
            Frames = frames;
        }

        public override string ToString() {
            return Text;
        }

        public static Instruction ADD_A_A() {
            Instruction instruction = new Instruction("ADD A, A", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_B() {
            Instruction instruction = new Instruction("ADD A, B", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador B.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_C() {
            Instruction instruction = new Instruction("ADD A, C", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador C.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_D() {
            Instruction instruction = new Instruction("ADD A, D", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador D.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_E() {
            Instruction instruction = new Instruction("ADD A, E", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado no registrador E.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_RAM(byte address) {
            Instruction instruction = new Instruction("ADD A, #"+Helpers.ToHex(address), "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço "+Helpers.ToHex(address)+".", 2, new int[] { 1, 2});
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT1() {
            Instruction instruction = new Instruction("ADD A, OUT1", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT2() {
            Instruction instruction = new Instruction("ADD A, OUT2", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT3() {
            Instruction instruction = new Instruction("ADD A, OUT3", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_A_OUT0() {
            Instruction instruction = new Instruction("ADD A, OUT0", "Adiciona o valor do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[0];
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_B_A() {
            Instruction instruction = new Instruction("ADD B, A", "Adiciona o valor do acumulador com o registrador B e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_C_A() {
            Instruction instruction = new Instruction("ADD C, A", "Adiciona o valor do acumulador com o registrador C e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_D_A() {
            Instruction instruction = new Instruction("ADD D, A", "Adiciona o valor do acumulador com o registrador D e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_E_A() {
            Instruction instruction = new Instruction("ADD E, A", "Adiciona o valor do acumulador com o registrador E e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.Reg[4];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_RAM_A(byte address) {
            Instruction instruction = new Instruction("ADD #" + Helpers.ToHex(address) + ", A", "Adiciona o valor do acumulador com o valor da memória no endreço "+Helpers.ToHex(address)+"e o resultado é colocado na memória no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.RAM[address];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN1_A() {
            Instruction instruction = new Instruction("ADD IN1, A", "Adiciona o valor do acumulador com o valor da entrada IN1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN2_A() {
            Instruction instruction = new Instruction("ADD IN2, A", "Adiciona o valor do acumulador com o valor da entrada IN2 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN3_A() {
            Instruction instruction = new Instruction("ADD IN3, A", "Adiciona o valor do acumulador com o valor da entrada IN3 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_IN0_A() {
            Instruction instruction = new Instruction("ADD IN0, A", "Adiciona o valor do acumulador com o valor da entrada IN0 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + simulator.In[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_A(byte data) {
            Instruction instruction = new Instruction("ADD "+ Helpers.ToHex(data) + ", A", "Adiciona o valor do acumulador com o valor "+Helpers.ToHex(data)+" e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_B(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", B", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador B.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_C(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", C", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador C.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_D(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", D", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador D.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_E(byte data) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", E", "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador E.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction ADD_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("ADD " + Helpers.ToHex(data) + ", #"+Helpers.ToHex(address), "Adiciona o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado na memória no endereço "+Helpers.ToHex(address)+".", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] + data;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction SUB_A_A() {
            Instruction instruction = new Instruction("SUB A, A", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_B() {
            Instruction instruction = new Instruction("SUB A, B", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador B.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_C() {
            Instruction instruction = new Instruction("SUB A, C", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador C.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_D() {
            Instruction instruction = new Instruction("SUB A, D", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador D.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_E() {
            Instruction instruction = new Instruction("SUB A, E", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado no registrador E.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_RAM(byte address) {
            Instruction instruction = new Instruction("SUB A, #" + Helpers.ToHex(address), "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na memória no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT1() {
            Instruction instruction = new Instruction("SUB A, OUT1", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT2() {
            Instruction instruction = new Instruction("SUB A, OUT2", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT3() {
            Instruction instruction = new Instruction("SUB A, OUT3", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_A_OUT0() {
            Instruction instruction = new Instruction("SUB A, OUT0", "Subtrai o valor do acumulador com o acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[0];
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_B_A() {
            Instruction instruction = new Instruction("SUB B, A", "Subtrai o valor do acumulador com o registrador B e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_C_A() {
            Instruction instruction = new Instruction("SUB C, A", "Subtrai o valor do acumulador com o registrador C e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_D_A() {
            Instruction instruction = new Instruction("SUB D, A", "Subtrai o valor do acumulador com o registrador D e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_E_A() {
            Instruction instruction = new Instruction("SUB E, A", "Subtrai o valor do acumulador com o registrador E e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.Reg[4];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_RAM_A(byte address) {
            Instruction instruction = new Instruction("SUB #" + Helpers.ToHex(address) + ", A", "Subtrai o valor do acumulador com o valor da memória no endreço " + Helpers.ToHex(address)+"e o resultado é colocado na memória no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.RAM[address];
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN1_A() {
            Instruction instruction = new Instruction("SUB IN1, A", "Subtrai o valor do acumulador com o valor da entrada IN1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[1];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN2_A() {
            Instruction instruction = new Instruction("SUB IN2, A", "Subtrai o valor do acumulador com o valor da entrada IN2 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[2];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN3_A() {
            Instruction instruction = new Instruction("SUB IN3, A", "Subtrai o valor do acumulador com o valor da entrada IN3 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[3];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_IN0_A() {
            Instruction instruction = new Instruction("SUB IN0, A", "Subtrai o valor do acumulador com o valor da entrada IN0 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - simulator.In[0];
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_A(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", A", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_B(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", B", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador B.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_C(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", C", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador C.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_D(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", D", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador D.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_E(byte data) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", E", "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador E.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
        }
        public static Instruction SUB_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("SUB " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Subtrai o valor do acumulador com o valor " + Helpers.ToHex(data)+" e o resultado é colocado na memória no endereço " + Helpers.ToHex(address)+".", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0] - data;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val < 0;
            };
            return instruction;
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
            Instruction instruction = new Instruction("AND A, #" + Helpers.ToHex(address), "Faz a operação AND do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço "+Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("AND #" + Helpers.ToHex(address) + ", A", "Faz a operação AND do valor na memória RAM no endereço " + Helpers.ToHex(address)+" com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", A", "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_B(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", B", "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_C(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", C", "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_D(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", D", "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_E(byte data) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", E", "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data & simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction AND_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("AND " + Helpers.ToHex(data) + ", #"+Helpers.ToHex(address), "Faz a operação AND do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado na memória ram no endereço "+Helpers.ToHex(address)+".", 3, new int[] { 1, 1, 2 });
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
            Instruction instruction = new Instruction("OR A, #" + Helpers.ToHex(address), "Faz a operação OR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("OR #" + Helpers.ToHex(address) + ", A", "Faz a operação OR do valor na memória RAM no endereço " + Helpers.ToHex(address)+" com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", A", "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_B(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", B", "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_C(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", C", "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_D(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", D", "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_E(byte data) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", E", "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data | simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction OR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("OR " + Helpers.ToHex(data) + ", #" + Helpers.ToHex(address), "Faz a operação OR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado na memória ram no endereço " + Helpers.ToHex(address)+".", 3, new int[] { 1, 1, 2 });
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
            Instruction instruction = new Instruction("XOR A, #" + Helpers.ToHex(address), "Faz a operação XOR do acumulador com o acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("XOR #" + Helpers.ToHex(address)+", A", "Faz a operação XOR do valor na memória RAM no endereço " + Helpers.ToHex(address)+" com o acumulador e o resultado se mantém no acumulador.", 2, new int[] { 1, 2 });
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
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", A", "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado se mantém no acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_B(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", B", "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_C(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", C", "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_D(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", D", "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_E(byte data) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", E", "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado no registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction XOR_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("XOR " + Helpers.ToHex(data)+", #" + Helpers.ToHex(address), "Faz a operação XOR do valor " + Helpers.ToHex(data)+" com o acumulador e o resultado é colocado na memória ram no endereço " + Helpers.ToHex(address)+".", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(data ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_A() {
            Instruction instruction = new Instruction("NOT A, A", "Faz a operação NOT no acumulador e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_B() {
            Instruction instruction = new Instruction("NOT A, B", "Faz a operação NOT no acumulador e o resultado é colocado no registrador B.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_C() {
            Instruction instruction = new Instruction("NOT A, C", "Faz a operação NOT no acumulador e o resultado é colocado no registrador C.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_D() {
            Instruction instruction = new Instruction("NOT A, D", "Faz a operação NOT no acumulador e o resultado é colocado no registrador D.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_E() {
            Instruction instruction = new Instruction("NOT A, E", "Faz a operação NOT no acumulador e o resultado é colocado no registrador E.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_RAM(byte address) {
            Instruction instruction = new Instruction("NOT A, #"+Helpers.ToHex(address), "Faz a operação NOT no acumulador e o resultado é colocado na memória RAM no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT1() {
            Instruction instruction = new Instruction("NOT A, OUT1", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT1.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT2() {
            Instruction instruction = new Instruction("NOT A, OUT2", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT2.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT3() {
            Instruction instruction = new Instruction("NOT A, OUT3", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT3.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_A_OUT0() {
            Instruction instruction = new Instruction("NOT A, OUT0", "Faz a operação NOT no acumulador e o resultado é colocado na saída OUT0.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = (byte)(255 ^ simulator.Reg[0]);
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_B_A() {
            Instruction instruction = new Instruction("NOT B, A", "Faz a operação NOT no registrador B e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[1]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_C_A() {
            Instruction instruction = new Instruction("NOT C, A", "Faz a operação NOT no registrador C e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[2]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_D_A() {
            Instruction instruction = new Instruction("NOT D, A", "Faz a operação NOT no registrador D e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[3]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_E_A() {
            Instruction instruction = new Instruction("NOT E, A", "Faz a operação NOT no registrador E e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.Reg[4]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_RAM_A(byte address) {
            Instruction instruction = new Instruction("NOT #"+Helpers.ToHex(address)+", A", "Faz a operação NOT na memória no endereço "+Helpers.ToHex(address)+" e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.RAM[address]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN1_A() {
            Instruction instruction = new Instruction("NOT IN1, A", "Faz a operação NOT na entrada IN1 e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[1]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN2_A() {
            Instruction instruction = new Instruction("NOT IN2, A", "Faz a operação NOT na entrada IN2 e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[2]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN3_A() {
            Instruction instruction = new Instruction("NOT IN3, A", "Faz a operação NOT na entrada IN3 e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[3]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_IN0_A() {
            Instruction instruction = new Instruction("NOT IN0, A", "Faz a operação NOT na entrada IN0 e o resultado é colocado no acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ simulator.In[0]);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_A(byte data) {
            Instruction instruction = new Instruction("NOT "+Helpers.ToHex(data)+", A", "Faz a operação NOT do valor "+Helpers.ToHex(data)+" e o resultado é colocado no acumulador.", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_B(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data)+", B", "Faz a operação NOT do valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador B.", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_C(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data)+", C", "Faz a operação NOT do valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador C.", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_D(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data)+", D", "Faz a operação NOT do valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador D.", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_E(byte data) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data)+", E", "Faz a operação NOT do valor " + Helpers.ToHex(data)+" e o resultado é colocado no registrador E.", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction NOT_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("NOT " + Helpers.ToHex(data)+", #"+ Helpers.ToHex(address), "Faz a operação NOT do valor " + Helpers.ToHex(data)+" e o resultado é colocado na memória no endereço "+Helpers.ToHex(address)+".", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = (byte)(255 ^ data);
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = false;
            };
            return instruction;
        }
        public static Instruction MOV_A_A() {
            Instruction instruction = new Instruction("MOV A, B", "Copia o conteúdo do acumulador para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_B() {
            Instruction instruction = new Instruction("MOV A, B", "Copia o conteúdo do acumulador para o registrador B.", 1, new int[]{ 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_C() {
            Instruction instruction = new Instruction("MOV A, C", "Copia o conteúdo do acumulador para o registrador C.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_D() {
            Instruction instruction = new Instruction("MOV A, D", "Copia o conteúdo do acumulador para o registrador D.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_E() {
            Instruction instruction = new Instruction("MOV A, E", "Copia o conteúdo do acumulador para o registrador E.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_RAM(byte address) {
            Instruction instruction = new Instruction("MOV A, #"+Helpers.ToHex(address), "Copia o conteúdo do acumulador para a memória no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT1() {
            Instruction instruction = new Instruction("MOV A, OUT1", "Copia o conteúdo do acumulador para a saída OUT1.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[1] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT2() {
            Instruction instruction = new Instruction("MOV A, OUT2", "Copia o conteúdo do acumulador para a saída OUT2.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[2] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT3() {
            Instruction instruction = new Instruction("MOV A, OUT3", "Copia o conteúdo do acumulador para a saída OUT3.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[3] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_A_OUT0() {
            Instruction instruction = new Instruction("MOV A, OUT0", "Copia o conteúdo do acumulador para a saída OUT0.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Out[0] = simulator.Reg[0];
            };
            return instruction;
        }
        public static Instruction MOV_B_A() {
            Instruction instruction = new Instruction("MOV B, A", "Copia o conteúdo do registrador B para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[1];
            };
            return instruction;
        }
        public static Instruction MOV_C_A() {
            Instruction instruction = new Instruction("MOV C, A", "Copia o conteúdo do registrador C para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[2];
            };
            return instruction;
        }
        public static Instruction MOV_D_A() {
            Instruction instruction = new Instruction("MOV D, A", "Copia o conteúdo do registrador D para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[3];
            };
            return instruction;
        }
        public static Instruction MOV_E_A() {
            Instruction instruction = new Instruction("MOV E, A", "Copia o conteúdo do registrador E para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.Reg[4];
            };
            return instruction;
        }
        public static Instruction MOV_RAM_A(byte address) {
            Instruction instruction = new Instruction("MOV #"+Helpers.ToHex(address)+", A", "Copia o conteúdo do valor da memória no endereço "+Helpers.ToHex(address)+" para o acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.RAM[address];
            };
            return instruction;
        }
        public static Instruction MOV_IN1_A() {
            Instruction instruction = new Instruction("MOV IN1, A", "Copia o valor da entrada IN1 para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[1];
            };
            return instruction;
        }
        public static Instruction MOV_IN2_A() {
            Instruction instruction = new Instruction("MOV IN2, A", "Copia o valor da entrada IN2 para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[2];
            };
            return instruction;
        }
        public static Instruction MOV_IN3_A() {
            Instruction instruction = new Instruction("MOV IN3, A", "Copia o valor da entrada IN3 para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[3];
            };
            return instruction;
        }
        public static Instruction MOV_IN0_A() {
            Instruction instruction = new Instruction("MOV IN0, A", "Copia o valor da entrada IN0 para o acumulador.", 1, new int[] { 1 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = simulator.In[0];
            };
            return instruction;
        }
        public static Instruction MOV_ROM_A(byte data) {
            Instruction instruction = new Instruction("MOV "+Helpers.ToHex(data)+", A", "Copia o valor " + data +" para o acumulador.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[0] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_B(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data)+", B", "Copia o valor " + Helpers.ToHex(data)+" para o registrador B.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[1] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_C(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data)+", C", "Copia o valor " + Helpers.ToHex(data)+" para o registrador C.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[2] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_D(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data)+", D", "Copia o valor " + Helpers.ToHex(data)+" para o registrador D.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[3] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_E(byte data) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data)+", E", "Copia o valor " + Helpers.ToHex(data)+" para o registrador E.", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.Reg[4] = data;
            };
            return instruction;
        }
        public static Instruction MOV_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("MOV " + Helpers.ToHex(data)+", #"+Helpers.ToHex(address), "Copia o valor " + Helpers.ToHex(data)+" para a memória no endereço "+Helpers.ToHex(address)+".", 3, new int[] { 1, 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                simulator.RAM[address] = data;
            };
            return instruction;
        }
        public static Instruction INC_A_A() {
            Instruction instruction = new Instruction("INC A, A", "Incrementa o valor do acumulador em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_B() {
            Instruction instruction = new Instruction("INC A, B", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador B.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_C() {
            Instruction instruction = new Instruction("INC A, C", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador C.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_D() {
            Instruction instruction = new Instruction("INC A, D", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador D.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_E() {
            Instruction instruction = new Instruction("INC A, E", "Incrementa o valor do acumulador em 1 e o resultado é colocado no registrador E.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_RAM(byte address) {
            Instruction instruction = new Instruction("INC A, #" + Helpers.ToHex(address), "Incrementa o valor do acumulador em 1 e o resultado é colocado na memória no endereço " + Helpers.ToHex(address)+".", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT1() {
            Instruction instruction = new Instruction("INC A, OUT1", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT1.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Out[1] = (byte)val;
                simulator.Flag_Z = simulator.Out[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT2() {
            Instruction instruction = new Instruction("INC A, OUT2", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT2.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Out[2] = (byte)val;
                simulator.Flag_Z = simulator.Out[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT3() {
            Instruction instruction = new Instruction("INC A, OUT3", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT3.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Out[3] = (byte)val;
                simulator.Flag_Z = simulator.Out[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_A_OUT0() {
            Instruction instruction = new Instruction("INC A, OUT0", "Incrementa o valor do acumulador em 1 e o resultado é colocado na saída OUT0.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[0]+1;
                simulator.Out[0] = (byte)val;
                simulator.Flag_Z = simulator.Out[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_B_A() {
            Instruction instruction = new Instruction("INC B, A", "Incrementa o valor do registrador B em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[1]+1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_C_A() {
            Instruction instruction = new Instruction("INC C, A", "Incrementa o valor do registrador C em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[2]+1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_D_A() {
            Instruction instruction = new Instruction("INC D, A", "Incrementa o valor do registrador D em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[3]+1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_E_A() {
            Instruction instruction = new Instruction("INC E, A", "Incrementa o valor do registrador E em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.Reg[4]+1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_RAM_A(byte address) {
            Instruction instruction = new Instruction("INC #" + Helpers.ToHex(address)+", A", "Incrementa o valor da memória no endreço " + Helpers.ToHex(address)+" em 1 e o resultado é colocado na memória no acumulador.", 2, new int[] { 1, 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.RAM[address]+1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN1_A() {
            Instruction instruction = new Instruction("INC IN1, A", "Incrementa da entrada IN1 em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[1] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN2_A() {
            Instruction instruction = new Instruction("INC IN2, A", "Incrementa da entrada IN2 em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[2] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN3_A() {
            Instruction instruction = new Instruction("INC IN3, A", "Incrementa da entrada IN3 em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[3] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_IN0_A() {
            Instruction instruction = new Instruction("INC IN0, A", "Incrementa da entrada IN3 em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = simulator.In[0] + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_A(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", A", "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado no acumulador.", 1, new int[] { 2 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[0] = (byte)val;
                simulator.Flag_Z = simulator.Reg[0] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_B(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", B", "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado no registrador B.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[1] = (byte)val;
                simulator.Flag_Z = simulator.Reg[1] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_C(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", C", "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado no registrador C.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[2] = (byte)val;
                simulator.Flag_Z = simulator.Reg[2] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_D(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", D", "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado no registrador D.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[3] = (byte)val;
                simulator.Flag_Z = simulator.Reg[3] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_E(byte data) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", E", "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado no registrador E.", 1, new int[] { 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.Reg[4] = (byte)val;
                simulator.Flag_Z = simulator.Reg[4] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
        }
        public static Instruction INC_ROM_RAM(byte data, byte address) {
            Instruction instruction = new Instruction("INC " + Helpers.ToHex(data)+", #" + Helpers.ToHex(address), "Incrementa o valor " + Helpers.ToHex(data)+" em 1 e o resultado é colocado na memória no endereço " + Helpers.ToHex(address)+".", 3, new int[] { 1, 2, 3 });
            instruction.Function = delegate (Simulator simulator) {
                int val = data + 1;
                simulator.RAM[address] = (byte)val;
                simulator.Flag_Z = simulator.RAM[address] == 0;
                simulator.Flag_C = val > 255;
            };
            return instruction;
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
