using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitGoal : MonoBehaviour
{
    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Ball")
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
