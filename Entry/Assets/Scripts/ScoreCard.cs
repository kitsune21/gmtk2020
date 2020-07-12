using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{
    public List<Text> holeScoresText;
    public List<Text> holeScoresPar;
    public List<int> holeScores;

    public void addScore(int newScore)
    {
        holeScores.Add(newScore);
        updateScoreCard();
    }

    private void updateScoreCard()
    {
        for(int i = 0; i < holeScoresText.Count; i++)
        {
            if (i < holeScores.Count)
            {
                holeScoresText[i].text = holeScores[i].ToString();
            }
            
        }
    }
}
