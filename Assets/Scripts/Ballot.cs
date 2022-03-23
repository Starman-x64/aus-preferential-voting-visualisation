using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballot : MonoBehaviour
{
    public Candidate[] candidateOrder;

    public Ballot(List<Candidate> candidates)
    {
        candidateOrder = new Candidate[candidates.Count];
        for (int i = 0; i < candidateOrder.Length; i++)
        {
            int index = Random.Range(0, candidateOrder.Length);
            while (candidateOrder[index] != null)
            {
                index = Random.Range(0, candidateOrder.Length);
            }
            candidateOrder[index] = candidates[i];
        }
    }
}
