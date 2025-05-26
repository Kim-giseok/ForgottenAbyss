using System;
using System.Linq;

public static class BT
{
	public static Node Selector(params Node[] children)
	{
		var node = new SelectorNode();
		foreach (var child in children)
			node.AddChild(child);
		return node;
	}

	public static Node Sequence(params Node[] children)
	{
		var node = new SequenceNode();
		foreach (var child in children)
			node.AddChild(child);
		return node;
	}

	public static Node Do<T>(params object[] args) where T : Node
	{
		return (Node)Activator.CreateInstance(typeof(T), args);
	}
	
	public static Node Do(Func<Node> factory)
	{
		return factory();
	}

	public static CheckNode Condition(Func<EnemyController, bool> callback)
	{
		return new CheckNode(callback);
	}
	
	public static CheckNode<T> Condition<T>(Func<T, bool> callback) where T : EnemyBaseController
	{
		return new CheckNode<T>(callback);
	}
	
	public static Node DoRandom(params (float, Node)[] nodes)
	{
		return Do(() => new RandomNode(nodes.ToList()));
	}

}