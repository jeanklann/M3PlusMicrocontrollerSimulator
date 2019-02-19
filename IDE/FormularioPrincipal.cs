using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace IDE
{
    public partial class FormularioPrincipal : Form
    {
        public FormularioPrincipal()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            UiStatics.Codigo = (Codigo) codigo1;
            UiStatics.Depurador = (Depurador) depurador1;
            UiStatics.Circuito = (Circuito) circuito1;
            UiStatics.MainForm = this;

            UiStatics.ThreadDepurador = new Thread(UiStatics.Depurador.UpdateAll);
            UiStatics.ThreadDepurador.Start();
        }


        private void reportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exceptionLog = new ExceptionLog();
            exceptionLog.Text = "Reportar um problema";
            exceptionLog.Show(this);
        }

        private void verPróximoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.GotoNextBreakpoint();
        }

        private void verAnteriorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.GotoPreviousBreakpoint();
        }

        private void removerTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.RemoveAllBreakpoint();
        }

        private void adicionarNaLinhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.AddBreakpoint();
        }

        private void removerNaLinhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.RemoveBreakpoint();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
            while (!UiStatics.Circuito.Loaded) Thread.Sleep(10);
            tabControl1.SelectTab(0);
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomMore();
                UiStatics.Depurador.ZoomMore();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomMore();
            }
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomLess();
                UiStatics.Depurador.ZoomLess();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomLess();
            }
        }

        private void zoomOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomReset();
                UiStatics.Depurador.ZoomReset();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomReset();
            }
        }

        private void analisarEConstruirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Compile();
        }

        private void rodarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Run();
        }

        private void pausarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Pause();
        }

        private void pularParaDentroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.StepIn();
        }

        private void pularParaForaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.StepOut();
        }

        private void pararToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Stop();
        }

        private void FormularioPrincipal_Leave(object sender, EventArgs e)
        {
            UiStatics.WantExit = true;
        }

        private void piscaLedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Text =
@"apagado:
mov IN3,a
and 20,a
jmpz apagado
pisca:
mov 01,a
mov a,out0
mov 00,a
mov a,out0
jmp pisca";
        }

        private void FormularioPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UiStatics.Simulador != null) UiStatics.Simulador.Stop();
            UiStatics.WantExit = true;
        }

        private void FormularioPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UiStatics.Codigo.Changed || UiStatics.Circuito.Changed)
            {
                var dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?",
                    "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    if (UiStatics.FilePath != null)
                    {
                        if (UiStatics.Save() == false)
                        {
                            if (!TrySave())
                            {
                                e.Cancel = true;
                                return;
                            }

                            UiStatics.WantExit = true;
                        }
                    }
                    else
                    {
                        if (!TrySave())
                            e.Cancel = true;
                        else
                            return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    UiStatics.WantExit = true;
                }
            }
            else
            {
                UiStatics.WantExit = true;
            }
        }

        private bool TrySave()
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Projeto do simulador (*.m3mprj)|*.m3mprj|Todos os arquivos (*.*)|*.*";
            var fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None)
                return false;

            UiStatics.FilePath = fileDialog.FileName;
            if (UiStatics.Save() == false)
            {
                MessageBox.Show(this, "Ocorreu um erro ao salvar o projeto.", "Erro ao salvar o projeto",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            UiStatics.Codigo.Changed = false;
            UiStatics.Circuito.Changed = false;
            return true;
        }


        private void desfazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Undo();
        }

        private void refazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Redo();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Cut();
        }

        private void copierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Copy();
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Paste();
        }

        private void selecionarTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.SelectAll();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        private void New()
        {
            if (UiStatics.Simulador != null && UiStatics.Simulador.Running) return;
            UiStatics.Simulador = null;
            if (UiStatics.Codigo.Changed || UiStatics.Circuito.Changed)
            {
                var dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?",
                    "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    if (UiStatics.FilePath != null)
                    {
                        if (UiStatics.Save()) return;
                        if (!TrySave()) return;
                        UiStatics.Codigo.scintilla.Text = "";
                        FileProject.Load(Application.StartupPath + "\\Default.m3mprj");
                        UiStatics.Codigo.Changed = false;
                        UiStatics.Circuito.Changed = false;
                        UiStatics.FilePath = null;
                    }
                    else
                    {
                        if (!TrySave()) return;
                        UiStatics.Codigo.scintilla.Text = "";
                        FileProject.Load(Application.StartupPath + "\\Default.m3mprj");
                        UiStatics.Codigo.Changed = false;
                        UiStatics.Circuito.Changed = false;
                        UiStatics.FilePath = null;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    UiStatics.Codigo.scintilla.Text = "";
                    FileProject.Load(Application.StartupPath + "\\Default.m3mprj");
                    UiStatics.Codigo.Changed = false;
                    UiStatics.Circuito.Changed = false;
                    UiStatics.FilePath = null;
                }
            }
            else
            {
                UiStatics.Codigo.scintilla.Text = "";
                FileProject.Load(Application.StartupPath + "\\Default.m3mprj");
                UiStatics.Codigo.Changed = false;
                UiStatics.Circuito.Changed = false;
                UiStatics.FilePath = null;
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            if (UiStatics.Codigo.Changed)
            {
                var dialogResult = MessageBox.Show(this, "Você tem alterações não salvas neste projeto, deseja salvar?",
                    "Salvar projeto", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    if (UiStatics.FilePath != null)
                    {
                        if (UiStatics.Save() == false)
                            if (TrySave())
                                TryOpen();
                    }
                    else
                    {
                        if (TrySave()) TryOpen();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    TryOpen();
                }
            }
            else
            {
                TryOpen();
            }
        }

        private bool TryOpen()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Projeto do simulador (*.m3mprj)|*.m3mprj|Todos os arquivos (*.*)|*.*";
            var fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None)
                return false;

            UiStatics.FilePath = fileDialog.FileName;
            if (UiStatics.Open() == false)
            {
                MessageBox.Show(this, "Ocorreu um erro ao carregar o projeto.", "Erro ao carregar o projeto",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            UiStatics.Codigo.Changed = false;
            UiStatics.Circuito.Changed = false;
            return true;
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UiStatics.FilePath != null)
            {
                if (UiStatics.Save() == false) TrySave();
            }
            else
            {
                TrySave();
            }
        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void propriedadesDoProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ajudaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var ajuda = new Ajuda();
            ajuda.Show(this);
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sobre = new Sobre();
            sobre.ShowDialog(this);
        }

        private void somaRegistradoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Text =
@"MOV 00, A
SOMA_A:
ADD 01, A
JMPC SOMA_B
JMP SOMA_A
SOMA_B:
MOV B, A
ADD 01, B
JMPC SOMA_C
MOV 00, A
JMP SOMA_A
SOMA_C:
MOV C, A
ADD 01, C
JMPC SOMA_D
MOV 00, A
JMP SOMA_A
SOMA_D:
MOV D, A
ADD 01, D
JMPC SOMA_E
MOV 00, A
JMP SOMA_A
SOMA_E:
MOV E, A
ADD 01, E
MOV 00, A
JMP SOMA_A";
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter =
                "Arquivo binário (*.bin)|*.bin|Arquivo hexadecimal (*.hex)|*.hex|Arquivo do logisim (*.mmmp)|*.mmmp|Todos os arquivos (*.*)|*.*";
            var fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None)
            {
            }
            else
            {
                try
                {
                    if (fileDialog.FileName.EndsWith(".bin"))
                        UiStatics.ExportBinary(fileDialog.FileName);
                    else if (fileDialog.FileName.EndsWith(".hex"))
                        UiStatics.ExportHex(fileDialog.FileName);
                    else if (fileDialog.FileName.EndsWith(".mmmp"))
                        UiStatics.ExportLogiSim(fileDialog.FileName);
                }
                catch (Exception e2)
                {
                    MessageBox.Show(this, $"Ocorreu um erro ao exportar. Erro: {e2.Message}", "Erro ao exportar", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter =
                "Arquivo binário (*.bin)|*.bin|Arquivo hexadecimal (*.hex)|*.hex|Arquivo do logisim (*.mmmp)|*.mmmp|Todos os arquivos (*.*)|*.*";
            var fileDialogResult = dialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel || fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) return;
            try
            {
                if (dialog.FileName.EndsWith(".bin"))
                    UiStatics.ImportBinary(dialog.FileName);
                else if (dialog.FileName.EndsWith(".hex"))
                    UiStatics.ImportHex(dialog.FileName);
                else if (dialog.FileName.EndsWith(".mmmp"))
                    UiStatics.ImportLogiSim(dialog.FileName);
            }
            catch (Exception e2)
            {
                MessageBox.Show(this, $"Ocorreu um erro ao importar o arquivo. Erro: {e2.Message}", "Erro ao importar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UiStatics.Run();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            UiStatics.Pause();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            UiStatics.Stop();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            UiStatics.StepIn();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            UiStatics.StepOut();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            UiStatics.Compile();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Undo();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Redo();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Copy();
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Cut();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            UiStatics.Codigo.scintilla.Paste();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            New();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (UiStatics.FilePath != null)
            {
                if (UiStatics.Save() == false) TrySave();
            }
            else
            {
                TrySave();
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomMore();
                UiStatics.Depurador.ZoomMore();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomMore();
            }
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomLess();
                UiStatics.Depurador.ZoomLess();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomLess();
            }
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                UiStatics.Codigo.ZoomReset();
                UiStatics.Depurador.ZoomReset();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UiStatics.Circuito.ZoomReset();
            }
        }

        private void logDasIntruçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //InstructionLogForm form = new InstructionLogForm();
            var form = new InstructionLogTableForm();
            form.Show(this);
        }
    }
}