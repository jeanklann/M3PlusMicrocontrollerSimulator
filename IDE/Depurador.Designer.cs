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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.zCheck = new Components.CheckboxField();
            this.cCheck = new Components.CheckboxField();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.out1Field = new IDE.Components.DataField();
            this.out0Field = new IDE.Components.DataField();
            this.out2Field = new IDE.Components.DataField();
            this.out3Field = new IDE.Components.DataField();
            this.in1Field = new IDE.Components.DataField();
            this.in0Field = new IDE.Components.DataField();
            this.in2Field = new IDE.Components.DataField();
            this.in3Field = new IDE.Components.DataField();
            this.bField = new IDE.Components.DataField();
            this.aField = new IDE.Components.DataField();
            this.eField = new IDE.Components.DataField();
            this.cField = new IDE.Components.DataField();
            this.dField = new IDE.Components.DataField();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.scintilla.Size = new System.Drawing.Size(281, 457);
            this.scintilla.TabIndex = 2;
            this.scintilla.UseTabs = true;
            this.scintilla.MarginClick += new System.EventHandler<ScintillaNET.MarginClickEventArgs>(this.scintilla_MarginClick);
            this.scintilla.TextChanged += new System.EventHandler(this.scintilla_TextChanged);
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
            this.groupBox1.Location = new System.Drawing.Point(3, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 191);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registradores";
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
            this.groupBox2.Location = new System.Drawing.Point(452, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 153);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Entradas";
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
            this.groupBox3.Location = new System.Drawing.Point(452, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 153);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Saídas";
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.zCheck);
            this.groupBox4.Controls.Add(this.cCheck);
            this.groupBox4.Location = new System.Drawing.Point(3, 231);
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
            this.zCheck.UseVisualStyleBackColor = true;
            // 
            // cCheck
            // 
            this.cCheck.AutoSize = true;
            this.cCheck.Location = new System.Drawing.Point(13, 26);
            this.cCheck.Name = "cCheck";
            this.cCheck.Size = new System.Drawing.Size(50, 17);
            this.cCheck.TabIndex = 6;
            this.cCheck.Text = "Carry";
            this.cCheck.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::IDE.Properties.Resources.play_button;
            this.btnPlay.Location = new System.Drawing.Point(3, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(25, 25);
            this.btnPlay.TabIndex = 16;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Image = global::IDE.Properties.Resources.square;
            this.btnStop.Location = new System.Drawing.Point(65, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(25, 25);
            this.btnStop.TabIndex = 17;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Image = global::IDE.Properties.Resources.signs;
            this.btnPause.Location = new System.Drawing.Point(34, 3);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(25, 25);
            this.btnPause.TabIndex = 18;
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnIn
            // 
            this.btnIn.Image = global::IDE.Properties.Resources.download;
            this.btnIn.Location = new System.Drawing.Point(96, 3);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(25, 25);
            this.btnIn.TabIndex = 19;
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnOut
            // 
            this.btnOut.Image = global::IDE.Properties.Resources.upward;
            this.btnOut.Location = new System.Drawing.Point(127, 3);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(25, 25);
            this.btnOut.TabIndex = 20;
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
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
            this.out0Field.Value = 0;
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
            this.out3Field.Value = 0;
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
            this.in0Field.Value = 0;
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
            this.in3Field.Value = 0;
            // 
            // bField
            // 
            this.bField.ByteQuantity = ((byte)(1));
            this.bField.Location = new System.Drawing.Point(27, 52);
            this.bField.Name = "bField";
            this.bField.Selected = IDE.Components.DataFieldType.DEC;
            this.bField.Size = new System.Drawing.Size(119, 28);
            this.bField.TabIndex = 6;
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
            this.dField.Value = 0;
            // 
            // Depurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scintilla);
            this.Name = "Depurador";
            this.Size = new System.Drawing.Size(633, 463);
            this.Load += new System.EventHandler(this.Depurador_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnOut;
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
    }
}
