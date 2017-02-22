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
            this.scintilla = new ScintillaNET.Scintilla();
            this.aField = new IDE.Components.DataField();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bField = new IDE.Components.DataField();
            this.cField = new IDE.Components.DataField();
            this.dField = new IDE.Components.DataField();
            this.eField = new IDE.Components.DataField();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
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
            this.scintilla.Size = new System.Drawing.Size(193, 429);
            this.scintilla.TabIndex = 2;
            this.scintilla.UseTabs = true;
            this.scintilla.MarginClick += new System.EventHandler<ScintillaNET.MarginClickEventArgs>(this.scintilla_MarginClick);
            this.scintilla.TextChanged += new System.EventHandler(this.scintilla_TextChanged);
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
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 191);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registradores";
            // 
            // Depurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scintilla);
            this.Name = "Depurador";
            this.Size = new System.Drawing.Size(591, 463);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}
