using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public int currentWeapon;
    public WeaponData wpnDta;
    CharacterModelData anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<CharacterModelData>();
        wpnDta = anim.Hand.GetChild(currentWeapon).GetComponent<WeaponData>();

    }
    public void Fire()
    {
        var bullet = Instantiate(wpnDta.Projectille);
        bullet.transform.position = wpnDta.FirePoint.position;
        bullet.transform.rotation = wpnDta.FirePoint.rotation;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50f;
        bullet.GetComponent<Rigidbody>().useGravity = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetButtonDown("Fire1") && anim.IsPlayer)
        {
            Fire();
        }
    }
}
