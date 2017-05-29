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
            try {
                base.Dispose(disposing);
            } catch (System.Exception) { }
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
            this.controlModuleStatesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bus = new System.Windows.Forms.Label();
            this.clock = new System.Windows.Forms.CheckBox();
            this.flagC = new System.Windows.Forms.CheckBox();
            this.flagZ = new System.Windows.Forms.CheckBox();
            this.EOI = new System.Windows.Forms.CheckBox();
            this.ROMrd = new System.Windows.Forms.CheckBox();
            this.ROMcs = new System.Windows.Forms.CheckBox();
            this.PCHbus = new System.Windows.Forms.CheckBox();
            this.PCLbus = new System.Windows.Forms.CheckBox();
            this.PCHclk = new System.Windows.Forms.CheckBox();
            this.PCLclk = new System.Windows.Forms.CheckBox();
            this.DataPCsel = new System.Windows.Forms.CheckBox();
            this.DIRclk = new System.Windows.Forms.CheckBox();
            this.SPclk = new System.Windows.Forms.CheckBox();
            this.SPIncDec = new System.Windows.Forms.CheckBox();
            this.SPsel = new System.Windows.Forms.CheckBox();
            this.SPen = new System.Windows.Forms.CheckBox();
            this.Reset = new System.Windows.Forms.CheckBox();
            this.ULAbus = new System.Windows.Forms.CheckBox();
            this.BUFclk = new System.Windows.Forms.CheckBox();
            this.ACbus = new System.Windows.Forms.CheckBox();
            this.ACclk = new System.Windows.Forms.CheckBox();
            this.RGbus = new System.Windows.Forms.CheckBox();
            this.RGPCclk = new System.Windows.Forms.CheckBox();
            this.RAMrd = new System.Windows.Forms.CheckBox();
            this.RAMwr = new System.Windows.Forms.CheckBox();
            this.RAMcs = new System.Windows.Forms.CheckBox();
            this.INbus = new System.Windows.Forms.CheckBox();
            this.OUTclk = new System.Windows.Forms.CheckBox();
            this.ULAop0 = new System.Windows.Forms.CheckBox();
            this.ULAop1 = new System.Windows.Forms.CheckBox();
            this.ULAop2 = new System.Windows.Forms.CheckBox();
            this.RGPB0 = new System.Windows.Forms.CheckBox();
            this.RGPB1 = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.controlModuleStatesPanel.SuspendLayout();
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
            // controlModuleStatesPanel
            // 
            this.controlModuleStatesPanel.AutoSize = true;
            this.controlModuleStatesPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlModuleStatesPanel.Controls.Add(this.bus);
            this.controlModuleStatesPanel.Controls.Add(this.clock);
            this.controlModuleStatesPanel.Controls.Add(this.flagC);
            this.controlModuleStatesPanel.Controls.Add(this.flagZ);
            this.controlModuleStatesPanel.Controls.Add(this.EOI);
            this.controlModuleStatesPanel.Controls.Add(this.ROMrd);
            this.controlModuleStatesPanel.Controls.Add(this.ROMcs);
            this.controlModuleStatesPanel.Controls.Add(this.PCHbus);
            this.controlModuleStatesPanel.Controls.Add(this.PCLbus);
            this.controlModuleStatesPanel.Controls.Add(this.PCHclk);
            this.controlModuleStatesPanel.Controls.Add(this.PCLclk);
            this.controlModuleStatesPanel.Controls.Add(this.DataPCsel);
            this.controlModuleStatesPanel.Controls.Add(this.DIRclk);
            this.controlModuleStatesPanel.Controls.Add(this.SPclk);
            this.controlModuleStatesPanel.Controls.Add(this.SPIncDec);
            this.controlModuleStatesPanel.Controls.Add(this.SPsel);
            this.controlModuleStatesPanel.Controls.Add(this.SPen);
            this.controlModuleStatesPanel.Controls.Add(this.Reset);
            this.controlModuleStatesPanel.Controls.Add(this.ULAbus);
            this.controlModuleStatesPanel.Controls.Add(this.BUFclk);
            this.controlModuleStatesPanel.Controls.Add(this.ACbus);
            this.controlModuleStatesPanel.Controls.Add(this.ACclk);
            this.controlModuleStatesPanel.Controls.Add(this.RGbus);
            this.controlModuleStatesPanel.Controls.Add(this.RGPCclk);
            this.controlModuleStatesPanel.Controls.Add(this.RAMrd);
            this.controlModuleStatesPanel.Controls.Add(this.RAMwr);
            this.controlModuleStatesPanel.Controls.Add(this.RAMcs);
            this.controlModuleStatesPanel.Controls.Add(this.INbus);
            this.controlModuleStatesPanel.Controls.Add(this.OUTclk);
            this.controlModuleStatesPanel.Controls.Add(this.ULAop0);
            this.controlModuleStatesPanel.Controls.Add(this.ULAop1);
            this.controlModuleStatesPanel.Controls.Add(this.ULAop2);
            this.controlModuleStatesPanel.Controls.Add(this.RGPB0);
            this.controlModuleStatesPanel.Controls.Add(this.RGPB1);
            this.controlModuleStatesPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlModuleStatesPanel.Location = new System.Drawing.Point(0, 247);
            this.controlModuleStatesPanel.Name = "controlModuleStatesPanel";
            this.controlModuleStatesPanel.Size = new System.Drawing.Size(547, 148);
            this.controlModuleStatesPanel.TabIndex = 1;
            // 
            // bus
            // 
            this.bus.AutoSize = true;
            this.bus.Enabled = false;
            this.bus.Location = new System.Drawing.Point(3, 3);
            this.bus.Margin = new System.Windows.Forms.Padding(3);
            this.bus.Name = "bus";
            this.bus.Size = new System.Drawing.Size(29, 26);
            this.bus.TabIndex = 0;
            this.bus.Text = "00\r\nBUS";
            this.bus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // clock
            // 
            this.clock.AutoSize = true;
            this.clock.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.clock.Enabled = false;
            this.clock.Location = new System.Drawing.Point(38, 3);
            this.clock.Name = "clock";
            this.clock.Size = new System.Drawing.Size(38, 31);
            this.clock.TabIndex = 1;
            this.clock.Text = "Clock";
            this.clock.UseVisualStyleBackColor = true;
            // 
            // flagC
            // 
            this.flagC.AutoSize = true;
            this.flagC.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.flagC.Enabled = false;
            this.flagC.Location = new System.Drawing.Point(82, 3);
            this.flagC.Name = "flagC";
            this.flagC.Size = new System.Drawing.Size(38, 31);
            this.flagC.TabIndex = 2;
            this.flagC.Text = "FlagC";
            this.flagC.UseVisualStyleBackColor = true;
            // 
            // flagZ
            // 
            this.flagZ.AutoSize = true;
            this.flagZ.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.flagZ.Enabled = false;
            this.flagZ.Location = new System.Drawing.Point(126, 3);
            this.flagZ.Name = "flagZ";
            this.flagZ.Size = new System.Drawing.Size(38, 31);
            this.flagZ.TabIndex = 3;
            this.flagZ.Text = "FlagZ";
            this.flagZ.UseVisualStyleBackColor = true;
            // 
            // EOI
            // 
            this.EOI.AutoSize = true;
            this.EOI.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.EOI.Enabled = false;
            this.EOI.Location = new System.Drawing.Point(170, 3);
            this.EOI.Name = "EOI";
            this.EOI.Size = new System.Drawing.Size(29, 31);
            this.EOI.TabIndex = 4;
            this.EOI.Text = "EOI";
            this.EOI.UseVisualStyleBackColor = true;
            // 
            // ROMrd
            // 
            this.ROMrd.AutoSize = true;
            this.ROMrd.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ROMrd.Enabled = false;
            this.ROMrd.Location = new System.Drawing.Point(205, 3);
            this.ROMrd.Name = "ROMrd";
            this.ROMrd.Size = new System.Drawing.Size(45, 31);
            this.ROMrd.TabIndex = 5;
            this.ROMrd.Text = "ROMrd";
            this.ROMrd.UseVisualStyleBackColor = true;
            // 
            // ROMcs
            // 
            this.ROMcs.AutoSize = true;
            this.ROMcs.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ROMcs.Enabled = false;
            this.ROMcs.Location = new System.Drawing.Point(256, 3);
            this.ROMcs.Name = "ROMcs";
            this.ROMcs.Size = new System.Drawing.Size(47, 31);
            this.ROMcs.TabIndex = 6;
            this.ROMcs.Text = "ROMcs";
            this.ROMcs.UseVisualStyleBackColor = true;
            // 
            // PCHbus
            // 
            this.PCHbus.AutoSize = true;
            this.PCHbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PCHbus.Enabled = false;
            this.PCHbus.Location = new System.Drawing.Point(309, 3);
            this.PCHbus.Name = "PCHbus";
            this.PCHbus.Size = new System.Drawing.Size(50, 31);
            this.PCHbus.TabIndex = 7;
            this.PCHbus.Text = "PCHbus";
            this.PCHbus.UseVisualStyleBackColor = true;
            // 
            // PCLbus
            // 
            this.PCLbus.AutoSize = true;
            this.PCLbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PCLbus.Enabled = false;
            this.PCLbus.Location = new System.Drawing.Point(365, 3);
            this.PCLbus.Name = "PCLbus";
            this.PCLbus.Size = new System.Drawing.Size(48, 31);
            this.PCLbus.TabIndex = 8;
            this.PCLbus.Text = "PCLbus";
            this.PCLbus.UseVisualStyleBackColor = true;
            // 
            // PCHclk
            // 
            this.PCHclk.AutoSize = true;
            this.PCHclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PCHclk.Enabled = false;
            this.PCHclk.Location = new System.Drawing.Point(419, 3);
            this.PCHclk.Name = "PCHclk";
            this.PCHclk.Size = new System.Drawing.Size(47, 31);
            this.PCHclk.TabIndex = 9;
            this.PCHclk.Text = "PCHclk";
            this.PCHclk.UseVisualStyleBackColor = true;
            // 
            // PCLclk
            // 
            this.PCLclk.AutoSize = true;
            this.PCLclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PCLclk.Enabled = false;
            this.PCLclk.Location = new System.Drawing.Point(472, 3);
            this.PCLclk.Name = "PCLclk";
            this.PCLclk.Size = new System.Drawing.Size(45, 31);
            this.PCLclk.TabIndex = 10;
            this.PCLclk.Text = "PCLclk";
            this.PCLclk.UseVisualStyleBackColor = true;
            // 
            // DataPCsel
            // 
            this.DataPCsel.AutoSize = true;
            this.DataPCsel.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.DataPCsel.Enabled = false;
            this.DataPCsel.Location = new System.Drawing.Point(3, 40);
            this.DataPCsel.Name = "DataPCsel";
            this.DataPCsel.Size = new System.Drawing.Size(61, 31);
            this.DataPCsel.TabIndex = 11;
            this.DataPCsel.Text = "DataPCsel";
            this.DataPCsel.UseVisualStyleBackColor = true;
            // 
            // DIRclk
            // 
            this.DIRclk.AutoSize = true;
            this.DIRclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.DIRclk.Enabled = false;
            this.DIRclk.Location = new System.Drawing.Point(70, 40);
            this.DIRclk.Name = "DIRclk";
            this.DIRclk.Size = new System.Drawing.Size(44, 31);
            this.DIRclk.TabIndex = 33;
            this.DIRclk.Text = "DIRclk";
            this.DIRclk.UseVisualStyleBackColor = true;
            // 
            // SPclk
            // 
            this.SPclk.AutoSize = true;
            this.SPclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SPclk.Enabled = false;
            this.SPclk.Location = new System.Drawing.Point(120, 40);
            this.SPclk.Name = "SPclk";
            this.SPclk.Size = new System.Drawing.Size(39, 31);
            this.SPclk.TabIndex = 12;
            this.SPclk.Text = "SPclk";
            this.SPclk.UseVisualStyleBackColor = true;
            // 
            // SPIncDec
            // 
            this.SPIncDec.AutoSize = true;
            this.SPIncDec.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SPIncDec.Enabled = false;
            this.SPIncDec.Location = new System.Drawing.Point(165, 40);
            this.SPIncDec.Name = "SPIncDec";
            this.SPIncDec.Size = new System.Drawing.Size(60, 31);
            this.SPIncDec.TabIndex = 13;
            this.SPIncDec.Text = "SPIncDec";
            this.SPIncDec.UseVisualStyleBackColor = true;
            // 
            // SPsel
            // 
            this.SPsel.AutoSize = true;
            this.SPsel.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SPsel.Enabled = false;
            this.SPsel.Location = new System.Drawing.Point(231, 40);
            this.SPsel.Name = "SPsel";
            this.SPsel.Size = new System.Drawing.Size(38, 31);
            this.SPsel.TabIndex = 14;
            this.SPsel.Text = "SPsel";
            this.SPsel.UseVisualStyleBackColor = true;
            // 
            // SPen
            // 
            this.SPen.AutoSize = true;
            this.SPen.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SPen.Enabled = false;
            this.SPen.Location = new System.Drawing.Point(275, 40);
            this.SPen.Name = "SPen";
            this.SPen.Size = new System.Drawing.Size(37, 31);
            this.SPen.TabIndex = 15;
            this.SPen.Text = "SPen";
            this.SPen.UseVisualStyleBackColor = true;
            // 
            // Reset
            // 
            this.Reset.AutoSize = true;
            this.Reset.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Reset.Enabled = false;
            this.Reset.Location = new System.Drawing.Point(318, 40);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(39, 31);
            this.Reset.TabIndex = 16;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            // 
            // ULAbus
            // 
            this.ULAbus.AutoSize = true;
            this.ULAbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ULAbus.Enabled = false;
            this.ULAbus.Location = new System.Drawing.Point(363, 40);
            this.ULAbus.Name = "ULAbus";
            this.ULAbus.Size = new System.Drawing.Size(49, 31);
            this.ULAbus.TabIndex = 17;
            this.ULAbus.Text = "ULAbus";
            this.ULAbus.UseVisualStyleBackColor = true;
            // 
            // BUFclk
            // 
            this.BUFclk.AutoSize = true;
            this.BUFclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BUFclk.Enabled = false;
            this.BUFclk.Location = new System.Drawing.Point(418, 40);
            this.BUFclk.Name = "BUFclk";
            this.BUFclk.Size = new System.Drawing.Size(46, 31);
            this.BUFclk.TabIndex = 18;
            this.BUFclk.Text = "BUFclk";
            this.BUFclk.UseVisualStyleBackColor = true;
            // 
            // ACbus
            // 
            this.ACbus.AutoSize = true;
            this.ACbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ACbus.Enabled = false;
            this.ACbus.Location = new System.Drawing.Point(470, 40);
            this.ACbus.Name = "ACbus";
            this.ACbus.Size = new System.Drawing.Size(42, 31);
            this.ACbus.TabIndex = 19;
            this.ACbus.Text = "ACbus";
            this.ACbus.UseVisualStyleBackColor = true;
            // 
            // ACclk
            // 
            this.ACclk.AutoSize = true;
            this.ACclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ACclk.Enabled = false;
            this.ACclk.Location = new System.Drawing.Point(3, 77);
            this.ACclk.Name = "ACclk";
            this.ACclk.Size = new System.Drawing.Size(39, 31);
            this.ACclk.TabIndex = 20;
            this.ACclk.Text = "ACclk";
            this.ACclk.UseVisualStyleBackColor = true;
            // 
            // RGbus
            // 
            this.RGbus.AutoSize = true;
            this.RGbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RGbus.Enabled = false;
            this.RGbus.Location = new System.Drawing.Point(48, 77);
            this.RGbus.Name = "RGbus";
            this.RGbus.Size = new System.Drawing.Size(44, 31);
            this.RGbus.TabIndex = 21;
            this.RGbus.Text = "RGbus";
            this.RGbus.UseVisualStyleBackColor = true;
            // 
            // RGPCclk
            // 
            this.RGPCclk.AutoSize = true;
            this.RGPCclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RGPCclk.Enabled = false;
            this.RGPCclk.Location = new System.Drawing.Point(98, 77);
            this.RGPCclk.Name = "RGPCclk";
            this.RGPCclk.Size = new System.Drawing.Size(60, 31);
            this.RGPCclk.TabIndex = 22;
            this.RGPCclk.Text = "RG/PCclk";
            this.RGPCclk.UseVisualStyleBackColor = true;
            // 
            // RAMrd
            // 
            this.RAMrd.AutoSize = true;
            this.RAMrd.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RAMrd.Enabled = false;
            this.RAMrd.Location = new System.Drawing.Point(164, 77);
            this.RAMrd.Name = "RAMrd";
            this.RAMrd.Size = new System.Drawing.Size(44, 31);
            this.RAMrd.TabIndex = 23;
            this.RAMrd.Text = "RAMrd";
            this.RAMrd.UseVisualStyleBackColor = true;
            // 
            // RAMwr
            // 
            this.RAMwr.AutoSize = true;
            this.RAMwr.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RAMwr.Enabled = false;
            this.RAMwr.Location = new System.Drawing.Point(214, 77);
            this.RAMwr.Name = "RAMwr";
            this.RAMwr.Size = new System.Drawing.Size(46, 31);
            this.RAMwr.TabIndex = 24;
            this.RAMwr.Text = "RAMwr";
            this.RAMwr.UseVisualStyleBackColor = true;
            // 
            // RAMcs
            // 
            this.RAMcs.AutoSize = true;
            this.RAMcs.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RAMcs.Enabled = false;
            this.RAMcs.Location = new System.Drawing.Point(266, 77);
            this.RAMcs.Name = "RAMcs";
            this.RAMcs.Size = new System.Drawing.Size(46, 31);
            this.RAMcs.TabIndex = 25;
            this.RAMcs.Text = "RAMcs";
            this.RAMcs.UseVisualStyleBackColor = true;
            // 
            // INbus
            // 
            this.INbus.AutoSize = true;
            this.INbus.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.INbus.Enabled = false;
            this.INbus.Location = new System.Drawing.Point(318, 77);
            this.INbus.Name = "INbus";
            this.INbus.Size = new System.Drawing.Size(39, 31);
            this.INbus.TabIndex = 26;
            this.INbus.Text = "INbus";
            this.INbus.UseVisualStyleBackColor = true;
            // 
            // OUTclk
            // 
            this.OUTclk.AutoSize = true;
            this.OUTclk.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.OUTclk.Enabled = false;
            this.OUTclk.Location = new System.Drawing.Point(363, 77);
            this.OUTclk.Name = "OUTclk";
            this.OUTclk.Size = new System.Drawing.Size(48, 31);
            this.OUTclk.TabIndex = 27;
            this.OUTclk.Text = "OUTclk";
            this.OUTclk.UseVisualStyleBackColor = true;
            // 
            // ULAop0
            // 
            this.ULAop0.AutoSize = true;
            this.ULAop0.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ULAop0.Enabled = false;
            this.ULAop0.Location = new System.Drawing.Point(417, 77);
            this.ULAop0.Name = "ULAop0";
            this.ULAop0.Size = new System.Drawing.Size(50, 31);
            this.ULAop0.TabIndex = 28;
            this.ULAop0.Text = "ULAop0";
            this.ULAop0.UseVisualStyleBackColor = true;
            // 
            // ULAop1
            // 
            this.ULAop1.AutoSize = true;
            this.ULAop1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ULAop1.Enabled = false;
            this.ULAop1.Location = new System.Drawing.Point(473, 77);
            this.ULAop1.Name = "ULAop1";
            this.ULAop1.Size = new System.Drawing.Size(50, 31);
            this.ULAop1.TabIndex = 29;
            this.ULAop1.Text = "ULAop1";
            this.ULAop1.UseVisualStyleBackColor = true;
            // 
            // ULAop2
            // 
            this.ULAop2.AutoSize = true;
            this.ULAop2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ULAop2.Enabled = false;
            this.ULAop2.Location = new System.Drawing.Point(3, 114);
            this.ULAop2.Name = "ULAop2";
            this.ULAop2.Size = new System.Drawing.Size(50, 31);
            this.ULAop2.TabIndex = 30;
            this.ULAop2.Text = "ULAop2";
            this.ULAop2.UseVisualStyleBackColor = true;
            // 
            // RGPB0
            // 
            this.RGPB0.AutoSize = true;
            this.RGPB0.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RGPB0.Enabled = false;
            this.RGPB0.Location = new System.Drawing.Point(59, 114);
            this.RGPB0.Name = "RGPB0";
            this.RGPB0.Size = new System.Drawing.Size(52, 31);
            this.RGPB0.TabIndex = 31;
            this.RGPB0.Text = "RG/PB0";
            this.RGPB0.UseVisualStyleBackColor = true;
            // 
            // RGPB1
            // 
            this.RGPB1.AutoSize = true;
            this.RGPB1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RGPB1.Enabled = false;
            this.RGPB1.Location = new System.Drawing.Point(117, 114);
            this.RGPB1.Name = "RGPB1";
            this.RGPB1.Size = new System.Drawing.Size(52, 31);
            this.RGPB1.TabIndex = 32;
            this.RGPB1.Text = "RG/PB1";
            this.RGPB1.UseVisualStyleBackColor = true;
            // 
            // Circuito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.controlModuleStatesPanel);
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
            this.controlModuleStatesPanel.ResumeLayout(false);
            this.controlModuleStatesPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private FlowLayoutPanel controlModuleStatesPanel;
        private Label bus;
        private CheckBox clock;
        private CheckBox flagC;
        private CheckBox flagZ;
        private CheckBox EOI;
        private CheckBox ROMrd;
        private CheckBox ROMcs;
        private CheckBox PCHbus;
        private CheckBox PCLbus;
        private CheckBox PCHclk;
        private CheckBox PCLclk;
        private CheckBox DataPCsel;
        private CheckBox SPclk;
        private CheckBox SPIncDec;
        private CheckBox SPsel;
        private CheckBox SPen;
        private CheckBox Reset;
        private CheckBox ULAbus;
        private CheckBox BUFclk;
        private CheckBox ACbus;
        private CheckBox ACclk;
        private CheckBox RGbus;
        private CheckBox RGPCclk;
        private CheckBox RAMrd;
        private CheckBox RAMwr;
        private CheckBox RAMcs;
        private CheckBox INbus;
        private CheckBox OUTclk;
        private CheckBox ULAop0;
        private CheckBox ULAop1;
        private CheckBox ULAop2;
        private CheckBox RGPB0;
        private CheckBox RGPB1;
        private CheckBox DIRclk;
    }
}
