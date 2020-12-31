using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _explosionAudio;

    void Start()
    {
        _explosionAudio = GetComponent<AudioSource>();
        _explosionAudio.Play();
        Destroy(gameObject, 2.3f);
    }

    
    void Update()
    {
        
    }
}
