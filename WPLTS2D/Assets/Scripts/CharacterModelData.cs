using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelData : MonoBehaviour
{
    public bool IsPlayer;
    bool direction;
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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = transform.GetComponentInParent<CapsuleCollider>().height/2 - transform.GetComponentInParent<CapsuleCollider>().center.y;
    }
    public bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + .05f))
        {
            Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        return false;
    }
    public void Jump()
    {
        Body.Play("Jump");
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
        Vector3 diff = p - p2;
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
        Arm.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        if(direction == true)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 90 + 180, 0);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Body.SetBool("Running", Speed > 1);
        Body.SetBool("Moving", Moving);
        Body.SetBool("Air", !IsGrounded());
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
    }
}
