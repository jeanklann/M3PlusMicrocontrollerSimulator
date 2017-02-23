using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScintillaNET;

namespace IDE {
    public static class UIStatics {
        public static Codigo Codigo;
        public static Depurador Depurador;

        public const int BREAKPOINT_INDEX_MARGIN = 1;
        public const int LABEL_MARGIN = 2;

        public const int BREAKPOINT_MARKER = 1;
        public const int INDEX_MARKER = 2;
        public const int LABEL_MARKER = 3;
        
        
        

        public static void ScintillaSetStyle(Scintilla scintilla, bool hasLabel = false) {
            scintilla.Styles[Style.Asm.CpuInstruction].ForeColor = System.Drawing.Color.Blue;
            scintilla.Styles[Style.Asm.CpuInstruction].Bold = true;

            scintilla.Styles[Style.Asm.Register].ForeColor = System.Drawing.Color.Navy;
            scintilla.Styles[Style.Asm.Register].Bold = true;

            scintilla.Styles[Style.Asm.Identifier].ForeColor = System.Drawing.Color.Maroon;

            scintilla.Styles[Style.Asm.Number].ForeColor = System.Drawing.Color.Red;



            scintilla.Styles[Style.Asm.Comment].ForeColor = System.Drawing.Color.Green;




            scintilla.Lexer = Lexer.Asm;

            scintilla.SetKeywords(0, "mov add sub inc jmp jmpc jmpz call ret and or xor not");
            scintilla.SetKeywords(1, "");
            scintilla.SetKeywords(2, "a b c d e out0 out1 out2 out3 in0 in1 in2 in3");
            
            Margin margin = scintilla.Margins[BREAKPOINT_INDEX_MARGIN];
            margin.Width = 16;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1<<BREAKPOINT_MARKER) | (1<<INDEX_MARKER);
            margin.Cursor = MarginCursor.Arrow;
            
            Marker marker = scintilla.Markers[BREAKPOINT_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(Color.Red);
            marker.SetForeColor(Color.Red);
            
            Marker marker2 = scintilla.Markers[INDEX_MARKER];
            marker2.Symbol = MarkerSymbol.ShortArrow;
            marker2.SetBackColor(Color.Yellow);
            marker2.SetForeColor(Color.Black);
            

            if (hasLabel) {
                
                Margin margin2 = scintilla.Margins[LABEL_MARGIN];
                margin2.Width = 16;
                margin2.Sensitive = true;
                margin2.Type = MarginType.Symbol;
                margin2.Mask = 1<<LABEL_MARKER;
                margin2.Cursor = MarginCursor.Arrow;

                Marker marker3 = scintilla.Markers[LABEL_MARKER];
                marker3.Symbol = MarkerSymbol.Bookmark;
                marker3.SetBackColor(Color.Gray);
                marker3.SetForeColor(Color.Black);
            }
        }
    }
}
