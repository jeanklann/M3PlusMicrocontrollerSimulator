using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDE.Components {
    public interface Component {
        bool UserInput { get; set; }
        void Refresh();
    }
}
