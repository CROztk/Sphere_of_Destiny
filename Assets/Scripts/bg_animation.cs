using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class bg_animation : MonoBehaviour
{
    public Sprite[] currentSprite;
    public float timeBetweenFrames = 0.7f;
    public Image bgImage;
    private int currentSpriteIndex = 0;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeBetweenFrames)
        {
            timer = 0.0f;
            currentSpriteIndex++;
            bgImage.sprite = currentSprite[(currentSpriteIndex + 1) % currentSprite.Length];
        }
    }
}
