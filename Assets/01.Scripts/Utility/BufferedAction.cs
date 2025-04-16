using System;

public class BufferedAction
{
    public string Name { get; private set; }
    private Func<bool> condition;
    private Action action;

    public BufferedAction(string name, Func<bool> condition, Action action)
    {
        Name = name;
        this.condition = condition;
        this.action = action;
    }

    public bool TryExecute()
    {
        if (condition())
        {
            action?.Invoke();
            return true;
        }
        return false;
    }
}
