using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public AudioClip m_ButtonClick;
    public float m_ClickVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Plays the button sound whenever a button is pressed
    public void PlayButtonSound()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick, m_ClickVolume);
    }
}
