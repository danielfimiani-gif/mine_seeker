using UnityEngine;

class GameManager : MonoBehaviourSingleton<GameManager>
{
    private Base _base;
    private Miner[] _miners;

    public float TargetOre { get; private set; }
    public float CurrentOre => _base.CurrentOre;
    public Miner[] Miners => _miners;
    public Base MinerBase => _base;

    void Start()
    {
        _base = FindFirstObjectByType<Base>();
        _miners = FindObjectsByType<Miner>(sortMode: UnityEngine.FindObjectsSortMode.InstanceID);

        CalculateTargetOre();
    }

    private void CalculateTargetOre()
    {
        float targetOre = 0;

        Mine[] mines = FindObjectsByType<Mine>(sortMode: UnityEngine.FindObjectsSortMode.InstanceID);
        if (mines.Length == 0)
        {
            Debug.LogError("No mines found in game");
            return;
        }

        foreach (Mine mine in mines)
            targetOre += mine.OreAmount;

        TargetOre = targetOre;
    }
}
