using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDE {
    [Serializable]
    public class ProjectFile {
        public string ProjectName;
        public int Frequency;
        public string Program;
        public byte[] Instructions;


    }
}
