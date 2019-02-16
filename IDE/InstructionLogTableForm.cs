using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace IDE
{
    public partial class InstructionLogTableForm : Form
    {



        public InstructionLogTableForm()
        {
            InitializeComponent();
        }
        private Thread _thread;
        private bool _closing;
        public List<string> GetColumns()
        {
            var list = new List<string>();
            list.Add("Bus");
            list.Add("FlagC");
            list.Add("FlagZ");
            list.Add("EOI");
            list.Add("ROMrd");
            list.Add("ROMcs");
            list.Add("PCHbus");
            list.Add("PCLbus");
            list.Add("PCHclk");
            list.Add("PCLclk");
            list.Add("Data/PC sel");
            list.Add("DIRclk");
            list.Add("SPclk");
            list.Add("SPInc/Dc");
            list.Add("SPsel");
            list.Add("SPen");
            list.Add("Reset");
            list.Add("ULAbus");
            list.Add("BUFclk");
            list.Add("ACbus");
            list.Add("ACclk");
            list.Add("RGbus");
            list.Add("RG/PCcl");
            list.Add("RAMrd");
            list.Add("RAMwr");
            list.Add("RAMcs");
            list.Add("INbus");
            list.Add("OUTclk");
            list.Add("ULAop0");
            list.Add("ULAop1");
            list.Add("ULAop2");
            list.Add("RGPB0");
            list.Add("RGPB1");
            return list;
        }

        public void Clear()
        {
            foreach (DataGridViewRow rows in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in rows.Cells)
                {
                    if(cell.Value != null && cell.Value.ToString() != string.Empty)
                    {
                        cell.Value = string.Empty;
                    }
                }
            }
        }
        private void UpdateThread()
        {
            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var lastClear = UiStatics.Circuito.InstructionLog.ClearCount;
            while (!UiStatics.WantExit && !_closing)
            {
                try
                {
                    lock (dataGridView1)
                    {
                        if (UiStatics.Circuito.InstructionLog.ClearCount != lastClear)
                            Clear();
                        var instrucoes = UiStatics.Circuito.InstructionLog.ToList();
                        var linha = 0;
                        foreach (var instrucao in instrucoes)
                        {
                            if (dataGridView1.Rows.Count <= linha)
                                dataGridView1.Rows.Add();
                            var valor = string.Empty;
                            if (!string.IsNullOrEmpty(instrucao.Nome))
                                valor = $"Nome: {instrucao.Nome}, Quantidade de bytes: {instrucao.QuantidadeBytes}";
                            if (instrucao.ClocksNecessarios.HasValue) valor += $", Clocks necessários: {instrucao.ClocksNecessarios}";
                            if (dataGridView1.Rows[linha].Cells[0].Value == null ||
                                dataGridView1.Rows[linha].Cells[0].Value.ToString() != valor)
                                dataGridView1.Rows[linha].Cells[0].Value = valor;
                            if (dataGridView1.Rows[linha].Cells[0].Style == null)
                                dataGridView1.Rows[linha].Cells[0].Style = style;
                            ++linha;
                            foreach (var sinal in instrucao.Sinais)
                            {
                                if (dataGridView1.Rows.Count <= linha)
                                    dataGridView1.Rows.Add();
                                for (var i = 0; i < sinal.Count; i++)
                                {
                                    if (dataGridView1.Rows[linha].Cells[i].Style == null)
                                        dataGridView1.Rows[linha].Cells[i].Style = style;
                                    valor = sinal[i].Valor;
                                    if (dataGridView1.Rows[linha].Cells[i].Value == null ||
                                        dataGridView1.Rows[linha].Cells[i].Value.ToString() != valor)
                                        dataGridView1.Rows[linha].Cells[i].Value = valor;
                                }
                                ++linha;
                            }
                            if (instrucao.Sinais.Count == 0)
                            {
                                if (dataGridView1.Rows.Count <= linha)
                                    dataGridView1.Rows.Add();
                                if (dataGridView1.Rows[linha].Cells[0].Style == null)
                                    dataGridView1.Rows[linha].Cells[0].Style = style;
                                valor = instrucao.Texto;
                                if (dataGridView1.Rows[linha].Cells[0].Value == null ||
                                    dataGridView1.Rows[linha].Cells[0].Value.ToString() != valor)
                                    dataGridView1.Rows[linha].Cells[0].Value = valor;
                                ++linha;
                            }
                        }
                    }
                }
                catch (Exception) { }
                Thread.Sleep(1000);
            }
        }

        private void InstructionLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
        }

        private void InstructionLogTableForm_Load(object sender, EventArgs e)
        {
            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var columns = GetColumns();
            foreach (var item in columns)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = item,
                    HeaderText = item,
                    ReadOnly = true,
                    Resizable = DataGridViewTriState.False,
                    FillWeight = item == "Bus" ? 20 : 10,
                    DefaultCellStyle = style,
                    ToolTipText = item,
                });
            }
            for (var i = 0; i < 150; i++)
            {
                dataGridView1.Rows.Add();
                for (var j = 0; j < columns.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].ToolTipText = columns[j];
                }
                
            }

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            _thread = new Thread(UpdateThread);
            _thread.Start();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (value != null && value.ToString().Length > 3)
                MessageBox.Show("Conteúdo da célula: " + value.ToString());
        }
    }
}
