namespace IDE {
    partial class Form1 {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.scintilla = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.AdditionalSelectionTyping = true;
            this.scintilla.AllowDrop = true;
            this.scintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla.Lexer = ScintillaNET.Lexer.Asm;
            this.scintilla.Location = new System.Drawing.Point(12, 32);
            this.scintilla.MultiPaste = ScintillaNET.MultiPaste.Each;
            this.scintilla.MultipleSelection = true;
            this.scintilla.Name = "scintilla";
            this.scintilla.ScrollWidth = 100;
            this.scintilla.Size = new System.Drawing.Size(552, 383);
            this.scintilla.TabIndex = 0;
            this.scintilla.UseTabs = true;
            this.scintilla.TextChanged += new System.EventHandler(this.scintilla_TextChanged);
            this.scintilla.Click += new System.EventHandler(this.scintilla_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 427);
            this.Controls.Add(this.scintilla);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla scintilla;
    }
}

