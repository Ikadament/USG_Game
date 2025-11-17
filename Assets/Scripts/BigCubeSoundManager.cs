using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCubeSoundManager : MonoBehaviour
{
    public float minFallSpeed = 8f;

    private Rigidbody2D rb;
    private GroundCheck groundCheck;

    public bool wasFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {

        // objet est en chute libre
        if (rb.velocity.y < -minFallSpeed)
        {
            wasFalling = true;

        }

        if(wasFalling && groundCheck.isGrounded)
        {
            FindObjectOfType<AudioManager>().Play("blockFall");
            Debug.Log("sound!");
            wasFalling = false;
        }
    }
}
