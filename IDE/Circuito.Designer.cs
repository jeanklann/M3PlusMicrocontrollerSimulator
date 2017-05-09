using System;
using System.Windows.Forms;

namespace IDE {
    partial class Circuito {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.entradaESaídaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entradaLógicaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saídaLógicaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tecladoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDe7SegmentosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portasLógicasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nANDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xNORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nOTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circuitosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decodificador7SegmentosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipflopJKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipflopRSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entradaESaídaToolStripMenuItem,
            this.portasLógicasToolStripMenuItem,
            this.circuitosToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 70);
            // 
            // entradaESaídaToolStripMenuItem
            // 
            this.entradaESaídaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entradaLógicaToolStripMenuItem,
            this.saídaLógicaToolStripMenuItem,
            this.tecladoToolStripMenuItem,
            this.displayDe7SegmentosToolStripMenuItem});
            this.entradaESaídaToolStripMenuItem.Name = "entradaESaídaToolStripMenuItem";
            this.entradaESaídaToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.entradaESaídaToolStripMenuItem.Text = "Entrada e Saída";
            // 
            // entradaLógicaToolStripMenuItem
            // 
            this.entradaLógicaToolStripMenuItem.Name = "entradaLógicaToolStripMenuItem";
            this.entradaLógicaToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.entradaLógicaToolStripMenuItem.Text = "Entrada lógica";
            this.entradaLógicaToolStripMenuItem.Click += new System.EventHandler(this.entradaLógicaToolStripMenuItem_Click);
            // 
            // saídaLógicaToolStripMenuItem
            // 
            this.saídaLógicaToolStripMenuItem.Name = "saídaLógicaToolStripMenuItem";
            this.saídaLógicaToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.saídaLógicaToolStripMenuItem.Text = "Saída lógica";
            this.saídaLógicaToolStripMenuItem.Click += new System.EventHandler(this.saídaLógicaToolStripMenuItem_Click);
            // 
            // tecladoToolStripMenuItem
            // 
            this.tecladoToolStripMenuItem.Name = "tecladoToolStripMenuItem";
            this.tecladoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.tecladoToolStripMenuItem.Text = "Teclado";
            this.tecladoToolStripMenuItem.Click += new System.EventHandler(this.tecladoToolStripMenuItem_Click);
            // 
            // displayDe7SegmentosToolStripMenuItem
            // 
            this.displayDe7SegmentosToolStripMenuItem.Name = "displayDe7SegmentosToolStripMenuItem";
            this.displayDe7SegmentosToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.displayDe7SegmentosToolStripMenuItem.Text = "Display de 7 segmentos";
            this.displayDe7SegmentosToolStripMenuItem.Click += new System.EventHandler(this.displayDe7SegmentosToolStripMenuItem_Click);
            // 
            // portasLógicasToolStripMenuItem
            // 
            this.portasLógicasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aNDToolStripMenuItem,
            this.nANDToolStripMenuItem,
            this.oRToolStripMenuItem,
            this.nORToolStripMenuItem,
            this.xORToolStripMenuItem,
            this.xNORToolStripMenuItem,
            this.nOTToolStripMenuItem});
            this.portasLógicasToolStripMenuItem.Name = "portasLógicasToolStripMenuItem";
            this.portasLógicasToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.portasLógicasToolStripMenuItem.Text = "Portas Lógicas";
            // 
            // aNDToolStripMenuItem
            // 
            this.aNDToolStripMenuItem.Name = "aNDToolStripMenuItem";
            this.aNDToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.aNDToolStripMenuItem.Text = "AND";
            this.aNDToolStripMenuItem.Click += new System.EventHandler(this.aNDToolStripMenuItem_Click);
            // 
            // nANDToolStripMenuItem
            // 
            this.nANDToolStripMenuItem.Name = "nANDToolStripMenuItem";
            this.nANDToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.nANDToolStripMenuItem.Text = "NAND";
            this.nANDToolStripMenuItem.Click += new System.EventHandler(this.nANDToolStripMenuItem_Click);
            // 
            // oRToolStripMenuItem
            // 
            this.oRToolStripMenuItem.Name = "oRToolStripMenuItem";
            this.oRToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.oRToolStripMenuItem.Text = "OR";
            this.oRToolStripMenuItem.Click += new System.EventHandler(this.oRToolStripMenuItem_Click);
            // 
            // nORToolStripMenuItem
            // 
            this.nORToolStripMenuItem.Name = "nORToolStripMenuItem";
            this.nORToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.nORToolStripMenuItem.Text = "NOR";
            this.nORToolStripMenuItem.Click += new System.EventHandler(this.nORToolStripMenuItem_Click);
            // 
            // xORToolStripMenuItem
            // 
            this.xORToolStripMenuItem.Name = "xORToolStripMenuItem";
            this.xORToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.xORToolStripMenuItem.Text = "XOR";
            this.xORToolStripMenuItem.Click += new System.EventHandler(this.xORToolStripMenuItem_Click);
            // 
            // xNORToolStripMenuItem
            // 
            this.xNORToolStripMenuItem.Name = "xNORToolStripMenuItem";
            this.xNORToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.xNORToolStripMenuItem.Text = "XNOR";
            this.xNORToolStripMenuItem.Click += new System.EventHandler(this.xNORToolStripMenuItem_Click);
            // 
            // nOTToolStripMenuItem
            // 
            this.nOTToolStripMenuItem.Name = "nOTToolStripMenuItem";
            this.nOTToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.nOTToolStripMenuItem.Text = "NOT";
            this.nOTToolStripMenuItem.Click += new System.EventHandler(this.nOTToolStripMenuItem_Click);
            // 
            // circuitosToolStripMenuItem
            // 
            this.circuitosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decodificador7SegmentosToolStripMenuItem,
            this.flipflopJKToolStripMenuItem,
            this.flipflopRSToolStripMenuItem});
            this.circuitosToolStripMenuItem.Name = "circuitosToolStripMenuItem";
            this.circuitosToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.circuitosToolStripMenuItem.Text = "Circuitos";
            // 
            // decodificador7SegmentosToolStripMenuItem
            // 
            this.decodificador7SegmentosToolStripMenuItem.Name = "decodificador7SegmentosToolStripMenuItem";
            this.decodificador7SegmentosToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.decodificador7SegmentosToolStripMenuItem.Text = "Decodificador 7 segmentos";
            this.decodificador7SegmentosToolStripMenuItem.Click += new System.EventHandler(this.decodificador7SegmentosToolStripMenuItem_Click);
            // 
            // flipflopJKToolStripMenuItem
            // 
            this.flipflopJKToolStripMenuItem.Name = "flipflopJKToolStripMenuItem";
            this.flipflopJKToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.flipflopJKToolStripMenuItem.Text = "Flip-flop JK";
            this.flipflopJKToolStripMenuItem.Click += new System.EventHandler(this.flipflopJKToolStripMenuItem_Click);
            // 
            // flipflopRSToolStripMenuItem
            // 
            this.flipflopRSToolStripMenuItem.Name = "flipflopRSToolStripMenuItem";
            this.flipflopRSToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.flipflopRSToolStripMenuItem.Text = "Flip-flop RS";
            this.flipflopRSToolStripMenuItem.Click += new System.EventHandler(this.flipflopRSToolStripMenuItem_Click);
            // 
            // Circuito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "Circuito";
            this.Size = new System.Drawing.Size(547, 395);
            this.Load += new System.EventHandler(this.Circuito_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Circuito_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Circuito_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Circuito_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Circuito_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Circuito_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Circuito_MouseDown);
            this.MouseLeave += new System.EventHandler(this.Circuito_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Circuito_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Circuito_MouseUp);
            this.Resize += new System.EventHandler(this.Circuito_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem portasLógicasToolStripMenuItem;
        private ToolStripMenuItem aNDToolStripMenuItem;
        private ToolStripMenuItem nANDToolStripMenuItem;
        private ToolStripMenuItem oRToolStripMenuItem;
        private ToolStripMenuItem nORToolStripMenuItem;
        private ToolStripMenuItem xORToolStripMenuItem;
        private ToolStripMenuItem xNORToolStripMenuItem;
        private ToolStripMenuItem nOTToolStripMenuItem;
        private ToolStripMenuItem circuitosToolStripMenuItem;
        private ToolStripMenuItem decodificador7SegmentosToolStripMenuItem;
        private ToolStripMenuItem entradaESaídaToolStripMenuItem;
        private ToolStripMenuItem entradaLógicaToolStripMenuItem;
        private ToolStripMenuItem saídaLógicaToolStripMenuItem;
        private ToolStripMenuItem tecladoToolStripMenuItem;
        private ToolStripMenuItem displayDe7SegmentosToolStripMenuItem;
        private ToolStripMenuItem flipflopJKToolStripMenuItem;
        private ToolStripMenuItem flipflopRSToolStripMenuItem;
    }
}
