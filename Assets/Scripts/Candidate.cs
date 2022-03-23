using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Candidate : MonoBehaviour
{
    public int votes = 0;
    public int maxVotes = 100;
    public float maxHeight = 10f;
    public Transform votePillar;
    public TextMesh voteCountText;
    public TextMesh differenceText;
    public Color partyColour;
    public MeshRenderer votesMeshRenderer;
    public bool eliminated = false;
    public int difference = 0;

    void Start()
    {
        differenceText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (eliminated)
        //    Eliminate();
        votePillar.localScale = new Vector3(1f, maxHeight * ((float) votes / maxVotes), 1f);
        voteCountText.text = votes.ToString();
    }

    public void Eliminate()
    {
        votes = 0;
        votesMeshRenderer.material.SetColor("_Color", Color.grey);
        eliminated = true;
    }

    public void UpdateColour()
    {
        if (eliminated)
        {
            return;
        }
        else
        {
            votesMeshRenderer.material.SetColor("_Color", partyColour);
        }
    }

    public void DisplayDifference()
    {
        differenceText.gameObject.SetActive(true);
        string diffText = difference >= 0 ? "+" : "";
        Color diffColor = difference > 0 ? Color.green : difference == 0 ? Color.yellow : Color.red;
        diffText += difference;
        differenceText.text = diffText;
        differenceText.color = diffColor;
        StartCoroutine(FloatDifferenceText());
    }

    IEnumerator FloatDifferenceText()
    {
        for (int i = 0; i < 30f; i++)
        {
            differenceText.transform.localPosition += new Vector3(0, 0.05f, 0);
            differenceText.color = new Color(differenceText.color.r, differenceText.color.g, differenceText.color.b, (30f - i) / 30f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        differenceText.transform.localPosition = new Vector3(0f, 1.5f, 0.5f);
        differenceText.gameObject.SetActive(false);
    }
}
