using System.Collections.Generic;

public class Bolts
{
    public enum Type { Linear, Test }
    public enum EffectType { Penetration, Reflection }
    public static BoltNode[] Get(Type type) => boltNodes[type];
    
    // 이펙트의 경우, 복수로 호출할 수 있도록 변경하기
    public static BoltEffect GetEffect(EffectType effectType) => effects[effectType];

    private static Dictionary<Type, BoltNode[]> boltNodes = new() {
        { Type.Linear , new BoltNode[] { new BoltLinearNode() } },
        { Type.Test, new BoltNode[] { new RandomSpreadNode(), new BoltTestDownNode() }}
        // { Type.Test, new BoltNode[] { new BlackHoleNode() }}
    };

    private static Dictionary<EffectType, BoltEffect> effects = new() { };

}