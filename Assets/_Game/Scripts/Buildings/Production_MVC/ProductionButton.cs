using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductionButton : MonoBehaviour
{
    public string buttonTextName;
    public Texture buttonIcon;
    public UnityAction OnClicked;

    public Button button;
    public TMP_Text text;
    public RawImage image;
 
    public void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        image = GetComponentInChildren<RawImage>();
    }
    public void Init(string name, Texture icon, UnityAction OnClicked)
    {
        buttonTextName = name;
        buttonIcon = icon;
        this.OnClicked = OnClicked;
    }

    public void ResetButton()
    {

    }
    public void UpdateButton()
    {
        text.text = buttonTextName;
        image.texture = buttonIcon;
        button.onClick.AddListener(OnClicked);
    }
}
