using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Data/Enemy/Stats", order = 100)]
class EnemyStats : ScriptableObject
{
    [SerializeField, Range(1f, 25f)] float attackDamage = 5f;
    [SerializeField, Range(0.5f, 5f)] float attackInterval = 1.5f;
    [SerializeField, Range(0f, 20f)] float chaseSpeed = 5f;
    [SerializeField, Range(0f, 20f)] float loseTrackRange = 5f;
    [SerializeField, Range(0f, 1f)] float pathfindingIntervals = 0.5f;
    [SerializeField, Range(0f, 20f)] float wanderSpeed = 5f;
    [SerializeField, Range(0f, 20f)] float searchRadius = 5f;
    [SerializeField, Range(0f, 20f)] float circleWanderRadius = 2f;
    [SerializeField, Range(4, 64)] int circleSearchPrecision = 16;

    public float AttackDamage => attackDamage;
    public float AttackInterval => attackInterval;
    public float ChaseSpeed => chaseSpeed;
    public float LoseTrackRange => loseTrackRange;
    public float PathfindingIntervals => pathfindingIntervals;
    public float WanderSpeed => wanderSpeed;
    public float SearchRadius => searchRadius;
    public float CircleWanderRadius => circleWanderRadius;
    public int CircleSearchPrecision => circleSearchPrecision;
}