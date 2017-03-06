using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace IDE.Components {
    public class CheckboxField:CheckBox, Component {
        private bool needRefresh = true;
        private bool userInput = false;
        private bool refreshing = false;

        public bool UserInput { get { return userInput; } set { userInput = value; } }
        private bool value = false;
        public bool Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value != this.value) {
                    this.value = value;
                    needRefresh = true;
                }

            }
        }
        public override void Refresh() {
            if (needRefresh) {
                refreshing = true;

                Checked = value;
                base.Refresh();
                needRefresh = false;

                refreshing = false;
            }
        }
        protected override void OnCheckedChanged(EventArgs e) {
            base.OnCheckedChanged(e);
            if (!refreshing) {
                value = Checked;
                needRefresh = true;
                userInput = true;
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
