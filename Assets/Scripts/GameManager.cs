using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject candidateFab;

    public List<Candidate> candies;
    public Dictionary<Candidate, int> previousCount;
    public Dictionary<Candidate, int> candidates;
    public List<Ballot> ballots;
    public int numVoters = 100;
    public int numCandidates = 5;
    public Candidate selectedCandidate;

    private void Start()
    {
        SpawnCandidates();        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GUIHandler.IsPointerOverUIObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "Candidate")
                {
                    selectedCandidate = hit.transform.root.GetComponent<Candidate>();
                    GUIHandler.firstColorClick = true;
                }
                else
                {
                    selectedCandidate = null;
                }
            }
            else
            {
                selectedCandidate = null;
            }
        }

        if (candies.Count != numCandidates)
        {
            SpawnCandidates();
        }
        DisplayVotes();
    }

    public void SpawnCandidates()
    {
        var leftmostPoint = new Vector3(numCandidates * 3 / 2f, 0f, 0f);
        var rightmostPoint = new Vector3(-numCandidates * 3 / 2f, 0f, 0f);
        foreach (Candidate c in candies)
        {
            Destroy(c.gameObject);
        }
        candies = new List<Candidate>();
        for (int i = 0; i < numCandidates; i++)
        {
            var c = Instantiate(candidateFab).GetComponent<Candidate>();
            c.transform.position = Vector3.Lerp(leftmostPoint, rightmostPoint, (float)i / numCandidates);
            c.partyColour = Random.ColorHSV(0f,1f,1f,1f,1f,1f);
            c.votesMeshRenderer.material.SetColor("_Color", c.partyColour);
            candies.Add(c);
        }
        UpdateCandidates();
        CreateBallots();
        UpdateVoteCount();
    }

    public void UpdateCandidates()
    {
        candidates = new Dictionary<Candidate, int>();
        previousCount = new Dictionary<Candidate, int>();
        foreach (Candidate candidate in candies)
        {
            candidates.Add(candidate, 0);
            previousCount.Add(candidate, 0);
        }
    }

    public void CreateBallots()
    {
        ballots = new List<Ballot>();
        for (int i = 0; i < numVoters; i++)
        {
            ballots.Add(new Ballot(candies));
        }
    }
    
    public void UpdateVoteCount()
    {
        previousCount = new Dictionary<Candidate, int>();
        foreach (KeyValuePair<Candidate, int> candidate in candidates)
        {
            previousCount.Add(candidate.Key, candidate.Value);
        }    

        for (int i = 0; i < candies.Count; i++)
        {
            candidates[candies[i]] = 0;
        }
        foreach (Ballot b in ballots)
        {
            int i = 0;
            while (b.candidateOrder[i].eliminated)
            {
                i++;
                if (i >= numCandidates)
                    break;
            }
            if (i < numCandidates)
                candidates[b.candidateOrder[i]]++;
        }


        foreach (KeyValuePair<Candidate, int> c in candidates)
        {
            if (!c.Key.eliminated)
            {
                c.Key.difference = c.Value - previousCount[c.Key];
                c.Key.DisplayDifference();
            }
        }
    }

    public void DisplayVotes()
    {
        foreach (KeyValuePair<Candidate, int> candidate in candidates)
        {
            candidate.Key.votes = candidate.Value;
        }
    }

    public void UpdateVotes()
    {
        CreateBallots();
        UpdateVoteCount();
    }

    public void EliminateLeastVotes()
    {
        var tempDictionary = candidates;
        Candidate min;
        KeyValuePair<Candidate, int> curr = new KeyValuePair<Candidate, int>(null, -1);
        int numEliminated = 0;
        foreach (KeyValuePair<Candidate, int> candidate in tempDictionary)
        {
            if (curr.Value == -1)
            {
                if (!candidate.Key.eliminated)
                    curr = candidate;
            }
            else
            {
                if (!candidate.Key.eliminated)
                {
                    if (curr.Value > candidate.Value)
                        curr = candidate;
                }
            }
            if (candidate.Key.eliminated)
            {
                numEliminated++;
            }
        }
        if (numEliminated >= numCandidates - 1)
        {
            DisplayWinner();
            return;
        }
        if (curr.Value != -1 && !curr.Key.eliminated)
        {
            min = curr.Key;
            min.Eliminate();
        }
        UpdateVoteCount();
    }

    public void DisplayWinner()
    {
        print("We have a winner!");
    }
}
