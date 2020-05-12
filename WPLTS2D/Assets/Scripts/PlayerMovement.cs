using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    public Transform Camera;
    Vector3 cameraOffset = new Vector3(0, 2f);
    float Zoom = 6;
    public Vector3 Speed = new Vector3(6, 5, 0);
    float runM = 1.5f;
    bool Direction = true; //t=right, f=left
    Rigidbody rb;
    CharacterModelData anim;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<CharacterModelData>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void JumpStuff()
    {
        if(Input.GetButtonDown("Jump") && anim.IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, Speed.y);
            anim.Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) && Mathf.Abs(rb.velocity.x) > .5f)
        {
            anim.Body.Play("Roll", 1);
        }
        if(Input.GetButton("Roll") && !anim.Moving)
        {
            anim.Body.SetBool("Crouching", true);
        }
        else
        {
            anim.Body.SetBool("Crouching", false);
        }
    }
    void FixedUpdate()
    {
        if(Camera != null)
            Camera.position = Vector3.Lerp(Camera.position, new Vector3(transform.position.x, transform.position.y, -Zoom)+cameraOffset, Time.deltaTime * 10);
        //deal with movement
        if (!CanMove)
            return;
        JumpStuff();
        float cspeed = Speed.x;
        if (Input.GetButton("Run"))
        {
            anim.Speed = 2;
            cspeed *= runM;
        }
        else
        {
            anim.Speed = 1;
        }
        float hormov = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(hormov * cspeed, rb.velocity.y);
        anim.Moving = rb.velocity.x != 0;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
