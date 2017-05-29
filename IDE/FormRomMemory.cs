using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IDE {
    public partial class FormRomMemory : Form {
        public FormRomMemory() {
            InitializeComponent();
        }
        private List<Label> Labels = new List<Label>();
        private Thread threadUpdate;
        private bool exitForm = false;
        private int lastHashSimulator = 0;
        private void FormRomMemory_Load(object sender, EventArgs e) {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int linha = 0; linha < 4096; linha++) {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[linha].Cells[0].Value = (linha * 16).ToString("X4");
                dataGridView1.Rows[linha].Cells[0].Style = style;

                for (int coluna = 1; coluna < 17; coluna++) {
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
            while (!UIStatics.WantExit && !exitForm) {

                try {
                    dataGridView1.ClearSelection();
                    if (UIStatics.Simulador == null) {
                        if (lastHashSimulator != 0) {
                            for (int linha = 0; linha < 4096; linha++) {
                                for (int coluna = 1; coluna < 17; coluna++) {
                                    dataGridView1.Rows[linha].Cells[coluna].Value = "00";
                                }
                            }
                        }
                        lastHashSimulator = 0;
                    }
                    if (UIStatics.Simulador != null && lastHashSimulator != UIStatics.Simulador.GetHashCode()) {
                        if (UIStatics.Simulador.CompiledProgram != null) {
                            lastHashSimulator = UIStatics.Simulador.GetHashCode();
                            for (int i = 0; i < UIStatics.Simulador.CompiledProgram.Length; i++) {
                                int linha;
                                int coluna;
                                getLineAndColumn(i, out linha, out coluna);
                                dataGridView1.Rows[linha].Cells[coluna].Value = UIStatics.Simulador.CompiledProgram[i].ToString("X2");
                            }
                        }
                    }
                    if (UIStatics.Simulador != null) {
                        int pc = UIStatics.Simulador.NextInstruction;
                        if (UIStatics.Simulador.Program != null && UIStatics.Simulador.Program[pc] != null) {
                            M3PlusMicrocontroller.Instruction Instruction = UIStatics.Simulador.Program[pc];
                            for (int i = 0; i < Instruction.Size; i++) {
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
    }
}
