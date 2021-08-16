using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnScript : MonoBehaviour
{
    public GameObject point;                //point for the sheep to go to once it enters the barn
    public AudioSource cowbell;


    private void Start()
    {
        References.barn = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Herdable")
        {
            if (other.GetComponent<SheepBehaviour>() != null)
            {
                SheepBehaviour thisSheep = other.GetComponent<SheepBehaviour>();
                thisSheep.returned = true;
                thisSheep.navAgent.speed = 5;
                thisSheep.navAgent.SetDestination(point.transform.position);
                References.levelManager.pointSystem += thisSheep.pointValue;
                thisSheep.particles.gameObject.SetActive(true);
                thisSheep.transform.parent = null;
                cowbell.pitch = Random.Range(0.6f, 1.2f);
                cowbell.Play();
            }

            if (other.GetComponent<Herdables>() != null)
            {
                Herdables thisCreature = other.GetComponent<Herdables>();
                thisCreature.returned = true;
                thisCreature.navAgent.speed = 5;
                thisCreature.navAgent.destination = point.transform.position;
                References.levelManager.pointSystem += thisCreature.pointValue;
                thisCreature.particles.gameObject.SetActive(true);
                cowbell.pitch = Random.Range(0.6f, 1.2f);
                cowbell.Play();
            }
        }
    }

    
}
