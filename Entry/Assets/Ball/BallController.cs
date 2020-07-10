using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float playerRotateAmount;

    public float increasePowerMeterAmount;
    private int powerMeterDirection = 1;

    public Slider powerMeter;

    public bool startPowerMeter;

    private float powerMeterFinalValue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            }else
            {
                startPowerMeter = true;
            }
        }
        updatePowerMeter();
    }

    private void updatePowerMeter()
    {
        if (startPowerMeter)
        {
            powerMeter.value += ((float)powerMeterDirection * increasePowerMeterAmount);
            if(powerMeter.value >= 1)
            {
                powerMeterDirection = -1;
            } else if(powerMeter.value <= 0)
            {
                powerMeterDirection = 1;
            }
        }
    }
    
    private void stopPowerMeter()
    {
        startPowerMeter = false;
        powerMeterFinalValue = powerMeter.value;
    }

    private void hitTheBall()
    {

    }

    private void resetCharacter()
    {
        powerMeterDirection = 1;
        startPowerMeter = false;
        powerMeterFinalValue = 0;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public float getPowerMeterValue()
    {
        return powerMeterFinalValue;
    }

    public Quaternion getRotation()
    {
        return transform.rotation;
    }
}
