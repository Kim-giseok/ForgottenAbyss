using System.Collections.Generic;

public class Bolts
{
    public enum Type { 
        Linear, 
        BlackHole,
        Rain,
        Heal,
        Decrescendo,
        Recursive,
        Laser,
        Forward,
        LevelUp,
        Parabola,
    }
    public enum EffectType { Penetration, Reflection }
    public static BoltNode[] Get(Type type) => boltNodes[type];
    
    // 이펙트의 경우, 복수로 호출할 수 있도록 변경하기
    public static BoltEffect GetEffect(EffectType effectType) => effects[effectType];

    private static Dictionary<Type, BoltNode[]> boltNodes = new() {
        { Type.Linear , new BoltNode[] { new LinearBolt() } },
        { Type.Laser, new BoltNode[] { new LaserBolt() }},
        { Type.BlackHole , new BoltNode[]{ new BlackHoleBolt() }},
        { Type.Rain, new BoltNode[] { new RainBolt() }},
        { Type.Heal, new BoltNode[] { new HealBolt() }},
        { Type.Decrescendo, new BoltNode[] { new DecrescendoBolt() }},
        { Type.Recursive, new BoltNode[] { new RecursiveBolt(), new RecursiveBolt(), new RecursiveBolt(), new RecursiveBolt()}},
        { Type.Forward, new BoltNode[] { new ForwardBolt() }},
        { Type.Parabola, new BoltNode[] { new ParabolaBolt() }},
    };

    private static Dictionary<EffectType, BoltEffect> effects = new()
    {
        { EffectType.Reflection, new ReflectionEffect() },
        { EffectType.Penetration , new PiercingEffect() }
    };

}