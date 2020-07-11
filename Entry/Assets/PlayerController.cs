using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float playerRotateAmount;

    public float increasePowerMeterAmount;
    private int powerMeterDirection = 1;

    public Slider powerMeter;
    public GameObject rageMeter;

    public bool startPowerMeter;

    public bool disableInput;

    private float powerMeterFinalValue;

    public GameObject myBall;
    public GameObject tempBallParent;

    // Start is called before the first frame update
    void Start()
    {
        disableInput = false;
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
                }
            }
        }
        if (myBall.GetComponent<BallMoveController>().getBallHasStopped())
        {
            resetCharacter();
            rageMeter.GetComponent<RageMeterController>().addRage();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        updatePowerMeter();
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
        powerMeterFinalValue = powerMeter.value;
        hitTheBall();
    }

    private void hitTheBall()
    {
        myBall.GetComponent<BallMoveController>().HitBall(getRotation(), getPowerMeterValue(), rageMeter.GetComponent<RageMeterController>().getRageLevel());
        myBall.transform.SetParent(tempBallParent.transform, true);
    }

    private void resetCharacter()
    {
        Debug.Log("test");
        disableInput = false;
        powerMeterDirection = 1;
        startPowerMeter = false;
        powerMeterFinalValue = 0;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = myBall.transform.position;
        myBall.transform.SetParent(transform, true);
    }

    public float getPowerMeterValue()
    {
        return powerMeterFinalValue;
    }

    public Vector3 getRotation()
    {
        return transform.up;
    }
}
