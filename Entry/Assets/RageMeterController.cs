using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMeterController : MonoBehaviour
{
    public float rageIncrementAmount;

    public Slider rageMeter;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float getRageLevel()
    {
        return rageMeter.value;
    }

    public void addRage()
    {
        rageMeter.value += rageIncrementAmount;
    }

    public void resetRage()
    {
        rageMeter.value = 0;
    }

    public void reduceRage()
    {
        rageMeter.value -= rageIncrementAmount;
    }
}
