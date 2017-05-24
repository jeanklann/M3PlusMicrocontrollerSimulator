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
        private Thread loadThread;
        public FormularioPrincipal() {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            UIStatics.Codigo = (Codigo)codigo1;
            UIStatics.Depurador = (Depurador)depurador1;
            UIStatics.Circuito = (Circuito)circuito1;
            UIStatics.MainForm = this;

            UIStatics.threadDepurador = new Thread(UIStatics.Depurador.UpdateAll);
            UIStatics.threadDepurador.Start();

        }
        private void LoadThread() {
            
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {

        }


        private void scintilla_Click(object sender, EventArgs e) {

        }



        private void reportarToolStripMenuItem_Click(object sender, EventArgs e) {
            ExceptionLog exceptionLog = new ExceptionLog();
            exceptionLog.Text = "Reportar um problema";
            exceptionLog.Show(this);
        }
        private void verPróximoToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.GotoNextBreakpoint();
        }

        private void verAnteriorToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.GotoPreviousBreakpoint();
        }

        private void removerTodosToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.RemoveAllBreakpoint();
        }

        private void adicionarNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.AddBreakpoint();
        }

        private void removerNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.RemoveBreakpoint();
        }

        private void Form1_Load(object sender, EventArgs e) {
            tabControl1.SelectTab(1);
            while (!UIStatics.Circuito.Loaded) {
                Thread.Sleep(10);
            }
            tabControl1.SelectTab(0);
        }

        private void codigo1_Load(object sender, EventArgs e) {

        }

        private void códigoToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void depuraçãoToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void circuitoToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomMore();
                UIStatics.Depurador.ZoomMore();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomMore();
            }
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomLess();
                UIStatics.Depurador.ZoomLess();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomLess();

            }
        }

        private void zoomOriginalToolStripMenuItem_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomReset();
                UIStatics.Depurador.ZoomReset();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomReset();
            }
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
            UIStatics.Codigo.scintilla.Text = "apagado:\nmov IN3,a\nand 20,a\njmpz apagado\npisca:\nmov 01,a\nmov a,out0\nmov 00,a\nmov a,out0\njmp pisca";
        }

        private void FormularioPrincipal_FormClosed(object sender, FormClosedEventArgs e) {
            if(UIStatics.Simulador != null) {
                UIStatics.Simulador.Stop();
            }
            UIStatics.WantExit = true;
        }

        private void FormularioPrincipal_FormClosing(object sender, FormClosingEventArgs e) {
            if (UIStatics.Codigo.Changed || UIStatics.Circuito.Changed) {
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
                            UIStatics.WantExit = true;
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
                    UIStatics.WantExit = true;
                    return;
                }
            } else {
                UIStatics.WantExit = true;
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
                    UIStatics.Codigo.Changed = false;
                    UIStatics.Circuito.Changed = false;
                }
            }
            return true;
        }
        

        private void desfazerToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Undo();
        }

        private void refazerToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Redo();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Cut();
        }

        private void copierToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Copy();
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Paste();
        }

        private void selecionarTudoToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.SelectAll();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e) {
            New();
        }

        private void New() {
            if (UIStatics.Simulador != null) return;
            if (UIStatics.Codigo.Changed || UIStatics.Circuito.Changed) {
                DialogResult dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?", "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes) {
                    if (UIStatics.FilePath != null) {
                        if (UIStatics.Save() == false) {
                            if (TrySave()) {
                                UIStatics.Codigo.scintilla.Text = "";
                                UIStatics.Codigo.Changed = false;
                                UIStatics.Circuito.Changed = false;
                                UIStatics.FilePath = null;
                            }
                        }
                    } else {
                        if (TrySave()) {
                            UIStatics.Codigo.scintilla.Text = "";
                            UIStatics.Codigo.Changed = false;
                            UIStatics.Circuito.Changed = false;
                            UIStatics.FilePath = null;
                        }
                    }
                } else if (dialogResult == DialogResult.No) {
                    UIStatics.Codigo.scintilla.Text = "";
                    UIStatics.Codigo.Changed = false;
                    UIStatics.Circuito.Changed = false;
                    UIStatics.FilePath = null;
                }
            } else {
                UIStatics.Codigo.scintilla.Text = "";
                UIStatics.Codigo.Changed = false;
                UIStatics.Circuito.Changed = false;
                UIStatics.FilePath = null;
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e) {
            Open();
        }

        private void Open() {
            if (UIStatics.Codigo.Changed) {
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
                    UIStatics.Codigo.Changed = false;
                    UIStatics.Circuito.Changed = false;
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
            Ajuda ajuda = new Ajuda();
            ajuda.Show(this);
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e) {
            Sobre sobre = new Sobre();
            sobre.ShowDialog(this);
        }

        private void somaRegistradoresToolStripMenuItem_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Text = "MOV 00, A\nSOMA_A:\nADD 01, A\nJMPC SOMA_B\nJMP SOMA_A\nSOMA_B:\nMOV B, A\nADD 01, B\nJMPC SOMA_C\nMOV 00, A\nJMP SOMA_A\nSOMA_C:\nMOV C, A\nADD 01, C\nJMPC SOMA_D\nMOV 00, A\nJMP SOMA_A\nSOMA_D:\nMOV D, A\nADD 01, D\nJMPC SOMA_E\nMOV 00, A\nJMP SOMA_A\nSOMA_E:\nMOV E, A\nADD 01, E\nMOV 00, A\nJMP SOMA_A";
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Arquivo binário (*.bin)|*.bin|Arquivo hexadecimal (*.hex)|*.hex|Arquivo do logisim (*.mmmp)|*.mmmp|Todos os arquivos (*.*)|*.*";
            DialogResult fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) {
                return;
            } else {
                if (fileDialog.FileName.EndsWith(".bin")){
                    if (UIStatics.ExportBinary(fileDialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao exportar.", "Erro ao exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (fileDialog.FileName.EndsWith(".hex")) {
                    if (UIStatics.ExportHex(fileDialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao exportar.", "Erro ao exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (fileDialog.FileName.EndsWith(".mmmp")) {
                    if (UIStatics.ExportLogiSim(fileDialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao exportar.", "Erro ao exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void importarToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Arquivo binário (*.bin)|*.bin|Arquivo hexadecimal (*.hex)|*.hex|Arquivo do logisim (*.mmmp)|*.mmmp|Todos os arquivos (*.*)|*.*";
            DialogResult fileDialogResult = dialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) {
                return;
            } else {
                if (dialog.FileName.EndsWith(".bin")) {
                    if (UIStatics.ImportBinary(dialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao importar.", "Erro ao importar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (dialog.FileName.EndsWith(".hex")) {
                    if (UIStatics.ImportHex(dialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao importar.", "Erro ao importar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (dialog.FileName.EndsWith(".mmmp")) {
                    if (UIStatics.ImportLogiSim(dialog.FileName) == false)
                        MessageBox.Show(this, "Ocorreu um erro ao importar.", "Erro ao importar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            UIStatics.Run();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            UIStatics.Pause();
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            UIStatics.Stop();
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            UIStatics.StepIn();
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            UIStatics.StepOut();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void toolStripButton6_Click(object sender, EventArgs e) {
            UIStatics.Compile();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void toolStripButton8_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Undo();
        }

        private void toolStripButton9_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Redo();
        }

        private void toolStripButton14_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Copy();
        }

        private void toolStripButton15_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Cut();
        }

        private void toolStripButton7_Click(object sender, EventArgs e) {
            UIStatics.Codigo.scintilla.Paste();
        }

        private void toolStripButton10_Click(object sender, EventArgs e) {
            New();
        }

        private void toolStripButton11_Click(object sender, EventArgs e) {
            Open();
        }

        private void toolStripButton12_Click(object sender, EventArgs e) {
            if (UIStatics.FilePath != null) {
                if (UIStatics.Save() == false) {
                    TrySave();
                }
            } else {
                TrySave();
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e) {
            TrySave();
        }

        private void toolStripButton16_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomMore();
                UIStatics.Depurador.ZoomMore();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomMore();
            }
        }

        private void toolStripButton17_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomLess();
                UIStatics.Depurador.ZoomLess();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomLess();

            }
        }

        private void toolStripButton18_Click(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                UIStatics.Codigo.ZoomReset();
                UIStatics.Depurador.ZoomReset();
            } else if (tabControl1.SelectedIndex == 1) {
                UIStatics.Circuito.ZoomReset();
            }
        }
    }
}
