using UnityEngine;

public enum FishType
{
    Normal,
    Predator,
    Spawner,
    Support,
    Legendary
}

public enum AttackType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "FishData", menuName = "Game/FishData", order = 0)]
public class FishData : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string fishName;
    public FishType type;
    public AttackType attackType;

    [Header("Tier & Evolution")]
    public int tier;             // 1~10
    public bool isPredator;
    public int evolutionStage;   // 0=Base, 1=Evo1, 2=Evo2
    public FishData nextEvolution; // null이면 최종 진화

    [Header("Stats")]
    public float coinPerSecond;  // 초당 코인
    public float baseAttack;     // 상어 전투 공격력
    public int predatorTargetMaxTier; // 포식 가능한 대상 티어 (Predator일 때)

    [Header("Visual")]
    public Sprite sprite;
    public float moveSpeed = 1f;
}
