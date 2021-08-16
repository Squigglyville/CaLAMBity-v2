using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepBehaviour : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public bool returned;                   //Has the sheep returned to the barn
    public float pointValue;                //How many points is the sheep worth
    public ParticleSystem particles;
    public AudioSource myAudio;
    public AudioClip[] audioClipArray;
    public bool running;
    float timeWhileRunning;
    public float timeUntilRunEnds;          
    public Flock theFlock;
    public float runDistance;               //How far to run when barked at
    public float remainingDistance;
    public Animator animator;
    
    //Used for the Knife Powerup
    public bool angry;
    public GameObject eyebrows;

    //Add sheep to Referenced list of all sheep
    protected private void OnEnable()
    {
        References.allSheep.Add(this);
    }

    //Remove sheep from Referenced list of all sheep
    protected private void OnDisable()
    {
        References.allSheep.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        returned = false;
        particles.gameObject.SetActive(false);
        theFlock = GetComponentInParent<Flock>();
        theFlock.sheep.Add(gameObject.transform);
        angry = false;
    }

    // Update is called once per frame
    void Update()
    {
        remainingDistance = navAgent.remainingDistance;                                                 //Always know the navagents remaining distance
        if(running == true && returned == false)                                                        //When barked at, running becomes true
        {
            Vector3 directionToPlayer = transform.position - References.thePlayer.transform.position;   //Get a direction from the sheep to the player
            directionToPlayer = directionToPlayer.normalized * runDistance;                             //Normalise the magnitude, then times it by the distance to run so all sheep run same distance
            Vector3 newPosition = transform.position + directionToPlayer;                               //Create a new Vector3 by extrapolating the line between the player and sheep
            navAgent.SetDestination(newPosition);                                                       //Set that position as the navagent destination
            
            timeWhileRunning += Time.deltaTime;
            if(timeWhileRunning >= timeUntilRunEnds)                                                    //After a set time, stop moving towards the destination
            {
                timeWhileRunning = 0;
                running = false;
                navAgent.ResetPath();
            }
        }

        //When the sheep enters the barn area
        if (returned == true)                                                                            
        {
            theFlock.sheep.Remove(gameObject.transform);    
            References.allSheep.Remove(this);                                                       
            angry = false;
            navAgent.destination = References.barn.point.transform.position;
        }

        if(angry == true)
        {
            navAgent.destination = References.thePlayer.transform.position;
            
        }
       
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //When the sheep encounters the Players Bark Collider
        if (other.GetComponent<characterController>() != null && returned == false && angry == false)
        {
            float distance = Vector3.Distance(transform.position, References.thePlayer.transform.position);
            animator.speed = 2;
            running = true;
            myAudio.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
            myAudio.PlayOneShot(myAudio.clip);
            theFlock.moving = true;
            
        }

        //When the sheep encounters the collider in the barn once it is out of sight
        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }


    }
}
