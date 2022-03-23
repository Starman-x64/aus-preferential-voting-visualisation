using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmartSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;
    private string baseText;

    private void Start()
    {
        baseText = text.text;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = baseText + slider.value;
    }
}
