using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaAventuras : MonoBehaviour
{
   
    [SerializeField] private AudioClip[] musicas; 
    private AudioSource audioSoruce; 

    void Start()
    {
        // elegimos una musica al azar
        audioSoruce = GetComponent<AudioSource>();
        audioSoruce.clip = musicas[Random.Range(0, musicas.Length)];
        audioSoruce.Play();

    }


}
