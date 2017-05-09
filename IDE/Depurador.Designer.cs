namespace IDE {
    partial class Depurador {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
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
            this.scintilla = new ScintillaNET.Scintilla();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bField = new IDE.Components.DataField();
            this.aField = new IDE.Components.DataField();
            this.eField = new IDE.Components.DataField();
            this.cField = new IDE.Components.DataField();
            this.dField = new IDE.Components.DataField();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.in1Field = new IDE.Components.DataField();
            this.in0Field = new IDE.Components.DataField();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.in2Field = new IDE.Components.DataField();
            this.in3Field = new IDE.Components.DataField();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.out1Field = new IDE.Components.DataField();
            this.out0Field = new IDE.Components.DataField();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.out2Field = new IDE.Components.DataField();
            this.out3Field = new IDE.Components.DataField();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.zCheck = new IDE.Components.CheckboxField();
            this.cCheck = new IDE.Components.CheckboxField();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyActive = new System.Windows.Forms.CheckBox();
            this.frequencyCombo = new System.Windows.Forms.ComboBox();
            this.internalSimulation = new System.Windows.Forms.CheckBox();
            this.abrirMemoriaRam = new System.Windows.Forms.Button();
            this.abrirMemoriaPilha = new System.Windows.Forms.Button();
            this.programCounter = new IDE.Components.DataField();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.realFrequency = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.abrirMemoriaROM = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.AdditionalSelectionTyping = true;
            this.scintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla.CaretLineVisible = true;
            this.scintilla.CaretStyle = ScintillaNET.CaretStyle.Invisible;
            this.scintilla.Lexer = ScintillaNET.Lexer.Asm;
            this.scintilla.Location = new System.Drawing.Point(165, 3);
            this.scintilla.MultiPaste = ScintillaNET.MultiPaste.Each;
            this.scintilla.MultipleSelection = true;
            this.scintilla.Name = "scintilla";
            this.scintilla.ReadOnly = true;
            this.scintilla.ScrollWidth = 100;
            this.scintilla.Size = new System.Drawing.Size(138, 457);
            this.scintilla.TabIndex = 2;
            this.scintilla.UseTabs = true;
            this.scintilla.MarginClick += new System.EventHandler<ScintillaNET.MarginClickEventArgs>(this.scintilla_MarginClick);
            this.scintilla.TextChanged += new System.EventHandler(this.scintilla_TextChanged);
            this.scintilla.Click += new System.EventHandler(this.scintilla_Click);
            this.scintilla.Leave += new System.EventHandler(this.scintilla_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "B";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "C";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "D";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "E";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bField);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.aField);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.eField);
            this.groupBox1.Controls.Add(this.cField);
            this.groupBox1.Controls.Add(this.dField);
            this.groupBox1.Location = new System.Drawing.Point(3, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 191);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registradores";
            // 
            // bField
            // 
            this.bField.ByteQuantity = ((byte)(1));
            this.bField.Location = new System.Drawing.Point(27, 52);
            this.bField.Name = "bField";
            this.bField.Selected = IDE.Components.DataFieldType.DEC;
            this.bField.Size = new System.Drawing.Size(119, 28);
            this.bField.TabIndex = 6;
            this.toolTip1.SetToolTip(this.bField, "Valor do registrador B");
            this.bField.UserInput = false;
            this.bField.Value = 0;
            // 
            // aField
            // 
            this.aField.ByteQuantity = ((byte)(1));
            this.aField.Location = new System.Drawing.Point(27, 19);
            this.aField.Name = "aField";
            this.aField.Selected = IDE.Components.DataFieldType.DEC;
            this.aField.Size = new System.Drawing.Size(119, 28);
            this.aField.TabIndex = 3;
            this.toolTip1.SetToolTip(this.aField, "Valor do registrador acumulador");
            this.aField.UserInput = false;
            this.aField.Value = 0;
            // 
            // eField
            // 
            this.eField.ByteQuantity = ((byte)(1));
            this.eField.Location = new System.Drawing.Point(27, 154);
            this.eField.Name = "eField";
            this.eField.Selected = IDE.Components.DataFieldType.DEC;
            this.eField.Size = new System.Drawing.Size(119, 28);
            this.eField.TabIndex = 9;
            this.toolTip1.SetToolTip(this.eField, "Valor do registrador E");
            this.eField.UserInput = false;
            this.eField.Value = 0;
            // 
            // cField
            // 
            this.cField.ByteQuantity = ((byte)(1));
            this.cField.Location = new System.Drawing.Point(27, 86);
            this.cField.Name = "cField";
            this.cField.Selected = IDE.Components.DataFieldType.DEC;
            this.cField.Size = new System.Drawing.Size(119, 28);
            this.cField.TabIndex = 7;
            this.toolTip1.SetToolTip(this.cField, "Valor do registrador C");
            this.cField.UserInput = false;
            this.cField.Value = 0;
            // 
            // dField
            // 
            this.dField.ByteQuantity = ((byte)(1));
            this.dField.Location = new System.Drawing.Point(27, 120);
            this.dField.Name = "dField";
            this.dField.Selected = IDE.Components.DataFieldType.DEC;
            this.dField.Size = new System.Drawing.Size(119, 28);
            this.dField.TabIndex = 8;
            this.toolTip1.SetToolTip(this.dField, "Valor do registrador D");
            this.dField.UserInput = false;
            this.dField.Value = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.in1Field);
            this.groupBox2.Controls.Add(this.in0Field);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.in2Field);
            this.groupBox2.Controls.Add(this.in3Field);
            this.groupBox2.Location = new System.Drawing.Point(309, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 153);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Entradas (somente leitura)";
            // 
            // in1Field
            // 
            this.in1Field.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in1Field.ByteQuantity = ((byte)(1));
            this.in1Field.Location = new System.Drawing.Point(40, 52);
            this.in1Field.Name = "in1Field";
            this.in1Field.Selected = IDE.Components.DataFieldType.DEC;
            this.in1Field.Size = new System.Drawing.Size(128, 28);
            this.in1Field.TabIndex = 6;
            this.toolTip1.SetToolTip(this.in1Field, "Valor da entrada IN2");
            this.in1Field.UserInput = false;
            this.in1Field.Value = 0;
            // 
            // in0Field
            // 
            this.in0Field.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in0Field.ByteQuantity = ((byte)(1));
            this.in0Field.Location = new System.Drawing.Point(40, 19);
            this.in0Field.Name = "in0Field";
            this.in0Field.Selected = IDE.Components.DataFieldType.DEC;
            this.in0Field.Size = new System.Drawing.Size(128, 28);
            this.in0Field.TabIndex = 3;
            this.toolTip1.SetToolTip(this.in0Field, "Valor da entrada IN1");
            this.in0Field.UserInput = false;
            this.in0Field.Value = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "IN3";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "IN0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "IN2";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "IN1";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // in2Field
            // 
            this.in2Field.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in2Field.ByteQuantity = ((byte)(1));
            this.in2Field.Location = new System.Drawing.Point(40, 86);
            this.in2Field.Name = "in2Field";
            this.in2Field.Selected = IDE.Components.DataFieldType.DEC;
            this.in2Field.Size = new System.Drawing.Size(128, 28);
            this.in2Field.TabIndex = 7;
            this.toolTip1.SetToolTip(this.in2Field, "Valor da entrada IN3");
            this.in2Field.UserInput = false;
            this.in2Field.Value = 0;
            // 
            // in3Field
            // 
            this.in3Field.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.in3Field.ByteQuantity = ((byte)(1));
            this.in3Field.Location = new System.Drawing.Point(40, 120);
            this.in3Field.Name = "in3Field";
            this.in3Field.Selected = IDE.Components.DataFieldType.DEC;
            this.in3Field.Size = new System.Drawing.Size(128, 28);
            this.in3Field.TabIndex = 8;
            this.toolTip1.SetToolTip(this.in3Field, "Valor da entrada IN4");
            this.in3Field.UserInput = false;
            this.in3Field.Value = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.out1Field);
            this.groupBox3.Controls.Add(this.out0Field);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.out2Field);
            this.groupBox3.Controls.Add(this.out3Field);
            this.groupBox3.Location = new System.Drawing.Point(309, 168);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 153);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Saídas (somente leitura)";
            // 
            // out1Field
            // 
            this.out1Field.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.out1Field.ByteQuantity = ((byte)(1));
            this.out1Field.Location = new System.Drawing.Point(49, 52);
            this.out1Field.Name = "out1Field";
            this.out1Field.Selected = IDE.Components.DataFieldType.DEC;
            this.out1Field.Size = new System.Drawing.Size(119, 28);
            this.out1Field.TabIndex = 6;
            this.toolTip1.SetToolTip(this.out1Field, "Valor da saída OUT2");
            this.out1Field.UserInput = false;
            this.out1Field.Value = 0;
            // 
            // out0Field
            // 
            this.out0Field.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.out0Field.ByteQuantity = ((byte)(1));
            this.out0Field.Location = new System.Drawing.Point(49, 19);
            this.out0Field.Name = "out0Field";
            this.out0Field.Selected = IDE.Components.DataFieldType.DEC;
            this.out0Field.Size = new System.Drawing.Size(119, 28);
            this.out0Field.TabIndex = 3;
            this.toolTip1.SetToolTip(this.out0Field, "Valor da saída OUT1");
            this.out0Field.UserInput = false;
            this.out0Field.Value = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "OUT3";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "OUT0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "OUT2";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "OUT1";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // out2Field
            // 
            this.out2Field.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.out2Field.ByteQuantity = ((byte)(1));
            this.out2Field.Location = new System.Drawing.Point(49, 86);
            this.out2Field.Name = "out2Field";
            this.out2Field.Selected = IDE.Components.DataFieldType.DEC;
            this.out2Field.Size = new System.Drawing.Size(119, 28);
            this.out2Field.TabIndex = 7;
            this.toolTip1.SetToolTip(this.out2Field, "Valor da saída OUT3");
            this.out2Field.UserInput = false;
            this.out2Field.Value = 0;
            // 
            // out3Field
            // 
            this.out3Field.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.out3Field.ByteQuantity = ((byte)(1));
            this.out3Field.Location = new System.Drawing.Point(49, 120);
            this.out3Field.Name = "out3Field";
            this.out3Field.Selected = IDE.Components.DataFieldType.DEC;
            this.out3Field.Size = new System.Drawing.Size(119, 28);
            this.out3Field.TabIndex = 8;
            this.toolTip1.SetToolTip(this.out3Field, "Valor da saída OUT4");
            this.out3Field.UserInput = false;
            this.out3Field.Value = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.zCheck);
            this.groupBox4.Controls.Add(this.cCheck);
            this.groupBox4.Location = new System.Drawing.Point(3, 362);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(156, 52);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flags";
            // 
            // zCheck
            // 
            this.zCheck.AutoSize = true;
            this.zCheck.Location = new System.Drawing.Point(98, 26);
            this.zCheck.Name = "zCheck";
            this.zCheck.Size = new System.Drawing.Size(48, 17);
            this.zCheck.TabIndex = 7;
            this.zCheck.Text = "Zero";
            this.toolTip1.SetToolTip(this.zCheck, "Flag zero: se o registrador está zerado");
            this.zCheck.UserInput = false;
            this.zCheck.UseVisualStyleBackColor = true;
            this.zCheck.Value = false;
            // 
            // cCheck
            // 
            this.cCheck.AutoSize = true;
            this.cCheck.Location = new System.Drawing.Point(13, 26);
            this.cCheck.Name = "cCheck";
            this.cCheck.Size = new System.Drawing.Size(50, 17);
            this.cCheck.TabIndex = 6;
            this.cCheck.Text = "Carry";
            this.toolTip1.SetToolTip(this.cCheck, "Flag carry: se excedeu o número de bits utilizado no registrador resultante");
            this.cCheck.UserInput = false;
            this.cCheck.UseVisualStyleBackColor = true;
            this.cCheck.Value = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 2;
            this.frequencyNumeric.Location = new System.Drawing.Point(6, 72);
            this.frequencyNumeric.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            this.frequencyNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(89, 20);
            this.frequencyNumeric.TabIndex = 21;
            this.toolTip1.SetToolTip(this.frequencyNumeric, "Frequência da simulação");
            this.frequencyNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequencyNumeric.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // frequencyActive
            // 
            this.frequencyActive.AutoSize = true;
            this.frequencyActive.Checked = true;
            this.frequencyActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.frequencyActive.Location = new System.Drawing.Point(6, 19);
            this.frequencyActive.Name = "frequencyActive";
            this.frequencyActive.Size = new System.Drawing.Size(109, 17);
            this.frequencyActive.TabIndex = 23;
            this.frequencyActive.Text = "Limitar frequência";
            this.toolTip1.SetToolTip(this.frequencyActive, "Limita ou não a frequência do simulador especificado abaixo");
            this.frequencyActive.UseVisualStyleBackColor = true;
            this.frequencyActive.CheckedChanged += new System.EventHandler(this.frequencyActive_CheckedChanged);
            // 
            // frequencyCombo
            // 
            this.frequencyCombo.FormattingEnabled = true;
            this.frequencyCombo.Items.AddRange(new object[] {
            "IPS",
            "kIPS",
            "MIPS"});
            this.frequencyCombo.Location = new System.Drawing.Point(101, 72);
            this.frequencyCombo.Name = "frequencyCombo";
            this.frequencyCombo.Size = new System.Drawing.Size(49, 21);
            this.frequencyCombo.TabIndex = 22;
            this.frequencyCombo.Text = "IPS";
            this.toolTip1.SetToolTip(this.frequencyCombo, "Em instruções por segundo, milhares de instruções por segundo, milhões de instruç" +
        "ões por segundo ou Hertz (para simulação interna).");
            this.frequencyCombo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // internalSimulation
            // 
            this.internalSimulation.AutoSize = true;
            this.internalSimulation.Location = new System.Drawing.Point(6, 42);
            this.internalSimulation.Name = "internalSimulation";
            this.internalSimulation.Size = new System.Drawing.Size(110, 17);
            this.internalSimulation.TabIndex = 25;
            this.internalSimulation.Text = "Simulação interna";
            this.toolTip1.SetToolTip(this.internalSimulation, "Ativa a simulação do circuito interno da M+++");
            this.internalSimulation.UseVisualStyleBackColor = true;
            this.internalSimulation.CheckedChanged += new System.EventHandler(this.internalSimulation_CheckedChanged);
            // 
            // abrirMemoriaRam
            // 
            this.abrirMemoriaRam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abrirMemoriaRam.Location = new System.Drawing.Point(309, 327);
            this.abrirMemoriaRam.Name = "abrirMemoriaRam";
            this.abrirMemoriaRam.Size = new System.Drawing.Size(178, 23);
            this.abrirMemoriaRam.TabIndex = 23;
            this.abrirMemoriaRam.Text = "Abrir memória RAM";
            this.toolTip1.SetToolTip(this.abrirMemoriaRam, "Abre a janela de visualização e edição da memória RAM");
            this.abrirMemoriaRam.UseVisualStyleBackColor = true;
            this.abrirMemoriaRam.Click += new System.EventHandler(this.abrirMemoriaRam_Click);
            // 
            // abrirMemoriaPilha
            // 
            this.abrirMemoriaPilha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abrirMemoriaPilha.Location = new System.Drawing.Point(309, 357);
            this.abrirMemoriaPilha.Name = "abrirMemoriaPilha";
            this.abrirMemoriaPilha.Size = new System.Drawing.Size(178, 23);
            this.abrirMemoriaPilha.TabIndex = 24;
            this.abrirMemoriaPilha.Text = "Abrir memória de pilha";
            this.toolTip1.SetToolTip(this.abrirMemoriaPilha, "Abre a janela de visualização e edição da memória de pilha");
            this.abrirMemoriaPilha.UseVisualStyleBackColor = true;
            this.abrirMemoriaPilha.Click += new System.EventHandler(this.abrirMemoriaPilha_Click);
            // 
            // programCounter
            // 
            this.programCounter.ByteQuantity = ((byte)(1));
            this.programCounter.Location = new System.Drawing.Point(36, 3);
            this.programCounter.Name = "programCounter";
            this.programCounter.Selected = IDE.Components.DataFieldType.DEC;
            this.programCounter.Size = new System.Drawing.Size(125, 28);
            this.programCounter.TabIndex = 25;
            this.toolTip1.SetToolTip(this.programCounter, "Program Counter (Contador de Programa)");
            this.programCounter.UserInput = false;
            this.programCounter.Value = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.internalSimulation);
            this.groupBox5.Controls.Add(this.realFrequency);
            this.groupBox5.Controls.Add(this.frequencyActive);
            this.groupBox5.Controls.Add(this.frequencyCombo);
            this.groupBox5.Controls.Add(this.frequencyNumeric);
            this.groupBox5.Location = new System.Drawing.Point(3, 37);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(156, 122);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Frequência";
            // 
            // realFrequency
            // 
            this.realFrequency.AutoSize = true;
            this.realFrequency.Location = new System.Drawing.Point(6, 99);
            this.realFrequency.Name = "realFrequency";
            this.realFrequency.Size = new System.Drawing.Size(105, 13);
            this.realFrequency.TabIndex = 24;
            this.realFrequency.Text = "Frequência real: 0Hz";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "PC: ";
            // 
            // abrirMemoriaROM
            // 
            this.abrirMemoriaROM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abrirMemoriaROM.Location = new System.Drawing.Point(309, 388);
            this.abrirMemoriaROM.Name = "abrirMemoriaROM";
            this.abrirMemoriaROM.Size = new System.Drawing.Size(178, 23);
            this.abrirMemoriaROM.TabIndex = 27;
            this.abrirMemoriaROM.Text = "Abrir memória ROM";
            this.toolTip1.SetToolTip(this.abrirMemoriaROM, "Abre a janela de visualização e edição da memória RAM");
            this.abrirMemoriaROM.UseVisualStyleBackColor = true;
            this.abrirMemoriaROM.Click += new System.EventHandler(this.abrirMemoriaROM_Click);
            // 
            // Depurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.abrirMemoriaROM);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.programCounter);
            this.Controls.Add(this.abrirMemoriaPilha);
            this.Controls.Add(this.abrirMemoriaRam);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scintilla);
            this.Name = "Depurador";
            this.Size = new System.Drawing.Size(490, 463);
            this.Load += new System.EventHandler(this.Depurador_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public ScintillaNET.Scintilla scintilla;
        public Components.DataField aField;
        public Components.DataField bField;
        public Components.DataField cField;
        public Components.DataField dField;
        public Components.DataField eField;
        public Components.CheckboxField cCheck;
        public Components.DataField in0Field;
        public Components.CheckboxField zCheck;
        public Components.DataField in1Field;
        public Components.DataField in2Field;
        public Components.DataField in3Field;
        public Components.DataField out1Field;
        public Components.DataField out0Field;
        public Components.DataField out2Field;
        public Components.DataField out3Field;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox frequencyCombo;
        private System.Windows.Forms.CheckBox frequencyActive;
        private System.Windows.Forms.Label realFrequency;
        private System.Windows.Forms.Button abrirMemoriaRam;
        private System.Windows.Forms.Button abrirMemoriaPilha;
        private System.Windows.Forms.CheckBox internalSimulation;
        private Components.DataField programCounter;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button abrirMemoriaROM;
    }
}
