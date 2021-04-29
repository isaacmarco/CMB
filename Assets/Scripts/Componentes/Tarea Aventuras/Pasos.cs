using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasos : MonoBehaviour
{
   
    [SerializeField]
    private AudioClip[] pasos; 
    private AudioSource audioSource; 
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    public void EscucharPaso()
    {
        if(audioSource.isPlaying)
            return; 
        // elegimos un pazo al azar y lo reproducimos
        audioSource.clip = pasos[Random.Range(0, pasos.Length)];
        audioSource.Play(); 
    }
}
