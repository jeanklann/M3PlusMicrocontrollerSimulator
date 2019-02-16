using System;
using System.Collections.Generic;

namespace M3PlusMicrocontroller
{
    public class TokenAnalyzer
    {
        private readonly List<Token> _tokens = new List<Token>();
        private int _index;
        private int _nextTokenindex;
        private string _program;

        public void Analyze(string program)
        {
            Token token;
            _program = program.ToUpper();
            do
            {
                token = NextToken_internal();
                _tokens.Add(token);
            } while (token.Type != TokenType.EoF);
        }

        public Token NextToken()
        {
            if (_tokens.Count == 0) throw new Exception("Not analyzed. Need to call function Analyze() first.");
            var token = _tokens[_nextTokenindex];
            ++_nextTokenindex;
            if (_nextTokenindex >= _tokens.Count) _nextTokenindex = _tokens.Count - 1;
            return token;
        }

        public Token PeekNextToken()
        {
            if (_tokens.Count == 0) throw new Exception("Not analyzed. Need to call function Analyze() first.");
            return _tokens[_nextTokenindex];
        }

        private Token NextToken_internal()
        {
            var beginIndex = _index;
            var value = "";
            var analyzed = false;
            do
            {
                if (_program.Length <= _index) break;
                value += _program[_index];
                ++_index;

                if (value == "") break;

                foreach (var item in Token.Space)
                {
                    //It's only space
                    if (item + "" != value) continue;
                    value = "";
                    break;
                }

                foreach (var item in Token.Lineseparator)
                {
                    //It's only a blank line
                    if (item + "" != value) continue;
                    value = "";
                    break;
                }

                var needContinue = false;
                foreach (var comment in Token.Comment)
                    if (comment == value)
                    {
                        //Comment
                        var c = true;
                        do
                        {
                            if (_program.Length <= _index) break;
                            foreach (var item in Token.Lineseparator)
                            {
                                //End of comment
                                if (item != _program[_index]) continue;
                                c = false;
                                break;
                            }

                            ++_index;
                        } while (c);

                        value = "";
                        beginIndex = _index;
                        needContinue = true;
                        break;
                    }

                if (needContinue) continue;
                if (value == "")
                {
                    beginIndex = _index;
                    continue;
                }

                if (value == Token.Instructionseparator + "") break;
                if (value == Token.Identificatorseparator + "") break;

                foreach (var item in Token.Lineseparator)
                {
                    //End of the line at the final of the string
                    if (item != value[value.Length - 1]) continue;
                    value = value.Substring(0, value.Length - 1);
                    analyzed = true;
                    break;
                }

                foreach (var item in Token.Space)
                {
                    //Space character at the final of the string
                    if (item != value[value.Length - 1]) continue;
                    value = value.Substring(0, value.Length - 1);
                    analyzed = true;
                    break;
                }

                if (Token.Identificatorseparator == value[value.Length - 1])
                {
                    //If it's an identificator
                    value = value.Substring(0, value.Length - 1);
                    --_index;
                    break;
                }

                if (Token.Instructionseparator == value[value.Length - 1])
                {
                    //Separator at the final of the string
                    value = value.Substring(0, value.Length - 1);
                    --_index;
                    break;
                }

                foreach (var item in Token.Comment)
                {
                    if (value.Length >= 2 && item == value.Substring(value.Length - 2, 2))
                    {
                        value = value.Substring(0, value.Length - 2);
                        --_index;
                        analyzed = true;
                        break;
                    }

                    if (value.Length < 1 || item != value[value.Length - 1] + "") continue;
                    value = value.Substring(0, value.Length - 1);
                    --_index;
                    analyzed = true;
                    break;
                }

                if (analyzed) break;
            } while (true);


            // Verifies what type of token is
            if (value == "") return new Token(TokenType.EoF, "", beginIndex);

            foreach (var item in Token.Registrers)
                if (item == value)
                    return new Token(TokenType.Registrer, value, beginIndex);
            foreach (var item in Token.Inputs)
                if (item == value)
                    return new Token(TokenType.Input, value, beginIndex);
            foreach (var item in Token.CpuInstructions)
                if (item == value)
                    return new Token(TokenType.CpuInstruction, value, beginIndex);
            foreach (var item in Token.Outputs)
                if (item == value)
                    return new Token(TokenType.Output, value, beginIndex);

            if (value == Token.Identificatorseparator + "")
                return new Token(TokenType.IdentificatorSeparator, value, beginIndex);
            if (value == Token.Instructionseparator + "") return new Token(TokenType.Separator, value, beginIndex);


            int number;

            if (value[0] == Token.Ram)
            {
                if (TryParseAnyNumber(value.Substring(1, value.Length - 1), out number))
                {
                    if (number < 0 || number > 255)
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(_program, beginIndex) +
                                                ". Valor inválido, os números para este processador devem estar entre 00 e FF em hexadecimal, ou entre 000 e 255 em decimal (00h - FFh e 000d - 255d).");
                    return new Token(TokenType.RamAddress, number.ToString(), beginIndex);
                }

                if (value.Length == 2)
                    for (var i = 1; i < Token.Registrers.Length; i++)
                        if (value[1] == Token.Registrers[i][0])
                            return new Token(TokenType.Dram, Token.Registrers[i], beginIndex);
                else
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(_program, beginIndex) +
                                            ". Valor de endereçamento inválido.");
            }
            else if (TryParseAnyNumber(value, out number))
            {
                if (number < 0 || number > 255)
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(_program, beginIndex) +
                                            ".  Valor inválido, os números para este processador devem estar entre 00 e FF em hexadecimal, ou entre 000 e 255 em decimal (00h - FFh e 000d - 255d).");
                return new Token(TokenType.Number, number.ToString(), beginIndex);
            }

            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0)
                    if (!(char.IsLetter(value[i]) || value[i] == '_'))
                        throw new CompilerError("Erro na linha " + Helpers.CountLines(_program, beginIndex) +
                                                ". Os labels devem começar com uma letra ou com underline (_).");
                if (!(char.IsLetterOrDigit(value[i]) || value[i] == '_'))
                    throw new CompilerError("Erro na linha " + Helpers.CountLines(_program, beginIndex) +
                                            ". Os labels devem ter somente letras, números e undelines (_).");
            }

            return new Token(TokenType.Identificator, value, beginIndex);
        }

        private static bool TryParseAnyNumber(string value, out int number)
        {
            if (value.Length > 2)
            {
                if (value[value.Length - 1] == 'H')
                {
                    if (value.Length != 3)
                    {
                        number = -1;
                        return false;
                    }

                    try
                    {
                        number = Convert.ToInt32(value.Substring(0, value.Length - 1), 16);
                        return true;
                    }
                    catch (Exception)
                    {
                        number = -1;
                        return false;
                    }
                }

                if (value[value.Length - 1] == 'D')
                    try
                    {
                        number = Convert.ToInt32(value.Substring(0, value.Length - 1));
                        return true;
                    }
                    catch (Exception)
                    {
                        number = -1;
                        return false;
                    }

                try
                {
                    number = Convert.ToInt32(value);
                    return true;
                }
                catch (Exception)
                {
                    number = -1;
                    return false;
                }
            }

            if (value.Length == 2)
            {
                try
                {
                    number = Convert.ToInt32(value, 16);
                    return true;
                }
                catch (Exception)
                {
                    number = -1;
                    return false;
                }
            }

            number = -1;
            return false;
        }
    }
}