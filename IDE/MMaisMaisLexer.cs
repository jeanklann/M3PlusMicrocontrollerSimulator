using System.Collections.Generic;
using ScintillaNET;

namespace IDE
{
    public class MMaisMaisLexer
    {
        private const int StyleDefault = 0;
        public const int CpuInstruction = 1;
        public const int Register = 2;
        public const int Identifier = 3;
        public const int Number = 4;
        public const int Address = 5;
        public const int Comment = 6;

        private const int StateDefault = 0;
        private const int StateIdentifier = 1;
        private const int StateNumber = 2;
        private const int StateComment = 3;
        private const int StateAddress = 4;

        private readonly HashSet<string> _cpuInstructions;
        private readonly HashSet<string> _registrers;

        public MMaisMaisLexer(string cpuInstructions = "", string registrers = "")
        {
            cpuInstructions = cpuInstructions.ToLower();
            registrers = registrers.ToLower();
            var cpuInstructionsList = cpuInstructions != string.Empty ? cpuInstructions.Split(' ') : new string[] { };
            var registrersList = registrers != string.Empty ? registrers.Split(' ') : new string[] { };
            _cpuInstructions = new HashSet<string>(cpuInstructionsList);
            _registrers = new HashSet<string>(registrersList);
        }

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;
            var currentPos = startPos;
            var length = 0;
            var state = StateDefault;
            var text = scintilla.Text + '\0';

            scintilla.StartStyling(startPos);
            while (currentPos < endPos + 1)
            {
                var c = char.ToLower(text[currentPos]);
                var reprocess = true;
                while (reprocess)
                {
                    reprocess = false;
                    switch (state)
                    {
                        case StateDefault:
                            if (c == ';')
                            {
                                scintilla.SetStyling(1, Comment);
                                state = StateComment;
                            }
                            else if (c == '/' && char.ToLower(text[currentPos + 1]) == '/')
                            {
                                scintilla.SetStyling(1, Comment);
                                state = StateComment;
                            }
                            else if (char.IsDigit(c) || c >= 'a' && c <= 'f')
                            {
                                state = StateNumber;
                                reprocess = true;
                            }
                            else if (c == '#')
                            {
                                length++;
                                state = StateAddress;
                            }
                            else if (char.IsLetterOrDigit(c) || c == '_')
                            {
                                state = StateIdentifier;
                                reprocess = true;
                            }
                            else
                            {
                                if (currentPos < endPos)
                                    scintilla.SetStyling(1, StyleDefault);
                            }

                            break;
                        case StateComment:
                            if (c != '\0')
                                scintilla.SetStyling(1, Comment);
                            if (c == '\n' || c == '\r') state = StateDefault;
                            break;
                        case StateNumber:
                            if (char.IsDigit(c) || c >= 'a' && c <= 'f' || c == 'd' || c == 'h')
                            {
                                length++;
                                if (length < 3 && c == 'h') state = StateIdentifier;
                                if (length > 2)
                                {
                                    if (length == 3 && c == 'h')
                                    {
                                    }
                                    else if (char.IsDigit(c))
                                    {
                                    }
                                    else if (c == 'd')
                                    {
                                        var valid = true;
                                        for (var i = currentPos - 1; i >= currentPos - (length - 1); i--)
                                        {
                                            var c2 = char.ToLower(text[i]);
                                            if (!char.IsDigit(c2)) valid = false;
                                        }

                                        if (!valid) state = StateIdentifier;
                                    }
                                    else
                                    {
                                        state = StateIdentifier;
                                    }
                                }
                            }
                            else
                            {
                                if (char.IsLetterOrDigit(c) || c == '_')
                                {
                                    state = StateIdentifier;
                                    reprocess = true;
                                }
                                else if (length < 2)
                                {
                                    state = StateIdentifier;
                                    reprocess = true;
                                }
                                else
                                {
                                    scintilla.SetStyling(length, Number);
                                    length = 0;
                                    state = StateDefault;
                                    reprocess = true;
                                }
                            }

                            break;
                        case StateAddress:
                            if (char.IsDigit(c) || c >= 'a' && c <= 'f' || c == 'd' || c == 'h')
                            {
                                length++;
                                if (length < 4 && c == 'h') state = StateIdentifier;
                                if (length > 3)
                                {
                                    if (length == 4 && c == 'h')
                                    {
                                    }
                                    else if (char.IsDigit(c))
                                    {
                                    }
                                    else if (c == 'd')
                                    {
                                        var valid = true;
                                        for (var i = currentPos - 1; i >= currentPos - (length - 2); i--)
                                        {
                                            var c2 = char.ToLower(text[i]);
                                            if (!char.IsDigit(c2)) valid = false;
                                        }

                                        if (!valid) state = StateIdentifier;
                                    }
                                    else
                                    {
                                        state = StateIdentifier;
                                    }
                                }
                            }
                            else
                            {
                                if (char.IsLetterOrDigit(c) || c == '_')
                                {
                                    state = StateIdentifier;
                                    reprocess = true;
                                }
                                else
                                {
                                    scintilla.SetStyling(length, Number);
                                    length = 0;
                                    state = StateDefault;
                                    reprocess = true;
                                }
                            }

                            break;
                        case StateIdentifier:
                            if (char.IsLetterOrDigit(c) || c == '_')
                            {
                                length++;
                            }
                            else
                            {
                                var style = Identifier;
                                var identifier = scintilla.GetTextRange(currentPos - length, length).ToLower();
                                if (_cpuInstructions.Contains(identifier))
                                    style = CpuInstruction;
                                else if (_registrers.Contains(identifier))
                                    style = Register;
                                scintilla.SetStyling(length, style);
                                length = 0;
                                state = StateDefault;
                                reprocess = true;
                            }

                            break;
                    }
                }

                currentPos++;
            }
        }
    }
}