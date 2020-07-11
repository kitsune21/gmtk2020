using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float playerRotateAmount;

    public float increasePowerMeterAmount;
    private int powerMeterDirection = 1;

    public GameObject powerMeterObj;
    public Slider powerMeter;
    public GameObject rageMeter;
    public GameObject myLives;

    public bool startPowerMeter;

    public bool disableInput;

    private float powerMeterFinalValue;

    public GameObject myBall;
    public GameObject tempBallParent;

    private Animator myAnim;
    public int framesTillHit;
    public int framesTillHitCount;

    public AudioClip[] sounds ;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        disableInput = false;
        myAnim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableInput)
        {
            if (!startPowerMeter)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(0, 0, playerRotateAmount);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Rotate(0, 0, -playerRotateAmount);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (startPowerMeter)
                {
                    stopPowerMeter();
                }
                else
                {
                    startPowerMeter = true;
                    powerMeterObj.SetActive(true);
                    Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    powerMeterObj.transform.position = Camera.main.WorldToScreenPoint(pos);
                }
            }
        }
        if (myBall.GetComponent<BallMoveController>().getBallHasStopped())
        {
            if (myBall.GetComponent<BallMoveController>().getHitWater())
            {
                Vector3 waterPos = myBall.GetComponent<BallMoveController>().getNewWaterPos();
                myBall.transform.position = waterPos;
            }
            rageMeter.GetComponent<RageMeterController>().addRage();
            resetCharacter();
        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        updatePowerMeter();
        waitFramesTillHittingBall();
    }

    private void waitFramesTillHittingBall()
    {
        if (framesTillHitCount < framesTillHit)
        {
            framesTillHitCount += 1;
        }
        else if (framesTillHitCount == framesTillHit)
        {
            myBall.GetComponent<BallMoveController>().HitBall(getRotation(), getPowerMeterValue(), rageMeter.GetComponent<RageMeterController>().getRageLevel());
            myBall.transform.SetParent(tempBallParent.transform, true);
            framesTillHitCount += 1;
        }
    }

    private void updatePowerMeter()
    {
        if (startPowerMeter)
        {
            powerMeter.value += ((float)powerMeterDirection * increasePowerMeterAmount);
            if (powerMeter.value >= 1)
            {
                powerMeterDirection = -1;
            }
            else if (powerMeter.value <= 0)
            {
                powerMeterDirection = 1;
            }
        }
    }

    private void stopPowerMeter()
    {
        startPowerMeter = false;
        disableInput = true;
        powerMeterObj.SetActive(false);
        powerMeterFinalValue = powerMeter.value;
        hitTheBall();
    }

    private void hitTheBall()
    {
        myAnim.SetTrigger("SwingInit");
        framesTillHitCount = 0;

        // play a random grunt when the player hits the ball
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    private void resetCharacter()
    {
        disableInput = false;
        powerMeterDirection = 1;
        startPowerMeter = false;
        powerMeterFinalValue = 0;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = myBall.transform.position;
        myBall.transform.SetParent(transform, true);
        myAnim.SetTrigger("ResetToIdle");
        if(rageMeter.GetComponent<RageMeterController>().getRageLevel() == 1f)
        {
            myLives.GetComponent<ClubController>().playerDied();
            rageMeter.GetComponent<RageMeterController>().resetRage();
        }
    }

    public float getPowerMeterValue()
    {
        return powerMeterFinalValue;
    }

    public Vector3 getRotation()
    {
        return -transform.up;
    }
}
