using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMeterController : MonoBehaviour
{
    public float rageIncrementAmount;

    public Slider rageMeter;

    public Sprite face1;
    public Sprite face2;
    public Sprite face3;
    public Sprite face4;
    public Sprite face5;

    private List<Sprite> myFacesList;

    public Image currentFace;
    
    // Start is called before the first frame update
    void Start()
    {
        currentFace.sprite = face1;
        myFacesList = new List<Sprite>();
        myFacesList.Add(face1);
        myFacesList.Add(face2);
        myFacesList.Add(face3);
        myFacesList.Add(face4);
        myFacesList.Add(face5);
    }

    public float getRageLevel()
    {
        return rageMeter.value;
    }

    public void addRage()
    {
        rageMeter.value += rageIncrementAmount;
        int faceIndex = (int)(rageMeter.value * 10) / 2;

        currentFace.sprite = myFacesList[rageMeter.value >= 0.9f ? 4 : faceIndex];
    }

    public void resetRage()
    {
        rageMeter.value = 0;
        currentFace.sprite = face1;
    }

    public void reduceRage()
    {
        rageMeter.value -= rageIncrementAmount;
    }
}
