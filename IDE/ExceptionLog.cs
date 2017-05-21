﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDE {
    public partial class ExceptionLog : Form {
        public ExceptionLog(Exception e = null) {
            InitializeComponent();
            textBox1.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\r\n";
            textBox1.Text += "Código-fonte digitado: \r\n========================\r\n" + UIStatics.Codigo.scintilla.Text.Replace("\n","\r\n") + "\r\n========================\r\n";
            if(UIStatics.Simulador != null) {
                textBox1.Text += "Programa compilado: \r\n========================\r\n" + UIStatics.Depurador.scintilla.Text + "\r\n========================\r\n";
                textBox1.Text += "Dados da simulação:\r\n";
                textBox1.Text += "Simulação rodando: " + (UIStatics.Simulador.Running ? "sim" : "não") + "\r\n";
                textBox1.Text += "PC: " + (UIStatics.Simulador.NextInstruction) + "\r\n";
                textBox1.Text += "Simulação interna: " + (UIStatics.Simulador.InternalSimulation ? "sim" : "não") + "\r\n";
                textBox1.Text += "Registrador A: " + (UIStatics.Simulador.Reg[0]) + "\r\n";
                textBox1.Text += "Registrador B: " + (UIStatics.Simulador.Reg[1]) + "\r\n";
                textBox1.Text += "Registrador C: " + (UIStatics.Simulador.Reg[2]) + "\r\n";
                textBox1.Text += "Registrador D: " + (UIStatics.Simulador.Reg[3]) + "\r\n";
                textBox1.Text += "Registrador E: " + (UIStatics.Simulador.Reg[4]) + "\r\n";
                textBox1.Text += "IN0: " + (UIStatics.Simulador.In[0]) + "\r\n";
                textBox1.Text += "IN1: " + (UIStatics.Simulador.In[1]) + "\r\n";
                textBox1.Text += "IN2: " + (UIStatics.Simulador.In[2]) + "\r\n";
                textBox1.Text += "IN3: " + (UIStatics.Simulador.In[3]) + "\r\n";
                textBox1.Text += "Out0: " + (UIStatics.Simulador.Out[0]) + "\r\n";
                textBox1.Text += "Out1: " + (UIStatics.Simulador.Out[1]) + "\r\n";
                textBox1.Text += "Out2: " + (UIStatics.Simulador.Out[2]) + "\r\n";
                textBox1.Text += "Out3: " + (UIStatics.Simulador.Out[3]) + "\r\n";
            }
            if (e != null) {
                textBox1.Text += "\r\n========================\r\n";
                textBox1.Text += "Exception message:\r\n";
                textBox1.Text += e.Message + "\r\n";
                textBox1.Text += "Stack trace:\r\n";
                textBox1.Text += e.StackTrace + "\r\n";
            }
            textBox1.Text += "\r\n========================\r\n";
        }

        private void button1_Click(object sender, EventArgs e) {
            Hide();
        }

        private void button2_Click(object sender, EventArgs e) {
            save();
        }
        private bool save() {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Log de erro (*.log)|*.log|Todos os arquivos (*.*)|*.*";
            DialogResult fileDialogResult = fileDialog.ShowDialog(this);
            if (fileDialogResult == DialogResult.Cancel ||
                fileDialogResult == DialogResult.Abort ||
                fileDialogResult == DialogResult.None) {
                return false;
            } else {
                string res = textBox1.Text;
                res += "Mensagem do usuário:\r\n";
                res += textBox2.Text;
                res += "\r\n========================\r\n";
                try {
                    File.AppendAllText(fileDialog.FileName, res);
                } catch (Exception e) {
                    MessageBox.Show(this, "Ocorreu um erro ao salvar o log.", "Erro ao salvar o log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
    }
}
