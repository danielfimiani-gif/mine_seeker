using UnityEngine;

class Mine : MonoBehaviour
{
    [SerializeField] private float oreAmount;
    [SerializeField] private bool isOccupied;

    private float _currentOre;
    public bool HasOre { get => _currentOre > 0; }
    public float OreAmount => oreAmount;

    void Awake()
    {
        _currentOre = oreAmount;
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
        private set { isOccupied = value; }
    }

    public void SetIsOccupied()
    {
        isOccupied = true;
    }

    public void SetIsFree()
    {
        isOccupied = false;
    }

    public float ExtractOre(float amount)
    {
        if (_currentOre - amount < 0)
        {
            float extractedAmount = _currentOre;
            _currentOre = 0;
            return extractedAmount;
        }

        _currentOre = Mathf.Max(0, _currentOre - amount);
        return amount;
    }
}
