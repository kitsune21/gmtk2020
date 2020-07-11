using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    private Vector3 ballTransformOnEnter;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
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
