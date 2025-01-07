using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProductionSlot : MonoBehaviour
{

    private Button button;
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private RawImage productionImage;
  
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetProduction(Data productionData)
    {
        productionText.text = productionData.dataName;
        productionImage.texture = productionData.icon;
       
    }
    public void ResetProduction()
    {
        productionText.text = "";
        productionImage.texture = null;
       
    }
}
