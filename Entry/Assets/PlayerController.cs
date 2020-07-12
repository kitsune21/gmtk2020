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
    public bool waitToStart;

    private float powerMeterFinalValue;

    public GameObject myBall;
    public GameObject tempBallParent;

    private Animator myAnim;
    public int framesTillHit;
    public int framesTillHitCount;

    public AudioClip[] sounds;
    public AudioClip rageInitYell;
    private AudioSource audioSource;

    public AudioClip defaultMusic;
    public AudioClip rageMusic;
    public AudioSource mainCameraAudio;

    private bool rageInControl = false;
    private int rageHitCount = 0;

    public GameObject strokeController;
    public GameObject strokeCardPanel;

    private int strokesCount;

    public GameObject relaxMeter;
    private bool startRelax;
    private int relaxMeterDirection;
    private int relaxAmount;
    private float returnRelaxAmount;

    public GameObject calmDownPanel;
    public Text calmDownText;

    public float secondsTillCalmDone;
    public float secondsTillCalmDoneMax;


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
        if (!waitToStart)
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
                        Vector3 pos = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
                        powerMeterObj.transform.position = Camera.main.WorldToScreenPoint(pos);
                    }
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
        updateRelaxMeter();
        if(secondsTillCalmDone < secondsTillCalmDoneMax)
        {
            secondsTillCalmDone += Time.deltaTime;
        } else if(calmDownPanel.activeSelf)
        {
            if(secondsTillCalmDone >= secondsTillCalmDoneMax)
            {
                calmDownPanel.SetActive(false);
                secondsTillCalmDone = 10000;
            }
        }
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
        if (!rageInControl)
        {
            myAnim.SetTrigger("SwingInit");
        }
        else
        {
            myAnim.SetTrigger("RageSwing");
        }
        framesTillHitCount = 0;

        strokesCount += 1;

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
            audioSource.PlayOneShot(rageInitYell);
            rageInControl = true;
        }
        powerMeter.value = 0;
    }

    public float getPowerMeterValue()
    {
        return powerMeterFinalValue;
    }

    public Vector3 getRotation()
    {
        return -transform.up;
    }

    public void gotTheHole()
    {
        strokeController.SetActive(true);
        strokeCardPanel.SetActive(true);
        strokeController.GetComponent<ScoreCard>().addScore(strokesCount);
        waitToStart = true;
    }

    public void startHole()
    {
        waitToStart = false;
        strokesCount = 0;
        startRelaxMeter();
    }

    public void startRelaxMeter()
    {
        relaxMeter.SetActive(true);
        startRelax = true;
        strokeCardPanel.SetActive(false);

    }
    private void updateRelaxMeter()
    {
        if (startRelax)
        {
            relaxMeter.GetComponentInChildren<Slider>().value += (float)relaxMeterDirection * increasePowerMeterAmount;
            if (relaxMeter.GetComponentInChildren<Slider>().value >= 3.2f)
            {
                relaxMeterDirection = -1;
            }
            else if (relaxMeter.GetComponentInChildren<Slider>().value <= 1)
            {
                relaxMeterDirection = 1;
            }
        }
    }
    public void stopRelax()
    {
        startRelax = false;
        relaxAmount = (int)relaxMeter.GetComponentInChildren<Slider>().value;
        returnRelaxAmount = 0;
        if(relaxAmount == 3)
        {
            returnRelaxAmount = 0.3f;
        } else if(relaxAmount == 2)
        {
            returnRelaxAmount = 0.2f;
        } else if(relaxAmount == 1)
        {
            returnRelaxAmount = 0.1f;
        }
        rageMeter.GetComponent<RageMeterController>().reduceRage(returnRelaxAmount);
        strokeController.SetActive(false);
        calmDownPanel.SetActive(true);
        calmDownText.text = "Calmed Down: " + relaxAmount.ToString();
        secondsTillCalmDone = 0;
    }
}
