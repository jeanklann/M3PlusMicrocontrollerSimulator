using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace IDE {
    public partial class FormRomMemory : Form {
        public FormRomMemory() {
            InitializeComponent();
        }
        private List<Label> Labels = new List<Label>();
        private Thread threadUpdate;
        private bool exitForm;
        private int lastHashSimulator;
        private void FormRomMemory_Load(object sender, EventArgs e) {
            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (var linha = 0; linha < 4096; linha++) {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[linha].Cells[0].Value = (linha * 16).ToString("X4");
                dataGridView1.Rows[linha].Cells[0].Style = style;

                for (var coluna = 1; coluna < 17; coluna++) {
                    dataGridView1.Rows[linha].Cells[coluna].Value = "00";
                }
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            threadUpdate = new Thread(ThreadUpdate);
            threadUpdate.Start();
        }

        private void ThreadUpdate() {
            while (!UiStatics.WantExit && !exitForm) {

                try {
                    dataGridView1.ClearSelection();
                    if (UiStatics.Simulador == null) {
                        if (lastHashSimulator != 0) {
                            for (var linha = 0; linha < 4096; linha++) {
                                for (var coluna = 1; coluna < 17; coluna++) {
                                    dataGridView1.Rows[linha].Cells[coluna].Value = "00";
                                }
                            }
                        }
                        lastHashSimulator = 0;
                    }
                    if (UiStatics.Simulador != null && lastHashSimulator != UiStatics.Simulador.GetHashCode()) {
                        if (UiStatics.Simulador.CompiledProgram != null) {
                            lastHashSimulator = UiStatics.Simulador.GetHashCode();
                            for (var i = 0; i < UiStatics.Simulador.CompiledProgram.Length; i++) {
                                int linha;
                                int coluna;
                                getLineAndColumn(i, out linha, out coluna);
                                dataGridView1.Rows[linha].Cells[coluna].Value = UiStatics.Simulador.CompiledProgram[i].ToString("X2");
                            }
                        }
                    }
                    if (UiStatics.Simulador != null) {
                        var pc = UiStatics.Simulador.NextInstruction;
                        if (UiStatics.Simulador.Program != null && UiStatics.Simulador.Program[pc] != null) {
                            var Instruction = UiStatics.Simulador.Program[pc];
                            for (var i = 0; i < Instruction.Size; i++) {
                                int linha;
                                int coluna;
                                getLineAndColumn(pc + i, out linha, out coluna);
                                dataGridView1.Rows[linha].Cells[coluna].Selected = true;
                            }
                        }
                    }
                    Thread.Sleep(20);
                } catch (Exception) {}
            }
        }

        private int getAddress(int line, int column) {
            return line * 16 + (column - 1);
        }
        private void getLineAndColumn(int address, out int line, out int column) {
            column = address % 16 + 1;
            line = address / 16;
        }

        private void FormRomMemory_Leave(object sender, EventArgs e) {
        }

        private void FormRomMemory_FormClosing(object sender, FormClosingEventArgs e) {
            exitForm = true;
        }

        private void FormRomMemory_VisibleChanged(object sender, EventArgs e) {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
