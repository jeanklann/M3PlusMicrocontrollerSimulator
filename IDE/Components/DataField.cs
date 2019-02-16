using System;
using System.Windows.Forms;

namespace IDE.Components {
    public partial class DataField : UserControl, IComponent {
        private bool _needRefresh = true;
        private bool _userInput;
        public bool UserInput { get => _userInput;
            set => _userInput = value;
        }

        public DataField() {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            Value = 0;
        }
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if(value != this._value) {
                    if(InputDec(value.ToString())) //if the new value is a valid value
                        this._value = value;
                    _needRefresh = true;
                }
                
            }
        }
        public override void Refresh() {
            if (_needRefresh) {
                if (_selected == DataFieldType.Dec) {
                    maskedTextBox1.Text = _value.ToString();
                } else if (_selected == DataFieldType.Bin) {
                    maskedTextBox1.Text = ToBin();
                } else if (_selected == DataFieldType.Hex) {
                    maskedTextBox1.Text = ToHex();
                }
                base.Refresh();
                _needRefresh = false;
            }
        }
        private DataFieldType _selected = DataFieldType.Dec;
        public DataFieldType Selected {
            get => _selected;
            set {
                switch (value) {
                    case DataFieldType.Dec: //DEC
                        if (_byteQuantity == 1)
                            maskedTextBox1.Mask = "990";
                        else if (_byteQuantity == 2)
                            maskedTextBox1.Mask = "99990";
                        else if (_byteQuantity == 3)
                            maskedTextBox1.Mask = "99999990";
                        else if (_byteQuantity == 4)
                            maskedTextBox1.Mask = "9999999990";
                        _needRefresh = true;
                        break;
                    case DataFieldType.Bin: //BIN
                        if (_byteQuantity == 1)
                            maskedTextBox1.Mask = "00000000";
                        else if (_byteQuantity == 2)
                            maskedTextBox1.Mask = "0000000000000000";
                        else if (_byteQuantity == 3)
                            maskedTextBox1.Mask = "000000000000000000000000";
                        else if (_byteQuantity == 4)
                            maskedTextBox1.Mask = "00000000000000000000000000000000";
                        _needRefresh = true;
                        break;
                    case DataFieldType.Hex: //HEX
                        if (_byteQuantity == 1)
                            maskedTextBox1.Mask = "AA";
                        else if (_byteQuantity == 2)
                            maskedTextBox1.Mask = "AAAA";
                        else if (_byteQuantity == 3)
                            maskedTextBox1.Mask = "AAAAAA";
                        else if (_byteQuantity == 4)
                            maskedTextBox1.Mask = "AAAAAAAA";
                        _needRefresh = true;
                        break;
                }
                _selected = value;
            }
        }
        private byte _byteQuantity = 1;
        public byte ByteQuantity
        {
            get => _byteQuantity;
            set
            {
                if (value > 4)
                    _byteQuantity = 4;
                else if (value < 1)
                    _byteQuantity = 1;
                else
                    _byteQuantity = value;

            }
        }

        private bool InputDec(string value) {
            var temp = -1;
            var parsed = int.TryParse(value, out temp);
            if (!parsed) return false;
            if (temp < 0) return false;
            if (temp >= (byte.MaxValue + 1) * _byteQuantity) return false;
            
            return true;
            
        }
        private bool InputBin(string value) {
            if (value.Length > 8 * _byteQuantity) return false;
            var temp = -1;
            try { temp = Convert.ToInt32(value, 2); } catch (Exception) { };
            if (temp < 0) return false;
            
            return true;
        }
        private bool InputHex(string value) {
            if (value.Length > 2 * _byteQuantity) return false;
            var temp = -1;
            try { temp = Convert.ToInt32(value, 16); } catch (Exception) { };
            if (temp < 0) return false;

            return true;
        }
        
        private string ToBin() {
            var res = Convert.ToString(_value, 2);
            var charQuant = 8 * _byteQuantity;
            while(res.Length < charQuant) {
                res = "0" + res;
            }
            return res;
        }
        private string ToHex() {
            var res = Convert.ToString(_value, 16);
            var charQuant = 2 * _byteQuantity;
            while (res.Length < charQuant) {
                res = "0" + res;
            }
            res = res.ToUpper();
            return res;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            switch (comboBox1.SelectedIndex) {
                case 0: //DEC
                    Selected = DataFieldType.Dec;
                    break;
                case 1: //BIN
                    Selected = DataFieldType.Bin;
                    break;
                case 2: //HEX
                    Selected = DataFieldType.Hex;
                    break;
            }
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e) {
            if(_selected == DataFieldType.Dec) {
                if (InputDec(maskedTextBox1.Text)) {
                    Value = int.Parse(maskedTextBox1.Text);
                    _userInput = true;
                } else {
                    _needRefresh = true;
                }
            } else if(_selected == DataFieldType.Bin) {
                if (InputBin(maskedTextBox1.Text)) {
                    Value = Convert.ToInt32(maskedTextBox1.Text, 2);
                    _userInput = true;
                } else {
                    _needRefresh = true;
                }
            } else if(_selected == DataFieldType.Hex) {
                if (InputHex(maskedTextBox1.Text)) {
                    Value = Convert.ToInt32(maskedTextBox1.Text, 16);
                    _userInput = true;
                } else {
                    _needRefresh = true;
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
        Dec = 0, Bin = 1, Hex = 2,  
    }
}
