using System.Collections.Generic;

public class Bolts
{
    public enum Type { 
        Linear, 
        BlackHole,
        Test,
        Heal
    }
    public enum EffectType { Penetration, Reflection }
    public static BoltNode[] Get(Type type) => boltNodes[type];
    
    // 이펙트의 경우, 복수로 호출할 수 있도록 변경하기
    public static BoltEffect GetEffect(EffectType effectType) => effects[effectType];

    private static Dictionary<Type, BoltNode[]> boltNodes = new() {
        { Type.Linear , new BoltNode[] { new BoltLinearNode() } },
        { Type.BlackHole , new BoltNode[]{ new BlackHoleBolt() }},
        { Type.Test, new BoltNode[] { new RandomSpreadNode(), new BoltTestDownNode() }},
        { Type.Heal, new BoltNode[] {}}
    };

    private static Dictionary<EffectType, BoltEffect> effects = new() { };

}