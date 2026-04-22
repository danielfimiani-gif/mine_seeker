using TMPro;
using UnityEngine;

class MinerRow : MonoBehaviour
{
    [SerializeField] private TMP_Text oreStatsText;
    [SerializeField] private TMP_Text healthStatsText;

    private int _minerID;
    private Miner _minerRef;

    void Update()
    {
        oreStatsText.text = $" Miner {_minerID} Ore : {_minerRef.Context.CurrentOre:0} / {_minerRef.Config.MaxOreCapacity:0}";
        healthStatsText.text = $"{_minerRef.Context.CurrentHP} / {_minerRef.Config.MaxHP}";
    }

    public void Initialize(int minerID, Miner minerRef)
    {
        this._minerID = minerID;
        this._minerRef = minerRef;
    }
}