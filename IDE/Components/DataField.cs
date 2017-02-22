using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IDE.Components {
    public partial class DataField : UserControl {
        public DataField() {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            Value = 0;
        }
        private int value = 0;
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (InputDec(value.ToString())) {
                    this.value = value;
                    if(selected == DataFieldType.DEC) {
                        maskedTextBox1.Text = this.value.ToString();
                    } else if(selected == DataFieldType.BIN) {
                        maskedTextBox1.Text = ToBIN();
                    } else if (selected == DataFieldType.HEX) {
                        maskedTextBox1.Text = ToHEX();
                    }
                }
            }
        }
        private DataFieldType selected = DataFieldType.DEC;
        public DataFieldType Selected {
            get {
                return selected;
            }
            set {
                switch (value) {
                    case DataFieldType.DEC: //DEC
                        if (byteQuantity == 1)
                            maskedTextBox1.Mask = "990";
                        else if (byteQuantity == 2)
                            maskedTextBox1.Mask = "99990";
                        else if (byteQuantity == 3)
                            maskedTextBox1.Mask = "99999990";
                        else if (byteQuantity == 4)
                            maskedTextBox1.Mask = "9999999990";

                        maskedTextBox1.Text = Value.ToString();
                        break;
                    case DataFieldType.BIN: //BIN
                        if (byteQuantity == 1)
                            maskedTextBox1.Mask = "00000000";
                        else if (byteQuantity == 2)
                            maskedTextBox1.Mask = "0000000000000000";
                        else if (byteQuantity == 3)
                            maskedTextBox1.Mask = "000000000000000000000000";
                        else if (byteQuantity == 4)
                            maskedTextBox1.Mask = "00000000000000000000000000000000";
                        maskedTextBox1.Text = ToBIN();
                        break;
                    case DataFieldType.HEX: //HEX
                        if (byteQuantity == 1)
                            maskedTextBox1.Mask = "AA";
                        else if (byteQuantity == 2)
                            maskedTextBox1.Mask = "AAAA";
                        else if (byteQuantity == 3)
                            maskedTextBox1.Mask = "AAAAAA";
                        else if (byteQuantity == 4)
                            maskedTextBox1.Mask = "AAAAAAAA";
                        maskedTextBox1.Text = ToHEX();
                        break;
                }
                selected = value;
            }
        }
        private byte byteQuantity = 1;
        public byte ByteQuantity
        {
            get { return byteQuantity; }
            set
            {
                if (value > 4)
                    byteQuantity = 4;
                else if (value < 1)
                    byteQuantity = 1;
                else
                    byteQuantity = value;

            }
        }

        private bool InputDec(string value) {
            int temp = -1;
            bool parsed = int.TryParse(value, out temp);
            if (!parsed) return false;
            if (temp < 0) return false;
            if (temp >= (byte.MaxValue + 1) * byteQuantity) return false;
            
            return true;
            
        }
        private bool InputBin(string value) {
            if (value.Length > 8 * byteQuantity) return false;
            int temp = -1;
            try { temp = Convert.ToInt32(value, 2); } catch (Exception) { };
            if (temp < 0) return false;
            
            return true;
        }
        private bool InputHex(string value) {
            if (value.Length > 2 * byteQuantity) return false;
            int temp = -1;
            try { temp = Convert.ToInt32(value, 16); } catch (Exception) { };
            if (temp < 0) return false;

            return true;
        }
        
        private string ToBIN() {
            string res = Convert.ToString(value, 2);
            int charQuant = 8 * byteQuantity;
            while(res.Length < charQuant) {
                res = "0" + res;
            }
            return res;
        }
        private string ToHEX() {
            string res = Convert.ToString(value, 16);
            int charQuant = 2 * byteQuantity;
            while (res.Length < charQuant) {
                res = "0" + res;
            }
            res = res.ToUpper();
            return res;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            switch (comboBox1.SelectedIndex) {
                case 0: //DEC
                    Selected = DataFieldType.DEC;
                    break;
                case 1: //BIN
                    Selected = DataFieldType.BIN;
                    break;
                case 2: //HEX
                    Selected = DataFieldType.HEX;
                    break;
            }
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e) {
            if(selected == DataFieldType.DEC) {
                if (InputDec(maskedTextBox1.Text)) {
                    value = int.Parse(maskedTextBox1.Text);
                } else {
                    maskedTextBox1.Text = value.ToString();
                }
            } else if(selected == DataFieldType.BIN) {
                if (InputBin(maskedTextBox1.Text)) {
                    value = Convert.ToInt32(maskedTextBox1.Text, 2);
                } else {
                    maskedTextBox1.Text = ToBIN();
                }
            } else if(selected == DataFieldType.HEX) {
                if (InputHex(maskedTextBox1.Text)) {
                    value = Convert.ToInt32(maskedTextBox1.Text, 16);
                } else {
                    maskedTextBox1.Text = ToHEX();
                }
            }
            
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char)13) { //Enter
                comboBox1.Focus();
            }
        }
    }
    public enum DataFieldType {
        DEC = 0, BIN = 1, HEX = 2,  
    }
}
