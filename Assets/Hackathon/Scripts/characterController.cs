using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class characterController : MonoBehaviour
{
    public float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public AudioSource bark;
    public Collider barkCollider;
    bool barking;
    float timeBarking;
    public float barkLength = 1;
    public Rigidbody myRigidBody;
    public float jumpHeight;
    public bool grounded = true;
    public GameObject cameraPosition1;
    public GameObject cameraPosition2;
    public Camera myCamera;
    bool cameraDown;
    public camMouseLook cameraControl;
    public Animator animator;
    public GameObject cloudDust;
    public bool stunned;
    float timeSinceStunned;
    public float timeForStun;
    public float stunReset;
    public GameObject particlePosition;
    public NavMeshAgent navMesh;
    public ParticleSystem sprintDust;

    public float barkCooldown;
    float barkTimer;
   

    // Start is called before the first frame update
    void Start()
    {
        
        barkCollider.enabled = false;
        barking = false;
        References.thePlayer = this;
        myRigidBody = GetComponent<Rigidbody>();
        myCamera.transform.position = cameraPosition2.transform.position;
        cameraDown = false;
        cameraControl = myCamera.GetComponent<camMouseLook>();
        animator = GetComponent<Animator>();
        sprintDust.gameObject.SetActive(true);
        sprintDust.Stop();
        navMesh = GetComponent<NavMeshAgent>();
        myCamera.GetComponent<camMouseLook>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if(stunned == false)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(straffe, 0, translation);

            if (straffe == 0 && translation == 0)
            {
                animator.SetBool("IsRunning", false);
            }
            else
            {
                animator.SetBool("IsRunning", true);
            }

            //jumping with spacebar
            if (Input.GetButtonDown("Jump") && grounded == true)
            {
                navMesh.enabled = false;
                myRigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                grounded = false;
                animator.SetTrigger("IsJumping");

            }
                                          
            //barking with left mouse button

            barkTimer += Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                if(barkTimer >= barkCooldown)
                {
                    bark.Play();
                    barkCollider.enabled = true;
                    barking = true;
                    barkTimer = 0;
                }
                                
            }

            if (barking == true)
            {
                timeBarking += Time.deltaTime;
                if (timeBarking >= barkLength)
                {
                    barkCollider.enabled = false;
                    barking = false;
                    timeBarking = 0;
                }
            }

            //changing camera view with right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                if (cameraDown == true)
                {
                    myCamera.transform.position = cameraPosition2.transform.position;

                    cameraDown = false;

                }
                else
                {
                    myCamera.transform.position = cameraPosition1.transform.position;

                    cameraDown = true;

                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if(straffe != 0 || translation != 0)
                {
                    if(grounded == true)
                    {
                        // Set current speed to run if shift is down
                        speed = sprintSpeed;
                        animator.SetBool("IsSprinting", true);
                        sprintDust.Play();
                    }
                    else
                    {
                        sprintDust.Stop();
                    }
                    
                }
                else
                {
                    sprintDust.Stop();
                }
                                
            }
            else
            {
                // Otherwise set current speed to walking speed
                speed = walkSpeed;
                animator.SetBool("IsSprinting", false);
                sprintDust.Stop();
            }
        }
        

        //when stunned, lock movement for a few seconds, then push the player away from the sheep
        if (stunned == true)
        {
            timeSinceStunned += Time.deltaTime;
            myRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            if(timeSinceStunned >= timeForStun)
            {
                myRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotation;
                myRigidBody.AddExplosionForce(5, transform.position, 5, .3f, ForceMode.Impulse);
                GetComponent<characterController>().enabled = true;

                if (timeSinceStunned >= stunReset)
                {
                    timeSinceStunned = 0;
                    stunned = false;
                }
            }
        }
    }

    //check player is touching ground
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
            navMesh.enabled = true;
        }

        if (collision.gameObject.tag == "Knife" && stunned == false)
        {
            Instantiate(cloudDust, particlePosition.transform);
            stunned = true;
            myCamera.transform.position = cameraPosition2.transform.position;

            cameraDown = false;
        }
    }

   
}
