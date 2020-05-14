using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Transition;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public Transform Camera;
    Vector3 Speed;
    Vector3 cameraOffset = new Vector3(0, 2f);
    float zoom = 6;
    float runM = 2.5f;
    bool Direction = true; //t=right, f=left
    Rigidbody rb;
    CharacterModelData anim;
    Transform currentclimb = null;
    bool InAir = false;
    float cspeed;
    float hormov;
    Conf.PlayerConfig config;
    float timestuck = 0f;
    Vector3 stuckpos;
    float airtime = 0f;
    int VaultLevel = 0;
    void Awake()
    {
        config = FindObjectOfType<GM>().config.Player;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<CharacterModelData>();
        cspeed = Speed.x;
        Speed = config.Speed;
        cameraOffset = config.CameraOffset;
        zoom = config.Zoom;
        runM = config.RunMultiplier;

    }
    void JumpStuff()
    {
        if(Input.GetButton("Jump") && anim.IsGrounded())
        {
            //check if we're trying to vault over something
            RaycastHit hit;
            Debug.DrawRay(transform.position - new Vector3(0, .05f, 0), transform.forward, Color.blue, 2);
            if (Physics.Raycast(transform.position - new Vector3(0, .05f, 0), transform.forward, out hit, config.VaultCheckDistance))
            {
                if (hit.transform.gameObject.tag == "Vault")
                {
                    VaultLevel = 1;
                    anim.Body.Play("Vault");
                    return;
                }
                if (hit.transform.gameObject.tag == "VaultLong")
                {
                    VaultLevel = 2;
                    anim.Body.Play("Vault");
                    return;
                }
            }
        }
        if (Input.GetButton("Jump") && anim.IsGrounded())
        {
            //create upwards velocity
            rb.velocity = new Vector3(rb.velocity.x, Speed.y);
            anim.Jump();
        }
        //crouch if we're stationary
        if (Input.GetButton("Roll") && !anim.Moving)
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
    void StuckFix(bool grounded)
    {
        if (!grounded && Mathf.Abs(transform.position.y - stuckpos.y) < .001f)
            timestuck += Time.deltaTime;
        else
            timestuck = 0f;
        stuckpos = transform.position;
        if (timestuck > .05f)
        {
            rb.velocity = new Vector3(rb.velocity.x, Speed.y);
            timestuck = 0f;
        }
    }
    void FixedUpdate()
    {
        //align camera
        if (Camera != null)
            Camera.position = Vector3.Lerp(Camera.position, new Vector3(transform.position.x, transform.position.y, -zoom) + cameraOffset, Time.deltaTime * 10);
        //deal with movement
        if (!CanMove) //don't move if you can't move. 
            return;
        //initialize important variables
        bool grounded = anim.IsGrounded();
        //fix player when stuck
        StuckFix(grounded);
        if (!grounded)
            airtime += Time.deltaTime;
        else
            airtime = 0f;
        float dir = GetDir();
        if(airtime > .15f)
            if(!anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Fall") && !anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                //treat this as if we just jumped
                anim.AccountForFall();
                anim.Body.Play("Fall");
            }
        if (grounded)//only modify horizontal movement if we're on the ground
            hormov = Input.GetAxis("Horizontal");
        //if we're vaulting over something
        if (anim.Body.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {
            rb.isKinematic = true;
            if(VaultLevel == 1)
                transform.position = transform.position + new Vector3(Time.deltaTime * 2.25f * dir * 1.5f, 0);
            if(VaultLevel == 2)
                transform.position = transform.position + new Vector3(Time.deltaTime * 2.85f * dir * 1.5f, 0);
            return;
        }
        rb.isKinematic = false;
        JumpStuff();
        if (grounded)
        {
            cspeed = Speed.x;//cspeed = current speed. set it to the walking speed
            if (Input.GetButton("Run"))
            {
                anim.Speed = 2;//tell the character model that we're running
                cspeed *= runM;//update the speed to run speed
                if (anim.direction == true && hormov < 0 || anim.direction == false && hormov > 0)//if you try to run away from the direction you're pointing, don't run backwards, just turn around.
                {
                    if (!anim.HandOccupied)
                        anim.direction = !anim.direction;//switch direction
                    else
                        cspeed = Speed.x; //if we're aiming, and we try running backwards, just make the player walk backwards
                }
            }
            else
            {
                anim.Speed = 1;//not running
            }
        }
        //is rolling, keep some kind of velocity going.
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
}