using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSheep : MonoBehaviour
{
    public GameObject Sheep;
    public int xPos;
    public int zPos;
    public int sheepCount;

    private void Start()
    {
        StartCoroutine(SheepDrop());
    }

    IEnumerator SheepDrop()
    {
        while(sheepCount < 10)
        {
            xPos = Random.Range(1, 5);
            zPos = Random.Range(1, 5);
            Instantiate(Sheep, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            sheepCount += 1;
        }
    }
}
