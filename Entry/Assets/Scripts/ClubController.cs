using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubController : MonoBehaviour
{
    public int clubCount;
    public int clubCountMax;

    public GameObject clubLife;
    private List<GameObject> clubLifeList;

    void Start()
    {
        clubLifeList = new List<GameObject>();
        for (int i = clubCountMax; i > 0; i--)
        {
            GameObject newLife = Instantiate(clubLife, transform);
            newLife.GetComponent<LifeController>().setId(i);
            clubLifeList.Add(newLife);
        }
        clubCount = clubCountMax;
    }

    public int playerDied()
    {
        if (clubCount > 0)
        {
            int nextID = (clubCountMax + 1) - clubCount;
            foreach (GameObject life in clubLifeList)
            {
                if (life.GetComponent<LifeController>().getId() == nextID)
                {
                    life.GetComponent<LifeController>().setIsGone();
                    clubCount -= 1;
                }
            }
        }
        return clubCount;
    }
}
