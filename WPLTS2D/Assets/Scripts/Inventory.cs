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
        I = FindObjectOfType<GM>().Master.AddInventory("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
