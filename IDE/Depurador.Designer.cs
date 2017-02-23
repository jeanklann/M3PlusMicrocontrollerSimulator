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
            this.dataField1 = new IDE.Components.DataField();
            this.dataField2 = new IDE.Components.DataField();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dataField4 = new IDE.Components.DataField();
            this.dataField5 = new IDE.Components.DataField();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataField3 = new IDE.Components.DataField();
            this.dataField6 = new IDE.Components.DataField();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dataField7 = new IDE.Components.DataField();
            this.dataField8 = new IDE.Components.DataField();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataField1);
            this.groupBox2.Controls.Add(this.dataField2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.dataField4);
            this.groupBox2.Controls.Add(this.dataField5);
            this.groupBox2.Location = new System.Drawing.Point(452, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 153);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Entradas";
            // 
            // dataField1
            // 
            this.dataField1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField1.ByteQuantity = ((byte)(1));
            this.dataField1.Location = new System.Drawing.Point(40, 52);
            this.dataField1.Name = "dataField1";
            this.dataField1.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField1.Size = new System.Drawing.Size(128, 28);
            this.dataField1.TabIndex = 6;
            this.dataField1.Value = 0;
            // 
            // dataField2
            // 
            this.dataField2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField2.ByteQuantity = ((byte)(1));
            this.dataField2.Location = new System.Drawing.Point(40, 19);
            this.dataField2.Name = "dataField2";
            this.dataField2.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField2.Size = new System.Drawing.Size(128, 28);
            this.dataField2.TabIndex = 3;
            this.dataField2.Value = 0;
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
            // dataField4
            // 
            this.dataField4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField4.ByteQuantity = ((byte)(1));
            this.dataField4.Location = new System.Drawing.Point(40, 86);
            this.dataField4.Name = "dataField4";
            this.dataField4.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField4.Size = new System.Drawing.Size(128, 28);
            this.dataField4.TabIndex = 7;
            this.dataField4.Value = 0;
            // 
            // dataField5
            // 
            this.dataField5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField5.ByteQuantity = ((byte)(1));
            this.dataField5.Location = new System.Drawing.Point(40, 120);
            this.dataField5.Name = "dataField5";
            this.dataField5.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField5.Size = new System.Drawing.Size(128, 28);
            this.dataField5.TabIndex = 8;
            this.dataField5.Value = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dataField3);
            this.groupBox3.Controls.Add(this.dataField6);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.dataField7);
            this.groupBox3.Controls.Add(this.dataField8);
            this.groupBox3.Location = new System.Drawing.Point(452, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 153);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Saídas";
            // 
            // dataField3
            // 
            this.dataField3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField3.ByteQuantity = ((byte)(1));
            this.dataField3.Location = new System.Drawing.Point(49, 52);
            this.dataField3.Name = "dataField3";
            this.dataField3.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField3.Size = new System.Drawing.Size(119, 28);
            this.dataField3.TabIndex = 6;
            this.dataField3.Value = 0;
            // 
            // dataField6
            // 
            this.dataField6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField6.ByteQuantity = ((byte)(1));
            this.dataField6.Location = new System.Drawing.Point(49, 19);
            this.dataField6.Name = "dataField6";
            this.dataField6.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField6.Size = new System.Drawing.Size(119, 28);
            this.dataField6.TabIndex = 3;
            this.dataField6.Value = 0;
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
            // dataField7
            // 
            this.dataField7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField7.ByteQuantity = ((byte)(1));
            this.dataField7.Location = new System.Drawing.Point(49, 86);
            this.dataField7.Name = "dataField7";
            this.dataField7.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField7.Size = new System.Drawing.Size(119, 28);
            this.dataField7.TabIndex = 7;
            this.dataField7.Value = 0;
            // 
            // dataField8
            // 
            this.dataField8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataField8.ByteQuantity = ((byte)(1));
            this.dataField8.Location = new System.Drawing.Point(49, 120);
            this.dataField8.Name = "dataField8";
            this.dataField8.Selected = IDE.Components.DataFieldType.DEC;
            this.dataField8.Size = new System.Drawing.Size(119, 28);
            this.dataField8.TabIndex = 8;
            this.dataField8.Value = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(3, 231);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(156, 52);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Flags";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(98, 26);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(48, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Zero";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Carry";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Image = global::IDE.Properties.Resources.play_button;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 16;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Image = global::IDE.Properties.Resources.square;
            this.button2.Location = new System.Drawing.Point(65, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 25);
            this.button2.TabIndex = 17;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Image = global::IDE.Properties.Resources.signs;
            this.button3.Location = new System.Drawing.Point(34, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 25);
            this.button3.TabIndex = 18;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Image = global::IDE.Properties.Resources.download;
            this.button4.Location = new System.Drawing.Point(96, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(25, 25);
            this.button4.TabIndex = 19;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Image = global::IDE.Properties.Resources.upward;
            this.button5.Location = new System.Drawing.Point(127, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(25, 25);
            this.button5.TabIndex = 20;
            this.button5.UseVisualStyleBackColor = true;
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 26);
            // 
            // Depurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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

        private ScintillaNET.Scintilla scintilla;
        private Components.DataField aField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Components.DataField bField;
        private Components.DataField cField;
        private Components.DataField dField;
        private Components.DataField eField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Components.DataField dataField1;
        private Components.DataField dataField2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private Components.DataField dataField4;
        private Components.DataField dataField5;
        private System.Windows.Forms.GroupBox groupBox3;
        private Components.DataField dataField3;
        private Components.DataField dataField6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private Components.DataField dataField7;
        private Components.DataField dataField8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
