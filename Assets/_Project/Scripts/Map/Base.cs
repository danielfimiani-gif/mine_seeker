using UnityEngine;

class Base : MonoBehaviour
{
    private float _currentOre;
    public float CurrentOre => _currentOre;

    public void DepositOre(float amount)
    {
        _currentOre += amount;
    }
}