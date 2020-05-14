using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int hits = 0;
    public int maxHits = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    void OnCollisionEnter(Collision col)
    {
        if (hits >= maxHits)
        {
            Destroy(gameObject);
            return;
        }
        hits += 1;
        if (col.transform.GetComponentInParent<AI>())
        {
            col.transform.GetComponentInParent<CharacterModelData>().Die();
            foreach (Rigidbody rb in col.transform.root.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(40, transform.position - transform.forward * 2, 1f, 0f, ForceMode.Impulse);
            }
        }
        if (hits >= maxHits)
            Destroy(gameObject);
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/BloodStream"));
        g.transform.position = transform.position;
        g.transform.rotation = transform.rotation;
        g.transform.parent = transform;
    }
    // Update is called once per frame
    void Update()
    {

    }
}