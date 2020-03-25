using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : Singleton<MusicScript>
{
    public AudioSource m_AudioSource;
    public float m_MaximumVolume;
    public float m_FadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeMusicIn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FadeMusicIn()
    {
        m_AudioSource.volume = 0;
        float addedVolume = 0;

        while (m_AudioSource.volume < m_MaximumVolume)
        {
            addedVolume += m_FadeSpeed;
            m_AudioSource.volume = addedVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeMusicOut()
    {
        float removedVolume = m_AudioSource.volume;

        while (m_AudioSource.volume > m_MaximumVolume)
        {
            removedVolume -= m_FadeSpeed;
            m_AudioSource.volume = removedVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
