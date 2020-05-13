using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum Behavior { Wander, Stationary, Panic};
public class AI : MonoBehaviour
{
    public int Team = 0;
    NavMeshAgent agent;
    CharacterModelData anim;
    Behavior behavior;

    Vector3 currentTarget;
    Vector3 startpoint;
    float wanderDist = 8f;
    // Start is called before the first frame update
    void Start()
    {
        behavior = Behavior.Wander;
        startpoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
        Wander();
    }

    public void GoTo(Vector3 target)
    {
        currentTarget = target;
        agent.SetDestination(target);
    }
    void OnArrive()
    {
        if(behavior == Behavior.Wander)
        {
            Wander();
        }
    }
    public void Wander()
    {
        Vector3 target = startpoint + new Vector3(Random.Range(-wanderDist / 2, wanderDist / 2), 0, 0);
        GoTo(target);
    }
    // Update is called once per frame
    void Update()
    {
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            OnArrive();
        }
    }
}
