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
    private List<UnitData> productionList = new();
    private List<InfoImage> infoImages = new();

    public void Init(InfoController productionController)
    {
        _controller = productionController;
    }

    public void UpdateView(InfoUIStrategyBase strategy)
    {

        buildingImage.texture = strategy.Icon;
        buildingNameText.text = strategy.Name;
        productionList = strategy.GetProductionData;
        PopulateProductionUI();
    }
    public void ResetView()
    {
        productionList?.Clear();
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

        if (productionList == null)
            return;

        foreach (UnitData unitData in productionList)
        {
            InfoImage infoImage = Instantiate(infoImagePrefab, buttonContainer);
            infoImage.PopulateUI(unitData.icon, unitData.name);
            infoImages.Add(infoImage);
        }


    }

    public void UpdateHealthChange(int health)
    {
        healthText.text = health.ToString();
    }

}


