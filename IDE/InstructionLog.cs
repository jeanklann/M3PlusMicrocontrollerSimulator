using System;
using System.Collections.Generic;
using M3PlusMicrocontroller;

namespace IDE {
    public class InstructionLog {
        private List<InstructionLogItem> Items;
        private Dictionary<Instruction, int> InstructionClockSize;
        public InstructionLog() {
            Items = new List<InstructionLogItem>();
            InstructionClockSize = new Dictionary<Instruction, int>();
        }
        public void Clear() {
            Items.Clear();
            InstructionClockSize.Clear();
        }
        public void Add(InstructionLogItem item) {
            if (item.EOI) {
                Instruction instruction = Items[Items.Count - 1].Instruction;
                if (instruction != null) {
                    int size = 0;
                    for (int i = Items.Count - 1; i >= 0; i--) {
                        if (Items[i].Instruction == null) continue;
                        if (instruction == Items[i].Instruction)
                            size++;
                        else
                            break;
                    }
                    if (InstructionClockSize.ContainsKey(instruction)){
                        InstructionClockSize.Remove(instruction);
                    }
                    InstructionClockSize.Add(instruction, size+1);
                }
            }
            Items.Add(item);
        }
        public void Add(string text) {
            Items.Add(new InstructionLogItem(text));
        }
        public void Iniciar() {
            Clear();
            Add("Iniciou a simulação");
        }
        public void Pausar() {
            Add("Pausou a simulação");
        }
        public void Parar() {
            Add("Parou a simulação");
        }
        public void Continuar() {
            Add("Continou a simulação");
        }
        public void PassoDentro() {
            Add("Passo dentro");
        }
        public void PassoFora() {
            Add("Passo fora");
        }
        public override string ToString() {
            int clock = 0;
            string res = "";
            try {
                foreach (InstructionLogItem item in Items) {
                    if (item.Instruction != null) {
                        if (clock == 0) {
                            res += "\r\nInstrução: " + item.Instruction.Text + "\r\n";
                            res += "Tamanho: " + item.Instruction.Size + " byte" + (item.Instruction.Size != 1 ? "s" : "") + "\r\n";
                            if (InstructionClockSize.ContainsKey(item.Instruction) && InstructionClockSize[item.Instruction] > 1) {
                                res += "Clocks necessários: " + InstructionClockSize[item.Instruction] + "\r\n";
                            } else {
                                res += "Não foi possível calcular o número de clocks necessários\r\n";
                            }
                        }
                        res += "Sinais do clock " + (clock + 1) + ": ";
                        res += item.ToString();
                        res += "\r\n";
                        ++clock;
                        if (item.EOI && clock > 3) clock = 0;
                    } else {
                        res += item.ToString();
                    }
                }
            } catch (Exception) {
                res += "Aguarde um momento.";
            }
            return res;
        }
    }
    public class InstructionLogItem {
        public string Text = "";
        public Instruction Instruction;
        public string bus = "00";
        public bool clock = false;
        public bool flagC = false;
        public bool flagZ = false;
        public bool EOI = false;
        public bool ROMrd = false;
        public bool ROMcs = false;
        public bool PCHbus = false;
        public bool PCLbus = false;
        public bool PCHclk = false;
        public bool PCLclk = false;
        public bool DataPCsel = false;
        public bool DIRclk = false;
        public bool SPclk = false;
        public bool SPIncDec = false;
        public bool SPsel = false;
        public bool SPen = false;
        public bool Reset = false;
        public bool ULAbus = false;
        public bool BUFclk = false;
        public bool ACbus = false;
        public bool ACclk = false;
        public bool RGbus = false;
        public bool RGPCclk = false;
        public bool RAMrd = false;
        public bool RAMwr = false;
        public bool RAMcs = false;
        public bool INbus = false;
        public bool OUTclk = false;
        public bool ULAop0 = false;
        public bool ULAop1 = false;
        public bool ULAop2 = false;
        public bool RGPB0 = false;
        public bool RGPB1 = false;
        public InstructionLogItem(Instruction instruction) {
            Instruction = instruction;
        }
        public InstructionLogItem(string text) {
            Text = text;
        }
        public override string ToString() {
            string res = "";
            if (Instruction != null) {
                res += "Bus: " + bus + ", ";
                res += "FlagC: " + (flagC ? "1" : "0") + ", ";
                res += "FlagZ: " + (flagZ ? "1" : "0") + ", ";
                res += "EOI: " + (EOI ? "1" : "0") + ", ";
                res += "ROMrd: " + (ROMrd ? "1" : "0") + ", ";
                res += "ROMcs: " + (ROMcs ? "1" : "0") + ", ";
                res += "PCHbus: " + (PCHbus ? "1" : "0") + ", ";
                res += "PCLbus: " + (PCLbus ? "1" : "0") + ", ";
                res += "PCHclk: " + (PCHclk ? "1" : "0") + ", ";
                res += "PCLclk: " + (PCLclk ? "1" : "0") + ", ";
                res += "Data/PC sel: " + (DataPCsel ? "1" : "0") + ", ";
                res += "DIRclk: " + (DIRclk ? "1" : "0") + ", ";
                res += "SPclk: " + (SPclk ? "1" : "0") + ", ";
                res += "SPInc/Dec: " + (SPIncDec ? "1" : "0") + ", ";
                res += "SPsel: " + (SPsel ? "1" : "0") + ", ";
                res += "SPen: " + (SPen ? "1" : "0") + ", ";
                res += "Reset: " + (Reset ? "1" : "0") + ", ";
                res += "ULAbus: " + (ULAbus ? "1" : "0") + ", ";
                res += "BUFclk: " + (BUFclk ? "1" : "0") + ", ";
                res += "ACbus: " + (ACbus ? "1" : "0") + ", ";
                res += "ACclk: " + (ACclk ? "1" : "0") + ", ";
                res += "RGbus: " + (RGbus ? "1" : "0") + ", ";
                res += "RG/PCclk: " + (RGPCclk ? "1" : "0") + ", ";
                res += "RAMrd: " + (RAMrd ? "1" : "0") + ", ";
                res += "RAMwr: " + (RAMwr ? "1" : "0") + ", ";
                res += "RAMcs: " + (RAMcs ? "1" : "0") + ", ";
                res += "INbus: " + (INbus ? "1" : "0") + ", ";
                res += "OUTclk: " + (OUTclk ? "1" : "0") + ", ";
                res += "ULAop0: " + (ULAop0 ? "1" : "0") + ", ";
                res += "ULAop1: " + (ULAop1 ? "1" : "0") + ", ";
                res += "ULAop2: " + (ULAop2 ? "1" : "0") + ", ";
                res += "RGPB0: " + (RGPB0 ? "1" : "0") + ", ";
                res += "RGPB1: " + (RGPB1 ? "1" : "0") + "\r\n";
            } else {
                res += Text + "\r\n";
            }
            return res;
        }
    }
}
