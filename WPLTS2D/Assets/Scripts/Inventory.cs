using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string Name;
    public string Desc;
    public float BaseDamage;
    public float MaxDamage;
    public float FireRate;
    public string Icon;
    public int ID;
}


public class Inventory : MonoBehaviour
{
    Transform Slots;
    List<Weapon> Weapons = new List<Weapon>();
    // Start is called before the first frame update
    void Start()
    {
        Slots = GameObject.Find(transform.root.gameObject.name + "/Screen/Inventory/Panel/Slots").transform;
    }
    void AddItem(Weapon wpn, int slot)
    {
        Weapons[slot] = wpn;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
