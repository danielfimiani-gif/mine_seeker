class MinerContext
{
    public float CurrentOre { get; private set; }
    public Mine CurrentMine { get; private set; }

    public void AddOre(float amount)
    {
        CurrentOre += amount;
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
}
