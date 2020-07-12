using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float maxForce;

    public bool ballHasStopped;
    public bool ballIsHit;
    private bool hitWater;
    private bool inWaterStopped;

    private int frameCount;
    public int maxFrameCount;

    public float powerRageMultiplier;

    public Vector3 newPosWater;
    // sounds
    public AudioClip wallSound;
    public AudioClip cupSound;
    public AudioClip moveSound;
    private AudioSource audioSource;

    public Vector3 posBeforeHit;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frameCount = 0;
        audioSource = GetComponent<AudioSource>();
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
                        if (hitWater)
                        {
                            inWaterStopped = true;
                        } else
                        {
                            posBeforeHit = transform.position;
                        }
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
        posBeforeHit = transform.position;
        frameCount = 0;
        rage = (1 - (1 / (1 + Mathf.Exp(rage * 10 - 5))));
        float force = ((power + rage) * powerRageMultiplier); 
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        ballIsHit = true;
        audioSource.PlayOneShot(moveSound);
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

    public bool getHitWater()
    {
        if (inWaterStopped)
        {
            inWaterStopped = false;
            hitWater = false;
            return true;
        }
        else
        {
            return inWaterStopped;
        }
    }

    public void resetHitWater()
    {
        inWaterStopped = false;
        hitWater = false;
    }

    public Vector3 getNewWaterPos()
    {
        return posBeforeHit;
    }

    public void setHitWater()
    {
        hitWater = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            audioSource.PlayOneShot(wallSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            audioSource.PlayOneShot(cupSound);

        }
    }
}
