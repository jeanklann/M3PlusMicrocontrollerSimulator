using System;
using System.Collections.Generic;

namespace M3PlusMicrocontroller {
    public class Instruction {
        public string Text { get; }
        public readonly string Description;
        private readonly Function _function;
        private readonly Direction _from;
        public readonly Direction To;
        public void Execute(Simulator sim)
        {
            var res = _function(sim, _from, To, this);
            if(To != null)
                To.Value = (byte) res;
            sim.FlagC = res > byte.MaxValue;
            sim.FlagZ = (To?.Value ?? 0) == 0;
        }
        public int Size => Bytes.Length;
        private byte[] _bytes;
        public byte[] Bytes => _bytes ?? (_bytes = Convert());
        public bool HasBreakpoint = false;
        public string Label { get; set; }

        private static readonly string[] descricao = {
            "{0}o registrador A com {1} e envia para {2}",
            "{0}{1} e envia para {2}"
        };
        
        public Instruction(string text, Function function, Direction from, Direction to, string description, int descriptionStyle = 0) {
            Text = $"{text} {from.Instruction}, {to.Instruction}";
            _function = function;
            _from = from;
            To = to;
            Description = string.Format(descricao[descriptionStyle], description, _from.Description, To.Description);
            ValidaFluxosPadroes();
        }
        public Instruction(string text, Function function, Direction to, string description)
        {
            Description = description;
            Text = $"{text} {to.Instruction}";
            _function = function;
            To = to;
        }
        
        public Instruction(string text, Function function, string description)
        {
            Description = description;
            Text = $"{text}";
            _function = function;
        }


        private void ValidaFluxosPadroes()
        {
            if (ValidaFluxo<Acumulator, Acumulator>()) return;       //A*A → A
            if (ValidaFluxo<Register, Acumulator>()) return;         //A*reg → A 
            if (ValidaFluxo<Acumulator, Register>()) return;         //A*A → reg
            if (ValidaFluxo<Acumulator, Ram>()) return;              //A*A → RAM
            if (ValidaFluxo<Acumulator, Output>()) return;           //A*A → OUT
            if (ValidaFluxo<Ram, Acumulator>()) return;              //A*RAM → A
            if (ValidaFluxo<Input, Acumulator>()) return;            //A*IN → A
            if (ValidaFluxo<Rom, Acumulator>()) return;              //A*ROM → A
            if (ValidaFluxo<Rom, Register>()) return;                //A*ROM → reg
            if (ValidaFluxo<Rom, Ram>()) return;                     //A*ROM → RAM
            if (ValidaFluxo<AddressRam, Acumulator>()) return;       //A*DRAM → A
            if (ValidaFluxo<Acumulator, AddressRam>()) return;       //A*A → DRAM
            throw new CompilerError(FluxoInvalido);
        }

        private const string FluxoInvalido = "Fluxo de controle inválido";
        private bool ValidaFluxo<TE, TD>(bool comparacaoExtra = true) 
            where TE : Direction
            where TD : Direction
        {
            if (!comparacaoExtra)
                return false;
            if (_from?.GetType() == typeof(TE) && To?.GetType() == typeof(TD))
                return true;
            return false;
        }
        private const byte PageByte = 0b000_00_111;
        public byte[] Convert()
        {
            
            var bytes = new List<byte>();
            var codePage = 0;
            var operation = GetByteOperation();
            var control = GetControl(ref codePage, out var extraBytes);
            var register = GetRegister();
            for (var i = 0; i < codePage; i++)
                bytes.Add(PageByte);
            bytes.Add((byte) (operation | control | register));
            foreach (var extraByte in extraBytes)
                bytes.Add(extraByte);
            return bytes.ToArray();
        }

        public static string FromBytes(byte[] bytes, int index, out int totalBytes, out byte[] address)
        {
            var bytesConverted = new byte[bytes.Length-index];
            Array.Copy(bytes, index, bytesConverted, 0, bytes.Length - index);
            var page = 0;
            if ((PageByte & bytesConverted[page]) == PageByte)
                ++page;
            if ((PageByte & bytesConverted[page]) == PageByte)
                ++page;
            
            var instruction = bytesConverted[page];
            var operation = ExtractOperation(page, instruction);
            var arguments = ExtractArguments(page, instruction, bytesConverted, out totalBytes, out address);
            return string.IsNullOrEmpty(arguments) ? operation : $"{operation} {arguments}";
        }

        private static string ExtractArguments(int page, byte instruction, byte[] bytes, out int totalBytes, out byte[] address)
        {
            totalBytes = 1;
            address = null;
            if (page == 0)
            {
                if (CompareByte(Control, instruction, 0b000_00_000))
                    return "A, A";
                if (CompareByte(Control, instruction, 0b000_00_001))
                    return $"A, {ExtractRegister(instruction)}";
                if (CompareByte(Control, instruction, 0b000_00_010))
                {
                    totalBytes = 2;
                    return $"A, #{bytes[page + 1]:X2}";
                }

                if (CompareByte(Control, instruction, 0b000_00_011))
                    return $"A, {ExtractOutput(instruction)}";
                if (CompareByte(Control, instruction, 0b000_00_100))
                    return $"{ExtractRegister(instruction)}, A";
                if (CompareByte(Control, instruction, 0b000_00_101))
                {
                    totalBytes = 2;
                    return $"#{bytes[page + 1]:X2}, A";
                }

                if (CompareByte(Control, instruction, 0b000_00_110))
                    return $"{ExtractInput(instruction)}, A";
            }
            else if (page == 1)
            {
                totalBytes = 2;
                if (CompareByte(Control, instruction, 0b000_00_000))
                {
                    totalBytes = 3;
                    return $"{bytes[page + 1]:X2}, A";
                }

                if (CompareByte(Control, instruction, 0b000_00_001))
                {
                    totalBytes = 3;
                    return $"{bytes[page + 1]:X2}, {ExtractRegister(instruction)}";
                }

                if (CompareByte(Control, instruction, 0b000_00_010))
                {
                    totalBytes = 4;
                    return $"{bytes[page + 1]:X2}, #{bytes[page + 2]:X2}";
                }

                if (CompareByte(Control, instruction, 0b000_00_011))
                {
                    totalBytes = 4;
                    SetAddress(page, bytes, out address);
                    return BuildAddress(page, bytes);
                }
                    
                if (CompareByte(Control, instruction, 0b000_00_100))
                {
                    totalBytes = 4;
                    SetAddress(page, bytes, out address);
                    return BuildAddress(page, bytes);
                }
                if (CompareByte(Control, instruction, 0b000_00_101))
                {
                    totalBytes = 4;
                    SetAddress(page, bytes, out address);
                    return BuildAddress(page, bytes);
                }
                if (CompareByte(Control, instruction, 0b000_00_110))
                {
                    totalBytes = 4;
                    SetAddress(page, bytes, out address);
                    return BuildAddress(page, bytes);
                }
                if (CompareByte(Control, instruction, 0b000_00_010))
                {
                    totalBytes = 4;
                    SetAddress(page, bytes, out address);
                    return BuildAddress(page, bytes);
                }
            }
            else if (page == 2)
            {
                totalBytes = 3;
                if (CompareByte(Control, instruction, 0b000_00_000))
                    return string.Empty;
                if (CompareByte(Control, instruction, 0b000_00_001))
                    return $"DRAM{ExtractRegister(instruction)}, A";
                if (CompareByte(Control, instruction, 0b000_00_010))
                    return $"A, DRAM{ExtractRegister(instruction)}";
                if (CompareByte(Control, instruction, 0b000_00_011))
                    return $"{ExtractRegister(instruction)}";
                if (CompareByte(Control, instruction, 0b000_00_100))
                    return $"{ExtractRegister(instruction)}";
                if (CompareByte(Control, instruction, 0b000_00_101))
                    return string.Empty;
                if (CompareByte(Control, instruction, 0b000_00_110))
                    return string.Empty;
            }
            return string.Empty;
        }

        private static void SetAddress(int page, byte[] bytes, out byte[] address)
        {
            address = new[] {bytes[page + 1], bytes[page + 2]};
        }

        private static string BuildAddress(int page, byte[] bytes)
        {
            return $"E_{bytes[page + 1]:X2}{bytes[page + 2]:X2}";
        }

        private static string ExtractRegister(byte instruction)
        {
            if (CompareByte(Register, instruction, 0b000_00_000))
                return "B";
            if (CompareByte(Register, instruction, 0b000_01_000))
                return "C";
            if (CompareByte(Register, instruction, 0b000_10_000))
                return "D";
            if (CompareByte(Register, instruction, 0b000_11_000))
                return "E";
            return string.Empty;
        }
        private static string ExtractInput(byte instruction)
        {
            if (CompareByte(Register, instruction, 0b000_00_000))
                return "IN0";
            if (CompareByte(Register, instruction, 0b000_01_000))
                return "IN1";
            if (CompareByte(Register, instruction, 0b000_10_000))
                return "IN2";
            if (CompareByte(Register, instruction, 0b000_11_000))
                return "IN3";
            return string.Empty;
        }
        private static string ExtractOutput(byte instruction)
        {
            if (CompareByte(Register, instruction, 0b000_00_000))
                return "OUT0";
            if (CompareByte(Register, instruction, 0b000_01_000))
                return "OUT1";
            if (CompareByte(Register, instruction, 0b000_10_000))
                return "OUT2";
            if (CompareByte(Register, instruction, 0b000_11_000))
                return "OUT3";
            return string.Empty;
        }
        private const byte Operation = 0b111_00_000;
        private const byte Control = 0b000_00_111;
        private const byte Register = 0b000_11_000;
        private static string ExtractOperation(int page, byte instruction)
        {
            if (page == 1)
            {
                if (CompareByte(Control, instruction, 0b000_00_011))
                    return "JMP";
                if (CompareByte(Control, instruction, 0b000_00_100))
                    return "JMPC";
                if (CompareByte(Control, instruction, 0b000_00_101))
                    return "JMPZ";
                if (CompareByte(Control, instruction, 0b000_00_110))
                    return "CALL";
            } else if (page == 2)
            {
                if (CompareByte(Control, instruction, 0b000_00_000))
                    return "RET";
                if (CompareByte(Control, instruction, 0b000_00_011))
                    return "PUSH";
                if (CompareByte(Control, instruction, 0b000_00_100))
                    return "POP";
                if (CompareByte(Control, instruction, 0b000_00_101))
                    return "PUSHA";
                if (CompareByte(Control, instruction, 0b000_00_110))
                    return "POPA";
            }
            if (CompareByte(Operation, instruction, 0b000_00_000))
                return "ADD";
            if (CompareByte(Operation, instruction, 0b001_00_000))
                return "SUB";
            if (CompareByte(Operation, instruction, 0b010_00_000))
                return "AND";
            if (CompareByte(Operation, instruction, 0b011_00_000))
                return "OR";
            if (CompareByte(Operation, instruction, 0b100_00_000))
                return "XOR";
            if (CompareByte(Operation, instruction, 0b101_00_000))
                return "NOT";
            if (CompareByte(Operation, instruction, 0b110_00_000))
                return "MOV";
            if (CompareByte(Operation, instruction, 0b111_00_000))
                return "INC";

            return null;
        }

        private static bool CompareByte(int part, byte value, byte compare) => (part & value) == compare;

        private byte GetRegister()
        {
            //0b000_00_0XX << 2 = 0b000_XX_000
            if (To is Register toReg)
                return (byte) (toReg.WitchOne-1 << 3); 
            if (To is Output toOut)
                return (byte) (toOut.WitchOne-1 << 3);
            if (_from is Register fromReg)
                return (byte) (fromReg.WitchOne-1 << 3);
            if (_from is Input fromIn)
                return (byte) (fromIn.WitchOne-1 << 3);
            return 0;
        }

        private byte GetControl(ref int codePage, out byte[] extraBytes)
        {
            extraBytes = new byte[0];
            if (ValidaFluxo<Acumulator, Acumulator>())
                return 0b000_00_000;
            if (ValidaFluxo<Acumulator, Register>())
                return 0b000_00_001;
            if (ValidaFluxo<Acumulator, Ram>())
                return 0b000_00_010;
            if (ValidaFluxo<Acumulator, Output>())
                return 0b000_00_011;
            if (ValidaFluxo<Register, Acumulator>())
                return 0b000_00_100;
            if (ValidaFluxo<Ram, Acumulator>())
            {
                extraBytes = new []{(_from as Ram)?.Address ?? 0};
                return 0b000_00_101;
            }

            if (ValidaFluxo<Input, Acumulator>())
                return 0b000_00_110;
            codePage++;
            if (ValidaFluxo<Rom, Acumulator>())
            {
                extraBytes = new []{(_from as Rom)?.Value?? 0};
                return 0b000_00_000;
            }

            if (ValidaFluxo<Rom, Register>())
            {
                extraBytes = new []{(_from as Rom)?.Value?? 0};
                return 0b000_00_001;
            }

            if (ValidaFluxo<Rom, Ram>())
            {
                extraBytes = new []
                {
                    (_from as Rom)?.Value ?? 0,
                    (To as Ram)?.Address ?? 0
                };
                return 0b000_00_010;
            }

            if (_function == Functions.Jmp)
            {
                MountAddress(out extraBytes);
                return 0b000_00_011;
            }

            if (_function == Functions.Jmpc)
            {
                MountAddress(out extraBytes);
                return 0b000_00_100;
            }

            if (_function == Functions.Jmpz)
            {
                MountAddress(out extraBytes);
                return 0b000_00_101;
            }

            if (_function == Functions.Call)
            {
                MountAddress(out extraBytes);
                return 0b000_00_110;
            }

            codePage++;
            if (_function == Functions.Ret)
                return 0b000_00_000;
            if (ValidaFluxo<AddressRam, Acumulator>())
            if (ValidaFluxo<AddressRam, Acumulator>())
                return 0b000_00_001;
            if (ValidaFluxo<Acumulator, AddressRam>())
                return 0b000_00_010;
            if(_function == Functions.Push && To is Register)
                return 0b000_00_011;
            if(_function == Functions.Pop && To is Register)
                return 0b000_00_100;
            if(_function == Functions.Push && To is Acumulator)
                return 0b000_00_101;
            if(_function == Functions.Pop && To is Acumulator)
                return 0b000_00_110;
            return 0b000_00_000;
        }

        private void MountAddress(out byte[] extraBytes)
        {
            extraBytes = new[]
            {
                (byte) ((To as Address)?.ValueAddress / ((int) byte.MaxValue + 1) ?? 0),
                (byte) ((To as Address)?.ValueAddress % ((int) byte.MaxValue + 1) ?? 0)
            };
        }

        private byte GetByteOperation()
        {
            if (_function == Functions.Add)
                return 0b000_00_000;
            if (_function == Functions.Sub)
                return 0b001_00_000;
            if (_function == Functions.And)
                return 0b010_00_000;
            if (_function == Functions.Or)
                return 0b011_00_000;
            if (_function == Functions.Xor)
                return 0b100_00_000;
            if (_function == Functions.Not)
                return 0b101_00_000;
            if (_function == Functions.Mov)
                return 0b110_00_000;
            if (_function == Functions.Inc)
                return 0b111_00_000;
            return 0b000_00_000;
        }
    }
}
