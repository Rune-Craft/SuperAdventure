namespace Engine;

public class MessageEventArgs (string message, bool addExtraNewLine) : EventArgs
{
    public string Message { get; private set; } = message;
    public bool AddExtraNewLine { get; private set; } = addExtraNewLine;
}
