using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Transition;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    public Transform Camera;
    public Vector3 Speed = new Vector3(1.7f, 5, 0);
    Vector3 cameraOffset = new Vector3(0, 2f);
    float Zoom = 6;
    float runM = 2.5f;
    bool Direction = true; //t=right, f=left
    Rigidbody rb;
    CharacterModelData anim;
    Transform currentclimb = null;
    bool InAir = false;
    float cspeed;
    float hormov;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<CharacterModelData>();
        cspeed = Speed.x;
    }
    void JumpStuff()
    {
        if(Input.GetButtonDown("Jump") && anim.IsGrounded())
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position - new Vector3(0, .05f, 0), transform.forward, Color.blue, 2);
            if (Physics.Raycast(transform.position - new Vector3(0, .05f, 0), transform.forward, out hit, .85f))
            {
                if (hit.transform.gameObject.tag == "Vault")
                {
                    anim.Body.Play("Vault");
                    return;
                }
                if(hit.transform.gameObject.tag == "Climb")
                {
                    anim.Body.Play("ClimbWall");
                    currentclimb = hit.transform;
                    return;
                }
            }
            rb.velocity = new Vector3(rb.velocity.x, Speed.y);
            InAir = true;
            anim.Jump();
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
    public float GetDir()
    {
        float dir = 1;
        if (!anim.direction)
            dir = -1;
        return dir;
    }
    public void FinishClimb()
    {
        transform.position += new Vector3(.65f * GetDir(), currentclimb.GetChild(0).position.y, 0);
        currentclimb = null;
    }
    public void OnJump()
    {
        CapsuleCollider c = GetComponent<CapsuleCollider>();
        c.height = 1.93f / 2;
        c.center = new Vector3(0, 1.39f, 0);
    }
    public void OnLand()
    {
        CapsuleCollider c = GetComponent<CapsuleCollider>();
        c.height = 1.93f;
        c.center = new Vector3(0, 0.89f, 0);
        InAir = false;
    }
    void FixedUpdate()
    {
        if(Camera != null)
            Camera.position = Vector3.Lerp(Camera.position, new Vector3(transform.position.x, transform.position.y, -Zoom)+cameraOffset, Time.deltaTime * 10);
        //deal with movement
        if (!CanMove)
            return;
        bool grounded = anim.IsGrounded();
        if (InAir)
        {
            if (grounded)
                InAir = false;
            else
            {
                //say you run into a climbable wall
                RaycastHit hit;
                if (Physics.Raycast(transform.position - new Vector3(0, .1f, 0), transform.forward, out hit, .85f))
                {
                    if(hit.transform.gameObject.tag == "Climb")
                    {
                        currentclimb = hit.transform;
                        anim.Body.Play("ClimbWall");
                        transform.position = new Vector3(transform.position.x, hit.transform.GetChild(0).position.y - 2.7f); 
                    }
                }
            }
        }
        float dir = GetDir();

        if(grounded)
            hormov = Input.GetAxis("Horizontal");
        if (anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {
            rb.isKinematic = true;
            transform.position = transform.position + new Vector3(Time.deltaTime * 2.25f * dir * 1.5f, 0);
            return;
        }
        rb.isKinematic = false;
        JumpStuff();
        if(grounded)
        {
            cspeed = Speed.x;
            if (Input.GetButton("Run"))
            {
                anim.Speed = 2;
                cspeed *= runM;
                if (anim.direction == true && hormov < 0 || anim.direction == false && hormov > 0)
                {
                    if (!anim.HandOccupied)
                        anim.direction = !anim.direction;
                    else
                        cspeed = Speed.x;
                }
            }
            else
            {
                anim.Speed = 1;
            }
        }

        if (anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            rb.velocity = new Vector2(dir * Speed.x * runM, rb.velocity.y);
            anim.Moving = rb.velocity.x != 0;
            grounded = false;
        }
        else
        {
            rb.velocity = new Vector2(hormov * cspeed, rb.velocity.y);
            anim.Moving = rb.velocity.x != 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
