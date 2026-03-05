using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [NonSerialized] public static SFXManager Instance;
    public AudioSource SFXObject;
    public float volume = 2f;

    // Audio clips
    [SerializeField] AudioClip playerJump;
    [SerializeField] AudioClip playerShoot;
    [SerializeField] AudioClip bulletHit;
    [SerializeField] AudioClip bulletMiss;


    void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
    }

    public void PlaySFXClip(AudioClip clip)
    {
        AudioSource audioSource = Instantiate(SFXObject, Vector3.zero, Quaternion.identity);

        // Setting up the audio source
        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayerJump()
    {
            PlaySFXClip(playerJump);
    }

    public void PlayerShoot()
    {
        PlaySFXClip(playerShoot);
    }

    public void BulletHit()
    {
        PlaySFXClip(bulletHit);
    }

    public void BulletMiss()
    { 
        PlaySFXClip(bulletMiss);
    }
}
