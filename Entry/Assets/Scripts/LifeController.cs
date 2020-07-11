using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    public int id;

    private bool isGone;
   

    public void setIsGone()
    {
        isGone = true;
        Image myImage = gameObject.GetComponentInChildren<Image>();
        myImage.color = new Color(myImage.color.r, myImage.color.b, myImage.color.g, 0.3f);
    }

    public bool getIsGone()
    {
        return isGone;
    }

    public void setId(int newID)
    {
        id = newID;
    }

    public int getId()
    {
        return id;
    }
}
