using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;
using M3PlusMicrocontroller;
using System.Threading;

namespace IDE {
    public partial class FormularioPrincipal : Form {

        public FormularioPrincipal() {
            InitializeComponent();

            UIStatics.Codigo = codigo1;
            UIStatics.Depurador = depurador1;
            UIStatics.Circuito = circuito1;

            UIStatics.MainForm = this;

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {

        }


        private void scintilla_Click(object sender, EventArgs e) {

        }

        

        private void verPróximoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.GotoNextBreakpoint();
        }

        private void verAnteriorToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.GotoPreviousBreakpoint();
        }

        private void removerTodosToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.RemoveAllBreakpoint();
        }

        private void adicionarNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.AddBreakpoint();
        }

        private void removerNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.RemoveBreakpoint();
        }

        private void Form1_Load(object sender, EventArgs e) {
            UIStatics.Circuito.LoadControl();
        }

        private void codigo1_Load(object sender, EventArgs e) {

        }

        private void códigoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = true;
            depurador1.Visible = false;
            circuito1.Visible = false;
        }

        private void depuraçãoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = false;
            depurador1.Visible = true;
            circuito1.Visible = false;
            if (UIStatics.threadDepurador == null) {
                UIStatics.threadDepurador = new Thread(UIStatics.Depurador.UpdateAll);
                UIStatics.threadDepurador.Start();
            }
        }

        private void circuitoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = false;
            depurador1.Visible = false;
            circuito1.Visible = true;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.ZoomMore();
            depurador1.ZoomMore();
            circuito1.ZoomMore();
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
            codigo1.ZoomLess();
            depurador1.ZoomLess();
            circuito1.ZoomLess();
        }

        private void zoomOriginalToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.ZoomReset();
            depurador1.ZoomReset();
            circuito1.ZoomReset();
        }

        private void analisarEConstruirToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Compile();
        }

        private void rodarToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Run();
        }

        private void pausarToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Pause();
        }

        private void pularParaDentroToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.StepIn();
        }

        private void pularParaForaToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.StepOut();
        }

        private void pararToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Stop();
        }

        private void FormularioPrincipal_Leave(object sender, EventArgs e) {
            UIStatics.WantExit = true;
        }

        private void piscaLedToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Text = "apagado:\nmov IN4,a\nand 32,a\njmpz apagado\npisca:\nmov 01,a\nmov a,out1\nmov 00,a\nmov a,out1\njmp pisca";
        }

        private void FormularioPrincipal_FormClosed(object sender, FormClosedEventArgs e) {
            if(UIStatics.Simulador != null) {
                UIStatics.Simulador.Stop();
            }
            UIStatics.WantExit = true;
        }

        private void FormularioPrincipal_FormClosing(object sender, FormClosingEventArgs e) {
            if (codigo1.Changed) {
                DialogResult dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?", "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if(dialogResult == DialogResult.Cancel) {
                    e.Cancel = true;
                    return;
                } else if(dialogResult == DialogResult.Yes) {
                    if(UIStatics.FilePath != null) {
                        if(UIStatics.Save() == false) {
                            if (!TrySave()) {
                                e.Cancel = true;
                                return;
                            }
                        } else {
                            return;
                        }
                    } else {
                        if(!TrySave()) {
                            e.Cancel = true;
                            return;
                        } else {
                            return;
                        }
                    }
                } else if(dialogResult == DialogResult.No) {
                    return;
                }
            } else {
                return;
            }
        }
        private bool TrySave() {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Projeto do simulador (*.m3mprj)|*.m3mprj|Todos os arquivos (*.*)|*.*";
            DialogResult fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) {
                return false;
            } else {
                UIStatics.FilePath = fileDialog.FileName;
                if (UIStatics.Save() == false) {
                    MessageBox.Show(this, "Ocorreu um erro ao salvar o projeto.", "Erro ao salvar o projeto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                } else {
                    codigo1.Changed = false;
                }
            }
            return true;
        }
        

        private void desfazerToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.Undo();
        }

        private void refazerToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.Redo();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.Cut();
        }

        private void copierToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.Copy();
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.Paste();
        }

        private void selecionarTudoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.scintilla.SelectAll();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (codigo1.Changed) {
                DialogResult dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?", "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes) {
                    if (UIStatics.FilePath != null) {
                        if (UIStatics.Save() == false) {
                            if (TrySave()) {
                                codigo1.scintilla.Text = "";
                                codigo1.Changed = false;
                                UIStatics.FilePath = null;
                            }
                        }
                    } else {
                        if (TrySave()) {
                            codigo1.scintilla.Text = "";
                            codigo1.Changed = false;
                            UIStatics.FilePath = null;
                        }
                    }
                } else if (dialogResult == DialogResult.No) {
                    codigo1.scintilla.Text = "";
                    codigo1.Changed = false;
                    UIStatics.FilePath = null;
                }
            } else {
                codigo1.scintilla.Text = "";
                codigo1.Changed = false;
                UIStatics.FilePath = null;
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e) {
            if (codigo1.Changed) {
                DialogResult dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?", "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes) {
                    if (UIStatics.FilePath != null) {
                        if (UIStatics.Save() == false) {
                            if (TrySave()) {
                                TryOpen();
                            }
                        }
                    } else {
                        if (TrySave()) {
                            TryOpen();
                        }
                    }
                } else if (dialogResult == DialogResult.No) {
                    TryOpen();
                }
            } else {
                TryOpen();
            }
        }

        private bool TryOpen() {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Projeto do simulador (*.m3mprj)|*.m3mprj|Todos os arquivos (*.*)|*.*";
            DialogResult fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) {
                return false;
            } else {
                UIStatics.FilePath = fileDialog.FileName;
                if (UIStatics.Open() == false) {
                    MessageBox.Show(this, "Ocorreu um erro ao carregar o projeto.", "Erro ao carregar o projeto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                } else {
                    codigo1.Changed = false;
                }
            }
            return true;
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e) {
            if (UIStatics.FilePath != null) {
                if (UIStatics.Save() == false) {
                    TrySave();
                }
            } else {
                TrySave();
            }
        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e) {
            TrySave();
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void propriedadesDoProjetoToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void ajudaToolStripMenuItem1_Click(object sender, EventArgs e) {

        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void somaRegistradoresToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Text = "MOV 0, A\nSOMA_A:\nADD 1, A\nJMPC SOMA_B\nJMP SOMA_A\nSOMA_B:\nMOV B, A\nADD 1, B\nJMPC SOMA_C\nMOV 0, A\nJMP SOMA_A\nSOMA_C:\nMOV C, A\nADD 1, C\nJMPC SOMA_D\nMOV 0, A\nJMP SOMA_A\nSOMA_D:\nMOV D, A\nADD 1, D\nJMPC SOMA_E\nMOV 0, A\nJMP SOMA_A\nSOMA_E:\nMOV E, A\nADD 1, E\nMOV 0, A\nJMP SOMA_A";
        }
    }
}
