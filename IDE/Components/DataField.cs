using System;
using System.Windows.Forms;

namespace IDE.Components {
    public partial class DataField : UserControl, Component {
        private bool needRefresh = true;
        private bool userInput;
        public bool UserInput { get => userInput;
            set => userInput = value;
        }

        public DataField() {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            Value = 0;
        }
        private int value;
        public int Value
        {
            get => value;
            set
            {
                if(value != this.value) {
                    if(InputDec(value.ToString())) //if the new value is a valid value
                        this.value = value;
                    needRefresh = true;
                }
                
            }
        }
        public override void Refresh() {
            if (needRefresh) {
                if (selected == DataFieldType.DEC) {
                    maskedTextBox1.Text = value.ToString();
                } else if (selected == DataFieldType.BIN) {
                    maskedTextBox1.Text = ToBIN();
                } else if (selected == DataFieldType.HEX) {
                    maskedTextBox1.Text = ToHEX();
                }
                base.Refresh();
                needRefresh = false;
            }
        }
        private DataFieldType selected = DataFieldType.DEC;
        public DataFieldType Selected {
            get => selected;
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
                        needRefresh = true;
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
                        needRefresh = true;
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
                        needRefresh = true;
                        break;
                }
                selected = value;
            }
        }
        private byte byteQuantity = 1;
        public byte ByteQuantity
        {
            get => byteQuantity;
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
            var temp = -1;
            var parsed = int.TryParse(value, out temp);
            if (!parsed) return false;
            if (temp < 0) return false;
            if (temp >= (byte.MaxValue + 1) * byteQuantity) return false;
            
            return true;
            
        }
        private bool InputBin(string value) {
            if (value.Length > 8 * byteQuantity) return false;
            var temp = -1;
            try { temp = Convert.ToInt32(value, 2); } catch (Exception) { };
            if (temp < 0) return false;
            
            return true;
        }
        private bool InputHex(string value) {
            if (value.Length > 2 * byteQuantity) return false;
            var temp = -1;
            try { temp = Convert.ToInt32(value, 16); } catch (Exception) { };
            if (temp < 0) return false;

            return true;
        }
        
        private string ToBIN() {
            var res = Convert.ToString(value, 2);
            var charQuant = 8 * byteQuantity;
            while(res.Length < charQuant) {
                res = "0" + res;
            }
            return res;
        }
        private string ToHEX() {
            var res = Convert.ToString(value, 16);
            var charQuant = 2 * byteQuantity;
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
                    Value = int.Parse(maskedTextBox1.Text);
                    userInput = true;
                } else {
                    needRefresh = true;
                }
            } else if(selected == DataFieldType.BIN) {
                if (InputBin(maskedTextBox1.Text)) {
                    Value = Convert.ToInt32(maskedTextBox1.Text, 2);
                    userInput = true;
                } else {
                    needRefresh = true;
                }
            } else if(selected == DataFieldType.HEX) {
                if (InputHex(maskedTextBox1.Text)) {
                    Value = Convert.ToInt32(maskedTextBox1.Text, 16);
                    userInput = true;
                } else {
                    needRefresh = true;
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
