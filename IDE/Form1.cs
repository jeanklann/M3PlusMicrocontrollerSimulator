using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace IDE {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            // Configure the ASM lexer styles
            //this.scintilla.Styles[ScintillaNET.Style.Asm.Default].ForeColor = System.Drawing.Color.Silver;

            this.scintilla.Styles[Style.Asm.CpuInstruction].ForeColor = System.Drawing.Color.Blue;
            this.scintilla.Styles[Style.Asm.CpuInstruction].Bold = true;

            this.scintilla.Styles[Style.Asm.Register].ForeColor = System.Drawing.Color.Navy;
            this.scintilla.Styles[Style.Asm.Register].Bold = true;

            this.scintilla.Styles[Style.Asm.Identifier].ForeColor = System.Drawing.Color.Maroon;

            this.scintilla.Styles[Style.Asm.Number].ForeColor = System.Drawing.Color.Red;



            this.scintilla.Styles[Style.Asm.Comment].ForeColor = System.Drawing.Color.Green;




            this.scintilla.Lexer = Lexer.Asm;

            this.scintilla.SetKeywords(0, "mov add sub inc jmp jmpc jmpz call ret");
            this.scintilla.SetKeywords(1, "");
            this.scintilla.SetKeywords(2, "a b c d e out1 out2 out3 out4 in1 in2 in3 in4");
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {
            
        }

        private int maxLineNumberCharLength;
        private void scintilla_TextChanged(object sender, EventArgs e) {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            if(maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;
            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void scintilla_Click(object sender, EventArgs e) {

        }
    }
}
