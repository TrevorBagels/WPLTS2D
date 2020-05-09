using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelData : MonoBehaviour
{
    public bool IsPlayer;
    bool direction;
    public Transform Arm;
    public Transform Hand;
    public Transform Bottom;
    public Animator Upper;
    public Animator Lower;

    public bool Moving;
    public bool HandOccupied;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(Bottom.position, -Bottom.up, out hit, .25f))
        {
            return true;
        }
        return false;
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
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Arm.rotation = Quaternion.Euler(0f, 0f, rot_z);
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
    void Update()
    {
        Lower.SetBool("Air", !IsGrounded());
        Lower.SetBool("Running", Moving);
        SetRotation();
    }
}
