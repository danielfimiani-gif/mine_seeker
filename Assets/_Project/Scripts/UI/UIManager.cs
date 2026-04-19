using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

class UIManager : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleUIAction;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Transform minerRowsContainer;
    [SerializeField] private MinerRow minerRowPrefab;
    [SerializeField] private TMP_Text targetOreText;
    [SerializeField] private TMP_Dropdown strategyDropdown;

    void OnEnable()
    {
        toggleUIAction.action.performed += HandleToggleUI;
        toggleUIAction.action.Enable();

    }

    void Start()
    {
        InstantiateMinerRows();
        PopulateStrategyDropDown();
    }

    void Update()
    {
        UpdateTargetText();
    }

    void OnDisable()
    {
        toggleUIAction.action.performed -= HandleToggleUI;
        toggleUIAction.action.Disable();
    }

    private void InstantiateMinerRows()
    {
        Miner[] miners = GameManager.Instance.Miners;
        for (int i = 0; i < miners.Length; i++)
        {
            MinerRow minerRow = Instantiate(minerRowPrefab, minerRowsContainer);
            minerRow.Initialize(i + 1, miners[i]);
        }
    }

    private void PopulateStrategyDropDown()
    {
        strategyDropdown.ClearOptions();
        List<string> options = new(System.Enum.GetNames(typeof(EPathFindingStrategy)));
        strategyDropdown.AddOptions(options);
        strategyDropdown.onValueChanged.AddListener(OnStrategyChanged);
    }
    private void UpdateTargetText()
    {
        float current = GameManager.Instance.CurrentOre;
        float target = GameManager.Instance.TargetOre;
        targetOreText.text = $"Total Ore {current:0} / {target:0}";
    }

    private void OnStrategyChanged(int index)
    {
        EPathFindingStrategy strategy = (EPathFindingStrategy)index;
        PathFindingManager.Instance.SetStrategy(strategy);
    }

    private void HandleToggleUI(InputAction.CallbackContext _)
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }
}