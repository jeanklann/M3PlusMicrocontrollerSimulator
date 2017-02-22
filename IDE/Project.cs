using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE {
    public class Project {
        public int Zoom = 0;
        public int Clock = 1; //in Hz
        public bool Autosave = false; //not developed
        public int AutosaveInterval = 1000*60*2; //2 minutes
    }

    public class FileProject {
        public string Directory;
        public string FileName;
        public bool Modified;
        public Project Project;
        public string Code;
    }
}
