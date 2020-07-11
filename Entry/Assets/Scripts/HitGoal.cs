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
        // find the starting point
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            Debug.Log(go.tag);
            if (go.CompareTag("Respawn"))
            {
                foreach (GameObject pl in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (pl.CompareTag("Ball"))
                    {
                        // transport player landing's position
                        pl.gameObject.transform.position = go.transform.position;
                        // stop the ball 
                        pl.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        return;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
        
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            // this holds the canvas and other objects
            GameObject parentContainer = col.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            DontDestroyOnLoad(parentContainer);
            SceneManager.LoadScene(nextScene);
        }
    }
}
