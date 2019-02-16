using System.Collections.Generic;

namespace IDE
{
    public class Instrucao
    {
        public string Nome;
        public int QuantidadeBytes;
        public int? ClocksNecessarios;
        public string Texto;
        public List<List<CulunaValor>> Sinais = new List<List<CulunaValor>>();
    }
}