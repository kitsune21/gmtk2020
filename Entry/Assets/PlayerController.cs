﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private SpriteRenderer mySprite;
    public int framesTillHit;
    public int framesTillHitCount;

    public AudioClip[] sounds;
    public AudioClip rageInitYell;
    private AudioSource audioSource;

    private AudioClip defaultMusic;
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

    public Camera mainCamera;
    public float lerpRange;
    public float lerpTimer;
    public float lertTimerMax;
    private Vector3 newCamPos;
    private bool moveCamera;

    public GameObject displayControls;
    private bool firstHit;
    public float numOfFramesTillControlsGo;
    public float numOfFramesTillControlsGoMax;
    private Image[] panelsList;
    private Color newColor;

    public GameObject rageInstructions;
    private int holeCount;

    // Start is called before the first frame update
    void Start()
    {
        disableInput = false;
        myAnim = GetComponentInChildren<Animator>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        mainCameraAudio = GetComponent<AudioSource>();
        defaultMusic = mainCameraAudio.clip;
        mainCameraAudio.loop = true;
        mainCameraAudio.Play();
        displayControls.SetActive(true);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        displayControls.transform.position = Camera.main.WorldToScreenPoint(pos);
        panelsList = displayControls.GetComponentsInChildren<Image>();
        newColor = new Color(panelsList[0].color.r, panelsList[0].color.g, panelsList[0].color.b, panelsList[0].color.a);
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
                    if (!startRelax)
                    {
                        if (startPowerMeter)
                        {
                            stopPowerMeter();
                        }
                        else
                        {
                            startPowerMeter = true;
                            powerMeterObj.SetActive(true);
                            float sideOfCharacter = 0;
                            float modifyHeight = 0;
                            if (transform.position.x > 5f)
                            {
                                sideOfCharacter = -1.5f;
                            }
                            else
                            {
                                sideOfCharacter = 1.5f;
                            }
                            if (transform.position.y > 2.5)
                            {
                                modifyHeight = -0.7f;
                            }
                            else if (transform.position.y < 2.5f)
                            {
                                modifyHeight = 0.7f;
                            }
                            Vector3 pos = new Vector3(transform.position.x + sideOfCharacter, transform.position.y + modifyHeight, transform.position.z);
                            powerMeterObj.transform.position = Camera.main.WorldToScreenPoint(pos);
                        }
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
                // play a random grunt when the ball stops
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
            }
            else
            {
                rageHitCount++;
                if (rageHitCount == 3)
                {
                    rageHitCount = 0;
                    rageInControl = false;
                    mainCameraAudio.clip = defaultMusic;
                    mainCameraAudio.loop = true;
                    mainCameraAudio.Play();
                    rageMeter.GetComponent<RageMeterController>().resetRage();
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
        lerpCamera();
        if (numOfFramesTillControlsGo < numOfFramesTillControlsGoMax)
        {
            numOfFramesTillControlsGo++;
            newColor = new Color(newColor.r, newColor.g, newColor.b, newColor.a - 0.001f);
            foreach(Image panel in panelsList)
            {
                panel.color = newColor;
            }
        }
        if(numOfFramesTillControlsGo >= numOfFramesTillControlsGoMax)
        {
            displayControls.SetActive(false);
        }
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
            newCamPos = getRotation();
            newCamPos = new Vector3(newCamPos.x * 0.18f, newCamPos.y * 0.18f, mainCamera.transform.position.z);
            lerpTimer = 0;
            //moveCamera = true;
        }
    }

    private void lerpCamera()
    {
        if (moveCamera)
        {
            if (lerpTimer < lertTimerMax)
            {
                lerpTimer += Time.deltaTime;
                lerpRange += 1.1f * Time.deltaTime;
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCamPos, lerpRange);

            }
            else if (lerpTimer >= lertTimerMax)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0, 0, mainCamera.transform.position.z), 1);
                lerpTimer = 10000;
                moveCamera = false;
                lerpRange = 0.1f;
            }
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
        if (!rageInControl)
        {
            if (rageMeter.GetComponent<RageMeterController>().getRageLevel() >= 1f)
            {
                int livesCount = myLives.GetComponent<ClubController>().playerDied();
                if (livesCount <= 0)
                {
                    // end the game
                    GameObject parentObject = GameObject.FindGameObjectWithTag("parent");
                    Destroy(parentObject);
                    SceneManager.LoadScene("GameOver");
                }
                else
                {
                    //change the music
                    mainCameraAudio.clip = rageMusic;
                    mainCameraAudio.Play();
                    audioSource.PlayOneShot(rageInitYell);
                    rageInControl = true;
                }
            }
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
        rageMeter.GetComponent<RageMeterController>().reduceRage(0.1f);
        strokeController.SetActive(true);
        strokeCardPanel.SetActive(true);
        strokeController.GetComponent<ScoreCard>().addScore(strokesCount);
        waitToStart = true;
    }

    public void startHole()
    {
        holeCount++;
        if (holeCount != 9)
        {
            waitToStart = false;
            strokesCount = 0;
            if (holeCount == 1)
            {
                rageInstructions.SetActive(true);
            }
            else
            {
                rageInstructions.SetActive(false);
            }
            startRelaxMeter();
        } else
        {
            strokeController.SetActive(false);
            Destroy(gameObject);
        }
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
            relaxMeter.GetComponentInChildren<Slider>().value += (float)relaxMeterDirection * (increasePowerMeterAmount + 0.05f);
            if (relaxMeter.GetComponentInChildren<Slider>().value >= relaxMeter.GetComponentInChildren<Slider>().maxValue)
            {
                relaxMeterDirection = -1;
            }
            else if (relaxMeter.GetComponentInChildren<Slider>().value <= relaxMeter.GetComponentInChildren<Slider>().minValue)
            {
                relaxMeterDirection = 1;
            }
        }
    }
    public void stopRelax()
    {
        relaxMeter.SetActive(false);
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
        mainCamera = Camera.main;
    }

    public int getHoleCount()
    {
        return holeCount;
    }
}
