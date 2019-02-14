using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDE {
    public partial class Sobre : Form {
        public Sobre() {
            InitializeComponent();
            textBox1.Text = string.Format(textBox1.Text, Application.ProductVersion);
        }
    }
}
