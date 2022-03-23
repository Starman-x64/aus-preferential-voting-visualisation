using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HSVPicker;
using UnityEngine.EventSystems;

public class GUIHandler : MonoBehaviour
{
    private GameManager gm;

    public Slider candidateSlider;
    public Slider votersSlider;
    public ColorPicker colourPicker;

    public static bool firstColorClick = true;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnGUI()
    {
        gm.numCandidates = (int)candidateSlider.value;
        gm.numVoters = (int)votersSlider.value;
        if (gm.selectedCandidate == null)
        {
            colourPicker.gameObject.SetActive(false);
            firstColorClick = true;
        }
        else
        {
            colourPicker.gameObject.SetActive(true);
            if (firstColorClick)
            {
                Color.RGBToHSV(gm.selectedCandidate.partyColour, out var H, out var S, out var V);
                colourPicker.H = H;
                colourPicker.S = S;
                colourPicker.V = V;
                firstColorClick = false;
            }
            gm.selectedCandidate.partyColour = colourPicker.CurrentColor;
            gm.selectedCandidate.UpdateColour();
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
