namespace DiarioMagna;

public class LayoutHeaderState
{
    public string IconClass { get; private set; } = "bi bi-newspaper";
    public string Title { get; private set; } = "Diario Magna";
    public string Subtitle { get; private set; } = "Sistema de flujo de noticias";

    public event Action? OnChange;

    public void Set(string iconClass, string title, string subtitle)
    {
        IconClass = iconClass;
        Title = title;
        Subtitle = subtitle;
        OnChange?.Invoke();
    }
}