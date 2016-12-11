using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager manger;

    public AudioClip shoot;
    public AudioClip die;
    public AudioClip sword;
    public AudioClip click;

    public GameObject[] objects;
    private AudioSource[] sources;

    void Start()
    {
        AudioManager.manger = this;

        sources = new AudioSource[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            sources[i] = objects[i].GetComponent<AudioSource>();
        }    
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void PlaySound(AudioClip clip, Vector3 position)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].isPlaying == false)
            {
                objects[i].transform.position = position;
                sources[i].clip = clip;
                sources[i].Play();

                return;
            }
        }
    }

    public void PlaySword(Vector3 position)
    {
        PlaySound(sword, position);
    }

    public void PlayDie(Vector3 position)
    {
        PlaySound(die, position);
    }

    public void PlayShoot(Vector3 position)
    {
        PlaySound(shoot, position);
    }

    public void PlayClick()
    {
        PlaySound(click, Vector3.zero);
    }
}
