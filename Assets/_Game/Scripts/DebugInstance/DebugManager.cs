using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject content;
    public static DebugManager instance;
    private void Awake()
    {
        instance = this;
    }


    public void PrintLog(string log)
    {
        Instantiate(text, content.transform).text = log ;
        
    }
}
