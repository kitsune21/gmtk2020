﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float maxForce;

    public bool ballHasStopped;
    public bool ballIsHit;

    private int frameCount;
    public int maxFrameCount;

    public float powerRageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frameCount = 0;
        // testing hitting from an angle, delete when we get player provided direction
        //transform.rotation = Quaternion.Euler(0, 0, 170);
        //HitBall(transform.right, actualForce);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ballIsHit)
        {
            if (rb.velocity.x == 0 && rb.velocity.y == 0)
            {
                if (frameCount >= maxFrameCount)
                {
                    if (rb.velocity.x == 0 && rb.velocity.y == 0)
                    {
                        ballHasStopped = true;
                        ballIsHit = false;
                        frameCount = 0;
                    }
                }
                frameCount += 1;
            }
        }
    }

    public void HitBall(Vector3 dir, float power, float rage)
    {
        frameCount = 0;

        float force = maxForce * ((power + rage) * powerRageMultiplier);
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        ballIsHit = true;
    }

    public bool getBallHasStopped()
    {
        if (ballHasStopped)
        {
            ballHasStopped = false;
            return true;
        }
        else
        {
            return ballHasStopped;
        }
    }
}
