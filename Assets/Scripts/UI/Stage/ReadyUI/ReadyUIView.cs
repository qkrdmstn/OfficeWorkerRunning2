using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadyUIView : MonoBehaviour
{
    public GameObject readyUIImage;
    public TextMeshProUGUI readyUIText;

    public void UpdateReadyText(string str)
    {
        readyUIText.text = str;
    }
}
