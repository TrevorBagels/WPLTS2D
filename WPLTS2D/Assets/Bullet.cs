using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject);
        if (hit)
        {
            Destroy(gameObject);
            return;
        }
        hit = true;
        if(col.transform.GetComponentInParent<AI>())
        {
            col.transform.GetComponentInParent<CharacterModelData>().Die();
            foreach(Rigidbody rb in col.transform.root.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(40, transform.position - transform.forward*2, 1f, 0f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
