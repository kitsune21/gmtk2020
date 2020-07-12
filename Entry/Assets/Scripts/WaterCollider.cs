using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    private Vector3 ballTransformOnEnter;

    public float speedReduction = 1.5f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            rb.velocity = rb.velocity / speedReduction;
            col.GetComponent<BallMoveController>().setHitWater();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collision.GetComponent<BallMoveController>().resetHitWater();
        }
    }
}
