using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float maxForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // testing hitting from an angle, delete when we get player provided direction
        //transform.rotation = Quaternion.Euler(0, 0, 170);
        //HitBall(transform.right, actualForce);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitBall(Vector3 dir, float power)
    {
        float force = maxForce * power;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
