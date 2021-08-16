using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public SheepBehaviour sheep;

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        sheep = GetComponent<SheepBehaviour>();
        transform.parent = null;
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    //Always move forward in the direction given by FlockBehaviour
    public void Move(Vector3 velocity)
    {
        if(sheep.running == false && sheep.returned == false)
        {
            transform.forward = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        
    }
}
