using System;

namespace IDE
{
    [Serializable]
    public class Project
    {
        public bool Autosave = false; //not developed
        public int AutosaveInterval = 1000 * 60 * 2; //2 minutes
    }
}