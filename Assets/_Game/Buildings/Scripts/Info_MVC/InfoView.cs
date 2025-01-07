using Game.Unit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoView : MonoBehaviour
{
    public RawImage buildingImage;
    public TMP_Text healthText;
    public TMP_Text buildingNameText;

    public Transform buttonContainer;
    public InfoImage infoImagePrefab;

    private InfoController _controller;
    private BuildingData _buildingData;
    private List<InfoImage> infoImages = new();
    public void Init(InfoController productionController)
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
        if (infoImages.Count > 0)
        {
            foreach (var infoImage in infoImages)
            {
                Destroy(infoImage.gameObject);
            }
            infoImages.Clear();
        }

        if (_buildingData != null)
        {
            foreach (UnitData unitData in _buildingData.unitDatas)
            {
                InfoImage infoImage = Instantiate(infoImagePrefab, buttonContainer);
                infoImage.PopulateUI(unitData.icon, unitData.name);
                infoImages.Add(infoImage);
            }
        }

    }

    public void UpdateHealthChange(int health)
    {
        healthText.text = health.ToString();
    }

}


