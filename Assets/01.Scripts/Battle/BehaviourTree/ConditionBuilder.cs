using System;
using System.Collections.Generic;


public class ConditionBuilder
{
	// 암묵적 형변환
	public static implicit operator ConditionNode(ConditionBuilder builder) => (ConditionNode)builder.conditionNode;

	private readonly Func<EnemyBaseController, bool> predicate;
	private readonly Node conditionNode;

	public ConditionBuilder(Func<EnemyBaseController, bool> predicate)
	{
		conditionNode =  new ConditionNode(predicate);
	}

	public ConditionBuilder Selector(params Node[] nodes)
	{
		var selector = BT.Selector(nodes);
		conditionNode.AddChild(selector);
		return this;
	}

	public ConditionBuilder Sequence(params Node[] nodes)
	{
		var sequence = BT.Sequence(nodes);
		conditionNode.AddChild(sequence);
		return this;
	}

	public ConditionBuilder Do<T>(params object[] args) where T : Node
	{
		var node = BT.Do<T>(args);
		conditionNode.AddChild(node);
		return this;
	}
}