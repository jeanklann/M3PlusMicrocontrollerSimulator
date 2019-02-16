using System;
using System.Windows.Forms;

namespace IDE.Components {
    public class CheckboxField:CheckBox, IComponent {
        private bool _needRefresh = true;
        private bool _userInput;
        private bool _refreshing;

        public bool UserInput { get => _userInput;
            set => _userInput = value;
        }
        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                if (value != this._value) {
                    this._value = value;
                    _needRefresh = true;
                }

            }
        }
        public override void Refresh() {
            if (_needRefresh) {
                _refreshing = true;

                Checked = _value;
                base.Refresh();
                _needRefresh = false;

                _refreshing = false;
            }
        }
        protected override void OnCheckedChanged(EventArgs e) {
            base.OnCheckedChanged(e);
            if (!_refreshing) {
                _value = Checked;
                _needRefresh = true;
                _userInput = true;
            }
        }
        /*
        protected override void OnValidating(CancelEventArgs e) {
            base.OnValidating(e);
            value = !value;
            userInput = true;
            needRefresh = true;
        }
        protected override void OnValidated(EventArgs e) {
            base.OnValidated(e);
            value = !value;
            userInput = true;
            needRefresh = true;
        }*/
    }
}
