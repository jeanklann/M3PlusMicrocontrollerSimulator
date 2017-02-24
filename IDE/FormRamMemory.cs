using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE {
    public partial class FormRamMemory : Form {
        public const int HEIGHT = 30;
        public FormRamMemory() {
            InitializeComponent();
        }
        public List<Components.DataField> Fields = new List<Components.DataField>();

        public void Build(int quantity) {
            Fields.Clear();
            for (int i = 0; i < quantity; i++) {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, HEIGHT));

                Label label = new Label();
                label.Text = i.ToString();
                tableLayoutPanel1.Controls.Add(label, i, 0);

                Components.DataField field = new Components.DataField();
                Fields.Add(field);
                tableLayoutPanel1.Controls.Add(field, i, 1);
            }
        }
    }
}
