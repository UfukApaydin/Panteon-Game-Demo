using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoImage : MonoBehaviour
{
    private TMP_Text _nameText;
    private RawImage _image;
    private void Awake()
    {
        _nameText = GetComponentInChildren<TMP_Text>();
        _image = GetComponentInChildren<RawImage>();
    }

    public void PopulateUI(Texture icon, string name)
    {
        _nameText.text = name;
        _image.texture = icon;
    }
}
