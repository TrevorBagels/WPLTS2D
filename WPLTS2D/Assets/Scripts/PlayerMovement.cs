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
        if(Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, Speed.y);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            anim.Body.Play("Roll", 1);
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
        float hormov = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(hormov * Speed.x, rb.velocity.y);
        anim.Moving = rb.velocity.x != 0;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
