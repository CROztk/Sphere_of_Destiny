using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mute_button : MonoBehaviour
{
    public AudioSource audioSource;
    public Image icon;
    public Sprite muteIcon;
    public Sprite unmuteIcon;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.mute = false;
        icon.sprite = unmuteIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MuteIt()
    {
        audioSource.mute = !audioSource.mute;
        icon.sprite = audioSource.mute ? muteIcon : unmuteIcon;
    }
}
