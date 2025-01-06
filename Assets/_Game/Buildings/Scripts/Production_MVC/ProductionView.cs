using Game.Unit;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionView : MonoBehaviour
{
    public RawImage buildingImage;
    public TMP_Text healthText;
    public TMP_Text buildingNameText;

    public Transform buttonContainer;
    public GameObject buttonPrefab;

    private ProductionController _controller;
    private BuildingData _buildingData;
    private List<GameObject> buttons = new();
    public void Init(ProductionController productionController)
    {
        _controller = productionController;
    }

    public void UpdateView(BuildingData buildingData)
    {
        _buildingData = buildingData;

        buildingImage.texture = _buildingData.icon;
        buildingNameText.text = _buildingData.name;

        PopulateProductionUI();
    }
    public void ResetView()
    {
        _buildingData = null;


        buildingImage.texture = null;
        buildingNameText.text = string.Empty;

        PopulateProductionUI();

    }
    private void PopulateProductionUI()
    {
        if (buttons.Count > 0)
        {
            foreach (var button in buttons)
            {
                Destroy(button);
            }
        }

        if (_buildingData != null)
        {
            foreach (UnitData unitData in _buildingData.unitDatas)
            {
                GameObject buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                buttonInstance.GetComponentInChildren<TMP_Text>().text = unitData.name;
                buttonInstance.GetComponent<Button>().onClick.AddListener(() => _controller.StartProduction(unitData));

                buttons.Add(buttonInstance);
            }
        }
      
    }
   
    public void UpdateHealthChange(int health)
    {
        healthText.text = health.ToString();
    }

}


