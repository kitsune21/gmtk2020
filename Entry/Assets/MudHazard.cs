using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudHazard : MonoBehaviour
{
    public float speedReduction = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            rb.velocity = rb.velocity / speedReduction;
        }
    }
}
