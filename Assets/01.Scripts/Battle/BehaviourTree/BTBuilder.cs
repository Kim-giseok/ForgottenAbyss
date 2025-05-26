using System;
using System.Collections.Generic;

// 한글 깨짐 체크
public class BTBuilder
{
    private readonly Stack<Node> _nodeStack = new();
    private Node root;
    
    private void AddNode(Node node)
    {
        if (_nodeStack.Count > 0)
        { 
            _nodeStack.Peek().AddChild(node);
        }
        else
        {
            root = node;
        }
    }
    
    public BTBuilder Do<T>(params object[] args) where T : Node
    {
        var node = (Node)Activator.CreateInstance(typeof(T), args);
        AddNode(node);
        return this;
    }

    public BTBuilder Do(Func<Node> factory)
    {
        var node = factory();
        AddNode(node);
        return this;
    }
    
    public BTBuilder End()
    {
        if (_nodeStack.Count > 0)
        {
            _nodeStack.Pop(); // 자식 완성 후 부모로 돌아감
        }
        else
        {
            throw new InvalidOperationException("BTBuilder: End() called but no open composite node exists.");
        }
        return this;
    }
    
    // composite 목록들
    public BTBuilder Condition(Func<EnemyBaseController, bool> callback)
    {
        var node = new ConditionNode(callback);
        AddNode(node);
        _nodeStack.Push(node);
        return this;
    }

    public BTBuilder Sequence(bool isIgnore = false)
    {
        var node = new SequenceNode();
        if (isIgnore) node.Ignore();
        
        AddNode(node);
        _nodeStack.Push(node);
        return this;
    }

    public BTBuilder Selector()
    {
        var node = new SelectorNode();
        AddNode(node);
        _nodeStack.Push(node);
        return this;
    }

    public Node Build()
    {
        if (_nodeStack.Count != 0)
        {
            throw new InvalidOperationException("BTBuilder: All composite nodes must be properly closed with End().");
        }

        return root;
    }
}