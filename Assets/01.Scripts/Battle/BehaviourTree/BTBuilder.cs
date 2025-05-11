using System;
using System.Collections.Generic;

public class BTBuilder
{
    private Stack<Node> _stack = new();
    
    public BTBuilder Sequence()
    {
        var node = new Sequence();
        // AddNode(node);
        // nodeStack.Push(node);
        return this;
    }
    

    public void Condition(Func<EnemyBaseController, bool> callback)
    {
        // new ConditionNode(callback);
    }
    
    public void End()
    {
        
    }       
}