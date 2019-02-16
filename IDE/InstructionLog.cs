using System;
using System.Collections.Generic;
using M3PlusMicrocontroller;

namespace IDE {
    public class InstructionLog {
        public int ClearCount;
        public List<InstructionLogItem> Items;
        private Dictionary<Instruction, int> _instructionClockSize;
        public InstructionLog() {
            Items = new List<InstructionLogItem>();
            _instructionClockSize = new Dictionary<Instruction, int>();
        }
        public void Clear() {
            Items.Clear();
            _instructionClockSize.Clear();
            ++ClearCount;
        }
        public void Add(InstructionLogItem item) {
            if (item.Eoi) {
                var instruction = Items[Items.Count - 1].Instruction;
                if (instruction != null) {
                    var size = 0;
                    for (var i = Items.Count - 1; i >= 0; i--) {
                        if (Items[i].Instruction == null) continue;
                        if (instruction == Items[i].Instruction) {
                            size++;
                            if (Items[i].Primeira) break;
                        } else
                            break;
                    }
                    if (_instructionClockSize.ContainsKey(instruction)){
                        _instructionClockSize.Remove(instruction);
                    }
                    _instructionClockSize.Add(instruction, size+1);
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
        public List<Instrucao> ToList()
        {
            var clock = 0;
            var res = new List<Instrucao>();
            Instrucao instrucaoAtual = null;
            try
            {
                foreach (var item in Items)
                {
                    if (item.Instruction != null)
                    {
                        if (clock == 0)
                        {
                            instrucaoAtual = new Instrucao();
                            instrucaoAtual.Nome = item.Instruction.Text;
                            instrucaoAtual.QuantidadeBytes = item.Instruction.Size;
                            if (_instructionClockSize.ContainsKey(item.Instruction) && _instructionClockSize[item.Instruction] > 1)
                                instrucaoAtual.ClocksNecessarios = _instructionClockSize[item.Instruction];
                            else
                                instrucaoAtual.ClocksNecessarios = null;
                            res.Add(instrucaoAtual);
                        }
                        instrucaoAtual.Sinais.Add(item.ToList());
                        ++clock;
                        if (item.Eoi && clock > 3) clock = 0;
                    }
                    else
                    {
                        instrucaoAtual = new Instrucao();
                        instrucaoAtual.Texto = item.Text;
                        res.Add(instrucaoAtual);
                        //instrucaoAtual.Sinais.Add(item.ToList());
                    }
                    
                }
            }
            catch (Exception)
            {
            }
            return res;
        }
        public override string ToString() {
            var clock = 0;
            var res = "";
            try {
                foreach (var item in Items) {
                    if (item.Instruction != null) {
                        if (clock == 0) {
                            res += "\r\nInstrução: " + item.Instruction.Text + "\r\n";
                            res += "Tamanho: " + item.Instruction.Size + " byte" + (item.Instruction.Size != 1 ? "s" : "") + "\r\n";
                            if (_instructionClockSize.ContainsKey(item.Instruction) && _instructionClockSize[item.Instruction] > 1) {
                                res += "Clocks necessários: " + _instructionClockSize[item.Instruction] + "\r\n";
                            } else {
                                res += "Não foi possível calcular o número de clocks necessários\r\n";
                            }
                        }
                        res += "Sinais do clock " + (clock + 1) + ": ";
                        res += item.ToString();
                        res += "\r\n";
                        ++clock;
                        if (item.Eoi && clock > 3) clock = 0;
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
    public class Instrucao
    {
        public string Nome;
        public int QuantidadeBytes;
        public int? ClocksNecessarios;
        public string Texto;
        public List<List<CulunaValor>> Sinais = new List<List<CulunaValor>>();
    }
    public class CulunaValor
    {
        public string Coluna;
        public string Valor;
    }
    public class InstructionLogItem {
        public string Text = "";
        public bool Primeira = false;
        public Instruction Instruction;
        public string Bus = "00";
        public bool Clock = false;
        public bool FlagC = false;
        public bool FlagZ = false;
        public bool Eoi = false;
        public bool RoMrd = false;
        public bool RoMcs = false;
        public bool PcHbus = false;
        public bool PcLbus = false;
        public bool PcHclk = false;
        public bool PcLclk = false;
        public bool DataPCsel = false;
        public bool DiRclk = false;
        public bool SPclk = false;
        public bool SpIncDec = false;
        public bool SPsel = false;
        public bool SPen = false;
        public bool Reset = false;
        public bool UlAbus = false;
        public bool BuFclk = false;
        public bool ACbus = false;
        public bool ACclk = false;
        public bool RGbus = false;
        public bool RgpCclk = false;
        public bool RaMrd = false;
        public bool RaMwr = false;
        public bool RaMcs = false;
        public bool Nbus = false;
        public bool OuTclk = false;
        public bool UlAop0 = false;
        public bool UlAop1 = false;
        public bool UlAop2 = false;
        public bool Rgpb0 = false;
        public bool Rgpb1 = false;
        public InstructionLogItem(Instruction instruction) {
            Instruction = instruction;
        }
        public InstructionLogItem(string text) {
            Text = text;
        }
        public List<CulunaValor> ToList()
        {
            var list = new List<CulunaValor>();
            if (Instruction != null)
            {
                list.Add(new CulunaValor() { Coluna = "Bus", Valor = Bus });
                list.Add(new CulunaValor() { Coluna = "FlagC", Valor = FlagC ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "FlagZ", Valor = FlagZ ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "EOI", Valor = Eoi ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ROMrd", Valor = RoMrd ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ROMcs", Valor = RoMcs ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "PCHbus", Valor = PcHbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "PCLbus", Valor = PcLbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "PCHclk", Valor = PcHclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "PCLclk", Valor = PcLclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "Data/PC sel", Valor = DataPCsel ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "DIRclk", Valor = DiRclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "SPclk", Valor = SPclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "SPInc/Dc", Valor = SpIncDec ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "SPsel", Valor = SPsel ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "SPen", Valor = SPen ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "Reset", Valor = Reset ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ULAbus", Valor = UlAbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "BUFclk", Valor = BuFclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ACbus", Valor = ACbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ACclk", Valor = ACclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RGbus", Valor = RGbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RG/PCcl", Valor = RgpCclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RAMrd", Valor = RaMrd ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RAMwr", Valor = RaMwr ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RAMcs", Valor = RaMcs ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "INbus", Valor = Nbus ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "OUTclk", Valor = OuTclk ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ULAop0", Valor = UlAop0 ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ULAop1", Valor = UlAop1 ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "ULAop2", Valor = UlAop2 ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RGPB0", Valor = Rgpb0 ? "1" : "0" });
                list.Add(new CulunaValor() { Coluna = "RGPB1", Valor = Rgpb1 ? "1" : "0" });
            }
            return list;
        }
        public override string ToString() {
            var res = "";
            if (Instruction != null) {
                res += "Bus: " + Bus + ", ";
                res += "FlagC: " + (FlagC ? "1" : "0") + ", ";
                res += "FlagZ: " + (FlagZ ? "1" : "0") + ", ";
                res += "EOI: " + (Eoi ? "1" : "0") + ", ";
                res += "ROMrd: " + (RoMrd ? "1" : "0") + ", ";
                res += "ROMcs: " + (RoMcs ? "1" : "0") + ", ";
                res += "PCHbus: " + (PcHbus ? "1" : "0") + ", ";
                res += "PCLbus: " + (PcLbus ? "1" : "0") + ", ";
                res += "PCHclk: " + (PcHclk ? "1" : "0") + ", ";
                res += "PCLclk: " + (PcLclk ? "1" : "0") + ", ";
                res += "Data/PC sel: " + (DataPCsel ? "1" : "0") + ", ";
                res += "DIRclk: " + (DiRclk ? "1" : "0") + ", ";
                res += "SPclk: " + (SPclk ? "1" : "0") + ", ";
                res += "SPInc/Dec: " + (SpIncDec ? "1" : "0") + ", ";
                res += "SPsel: " + (SPsel ? "1" : "0") + ", ";
                res += "SPen: " + (SPen ? "1" : "0") + ", ";
                res += "Reset: " + (Reset ? "1" : "0") + ", ";
                res += "ULAbus: " + (UlAbus ? "1" : "0") + ", ";
                res += "BUFclk: " + (BuFclk ? "1" : "0") + ", ";
                res += "ACbus: " + (ACbus ? "1" : "0") + ", ";
                res += "ACclk: " + (ACclk ? "1" : "0") + ", ";
                res += "RGbus: " + (RGbus ? "1" : "0") + ", ";
                res += "RG/PCclk: " + (RgpCclk ? "1" : "0") + ", ";
                res += "RAMrd: " + (RaMrd ? "1" : "0") + ", ";
                res += "RAMwr: " + (RaMwr ? "1" : "0") + ", ";
                res += "RAMcs: " + (RaMcs ? "1" : "0") + ", ";
                res += "INbus: " + (Nbus ? "1" : "0") + ", ";
                res += "OUTclk: " + (OuTclk ? "1" : "0") + ", ";
                res += "ULAop0: " + (UlAop0 ? "1" : "0") + ", ";
                res += "ULAop1: " + (UlAop1 ? "1" : "0") + ", ";
                res += "ULAop2: " + (UlAop2 ? "1" : "0") + ", ";
                res += "RGPB0: " + (Rgpb0 ? "1" : "0") + ", ";
                res += "RGPB1: " + (Rgpb1 ? "1" : "0") + "\r\n";
            } else {
                res += Text + "\r\n";
            }
            return res;
        }
    }
}
