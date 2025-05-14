using System;

public class Cue
{
    public bool IsAuto = true;
    public Action CurrAction;
    
    public Cue(Action newAction) => CurrAction = newAction;
}