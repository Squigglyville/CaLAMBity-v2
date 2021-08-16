using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float pointSystem;
    public Text pointValueText;
    public bool colourChange;
    Color targetcolour;
    float timeSinceColourChanged;
    public float timeUntilColourChanges;
    public Image sheepImage;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector2(0, Physics2D.gravity.y * 6);

        References.levelManager = this;
    }

    void Update()
    {
        pointValueText.text = pointSystem.ToString();

        if(colourChange == true)
        {
            pointValueText.color = Color.white;
            timeSinceColourChanged += Time.deltaTime;

            sheepImage.CrossFadeColor(targetcolour, 2, false, false);
            if(timeSinceColourChanged >= timeUntilColourChanges)
            {
                targetcolour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                timeSinceColourChanged = 0;
            }
            
            
        }
    }


}
