namespace IDE.Components {
    public interface Component {
        bool UserInput { get; set; }
        void Refresh();
    }
}
