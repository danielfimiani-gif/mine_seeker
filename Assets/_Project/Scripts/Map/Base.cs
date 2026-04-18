using UnityEngine;

class Base : MonoBehaviour
{
    private float _currentOre;

    public void DepositOre(float amount)
    {
        _currentOre += amount;
    }
}