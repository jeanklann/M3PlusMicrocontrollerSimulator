using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IDE;
using M3PlusMicrocontroller;
using NUnit.Framework;

namespace M3MicrocontrollerTests
{
    [TestFixture]
    public class CompilerTests
    {
        [TestCaseSource(typeof(CompilerTests), nameof(TestCases))]
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
                Assert.AreEqual(result[i], bytes[i],
                    $"Was expected byte {result[i]:X2} but was {bytes[i]:X2}. (index {i})");
        }
        [TestCaseSource(typeof(CompilerTests), nameof(TestCases))]
        public void MustDecompileCorrectly(string result, byte[] bytes)
        {
            var decompiler = new Decompiler();
            var res = decompiler.Decompile(bytes);
            Assert.AreEqual(result, res.Trim());
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                // All operations
                yield return new TestCaseData("ADD A, A", new byte[] {0b000_00_000});
                yield return new TestCaseData("SUB A, A", new byte[] {0b001_00_000});
                yield return new TestCaseData("AND A, A", new byte[] {0b010_00_000});
                yield return new TestCaseData("OR A, A", new byte[] {0b011_00_000});
                yield return new TestCaseData("XOR A, A", new byte[] {0b100_00_000});
                yield return new TestCaseData("NOT A, A", new byte[] {0b101_00_000});
                yield return new TestCaseData("MOV A, A", new byte[] {0b110_00_000});
                yield return new TestCaseData("INC A, A", new byte[] {0b111_00_000});
                //All registers
                yield return new TestCaseData("MOV A, B", new byte[] {0b110_00_001});
                yield return new TestCaseData("MOV A, C", new byte[] {0b110_01_001});
                yield return new TestCaseData("MOV A, D", new byte[] {0b110_10_001});
                yield return new TestCaseData("MOV A, E", new byte[] {0b110_11_001});
                //All outputs
                yield return new TestCaseData("MOV A, OUT0", new byte[] {0b110_00_011});
                yield return new TestCaseData("MOV A, OUT1", new byte[] {0b110_01_011});
                yield return new TestCaseData("MOV A, OUT2", new byte[] {0b110_10_011});
                yield return new TestCaseData("MOV A, OUT3", new byte[] {0b110_11_011});
                //All registers (other side)
                yield return new TestCaseData("MOV B, A", new byte[] {0b110_00_100});
                yield return new TestCaseData("MOV C, A", new byte[] {0b110_01_100});
                yield return new TestCaseData("MOV D, A", new byte[] {0b110_10_100});
                yield return new TestCaseData("MOV E, A", new byte[] {0b110_11_100});
                //All inputs
                yield return new TestCaseData("MOV IN0, A", new byte[] {0b110_00_110});
                yield return new TestCaseData("MOV IN1, A", new byte[] {0b110_01_110});
                yield return new TestCaseData("MOV IN2, A", new byte[] {0b110_10_110});
                yield return new TestCaseData("MOV IN3, A", new byte[] {0b110_11_110});
                //Ram
                yield return new TestCaseData("MOV A, #FF", new byte[] {0b110_00_010, 0b1111_1111});
                yield return new TestCaseData("MOV #FF, A", new byte[] {0b110_00_101, 0b1111_1111});
                //Rom
                yield return new TestCaseData("MOV FF, A", new byte[] {0b000_00_111, 0b110_00_000, 0b1111_1111});
                yield return new TestCaseData("MOV FF, B", new byte[] {0b000_00_111, 0b110_00_001, 0b1111_1111});
                yield return new TestCaseData("MOV FF, C", new byte[] {0b000_00_111, 0b110_01_001, 0b1111_1111});
                yield return new TestCaseData("MOV FF, D", new byte[] {0b000_00_111, 0b110_10_001, 0b1111_1111});
                yield return new TestCaseData("MOV FF, E", new byte[] {0b000_00_111, 0b110_11_001, 0b1111_1111});
                //Rom -> Ram
                yield return new TestCaseData("MOV FF, #33",
                    new byte[] {0b000_00_111, 0b110_00_010, 0b1111_1111, 0b0011_0011});
                yield return new TestCaseData("MOV 33, #FF",
                    new byte[] {0b000_00_111, 0b110_00_010, 0b0011_0011, 0b1111_1111});
                yield return new TestCaseData("MOV 11, #12",
                    new byte[] {0b000_00_111, 0b110_00_010, 0b0001_0001, 0b0001_0010});
                //Jmp / calls
                yield return new TestCaseData("MOV A, A\r\nE_0001:\r\nJMP E_0001",
                    new byte[] {0b110_00_000, 0b000_00_111, 0b000_00_011, 0b0000_0000, 0b0000_0001});
                yield return new TestCaseData("MOV A, A\r\nE_0001:\r\nJMPC E_0001",
                    new byte[] {0b110_00_000, 0b000_00_111, 0b000_00_100, 0b0000_0000, 0b0000_0001});
                yield return new TestCaseData("MOV A, A\r\nE_0001:\r\nJMPZ E_0001",
                    new byte[] {0b110_00_000, 0b000_00_111, 0b000_00_101, 0b0000_0000, 0b0000_0001});
                yield return new TestCaseData("MOV A, A\r\nE_0001:\r\nCALL E_0001",
                    new byte[] {0b110_00_000, 0b000_00_111, 0b000_00_110, 0b0000_0000, 0b0000_0001});
                yield return new TestCaseData("RET", new byte[] {0b000_00_111, 0b000_00_111, 0b000_00_000});
                //Dram left
                yield return new TestCaseData("MOV #B, A", new byte[] {0b000_00_111, 0b000_00_111, 0b110_00_001});
                yield return new TestCaseData("MOV #C, A", new byte[] {0b000_00_111, 0b000_00_111, 0b110_01_001});
                yield return new TestCaseData("MOV #D, A", new byte[] {0b000_00_111, 0b000_00_111, 0b110_10_001});
                yield return new TestCaseData("MOV #E, A", new byte[] {0b000_00_111, 0b000_00_111, 0b110_11_001});
                //Dram right
                yield return new TestCaseData("MOV A, #B", new byte[] {0b000_00_111, 0b000_00_111, 0b110_00_010});
                yield return new TestCaseData("MOV A, #C", new byte[] {0b000_00_111, 0b000_00_111, 0b110_01_010});
                yield return new TestCaseData("MOV A, #D", new byte[] {0b000_00_111, 0b000_00_111, 0b110_10_010});
                yield return new TestCaseData("MOV A, #E", new byte[] {0b000_00_111, 0b000_00_111, 0b110_11_010});
                //Pushes
                yield return new TestCaseData("PUSH B", new byte[] {0b000_00_111, 0b000_00_111, 0b000_00_011});
                yield return new TestCaseData("PUSH C", new byte[] {0b000_00_111, 0b000_00_111, 0b000_01_011});
                yield return new TestCaseData("PUSH D", new byte[] {0b000_00_111, 0b000_00_111, 0b000_10_011});
                yield return new TestCaseData("PUSH E", new byte[] {0b000_00_111, 0b000_00_111, 0b000_11_011});
                //Pops
                yield return new TestCaseData("POP B", new byte[] {0b000_00_111, 0b000_00_111, 0b000_00_100});
                yield return new TestCaseData("POP C", new byte[] {0b000_00_111, 0b000_00_111, 0b000_01_100});
                yield return new TestCaseData("POP D", new byte[] {0b000_00_111, 0b000_00_111, 0b000_10_100});
                yield return new TestCaseData("POP E", new byte[] {0b000_00_111, 0b000_00_111, 0b000_11_100});
                //Pusha / Popa
                yield return new TestCaseData("PUSHA", new byte[] {0b000_00_111, 0b000_00_111, 0b000_00_101});
                yield return new TestCaseData("POPA", new byte[] {0b000_00_111, 0b000_00_111, 0b000_00_110});
            }
        }
    }
}