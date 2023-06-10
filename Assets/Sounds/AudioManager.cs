using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private float defualtVol;
    [SerializeField]
    private float transitionTime;
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, endSource;

    bool m_checkIfEntered = true;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Happy ver1");
    }
    private void Update()
    {
        if(m_player.GetCurrentHealth() < 50f && m_checkIfEntered)
        {
            StartCoroutine(ChangeMusic("Sad ver1"));
            m_checkIfEntered = false;
        }
        if (m_player.GetCurrentHealth() >= 50f && !m_checkIfEntered)
        {
            StartCoroutine(ChangeMusic("Happy ver1"));
            m_checkIfEntered = true;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayEnd(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            endSource.PlayOneShot(s.clip);
        }
    }

    private IEnumerator ChangeMusic(string audioNamePlaying)
    {
        float percentage = 0;
        while(musicSource.volume > 0)
        {
            musicSource.volume = Mathf.Lerp(defualtVol, 0, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }
        PlayMusic(audioNamePlaying);
        percentage = 0;
        while (musicSource.volume < defualtVol)
        {
            musicSource.volume = Mathf.Lerp(0, defualtVol, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }
    }
}
