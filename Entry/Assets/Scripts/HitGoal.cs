using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitGoal : MonoBehaviour
{
    public float numOfSecondsToWaitTransition;
    private bool startTimer;
    private bool canTransitionNow;

    private GameObject parentContainer;
    private GameObject myBall;
    private AudioSource audioSource;

    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // find the starting point
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
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
        if (startTimer)
        {
            numOfSecondsToWaitTransition -= Time.deltaTime;
        }
        if(numOfSecondsToWaitTransition <= 0)
        {
            transitionScene();
        }
    }

    private void transitionScene()
    {
        myBall.SetActive(true);
        DontDestroyOnLoad(parentContainer);
        SceneManager.LoadScene(nextScene);

        GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
        playerObj[0].GetComponent<PlayerController>().gotTheHole();
        //disable player input
        //display the score screen
    }
        
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            audioSource.Play();
            // this holds the canvas and other objects
            parentContainer = col.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            startTimer = true;
            myBall = col.gameObject;
            myBall.SetActive(false);
        }
    }
}
