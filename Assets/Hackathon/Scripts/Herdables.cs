using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Herdables : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public bool returned;
    public float pointValue;
    public ParticleSystem particles;
    public AudioSource myAudio;
    public AudioClip[] audioClipArray;
    public bool running;
    float timeWhileRunning;
    public float timeUntilRunEnds;
    public float runDistance;
    public float remainingDistance;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        returned = false;
        particles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        remainingDistance = navAgent.remainingDistance;
        if (running == true && returned == false)
        {
            Vector3 directionToPlayer = transform.position - References.thePlayer.transform.position;
            directionToPlayer = directionToPlayer.normalized * runDistance;
            Vector3 newPosition = transform.position + directionToPlayer;
            navAgent.SetDestination(newPosition);

            timeWhileRunning += Time.deltaTime;
            if (timeWhileRunning >= timeUntilRunEnds)
            {
                navAgent.ResetPath();
                timeWhileRunning = 0;
                running = false;
                animator.SetBool("Angry", true);
                animator.SetBool("IsRunning", false);
                
            }
        }

        if(animator.GetBool("Angry") == true)
        {
            transform.LookAt(References.thePlayer.transform);
        }

        if(returned == true)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("Angry", false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<characterController>() != null && returned == false)
        {
            float distance = Vector3.Distance(transform.position, References.thePlayer.transform.position);
            running = true;
            myAudio.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
            myAudio.PlayOneShot(myAudio.clip);
            animator.SetBool("IsRunning", true);
            animator.SetBool("Angry", false);
        }

    }
}
