using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity {
    Animator anim;
    Rigidbody2D rb;
    static int[] direction = new int[2];
    private float input_x;
    private float input_y;
    private bool action = false;

    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.drag = speed * 5f;

        InitActions();
    }
	
	void Update ()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");

        action = Input.GetButton("Action");
    }

    void FixedUpdate()
    {
        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        anim.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            if(action == false)
            {
                anim.SetFloat("x", input_x);
                anim.SetFloat("y", input_y);
                direction[0] = Mathf.RoundToInt(input_x);
                direction[1] = Mathf.RoundToInt(input_y);
            }
            else
            {
                if (direction[0] == 1 || direction[0] == -1)
                    input_y = 0;
                if (direction[1] == 1 || direction[1] == -1)
                    input_x = 0;
            }
            //transform.position += new Vector3(input_x, input_y).normalized * speed * Time.deltaTime;
            //rb.velocity = new Vector3(input_x, input_y).normalized * speed * Time.deltaTime;

            
            rb.velocity = new Vector3(input_x, input_y).normalized * speed;
        }

        //Action
        ThrowRayCast();
    }

    //Own functions

    Transform thingToPull; // null if nothing, else a link to some pullable crate
    public LayerMask crateLayer;
    private Rigidbody2D crateRB;

    void InitActions()
    {
    }

    void ThrowRayCast()
    {
        
        if(action)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction[0], direction[1]), 0.5f, crateLayer);
            if (hit.collider != null)
            {
                if (crateRB == null)
                {
                    crateRB = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                    crateRB.drag = speed * 5f;
                }
                crateRB.velocity = new Vector3(input_x, input_y).normalized * speed;
            }
        }
        else
        {
            if(crateRB != null)
            {
                crateRB.velocity = Vector2.zero;
                crateRB = null;
            }
        }
            
    }
}
