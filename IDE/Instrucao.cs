using System.Collections.Generic;

namespace IDE
{
    public class Instrucao
    {
        public int? ClocksNecessarios;
        public string Nome;
        public int QuantidadeBytes;
        public List<List<CulunaValor>> Sinais = new List<List<CulunaValor>>();
        public string Texto;
    }
}