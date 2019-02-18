using System.Collections.Generic;
using System.Linq;
using M3PlusMicrocontroller;
using NUnit.Framework;

namespace M3MicrocontrollerTests
{
    [TestFixture]
    public class CompilerTests
    {
        [TestCase("ADD A, A", new byte[]{0b000_00_000})]
        [TestCase("SUB A, A", new byte[]{0b001_00_000})]
        [TestCase("AND A, A", new byte[]{0b010_00_000})]
        [TestCase("OR A, A", new byte[]{0b011_00_000})]
        [TestCase("XOR A, A", new byte[]{0b100_00_000})]
        [TestCase("NOT A, A", new byte[]{0b101_00_000})]
        [TestCase("MOV A, A", new byte[]{0b110_00_000})]
        [TestCase("INC A, A", new byte[]{0b111_00_000})]
        
        [TestCase("ADD A, B", new byte[]{0b000_00_001})]
        [TestCase("ADD A, C", new byte[]{0b000_01_001})]
        [TestCase("ADD A, D", new byte[]{0b000_10_001})]
        [TestCase("ADD A, E", new byte[]{0b000_11_001})]
        
        [TestCase("ADD A, OUT0", new byte[]{0b000_00_011})]
        [TestCase("ADD A, OUT1", new byte[]{0b000_01_011})]
        [TestCase("ADD A, OUT2", new byte[]{0b000_10_011})]
        [TestCase("ADD A, OUT3", new byte[]{0b000_11_011})]
        
        [TestCase("ADD B, A", new byte[]{0b000_00_100})]
        [TestCase("ADD C, A", new byte[]{0b000_01_100})]
        [TestCase("ADD D, A", new byte[]{0b000_10_100})]
        [TestCase("ADD E, A", new byte[]{0b000_11_100})]

        [TestCase("ADD IN0, A", new byte[]{0b000_00_110})]
        [TestCase("ADD IN1, A", new byte[]{0b000_01_110})]
        [TestCase("ADD IN2, A", new byte[]{0b000_10_110})]
        [TestCase("ADD IN3, A", new byte[]{0b000_11_110})]

        [TestCase("ADD A, #FF", new byte[]{0b000_00_010, 0b1111_1111})]
        [TestCase("ADD #FF, A", new byte[]{0b000_00_101, 0b1111_1111})]
        
        [TestCase("ADD FF, A", new byte[]{0b000_00_111, 0b000_00_000, 0b1111_1111})]
        
        [TestCase("ADD FF, B", new byte[]{0b000_00_111, 0b000_00_001, 0b1111_1111})]
        [TestCase("ADD FF, C", new byte[]{0b000_00_111, 0b000_01_001, 0b1111_1111})]
        [TestCase("ADD FF, D", new byte[]{0b000_00_111, 0b000_10_001, 0b1111_1111})]
        [TestCase("ADD FF, E", new byte[]{0b000_00_111, 0b000_11_001, 0b1111_1111})]
        
        [TestCase("ADD FF, #33", new byte[]{0b000_00_111, 0b000_00_010, 0b1111_1111, 0b0011_0011})]
        
        [TestCase("ADD A, A\r\nTESTE: JMP TESTE", new byte[]{0b000_00_000, 0b000_00_111, 0b000_00_011, 0b0000_0000, 0b0000_0001})]
        [TestCase("ADD A, A\r\nTESTE: JMPC TESTE", new byte[]{0b000_00_000, 0b000_00_111, 0b000_00_100, 0b0000_0000, 0b0000_0001})]
        [TestCase("ADD A, A\r\nTESTE: JMPZ TESTE", new byte[]{0b000_00_000, 0b000_00_111, 0b000_00_101, 0b0000_0000, 0b0000_0001})]
        [TestCase("ADD A, A\r\nTESTE: CALL TESTE", new byte[]{0b000_00_000, 0b000_00_111, 0b000_00_110, 0b0000_0000, 0b0000_0001})]
        [TestCase("RET", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_000})]
        
        [TestCase("ADD #B, A", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_001})]
        [TestCase("ADD #C, A", new byte[]{0b000_00_111, 0b000_00_111, 0b000_01_001})]
        
        [TestCase("ADD A, #B", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_010})]
        [TestCase("ADD A, #C", new byte[]{0b000_00_111, 0b000_00_111, 0b000_01_010})]
        
        [TestCase("PUSH B", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_011})]
        [TestCase("PUSH C", new byte[]{0b000_00_111, 0b000_00_111, 0b000_01_011})]
        
        [TestCase("POP B", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_100})]
        [TestCase("POP C", new byte[]{0b000_00_111, 0b000_00_111, 0b000_01_100})]

        [TestCase("PUSHA", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_101})]
        [TestCase("POPA", new byte[]{0b000_00_111, 0b000_00_111, 0b000_00_110})]

        public void MustCompileCorrectly(string instructions, byte[] result)
        {
            var compiler = new Compiler();
            var resultInstructions = compiler.Compile(instructions, null);
            var resultBytes = resultInstructions.Select(x => x?.Convert());
            var byteList = new List<byte>();
            foreach (var item in resultBytes.Where(x => x != null))
                byteList.AddRange(item);
            var bytes = byteList.ToArray();
            Assert.AreEqual(result.Length, bytes.Length, "There are a different quantity of bytes");
            for (var i = 0; i < result.Length; i++)
                Assert.AreEqual(result[i], bytes[i], $"Was expected byte {result[i]:X2} but was {bytes[i]:X2}. (index {i})");
        }
    }
}