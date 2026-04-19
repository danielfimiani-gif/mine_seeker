using TMPro;
using UnityEngine;

class MinerRow : MonoBehaviour
{
    [SerializeField] private TMP_Text minerStatsText;

    private int _minerID;
    private Miner _minerRef;

    void Update()
    {
        minerStatsText.text = $" Miner {_minerID} Ore : {_minerRef.Context.CurrentOre:0} / {_minerRef.Config.MaxOreCapacity:0}";
    }

    public void Initialize(int minerID, Miner minerRef)
    {
        this._minerID = minerID;
        this._minerRef = minerRef;
    }
}