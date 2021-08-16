using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpScript : MonoBehaviour
{
    public UnityEvent whenUsed;
    public GameObject goldParticles;
    public GameObject evilParticles;
    public GameObject knife;
    
    public GameObject particles;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<characterController>() != null)
        {
            Use();
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
            References.audioManager.myAudio.PlayOneShot(References.audioManager.audioClipArray[0]);
        }
        
    }
    

    public void Use()
    {
        whenUsed.Invoke();
    }

    public void MakeBig()
    {
        foreach (SheepBehaviour sheep in References.allSheep)
        {
            sheep.transform.localScale = sheep.transform.localScale * 3;
            sheep.myAudio.pitch = .8f;
            var sheepStats = sheep.GetComponent<SheepBehaviour>();
            sheepStats.pointValue = sheepStats.pointValue * 2;
            sheep.particles.transform.localScale = sheep.particles.transform.localScale * 3;
        }
        
    }

    public void ChangeColourRainbow()
    {
        foreach (SheepBehaviour sheep in References.allSheep)
        {
            var sheepRenderer = sheep.GetComponentInChildren<SkinnedMeshRenderer>();
            sheepRenderer.materials[2].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            
            References.levelManager.colourChange = true;
        }
    }

    public void GoldSheep()
    {
        int index = Random.Range(0, References.allSheep.Count);
        GameObject thisSheep = References.allSheep[index].gameObject;
        var sheepRenderer = thisSheep.GetComponentInChildren<SkinnedMeshRenderer>();
        sheepRenderer.materials[2].color = Color.yellow;
        var sheepStats = thisSheep.GetComponent<SheepBehaviour>();
        sheepStats.pointValue = 1000;
        Instantiate(goldParticles, new Vector3(thisSheep.transform.position.x, thisSheep.transform.position.y + 2, thisSheep.transform.position.z), thisSheep.transform.rotation, thisSheep.transform);
    }

    public void AngrySheep()
    {
        int index = Random.Range(0, References.allSheep.Count);
        GameObject thisSheep = References.allSheep[index].gameObject;
        var sheepRenderer = thisSheep.GetComponentInChildren<SkinnedMeshRenderer>();
        sheepRenderer.materials[2].color = Color.black;
        var sheepStats = thisSheep.GetComponent<SheepBehaviour>();
        sheepStats.angry = true;
        sheepStats.theFlock.sheep.Remove(gameObject.transform);
        FlockAgent flockagent = thisSheep.GetComponent<FlockAgent>();
        sheepStats.theFlock.agents.Remove(flockagent);
        TextMesh sheepneck = thisSheep.GetComponentInChildren<TextMesh>();
        Instantiate(evilParticles, new Vector3(thisSheep.transform.position.x, thisSheep.transform.position.y + 2, thisSheep.transform.position.z), thisSheep.transform.rotation, thisSheep.transform);
        Instantiate(knife, new Vector3(thisSheep.transform.position.x, thisSheep.transform.position.y, thisSheep.transform.position.z), thisSheep.transform.rotation, sheepneck.transform);
        sheepStats.eyebrows.SetActive(true);
        References.audioManager.myAudio.PlayOneShot(References.audioManager.audioClipArray[1]);
        sheepStats.navAgent.speed = 8;
    }

    public void MakeSmall()
    {
        foreach (SheepBehaviour sheep in References.allSheep)
        {
            sheep.transform.localScale = sheep.transform.localScale / 3;
            sheep.myAudio.pitch = 1.3f;
        }
    }
}
