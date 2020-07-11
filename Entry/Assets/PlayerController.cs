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

    public AudioClip[] sounds;
    private AudioSource audioSource;

    public AudioClip defaultMusic;
    public AudioClip rageMusic;
    public AudioSource mainCameraAudio;

    private bool rageInControl = false;
    private int rageHitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        disableInput = false;
        myAnim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        mainCameraAudio = GetComponent<AudioSource>();
        mainCameraAudio.clip = defaultMusic;
        mainCameraAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableInput)
        {
            if (!startPowerMeter)
            {
                // if rage is full, set a random rotation for the player and hit the ball a random power
                if (rageInControl)
                {
                    var euler = transform.eulerAngles;
                    euler.z = Random.Range(0.0f, 360.0f);
                    transform.eulerAngles = euler;
                    powerMeter.value = Random.Range(0.5f, 1f);
                    stopPowerMeter();
                }
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
            // don't increase rage if this was a rage move
            if (!rageInControl)
            {
                rageMeter.GetComponent<RageMeterController>().addRage();
            }
            else
            {
                rageHitCount++;
                if (rageHitCount == 3)
                {
                    rageHitCount = 0;
                    rageInControl = false;
                    mainCameraAudio.clip = defaultMusic;
                    mainCameraAudio.Play();
                }
            }
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
        if(rageMeter.GetComponent<RageMeterController>().getRageLevel() == .3f)
        {
            myLives.GetComponent<ClubController>().playerDied();
            rageMeter.GetComponent<RageMeterController>().resetRage();
            //change the music
            mainCameraAudio.clip = rageMusic;
            mainCameraAudio.Play();
            rageInControl = true;
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
