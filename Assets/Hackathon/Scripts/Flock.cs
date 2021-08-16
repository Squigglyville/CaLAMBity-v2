using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flock : MonoBehaviour
{
    //The Flock is the gameObject from which the each sheep spawns from and then stays close to
    //Its position is recalculated every time the sheep are barked at, so it always remains in the centre of all the sheep of its flock

    public FlockAgent agentPrefab;                                  //The FlockAgent prefab that each sheep has to mark them as a member of this particular flock
    public List<FlockAgent> agents = new List<FlockAgent>();        //List of all the FlockAgents in this Flock
    public List<Transform> sheep = new List<Transform>();           //List of all the sheep transforms in this flock
    public float x = 0f;                                        
    public float y = 0f;                                            //x, y, z coords used to get the average vector position of all the sheep in the flock
    public float z = 0f;
    public FlockBehavior behavior;                                  //Access the CompositeFlockBehaviour in each sheep, which is a combination of many flock behaviours
    public StayInRadiusBehavior radius;                             //The radius around the Flock that the sheep will stay within
    public NavMeshAgent navAgent;                                   //Access the navAgent in each sheep
    public SheepBehaviour mySheep;                                  //Access the SheepBehaviour in each sheep
    float waitTime;                                                 //Timer for when the Flock position moves after the sheep have settled
            
    public bool moving;                                             //When the flock has been displaced by the player, this is true

    [Range(3, 50)]
    public int startingCount = 250;                                 //How many sheep to spawn at Start
    [Range(1f, 100f)]
    public float driveFactor = 10f;                                 //Factor to increase sheep general movement 
    [Range(1f, 100f)]
    public float maxSpeed = 5f;                                     //Maximum speed at which sheep moves
    [Range(1f, 100f)]
    public float neighborRadius = 1.5f;                             //Radius at which sheep checks for other objects and sheep
    

    float squareMaxSpeed;
    float squareNeighborRadius;                                     //Square values to prevent negatives 
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }    //Used for AvoidanceBehaviour to detect sheep to avoid

    private void Awake()
    {
        //Instantiate a number of sheep equal to the starting count, at this location, with a random forward rotation and as a child of this Flock
        //Then add the agent to this Flocks list of agent
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, transform.position, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
               
    }

    
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;               
        squareNeighborRadius = neighborRadius * neighborRadius;
        
        mySheep = GetComponentInChildren<SheepBehaviour>();     //Getting the first sheep in the list for sampling purposes
                
    }

    // Update is called once per frame
    void Update()
    {
        radius.center = transform.position;                                 //Make sure the StayInRadius circle is always centred on the middle of the flock

        //Calculates the random movements within a flock that occur when the player does not interact
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);              //List of nearby objects with colliders to each sheep
            Vector3 move = behavior.CalculateMove(agent, context, this);    //Calculate move according to the CompositeFlockBehaviour using nearby objects as context
            move *= driveFactor;                                            //Increase distance and speed moved at
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;                          //Prevent the move from being too great and causing errors
            }
            agent.Move(move);                                               //Move the agent
        }
        
        //When any sheep of the flock has been barked at, moving becomes true
        //While moving, the flock waits until the sheep have settled, then recalculates its position by using the average of all the sheep in its flock
        if (moving == true)
        {                          
                                      
            waitTime += Time.deltaTime;
            if (waitTime >= mySheep.timeUntilRunEnds + 1)
            {
                moving = false;
                waitTime = 0;
                GetAverageVector();
            }
        }
        else
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        //if there are no agents left in the flock, destroy the flock
        if (agents.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    //Makes a list of transforms and an array of colliders for each agent that detects other agent colliders within a radius
    //Then returns what it finds
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    //Get all the Transforms in all the sheep of this flock and find the average position
    void GetAverageVector()
    {
        if (sheep.Count == 0)
        {
            gameObject.transform.position = Vector3.zero;
        }

        foreach (Transform pos in sheep)
        {
            x += pos.transform.position.x;
            y += pos.transform.position.y;
            z += pos.transform.position.z;
        }
        Vector3 averageVector = new Vector3(x / sheep.Count, y / sheep.Count, z / sheep.Count);
        
        transform.position = averageVector;
        
    }
}
