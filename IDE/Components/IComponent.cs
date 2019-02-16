namespace IDE.Components
{
    public interface IComponent
    {
        bool UserInput { get; set; }
        void Refresh();
    }
}