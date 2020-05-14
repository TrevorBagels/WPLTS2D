using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterModelData : MonoBehaviour
{
    bool InRagdoll = false;
    public bool IsPlayer;
    public bool direction;
    Rigidbody rb;
    public Transform Arm;
    public Transform Hand;
    public Transform Bottom;
    public Animator Body;
    public Vector3 TestThingy;
    public bool Moving;
    public int Speed = 1;
    public bool HandOccupied;
    float distToGround;
    bool jumped = false;
    float jumpmax; //new jump point
    float jumplow;//initial jump point
    NavMeshAgent agent;
    public Collider Hitbox;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetJointsState(true);
        distToGround = transform.GetComponentInParent<CapsuleCollider>().height/2 - transform.GetComponentInParent<CapsuleCollider>().center.y;
    }

    public void Die()
    {
        if (GetComponent<Rigidbody>())
            Destroy(GetComponent<Rigidbody>());
        SetJointsState(false);
        GetComponentInChildren<Animator>().enabled = false;
        InRagdoll = true;
        if(GetComponent<NavMeshAgent>())
        {
            Destroy(GetComponent<NavMeshAgent>());
        }
        if(GetComponent<AI>())
        {
            GetComponent<AI>().enabled = false;
        }

    }
    void SetLowWeight()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.mass = .05f;
        }
    }
    //true for living, false for dead
    void SetJointsState(bool state)
    {
        if(state == false)
        {
            Invoke("SetLowWeight", .5f);
        }
        
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = state;
        }
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach(Collider c in cols)
        {
            c.enabled = !state;
        }
        Hitbox.enabled = state;
        GetComponent<CapsuleCollider>().enabled = state;
        if(GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().isKinematic = !state;

    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + .15f))
        {
           // Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        return false;
    }
    public void OnLand()
    {
        jumped = false;

    }
    public void Jump()
    {
        AccountForFall();
        Body.Play("Jump");
    }
    public void AccountForFall()
    {
        jumped = true;
        jumpmax = transform.position.y;
        jumplow = transform.position.y;
    }
    void SetRotation()
    {
        Vector3 p = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        if (IsPlayer)
        {
            p = Camera.main.WorldToScreenPoint(Arm.position);
            p2 = Input.mousePosition;
        }
        if (p2.x > p.x)
            direction = true;
        else
            direction = false;
        Vector3 diff = p2 - p;
        diff.Normalize();
        float addtox = 0;
        float multiplyz = 1;
        if (!direction)
        {
            addtox = 180;
            multiplyz = -1;
        }
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Arm.rotation = Quaternion.Euler(0f-addtox, 0f, rot_z*multiplyz);
        Arm.GetChild(0).localEulerAngles = new Vector3(-90, 0, 0);
        Arm.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 2, 0);


    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (InRagdoll)
            return;
        Body.SetBool("Running", Speed > 1);
        Body.SetBool("Moving", Moving);
        Body.SetBool("Air", !IsGrounded());
        if(!IsPlayer)
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
            Moving = Mathf.Abs(agent.velocity.x) > .5f;
            Body.SetFloat("MovementX", Mathf.Clamp(Mathf.Abs(agent.desiredVelocity.x), -1, 1));
            if(agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                var lookrot = Quaternion.LookRotation(agent.velocity.normalized);
                lookrot.x = 0;
                lookrot.z = 0;
                transform.rotation = lookrot;
            }
            return;
        }
        if(jumped)
        {
            jumpmax = Mathf.Clamp(transform.position.y, jumpmax, Mathf.Infinity);
            jumplow = Mathf.Clamp(transform.position.y, Mathf.NegativeInfinity, jumplow);
            Body.SetBool("JumpRoll", (jumpmax - jumplow) > 1.95);
        }
        if(IsPlayer)
        {
            float d = 1;
            if (!direction)
                d = -1;
            Body.SetFloat("MovementX", Input.GetAxis("Horizontal")*d);
            HandOccupied = Input.GetButton("Fire2");
        }
        if(HandOccupied)
            SetRotation();
        if (direction == true)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 90 + 180, 0);
        }
    }
}
