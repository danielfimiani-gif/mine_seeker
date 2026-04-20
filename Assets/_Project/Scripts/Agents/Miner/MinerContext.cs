using UnityEngine;

class MinerContext
{
    public float CurrentHP { get; private set; }
    public float CurrentOre { get; private set; }
    public Mine CurrentMine { get; private set; }
    public bool IsAlive => CurrentHP > 0f;

    public void InitializeHP(float maxHP)
    {
        CurrentHP = maxHP;
    }

    public void AddOre(float amount)
    {
        CurrentOre += amount;
    }

    public void RemoveOre(float amount)
    {
        CurrentOre = Mathf.Max(0, CurrentOre - amount);
    }

    public void ResetOre()
    {
        CurrentOre = 0f;
    }

    public void AssignMine(Mine mine)
    {
        mine.SetIsOccupied();
        CurrentMine = mine;
    }

    public void ClearMine()
    {
        if (CurrentMine != null)
            CurrentMine.SetIsFree();

        CurrentMine = null;
    }

    public bool HasOre()
    {
        return CurrentOre > 0;
    }

    public void TakeDamage(float amount)
    {
        CurrentHP = Mathf.Max(0f, CurrentHP - amount);
    }
}
