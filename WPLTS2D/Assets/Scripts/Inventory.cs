using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Transform Slots;
    Transform AllItems;
    public dynamic I;
    // Start is called before the first frame update
    void Start()
    {
        Slots = GameObject.Find(transform.root.gameObject.name + "/Screen/Inventory/Panel/Slots").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
