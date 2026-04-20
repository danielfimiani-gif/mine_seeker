using UnityEngine;

[CreateAssetMenu(fileName = "MinerStats", menuName = "Data/Miner/Stats", order = 100)]
class MinerStats : ScriptableObject
{
    [SerializeField, Range(10f, 250f)] float maxHP;
    [SerializeField, Range(10f, 150f)] float maxOreCapacity;
    [SerializeField, Range(0.5f, 2f)] float miningSpeed;
    [SerializeField, Range(1f, 50f)] float walkSpeed;
    [SerializeField, Range(1f, 50f)] float walkWithOreSpeed;
    [SerializeField, Range(1f, 50f)] float runSpeed;
    [SerializeField, Range(1f, 20f)] float orePerHit;
    [SerializeField, Range(0.5f, 5f)] float unloadSpeed;

    public float MaxOreCapacity => maxOreCapacity;
    public float MiningSpeed => miningSpeed;
    public float RunSpeed => runSpeed;
    public float OrePerHit => orePerHit;
    public float UnloadSpeed => unloadSpeed;
    public float MaxHP => maxHP;

    public float GetWalkSpeed(bool hasOre)
    {
        return hasOre ? walkWithOreSpeed : walkSpeed;
    }
}