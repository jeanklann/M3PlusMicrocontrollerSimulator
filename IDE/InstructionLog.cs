using System;
using System.Collections.Generic;
using M3PlusMicrocontroller;

namespace IDE
{
    public class InstructionLog
    {
        private readonly Dictionary<Instruction, int> _instructionClockSize;
        public int ClearCount;
        public List<InstructionLogItem> Items;

        public InstructionLog()
        {
            Items = new List<InstructionLogItem>();
            _instructionClockSize = new Dictionary<Instruction, int>();
        }

        public void Clear()
        {
            Items.Clear();
            _instructionClockSize.Clear();
            ++ClearCount;
        }

        public void Add(InstructionLogItem item)
        {
            if (item.Eoi)
            {
                var instruction = Items[Items.Count - 1].Instruction;
                if (instruction != null)
                {
                    var size = 0;
                    for (var i = Items.Count - 1; i >= 0; i--)
                    {
                        if (Items[i].Instruction == null) continue;
                        if (instruction == Items[i].Instruction)
                        {
                            size++;
                            if (Items[i].Primeira) break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (_instructionClockSize.ContainsKey(instruction)) _instructionClockSize.Remove(instruction);
                    _instructionClockSize.Add(instruction, size + 1);
                }
            }

            Items.Add(item);
        }

        public void Add(string text)
        {
            Items.Add(new InstructionLogItem(text));
        }

        public void Iniciar()
        {
            Clear();
            Add("Iniciou a simulação");
        }

        public void Pausar()
        {
            Add("Pausou a simulação");
        }

        public void Parar()
        {
            Add("Parou a simulação");
        }

        public void Continuar()
        {
            Add("Continou a simulação");
        }

        public void PassoDentro()
        {
            Add("Passo dentro");
        }

        public void PassoFora()
        {
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
                            if (_instructionClockSize.ContainsKey(item.Instruction) &&
                                _instructionClockSize[item.Instruction] > 1)
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

        public override string ToString()
        {
            var clock = 0;
            var res = "";
            try
            {
                foreach (var item in Items)
                {
                    if (item.Instruction != null)
                    {
                        if (clock == 0)
                        {
                            res += "\r\nInstrução: " + item.Instruction.Text + "\r\n";
                            res += "Tamanho: " + item.Instruction.Size + " byte" +
                                   (item.Instruction.Size != 1 ? "s" : "") + "\r\n";
                            if (_instructionClockSize.ContainsKey(item.Instruction) &&
                                _instructionClockSize[item.Instruction] > 1)
                                res += "Clocks necessários: " + _instructionClockSize[item.Instruction] + "\r\n";
                            else
                                res += "Não foi possível calcular o número de clocks necessários\r\n";
                        }

                        res += "Sinais do clock " + (clock + 1) + ": ";
                        res += item.ToString();
                        res += "\r\n";
                        ++clock;
                        if (item.Eoi && clock > 3) clock = 0;
                    }
                    else
                    {
                        res += item.ToString();
                    }
                }
            }
            catch (Exception)
            {
                res += "Aguarde um momento.";
            }

            return res;
        }
    }
}